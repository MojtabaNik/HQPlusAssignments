using HQPlusAssignments.Application.Core.Report;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Common;
using NUnit.Extension.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace HQPlusAssignments.Application.Test.System
{
    [DependencyInjectingTestFixture]
    public class MailServiceTest
    {
        private readonly IMailService _mailService;
        private readonly IFileService _fileService;

        public MailServiceTest(IMailService mailService,
                               IFileService fileService)
        {
            _mailService = mailService;
            _fileService = fileService;
        }

        [Test]
        public void SendEmailAsync_Test()
        {
            _mailService.SendEmailAsync(new Core.System.Dtos.MailRequest
            {
                ToEmail = "m0jt0ba@gmail.com",
                Body = "This is for test",
                Subject = "Hello There!",
                Attachments = new List<Core.System.Dtos.MailAttachment>
                {
                    new Core.System.Dtos.MailAttachment{
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        FileName = "Report.xlsx",
                        FileContent = _fileService.ReadFileBytes(Path.Combine(FilePathHelper.GetTestDataFolder("System"), "Report.xlsx"))
                    }
                }
            }).GetAwaiter().GetResult();
        }
    }
}
