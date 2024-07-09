using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using ShoppingMall.Api.Login;
using ShoppingMall.Models;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/logout")]
    public class LogoutController : ApiController
    {
        [Route("logout")]
        [HttpGet]
        public IHttpActionResult LogoutProccess()
        {
            try
            {
                HttpContext context = HttpContext.Current;
                context.Session.Clear();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}