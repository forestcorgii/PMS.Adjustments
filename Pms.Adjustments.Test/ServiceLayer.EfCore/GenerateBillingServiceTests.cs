using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Models;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Persistence;
using Pms.Adjustments.ServiceLayer.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pms.Adjustments.Test.ServiceLayer.EfCore
{
    public class GenerateBillingServiceTests
    {

        private IDbContextFactory<AdjustmentDbContext> _factory;
        private IGenerateBillingService _billingCreationService;
        private IManageBillingService _manageBillingService;

        private Cutoff _cutoff;

        public GenerateBillingServiceTests()
        {
            _factory = new AdjustmentDbContextFactoryFixture();
            _manageBillingService = new ManageBillingService(_factory);
            //_billingCreationService = new GenerateBillingService(_fixture.Factory);
            _cutoff = new();
        }


        [Fact]
        public void ShouldGenerateBillingsByTimesheetView()
        {
            string eeId = "DYYJ";
            string cutoffId = _cutoff.CutoffId;
            using AdjustmentDbContext context = _factory.CreateDbContext();
            List<TimesheetView> timesheetsWithAllowance = context.Timesheets.Where(ts => ts.Allowance > 0).ToList();
            List<TimesheetView> timesheetsWithPCV = context.Timesheets.Where(ts => ts.RawPCV != "").ToList();


            // Reset Billings
            _manageBillingService.ResetBillings(eeId, cutoffId);


            List<Billing> billings = new();
            foreach (TimesheetView timesheet in timesheetsWithAllowance)
            {
                var billing = new Billing()
                {
                    EEId = eeId,
                    CutoffId = cutoffId,
                    AdjustmentName = "ALLOWANCE",
                    Amount = timesheet.Allowance,
                    AdjustmentType = AdjustmentChoices.ADJUST1,
                    Deducted = true,
                    DateCreated = DateTime.Now
                };
                billing.BillingId = Billing.GenerateId(billing);
                billings.Add(billing);
            }

            foreach (TimesheetView timesheet in timesheetsWithPCV)
            {
                string[] rawPCVs = timesheet.RawPCV.Split("|");
                //foreach (string rawPCV in rawPCVs)
                for (int i = 0; i < rawPCVs.Length; i++)
                {
                    string rawPCV = rawPCVs[0];
                    string[] rawPcvArgs = rawPCV.Split("~");
                    string remarks = rawPcvArgs[0];
                    double amount = double.Parse(rawPcvArgs[1]);

                    var billing = new Billing()
                    {
                        EEId = eeId,
                        CutoffId = cutoffId,
                        AdjustmentName = "PCV",
                        Amount = amount,
                        AdjustmentType = AdjustmentChoices.ADJUST1,
                        Deducted = true,
                        Remarks = remarks,
                        DateCreated = DateTime.Now
                    };
                    billing.BillingId = Billing.GenerateId(billing, i);
                    billings.Add(billing);
                }
            }


            foreach (Billing billing in billings)
            {
                _manageBillingService.AddBilling(billing);
                Assert.True(context.Billings.Any(b => b.BillingId == billing.BillingId));
            }
        }
    }
}
