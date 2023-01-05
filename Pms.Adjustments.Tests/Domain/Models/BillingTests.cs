using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Enums;
using System;
using Xunit;

namespace Pms.Adjustments.Domain.Tests.Domain.Models
{
    public class BillingTests
    {
        [Theory]
        [InlineData("DYYJ_PCV", "2208-1", 0, "DYYJ_PCV_2208-1_0")]
        [InlineData("TEST_ALLOWANCE", "2208-2", 1, "TEST_ALLOWANCE_2208-2_1")]
        public void GenerateIdWithRecordIdTestShouldEqual(string recordId, string cutoffId, int iterator, string expected)
        {
            // GIVEN
            Billing _sut = new()
            {
                RecordId = recordId,
                CutoffId = cutoffId
            };

            //WHEN
            string actual = Billing.GenerateId(_sut, iterator);

            // THEN
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("DYYJ", AdjustmentTypes.PCV, "2208-1", 0, "DYYJ_PCV_2208-1_0")]
        [InlineData("TEST", AdjustmentTypes.ALLOWANCE, "2208-2", 1, "TEST_ALLOWANCE_2208-2_1")]
        public void GenerateIdWithoutRecordIdTestShouldEqual(string eeId, AdjustmentTypes adjustmentType, string cutoffId, int iterator, string expected)
        {
            // GIVEN
            Billing _sut = new()
            {
                EEId = eeId,
                AdjustmentType = adjustmentType,
                CutoffId = cutoffId
            };

            // WHEN
            string actual = Billing.GenerateId(_sut, iterator);

            // THEN
            Assert.Equal(expected, actual);
        }
    }
}
