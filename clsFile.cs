using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace FanFunction
{
    /// <summary>
    /// 关于文件操作的类
    /// </summary>
    public class clsFile
    {
        /// <summary>
        /// 创建一个文件，如果文件夹不存在时则先创建文件夹，系统自带的File.Create如果文件夹不存在无法建立文件
        /// </summary>
        /// <param name="path"></param>
        public static void Create(string path)
        {
            CreatFolder(path);
            File.Create(path);
        }
        /// <summary>
        /// 创建文件夹,目录名要以斜杠"\"结尾
        /// </summary>
        /// <param name="strPath">目录路径</param>
        public static void CreatFolder(string strPath)
        {
            strPath = strPath.Substring(0, strPath.LastIndexOf("\\") + 1);
            if (System.IO.Directory.Exists(strPath) == false)
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
        }
        /// <summary>
        /// http文件下载
        /// </summary>
        /// <param name="httpURL">http文件地址</param>
        /// <param name="physicsPath">本地保存的地址</param>
        public static void DownloadFile(string httpURL, string physicsPath)
        {
            Stream stream = null;
            FileStream outputStream = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(httpURL);
                response = (HttpWebResponse)request.GetResponse();
                outputStream = new FileStream(physicsPath, FileMode.Create);
                stream = response.GetResponseStream();
                string strDirPath = physicsPath.Substring(0, physicsPath.LastIndexOf("\\"));
                //文件夹不存在就创建
                if (Directory.Exists(strDirPath) == false)
                {
                    Directory.CreateDirectory(strDirPath);
                }
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = stream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = stream.Read(buffer, 0, bufferSize);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("clsFile类的DownloadFile方法出现异常。当前本地保存地址是：" + physicsPath + "，当前http地址是：" + httpURL + "," + ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (outputStream != null)
                    outputStream.Close();
                if (response != null)
                    response.Close();
            }
        }

        /// <summary>
        /// 获取文本内容
        /// </summary>
        /// <param name="strPath">文件路径</param>
        /// <param name="removeEmptyLine">是否去除空白行，默认为不去除</param>
        /// <returns></returns>
        public static List<string> GetFileContent(string strPath, bool removeEmptyLine = false)
        {
            List<string> contents = new List<string>();
            using (StreamReader sr = new StreamReader(strPath))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (removeEmptyLine && string.IsNullOrEmpty(line))
                        continue;
                    contents.Add(line);
                }
            }
            return contents;
        }
    }
}
