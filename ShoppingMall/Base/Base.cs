using NLog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ShoppingMall.Base
{
    public class Base
    {
        private SqlCommand Command;
        private ConnectionMultiplexer Redis;
        private readonly byte[] KEY;
        private readonly byte[] IV;

        public Base()
        {
            KEY = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["AesKey"]);
            IV = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["AesIv"]);
        }

        /// <summary>
        /// 取得Redis連線
        /// </summary>
        public ConnectionMultiplexer RedisConnection()
        {
            if (Redis == null || !Redis.IsConnected)
            {
                Redis = ConnectionMultiplexer.Connect(ConfigurationManager.ConnectionStrings["MyRedisConn"].ConnectionString);
            }

            return Redis;
        }

        /// <summary>
        /// 取得MSSQL連線
        /// </summary>
        public SqlCommand MsSqlConnection() 
        {
            Command = new SqlCommand(); //宣告SqlCommand物件
            Command.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString); //設定連線字串

            return Command;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        public string AesEncrypt(string encryptData)
        {
            byte[] encrypted;

            // 创建一个 AES 实例
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = KEY;
                aesAlg.IV = IV;

                // 创建加密器
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // 创建内存流来存储加密后的数据
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // 创建加密流
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        // 将数据写入加密流
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(encryptData);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        public string AesDecrypt(string decryptData)
        {
            byte[] cipherText = Convert.FromBase64String(decryptData);
            string plainText = null;

            // 创建一个 AES 实例
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = KEY;
                aesAlg.IV = IV;

                // 创建解密器
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // 创建内存流来存储解密后的数据
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    // 创建加密流
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        // 从加密流中读取数据
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plainText;
        }

        /// <summary>
        /// 隨機產生N位數的字串(Base64)
        /// </summary>
        public string GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            string randomBase64String = Convert.ToBase64String(randomBytes);
            return randomBase64String;
        }

        /// <summary>
        /// 隨機產生N位數的字串
        /// </summary>
        public string GenerateRandomString(int length)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                char randomChar = chars[index];

                sb.Append(randomChar);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 檢查是否有權限執行
        /// </summary>
        /// <returns></returns>
        public bool CheckPermission(int permission)
        {
            HttpContext context = HttpContext.Current;

            if (context.Session["permissions"] == null) return false;
            string[] allPermissions = context.Session["permissions"].ToString().Split(',');

            return allPermissions.Contains(permission.ToString());
        }

        /// <summary>
        /// 寫入log
        /// </summary>
        public void Logger(string message) { 
            Logger log = LogManager.GetCurrentClassLogger();

            log.Info(message);
        }
    }
}
