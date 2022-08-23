using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.Domain.Services
{
    public interface IProvideBillingService
    {
        double GetTotalAdvances(string eeId, string cutoffId);
        //IEnumerable<Billing> GetBillings(string eeId, string cutoffId);
        IEnumerable<Billing> GetBillings(string cutoffId);
        IEnumerable<Billing> GetBillings(string cutoffId, string adjustmentName);
    }
}
