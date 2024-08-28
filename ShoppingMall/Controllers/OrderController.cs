using Newtonsoft.Json;
using ShoppingMall.Api.Order;
using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Interface;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Enum;
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
        private IOrder _order;
        private ITools _tools;
        private ILogHelper _logHelper;

        public OrderController(IOrder order, ITools tools, ILogHelper logHelper)
        {
            _order = order;
            _tools = tools;
            _logHelper = logHelper;
        }

        /// <summary>
        /// 取得訂單資料
        /// </summary>
        [Route("getOrderData")]
        [HttpGet]
        public IHttpActionResult GetOrderData([FromUri] OrderConditionDataDto searchData)
        {
            try
            {
                List<OrderConditionDataDto> conditionData = new List<OrderConditionDataDto>();

                if (searchData == null)
                {
                    conditionData.Add(new OrderConditionDataDto
                    {
                        Id = "",
                        StartDate = (DateTime?)null,
                        EndDate = (DateTime?)null,
                        DeliveryState = -1
                    });
                }
                else
                {
                    conditionData.Add(new OrderConditionDataDto
                    {
                        Id = searchData.Id == null ? "" : searchData.Id,
                        StartDate = searchData.StartDate == null ? (DateTime?)null : Convert.ToDateTime(searchData.StartDate),
                        EndDate = searchData.EndDate == null ? (DateTime?)null : Convert.ToDateTime(searchData.EndDate),
                        DeliveryState = searchData.DeliveryState == null ? -1 : searchData.DeliveryState
                    });
                }

                _logHelper.Info(JsonConvert.SerializeObject(conditionData));

                bool inputVaild = _order.CheckConditionInputData(conditionData[0]);

                if (!inputVaild)
                {
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }

                List<OrderDataDtoResponse> orderData = _order.GetOrderData(conditionData[0]);

                return Ok(orderData);
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
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
                List<OrderOptionDataDtoResponse> orderOptionData = _order.GetOrderOptionData();

                return Ok(orderOptionData);
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
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
                _logHelper.Info(JsonConvert.SerializeObject(insertData));

                // 檢查權限
                if (!_tools.CheckPermission((int)Permissions.OrderInsert)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = _order.CheckInsertInputData(insertData);

                if (inputVaild)
                {
                    bool result = _order.InsertOrderData(insertData);

                    return Ok(result);
                }
                else
                {
                    _logHelper.Warn(JsonConvert.SerializeObject(insertData));
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
        /// 編輯訂單資料
        /// </summary>
        [Route("updateOrderData")]
        [HttpPut]
        public IHttpActionResult UpdateOrderData([FromBody] UpdateOrderDataDto updateData)
        {
            try
            {
                _logHelper.Info(JsonConvert.SerializeObject(updateData));

                // 檢查權限
                if (!_tools.CheckPermission((int)Permissions.OrderUpdate)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = _order.CheckUpdateInputData(updateData);

                if (inputVaild)
                {
                    bool result = _order.UpdateOrderData(updateData);

                    return Ok(result);
                }
                else
                {
                    _logHelper.Warn(JsonConvert.SerializeObject(updateData));
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
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