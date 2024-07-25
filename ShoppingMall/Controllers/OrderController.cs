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
        private OrderProccess orderProccessClass;
        private OrderOption orderOptionClass;

        public OrderController()
        {
            orderProccessClass = new OrderProccess();
            orderOptionClass = new OrderOption();
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
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// 取得訂單管理頁面的選項
        /// </summary>
        /// <returns></returns>
        [Route("getOrderOptionData")]
        [HttpGet]
        public IHttpActionResult GetCommodityOptionData()
        {
            try
            {
                List<OrderOptionDataDtoResponse> orderOptionData = orderOptionClass.GetOrderOptionData();

                return Ok(orderOptionData);
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }
    }
}