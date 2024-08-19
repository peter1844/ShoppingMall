using ShoppingMall.Api.Admin;
using ShoppingMall.Api.Menu;
using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Interface;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Menu;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/menu")]
    public class MenuController : ApiController
    {
        private IMenu _menu;
        private ITools _tools;
        private ILogHelper _logHelper;

        public MenuController()
        {
            _menu = new MenuProccess();
            _tools = new Tools();
            _logHelper = new LogHelper();
        }

        public MenuController(IMenu menu, ITools tools, ILogHelper logHelper)
        {
            _menu = menu;
            _tools = tools;
            _logHelper = logHelper;
        }

        /// <summary>
        /// 設定語系
        /// </summary>
        [Route("setLanguage")]
        [HttpPost]
        public IHttpActionResult setLanguage([FromBody] MenuLanguageDto languageData)
        {
            try
            {
                _menu.SetLanguage(languageData.Language);

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }
    }
}