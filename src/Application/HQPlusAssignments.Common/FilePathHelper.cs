using Microsoft.DotNet.PlatformAbstractions;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HQPlusAssignments.Common
{
    public static class FilePathHelper
    {
        /// <summary>
        /// Get Calling Assembly Directory
        /// </summary>
        public static string CallingAssemblyDirectoryPath
        {
            get
            {
                var currentAssembly = Assembly.GetCallingAssembly();
                return Path.GetDirectoryName(currentAssembly.Location.Substring(0, currentAssembly.Location.IndexOf(currentAssembly.ManifestModule.Name)));
            }
        }

        /// <summary>
        /// This method is used to get test data folder which should be exists in {base}/{testDataFolderName}/data
        /// </summary>
        /// <param name="testDataFolder"></param>
        /// <returns>Full path to data folder</returns>
        public static string GetTestDataFolder(string testDataFolder)
        {
            string startupPath = ApplicationEnvironment.ApplicationBasePath;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            return Path.Combine(projectPath, testDataFolder, "data");
        }
    }
}
