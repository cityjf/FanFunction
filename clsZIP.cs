using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace FanFunction
{
    /// <summary>
    /// 压缩和解压缩的类，需引用ICSharpCode.SharpZipLib.dll
    /// 下载地址http://www.icsharpcode.net/opensource/sharpziplib/
    /// </summary>
    public class clsZIP
    {
        /// <summary>
        /// 递归压缩文件夹方法
        /// </summary>
        /// <param name="FolderToZip"></param>
        /// <param name="s"></param>
        /// <param name="ParentFolderName"></param>
        /// <returns></returns>
        private static bool ZipFileDictory(string FolderToZip, ZipOutputStream s, string ParentFolderName)
        {
            bool res = true;
            string[] folders, filenames;
            ZipEntry entry = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();
            try
            {

                //创建当前文件夹
                entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/"));  //加上 “/” 才会当成是文件夹创建
                s.PutNextEntry(entry);
                s.Flush();

                //先压缩文件，再递归压缩文件夹 
                filenames = Directory.GetFiles(FolderToZip);
                foreach (string file in filenames)
                {
                    //打开压缩文件
                    fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/" + Path.GetFileName(file)));

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    entry.Crc = crc.Value;

                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                if (entry != null)
                {
                    entry = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            folders = Directory.GetDirectories(FolderToZip);
            foreach (string folder in folders)
            {
                if (!ZipFileDictory(folder, s, Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip))))
                {
                    return false;
                }
            }
            return res;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="FileToZip">要进行压缩的文件名</param>
        /// <param name="ZipedFile">压缩后生成的压缩文件名</param>
        /// <returns></returns>
        private static bool ZipFile(string FileToZip, string ZipedFile, String Password)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(FileToZip))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + FileToZip + " 不存在!");
            }
            FileStream ZipFile = null;
            ZipOutputStream ZipStream = null;
            ZipEntry ZipEntry = null;
            bool res = true;
            try
            {
                ZipFile = File.OpenRead(FileToZip);
                byte[] buffer = new byte[ZipFile.Length];
                ZipFile.Read(buffer, 0, buffer.Length);
                ZipFile.Close();

                ZipFile = File.Create(ZipedFile);
                ZipStream = new ZipOutputStream(ZipFile);
                ZipStream.Password = Password;
                ZipEntry = new ZipEntry(Path.GetFileName(FileToZip));
                ZipStream.PutNextEntry(ZipEntry);
                ZipStream.SetLevel(6);

                ZipStream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (ZipEntry != null)
                {
                    ZipEntry = null;
                }
                if (ZipStream != null)
                {
                    ZipStream.Finish();
                    ZipStream.Close();
                }
                if (ZipFile != null)
                {
                    ZipFile.Close();
                    ZipFile = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            return res;
        }
        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="FolderToZip">待压缩的文件夹，全路径格式</param>
        /// <param name="ZipedFile">压缩后的文件名，全路径格式</param>
        /// <param name="Password"></param>
        /// <returns></returns>
        private static bool ZipFileDictory(string FolderToZip, string ZipedFile, String Password)
        {
            bool res;
            if (!Directory.Exists(FolderToZip))
            {
                return false;
            }

            ZipOutputStream s = new ZipOutputStream(File.Create(ZipedFile));
            s.SetLevel(6);
            s.Password = Password;

            res = ZipFileDictory(FolderToZip, s, "");

            s.Finish();
            s.Close();

            return res;
        }
        /// <summary>
        /// 压缩文件或文件夹
        /// </summary>
        /// <param name="FileToZip">待压缩的文件或文件夹，物理路径</param>
        /// <param name="ZipedFile">压缩后生成的压缩文件名，物理路径</param>
        /// <returns>成功与否，如果未找到指定的文件或文件夹会抛出异常</returns>
        public static bool Zip(String FileToZip, String ZipedFile)
        {
            if (Directory.Exists(FileToZip))
            {
                return ZipFileDictory(FileToZip, ZipedFile, string.Empty);
            }
            else if (File.Exists(FileToZip))
            {
                return ZipFile(FileToZip, ZipedFile, string.Empty);
            }
            else
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件或目录: " + FileToZip + " 不存在!");
            }
        }
        /// <summary>
        /// 压缩文件或文件夹(可以加密)
        /// </summary>
        /// <param name="FileToZip">待压缩的文件或文件夹，物理路径</param>
        /// <param name="ZipedFile">压缩后生成的压缩文件名，物理路径</param>
        /// <param name="Password">压缩文件是否加密，加密请传入密码，否则为空</param>
        /// <returns>成功与否，如果未找到指定的文件或文件夹会抛出异常</returns>
        public static bool Zip(String FileToZip, String ZipedFile, String Password)
        {
            if (Directory.Exists(FileToZip))
            {
                return ZipFileDictory(FileToZip, ZipedFile, Password);
            }
            else if (File.Exists(FileToZip))
            {
                return ZipFile(FileToZip, ZipedFile, Password);
            }
            else
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件或目录: " + FileToZip + " 不存在!");
            }
        }
        /// <summary>
        /// 解压功能(解压压缩文件到指定目录)
        /// </summary>
        /// <param name="FileToUpZip">待解压的文件，物理路径</param>
        /// <param name="ZipedFolder">指定解压目标目录，物理路径</param>
        public static void UnZip(string FileToUpZip, string ZipedFolder)
        {
            UnZip(FileToUpZip, ZipedFolder, string.Empty);
        }
        /// <summary>
        /// 解压功能(解压压缩文件到指定目录,可以解压带密码的)
        /// </summary>
        /// <param name="FileToUpZip">待解压的文件，物理路径</param>
        /// <param name="ZipedFolder">指定解压目标目录，物理路径</param>
        /// <param name="Password">压缩文件是否有密码，有则传入密码，否则为空</param>
        public static void UnZip(string FileToUpZip, string ZipedFolder, string Password)
        {
            if (!File.Exists(FileToUpZip))
            {
                return;
            }

            if (!Directory.Exists(ZipedFolder))
            {
                Directory.CreateDirectory(ZipedFolder);
            }

            ZipInputStream s = null;
            ZipEntry theEntry = null;

            string fileName;
            FileStream streamWriter = null;
            try
            {
                s = new ZipInputStream(File.OpenRead(FileToUpZip));
                s.Password = Password;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.Name != String.Empty)
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        //判断文件路径是否是文件夹
                        if (fileName.EndsWith("/") || fileName.EndsWith("//"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        streamWriter = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter = null;
                }
                if (theEntry != null)
                {
                    theEntry = null;
                }
                if (s != null)
                {
                    s.Close();
                    s = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
        }
    }
}
