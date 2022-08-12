using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.ServiceLayer.EfCore
{
    public class GenerateBillingService : IGenerateBillingService
    {
        protected IDbContextFactory<AdjustmentDbContext> _factory;
        private IManageBillingService _manageBillingService;

        public GenerateBillingService(IDbContextFactory<AdjustmentDbContext> factory,IManageBillingService manageBillingService)
        {
            _factory = factory;
            _manageBillingService = manageBillingService;
        }

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
