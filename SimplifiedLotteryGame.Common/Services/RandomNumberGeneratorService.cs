namespace SimplifiedLotteryGame.Common.Services
{
    public class RandomNumberGeneratorService : IRandomNumberGeneratorService
    {
        public int GenerateRandomNumberOfTickets(int min, int max)
        {
            return Random.Shared.Next(min, max);
        }

        public int GenerateRandomNumberOfPlayers(int min, int max)
        {
            return Random.Shared.Next(min, max);
        }
    }
}
