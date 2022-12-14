using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.Domain.Services
{
    public interface IGenerateBillingService
    {
        IEnumerable<Billing> GenerateBillingFromTimesheetView(string eeId,string cutoffId);
        IEnumerable<Billing> GenerateBillingFromRecords(string eeId,string cutoffId);

        IEnumerable<string> CollectEEIdWithPcv(string payrollCodeId, string cutoffId);
        IEnumerable<string> CollectEEIdWithBillingRecord(string payrollCodeId, string cutoffId);
    }
}
