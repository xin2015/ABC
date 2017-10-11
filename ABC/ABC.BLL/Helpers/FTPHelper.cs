using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ABC.BLL.Helpers
{
    public class FTPHelper
    {
        private NetworkCredential networkCredential;
        private string baseUriString;

        public FTPHelper(string baseUriString, string userName, string password)
        {
            this.baseUriString = baseUriString;
            networkCredential = new NetworkCredential(userName, password);
        }

        private FtpWebRequest GetRequest(string requestUriString, string method)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(requestUriString);
            request.Credentials = networkCredential;
            request.Method = method;
            return request;
        }

        public string Get(string path, string method, Encoding encoding)
        {
            FtpWebRequest request = GetRequest(baseUriString + "/" + path, method);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), encoding);
            string result = sr.ReadToEnd();
            sr.Close();
            response.Close();
            return result;
        }

        public string Get(string path, string method)
        {
            return Get(path, method, Encoding.Default);
        }

        public string DownloadFile(string path)
        {
            return Get(path, WebRequestMethods.Ftp.DownloadFile);
        }

        public string DeleteFile(string path)
        {
            return Get(path, WebRequestMethods.Ftp.DeleteFile);
        }

        public string[] ListDirectoryDetails(string path)
        {
            string result = Get(path, WebRequestMethods.Ftp.ListDirectoryDetails);
            return result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] ListDirectory(string path)
        {
            string result = Get(path, WebRequestMethods.Ftp.ListDirectory);
            return result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
