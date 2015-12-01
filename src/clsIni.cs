using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace FanFunction
{
 /*
 * 关于ini文件的操作,一种方式是用Kernal32.dll中封装的2个方法WritePrivateProfileString 和 GetPrivateProfileString
    另一种方式是用流的方式,把ini当成文本文件操作
 * 这里使用调用Kernal32.dll的方式
 */


    /*
     * 例：
        [config]
        PATH=D:\ABC\
        PATH1=D:\ABC11\
     
     * 调用：
     *  clsIni ini = new clsIni("aa.ini");
        string strini1 = ini.IniReadValue("config", "PATH");
        ArrayList arr = ini.GetAllKey("config");
        ArrayList arr1 = ini.GetAllSections();
        ArrayList arr2 = ini.GetIniSectionValue("config");
        ini.IniWriteValue("config", "Path2", "123123");
     * 
     */
    public class clsIni
    {
        /// <summary>
        /// 文件物理路径
        /// </summary>
        private string filePath;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        /// <summary>
        /// 写入INI文件的API函数
        /// </summary>
        /// <param name="section">INI文件中的段落名称</param>
        /// <param name="key">INI文件中的关键字</param>
        /// <param name="val">INI文件中关键字的数值</param>
        /// <param name="filePath">INI文件的完整的路径和名称</param>
        /// <returns></returns>
　　　　[System.Runtime.InteropServices.DllImport("kernel32")]
　　　　private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        /// <summary>
        /// 读取INI文件的API函数
        /// </summary>
        /// <param name="section">INI文件中的段落名称</param>
        /// <param name="key">INI文件中的关键字</param>
        /// <param name="def">无法读取时候时候的缺省数值</param>
        /// <param name="retVal">读取数值</param>
        /// <param name="size">数值的大小</param>
        /// <param name="filePath">INI文件的完整路径和名称</param>
        /// <returns></returns>
　　　　[System.Runtime.InteropServices.DllImport("kernel32")]
　　　　private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        //获取section下所有key,GetPrivateProfileStringA名字不能改变
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private extern static int GetPrivateProfileStringA(string section, string key, string def, byte[] buffer, int size, string strFileName);
        //获取ini文件所有的section
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        private extern static int GetPrivateProfileSectionNamesA(byte[] buffer, int iLen, string fileName);
        //获取指定Section的key和value        
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string lpFileName); 
        /// <summary>
        /// //构造函数
        /// </summary>
        /// <param name="fileName">文件名</param>
        public clsIni(string fileName)
        {
            filePath = System.Windows.Forms.Application.StartupPath + "\\" + fileName;//WinForm中获取路径的方法
        }
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="Section">根节点名称(如 [FarmCrop] )</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void IniWriteValue(string section, string key, string value)
        {
            if (WritePrivateProfileString(section, key, value, filePath) == false)
            {
                throw (new Exception("写Ini文件出错"));
            }
        }
        /// <summary>
        /// 读出INI文件下某根节点名称下某key对应的value
        /// </summary>
        /// <param name="section">根节点名称(如 [FarmCrop] )</param>
        /// <param name="key">键</param>
        public string IniReadValue(string section, string key)
        {
            StringBuilder builder = new StringBuilder(65535);
            int i = GetPrivateProfileString(section, key, "", builder, 65535, filePath);
            return builder.ToString();
        }
        /// <summary>
        /// 获取某section下所有key集合
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public ArrayList GetAllKey(string section)
        {
            byte[] buffer = new byte[65535];
            int rel = GetPrivateProfileStringA(section, null, "", buffer, buffer.GetUpperBound(0), filePath);

            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                        arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }
        /// <summary>
        /// 返回该配置文件中所有section名称的集合
        /// </summary>
        /// <returns></returns>
        public ArrayList GetAllSections()
        {
            byte[] buffer = new byte[65535];
            int rel = GetPrivateProfileSectionNamesA(buffer, buffer.GetUpperBound(0), filePath);
            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }
        /// <summary>
        /// 读取指定节点下的所有key 和value(例:path=C:\)
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public ArrayList GetIniSectionValue(string section)
        {
            byte[] buffer = new byte[5120];
            int rel = GetPrivateProfileSection(section, buffer, buffer.GetUpperBound(0), filePath);

            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }
    }
}