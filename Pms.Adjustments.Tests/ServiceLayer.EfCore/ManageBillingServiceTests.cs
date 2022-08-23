using Xunit;
using Pms.Adjustments.ServiceLayer.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pms.Adjustments.Test;
using Pms.Adjustments.Persistence;
using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Domain.Models;
using Pms.Adjustments.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Pms.Adjustments.ServiceLayer.EfCore.Tests
{
    public class ManageBillingServiceTests
    {
        private IDbContextFactory<AdjustmentDbContext> _factory;
        private IManageBillingService _manageBillingService;
        private Cutoff _cutoff;

        public ManageBillingServiceTests()
        {
            _factory = new AdjustmentDbContextFactoryFixture();
            _manageBillingService = new BillingManager(_factory);
            _cutoff = new();
        }


        [Fact]
        public void ShouldAddBilling()
        {
            using AdjustmentDbContext context = _factory.CreateDbContext();
            Billing expectedBilling = new()
            {
                BillingId = $"DYYJ_PCV_{_cutoff.CutoffId}_4",
                EEId = "DYYJ",
                CutoffId = _cutoff.CutoffId,
                AdjustmentName = "PCV",
                Amount = 600,
                Deducted = true,
                DateCreated = DateTime.Now,
                AdjustmentType = AdjustmentChoices.ADJUST1
            };

            _manageBillingService.AddBilling(expectedBilling);


            Billing actualBilling = context.Billings.Where(b => b.BillingId == $"DYYJ_PCV_{_cutoff.CutoffId}_4").FirstOrDefault();
            Assert.NotNull(actualBilling);

            context.Remove(actualBilling);
            context.SaveChanges();
        }
        [Fact]
        public void ShouldThrowOldBillingExceptionWhenResettingBilling()
        {
            Assert.Throws<OldBillingException>(() =>
            {
                using AdjustmentDbContext context = _factory.CreateDbContext();
                _manageBillingService.ResetBillings("DYYJ", "2207-1");
            });
        }


        [Fact]
        public void ShouldResetEEBillingsByCutoffId()
        {
            using AdjustmentDbContext context = _factory.CreateDbContext();

            _manageBillingService.ResetBillings("FJFC", _cutoff.CutoffId);

            bool hasEEBillingsRemains = context.Billings
                .Where(b => b.EEId == "FJFC")
                .Where(b => b.CutoffId == _cutoff.CutoffId).Any();

            bool hasAnyBillingsRemains = context.Billings.Any();

            Assert.False(hasEEBillingsRemains);
            Assert.True(hasAnyBillingsRemains);
        }
        [Fact]
        public void ShouldThrowOldBillingExceptionWhenAddingBilling()
        {
            Assert.Throws<OldBillingException>(() =>
            {
                using AdjustmentDbContext context = _factory.CreateDbContext();
                Billing expectedBilling = new()
                {
                    BillingId = "FJFC_PCV_2207-2_1",
                    EEId = "FJFC",
                    CutoffId = "2207-2",
                    AdjustmentName = "PCV",
                    Amount = 600,
                    Deducted = true,
                    DateCreated = DateTime.Now,
                    AdjustmentType = AdjustmentChoices.ADJUST1
                };

                _manageBillingService.AddBilling(expectedBilling);
            });
        }

    }
}