using ShoppingMall.Api.Menu;
using ShoppingMall.App_Code;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Member;
using ShoppingMall.Models.Menu;
using System;
using System.Collections.Generic;
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
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }
    }
}