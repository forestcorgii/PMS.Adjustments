using Pms.Adjustments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.Domain.Models
{
    public class BillingRecord : IBillingRecord
    {

        public string RecordId { get; set; }

        public string EEId { get; set; }
        public EmployeeView EE { get; set; }

        public AdjustmentTypes AdjustmentType { get; set; }

        public AdjustmentOptions AdjustmentOption { get; set; }

        public double Balance { get; set; }
        public double Advances { get; set; }
        public double Amortization { get; set; }

        public DateTime EffectivityDate { get; set; }

        public DeductionOptions DeductionOption { get; set; }

        public BillingRecordStatus Status { get; set; }


        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string Remarks { get; set; }

        public static string GenerateId(BillingRecord rec) =>
             $"{rec.EEId}_{rec.AdjustmentType}_{rec.EffectivityDate:MMyy}";






        public void Validate()
        {
            if (EEId is null)
                throw new Exception("EE Id should not be blank.");

            if (RecordId is null)
                throw new Exception("Record Id should not be blank.");

            //if (Balance > Advances)
            //    throw new Exception("Remaining Balance should not be greater than Advances.");

            if (Amortization > Advances)
                throw new Exception("Monthly Amortization should not be greater than Advances.");
        }




    }
}
