using ShoppingMall.Interface;
using ShoppingMall.Runtime;
using System;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace ShoppingMall
{
    public class WebApiApplication : HttpApplication
    {
        private IVersionListner _versionListner;
        private IDeleteOrder _deleteOrder;

        public WebApiApplication()
        {
            _versionListner = new VersionListner();
            _deleteOrder = new DeleteOrder();
        }

        public WebApiApplication(IVersionListner versionListner, IDeleteOrder deleteOrder)
        {
            _versionListner = versionListner;
            _deleteOrder = deleteOrder;
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // 監聽檔案變化
            _versionListner.Initialize();

            // 定期執行刪除訂單
            _deleteOrder.DeleteOrderTimer();
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

        protected void Application_End(object sender, EventArgs e)
        {
            // 釋放版本監聽
            _versionListner.Dispose();
        }
    }
}
