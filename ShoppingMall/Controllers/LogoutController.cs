﻿using System;
using System.Web;
using System.Web.Http;
using ShoppingMall.Api.Logout;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/logout")]
    public class LogoutController : ApiController
    {
        private Logout logoutClass;

        public LogoutController() {
            logoutClass = new Logout();
        }

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
                return InternalServerError(ex);
            }
        }
    }
}