using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using HQPlusAssignments.Common.Extensions;
using NUnit.Framework;

namespace HQPlusAssignments.Common.Test.Extensions
{
    public class IEnumerableExtensionsTest
    {

        [Test]
        public void GetExcelFromDynamicObject_Test()
        {
            var sample = new List<dynamic>();
            for (int i = 0; i < 500; i++)
            {
                sample.Add(new { RowNumber = i + 1, Name = "Mojtaba", LastName = "Nik" });
            }

            var result = sample.ToExcel();
            Assert.That(result, Has.Length.GreaterThan(0));

            var excelContent = ExcelHelper.GetDataTableFromExcel(result, true);
            Assert.That(excelContent.Rows.Count, Is.EqualTo(500));
            Assert.That(excelContent.Rows[399]["ROWNUMBER"], Is.EqualTo("400"));
            Assert.That(excelContent.Rows[399]["NAME"], Is.EqualTo("Mojtaba"));
            Assert.That(excelContent.Rows[399]["LastName"], Is.EqualTo("Nik"));
        }

        [Test]
        public void GetExcelFromDifferentObjectsInAList_Test()
        {
            var sample = new List<dynamic>
            {
                new { RowNumber = 1, Name = "Mojtaba", LastName = "Nik" },
                new { test = 2, Name1 = "Mojtaba", LastName2 = "Nik" }
            };

            Assert.Throws(Is.TypeOf<TargetException>(), () => sample.ToExcel());
        }
    }
}
