using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Models;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Persistence;
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

        private AdjustmentDbContextFactoryFixture _fixture;
        private IGenerateBillingService _billingCreationService;
        private Cutoff _cutoff;

        public GenerateBillingServiceTests()
        {
            //_fixture = new();
            //_billingCreationService = new GenerateBillingService(_fixture.Factory);
            //_cutoff = new();
        }


        [Fact]
        public void ShouldGenerateBillingsByTimesheetView()
        {
            //using AdjustmentDbContext context = _fixture.CreateDbContext();
            //IEnumerable<TimesheetView> = context

        }
    }
}
