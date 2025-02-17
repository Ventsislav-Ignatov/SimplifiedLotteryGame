namespace SimplifiedLotteryGame.Common
{
    public static class GameMessages
    {
        public const string GameName = "Bede Lottery";

        public const string InitialPlayerName = "Player 1";

        public const string WelcomeMessage = "Welcome to the {0}, Player 1!\n";

        public const string BalanceInfo = "* Your digital balance: {0}$";

        public const string TicketPriceInfo = "* Ticket Price: ${0} each\n";

        public const string TicketPurchasePrompt = "How many tickets do you want to buy, Player 1?";

        public const string InvalidTicketInput = "Invalid input! Please enter a valid number of tickets.";

        public const string CpuPlayersCount = "\n{0} other CPU players also have purchased tickets.\n";

        public const string TicketDrawResultsHeader = "\nTicket Draw Results:\n";

        public const string GrandPrizeLabel = "* Grand Prize: ";

        public const string SecondTierLabel = "* Second Tier: ";

        public const string ThirdTierLabel = "* Third Tier: ";

        public const string GrandPrizeWinnerMessage = "Player {0} win ${1:0.00}!";

        public const string SecondAndThirdWinnersMessageSecond = "Players {0} win ${1:0.00} each!";

        public const string HouseRevenueMessage = "\nHouse Revenue: ${0:0.00}";

        public const string GrandPrizeName = "Grand Prize";

        public const string SecondTierPrizeName = "Second Tier Prize";
        
        public const string ThirdTierPrizeName = "Third Tier Prize";
    }
}