namespace SimplifiedLotteryGame.Common.Services
{
    public class ConsoleReader : IConsoleReader
    {
        public Func<string> GetInputProvider()
        {
            return () => Console.ReadLine();
        }
    }
}
