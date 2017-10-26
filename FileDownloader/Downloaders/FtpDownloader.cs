using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace FileDownloader.Downloaders
{
    /// <summary>
    /// Responsible for downloading files based on FTP protocol
    /// </summary>
    public class FtpDownloader : BaseDownloader, IDownloader
    {
        /// <summary>
        /// Download resource
        /// </summary>
        /// <param name="fileStream">File stream where downloaded bytes will be written</param>
        /// <param name="url">Url of resource to download</param>
        public void Download(Stream fileStream, Uri url)
        {
            Console.WriteLine("Preparing download..");
            var networkStream = CreateNetworkStream(url, fileStream.Position);

            try
            {
                fileStream.SetLength(Size);
                DoDownload(fileStream, networkStream);
            }
            catch (Exception)
            {
                networkStream.Close();
                throw;
            }
        }

        /// <summary>
        /// Prepares network stream
        /// </summary>
        /// <param name="url">Resource url</param>
        /// <param name="bytesRead">Bytes already read</param>
        /// <returns>Network stream</returns>
        protected virtual Stream CreateNetworkStream(Uri url, long bytesRead)
        {
            FtpWebRequest request = CreateFtpWebRequest(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            if (bytesRead > 0)  request.ContentOffset = bytesRead;

            WebResponse response = request.GetResponse();

            if (bytesRead == 0)
            {
                Size = (int)response.ContentLength;
                var sizeInKb = Size / 1024;
                Console.WriteLine($"Size in kb is {sizeInKb}");
            }

            //create network stream
            return response.GetResponseStream();
        }

        /// <summary>
        /// Creates FTP web request
        /// </summary>
        /// <param name="ftpDirectoryPath">FTP directory path</param>
        /// <returns>FTP web request</returns>
        private FtpWebRequest CreateFtpWebRequest(Uri ftpDirectoryPath)
        {
            var userName = ConfigurationManager.AppSettings["ftpUserName"];
            var password = ConfigurationManager.AppSettings["ftpPassword"];

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpDirectoryPath);

            //Set proxy to null. Under current configuration if this option is not set then the proxy that is used will get an html response from the web content gateway (firewall monitoring system)
            request.Proxy = null;
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = true;

            request.Credentials = new NetworkCredential(userName, password);

            return request;
        }
    }
}
