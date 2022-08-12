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
        private IGenerateBillingService _generateBillingService;
        private IManageBillingService _manageBillingService;

        private Cutoff _cutoff;

        public GenerateBillingServiceTests()
        {
            _factory = new AdjustmentDbContextFactoryFixture();
            _manageBillingService = new ManageBillingService(_factory);
            _generateBillingService = new GenerateBillingService(_factory);
            _cutoff = new();
        }


        [Fact]
        public void ShouldGenerateBillingsByTimesheetView()
        {
            // GIVEN
            string eeId = "DYYJ";
            string cutoffId = _cutoff.CutoffId;
            using AdjustmentDbContext context = _factory.CreateDbContext();
            int expectedBillingCount = context.Timesheets.Where(ts => ts.Allowance > 0).Count();

            List<TimesheetView> timesheetsWithPCV = context.Timesheets.Where(ts => ts.RawPCV != "").ToList();
            foreach (TimesheetView timesheet in timesheetsWithPCV)
                expectedBillingCount += timesheet.RawPCV.Split("|").Length;


            // WHEN
            IEnumerable<Billing> billings = _generateBillingService.GenerateBillingFromTimesheetView(eeId, cutoffId);
            int actualBillingCount = billings.Count();


            // THEN
            Assert.Equal(expectedBillingCount, actualBillingCount);
        }
    }
}
