﻿using System.Web;

namespace ShoppingMall.Api.Logout
{
    public class Logout : ShoppingMall.Base.Base
    {
        public void LogoutProccess() {
            
            HttpContext context = HttpContext.Current;

            string sessionToken = AesDecrypt((string)context.Session["token"]);
            string[] realTokenData = sessionToken.Split(',');

            context.Session.Clear();
            RedisConnection().GetDatabase().KeyDelete($"{realTokenData[0]}_token");
        }
    }
}
