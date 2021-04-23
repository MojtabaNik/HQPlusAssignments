using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Resources.SystemErrors;
using System;
using System.IO;
using System.Reflection;

namespace HQPlusAssignments.Application.System
{
    public class FileService : IFileService
    {


        /// <summary>
        /// Read file content as text from a path.
        /// </summary>
        /// <param name="filePath">file path based on os</param>
        /// <returns>file content as text</returns>
        public string ReadFileContent(string filePath)
        {
            try
            {
                return File.Exists(filePath)
                    ? File.ReadAllText(filePath)
                    : throw new UserFriendlyException(SystemErrorResourceKeys.FileNotFound);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (IOException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception)
            {
                //ToDo: Log Exception
                throw new UserFriendlyException(SystemErrorResourceKeys.SystemUnhandledException);
            }
        }
    }
}
