using ShoppingMall.Api.Order;
using ShoppingMall.App_Code;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Order;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        private OrderPermissions orderPermissionsClass;
        private OrderProccess orderProccessClass;
        private OrderOption orderOptionClass;
        private Base.Base baseClass;

        public OrderController()
        {
            orderPermissionsClass = new OrderPermissions();
            orderProccessClass = new OrderProccess();
            orderOptionClass = new OrderOption();
            baseClass = new Base.Base();
        }

        /// <summary>
        /// 取得訂單頁面權限
        /// </summary>
        [Route("getOrderPermissions")]
        [HttpGet]
        public IHttpActionResult getOrderPermissions()
        {
            try
            {
                List<OrderPermissionsDtoResponse> orderPermissions = orderPermissionsClass.GetAllOrderPermissions();

                return Ok(orderPermissions);
            }
            catch (Exception ex)
            {
                Base.Base.Logger(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// 取得訂單資料
        /// </summary>
        [Route("getOrderData")]
        [HttpGet]
        public IHttpActionResult GetOrderData()
        {
            try
            {
                List<ConditionDataDto> conditionData = new List<ConditionDataDto>();
                HttpRequest request = HttpContext.Current.Request;

                conditionData.Add(new ConditionDataDto
                {
                    Id = string.IsNullOrEmpty(request.QueryString["Id"]) ? "" : request.QueryString["Id"],
                    StartDate = string.IsNullOrEmpty(request.QueryString["StartDate"]) ? (DateTime?)null : Convert.ToDateTime(request.QueryString["StartDate"]),
                    EndDate = string.IsNullOrEmpty(request.QueryString["EndDate"]) ? (DateTime?)null : Convert.ToDateTime(request.QueryString["EndDate"]),
                    DeliveryState = string.IsNullOrEmpty(request.QueryString["DeliveryState"]) ? -1 : Convert.ToInt32(request.QueryString["DeliveryState"])
                });

                bool inputVaild = orderProccessClass.CheckConditionInputData(conditionData[0]);

                if (!inputVaild)
                {
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
                
                List<OrderDataDtoResponse> orderData = orderProccessClass.GetOrderData(conditionData[0]);

                return Ok(orderData);
            }
            catch (Exception ex)
            {
                Base.Base.Logger(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// 取得訂單管理頁面的選項
        /// </summary>
        /// <returns></returns>
        [Route("getOrderOptionData")]
        [HttpGet]
        public IHttpActionResult GetOrderOptionData()
        {
            try
            {
                List<OrderOptionDataDtoResponse> orderOptionData = orderOptionClass.GetOrderOptionData();

                return Ok(orderOptionData);
            }
            catch (Exception ex)
            {
                Base.Base.Logger(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// 新增訂單資料
        /// </summary>
        [Route("insertOrderData")]
        [HttpPost]
        public IHttpActionResult InsertOrderData([FromBody] InsertOrderDataDto insertData)
        {
            try
            {
                // 檢查權限
                if (!baseClass.CheckPermission((int)Permissions.OrderInsert)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = orderProccessClass.CheckInsertInputData(insertData);

                if (inputVaild)
                {
                    bool result = orderProccessClass.InsertOrderData(insertData);

                    return Ok(result);
                }
                else
                {
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
            }
            catch (Exception ex)
            {
                Base.Base.Logger(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// 編輯訂單資料
        /// </summary>
        [Route("updateOrderData")]
        [HttpPut]
        public IHttpActionResult UpdateOrderData([FromBody] UpdateOrderDataDto updateData)
        {
            try
            {
                // 檢查權限
                if (!baseClass.CheckPermission((int)Permissions.OrderUpdate)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = orderProccessClass.CheckUpdateInputData(updateData);

                if (inputVaild)
                {
                    bool result = orderProccessClass.UpdateOrderData(updateData);

                    return Ok(result);
                }
                else
                {
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
            }
            catch (Exception ex)
            {
                Base.Base.Logger(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }
    }
}