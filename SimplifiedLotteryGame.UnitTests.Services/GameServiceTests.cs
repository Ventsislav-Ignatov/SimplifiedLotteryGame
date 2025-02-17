using Microsoft.EntityFrameworkCore;
using Moq;
using SimplifiedLotteryGame.Common;
using SimplifiedLotteryGame.Common.Services;
using SimplifiedLotteryGame.Data.Models;
using SimplifiedLotteryGame.Services;

namespace SimplifiedLotteryGame.UnitTests.Services
{
    public class GameServiceTests : ServiceBaseTests
    {
        private Mock<IPlayerFactoryService> _mockPlayerFactoryService;
        private Mock<IRandomNumberGeneratorService> _mockRandomNumberGenerator;
        private Mock<IConsoleReader> _mockConsoleReader;
        private IGameService _gameService;
        private IRandomNumberGeneratorService _randomNumberGenerator;

        [SetUp]
        public void Setup()
        {
            _mockPlayerFactoryService = new Mock<IPlayerFactoryService>();
            _mockRandomNumberGenerator = new Mock<IRandomNumberGeneratorService>();
            _mockConsoleReader = new Mock<IConsoleReader>();

            _randomNumberGenerator = new RandomNumberGeneratorService();

            _gameService = new GameService(
                _mockPlayerFactoryService.Object,
                _mockRandomNumberGenerator.Object,
                DbContext,
                _mockConsoleReader.Object
            );
        }

        [Test]
        public async Task StartGameAsync_ShouldHandlePlayersWithAndWithoutTickets()
        {
            // Arrange
            int playerIdCounter = 1;
            int ticketIdCounter = 1;

            _mockPlayerFactoryService.Setup(p => p.CreatePlayer(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()))
                .Returns((string name, int gameId, int? tickets) => new Player
                {
                    Name = name,
                    Id = playerIdCounter++,
                    Tickets = tickets.HasValue
                        ? Enumerable.Range(0, tickets.Value)
                            .Select(_ => new Ticket { Id = ticketIdCounter++, GameId = gameId, PlayerId = playerIdCounter, IsActive = true })
                            .ToList()
                        : Enumerable.Range(0, 5)
                            .Select(_ => new Ticket { Id = ticketIdCounter++, GameId = gameId, PlayerId = playerIdCounter, IsActive = true })
                            .ToList()
                });

            _mockRandomNumberGenerator.Setup(r => r.GenerateRandomNumberOfPlayers(It.IsAny<int>(), It.IsAny<int>())).Returns(10);

            _mockRandomNumberGenerator.Setup(r => r.GenerateRandomNumberOfTickets(It.IsAny<int>(), It.IsAny<int>())).Returns(4);


            _mockConsoleReader.Setup(c => c.GetInputProvider()).Returns(() => () => "3"); // User buys 3 tickets

            _gameService = new GameService(
                _mockPlayerFactoryService.Object,
                _mockRandomNumberGenerator.Object,
                DbContext,
                _mockConsoleReader.Object
            );

            // Act
             await _gameService.StartGameAsync();

            // Assert
            var game = await DbContext.Games.FirstOrDefaultAsync();
            Assert.IsNotNull(game);
            Assert.AreEqual(GameMessages.GameName, game.Name);

            var players = await DbContext.Players.ToListAsync();
            Assert.IsTrue(players.Count == 11);

            var userPlayer = players.FirstOrDefault(p => p.Name == GameMessages.InitialPlayerName);
            Assert.IsNotNull(userPlayer);
            Assert.IsTrue(userPlayer.Tickets.Count == 3);

            var cpuPlayers = players.Where(p => p.Name != GameMessages.InitialPlayerName).ToList();
            Assert.AreEqual(10, cpuPlayers.Count);

            var prizes = await DbContext.Prizes.ToListAsync();
            Assert.AreEqual(3, prizes.Count);

            var tickets = await DbContext.Tickets.ToListAsync();
            Assert.AreEqual(53, tickets.Count);

            var winners = await DbContext.PlayerPrizes.ToListAsync();
            Assert.AreEqual(17, winners.Count);

            Assert.AreEqual(game.Revenue, 5.30);
        }
    }
}