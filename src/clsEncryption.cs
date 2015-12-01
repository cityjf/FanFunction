using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace FanFunction
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public class clsEncryption
    {
        #region DES加密解密
        /// <summary>
        /// DES加密解密
        /// </summary>
        public class DES
        {
            //默认密钥向量
            private static byte[] Keys = { 0xEF, 0x90, 0xCD, 0x56, 0x12, 0xAB, 0x78, 0x34 };
            //默认密钥8位
            private static string key = "shaohu.f";
            /// <summary>
            /// 加密字符串(使用默认密钥)
            /// </summary>
            /// <param name="encryptString">待加密的字符串</param>
            /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
            public static string EncryptDES(string encryptString)
            {
                return EncryptDES(encryptString, key);
            }
            /// <summary>
            /// 解密字符串(使用默认密钥)
            /// </summary>
            /// <param name="decryptString">待解密的字符串</param>
            /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
            public static string DecryptDES(string decryptString)
            {
                return DecryptDES(decryptString, key);
            }
            /// <summary>
            /// 加密字符串
            /// </summary>
            /// <param name="encryptString">待加密的字符串</param>
            /// <param name="encryptKey">加密密钥,要求为8位</param>
            /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
            public static string EncryptDES(string encryptString, string encryptKey)
            {
                try
                {
                    byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                    byte[] rgbIV = Keys;
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                    DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                        {
                            cStream.Write(inputByteArray, 0, inputByteArray.Length);
                            cStream.FlushFinalBlock();
                            return Convert.ToBase64String(mStream.ToArray());
                        }
                    }
                }
                catch
                {
                    return encryptString;
                }
            }
            /// <summary>
            /// 解密字符串
            /// </summary>
            /// <param name="decryptString">待解密的字符串</param>
            /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
            /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
            public static string DecryptDES(string decryptString, string decryptKey)
            {
                try
                {
                    byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                    byte[] rgbIV = Keys;
                    byte[] inputByteArray = Convert.FromBase64String(decryptString);
                    DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                        {
                            cStream.Write(inputByteArray, 0, inputByteArray.Length);
                            cStream.FlushFinalBlock();
                            return Encoding.UTF8.GetString(mStream.ToArray());
                        }
                    }
                }
                catch
                {
                    return decryptString;
                }
            }
        }
        #endregion
        #region AES加密解密
        /// <summary>
        /// AES加密解密
        /// </summary>
        public class AES
        {
            //默认密钥向量
            private static byte[] Keys = { 0xAB, 0x56, 0x90, 0x34, 0xAB, 0x12, 0x56, 0xCD, 0x78, 0xCD, 0xEF, 0x34, 0x78, 0x12, 0x90, 0xEF };
            //默认密钥16位
            private static string key = "shaohu.fanshaohu";
            /// <summary>
            /// 加密字符串(使用默认密钥)
            /// </summary>
            /// <param name="encryptString">待加密的字符串</param>
            /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
            public static string EncryptAES(string encryptString)
            {
                return EncryptAES(encryptString, key);
            }
            /// <summary>
            /// 解密字符串(使用默认密钥)
            /// </summary>
            /// <param name="decryptString">待解密的字符串</param>
            /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
            public static string DecryptAES(string decryptString)
            {
                return DecryptAES(decryptString, key);
            }
            /// <summary>
            /// 加密字符串
            /// </summary>
            /// <param name="encryptString">待加密的字符串</param>
            /// <param name="encryptKey">加密密钥,要求为16位</param>
            /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
            public static string EncryptAES(string encryptString, string encryptKey)
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                Rijndael aes = Rijndael.Create();
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            /// <summary>
            /// 解密字符串
            /// </summary>
            /// <param name="decryptString">待解密的字符串</param>
            /// <param name="decryptKey">解密密钥,要求为16位,和加密密钥相同</param>
            /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
            public static string DecryptAES(string decryptString, string decryptKey)
            {
                try
                {
                    byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 16));
                    byte[] rgbIV = Keys;
                    byte[] inputByteArray = Convert.FromBase64String(decryptString);
                    Rijndael aes = Rijndael.Create();
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                        {
                            cStream.Write(inputByteArray, 0, inputByteArray.Length);
                            cStream.FlushFinalBlock();
                            return Encoding.UTF8.GetString(mStream.ToArray());
                        }
                    }
                }
                catch
                {
                    return decryptString;
                }
            }
        }
        #endregion
        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        public class md5
        {
            /// <summary>
            /// 加密
            /// </summary>
            /// <param name="encryptString">待加密的字符串</param>
            /// <returns></returns>
            public static string EncryptMD5(string encryptString)
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] data = Encoding.Unicode.GetBytes(encryptString);
                    byte[] hash = md5.ComputeHash(data);
                    return Convert.ToBase64String(hash);
                }
            }
            /// <summary>
            /// 二次加密
            /// </summary>
            /// <param name="encryptString">待加密的字符串</param>
            /// <returns></returns>
            public static string EncryptMD5Second(string encryptString)
            {
                return EncryptMD5(EncryptMD5(encryptString));
            }
            /// <summary>
            /// 二次加密(第一次加密后加上密钥再进行第二次加密)
            /// </summary>
            /// <param name="encryptString">待加密的字符串</param>
            /// <param name="encryptKey">密钥</param>
            /// <returns></returns>
            public static string EncryptMD5Second(string encryptString, string encryptKey)
            {
                return EncryptMD5(EncryptMD5(encryptString) + encryptKey);
            }
        }
        #endregion
    }
}
