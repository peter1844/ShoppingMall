using Newtonsoft.Json;
using ShoppingMall.Models.Config;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ShoppingMall.Helper
{
    public static class ConfigurationsHelper
    {
        private static readonly byte[] KEY;
        private static readonly byte[] IV;
        private static readonly string MSSQL_CONNECTION_STRING;
        private static readonly string REDIS_CONNECTION_STRING;

        private static string jsVersion;
        private static string cssVersion;

        static ConfigurationsHelper()
        {
            KEY = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["AesKey"]);
            IV = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["AesIv"]);
            MSSQL_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
            REDIS_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["MyRedisConn"].ConnectionString;

            LoadVersion();
        }

        /// <summary>
        /// 初始化版本號
        /// </summary>
        public static void LoadVersion()
        {
            try
            {
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/Version.json";
                string jsonContent = File.ReadAllText(filePath);
                VersionData config = JsonConvert.DeserializeObject<VersionData>(jsonContent);

                jsVersion = config.JsVersion;
                cssVersion = config.CssVersion;
            }
            catch (Exception ex)
            {
                LogHelper.Warn(ex.Message);
            }
        }

        /// <summary>
        /// 取得AES加密的Key
        /// </summary>
        public static byte[] GetKey() 
        {
            return KEY;
        }

        /// <summary>
        /// 取得AES加密的Iv
        /// </summary>
        public static byte[] GetIv()
        {
            return IV;
        }

        /// <summary>
        /// 取得MsSql的連線字串
        /// </summary>
        public static string GetMsSqlConnectString()
        {
            return MSSQL_CONNECTION_STRING;
        }

        /// <summary>
        /// 取得Redis的連線字串
        /// </summary>
        public static string GetRedisConnectString()
        {
            return REDIS_CONNECTION_STRING;
        }

        /// <summary>
        /// 取得Js的版本號
        /// </summary>
        public static string GetJsVersion()
        {
            return jsVersion;
        }

        /// <summary>
        /// 取得Css的版本號
        /// </summary>
        public static string GetCssVersion()
        {
            return cssVersion;
        }
    }
}
