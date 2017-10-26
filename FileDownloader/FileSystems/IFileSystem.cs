using System.IO;

namespace FileDownloader.FileSystems
{
    /// <summary>
    /// File System interface
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Deletes file from file system
        /// </summary>
        /// <param name="fileName">File name</param>
        void DeleteFile(string fileName);

        /// <summary>
        /// Generates new file name based on one which already exists on the file system
        /// </summary>
        /// <returns>New file name</returns>
        string GenerateFileName(string fileName);

        /// <summary>
        /// Prepare directory where to download file
        /// </summary>
        /// <param name="fileName">File name</param>
        void PrepareDirectory(string fileName);

        /// <summary>
        /// Create file stream
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>File stream</returns>
        Stream CreateStream(string fileName);
    }
}
