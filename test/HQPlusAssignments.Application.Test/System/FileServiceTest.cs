using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Common;
using HQPlusAssignments.Resources.SystemErrors;
using NUnit.Extension.DependencyInjection;
using NUnit.Framework;
using System.IO;

namespace HQPlusAssignments.Application.Test.System
{
    [DependencyInjectingTestFixture]
    public class FileServiceTest
    {
        private readonly IFileService _fileService;

        public FileServiceTest(IFileService fileService)
        {
            _fileService = fileService;
        }

        [Test]
        public void ReadFileContent_WrongPath_Test()
        {
            Assert.Throws(
           Is.TypeOf<UserFriendlyException>().And.Message.EqualTo(SystemErrorResourceKeys.FileNotFound),
           () => _fileService.ReadFileContent("test"));
        }

        [Test]
        public void ReadFileContent_RightPath_Test()
        {
            var result = _fileService.ReadFileContent(Path.Combine(FilePathHelper.GetTestDataFolder("System"), "test.txt"));
            Assert.AreEqual("Test", result);
        }


        [Test]
        public void ReadFileBytes_WrongPath_Test()
        {
            Assert.Throws(
           Is.TypeOf<UserFriendlyException>().And.Message.EqualTo(SystemErrorResourceKeys.FileNotFound),
           () => _fileService.ReadFileBytes("test"));
        }

        [Test]
        public void ReadFileBytes_RightPath_Test()
        {
            var result = _fileService.ReadFileBytes(Path.Combine(FilePathHelper.GetTestDataFolder("System"), "test.txt"));
            Assert.That(result.Length, Is.GreaterThan(0));
        }
    }
}
