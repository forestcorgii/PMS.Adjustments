using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Pms.Adjustments.Domain;
using Pms.Adjustments.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pms.Adjustments.ServiceLayer.Files
{
    public class BillingExporter
    {

        public int ExportBillings(IEnumerable<Billing> billings, string adjustmentName,string filename)
        {
            if (billings.Count() > 0)
            {
                IWorkbook nWorkBook = new HSSFWorkbook();
                ISheet nSheet = nWorkBook.CreateSheet(adjustmentName);

                int ridx = 0;
                IRow nRow = nSheet.CreateRow(ridx);

                nRow.CreateCell(0).SetCellValue("EE ID");
                nRow.CreateCell(1).SetCellValue("FULLNAME");
                nRow.CreateCell(2).SetCellValue("ADJUSTMENT NAME");
                nRow.CreateCell(3).SetCellValue("AMOUNT");
                nRow.CreateCell(4).SetCellValue("REMARKS");

                ridx++;

                foreach (Billing billing in billings)
                {
                    nRow = nSheet.CreateRow(ridx);
                    nRow.CreateCell(0).SetCellValue(billing.EEId);
                    if (billing.EE is not null)
                        nRow.CreateCell(1).SetCellValue(billing.EE.Fullname);

                    nRow.CreateCell(2).SetCellValue(billing.AdjustmentName);
                    nRow.CreateCell(3).SetCellValue(billing.Amount);
                    nRow.CreateCell(4).SetCellValue(billing.Remarks);

                    ridx++;
                }

                using (FileStream nNewPayreg = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    nWorkBook.Write(nNewPayreg);

                return ridx-1;
            }
            return 0;
        }

    }
}
