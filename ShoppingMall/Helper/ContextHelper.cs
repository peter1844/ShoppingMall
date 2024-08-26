using ShoppingMall.Interface;
using System.Web;

namespace ShoppingMall.Helper
{
    public class ContextHelper : IContextHelper
    {
        public HttpContextBase GetContext()
        {
            return new HttpContextWrapper(HttpContext.Current);
        }

        public HttpRequestBase GetHttpRequest()
        {
            return new HttpRequestWrapper(HttpContext.Current.Request);
        }

        public void ClearContextSession()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
