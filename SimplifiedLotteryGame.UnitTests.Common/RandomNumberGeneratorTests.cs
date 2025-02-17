namespace SimplifiedLotteryGame.UnitTests.Common
{
    using NUnit.Framework;
    using SimplifiedLotteryGame.Common.Constants;
    using SimplifiedLotteryGame.Common.Services;

    [TestFixture]
    public class RandomNumberGeneratorTests
    {
        private IRandomNumberGeneratorService _randomNumberGenerator;

        [SetUp]
        public void Setup()
        {
            _randomNumberGenerator = new SimplifiedLotteryGame.Common.Services.RandomNumberGeneratorService();
        }

        [Test]
        public void GenerateRandomNumberOfTickets_ShouldReturnNumberWithinRange()
        {
            // Arrange
            int min = TicketConstants.MinimumTickets;
            int max = TicketConstants.MaximumTickets;

            // Act
            int result = _randomNumberGenerator.GenerateRandomNumberOfTickets(min, max);

            // Assert
            Assert.That(result, Is.GreaterThanOrEqualTo(min));
            Assert.That(result, Is.LessThan(max));
        }

        [Test]
        public void GenerateRandomNumberOfPlayers_ShouldReturnNumberWithinRange()
        {
            // Arrange
            int min = PlayerConstants.MinimumPlayers;
            int max = PlayerConstants.MaximumPlayers;

            // Act
            int result = _randomNumberGenerator.GenerateRandomNumberOfPlayers(min, max);

            // Assert
            Assert.That(result, Is.GreaterThanOrEqualTo(min));
            Assert.That(result, Is.LessThan(max));
        }
    }

}