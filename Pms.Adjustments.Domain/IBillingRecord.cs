using Pms.Adjustments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.Domain
{
    public interface IBillingRecord
    {
        double Balance { get; set; }
        double Advances { get; set; }
        double Amortization { get; set; }

        public DateTime EffectivityDate { get; set; }

        DeductionOptions DeductionOption { get; set; }

        BillingRecordStatus Status { get; set; }
    }
}
