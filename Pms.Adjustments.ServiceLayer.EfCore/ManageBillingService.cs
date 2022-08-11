﻿using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Exceptions;
using Pms.Adjustments.Domain.Models;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Persistence;
using System;
using System.Linq;

namespace Pms.Adjustments.ServiceLayer.EfCore
{
    public class ManageBillingService : IManageBillingService
    {
        private AdjustmentDbContextFactory _factory;

        public ManageBillingService(AdjustmentDbContextFactory factory)
        {
            _factory = factory;
        }

        private void ValidateCutoffId(string cutoffId)
        {
            Cutoff cutoff = new(cutoffId);
            if (cutoff.CutoffDate < DateTime.Now)
                throw new OldBillingException("", cutoffId);
        }

        public void AddBilling(Billing billing)
        {
            ValidateCutoffId(billing.CutoffId);

            using AdjustmentDbContext context = _factory.CreateDbContext();
            context.Add(billing);
            context.SaveChanges();
        }

        public void ResetBillings(string eeId, string cutoffId)
        {
            ValidateCutoffId(cutoffId);

            using AdjustmentDbContext context = _factory.CreateDbContext();
            IQueryable<Billing> eeBillings = context.Billings.Where(b => b.EEId == eeId && b.CutoffId == cutoffId);
            context.Billings.RemoveRange(eeBillings);
            context.SaveChanges();
        }
    }
}