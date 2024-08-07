using ShoppingMall.Runtime;
using System;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace ShoppingMall
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // 監聽檔案變化
            VersionListner.Initialize();

            // 定期執行刪除訂單
            DeleteOrder.DeleteOrderTimer();
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }
    }
}
