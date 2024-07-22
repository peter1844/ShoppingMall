﻿using ShoppingMall.Api.Logout;
using ShoppingMall.Models.Common;
using System;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/logout")]
    public class LogoutController : ApiController
    {
        private Logout logoutClass;

        public LogoutController() {
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
                return Ok(new ExceptionData { ErrorMessage = ex.Message});
            }
        }
    }
}