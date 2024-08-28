using ShoppingMall.Api.Logout;
using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Interface;
using ShoppingMall.Models.Common;
using System;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/logout")]
    public class LogoutController : ApiController
    {
        private ILogout _logout;
        private ITools _tools;
        private ILogHelper _logHelper;

        public LogoutController(ILogout logout, ITools tools, ILogHelper logHelper)
        {
            _logout = logout;
            _tools = tools;
            _logHelper = logHelper;
        }

        /// <summary>
        /// 登出功能
        /// </summary>
        /// <returns></returns>
        [Route("logout")]
        [HttpGet]
        public IHttpActionResult LogoutProccess()
        {
            try
            {
                _logout.LogoutProccess();

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