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
                bool inputVaild = AdminProccessClass.CheckInsertInputData(insertData);

                if (inputVaild)
                {
                    bool result = AdminProccessClass.InsertAdminData(insertData);

                    return Ok(result);
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
        [Route("updateAdminData")]
        [HttpPut]
        public IHttpActionResult UpdateAdminData([FromBody] UpdateAdminDataDto updateData)
        {
            try
            {
                bool inputVaild = AdminProccessClass.CheckUpdateInputData(updateData);

                if (inputVaild)
                {
                    bool result = AdminProccessClass.UpdateAdminData(updateData);

                    return Ok(result);
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
        [Route("deleteAdminData")]
        [HttpDelete]
        public IHttpActionResult DeleteAdminData([FromBody] DeleteAdminDataDto deleteData)
        {
            try
            {
                bool inputVaild = AdminProccessClass.CheckDeleteInputData(deleteData);

                if (inputVaild)
                {
                    bool result = AdminProccessClass.DeleteAdminData(deleteData);

                    return Ok(result);
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