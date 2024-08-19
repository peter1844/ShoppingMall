using ShoppingMall.App_Code;
using ShoppingMall.Models.Menu;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;
using ShoppingMall.Interface;
using System.Runtime.Remoting.Contexts;
using ShoppingMall.Helper;

namespace ShoppingMall.Api.Menu
{
    public class MenuProccess : IMenu
    {
        private IContextHelper _contextHelper;

        public MenuProccess(IContextHelper contextHelper = null)
        {
            _contextHelper = contextHelper ?? new ContextHelper();
        }

        /// <summary>
        /// 設定語系
        /// </summary>
        public void SetLanguage(string language)
        {
            try
            {
                _contextHelper.GetContext().Session["lang"] = language;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
