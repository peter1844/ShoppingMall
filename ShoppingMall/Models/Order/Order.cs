using System;
using System.Collections.Generic;

namespace ShoppingMall.Models.Order
{
    /// <summary>
    /// 訂單資料
    /// </summary>
    public class OrderDataDtoResponse
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 會員名字
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// 訂單日期
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// 付款方式ID
        /// </summary>
        public int PayTypeId { get; set; }
        /// <summary>
        /// 付款狀態ID
        /// </summary>
        public int PayStateId { get; set; }
        /// <summary>
        /// 配送方式ID
        /// </summary>
        public int DeliverTypeId { get; set; }
        /// <summary>
        /// 配送狀態ID
        /// </summary>
        public int DeliverStateId { get; set; }
        /// <summary>
        /// 配送狀態名稱
        /// </summary>
        public string DeliverStateName { get; set; }
        /// <summary>
        /// 總金額
        /// </summary>
        public int TotalMoney { get; set; }
        /// <summary>
        /// 訂單詳細資料
        /// </summary>
        public List<OrderDetailData> DetailDatas { get; set; }
    }

    /// <summary>
    /// 篩選條件
    /// </summary>
    public class ConditionDataDto
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 訂單日期-起
        /// </summary>
        public DateTime? StartDate { set; get; }
        /// <summary>
        /// 訂單日期-迄
        /// </summary>
        public DateTime? EndDate { set; get; }
        /// <summary>
        /// 配送狀態
        /// </summary>
        public int DeliveryState { get; set; }
    }

    /// <summary>
    /// 訂單詳細資料
    /// </summary>
    public class OrderDetailData
    {
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string CommodityName { get; set; }
        /// <summary>
        /// 購買數量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 購買單價
        /// </summary>
        public int Price { get; set; }
        public string Image { get; set; }
    }

    /// <summary>
    /// 編輯訂單傳入資料
    /// </summary>
    public class UpdateOrderDataDto
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 付款方式ID
        /// </summary>
        public int PayTypeId { get; set; }
        /// <summary>
        /// 付款狀態ID
        /// </summary>
        public int PayStateId { get; set; }
        /// <summary>
        /// 配送方式ID
        /// </summary>
        public int DeliverTypeId { get; set; }
        /// <summary>
        /// 配送狀態ID
        /// </summary>
        public int DeliverStateId { get; set; }
    }
}