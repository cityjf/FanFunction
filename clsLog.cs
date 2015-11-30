using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FanFunction
{
    /// <summary>
    /// 记录执行日志的类
    /// </summary>
    public class clsLog
    {
        /// <summary>
        /// 默认记录日志文件到C盘根目录,文件名为当天的日期，如：2012-12-12.log，注意：web下没有写入到某个磁盘的权限
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLog(string log)
        {
            WriteLog(log, "C:\\", DateTime.Now.ToString("yyyy-MM-dd") + ".log");
        }
        /// <summary>
        /// 记录日志文件到指定目录,文件名为当天的日期，如：2012-12-12.log
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="path">日志的路径，如C:\\123\</param>
        public static void WriteLog(string log, string path)
        {
            WriteLog(log, path, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
        }
        /// <summary>
        /// 记录日志文件到指定目录,文件名为指定名称
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="path">日志的路径，如C:\\123\</param>
        public static void WriteLog(string log, string path, string strFileName)
        {
            string strFilePath = path;//日志目录
            //判断文件夹是否存在
            if (Directory.Exists(strFilePath) == false)
            {
                Directory.CreateDirectory(strFilePath);
            }
            if (!path.EndsWith("\\")) path = path + "\\";//末尾加上斜杠
            string fileName = strFilePath + strFileName;
            //如果文件存在
            if (File.Exists(fileName))
            {
                StreamWriter swAddTo = File.AppendText(fileName);
                //swAddTo.WriteLine("----------分割线----------");
                //swAddTo.WriteLine("时间：" + System.DateTime.Now);
                swAddTo.WriteLine(log);
                swAddTo.Close();
            }
            else
            {
                StreamWriter swCreat = new StreamWriter(fileName);
                //swCreat.WriteLine("----------分割线----------");
                //swCreat.WriteLine("时间：" + System.DateTime.Now);
                swCreat.WriteLine(log);
                swCreat.Close();
            }
        }
    }
}
