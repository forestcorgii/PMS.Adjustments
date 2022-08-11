using Pms.Adjustments.Domain;
using System;
using Xunit;

namespace Pms.Adjustments.Domain.Tests
{
    public class AdjustmentBillingTests
    {


        [Theory]
        [InlineData("DYYJ_PCV", "2208-1", "0", "DYYJ_PCV_2208-1_0")]
        [InlineData("TEST_ALLOWANCE", "2208-2", "1", "TEST_ALLOWANCE_2208-2_1")]
        public void GenerateIdWithRecordIdTestShouldEqual(string recordId, string cutoffId, string iterator, string expected)
        {
            // ARRANGE
            AdjustmentBilling _sut = new()
            {
                RecordId = recordId,
                CutoffId = cutoffId
            };

            //ACT
            string actual = AdjustmentBilling.GenerateId(_sut,iterator);

            //ASSERT
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("DYYJ", "PCV", "2208-1", "0", "DYYJ_PCV_2208-1_0")]
        [InlineData("TEST", "ALLOWANCE", "2208-2", "1", "TEST_ALLOWANCE_2208-2_1")]
        public void GenerateIdWithoutRecordIdTestShouldEqual(string eeId, string adjustmentName, string cutoffId, string iterator, string expected)
        {
            // ARRANGE
            AdjustmentBilling _sut = new()
            {
                EEId = eeId,
                AdjustmentName = adjustmentName,
                CutoffId = cutoffId
            };

            //ACT
            string actual = AdjustmentBilling.GenerateId(_sut, iterator);

            //ASSERT
            Assert.Equal(expected, actual);
        }
    }
}
