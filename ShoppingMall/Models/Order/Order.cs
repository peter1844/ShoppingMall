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
    public class OrderConditionDataDto
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
    /// 新增訂單傳入資料
    /// </summary>
    public class InsertOrderDataDto
    {
        /// <summary>
        /// 會員ID
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 配送方式
        /// </summary>
        public int DeliverType { get; set; }
        /// <summary>
        /// 訂單總金額
        /// </summary>
        public int TotalMoney { set; get; }
        /// <summary>
        /// 購買商品資料
        /// </summary>
        public List<CommodityInsertData> CommodityDatas { get; set; }
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

    /// <summary>
    /// 刪除訂單傳入資料
    /// </summary>
    public class DeleteOrderDataDto
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderId { set; get; }
    }

    /// <summary>
    /// 商品傳入資料
    /// </summary>
    public class CommodityInsertData
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int CommodityId { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        public int Price { set; get; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { set; get; }
    }
}