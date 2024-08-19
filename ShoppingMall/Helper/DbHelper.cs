using ShoppingMall.Interface;
using StackExchange.Redis;
using System.Data.SqlClient;

namespace ShoppingMall.Helper
{
    public class DbHelper : IDbHelper
    {
        private SqlCommand command;
        private ConnectionMultiplexer redis;
        private IConfigurationsHelper _configurationsHelper;

        public DbHelper(IConfigurationsHelper configurations = null) 
        {
            _configurationsHelper = configurations ?? new ConfigurationsHelper();
        }

        /// <summary>
        /// 取得MSSQL連線
        /// </summary>
        public SqlCommand MsSqlConnection()
        {
            command = new SqlCommand(); //宣告SqlCommand物件
            command.Connection = new SqlConnection(_configurationsHelper.GetMsSqlConnectString()); //設定連線字串

            return command;
        }

        public void SetMsSqlConnection()
        {
            command = new SqlCommand(); //宣告SqlCommand物件
            command.Connection = new SqlConnection(_configurationsHelper.GetMsSqlConnectString()); //設定連線字串            
        }

        /// <summary>
        /// 取得Redis連線
        /// </summary>
        public ConnectionMultiplexer RedisConnection()
        {
            if (redis == null || !redis.IsConnected)
            {
                redis = ConnectionMultiplexer.Connect(_configurationsHelper.GetRedisConnectString());
            }

            return redis;
        }
    }
}
