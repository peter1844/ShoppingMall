using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Login;
using ShoppingMall.Models.Member;
using System;
using System.Collections.Generic;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface IMenu
    {
        /// <summary>
        /// 設定語系
        /// </summary>
        void SetLanguage(string language);
    }
}
