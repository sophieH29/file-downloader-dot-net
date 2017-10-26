using System.IO;
using Renci.SshNet;

namespace FileDownloader.Downloaders
{
    /// <summary>
    /// SFTP Client wrapper
    /// </summary>
    public class SftpClientWrapper: SftpClient, ISftpClientWrapper
    {
        public SftpClientWrapper(ConnectionInfo connectionInfo) : base(connectionInfo)
        {
        }

        public SftpClientWrapper(string host, int port, string username, string password) : base(host, port, username, password)
        {
        }

        public SftpClientWrapper(string host, string username, string password) : base(host, username, password)
        {
        }

        public SftpClientWrapper(string host, int port, string username, params PrivateKeyFile[] keyFiles) : base(host, port, username, keyFiles)
        {
        }

        public SftpClientWrapper(string host, string username, params PrivateKeyFile[] keyFiles) : base(host, username, keyFiles)
        {
        }

        /// <summary>
        /// Creates SFTP stream
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="fileMode">File mode</param>
        /// <returns>Stream</returns>
        public virtual Stream CreateStream(string path, FileMode fileMode)
        {
            return Open(path, fileMode);
        }

        /// <summary>
        /// Connects client
        /// </summary>
        public virtual void ConnectClient()
        {
            Connect();
        }

        /// <summary>
        /// Disconnects client
        /// </summary>
        public virtual void DisconnectClient()
        {
            Disconnect();
        }
    }
}
