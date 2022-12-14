using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Enums;
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

        private static bool _databaseInitialized;

        public AdjustmentDbContextFactoryFixture()
        {
            CreateFactory();
            if (!_databaseInitialized)
            {
                using (var context = CreateDbContext())
                {
                    context.Database.Migrate();
                    //TrySeeding(context);
                }

                _databaseInitialized = true;
            }
        }

        private void TrySeeding(AdjustmentDbContext context)
        {
            Cutoff cutoff = new();
            List<Billing> billings = new()
            {
                AddSeedBilling("DYYJ", cutoff.CutoffId, AdjustmentTypes.PCV,  300),
                AddSeedBilling("DYYJ", cutoff.CutoffId, AdjustmentTypes.ALLOWANCE, 300),
                AddSeedBilling("DYYJ", "2207-1", AdjustmentTypes.PCV,  300),
                AddSeedBilling("FJFC",  "2207-1", AdjustmentTypes.ALLOWANCE, 1000),
            };

            foreach (Billing billing in billings)
            {
                if (!context.Billings.Any(b => b.BillingId == billing.BillingId))
                    context.Add(billing);
            }
            context.SaveChanges();
        }

        private Billing AddSeedBilling(string eeId, string cutoffId, AdjustmentTypes adjustmentType, double amount, int iterator = 0)
        {
            Billing billing = new()
            {
                EEId = eeId,
                CutoffId = cutoffId,
                AdjustmentType = adjustmentType,
                Amount = amount,
                Applied = true,
                DateCreated = DateTime.Now,
                AdjustmentOption = AdjustmentOptions.ADJUST1
            };
            billing.BillingId = Billing.GenerateId(billing, iterator);

            return billing;
        }

        public AdjustmentDbContextFactory Factory;
        public void CreateFactory()
            => Factory = new AdjustmentDbContextFactory(ConnectionString);

        public AdjustmentDbContext CreateDbContext()
            => Factory.CreateDbContext();
    }
}
