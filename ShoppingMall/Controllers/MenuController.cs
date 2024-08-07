using ShoppingMall.Api.Menu;
using ShoppingMall.Helper;
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
        private MenuPermissions menuPermissionsClass;

        public MenuController()
        {
            menuPermissionsClass = new MenuPermissions();
        }

        /// <summary>
        /// 取得菜單頁面權限
        /// </summary>
        [Route("getMenuPermissions")]
        [HttpGet]
        public IHttpActionResult getMenuPermissions()
        {
            try
            {
                List<MenuPermissionsDtoResponse> menuPermissions = menuPermissionsClass.GetAllMenuPermissions();

                return Ok(menuPermissions);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
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
                HttpContext context = HttpContext.Current;

                context.Session["lang"] = languageData.Language;

                return Ok(true);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }
    }
}