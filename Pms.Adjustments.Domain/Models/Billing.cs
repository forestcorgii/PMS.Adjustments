using Pms.Adjustments.Domain.Enums;
using Pms.Adjustments.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pms.Adjustments.Domain
{
    public class Billing
    {
        public string BillingId { get; set; }

        public string EEId { get; set; }
        public EmployeeView EE { get; set; }

        public string RecordId { get; set; }
        public BillingRecord Record { get; set; }

        public string CutoffId { get; set; }


        public double Amount { get; set; }
        public bool Applied { get; set; }
        public string Remarks { get; set; }

        public AdjustmentTypes AdjustmentType { get; set; }
        public AdjustmentOptions AdjustmentOption { get; set; }

        public DateTime DateCreated { get; set; }

        public override string ToString() => BillingId is not null ? BillingId : "EMPTY BILLING";


        public static string GenerateId(Billing bil, int iterator = 0) =>
            $"{(bil.RecordId is not null && bil.RecordId != "" ? bil.RecordId : $"{bil.EEId}_{bil.AdjustmentType}")}_" +
            $"{bil.CutoffId}_{iterator}";
    }



}
