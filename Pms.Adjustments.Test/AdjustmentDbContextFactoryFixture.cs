using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Models;
using Pms.Adjustments.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.Test
{
    public class AdjustmentDbContextFactoryFixture : IDbContextFactory<AdjustmentDbContext>
    {
        private const string ConnectionString = "server=localhost;database=payroll3Test_efdb;user=root;password=Soft1234;";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public AdjustmentDbContextFactoryFixture()
        {
            CreateFactory();
            if (!_databaseInitialized)
            {
                using (var context = CreateDbContext())
                {
                    context.Database.Migrate();
                    TrySeeding(context);
                }

                _databaseInitialized = true;
            }
        }

        private void TrySeeding(AdjustmentDbContext context)
        {
            Cutoff cutoff = new();
            List<Billing> billings = new()
            {
                new Billing()
                {
                    BillingId = $"DYYJ_PCV_{cutoff.CutoffId}_0",
                    EEId = "DYYJ",
                    CutoffId = cutoff.CutoffId,
                    AdjustmentName = "PCV",
                    Amount = 300,
                    Deducted = true,
                    DateCreated = DateTime.Now,
                    AdjustmentType = AdjustmentChoices.ADJUST1
                },
                new Billing()
                {
                    BillingId = $"DYYJ_ALLOWANCE_{cutoff.CutoffId}_0",
                    EEId = "DYYJ",
                    CutoffId = cutoff.CutoffId,
                    AdjustmentName = "ALLOWANCE",
                    Amount = 300,
                    Deducted = true,
                    DateCreated = DateTime.Now,
                    AdjustmentType = AdjustmentChoices.ADJUST1
                },
                new Billing()
                {
                    BillingId = "DYYJ_PCV_2207-1_0",
                    EEId = "DYYJ",
                    CutoffId = "2207-1",
                    AdjustmentName = "PCV",
                    Amount = 300,
                    Deducted = true,
                    DateCreated = DateTime.Now,
                    AdjustmentType = AdjustmentChoices.ADJUST1
                },
                new Billing()
                {
                    BillingId = "FJFC_ALLOWANCE_2207-1_0",
                    EEId = "FJFC",
                    CutoffId = "2207-1",
                    AdjustmentName = "ALLOWANCE",
                    Amount = 1000,
                    Deducted = true,
                    DateCreated = DateTime.Now,
                    AdjustmentType = AdjustmentChoices.ADJUST1
                }
            };

            foreach (Billing billing in billings)
            {
                if (!context.Billings.Any(b => b.BillingId == billing.BillingId))
                    context.Add(billing);
            }
            context.SaveChanges();
        }


        public AdjustmentDbContextFactory Factory;
        public void CreateFactory()
            => Factory = new AdjustmentDbContextFactory(ConnectionString);

        public AdjustmentDbContext CreateDbContext()
            => Factory.CreateDbContext();
    }
}
