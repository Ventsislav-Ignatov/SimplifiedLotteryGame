namespace SimplifiedLotteryGame.Common.Services
{
    public interface IConsoleReader
    {
        Func<string> GetInputProvider();
    }
}
