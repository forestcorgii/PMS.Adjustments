using Xunit;
using Pms.Adjustments.ServiceLayer.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Persistence;
using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Test;

namespace Pms.Adjustments.ServiceLayer.EfCore.Tests
{
    public class ProvideBillingServiceTests
    {

        private IProvideBillingService _billingProvider;
        private IDbContextFactory<AdjustmentDbContext> _factory;
        public ProvideBillingServiceTests()
        {
            _factory = new AdjustmentDbContextFactoryFixture();
            _billingProvider = new BillingProvider(_factory);
        }

        [Fact()]
        public void GetBillingsTest()
        {
            _billingProvider.GetBillings("2208-1");
            _billingProvider.GetBillings("DYYJ");
            //_billingProvider.GetBillings("DYYJ","2208-1");

            Assert.True(true);
        }
    }
}