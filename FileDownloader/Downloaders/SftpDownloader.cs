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
        /// Host of SFTP connection
        /// </summary>
        private readonly string _host;

        /// <summary>
        /// Username of SFTP connection
        /// </summary>
        private readonly string _username;

        /// <summary>
        /// Password of SFTP connection
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// Initiates SFTP Downloader with defined maximum retries count
        /// </summary>
        public SftpDownloader()
        {
            _host = ConfigurationManager.AppSettings["sftpHost"];
            _username = ConfigurationManager.AppSettings["sftpUserName"];
            _password = ConfigurationManager.AppSettings["sftpPassword"];
        }

        /// <summary>
        /// Download resource
        /// </summary>
        /// <param name="fileStream">File stream where downloaded bytes will be written</param>
        /// <param name="url">Url of resource to download</param>
        public void Download(Stream fileStream, Uri url)
        {
            using (ISftpClientWrapper client = new SftpClientWrapper(_host, _username, _password))
            {
                client.ConnectClient();

                using (var sourceStream = client.CreateStream(url.LocalPath, FileMode.Open))
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

                client.DisconnectClient();
            }
        }
    }
}
