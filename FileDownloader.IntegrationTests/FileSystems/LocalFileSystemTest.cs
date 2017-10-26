using System.IO;
using FileDownloader.FileSystems;
using NUnit.Framework;

namespace FileDownloader.IntegrationTests.FileSystems
{
    [TestFixture]
    public class LocalFileSystemTest
    {
        private string _destinationPath = @"C:\TestDownloadedFiles";
        private string _fileName = "filename.txt";
        private LocalFileSystem _localFileSystem;


        [Test]
        public void VerifyPrepareDirectory()
        {
           _localFileSystem = new LocalFileSystem(_destinationPath);
           _localFileSystem.PrepareDirectory(_fileName);

            Assert.IsTrue(Directory.Exists(_destinationPath), "Folder should be created");

            Directory.Delete(_destinationPath);
        }

        [Test]
        public void VerifyGenerateFileName()
        {
            _localFileSystem = new LocalFileSystem(_destinationPath);
            _localFileSystem.PrepareDirectory(_fileName);

            var generatedFileName = _localFileSystem.GenerateFileName(_fileName);
            Assert.AreEqual(_fileName, generatedFileName, "Generated file should be correct");

            Directory.Delete(_destinationPath);
        }

        [Test]
        public void VerifyGenerateFileNameWhenFileExists()
        {
            _localFileSystem = new LocalFileSystem(_destinationPath);
            _localFileSystem.PrepareDirectory(_fileName);

            // Create a file to write to.
            using (StreamWriter sw = File.CreateText($@"{_destinationPath}\{_fileName}"))
            {
                sw.WriteLine("Hello");
                sw.WriteLine("And");
                sw.WriteLine("Welcome");
            }

            var generatedFileName = _localFileSystem.GenerateFileName(_fileName);

            _localFileSystem.DeleteFile(_fileName);
            Assert.AreNotEqual(_fileName, generatedFileName, "Should be generated new file name");

            Directory.Delete(_destinationPath);
        }
    }

}
