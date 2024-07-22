using ShoppingMall.Api.Admin;
using ShoppingMall.Api.Commodity;
using ShoppingMall.App_Code;
using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Order;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        private OrderOption orderOptionClass;

        public OrderController()
        {
            orderOptionClass = new OrderOption();
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