# file-downloader-dot-net

**FileDownloader**
==================

This is a HTTP(S) / FTP / SFTP file downloader with support of resuming download. The main goal of .NET File Downloader is to facilitate downloading of big files on bad internet connections. It supports resuming of partially downloaded files. So if the download is interrupted and restarted, only the remaining part of file would be downloaded again. It has configurable retries count, so after all retries where done and still it was interupting, the partly saved file will be deleted and download will be stopped.

**Storage** 

For now it is implemented to save downloaded files in LocalFileSystem. But where created stubs storage implementations like AwsS3FileSystem to show how easily this solution can be expanded.

**Downloader** 

Currently implemented 3 types of Downloader
- HttpDownloader - for http / https 
- FtpDownloader
- SftpDownloader

FTP and SFTP are configurable through App.config

**App.config**
- *maxRetryCount* - count of download retries
- *ftpDirectoryPath* - FTP directory path
- *ftpPassword* - FTP password
- *sftpHost* -  SFTP host
- *sftpUserName* - SFTP username
- *sftpPassword* - SFTP password
- *storageType* - storage types (local, aws, azure)
- *destinationPath* - where to save downloaded files
- *sources* - resources that has to be downloaded. Can be taken from here or or passed through args


**Implementation diagram**

![Diagram](https://raw.githubusercontent.com/sophieH29/file-downloader-dot-net/master/DownloadManageFactory.jpg)

