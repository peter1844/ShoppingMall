using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace ShoppingMall.App_Code
{
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
        InvaildLogin = 4012
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