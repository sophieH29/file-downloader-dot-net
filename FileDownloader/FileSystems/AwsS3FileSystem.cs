using System;
using System.IO;

namespace FileDownloader.FileSystems
{
    /// <summary>
    /// Defines Amazon storage file system
    /// TODO: Implement if we need Amazon storage for saving downloaded files. Was created as an example of expanding capabilities
    /// </summary>
    public class AwsS3FileSystem : IFileSystem
    {
        private readonly string _destinationPath;

        public AwsS3FileSystem(string destinationPath)
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
