using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShoppingMall.Api.Login;
using ShoppingMall.Models;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        [Route("checkLoginByToken")]
        [HttpGet]
        public IHttpActionResult test()
        {

            return Ok(1);
        }
    }
}