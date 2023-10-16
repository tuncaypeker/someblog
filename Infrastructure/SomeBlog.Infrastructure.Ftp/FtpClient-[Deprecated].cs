using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SomeBlog.Infrastructure.Ftp
{
    /// <summary>
    /// Deprecated: Please Use FtpFluentClient
    /// </summary>
    public class FtpClient
    {
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public FtpClient(string address, string username, string password)
        {
            Address = address;
            Username = username;
            Password = password;
        }

        public List<string> GetFiles(string ftpDirectory)
        {
            var url = this.Address;
            if (!string.IsNullOrEmpty(ftpDirectory))
                url = $"{url.TrimEnd('/')}/{ftpDirectory}";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(this.Username, this.Password);
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                reader.Close();
                response.Close();

                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception exc)
            {
                return new List<string>();
            }
        }

        public string DownloadString(string ftpPath)
        {
            var url = this.Address;
            if (!string.IsNullOrEmpty(ftpPath))
                url = $"{url.TrimEnd('/')}/{ftpPath}";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(this.Username, this.Password);
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string content = reader.ReadToEnd();

                reader.Close();
                response.Close();

                return content;
            }
            catch (WebException e)
            {
                var status = ((FtpWebResponse)e.Response).StatusDescription;

                return null;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public bool Upload(string localFilePath, string ftpDirectory)
        {
            if (!System.IO.File.Exists(localFilePath))
                return false;

            var localFileContents = System.IO.File.ReadAllBytes(localFilePath);
            var localFileName = Path.GetFileName(localFilePath);

            var remoteFilePath = this.Address;
            if (!string.IsNullOrEmpty(ftpDirectory))
                remoteFilePath = $"{remoteFilePath.TrimEnd('/')}/{ftpDirectory}";

            remoteFilePath = $"{remoteFilePath.TrimEnd('/')}/{localFileName}";
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteFilePath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UsePassive = false;
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(this.Username, this.Password);

                request.ContentLength = localFileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(localFileContents, 0, localFileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();

                return true;
            }
            catch (WebException e) { 
                var status = ((FtpWebResponse)e.Response).StatusDescription;

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UploadTxt(string text, string fileName, string ftpDirectory)
        {
            var localFileContents = Encoding.ASCII.GetBytes(text);

            var remoteFilePath = this.Address;
            if (!string.IsNullOrEmpty(ftpDirectory))
                remoteFilePath = $"{remoteFilePath.TrimEnd('/')}/{ftpDirectory}";

            remoteFilePath = $"{remoteFilePath.TrimEnd('/')}/{fileName}";
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteFilePath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UsePassive = false;
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(this.Username, this.Password);

                request.ContentLength = localFileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(localFileContents, 0, localFileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool UploadDirectory(string dirPath, string uploadPath)
        {
            string[] files = Directory.GetFiles(dirPath, "*.*");
            string[] subDirs = Directory.GetDirectories(dirPath);

            foreach (string file in files)
            {
                Upload(file, uploadPath + "/" + Path.GetFileName(file));
            }

            foreach (string subDir in subDirs)
            {
                CreateDirectory(uploadPath + "/" + Path.GetFileName(subDir));
                UploadDirectory(subDir, uploadPath + "/" + Path.GetFileName(subDir));
            }

            return true;
        }

        public bool Delete(string remoteFilePath)
        {
            try
            {
                remoteFilePath = $"{this.Address.TrimEnd('/')}/{remoteFilePath}";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteFilePath);

                request.Credentials = new NetworkCredential(this.Username, this.Password);
                request.Method = WebRequestMethods.Ftp.DeleteFile;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool CreateDirectory(string directory)
        {
            try
            {
                var remoteFilePath = $"{this.Address.TrimEnd('/')}/{directory.TrimStart('/')}";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteFilePath);

                request.Credentials = new NetworkCredential(this.Username, this.Password);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();

                return true;
            }
            catch (WebException e)
            {
                var status = ((FtpWebResponse)e.Response).StatusDescription;
                if (status.Contains("that file already exists"))
                    return true;

                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
