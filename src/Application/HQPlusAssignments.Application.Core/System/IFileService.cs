namespace HQPlusAssignments.Application.Core.System
{
    public interface IFileService
    {
        /// <summary>
        /// Read file content as text from a path.
        /// </summary>
        /// <param name="filePath">file path based on os</param>
        /// <returns>file content as text</returns>
        string ReadFileContent(string filePath);

        /// <summary>
        /// Read file content as byte[] from a path.
        /// </summary>
        /// <param name="filePath">file path based on os</param>
        /// <returns>file content as byte[]</returns>
        byte[] ReadFileBytes(string filePath);
    }
}
