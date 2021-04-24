using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HQPlusAssignments.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// This method get IEnumerable list of any object and returns excel file as byte[]
        /// </summary>
        /// <param name="report">IEnumerable list of any object</param>
        /// <param name="sheetName">Name of worksheet that you want to show in excel</param>
        /// <returns>byte[] of excel file</returns>
        public static byte[] ToExcel(this IEnumerable<object> report, string sheetName = "Sheet1")
        {
            //Set ExcelPackage License
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Get your template and output file paths
            using (var pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                //populate our Data
                if (report.Any())
                {
                    for (int row = 0; row < report.Count(); row++)
                    {
                        //Take each property as a column
                        var column = 1;
                        foreach (PropertyInfo prop in report
                            .FirstOrDefault()
                            .GetType()
                            .GetProperties())
                        {
                            //Set Header
                            if (row == 0)
                            {
                                ws.Cells[1, column].Value = prop.Name.ToUpper();
                            }

                            //Set Value
                            var arrayOfReport = report.ToArray();

                            ws.Cells[row + 2, column].Value = prop.GetValue(arrayOfReport[row], null);

                            column++;
                        }
                    }


                    //create a range for the table
                    ExcelRange range = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];

                    //Set Alignments
                    range.AutoFitColumns();
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //add a table to the range
                    ExcelTable tab = ws.Tables.Add(range, "Table1");

                    //format the table
                    tab.TableStyle = TableStyles.Medium2;
                }

                return pck.GetAsByteArray();
            }
        }

    }
}
