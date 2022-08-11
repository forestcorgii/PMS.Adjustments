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
        public DbSet<TimesheetView> Timesheets => Set<TimesheetView>();

        public AdjustmentDbContext(DbContextOptions options) : base(options) { }

        //public AdjustmentDbContext() { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySQL("server=localhost;database=payroll3_efdb;user=root;password=Soft1234;", options =>
        //        options.MigrationsHistoryTable("AdjustmentsMigrationHistory"));
        //}
        //"server=localhost;database=payroll3_efdb;user=root;password=Soft1234;"

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BillingConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeViewConfiguration());
            modelBuilder.ApplyConfiguration(new TimesheetConfiguration());
        }
    }
}
