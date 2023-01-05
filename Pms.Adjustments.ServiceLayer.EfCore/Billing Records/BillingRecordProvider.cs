using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Domain.Models;
using Pms.Adjustments.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.ServiceLayer.EfCore.Billing_Records
{
    public class BillingRecordProvider
    {
        protected IDbContextFactory<AdjustmentDbContext> _factory;

        public BillingRecordProvider(IDbContextFactory<AdjustmentDbContext> factory)
        {
            _factory = factory;
        }


        public IEnumerable<BillingRecord> GetBillingRecords()
        {
            using var context = _factory.CreateDbContext();
            return context.BillingRecords.Include(r => r.EE).ToList();
        }

        public IEnumerable<BillingRecord> GetBillingRecordsByPayrollCode(string payrollCode)
        {
            using var context = _factory.CreateDbContext();
            return context.BillingRecords
                .Include(r => r.EE)
                .Where(r => r.EE.PayrollCode == payrollCode)
                .ToList();
        }




    }
}
