using Microsoft.EntityFrameworkCore;

using System;

namespace Pms.Adjustments.Persistence
{
    public class AdjustmentDbContextFactory
    {

        private readonly string _connectionString;

        public AdjustmentDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AdjustmentDbContext CreateDbContext()
        {
            DbContextOptions dbContextOptions = new DbContextOptionsBuilder()
                .UseMySQL(
                    _connectionString,
                    options => options.MigrationsHistoryTable("AdjustmentsMigrationHistory")
                )
                .Options;

            return new AdjustmentDbContext(dbContextOptions);
        }
    }
}
