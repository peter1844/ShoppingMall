using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Login;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface IDbHelper
    {
        /// <summary>
        /// 取得MSSQL連線
        /// </summary>
        SqlCommand MsSqlConnection();

        /// <summary>
        /// 取得Redis連線
        /// </summary>
        ConnectionMultiplexer RedisConnection();
    }
}
