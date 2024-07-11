using System;
using System.Web;
using System.Web.Http;
using ShoppingMall.Api.Logout;
using ShoppingMall.Models.Common;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/logout")]
    public class LogoutController : ApiController
    {
        private Logout LogoutClass;

        public LogoutController() {
            LogoutClass = new Logout();
        }

        [Route("logout")]
        [HttpGet]
        public IHttpActionResult LogoutProccess()
        {
            try
            {
                LogoutClass.LogoutProccess();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { StatusErrorCode = "A105"});
            }
        }
    }
}