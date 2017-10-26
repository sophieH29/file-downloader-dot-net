using System;
using System.Configuration;
using FileDownloader.Factories;
using FileDownloader.Managers;

namespace FileDownloader
{
    class Program
    {
        /// <summary>
        /// Default numbers of retries
        /// </summary>
        private const int DefaultMaxRetryCount = 10;

        static void Main(string[] args)
        {
            string[] filesSources;

            if (args?.Length > 0)
            {
                filesSources = args;
            }
            else
            {
                filesSources = ConfigurationManager.AppSettings["sources"]?.Split(new[] { "," },
                                                            StringSplitOptions.RemoveEmptyEntries);
            }

            if (filesSources == null)
            {
                Console.WriteLine("Nothing to download");
                return;
            }

            if (!byte.TryParse(ConfigurationManager.AppSettings["maxRetryCount"], out var maxRetry))
            {
                maxRetry = DefaultMaxRetryCount;
            }

            foreach (var sourceUrl in filesSources)
            {
                Console.WriteLine();
                Console.WriteLine($"Starting download of {sourceUrl}");
                IDownloadManager downloadManager = DownloadManagerFactory.GetInstance().GetDownloadManager(sourceUrl, maxRetry);

                string downloadedFileName = downloadManager.DownloadFile();

                Console.WriteLine(
                    downloadedFileName != null
                        ? $"File {downloadedFileName} was successfuly downloaded from resource {sourceUrl}!"
                        : $"An error occured during download from resource {sourceUrl}!");
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
