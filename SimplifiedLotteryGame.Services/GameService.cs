using Microsoft.EntityFrameworkCore;
using SimplifiedLotteryGame.Common;
using SimplifiedLotteryGame.Common.Constants;
using SimplifiedLotteryGame.Common.Services;
using SimplifiedLotteryGame.Data;
using SimplifiedLotteryGame.Data.Models;
using System.Text;

namespace SimplifiedLotteryGame.Services
{
    public class GameService(IPlayerFactoryService playerFactoryService, IRandomNumberGeneratorService randomNumberGenerator, DataContext dbContext, IConsoleReader reader) : IGameService
    {
        public async Task StartGameAsync()
        {
            var game = await InitializeGameAsync();
            var players = await InitializePlayersAsync(game, reader.GetInputProvider());
            var prizes = await GeneratePrizesAsync(game, players);

            await dbContext.SaveChangesAsync();

            var winners = await DrawWinnersAsync(game, prizes);
            decimal houseProfit = CalculateHouseProfit(prizes, players);

            game.Revenue = houseProfit;
            await dbContext.PlayerPrizes.AddRangeAsync(winners);
            await dbContext.SaveChangesAsync();

            DisplayResults(game, winners, houseProfit);
        }

        private async Task<Game> InitializeGameAsync()
        {
            var game = new Game { Name = GameMessages.GameName };
            await dbContext.Games.AddAsync(game);
            await dbContext.SaveChangesAsync();
            return game;
        }

        private async Task<List<Player>> InitializePlayersAsync(Game game, Func<string> inputProvider)
        {
            Console.WriteLine(GameMessages.WelcomeMessage, game.Name);
            Console.WriteLine(GameMessages.BalanceInfo, PlayerConstants.InitialBalance);
            Console.WriteLine(GameMessages.TicketPriceInfo, TicketConstants.TicketPrice);

            int userTickets = BuyTickets(inputProvider);
            var players = new List<Player>
            {
                playerFactoryService.CreatePlayer(GameMessages.InitialPlayerName, game.Id, userTickets)
            };

            int cpuPlayersCount = randomNumberGenerator.GenerateRandomNumberOfPlayers(PlayerConstants.MinimumPlayers, PlayerConstants.MaximumPlayers);
            Console.WriteLine(GameMessages.CpuPlayersCount, cpuPlayersCount);

            for (int i = 1; i <= cpuPlayersCount; i++)
            {
                players.Add(playerFactoryService.CreatePlayer($"{GameMessages.InitialPlayerName.Substring(0, 6)} {i + 1}", game.Id, null));
            }

            await dbContext.Players.AddRangeAsync(players);
            return players;
        }

        private int BuyTickets(Func<string> inputProvider)
        {
            Console.WriteLine(GameMessages.TicketPurchasePrompt);
            int tickets;
            while (!int.TryParse(inputProvider(), out tickets) || tickets < TicketConstants.MinimumTickets || tickets >= TicketConstants.MaximumTickets)
            {
                Console.WriteLine(GameMessages.InvalidTicketInput);
            }
            return tickets;
        }

        private async Task<List<Prize>> GeneratePrizesAsync(Game game, List<Player> players)
        {
            int totalTickets = players.Sum(p => p.Tickets.Count);
            decimal totalRevenue = totalTickets * TicketConstants.TicketPrice;

            var prizes = new List<Prize>
            {
                new() { Name = GameMessages.GrandPrizeName, PrizeValue = totalRevenue * 0.50m, GameId = game.Id },
                new() { Name = GameMessages.SecondTierPrizeName, PrizeValue = totalRevenue * 0.30m, GameId = game.Id },
                new() { Name = GameMessages.ThirdTierPrizeName, PrizeValue = totalRevenue * 0.10m, GameId = game.Id }
            };

            await dbContext.Prizes.AddRangeAsync(prizes);
            return prizes;
        }

        private async Task<List<PlayerPrize>> DrawWinnersAsync(Game game, List<Prize> prizes)
        {
            var winners = new List<PlayerPrize>();

            var tickets = await dbContext.Tickets
                .Include(t => t.Player)
                .Where(t => t.GameId == game.Id)
                .ToListAsync();

            var grandPrizeTicket = SelectWinner(tickets);
            winners.Add(CreatePlayerPrize(grandPrizeTicket, prizes[0]));
            grandPrizeTicket.IsActive = false;

            int secondTierWinners = (int)Math.Round(tickets.Count * 0.10);
            int thirdTierWinners = (int)Math.Round(tickets.Count * 0.20);

            DrawTierWinners(winners, tickets, prizes[1], secondTierWinners);
            DrawTierWinners(winners, tickets, prizes[2], thirdTierWinners);

            return winners;
        }

        private Ticket SelectWinner(List<Ticket> tickets)
        {
            var activeTickets = tickets.Where(t => t.IsActive).ToList();
            if (!activeTickets.Any())
                return null;

            var winnerTicket = activeTickets[randomNumberGenerator.GenerateRandomNumberOfTickets(0, activeTickets.Count)];
            return winnerTicket;
        }

        private void DrawTierWinners(List<PlayerPrize> winners, List<Ticket> tickets, Prize prize, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var winnerTicket = SelectWinner(tickets);

                if (winnerTicket == null) break;

                winners.Add(CreatePlayerPrize(winnerTicket, prize));
                winnerTicket.IsActive = false;
            }
        }

        private PlayerPrize CreatePlayerPrize(Ticket ticket, Prize prize)
        {
            return new PlayerPrize { PlayerId = ticket.PlayerId, PrizeId = prize.Id, TicketId = ticket.Id };
        }

        private decimal CalculateHouseProfit(List<Prize> prizes, List<Player> players)
        {
            decimal totalRevenue = players.Sum(p => p.Tickets.Count) * TicketConstants.TicketPrice;

            decimal distributedAmount = 0;
            decimal roundingAdjustment = 0;

            foreach (var prize in prizes)
            {
                int winnerCount = 0;
                if (prize.Name == GameMessages.SecondTierPrizeName)
                    winnerCount = (int)Math.Round(players.Sum(p => p.Tickets.Count) * 0.10);
                else if (prize.Name == GameMessages.ThirdTierPrizeName)
                    winnerCount = (int)Math.Round(players.Sum(p => p.Tickets.Count) * 0.20);

                if (winnerCount > 0)
                {
                    decimal exactShare = prize.PrizeValue / winnerCount;
                    decimal adjustedShare = Math.Floor(exactShare);
                    decimal totalAdjustedAmount = adjustedShare * winnerCount;
                    roundingAdjustment += (prize.PrizeValue - totalAdjustedAmount);
                    distributedAmount += totalAdjustedAmount;
                }
                else
                {
                    distributedAmount += prize.PrizeValue;
                }
            }

            return totalRevenue - (distributedAmount + roundingAdjustment);
        }


        private void DisplayResults(Game game, List<PlayerPrize> winners, decimal houseProfit)
        {
            var gameWinners = dbContext.PlayerPrizes
                .Include(x => x.Player)
                .Include(x => x.Prize)
                .Where(x => x.Prize.GameId == game.Id)
                .OrderByDescending(x => x.Prize.PrizeValue)
                .ToList();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GameMessages.TicketDrawResultsHeader);

            var prizeGroups = gameWinners.GroupBy(w => w.Prize.PrizeValue).OrderByDescending(g => g.Key);
            int tier = 1;

            foreach (var group in prizeGroups)
            {
                sb.Append(tier switch
                {
                    1 => GameMessages.GrandPrizeLabel,
                    2 => GameMessages.SecondTierLabel,
                    _ => GameMessages.ThirdTierLabel
                });

                var playerNumbers = group.Select(w => w.Player.Name.Replace("Player ", ""));

                sb.AppendLine(tier switch
                {
                    1 => string.Format(GameMessages.GrandPrizeWinnerMessage, string.Join(", ", playerNumbers), group.First().Prize.PrizeValue),
                    _ => string.Format(GameMessages.SecondAndThirdWinnersMessageSecond, string.Join(", ", playerNumbers), group.First().Prize.PrizeValue)
                });

                tier++;
            }

            sb.AppendLine(string.Format(GameMessages.HouseRevenueMessage, houseProfit));
            Console.WriteLine(sb.ToString());
        }
    }
}
