using ShoppingMall.App_Code;
using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Order;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Commodity
{
    public class OrderOption : ShoppingMall.Base.Base
    {
        /// <summary>
        /// 取得訂單管理頁面所需的選項
        /// </summary>
        public List<OrderOptionDataDtoResponse> GetOrderOptionData()
        {
            List<OrderOptionDataDtoResponse> orderOptionData = new List<OrderOptionDataDtoResponse>();
            List<DeliveryType> deliveryTypeData = new List<DeliveryType>();
            List<DeliveryState> deliveryStateData = new List<DeliveryState>();

            try
            {
                Array deliveryTypeArray = Enum.GetValues(typeof(DeliveryTypeCode));
                Array deliveryStateArray = Enum.GetValues(typeof(DeliveryStateCode));

                foreach (DeliveryTypeCode value in deliveryTypeArray)
                {
                    deliveryTypeData.Add(new DeliveryType
                    {
                        TypeId = (int)value,
                        TypeName = value.ToString()
                    });
                }

                foreach (DeliveryStateCode value in deliveryStateArray)
                {
                    deliveryStateData.Add(new DeliveryState
                    {
                        StateId = (int)value,
                        StateName = value.ToString()
                    });
                }

                orderOptionData.Add(new OrderOptionDataDtoResponse{
                    DeliveryTypes = deliveryTypeData,
                    DeliveryStates = deliveryStateData
                });

                return orderOptionData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
