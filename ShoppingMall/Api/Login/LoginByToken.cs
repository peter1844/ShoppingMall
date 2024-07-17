using System.Web;

namespace ShoppingMall.Api.Login
{
    public class LoginByToken : ShoppingMall.Base.Base
    {
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

        public bool IsValidToken(string token)
        {
            string decryptToken = AesDecrypt(token);
            string[] tokenData = decryptToken.Split(',');

            if (tokenData.Length != 2) return false;

            bool isVaild = RedisConnection().GetDatabase().StringGet($"{tokenData[0]}_token") == token ? true : false;

            return isVaild;
        }
    }
}
