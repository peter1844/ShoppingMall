using ShoppingMall.Api.Admin;
using ShoppingMall.Api.Commodity;
using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Common;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/commodity")]
    public class CommodityController : ApiController
    {
        private AdminProccess AdminProccessClass;
        private AdminOption AdminOptionClass;
        private CommodityOption CommodityOptionClass;

        public CommodityController()
        {
            AdminProccessClass = new AdminProccess();
            AdminOptionClass = new AdminOption();
            CommodityOptionClass = new CommodityOption();
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
        [Route("getCommodityOptionData")]
        [HttpGet]
        public IHttpActionResult GetCommodityOptionData()
        {
            try
            {
                List<CommodityOptionDataDtoResponse> adminOptionData = CommodityOptionClass.GetAllCommodityOptionData();

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
    }
}