using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.Domain.Exceptions
{
    public class BillingAlreadyExistsException : Exception
    {
        public string  BillingId { get; set; }
        public override string Message => "Billing laready exists, have You already reset billings."; 

        public BillingAlreadyExistsException(string billingId)
        {
            BillingId = billingId;
        }

    }
}
