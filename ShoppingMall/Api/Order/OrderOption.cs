using ShoppingMall.Api.Commodity;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Order;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;

namespace ShoppingMall.Api.Order
{
    public class OrderOption
    {
        private CommodityProccess commodityProccessclass;

        public OrderOption()
        {
            commodityProccessclass = new CommodityProccess();
        }
        /// <summary>
        /// 取得訂單管理頁面所需的選項
        /// </summary>
        public List<OrderOptionDataDtoResponse> GetOrderOptionData()
        {
            List<OrderOptionDataDtoResponse> orderOptionData = new List<OrderOptionDataDtoResponse>();
            List<PayType> payTypeData = new List<PayType>();
            List<PayState> payStateData = new List<PayState>();
            List<DeliveryType> deliveryTypeData = new List<DeliveryType>();
            List<DeliveryState> deliveryStateData = new List<DeliveryState>();
            List<OpenCommodityData> commodityData = new List<OpenCommodityData>();

            try
            {
                Array payTypeArray = Enum.GetValues(typeof(PayTypeCode));
                Array payStateArray = Enum.GetValues(typeof(PayStateCode));
                Array deliveryTypeArray = Enum.GetValues(typeof(DeliveryTypeCode));
                Array deliveryStateArray = Enum.GetValues(typeof(DeliveryStateCode));

                foreach (PayTypeCode value in payTypeArray)
                {
                    payTypeData.Add(new PayType
                    {
                        TypeId = (int)value,
                        TypeName = value.ToString()
                    });
                }

                foreach (PayStateCode value in payStateArray)
                {
                    payStateData.Add(new PayState
                    {
                        StateId = (int)value,
                        StateName = value.ToString()
                    });
                }

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

                commodityData = commodityProccessclass.GetOpenCommodityData();

                orderOptionData.Add(new OrderOptionDataDtoResponse
                {
                    PayTypes = payTypeData,
                    PayStates = payStateData,
                    DeliveryTypes = deliveryTypeData,
                    DeliveryStates = deliveryStateData,
                    OpenCommodityDatas = commodityData
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
