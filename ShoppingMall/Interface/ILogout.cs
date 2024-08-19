using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Commodity;
using System.Collections.Generic;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface ILogout
    {
        /// <summary>
        /// 登出功能
        /// </summary>
        void LogoutProccess();
    }
}
