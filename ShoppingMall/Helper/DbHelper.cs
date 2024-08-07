using StackExchange.Redis;
using System.Data.SqlClient;

namespace ShoppingMall.Helper
{
    public static class DbHelper
    {
        private static SqlCommand command;
        private static ConnectionMultiplexer redis;

        /// <summary>
        /// 取得MSSQL連線
        /// </summary>
        public static SqlCommand MsSqlConnection()
        {
            command = new SqlCommand(); //宣告SqlCommand物件
            command.Connection = new SqlConnection(ConfigurationsHelper.MSSQL_CONNECTION_STRING); //設定連線字串

            return command;
        }

        /// <summary>
        /// 取得Redis連線
        /// </summary>
        public static ConnectionMultiplexer RedisConnection()
        {
            if (redis == null || !redis.IsConnected)
            {
                redis = ConnectionMultiplexer.Connect(ConfigurationsHelper.REDIS_CONNECTION_STRING);
            }

            return redis;
        }
    }
}
