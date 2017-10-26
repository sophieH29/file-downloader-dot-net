namespace FileDownloader.Managers
{
    /// <summary>
    /// Responsible for file downloading and storing
    /// </summary>
    public interface IDownloadManager
    {
        /// <summary>
        /// StartDownload specific resource
        /// </summary>
        /// <returns>Donloaded file name</returns>
        string DownloadFile();
    }
}
