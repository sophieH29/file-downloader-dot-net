using System;
using System.IO;
using FileDownloader.Downloaders;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace FileDownloader.Tests.Downloaders
{
    [TestFixture]
    public class FtpDownloaderTest
    {
        private Mock<FtpDownloader> _ftpDownloader;
        private Mock<Stream> _fileStream;
        private Mock<Stream> _networkStream;
        private readonly Uri _url = new Uri("http://aaaa/bb.jpg");

        [SetUp]
        public void Setup()
        {
            _ftpDownloader = new Mock<FtpDownloader> { CallBase = true };
            _fileStream = new Mock<Stream>();
            _networkStream = new Mock<Stream>();
        }

        [Test]
        public void FtpDownloader_DownloadMethod()
        {
            _ftpDownloader.Protected().Setup<Stream>("CreateNetworkStream", _url, ItExpr.IsAny<long>()).Returns(_networkStream.Object);
            _fileStream.Setup(stream => stream.SetLength(It.IsAny<long>())).Verifiable();

            _ftpDownloader.Protected().Setup("DoDownload", _fileStream.Object, _networkStream.Object).Verifiable();

            _ftpDownloader.Object.Download(_fileStream.Object, _url);
        }
    }
}
