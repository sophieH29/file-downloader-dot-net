using System;
using System.IO;
using FileDownloader.FileSystems;
using FileDownloader.Downloaders;

namespace FileDownloader.Managers
{
    /// <inheritdoc />
    /// <summary>
    /// Responsible for file downloading and storing
    /// </summary>
    public class SimpleDownloadManager : IDownloadManager
    {
        /// <summary>
        /// File name that will be used for downloaded resource
        /// </summary>
        private string _fileName;

        /// <summary>
        /// Defines file system
        /// </summary>
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Defines downloader that will be used for specific resource download
        /// </summary>
        private readonly IDownloader _downloader;

        /// <summary>
        /// Resource that needs to be downloaded
        /// </summary>
        private readonly Uri _sourceUrl;

        /// <summary>
        /// Max count of downloads retries
        /// </summary>
        private readonly byte _maxRetry;

        /// <summary>
        /// Creates instance of SimpleDownloadManager
        /// </summary>
        /// <param name="fileSystem">File system to use for file managing</param>
        /// <param name="downloader">Downloader to use for file downloading</param>
        /// <param name="sourceUrl">Source url to download</param>
        /// <param name="maxRetry">Max retries of downloads</param>
        public SimpleDownloadManager(IFileSystem fileSystem,
                                        IDownloader downloader,
                                        Uri sourceUrl,
                                        byte maxRetry) {
            _fileSystem = fileSystem;
            _downloader = downloader;
            _sourceUrl = sourceUrl;
            _fileName = Path.GetFileName(_sourceUrl.LocalPath);
            _maxRetry = maxRetry;
                                        }

        /// <summary>
        /// StartDownload resource
        /// </summary>
        public string DownloadFile()
        {
           try
            {
                Console.WriteLine($"Preparing download for url {_sourceUrl.OriginalString}");

               
                _fileName = _fileSystem.GenerateFileName(_fileName);
                _fileSystem.PrepareDirectory(_fileName);

                var fileStream = _fileSystem.CreateStream(_fileName);

                WithRetry(() => _downloader.Download(fileStream, _sourceUrl));

                return _fileName;

            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured during download: {e}");
                _fileSystem.DeleteFile(_fileName);
                return null;
            }
        }

        /// <summary>
        /// Retries of execute downloading of specified amount of times
        /// </summary>
        /// <param name="method">Method to execute</param>
        protected virtual void WithRetry(Action method)
        {
            int tryCount = 0;
            bool done = false;
            do
            {
                try
                {
                    method();
                    done = true;
                }
                catch (Exception)
                {
                    if (tryCount < _maxRetry)
                    {
                        tryCount++;
                        Console.WriteLine($"Retry #{tryCount} downloading...");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            while (!done);
        }
    }
}
