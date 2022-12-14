using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Exceptions;
using Pms.Adjustments.Domain.Models;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Persistence;
using System;
using System.Linq;

namespace Pms.Adjustments.ServiceLayer.EfCore
{
    public class BillingManager : IManageBillingService
    {
        protected IDbContextFactory<AdjustmentDbContext> _factory;

        public BillingManager(IDbContextFactory<AdjustmentDbContext> factory)
        {
            _factory = factory;
        }

        private void ValidateCutoffId(string cutoffId)
        {
            //Cutoff cutoff = new(cutoffId);
            //TODO: UNCOMMENT ONCE DONE. COMMENTED TO TEST USING OLD DATA.
            //if (cutoff.CutoffDate < DateTime.Now)
            //    throw new OldBillingException("", cutoffId);
        }

        public void AddBilling(Billing billing)
        {
            ValidateCutoffId(billing.CutoffId);

            using AdjustmentDbContext context = _factory.CreateDbContext();
            if (context.Billings.Any(b => b.BillingId == billing.BillingId))
                context.Update(billing);
            else
                context.Add(billing);

            context.SaveChanges();
        }

        public void ResetBillings(string eeId, string cutoffId)
        {
            ValidateCutoffId(cutoffId);

            using AdjustmentDbContext context = _factory.CreateDbContext();
            IQueryable<Billing> eeBillings = context.Billings.Where(b => b.EEId == eeId && b.CutoffId == cutoffId && b.Applied);
            context.Billings.RemoveRange(eeBillings);
            context.SaveChanges();
        }
    }
}
