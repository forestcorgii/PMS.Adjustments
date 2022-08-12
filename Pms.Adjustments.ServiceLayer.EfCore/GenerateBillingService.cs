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
        //private IManageBillingService _manageBillingService;

        public GenerateBillingService(IDbContextFactory<AdjustmentDbContext> factory)
        {
            _factory = factory;
            //_manageBillingService = manageBillingService;
        }

        public IEnumerable<Billing> GenerateBillingFromRecords(string eeId, string cutoffId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Billing> GenerateBillingFromTimesheetView(string eeId, string cutoffId)
        {
            using AdjustmentDbContext context = _factory.CreateDbContext();
            List<TimesheetView> timesheetsWithAllowance = context.Timesheets.Where(ts => ts.Allowance > 0).ToList();
            List<TimesheetView> timesheetsWithPCV = context.Timesheets.Where(ts => ts.RawPCV != "").ToList();

            List<Billing> billings = new();
            foreach (TimesheetView timesheet in timesheetsWithAllowance)
            {
                var billing = new Billing()
                {
                    EEId = eeId,
                    CutoffId = cutoffId,
                    AdjustmentName = "ALLOWANCE",
                    Amount = timesheet.Allowance,
                    AdjustmentType = AdjustmentChoices.ADJUST1,
                    Deducted = true,
                    DateCreated = DateTime.Now
                };
                billing.BillingId = Billing.GenerateId(billing);
                billings.Add(billing);
            }

            foreach (TimesheetView timesheet in timesheetsWithPCV)
            {
                string[] rawPCVs = timesheet.RawPCV.Split("|");
                for (int i = 0; i < rawPCVs.Length; i++)
                {
                    string rawPCV = rawPCVs[0];
                    string[] rawPcvArgs = rawPCV.Split("~");
                    string remarks = rawPcvArgs[0];
                    double amount = double.Parse(rawPcvArgs[1]);

                    var billing = new Billing()
                    {
                        EEId = eeId,
                        CutoffId = cutoffId,
                        AdjustmentName = "PCV",
                        Amount = amount,
                        AdjustmentType = AdjustmentChoices.ADJUST1,
                        Deducted = true,
                        Remarks = remarks,
                        DateCreated = DateTime.Now
                    };
                    billing.BillingId = Billing.GenerateId(billing, i);
                    billings.Add(billing);
                }
            }

            return billings;
        }
    }
}
