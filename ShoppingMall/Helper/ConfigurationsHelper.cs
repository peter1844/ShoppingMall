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
        public static readonly byte[] KEY;
        public static readonly byte[] IV;
        public static readonly string MSSQL_CONNECTION_STRING;
        public static readonly string REDIS_CONNECTION_STRING;

        public static string jsVersion;
        public static string cssVersion;

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
                LogHelper.logger.Warn(ex.Message);
            }
        }
    }
}
