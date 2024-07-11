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

        public AdminController()
        {
            AdminProccessClass = new AdminProccess();
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
    }
}