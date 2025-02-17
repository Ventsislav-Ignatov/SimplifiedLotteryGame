using SimplifiedLotteryGame.Data.Models;

namespace SimplifiedLotteryGame.Common.Services
{
    public interface IPlayerFactoryService
    {
        public Player CreatePlayer(string name, int gameId, int? numberOfTicket);
    }
}
