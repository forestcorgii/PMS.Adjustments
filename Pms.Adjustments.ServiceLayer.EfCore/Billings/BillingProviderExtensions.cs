using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Enums;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.ServiceLayer.EfCore
{
    public static class BillingProviderExtensions
    {
        public static IEnumerable<AdjustmentTypes> ExtractAdjustmentNames(this IEnumerable<Billing> billings)
        {
            return billings
                .GroupBy(b => b.AdjustmentType)
                .Select(n => n.First())
                .OrderBy(b => b.AdjustmentType)
                .Select(b => b.AdjustmentType)
                .ToList();
        }
    }
}
