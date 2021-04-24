using System.Data;
using System.IO;
using System.Linq;

namespace HQPlusAssignments.Common
{
    public static class ExcelHelper
    {
        /// <summary>
        /// This method convert byte[] of excel to DataTable for Test Purposes
        /// </summary>
        /// <param name="excelFile">Excel file as array</param>
        /// <param name="hasHeader">If excel file has header included then it should be true</param>
        /// <returns>DataTable of inserted file</returns>
        public static DataTable GetDataTableFromExcel(byte[] excelFile, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var memoryStream = new MemoryStream(excelFile))
                {
                    pck.Load(memoryStream);
                }

                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
    }
}
