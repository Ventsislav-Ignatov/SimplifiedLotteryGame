using Moq;
using SimplifiedLotteryGame.Common.Constants;
using SimplifiedLotteryGame.Common.Services;
using SimplifiedLotteryGame.Data.Models;

namespace SimplifiedLotteryGame.UnitTests.Common
{
    [TestFixture]
    public class PlayerFactoryServiceTests
    {
        private Mock<IRandomNumberGeneratorService> _mockRandomNumberGenerator;
        private PlayerFactoryService _playerFactoryService;

        [SetUp]
        public void Setup()
        {
            _mockRandomNumberGenerator = new Mock<IRandomNumberGeneratorService>();
            _playerFactoryService = new PlayerFactoryService(_mockRandomNumberGenerator.Object);
        }

        [Test]
        public void CreatePlayer_WithSpecificNumberOfTickets_ShouldDeductBalanceAndAssignTickets()
        {
            // Arrange
            string playerName = "Player";
            int gameId = 1;
            int numberOfTickets = 3;

            // Act
            Player player = _playerFactoryService.CreatePlayer(playerName, gameId, numberOfTickets);

            // Assert
            Assert.That(player.Name, Is.EqualTo(playerName));
            Assert.That(player.Balance, Is.EqualTo(PlayerConstants.InitialBalance - numberOfTickets));
            Assert.That(player.Tickets.Count, Is.EqualTo(numberOfTickets));
            Assert.That(player.Tickets.All(t => t.GameId == gameId));
        }

        [Test]
        public void CreatePlayer_WithoutSpecificNumberOfTickets_ShouldUseRandomTicketCount()
        {
            // Arrange
            string playerName = "Player";
            int gameId = 1;
            int randomTicketCount = 5;

            _mockRandomNumberGenerator.Setup(rng => rng.GenerateRandomNumberOfTickets(It.Is<int>(x => x == TicketConstants.MinimumTickets),
                It.Is<int>(x => x == TicketConstants.MaximumTickets)))
                .Returns(randomTicketCount);

            // Act
            Player player = _playerFactoryService.CreatePlayer(playerName, gameId, null);

            // Assert
            Assert.That(player.Name, Is.EqualTo(playerName));
            Assert.That(player.Balance, Is.EqualTo(PlayerConstants.InitialBalance - randomTicketCount));
            Assert.That(player.Tickets.Count, Is.EqualTo(randomTicketCount));
            Assert.That(player.Tickets.All(t => t.GameId == gameId));
        }
    }

}
