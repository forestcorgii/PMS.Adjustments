﻿using Microsoft.EntityFrameworkCore;
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
    public class BillingGenerator : IGenerateBillingService
    {
        protected IDbContextFactory<AdjustmentDbContext> _factory;
        //private IManageBillingService _manageBillingService;

        public BillingGenerator(IDbContextFactory<AdjustmentDbContext> factory)
        {
            _factory = factory;
            //_manageBillingService = manageBillingService;
        }

        public IEnumerable<Billing> GenerateBillingFromRecords(string eeId, string cutoffId)
        {
            return default;
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
                    AdjustmentName = "ALLOWANCE",
                    Amount = timesheet.Allowance,
                    AdjustmentType = AdjustmentChoices.ADJUST1,
                    Deducted = true,
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
