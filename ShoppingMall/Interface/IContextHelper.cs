using System.Web;

namespace ShoppingMall.Interface
{
    public interface IContextHelper
    {
        HttpContextBase GetContext();
        HttpRequestBase GetHttpRequest();
        void ClearContextSession();
    }
}
