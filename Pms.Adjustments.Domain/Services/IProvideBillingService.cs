﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.Domain.Services
{
    public interface IProvideBillingService
    {
        IEnumerable<Billing> GetBillings(string eeId,string cutoffId);
    }
}