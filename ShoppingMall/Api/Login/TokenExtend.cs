using Newtonsoft.Json.Linq;
using System;
using System.Web;

namespace ShoppingMall.Api.Login
{
    public class TokenExtend : ShoppingMall.Base.Base
    {
        /// <summary>
        /// token驗證通過時延長時間
        /// </summary>
        public void ExtendRedisLoginToken(string token)
        {
            string decryptToken = AesDecrypt(token);
            string[] tokenData = decryptToken.Split(',');

            RedisConnection().GetDatabase().KeyExpire($"{tokenData[0]}_token", TimeSpan.FromMinutes(20));
        }
    }
}
