using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace FanFunction
{
    /// <summary>
    /// ftp操作相关的类,调用之前必须实例化此类，有2种方式实例化此类(一)传入ftp的用户名和密码，适用于需要登录的ftp（二）不需要传参数，适用于不需要登录的ftp
    /// </summary>
    public class clsFtp
    {
        /// <summary>
        /// ftp连接
        /// </summary>
        /// <param name="path">ftp地址</param>
        /// <returns></returns>
        private FtpWebRequest FtpConnect(string path)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri(path));
                if (!string.IsNullOrEmpty(ftpUser) && !string.IsNullOrEmpty(ftpPwd))
                {
                    ftpRequest.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                }
                ftpRequest.UseBinary = true;//文本文件传送可以用false,其他true
                ftpRequest.KeepAlive = false;//如果用递归获取列表,此句非常重要
                return ftpRequest;
            }
            catch (Exception ex)
            {
                throw new Exception("clsFtp类的FtpConnect方法出现异常," + ex.Message);
            }
        }

        /// <summary>
        /// ftp用户名
        /// </summary>
        private string ftpUser;

        public string FtpUser
        {
            get { return ftpUser; }
            set { ftpUser = value; }
        }

        /// <summary>
        /// ftp密码
        /// </summary>
        private string ftpPwd;

        public string FtpPwd
        {
            get { return ftpPwd; }
            set { ftpPwd = value; }
        }

        /// <summary>
        /// ftp需要登录
        /// </summary>
        /// <param name="strUser"></param>
        /// <param name="strPwd"></param>
        public clsFtp(string strUser, string strPwd)
        {
            this.ftpUser = strUser;
            this.ftpPwd = strPwd;
        }

        /// <summary>
        /// ftp不需要登录
        /// </summary>
        public clsFtp()
        { }

        /// <summary>
        /// 单个文件下载方法 
        /// </summary>
        /// <param name="physicsPath">本地存放路径,如:C:\1.txt</param>
        /// <param name="ftpPath">ftp地址,如:ftp://127.0.0.1/1.txt </param>
        public void DownloadFile(string physicsPath, string ftpPath)
        {
            Stream ftpStream = null;
            FileStream outputStream = null;
            FtpWebResponse response = null;
            FtpWebRequest ftpRequest = FtpConnect(ftpPath);
            if (ftpRequest != null)
            {
                try
                {
                    string strBackupPath = physicsPath.Substring(0, physicsPath.LastIndexOf("\\"));
                    //文件夹不存在就创建
                    if (Directory.Exists(strBackupPath) == false)
                    {
                        Directory.CreateDirectory(strBackupPath);
                    }
                    outputStream = new FileStream(physicsPath, FileMode.Create);
                    //设置要发送到 FTP 服务器的命令   
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    response = (FtpWebResponse)ftpRequest.GetResponse();
                    ftpStream = response.GetResponseStream();
                    long cl = response.ContentLength;
                    int bufferSize = 2048;
                    int readCount;
                    byte[] buffer = new byte[bufferSize];
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        outputStream.Write(buffer, 0, readCount);
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("clsFtp类的DownloadFile方法出现异常。当前本地保存地址是：" + physicsPath + "，当前ftp下载地址是：" + ftpPath + "," + ex.Message);
                }
                finally
                {
                    if (ftpStream != null)
                        ftpStream.Close();
                    if (outputStream != null)
                        outputStream.Close();
                    if (response != null)
                        response.Close();
                }
            }
            else
            {
                throw new Exception("clsFtp类的DownloadFile方法出现空的ftpRequest对象");
            }
        }
        /// <summary>
        /// 获取ftp文件列表
        /// </summary>
        /// <param name="path">ftp地址,如:ftp://127.0.0.1/ </param>
        /// <returns></returns>
        public string[] GetFtpFileList(string path)
        {
            return GetFtpFileList(path, WebRequestMethods.Ftp.ListDirectory);
        }
        /// <summary>
        /// 获取ftp文件列表
        /// </summary>
        /// <param name="path">ftp地址,如:ftp://127.0.0.1/</param>
        /// <param name="methodType">类型,如:WebRequestMethods.Ftp.ListDirectory</param>
        /// <returns>文件目录列表</returns>
        public string[] GetFtpFileList(string path, string methodType)
        {
            StreamReader reader = null;
            WebResponse ftpResponse = null;
            FtpWebRequest ftpRequest = FtpConnect(path);
            if (ftpRequest != null)
            {
                StringBuilder builder = new StringBuilder();
                ftpRequest.Method = methodType;
                try
                {
                    ftpResponse = ftpRequest.GetResponse();
                    reader = new StreamReader(ftpResponse.GetResponseStream(), Encoding.Default);
                    string line = reader.ReadToEnd();
                    reader.Close();
                    ftpResponse.Close();
                    return line.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                }
                catch (Exception ex)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (ftpResponse != null)
                    {
                        ftpResponse.Close();
                    }
                    throw new Exception("clsFtp类的GetFtpFileList方法出现异常。当前path地址是：" + path + "，当前methodType地址是：" + methodType + "。" + ex.Message);
                }
            }
            else
            {
                throw new Exception("clsFtp类的GetFtpFileList方法出现空的ftpRequest对象");
            }
        }
        /// <summary>
        /// 上传到ftp
        /// </summary>
        /// <param name="fileName">本地文件的完整路径,如:C:\1.txt</param>
        /// <param name="uploadUrl">要上传到ftp的完整路径,如:ftp://127.0.0.1/1.txt </param>
        /// <returns></returns>
        public FtpStatusCode UploadToFTP(string fileName, string uploadUrl)
        {
            Stream requestStream = null;
            FileStream fileStream = null;
            FtpWebResponse uploadResponse = null;
            try
            {
                Uri uri = new Uri(uploadUrl);
                FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(uri);
                uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
                uploadRequest.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                requestStream = uploadRequest.GetRequestStream();
                fileStream = File.Open(fileName, FileMode.Open);

                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    requestStream.Write(buffer, 0, bytesRead);
                }
                requestStream.Close();

                uploadResponse = (FtpWebResponse)uploadRequest.GetResponse();
                return uploadResponse.StatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("上传方法UploadToFTP出错,当前的文件的本地路径是：" + fileName + "上传到ftp的地址是：" + uploadUrl + "," + ex.Message);
            }
            finally
            {
                if (uploadResponse != null)
                    uploadResponse.Close();
                if (fileStream != null)
                    fileStream.Close();
                if (requestStream != null)
                    requestStream.Close();
            }
        }
        /// <summary>
        /// 获取ftp文件大小
        /// </summary>
        /// <param name="strUrl">如:ftp://127.0.0.1/1.txt </param>
        /// <returns></returns>
        public long GetFileSize(string strUrl)
        {
            long fileSize = 0;
            try
            {
                FtpWebRequest ftpRequest = FtpConnect(strUrl);
                ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                fileSize = response.ContentLength;
                response.Close();
                return fileSize;
            }
            catch (Exception ex)
            {
                throw new Exception("获取ftp文件大小的方法出现错误，" + ex.Message);
            }
        }
        /// <summary>
        /// ftp无限层建立文件夹
        /// </summary>
        /// <param name="strDirectory">如:ftp://127.0.0.1/1/2/3 </param>
        public void CreatDirectorySome(string strDirectory)
        {
            string[] lstDirectory = strDirectory.Split('/');
            string tempDirectory = string.Empty;
            for (int i = 0; i < lstDirectory.Length - 1; i++)
            {
                tempDirectory += lstDirectory[i].ToString() + "/";
                if (i > 2)
                {
                    CreatDirectory(tempDirectory);
                }
            }
        }
        /// <summary>
        /// ftp建立文件夹
        /// </summary>
        /// <param name="strDirectory">如:ftp://127.0.0.1/1 </param>
        public void CreatDirectory(string strDirectory)
        {
            FtpWebRequest ftpRequest = null;
            FtpWebResponse response = null;
            try
            {
                ftpRequest = FtpConnect(strDirectory);
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                response = (FtpWebResponse)ftpRequest.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("建立文件夹的方法出现错误，当前要建立的文件夹路径是：" + strDirectory + "," + ex.Message);
            }
        }
        /// <summary>
        /// ftp删除文件
        /// </summary>
        /// <param name="strUrl">如:ftp://127.0.0.1/1.txt </param>
        public void Delete(string strUrl)
        {
            FtpWebRequest ftpRequest = null;
            FtpWebResponse response = null;
            try
            {
                ftpRequest = FtpConnect(strUrl);
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                response = (FtpWebResponse)ftpRequest.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("调用删除文件的方法出现错误，当前要删除的路径是：" + strUrl + "，" + ex.Message);
            }
        }
    }
}
