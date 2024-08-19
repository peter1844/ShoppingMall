using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Login;
using System.Collections.Generic;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface IConfigurationsHelper
    {
        /// <summary>
        /// 初始化版本號
        /// </summary>
        void LoadVersion();

        /// <summary>
        /// 取得AES加密的Key
        /// </summary>
        byte[] GetKey();

        /// <summary>
        /// 取得AES加密的Iv
        /// </summary>
        byte[] GetIv();

        /// <summary>
        /// 取得MsSql的連線字串
        /// </summary>
        string GetMsSqlConnectString();

        /// <summary>
        /// 取得Redis的連線字串
        /// </summary>
        string GetRedisConnectString();

        /// <summary>
        /// 取得Js的版本號
        /// </summary>
        string GetJsVersion();

        /// <summary>
        /// 取得Css的版本號
        /// </summary>
        string GetCssVersion();
    }
}
