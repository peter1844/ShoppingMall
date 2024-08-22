using System.Web;

namespace ShoppingMall.Interface
{
    public interface IContextHelper
    {
        HttpContextBase GetContext();
        void ClearContextSession();
    }
}
