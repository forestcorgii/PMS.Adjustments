using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.ServiceLayer.EfCore
{
    public class GenerateBillingService : IGenerateBillingService
    {
        public IEnumerable<Billing> GenerateBillingFromRecords(string eeId, string cutoffId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Billing> GenerateBillingFromTimesheetView(string eeId, string cutoffId)
        {
            throw new NotImplementedException();
        }
    }
}
