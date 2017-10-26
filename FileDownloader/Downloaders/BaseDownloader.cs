using System.IO;

namespace FileDownloader.Downloaders
{
    /// <summary>
    /// Base implementaion of generic methods for downloading
    /// </summary>
    public abstract class BaseDownloader
    {
        /// <summary>
        /// Size of file in bytes
        /// </summary>
        protected int Size;

        /// <summary>
        /// Responsible for download process
        /// </summary>
        /// <param name="fileStream">File stream where to write bytes</param>
        /// <param name="networkStream">Network stream from where to get bytes</param>
        protected virtual void DoDownload(Stream fileStream, Stream networkStream)
        {
            byte[] buffer = new byte[10240];
            int byteSize;

            while ((byteSize = networkStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, byteSize);
                fileStream.Flush();
            }

            networkStream.Close();
            fileStream.Close();
        }
    }
}
