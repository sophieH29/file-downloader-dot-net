using System;
using System.IO;
using FileDownloader.Downloaders;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace FileDownloader.Tests.Downloaders
{
    [TestFixture]
    public class HttpDownloaderTest
    {
        private Mock<HttpDownloader> _httpDownloader;
        private Mock<Stream> _fileStream;
        private Mock<Stream> _networkStream;
        private readonly Uri _url = new Uri("http://aaaa/bb.jpg");

        [SetUp]
        public void Setup()
        {
            _httpDownloader = new Mock<HttpDownloader> {CallBase = true};
            _fileStream = new Mock<Stream>();
            _networkStream = new Mock<Stream>();
        }

        [Test]
        public void HttpDownloader_DownloadMethod()
        {
            _httpDownloader.Protected().Setup<Stream>("CreateNetworkStream", _url, ItExpr.IsAny<long>()).Returns(_networkStream.Object);
            _fileStream.Setup(stream => stream.SetLength(It.IsAny<long>())).Verifiable();

            _httpDownloader.Protected().Setup("DoDownload", _fileStream.Object, _networkStream.Object).Verifiable();

            _httpDownloader.Object.Download(_fileStream.Object, _url);
        }
    }
}
