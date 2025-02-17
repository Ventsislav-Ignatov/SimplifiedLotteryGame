using SimplifiedLotteryGame.Common.Constants;
using SimplifiedLotteryGame.Data.Models;

namespace SimplifiedLotteryGame.Common.Services
{
    public class PlayerFactoryService(IRandomNumberGeneratorService randomNumberGenerator) : IPlayerFactoryService
    {
        public Player CreatePlayer(string name, int gameId, int? numberOfTicket)
        {
            var player = new Player
            {
                Name = name,
                Balance = PlayerConstants.InitialBalance,
                Tickets = new List<Ticket>()
            };

            int ticketsToBuy;

            if (numberOfTicket.HasValue)
            {
                ticketsToBuy = Math.Min(numberOfTicket.Value, (int)player.Balance);
                player.Balance -= ticketsToBuy;

                for (int i = 1; i <= ticketsToBuy; i++)
                {
                    player.Tickets.Add(new Ticket { GameId = gameId, IsActive = true, PlayerId = player.Id, TicketNumber = Guid.NewGuid() });
                }

                return player;
            }

            ticketsToBuy = randomNumberGenerator.GenerateRandomNumberOfTickets(TicketConstants.MinimumTickets, TicketConstants.MaximumTickets);

            player.Balance -= ticketsToBuy;

            for (int i = 1; i <= ticketsToBuy; i++)
            {
                player.Tickets.Add(new Ticket { GameId = gameId, IsActive = true, PlayerId = player.Id, TicketNumber = Guid.NewGuid() });
            }

            return player;
        }
    }
}
