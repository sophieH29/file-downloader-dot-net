using System;
using System.IO;
using FileDownloader.Downloaders;
using FileDownloader.FileSystems;
using FileDownloader.Managers;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace FileDownloader.Tests.Managers
{
    [TestFixture]
    public class SimpleDownloadManagerTest
    {
        private Mock<IFileSystem> _fileSystemMock;
        private Mock<IDownloader> _downloaderMock;
        private Mock<SimpleDownloadManager> _simpleDownloadManager;
        private Uri Url => new Uri("http://aaaa/image.jpg");
        private const string FileName = "image.jpg";
        private Mock<Stream> _fileStream;
        private readonly byte _maxRetryCount = 10;
        

        [SetUp]
        public void Setup()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _downloaderMock = new Mock<IDownloader>();

            _fileStream = new Mock<Stream>();
        }

        [TearDown]
        public void TearDown()
        {
            _fileSystemMock.Verify();
            _downloaderMock.Verify();
        }

        [Test]
        public void DownloaderManager_ShouldReturnDownloadedFileName()
        {
            _fileSystemMock.Setup(fileSystem => fileSystem.GenerateFileName(FileName)).Returns(FileName);
            _fileSystemMock.Setup(fileSystem => fileSystem.PrepareDirectory(FileName)).Verifiable();
            _fileSystemMock.Setup(fileSystem => fileSystem.CreateStream(FileName)).Returns(_fileStream.Object);

            _downloaderMock.Setup(downloader => downloader.Download(_fileStream.Object, It.IsAny<Uri>())).Verifiable();

            void DownloadAction() => _downloaderMock.Object.Download(_fileStream.Object, It.IsAny<Uri>());

            _simpleDownloadManager = new Mock<SimpleDownloadManager>(_fileSystemMock.Object, _downloaderMock.Object, Url, _maxRetryCount) { CallBase = true };
            
            _simpleDownloadManager.Protected().Setup("WithRetry", (Action)DownloadAction).Verifiable();

            var fileName = _simpleDownloadManager.Object.DownloadFile();

            Assert.AreEqual(FileName, fileName, $"Downloaded file should be equal {FileName}");
        }

        [Test]
        public void DownloaderManager_WithRetryShouldBeCalled()
        {
           
        }
    }
}
