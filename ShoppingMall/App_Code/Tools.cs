using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace ShoppingMall.App_Code
{
    /// <summary>
    /// 角色
    /// </summary>
    public enum Roles
    {
        /// <summary>
        /// 超級管理員
        /// </summary>
        SuperAdmin = 1,
        /// <summary>
        /// 一般管理員
        /// </summary>
        Admin = 2,
        /// <summary>
        /// 客服部
        /// </summary>
        CustomerService = 3,
        /// <summary>
        /// 產品部
        /// </summary>
        Commodity = 4
    }

    /// <summary>
    /// 權限
    /// </summary>
    public enum Permissions
    {
        /// <summary>
        /// 會員管理
        /// </summary>
        Member = 1,
        /// <summary>
        /// 會員管理-編輯
        /// </summary>
        MemberUpdate = 2,
        /// <summary>
        /// 商品管理
        /// </summary>
        Commodity = 3,
        /// <summary>
        /// 商品管理-新增
        /// </summary>
        CommodityInsert = 4,
        /// <summary>
        /// 商品管理-編輯
        /// </summary>
        CommodityUpdate = 5,
        /// <summary>
        /// 訂單管理
        /// </summary>
        Order = 6,
        /// <summary>
        /// 訂單管理-模擬下單
        /// </summary>
        OrderInsert = 7,
        /// <summary>
        /// 訂單管理-編輯
        /// </summary>
        OrderUpdate = 8,
        /// <summary>
        /// 訂單管理-刪除
        /// </summary>
        OrderDelete = 9,
        /// <summary>
        /// 管理者帳號管理
        /// </summary>
        Admin = 10,
        /// <summary>
        /// 管理者帳號管理-新增
        /// </summary>
        AdminInsert = 11,
        /// <summary>
        /// 管理者帳號管理-編輯
        /// </summary>
        AdminUpdate = 12,
        /// <summary>
        /// 管理者帳號管理-刪除
        /// </summary>
        AdminDelete = 13
    }

    /// <summary>
    /// 狀態碼
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 傳入參數驗證失敗
        /// </summary>
        InvaildInputData = 100,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// DB操作失敗
        /// </summary>
        DbError = 300,
        /// <summary>
        /// 無效Token
        /// </summary>
        InvaildToken = 401,
        /// <summary>
        /// Header未夾帶Token
        /// </summary>
        NoHeaderToken = 4011,
        /// <summary>
        /// 登入失敗
        /// </summary>
        InvaildLogin = 4012,
        /// <summary>
        /// 沒有權限
        /// </summary>
        NoPermission = 4013,
        /// <summary>
        /// 庫存量不足
        /// </summary>
        StockError = 700
    }
    
    /// <summary>
    /// 付款方式
    /// </summary>
    public enum PayTypeCode
    {
        /// <summary>
        /// 線上刷卡
        /// </summary>
        Credit = 1,
        /// <summary>
        /// 匯款
        /// </summary>
        Remittance = 2,
        /// <summary>
        /// LinePay
        /// </summary>
        LinePay = 3,
        /// <summary>
        /// 貨到付款
        /// </summary>
        CashOnDelivery = 4
    }

    /// <summary>
    /// 付款狀態
    /// </summary>
    public enum PayStateCode
    {
        /// <summary>
        /// 未付款
        /// </summary>
        UnPaid = 0,
        /// <summary>
        /// 已付款
        /// </summary>
        AlreadyPaid = 1
    }

    /// <summary>
    /// 配送方式
    /// </summary>
    public enum DeliveryTypeCode
    {
        /// <summary>
        /// 陸運
        /// </summary>
        LandTransportation = 1,
        /// <summary>
        /// 海運
        /// </summary>
        Shipping = 2,
        /// <summary>
        /// 空運
        /// </summary>
        AirTransportation = 3
    }

    /// <summary>
    /// 配送狀態
    /// </summary>
    public enum DeliveryStateCode
    {
        /// <summary>
        /// 未出貨
        /// </summary>
        NotShipped = 0,
        /// <summary>
        /// 已出貨
        /// </summary>
        Shipped = 1,
        /// <summary>
        /// 退貨
        /// </summary>
        Return = 2,
    }

    public class Tools
    {
    }
}