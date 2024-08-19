using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Login;
using System.Collections.Generic;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface IContextHelper
    {
        HttpContextBase GetContext();
    }
}
