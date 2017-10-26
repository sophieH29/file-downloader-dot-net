using System;
using System.IO;

namespace FileDownloader.FileSystems
{
    /// <summary>
    /// Defines Local File System
    /// </summary>
    public class LocalFileSystem : IFileSystem
    {
        private const string DefaultDestinationPath = @"D:\Projects\DownloadedFiles";
        private readonly string _destinationPath;

        /// <summary>
        /// Initiates LocalFileSystem with predefined destination path
        /// </summary>
        /// <param name="destinationPath">Destination path where files would be dwnloaded</param>
        public LocalFileSystem(string destinationPath)
        {
            _destinationPath = destinationPath ?? DefaultDestinationPath;
        }

        /// <summary>
        /// Deletes file from file system
        /// </summary>
        /// <param name="fileName">File name</param>
        public void DeleteFile(string fileName)
        {
            var fullFileName = GetFullFileName(fileName);

            if (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
            }
        }

        /// <summary>
        /// Generates new file name based on one which already exists on the file system
        /// </summary>
        /// <returns>New file name</returns>
        public string GenerateFileName(string fileName)
        {
            string fullFileName = GetFullFileName(fileName);

            if (!File.Exists(fullFileName)) return fileName;

            string fileExtension = Path.GetExtension(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            fileName = fileNameWithoutExtension + DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension;

            return fileName;
        }

        /// <summary>
        /// Prepare directory where to download file
        /// </summary>
        /// <param name="fileName">File name</param>
        public void PrepareDirectory(string fileName)
        {
           var fileInfo = new FileInfo(GetFullFileName(fileName));
            if (fileInfo.DirectoryName != null && !Directory.Exists(fileInfo.DirectoryName))
                Directory.CreateDirectory(fileInfo.DirectoryName);
        }

        /// <summary>
        /// Create file stream
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>File stream</returns>
        public Stream CreateStream(string fileName)
        {
            Console.WriteLine("Creating local file stream...");

            var fullFileName = GetFullFileName(fileName);
            return new FileStream(fullFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }

        /// <summary>
        /// Get full file path with name
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Full file name</returns>
        private string GetFullFileName(string fileName)
        {
            return $@"{_destinationPath}\{fileName}";
        }
    }
}
