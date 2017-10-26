using System;
using System.Configuration;

using FileDownloader.Downloaders;
using FileDownloader.FileSystems;
using FileDownloader.Managers;
using FileDownloader.Enums;

namespace FileDownloader.Factories
{
    /// <summary>
    /// Responsible for creating instance of specific StartDownload manager based on parameters
    /// </summary>
    public class DownloadManagerFactory
    {
        /// <summary>
        /// Private constructor of DownloadManagerFactory
        /// </summary>
        private DownloadManagerFactory() { }

        /// <summary>
        /// Factory instance
        /// </summary>
        private static DownloadManagerFactory _downloadManagerFactoryInstance;

        /// <summary>
        /// Object locker
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// Returns already created instance of DownloadManagerFactory or creates new one
        /// </summary>
        /// <returns></returns>
        public static DownloadManagerFactory GetInstance()
        {
            lock (Locker)
            {
                if (_downloadManagerFactoryInstance == null)
                {
                    _downloadManagerFactoryInstance = new DownloadManagerFactory();
                }
            }

            return _downloadManagerFactoryInstance;
        }

        /// <summary>
        /// Creates instance of specific StartDownload manager based on parameters
        /// </summary>
        /// <param name="url">File source to download</param>
        /// <param name="maxRetry">Max retry count</param>
        /// <returns>instance of IDownloadManager</returns>
        public IDownloadManager GetDownloadManager(string url, byte maxRetry)
        {
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                return null;
            }

            var protocol = uri.Scheme;
            IDownloader downloader = GetDownloader(protocol);

            return downloader != null ? new SimpleDownloadManager(GetFileSystem(), downloader, uri, maxRetry) : null;
        }

        /// <summary>
        /// Get instance of downloader based on protocol
        /// </summary>
        /// <param name="protocol">Uri scheme</param>
        /// <returns>Instance of IDownloader</returns>
        private IDownloader GetDownloader(string protocol)
        {
            ProtocolTypes protocolType;
            if (Enum.TryParse(protocol, out protocolType))
            {
                switch (protocolType)
                {
                    case ProtocolTypes.http:
                    case ProtocolTypes.https:
                    {
                        return new HttpDownloader();
                    }
                    case ProtocolTypes.ftp:
                    {
                        return new FtpDownloader();
                    }
                    case ProtocolTypes.sftp:
                    {
                        return new SftpDownloader();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get instance of storage based on configuration
        /// </summary>
        /// <returns>Instance of IFileSystem</returns>
        private IFileSystem GetFileSystem()
        {
            var destinationPath = ConfigurationManager.AppSettings["destinationPath"];
            var storage = ConfigurationManager.AppSettings["storageType"];

            StorageTypes storageType;
            if (Enum.TryParse(storage, out storageType))
            {
                switch (storageType)
                {
                    case StorageTypes.local:
                    {
                        return new LocalFileSystem(destinationPath);
                    }
                    case StorageTypes.aws:
                    {
                        return new AwsS3FileSystem(destinationPath);
                    }
                    case StorageTypes.azure:
                    {
                        return new AzureBlobFileSystem(destinationPath);
                    }
                }
            }

            return new LocalFileSystem(null);
        }
    }
}
