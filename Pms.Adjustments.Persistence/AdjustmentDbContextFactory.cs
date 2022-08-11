using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.Persistence
{
    public class AdjustmentDbContextFactory : IDbContextFactory<AdjustmentDbContext>
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

            //return new AdjustmentDbContext();
            return new AdjustmentDbContext(dbContextOptions);
        }
    }
}
