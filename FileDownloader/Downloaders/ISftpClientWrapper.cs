using System;
using System.IO;

namespace FileDownloader.Downloaders
{
    /// <summary>
    /// SFTP Client wrapper
    /// </summary>
    public interface ISftpClientWrapper : IDisposable
    {
        /// <summary>
        /// Creates SFTP stream
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="fileMode">File mode</param>
        /// <returns>Stream</returns>
        Stream CreateStream(string path, FileMode fileMode);

        /// <summary>
        /// Connects client
        /// </summary>
        void ConnectClient();

        /// <summary>
        /// Disconnects client
        /// </summary>
        void DisconnectClient();
    }
}
