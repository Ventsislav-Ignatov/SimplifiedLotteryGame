using SimplifiedLotteryGame.Services;

namespace SimplifiedLotteryGame.ConsoleApp
{
    internal class Application(IGameService gameService) : IApplication
    {
        public async Task Run()
        {
            await gameService.StartGameAsync();
        }
    }
}
