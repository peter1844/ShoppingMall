using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using System.Web;

namespace ShoppingMall.Api.Logout
{
    public class Logout
    {
        /// <summary>
        /// 登出功能
        /// </summary>
        public void LogoutProccess()
        {

            HttpContext context = HttpContext.Current;

            string sessionToken = Tools.AesDecrypt((string)context.Session["token"]);
            string[] realTokenData = sessionToken.Split(',');

            context.Session.Clear();
            DbHelper.RedisConnection().GetDatabase().KeyDelete($"{realTokenData[0]}_token");
        }
    }
}
