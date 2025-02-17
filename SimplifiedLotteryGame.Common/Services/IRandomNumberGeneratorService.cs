namespace SimplifiedLotteryGame.Common.Services
{
    public interface IRandomNumberGeneratorService
    {
        int GenerateRandomNumberOfTickets(int min, int max);

        int GenerateRandomNumberOfPlayers(int min, int max);

    }
}
