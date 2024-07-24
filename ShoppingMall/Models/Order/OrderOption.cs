using System.Collections.Generic;

namespace ShoppingMall.Models.Order
{
    /// <summary>
    /// 訂單頁面選項資料
    /// </summary>
    public class OrderOptionDataDtoResponse
    {
        /// <summary>
        /// 付款方式
        /// </summary>
        public List<PayType> PayTypes { get; set; }
        /// <summary>
        /// 付款狀態
        /// </summary>
        public List<PayState> PayStates { get; set; }
        /// <summary>
        /// 配送方式
        /// </summary>
        public List<DeliveryType> DeliveryTypes { get; set; }
        /// <summary>
        /// 配送狀態
        /// </summary>
        public List<DeliveryState> DeliveryStates { get; set; }
    }

    /// <summary>
    /// 付款方式
    /// </summary>
    public class PayType
    {
        /// <summary>
        /// 付款方式ID
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 付款方式名稱
        /// </summary>
        public string TypeName { get; set; }
    }

    /// <summary>
    /// 付款狀態
    /// </summary>
    public class PayState
    {
        /// <summary>
        /// 付款狀態ID
        /// </summary>
        public int StateId { get; set; }
        /// <summary>
        /// 付款狀態名稱
        /// </summary>
        public string StateName { get; set; }
    }

    /// <summary>
    /// 配送方式
    /// </summary>
    public class DeliveryType
    {
        /// <summary>
        /// 配送方式ID
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 配送方式名稱
        /// </summary>
        public string TypeName { get; set; }
    }

    /// <summary>
    /// 配送狀態
    /// </summary>
    public class DeliveryState
    {
        /// <summary>
        /// 配送狀態ID
        /// </summary>
        public int StateId { get; set; }
        /// <summary>
        /// 配送狀態名稱
        /// </summary>
        public string StateName { get; set; }
    }
}