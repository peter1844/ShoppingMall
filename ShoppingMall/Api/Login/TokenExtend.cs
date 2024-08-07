using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using System;

namespace ShoppingMall.Api.Login
{
    public class TokenExtend
    {
        /// <summary>
        /// token驗證通過時延長時間
        /// </summary>
        public void ExtendRedisLoginToken(string token)
        {
            string decryptToken = Tools.AesDecrypt(token);
            string[] tokenData = decryptToken.Split(',');

            DbHelper.RedisConnection().GetDatabase().KeyExpire($"{tokenData[0]}_token", TimeSpan.FromMinutes(20));
        }
    }
}
