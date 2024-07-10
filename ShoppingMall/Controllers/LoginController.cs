using ShoppingMall.Api.Login;
using ShoppingMall.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private LoginByAcc loginByAcc;
        private LoginByToken loginByToken;

        public LoginController()
        {
            loginByAcc = new LoginByAcc();
            loginByToken = new LoginByToken();
        }

        [Route("checkLoginByToken")]
        [HttpGet]
        public IHttpActionResult LoginByToken()
        {
            try
            {
                bool checkResult = loginByToken.CheckLoginByToken();

                if (checkResult)
                {
                    return Ok(checkResult);
                }
                else
                {
                    HttpContext context = HttpContext.Current;
                    context.Session.Clear();

                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("checkLoginByAccPwd")]
        [HttpPost]
        public IHttpActionResult LoginByAccountPassword([FromBody] LoginData loginData)
        {
            try
            {
                bool inputVaild = loginByAcc.CheckInputData(loginData);

                if (inputVaild)
                {
                    List<AdminUserData> adminUserData = loginByAcc.TestSp(loginData);
                    //List<AdminUserData> adminUserData = loginByAcc.CheckLoginByAccountPassword(loginData);

                    if (adminUserData.Count == 0)
                    {
                        return Unauthorized();
                    }
                    else
                    {
                        return Ok(adminUserData);
                    }
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}