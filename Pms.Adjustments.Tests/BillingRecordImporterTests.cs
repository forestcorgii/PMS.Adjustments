using Xunit;
using Pms.Adjustments.ServiceLayer.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pms.Adjustments.Domain.Models;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Persistence;
using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Test;
using Pms.Adjustments.ServiceLayer.EfCore.Billing_Records;

namespace Pms.Adjustments.ServiceLayer.Files.Tests.ServiceLayer.EfCore
{
    public class BillingRecordImporterTests
    {
        private IDbContextFactory<AdjustmentDbContext> _factory;
        private BillingRecordManager Manager;
        private Cutoff _cutoff;

        public BillingRecordImporterTests()
        {
            _factory = new AdjustmentDbContextFactoryFixture();
            Manager = new BillingRecordManager(_factory);
            _cutoff = new();
        }

        [Fact()]
        public void ImportTest()
        {
            BillingRecordImporter importer = new();
            IEnumerable<BillingRecord> records = importer.Import($"{AppDomain.CurrentDomain.BaseDirectory}\\TEMPLATES\\SSS LOAN.xls");
            //foreach (BillingRecord record in records)
            //    Manager.Save(record);
            
            
            Assert.NotNull(records);
            Assert.NotEmpty(records);
        }
    }
}