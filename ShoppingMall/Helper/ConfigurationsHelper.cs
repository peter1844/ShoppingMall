using Newtonsoft.Json;
using ShoppingMall.Interface;
using ShoppingMall.Models.Config;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ShoppingMall.Helper
{
    public class ConfigurationsHelper : IConfigurationsHelper
    {
        private byte[] KEY;
        private byte[] IV;
        private string MSSQL_CONNECTION_STRING;
        private string REDIS_CONNECTION_STRING;

        private string jsVersion;
        private string cssVersion;

        private ILogHelper _logHelper;

        public ConfigurationsHelper(ILogHelper logHelper)
        {
            _logHelper = logHelper;

            KEY = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["AesKey"]);
            IV = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["AesIv"]);
            MSSQL_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
            REDIS_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["MyRedisConn"].ConnectionString;

            LoadVersion();
        }

        /// <summary>
        /// 初始化版本號
        /// </summary>
        public void LoadVersion()
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
                _logHelper.Error(ex.Message);
            }
        }

        /// <summary>
        /// 取得AES加密的Key
        /// </summary>
        public byte[] GetKey()
        {
            return KEY;
        }

        /// <summary>
        /// 取得AES加密的Iv
        /// </summary>
        public byte[] GetIv()
        {
            return IV;
        }

        /// <summary>
        /// 取得MsSql的連線字串
        /// </summary>
        public string GetMsSqlConnectString()
        {
            return MSSQL_CONNECTION_STRING;
        }

        /// <summary>
        /// 取得Redis的連線字串
        /// </summary>
        public string GetRedisConnectString()
        {
            return REDIS_CONNECTION_STRING;
        }

        /// <summary>
        /// 取得Js的版本號
        /// </summary>
        public string GetJsVersion()
        {
            return jsVersion;
        }

        /// <summary>
        /// 取得Css的版本號
        /// </summary>
        public string GetCssVersion()
        {
            return cssVersion;
        }
    }
}
