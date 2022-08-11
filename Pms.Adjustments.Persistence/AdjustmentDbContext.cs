using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Domain;
using System;
using System.Linq;

namespace Pms.Adjustments.Persistence
{
    public class AdjustmentDbContext : DbContext
    {
        public DbSet<Billing> Billings => Set<Billing>();
        public DbSet<EmployeeView> Employees => Set<EmployeeView>();

        public AdjustmentDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BillingConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeViewConfiguration());
        }
    }
}
