using ShoppingMall.Api.Login;
using ShoppingMall.Helper;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Login;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using ShoppingMall.App_Code;
using Newtonsoft.Json;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private LoginByAcc loginByAccClass;
        private LoginByToken loginByTokenClass;

        public LoginController()
        {
            loginByAccClass = new LoginByAcc();
            loginByTokenClass = new LoginByToken();
        }

        /// <summary>
        /// 用帳號密碼檢查登入
        /// </summary>
        [Route("checkLoginByAccPwd")]
        [HttpPost]
        public IHttpActionResult LoginByAccountPassword([FromBody] LoginDataDto loginData)
        {
            try
            {
                //LogHelper.logger.Info(JsonConvert.SerializeObject(loginData));

                bool inputVaild = loginByAccClass.CheckInputData(loginData);

                if (inputVaild)
                {
                    List<AdminUserDataDtoResponse> adminUserData = loginByAccClass.CheckLoginByAccountPassword(loginData);

                    if (adminUserData.Count == 0)
                    {
                        return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildLogin.ToString() });
                    }
                    else
                    {
                        loginByAccClass.SetLoginAdminPermissions(adminUserData[0].AdminId);

                        return Ok(adminUserData);
                    }
                }
                else
                {
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
            }
        }

        /// <summary>
        /// 用Token檢查登入
        /// </summary>
        [Route("checkLoginByToken")]
        [HttpGet]
        public IHttpActionResult LoginByToken()
        {
            try
            {
                bool checkResult = loginByTokenClass.CheckLoginByToken();

                if (checkResult)
                {
                    return Ok(checkResult);
                }
                else
                {
                    HttpContext context = HttpContext.Current;
                    context.Session.Clear();

                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildToken.ToString() });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
            }
        }
    }
}