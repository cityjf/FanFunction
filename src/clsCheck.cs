using System;
using System.Collections.Generic;
using System.Text;

namespace FanFunction
{
    public class clsCheck
    {
        /// <summary>
        /// 真正判断文件类型的关键函数(不太准确，比如txt获取到的值都不一样)
        /// </summary>
        /// <param name="hifile"></param>
        /// <returns></returns>
        public static string GetFileTrueType(System.Web.HttpPostedFile postedFile)
        {
            //System.IO.FileStream fs = new System.IO.FileStream(strPhysicsPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.Stream fs = postedFile.InputStream;
            System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch { }
            r.Close();
            fs.Close();
            /*文件扩展名说明
             *7173        gif 
             *255216      jpg
             *13780       png
             *6677        bmp
             *239187      txt,aspx,asp,sql
             *208207      xls.doc.ppt
             *6063        xml
             *6033        htm,html
             *4742        js
             *8075        xlsx,zip,pptx,mmap,zip
             *8297        rar   
             *01          accdb,mdb
             *7790        exe,dll           
             *5666        psd 
             *255254      rdp 
             *10056       bt种子 
             *64101       bat 
             */
            if (fileclass == "255216")//说明255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar
            {
                return "jpg";
            }
            else if (fileclass == "7173")
            {
                return "gif";
            }
            else if (fileclass == "6677")
            {
                return "bmp";
            }
            else if (fileclass == "13780")
            {
                return "png";
            }
            else if (fileclass == "7790")
            {
                return "exe";
            }
            else if (fileclass == "8297")
            {
                return "rar/zip";
            }
            else if (fileclass == "208207")
            {
                return "doc/xls";
            }
            else if (fileclass == "8075")
            {
                return "docx/xlsx";
            }
            else if (fileclass == "5155")
            {
                return "txt";
            }
            else if (fileclass == "6787")
            {
                return "swf";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 真正判断文件类型的关键函数(未测试)
        /// </summary>
        /// <param name="hifile"></param>
        /// <returns></returns>
        public static string GetFileTrueType(string postedFile)
        {
            System.IO.FileStream fs = new System.IO.FileStream(postedFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch { }
            r.Close();
            fs.Close();
            /*文件扩展名说明
             *7173        gif 
             *255216      jpg
             *13780       png
             *6677        bmp
             *239187      txt,aspx,asp,sql
             *208207      xls.doc.ppt
             *6063        xml
             *6033        htm,html
             *4742        js
             *8075        xlsx,zip,pptx,mmap,zip
             *8297        rar   
             *01          accdb,mdb
             *7790        exe,dll           
             *5666        psd 
             *255254      rdp 
             *10056       bt种子 
             *64101       bat 
             */
            if (fileclass == "255216")//说明255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar
            {
                return "jpg";
            }
            else if (fileclass == "7173")
            {
                return "gif";
            }
            else if (fileclass == "6677")
            {
                return "bmp";
            }
            else if (fileclass == "13780")
            {
                return "png";
            }
            else if (fileclass == "7790")
            {
                return "exe";
            }
            else if (fileclass == "8297")
            {
                return "rar/zip";
            }
            else if (fileclass == "208207")
            {
                return "doc/xls";
            }
            else if (fileclass == "8075")
            {
                return "docx/xlsx";
            }
            else if (fileclass == "5155")
            {
                return "txt";
            }
            else if (fileclass == "6787")
            {
                return "swf";
            }
            else
            {
                return "";
            }
        }


    }
}
