﻿using System;
using System.Configuration;
using System.IO;
using FileDownloader.Downloaders;
using NUnit.Framework;

namespace FileDownloader.IntegrationTests.Downloaders
{
    [TestFixture]
    public class FtpDownloaderTest
    {
        private readonly Uri _url = new Uri(ConfigurationManager.AppSettings["ftpTestSource"]);
        private readonly long _expectedFileSize = Convert.ToInt64(ConfigurationManager.AppSettings["ftpTestSourceSizeInKb"]);
        private string _destinationPath = @"C:\TestDownloadedFiles";
        private string _fileName = "agoda.jpg";
        private FtpDownloader _ftpDownloader;
        private Stream _fileStream;
        private string FullFileName => $@"{_destinationPath}\{_fileName}";

        [SetUp]
        public void Setup()
        {
            _ftpDownloader = new FtpDownloader();
            Directory.CreateDirectory(_destinationPath);
            _fileStream = new FileStream(FullFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_destinationPath);
        }

        [Test]
        public void VerifyDwonload()
        {
            _ftpDownloader.Download(_fileStream, _url);

            Assert.True(File.Exists(FullFileName), "File wasn't downloaded");

            var fileInfo = new FileInfo(FullFileName);
            var fileSize = fileInfo.Length / 1024;

            Assert.AreEqual(_expectedFileSize, fileSize, $"Downloaded file size should be {_expectedFileSize}");

            File.Delete(FullFileName);
        }
    }
}
