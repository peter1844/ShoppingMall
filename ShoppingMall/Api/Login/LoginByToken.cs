using System.Web;

namespace ShoppingMall.Api.Login
{
    public class LoginByToken : ShoppingMall.Base.Base
    {
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
            string decryptToken = AesDecrypt(token);
            string[] tokenData = decryptToken.Split(',');

            // 檢查AES解密後的資料是否合法
            if (tokenData.Length != 2) return false;

            bool isVaild = RedisConnection().GetDatabase().StringGet($"{tokenData[0]}_token") == token ? true : false;

            return isVaild;
        }
    }
}
