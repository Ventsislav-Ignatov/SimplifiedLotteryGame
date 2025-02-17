using Microsoft.EntityFrameworkCore;
using SimplifiedLotteryGame.Data;

namespace SimplifiedLotteryGame.UnitTests.Services
{
    public abstract class ServiceBaseTests : IDisposable
    {
        protected DataContext DbContext;
        protected ServiceBaseTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            DbContext = new DataContext(options);
        }
        public void Dispose()
        {
            DbContext.Dispose();
            GC.SuppressFinalize(this); // Suppress finalization
        }
    }
}
