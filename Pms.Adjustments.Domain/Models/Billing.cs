using System;
using System.ComponentModel.DataAnnotations;

namespace Pms.Adjustments.Domain
{
    public class Billing
    {
        [Key]
        public string BillingId { get; set; }

        public string EEId { get; set; }
        public virtual EmployeeView EE { get; set; }

        public string RecordId { get; set; }
        //public virtual AdjustmentRecord Record { get; set; }

        public string CutoffId { get; set; }


        public string PayrollCode { get; set; }

        public string BankCategory { get; set; }

        public double Amount { get; set; }
        public bool Deducted { get; set; }
        public string Remarks { get; set; }

        public string AdjustmentName { get; set; }
        public AdjustmentChoices AdjustmentType { get; set; }

        public DateTime DateCreated { get; set; }

        public override string ToString() => BillingId is not null ? BillingId : "EMPTY BILLING";


        public static string GenerateId(Billing bil, int iterator = 0) =>
            $"{(bil.RecordId is not null && bil.RecordId != "" ? bil.RecordId : $"{bil.EEId}_{bil.AdjustmentName}")}_" +
            $"{bil.CutoffId}_{iterator}";
    }

    public enum AdjustmentChoices
    {
        ADJUST1 = 1,
        ADJUST2 = 2
    }

    public enum RequestTypeChoices
    {
        BYREQUEST = 0,
        ONLY30TH = 30,
        ONLY15TH = 15,
        EVERYPAYROLL = 1,
        COMPLETED = 2
    }
}
