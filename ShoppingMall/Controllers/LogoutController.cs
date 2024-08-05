using ShoppingMall.Api.Logout;
using ShoppingMall.Models.Common;
using System;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/logout")]
    public class LogoutController : ApiController
    {
        private Base.Base baseClass;
        private Logout logoutClass;

        public LogoutController() {
            baseClass = new Base.Base();
            logoutClass = new Logout();
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
                logoutClass.LogoutProccess();

                return Ok(true);
            }
            catch (Exception ex)
            {
                Base.Base.Logger(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message});
            }
        }
    }
}