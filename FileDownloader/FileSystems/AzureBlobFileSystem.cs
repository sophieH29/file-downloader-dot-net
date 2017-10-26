using System;
using System.IO;

namespace FileDownloader.FileSystems
{
    /// <summary>
    /// Defines Azure storage file system
    /// TODO: Implement if we need Azure storage for saving downloaded files. Was created as an example of expanding capabilities
    /// </summary>
    public class AzureBlobFileSystem : IFileSystem
    {
        private readonly string _destinationPath;

        public AzureBlobFileSystem(string destinationPath)
        {
            _destinationPath = destinationPath;
        }

        public void DeleteFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public string GenerateFileName(string fileName)
        {
            throw new NotImplementedException();
        }

        public void PrepareDirectory(string fileName)
        {
            throw new NotImplementedException();
        }

        public Stream CreateStream(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
