using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.Domain.Services
{
    public interface IBillingCreationService
    {
        void AddBilling(Billing billing);
        void ResetBillings(string eeId, string cutoffId);
    }
}
