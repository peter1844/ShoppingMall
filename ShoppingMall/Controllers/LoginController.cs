using ShoppingMall.Api.Login;
using ShoppingMall.Api.Token;
using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Interface;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Enum;
using ShoppingMall.Models.Login;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private ILogin _login;
        private IToken _token;
        private ITools _tools;
        private ILogHelper _logHelper;
        private IContextHelper _contextHelper;

        public LoginController(ILogin login, IToken token, ITools tools, ILogHelper logHelper, IContextHelper contextHelper)
        {
            _login = login;
            _token = token;
            _tools = tools;
            _logHelper = logHelper;
            _contextHelper = contextHelper;
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
                bool inputVaild = _login.CheckInputData(loginData);

                if (inputVaild)
                {
                    List<AdminUserDataDtoResponse> adminUserData = _login.CheckLoginByAccountPassword(loginData);

                    if (adminUserData.Count == 0)
                    {
                        return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildLogin.ToString() });
                    }
                    else
                    {
                        _login.SetLoginAdminPermissions(adminUserData[0].AdminId);

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
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
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
                bool checkResult = _token.CheckLoginByToken();

                if (checkResult)
                {
                    return Ok(checkResult);
                }
                else
                {
                    _contextHelper.ClearContextSession();

                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildToken.ToString() });
                }
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }
    }
}