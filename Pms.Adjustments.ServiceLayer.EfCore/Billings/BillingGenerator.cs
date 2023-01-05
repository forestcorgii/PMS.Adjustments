using Microsoft.EntityFrameworkCore;
using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Enums;
using Pms.Adjustments.Domain.Models;
using Pms.Adjustments.Domain.Services;
using Pms.Adjustments.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.ServiceLayer.EfCore
{
    public class BillingGenerator : IGenerateBillingService
    {
        protected IDbContextFactory<AdjustmentDbContext> _factory;
        //private IManageBillingService _manageBillingService;

        public BillingGenerator(IDbContextFactory<AdjustmentDbContext> factory)
        {
            _factory = factory;
            //_manageBillingService = manageBillingService;
        }

        public IEnumerable<string> CollectEEIdWithPcv(string payrollCodeId, string cutoffId)
        {
            using AdjustmentDbContext context = _factory.CreateDbContext();
            IEnumerable<string> eeIds = context.Timesheets
                .Include(ts=>ts.EE)
                .Where(ts => ts.EE.PayrollCode == payrollCodeId)
                .Where(ts => ts.CutoffId == cutoffId)
                .Where(ts => ts.RawPCV != "" || ts.Allowance > 0)
                .Select(ts => ts.EEId)
                .Distinct()
                .ToList();

            return eeIds;
        }

        public IEnumerable<string> CollectEEIdWithBillingRecord(string payrollCodeId, string cutoffId)
        {
            using AdjustmentDbContext context = _factory.CreateDbContext();
            IEnumerable<string> eeIds = context.BillingRecords
                .Include(b => b.EE)
                .Where(b => b.EE.PayrollCode == payrollCodeId)
                .Select(b => b.EEId)
                .Distinct()
                .ToList();

            return eeIds;
        }

        public IEnumerable<Billing> GenerateBillingFromRecords(string eeId, string cutoffId)
        {
            Cutoff cutoff = new(cutoffId);
            using AdjustmentDbContext context = _factory.CreateDbContext();
            IEnumerable<BillingRecord> records = context.BillingRecords
                .Where(r => r.EEId == eeId)
                .Where(r => r.Balance > 0)
                .Where(r => r.DeductionOption == cutoff.DeductionOption || r.DeductionOption == DeductionOptions.EVERYPAYROLL)
                .Where(r => r.Status == BillingRecordStatus.ON_GOING)
                .ToList();

            List<Billing> billings = new();
            foreach (BillingRecord record in records)
            {
                if (!context.Billings.Any(b => b.RecordId == record.RecordId && b.CutoffId == cutoffId))
                {
                    double amount = record.Amortization;
                    if (record.Amortization > record.Balance)
                        amount = record.Balance;

                    Billing billing = new()
                    {
                        EEId = eeId,
                        CutoffId = cutoffId,
                        AdjustmentType = record.AdjustmentType,
                        Amount = amount,
                        AdjustmentOption = AdjustmentOptions.ADJUST2,
                        Applied = false,
                        DateCreated = DateTime.Now
                    };
                    billing.BillingId = Billing.GenerateId(billing);
                    billings.Add(billing);
                }
            }

            return billings;
        }


        public IEnumerable<Billing> GenerateBillingFromTimesheetView(string eeId, string cutoffId)
        {
            using AdjustmentDbContext context = _factory.CreateDbContext();
            IEnumerable<TimesheetView> eeTimesheets = context.Timesheets
                .Where(ts => ts.EEId == eeId)
                .Where(ts => ts.CutoffId == cutoffId)
                .ToList();

            List<Billing> billings = new();

            IEnumerable<TimesheetView> timesheetsWithAllowance = eeTimesheets.Where(ts => ts.Allowance > 0);
            foreach (TimesheetView timesheet in timesheetsWithAllowance)
            {
                var billing = new Billing()
                {
                    EEId = eeId,
                    CutoffId = cutoffId,
                    AdjustmentType = AdjustmentTypes.ALLOWANCE,
                    Amount = timesheet.Allowance,
                    AdjustmentOption = AdjustmentOptions.ADJUST1,
                    Applied = false,
                    DateCreated = DateTime.Now
                };
                billing.BillingId = Billing.GenerateId(billing);
                billings.Add(billing);
            }

            IEnumerable<TimesheetView> timesheetsWithPCV = eeTimesheets.Where(ts => ts.RawPCV != "");
            foreach (TimesheetView timesheet in timesheetsWithPCV)
            {
                string[] rawPCVs = timesheet.RawPCV.Split("|");
                for (int i = 0; i < rawPCVs.Length; i++)
                {
                    string rawPCV = rawPCVs[i];
                    string[] rawPcvArgs = rawPCV.Split("~");
                    string remarks = rawPcvArgs[0];
                    double amount = double.Parse(rawPcvArgs[1]);

                    var billing = new Billing()
                    {
                        EEId = eeId,
                        CutoffId = cutoffId,
                        AdjustmentType = AdjustmentTypes.PCV,
                        Amount = amount,
                        AdjustmentOption = AdjustmentOptions.ADJUST1,
                        Applied = false,
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
