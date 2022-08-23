using Xunit;
using Pms.Adjustments.ServiceLayer.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pms.Adjustments.Persistence;
using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Test;
using Pms.Adjustments.ServiceLayer.EfCore;
using System.IO;

namespace Pms.Adjustments.ServiceLayer.Files.Tests.ServiceLayer.EfCore
{
    public class AdjustmentExporterTests
    {
        private IProvideBillingService _billingProvider;
        private IDbContextFactory<AdjustmentDbContext> _factory;
        public AdjustmentExporterTests()
        {
            _factory = new AdjustmentDbContextFactoryFixture();
            _billingProvider = new BillingProvider(_factory);
        }

        [Fact()]
        public void ShouldExportBillings()
        {
            string cutoffId = "2208-1";
            string adjustmentName = "PCV";
            string payrollCode = "P1A";
            IEnumerable<Domain.Billing> billings = _billingProvider.GetBillings(cutoffId, adjustmentName);
            int expectedResult = billings.Count();


            string filedir = $@"{AppDomain.CurrentDomain.BaseDirectory}\BILLING\{adjustmentName}";
            Directory.CreateDirectory(filedir);
            string filename = $@"{adjustmentName}_{payrollCode}_{cutoffId}.xls";
            string billingExportFullname = $@"{filedir}\{filename}";

            BillingExporter exporter = new();
            int actualResult = exporter.ExportBillings(billings, adjustmentName, billingExportFullname);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}