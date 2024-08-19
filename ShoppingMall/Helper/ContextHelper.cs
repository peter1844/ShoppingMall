using NLog;
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
    }
}
