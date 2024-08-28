using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Interface;
using System;
using System.Web;

namespace ShoppingMall.Api.Token
{
    public class TokenProccess : IToken
    {
        private IDbHelper _dbHelper;
        private ITools _tools;

        public TokenProccess(IDbHelper dbHelper, ITools tools)
        {
            _dbHelper = dbHelper;
            _tools = tools;
        }

        /// <summary>
        /// 用Token檢查登入
        /// </summary>
        public bool CheckLoginByToken()
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers["token"];

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            else
            {
                return IsValidToken(token);
            }
        }

        /// <summary>
        /// 檢查是否為有效Token
        /// </summary>
        public bool IsValidToken(string token)
        {
            string decryptToken = _tools.AesDecrypt(token);
            string[] tokenData = decryptToken.Split(',');

            // 檢查AES解密後的資料是否合法
            if (tokenData.Length != 2) return false;

            bool isVaild = _dbHelper.RedisConnection().GetDatabase().StringGet($"{tokenData[0]}_token") == token ? true : false;

            return isVaild;
        }

        /// <summary>
        /// token驗證通過時延長時間
        /// </summary>
        public void ExtendRedisLoginToken(string token)
        {
            string decryptToken = _tools.AesDecrypt(token);
            string[] tokenData = decryptToken.Split(',');

            _dbHelper.RedisConnection().GetDatabase().KeyExpire($"{tokenData[0]}_token", TimeSpan.FromMinutes(20));
        }
    }
}
