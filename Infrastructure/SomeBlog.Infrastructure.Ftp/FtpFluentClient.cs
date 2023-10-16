using FluentFTP;
using SomeBlog.Infrastructure.Ftp.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SomeBlog.Infrastructure.Ftp
{
    public class FtpFluentClient
    {
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        private FluentFTP.FtpClient ftpClient;
        public bool StayConnected;

        public FtpFluentClient(string address, int port, string username, string password, bool stayConnected = false)
        {
            Address = address;
            Username = username;
            Password = password;
            StayConnected = stayConnected;

            ftpClient = new FluentFTP.FtpClient(Address, port,new NetworkCredential(Username, Password));
        }

        public List<string> GetFiles(string ftpDirectory)
        {
            // begin connecting to the server
            if(!ftpClient.IsConnected)
                ftpClient.Connect();

            var items = ftpClient.GetListing(ftpDirectory);
            var list = new List<string>();
            foreach (FtpListItem item in items)
            {
                list.Add(item.Name);
            }

            if (ftpClient.IsConnected && !StayConnected)
                ftpClient.Disconnect();

            return list;
        }

        public string DownloadString(string ftpPath, int retryCount = 1)
        {
            bool result = false;
            int tryCount = 0;

            while (tryCount < retryCount && !result)
            {
                try
                {
                    tryCount += 1;

                    if (!ftpClient.IsConnected)
                        ftpClient.Connect();

                    byte[] bytes;
                    if (!ftpClient.Download(out bytes, ftpPath))
                        return null;

                    return System.Text.Encoding.UTF8.GetString(bytes);
                }
                catch (WebException e)
                {
                    var status = ((FtpWebResponse)e.Response).StatusDescription;

                    result = false;
                }
                catch (Exception exc)
                {
                    result = false;
                }
                finally
                {
                    if (ftpClient.IsConnected && !StayConnected)
                        ftpClient.Disconnect();
                }
            }

            return null;
        }

        public bool Upload(string localFilePath, string ftpDirectory, int retryCount = 1)
        {
            if (!System.IO.File.Exists(localFilePath))
                return false;

            var localFileContents = System.IO.File.ReadAllBytes(localFilePath);
            var localFileName = Path.GetFileName(localFilePath);

            var remoteFilePath = "";
            if (!string.IsNullOrEmpty(ftpDirectory))
                remoteFilePath = $"{remoteFilePath.TrimEnd('/')}/{ftpDirectory}";

            remoteFilePath = $"{remoteFilePath.TrimEnd('/')}/{localFileName}";

            int tryCount = 0;
            bool result = false;

            while (tryCount < retryCount && !result)
            {
                tryCount += 1;
                try
                {
                    if (!ftpClient.IsConnected)
                        ftpClient.Connect();

                    ftpClient.RetryAttempts = 5;

                    var ftpStatus = ftpClient.UploadFile(localFilePath, remoteFilePath, FtpRemoteExists.Overwrite, createRemoteDir: false, FtpVerify.Retry);
                    if (ftpStatus == FtpStatus.Success)
                        return true;

                    result = false;
                }
                catch (WebException e)
                {
                    var status = ((FtpWebResponse)e.Response).StatusDescription;

                    result = false;
                }
                catch (Exception ex)
                {
                    result = false;
                }
                finally
                {
                    if (ftpClient.IsConnected && !StayConnected)
                        ftpClient.Disconnect();
                }
            }

            return false;
        }

        public bool CompareFileSize(string localFilePath, string remoteFilePath)
        {
            try
            {
                if (!ftpClient.IsConnected)
                    ftpClient.Connect();

                var result = ftpClient.CompareFile(localFilePath, remoteFilePath, FtpCompareOption.Size);

                return result == FtpCompareResult.Equal;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (ftpClient.IsConnected && !StayConnected)
                    ftpClient.Disconnect();
            }
        }

        public bool UploadTxt(string text, string fileName, string ftpDirectory, int retryCount = 1)
        {
            var localFileContents = Encoding.UTF8.GetBytes(text);

            var remoteFilePath = "";
            if (!string.IsNullOrEmpty(ftpDirectory))
                remoteFilePath = $"{remoteFilePath.TrimEnd('/')}/{ftpDirectory}";

            remoteFilePath = $"{remoteFilePath.TrimEnd('/')}/{fileName}";
            int tryCount = 0;
            bool result = false;

            while (tryCount < retryCount && !result)
            {
                tryCount += 1;

                try
                {
                    if (!ftpClient.IsConnected)
                        ftpClient.Connect();

                    var ftpStatus = ftpClient.Upload(localFileContents, remoteFilePath, FtpRemoteExists.Overwrite, createRemoteDir: false);
                    if (ftpStatus == FtpStatus.Success)
                        return true;

                    result = false;
                }
                catch (Exception ex)
                {
                    result = false;
                }
                finally
                {
                    if (ftpClient.IsConnected && !StayConnected)
                        ftpClient.Disconnect();
                }
            }

            return false;
        }

        public bool UploadDirectory(string dirPath, string uploadPath, bool makeMirror = false)
        {
            try
            {
                if (!ftpClient.IsConnected)
                    ftpClient.Connect();

                var ftpStatus = ftpClient.UploadDirectory(dirPath, uploadPath, makeMirror ? FtpFolderSyncMode.Mirror : FtpFolderSyncMode.Update
                    , FtpRemoteExists.Overwrite, FtpVerify.Retry);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (ftpClient.IsConnected && !StayConnected)
                    ftpClient.Disconnect();
            }
        }

        public bool Delete(string remoteFilePath, int retryCount = 3)
        {
            int currentCount = 0;
            var result = false;

            while (currentCount <= retryCount && !result)
            {
                currentCount += 1;

                try
                {
                    if (!ftpClient.IsConnected)
                        ftpClient.Connect();

                    ftpClient.DeleteFile(remoteFilePath);

                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "The system cannot find the file specified. ")
                        return false;

                    result = false;
                }
                finally
                {
                    if (ftpClient.IsConnected && !StayConnected)
                        ftpClient.Disconnect();
                }
            }

            return false;
        }

        public bool CreateDirectory(string directory, int retryCount = 3)
        {
            int currentCount = 0;
            bool result = false;

            while (currentCount < retryCount && !result)
            {
                currentCount += 1;

                try
                {
                    var remoteFilePath = $"/{directory.TrimStart('/')}";

                    if (!ftpClient.IsConnected)
                        ftpClient.Connect();

                    var createDirectoryResult = ftpClient.CreateDirectory(remoteFilePath, force: true);
                    if (!createDirectoryResult)
                    {
                        if (ftpClient.DirectoryExists(remoteFilePath))
                            return true;

                        result = false;
                    }

                    if (createDirectoryResult) return true;

                    result = false;
                }
                catch (WebException e)
                {
                    var status = ((FtpWebResponse)e.Response).StatusDescription;
                    if (status.Contains("that file already exists"))
                        return true;

                    result = false;
                }
                catch (Exception ex)
                {

                    return false;
                }
                finally
                {
                    if (ftpClient.IsConnected && !StayConnected)
                        ftpClient.Disconnect();
                }
            }

            return false;
        }

        public bool DeleteDirectory(string directory, int retryCount = 3)
        {
            int currentCount = 0;
            bool result = false;

            while (currentCount < retryCount && !result)
            {
                currentCount += 1;

                try
                {
                    var remoteFilePath = $"/{directory.TrimStart('/')}";

                    if (!ftpClient.IsConnected)
                        ftpClient.Connect();

                    ftpClient.DeleteDirectory(remoteFilePath);
                    return true;
                }
                catch (Exception ex)
                {
                    result = false;
                }
                finally
                {
                    if (ftpClient.IsConnected && !StayConnected)
                        ftpClient.Disconnect();
                }
            }

            return false;
        }

        public bool DirectoryExists(string directory)
        {
            try
            {
                var remoteFilePath = $"/{directory.TrimStart('/')}";

                if (!ftpClient.IsConnected)
                    ftpClient.Connect();

                return ftpClient.DirectoryExists(directory);
            }
            catch (WebException e)
            {
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                if (ftpClient.IsConnected && !StayConnected)
                    ftpClient.Disconnect();
            }
        }

        public bool FileExists(string filePath)
        {
            try
            {
                if (!ftpClient.IsConnected)
                    ftpClient.Connect();

                return ftpClient.FileExists(filePath);
            }
            catch (WebException e)
            {
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                if (ftpClient.IsConnected && !StayConnected)
                    ftpClient.Disconnect();
            }
        }

        public bool SetWritePermission(string remoteFilePath)
        {
            try
            {
                if (!ftpClient.IsConnected)
                    ftpClient.Connect();

                ftpClient.SetFilePermissions(remoteFilePath, FtpPermission.Write, FtpPermission.Write, FtpPermission.Write);

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
            finally
            {
                if (ftpClient.IsConnected && !StayConnected)
                    ftpClient.Disconnect();
            }
        }

        public bool Disconnect()
        {
            if (ftpClient.IsConnected)
                ftpClient.Disconnect();

            return true;
        }

        //# Fast One Shot, static methods
        public static bool Delete(FtpCredentialsDto c, string remoteFilePath) 
        {
            var client = new FtpFluentClient(c.Address, c.Port, c.Username, c.Password, stayConnected: true);
            var result = client.Delete(remoteFilePath);

            client.Disconnect();

            return (!result && client.FileExists(remoteFilePath));
        }
    }
}
