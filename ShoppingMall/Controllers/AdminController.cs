using ShoppingMall.Api.Admin;
using ShoppingMall.Models.Admin;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using ShoppingMall.Models.Common;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        private AdminProccess AdminProccessClass;
        private AdminOption AdminOptionClass;

        public AdminController()
        {
            AdminProccessClass = new AdminProccess();
            AdminOptionClass = new AdminOption();
        }

        [Route("getAdminData")]
        [HttpGet]
        public IHttpActionResult GetAdminData()
        {
            try
            {
                List<AdminUserDataDtoResponse> adminUserData = AdminProccessClass.GetAllAdminUserData();

                return Ok(adminUserData);
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { StatusErrorCode = ex.Message });
            }
        }
        [Route("getAdminOptionData")]
        [HttpGet]
        public IHttpActionResult GetAdminOptionData()
        {
            try
            {
                List<AdminOptionDataDtoResponse> adminOptionData = AdminOptionClass.GetAllAdminOptionData();

                return Ok(adminOptionData);
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { StatusErrorCode = ex.Message });
            }
        }
        [Route("insertAdminData")]
        [HttpPost]
        public IHttpActionResult InsertAdminData([FromBody] InsertAdminDataDto insertData)
        {
            try
            {
                bool inputVaild = AdminProccessClass.CheckInputData(insertData);

                if (inputVaild)
                {
                    int adminUserData = AdminProccessClass.InsertAdminData(insertData);

                    return Ok(adminUserData);
                    
                }
                else
                {
                    return Ok(new ExceptionData { StatusErrorCode = "A101" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { StatusErrorCode = ex.Message });
            }
        }
    }
}