using ShoppingMall.Api.Order;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Order;
using System;
using System.Collections.Generic;
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
                List<OrderDataDtoResponse> orderData = orderProccessClass.GetOrderData();

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