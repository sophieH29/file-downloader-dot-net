using System;
using System.Configuration;
using System.IO;

namespace FileDownloader.Downloaders
{
    /// <inheritdoc cref="BaseDownloader" />
    /// <summary>
    /// Responsible for downloading files based on SFTP protocol
    /// </summary>
    public class SftpDownloader : BaseDownloader, IDownloader
    {
        /// <summary>
        /// SFTP Client instance
        /// </summary>
        private readonly SftpClientWrapper _sftpClient;

        /// <summary>
        /// Initiates SFTP Downloader
        /// </summary>
        public SftpDownloader()
        {
            var host = ConfigurationManager.AppSettings["sftpHost"];
            var username = ConfigurationManager.AppSettings["sftpUserName"];
            var password = ConfigurationManager.AppSettings["sftpPassword"];

            _sftpClient = new SftpClientWrapper(host, username, password);
        }

        /// <summary>
        /// Initiates SFTP Downloader with SFTP client passed through. Used for unit tests
        /// </summary>
        /// <param name="sftpClient">SFTP client</param>
        public SftpDownloader(SftpClientWrapper sftpClient)
        {
            _sftpClient = sftpClient;
        }

        /// <summary>
        /// Download resource
        /// </summary>
        /// <param name="fileStream">File stream where downloaded bytes will be written</param>
        /// <param name="url">Url of resource to download</param>
        public void Download(Stream fileStream, Uri url)
        {
            _sftpClient.ConnectClient();

                using (var sourceStream = _sftpClient.CreateStream(url.LocalPath, FileMode.Open))
                {
                    if (fileStream.Position > 0) sourceStream.Seek(fileStream.Position, SeekOrigin.Begin);

                    if (fileStream.Position == 0)
                    {
                        Size = (int)sourceStream.Length;
                        var sizeInKb = Size / 1024;
                        Console.WriteLine($"Size in kb is {sizeInKb}");
                    }

                    DoDownload(fileStream, sourceStream);
                }

            _sftpClient.DisconnectClient();
        }
    }
}
