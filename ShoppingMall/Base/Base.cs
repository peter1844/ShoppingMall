﻿using StackExchange.Redis;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingMall.Base
{
    public class Base
    {
        public ConnectionMultiplexer Redis;
        private readonly SqlConnection SQLCONNECTION;
        private readonly byte[] KEY;
        private readonly byte[] IV;

        public Base()
        {
            SQLCONNECTION = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString);

            KEY = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["AesKey"]);
            IV = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["AesIv"]);
        }
        public ConnectionMultiplexer RedisConnection()
        {
            if (Redis == null || !Redis.IsConnected)
            {
                Redis = ConnectionMultiplexer.Connect(ConfigurationManager.ConnectionStrings["MyRedisConn"].ConnectionString);
            }

            return Redis;
        }
        public DataTable ExcuteQueryBySp(string acc, string pwd) 
        {
            SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
            cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString); //設定連線字串
            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件

            cmd.CommandText = "EXEC pro_bkg_getLoginData @acc,@pwd";
            cmd.Parameters.AddWithValue("@acc", acc);
            cmd.Parameters.AddWithValue("@pwd", pwd);
            cmd.Connection.Open(); //開啟資料庫連線

            da.SelectCommand = cmd; //執行
            da.Fill(dt); //結果存放至DataTable

            cmd.Connection.Close(); //關閉連線

            //int a = dt.Rows.Count;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    //這邊可以開始處理取回來的操作
            //    int aa = Convert.ToInt32(dt.Rows[i]["f_id"]);
            //}
            //string bb = dt.Rows[0]["f_name"].ToString();
            return dt;
        }

        public SqlDataReader ExecuteQuery(string sql)
        {
            SQLCONNECTION.Open();
            SqlCommand command = new SqlCommand(sql, SQLCONNECTION);
            SqlDataReader reader = command.ExecuteReader();

            return reader;
        }
        public int ExecuteInsert(string sql)
        {
            sql += "SELECT SCOPE_IDENTITY();";

            SQLCONNECTION.Open();
            SqlCommand command = new SqlCommand(sql, SQLCONNECTION);
            int newId = Convert.ToInt32(command.ExecuteScalar());

            return newId;
        }
        public int ExecuteUpdate(string sql)
        {
            SQLCONNECTION.Open();
            SqlCommand command = new SqlCommand(sql, SQLCONNECTION);
            int result = command.ExecuteNonQuery();

            return result;
        }
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
        public string GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];

            // 使用 RNGCryptoServiceProvider 生成随机字节
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            string randomBase64String = Convert.ToBase64String(randomBytes);
            return randomBase64String;
        }
    }
}
