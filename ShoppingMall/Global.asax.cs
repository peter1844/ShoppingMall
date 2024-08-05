using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;
using ShoppingMall.Api.Order;

namespace ShoppingMall
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {            
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // 定期執行刪除訂單
            Timer timer = new Timer(60000);

            timer.Elapsed += (s, ea) =>
            {
                try
                {
                    OrderProccess.DeleteOrderData();
                }
                catch (Exception ex)
                {
                    Base.Base.Logger(ex.Message);
                }
            };

            timer.Start();
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
