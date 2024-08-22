using StackExchange.Redis;
using System.Data.SqlClient;

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
