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
        private IDbContextFactory<AdjustmentDbContext> _fixture;
        private IManageBillingService _billingCreationService;
        private Cutoff _cutoff;

        public ManageBillingServiceTests()
        {
            _fixture = new AdjustmentDbContextFactoryFixture();
            _billingCreationService = new ManageBillingService(_fixture);
            _cutoff = new();
        }


        [Fact]
        public void ShouldAddBilling()
        {
            using AdjustmentDbContext context = _fixture.CreateDbContext();
            Billing expectedBilling = new()
            {
                BillingId = $"DYYJ_PCV_{_cutoff.CutoffId}_1",
                EEId = "DYYJ",
                CutoffId = _cutoff.CutoffId,
                AdjustmentName = "PCV",
                Amount = 600,
                Deducted = true,
                DateCreated = DateTime.Now,
                AdjustmentType = AdjustmentChoices.ADJUST1
            };

            _billingCreationService.AddBilling(expectedBilling);


            Billing actualBilling = context.Billings.Where(b => b.BillingId == $"DYYJ_PCV_{_cutoff.CutoffId}_1").FirstOrDefault();
            Assert.NotNull(actualBilling);
        }
        [Fact]
        public void ShouldThrowOldBillingExceptionWhenResettingBilling()
        {
            Assert.Throws<OldBillingException>(() =>
            {
                using AdjustmentDbContext context = _fixture.CreateDbContext();
                _billingCreationService.ResetBillings("DYYJ", "2207-1");
            });
        }


        [Fact]
        public void ShouldResetEEBillingsByCutoffId()
        {
            using AdjustmentDbContext context = _fixture.CreateDbContext();

            _billingCreationService.ResetBillings("DYYJ", _cutoff.CutoffId);

            bool hasEEBillingsRemains = context.Billings
                .Where(b => b.EEId == "DYYJ")
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
                using AdjustmentDbContext context = _fixture.CreateDbContext();
                Billing expectedBilling = new()
                {
                    BillingId = "DYYJ_PCV_2207-2_1",
                    EEId = "DYYJ",
                    CutoffId = "2207-2",
                    AdjustmentName = "PCV",
                    Amount = 600,
                    Deducted = true,
                    DateCreated = DateTime.Now,
                    AdjustmentType = AdjustmentChoices.ADJUST1
                };

                _billingCreationService.AddBilling(expectedBilling);
            });
        }

    }
}