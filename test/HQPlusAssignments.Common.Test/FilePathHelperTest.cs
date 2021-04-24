using NUnit.Framework;
using System.IO;

namespace HQPlusAssignments.Common.Test
{
    public class FilePathHelperTest
    {
        [Test]
        public void CallingAssemblyDirectoryPath_Test()
        {
            var path = FilePathHelper.CallingAssemblyDirectoryPath;
            StringAssert.Contains("HQPlusAssignments.Common.Test", path);
        }

        [Test]
        public void GetTestDataFolder_Test()
        {
            var path = FilePathHelper.GetTestDataFolder("Extensions");
            StringAssert.Contains(Path.Combine("HQPlusAssignments.Common.Test", "Extensions"), path);
        }
    }
}
