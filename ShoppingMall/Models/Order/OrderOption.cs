using System.Collections.Generic;

namespace ShoppingMall.Models.Order
{
    /// <summary>
    /// 訂單頁面選項資料
    /// </summary>
    public class OrderOptionDataDtoResponse
    {
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
    /// 配送方式
    /// </summary>
    public class DeliveryType
    {
        /// <summary>
        /// 方式ID
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string TypeName { get; set; }
    }

    /// <summary>
    /// 配送狀態
    /// </summary>
    public class DeliveryState
    {
        /// <summary>
        /// 狀態ID
        /// </summary>
        public int StateId { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string StateName { get; set; }
    }
}