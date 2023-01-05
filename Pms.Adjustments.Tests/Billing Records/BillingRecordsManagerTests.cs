using Xunit;
using Pms.Adjustments.ServiceLayer.EfCore.Billing_Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Persistence;
using Pms.Adjustments.Test;
using Pms.Adjustments.Domain.Models;

namespace Pms.Adjustments.ServiceLayer.EfCore.Billing_Records.Tests.ServiceLayer.EfCore
{
    public class BillingRecordsManagerTests
    {
        private BillingRecordManager _manager;
        private BillingRecordProvider _provider;
        private IDbContextFactory<AdjustmentDbContext> _factory;
        public BillingRecordsManagerTests()
        {
            _factory = new AdjustmentDbContextFactoryFixture();
            _manager = new(_factory);
            _provider = new(_factory);
        }



        [Fact()]
        public void SaveTest()
        {
            BillingRecord expectedRecord = new()
            {
                EEId = "DYYJ",
                Advances = 10000,
                Balance = 8000,
                Amortization = 1000,
                DeductionOption = Domain.Enums.DeductionOptions.ONLY15TH,
                AdjustmentType = Domain.Enums.AdjustmentTypes.HDMF_SL,
                Status = Domain.Enums.BillingRecordStatus.ON_GOING,
                EffectivityDate = DateTime.Now,
                Remarks = "This is a test.",
            };
            expectedRecord.RecordId = BillingRecord.GenerateId(expectedRecord);

            _manager.Save(expectedRecord);

            IEnumerable<BillingRecord> records = _provider.GetBillingRecords();
            Assert.NotNull(records);
            Assert.NotEmpty(records);
            Assert.Contains(records, r => r.RecordId == expectedRecord.RecordId);
        }
    }
}