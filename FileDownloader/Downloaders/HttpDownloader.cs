using System;
using System.IO;
using System.Net;

namespace FileDownloader.Downloaders
{
    /// <inheritdoc cref="BaseDownloader" />
    /// <summary>
    /// Responsible for downloading files based on HTTP protocol
    /// </summary>
    public class HttpDownloader : BaseDownloader, IDownloader
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
            Console.WriteLine("Creating network stream...");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (bytesRead > 0) request.AddRange(bytesRead);

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
    }
}
