using ShoppingMall.Api.Login;
using ShoppingMall.Models.Login;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using ShoppingMall.Models.Common;

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

                    return Ok(new ExceptionData { StatusErrorCode = "A103"});
                }
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { StatusErrorCode = "A104"});
            }
        }

        [Route("checkLoginByAccPwd")]
        [HttpPost]
        public IHttpActionResult LoginByAccountPassword([FromBody] LoginDataDto loginData)
        {
            try
            {
                bool inputVaild = loginByAcc.CheckInputData(loginData);
                
                if (inputVaild)
                {
                    List<AdminUserDataDtoResponse> adminUserData = loginByAcc.CheckLoginByAccountPassword(loginData);

                    if (adminUserData.Count == 0)
                    {
                        return Ok(new ExceptionData { StatusErrorCode = "A100"});
                    }
                    else
                    {
                        return Ok(adminUserData);
                    }
                }
                else
                {
                    return Ok(new ExceptionData { StatusErrorCode = "A101"});
                }
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { StatusErrorCode = ex.Message});
            }
        }
    }
}