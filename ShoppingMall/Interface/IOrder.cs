using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Enum;
using ShoppingMall.Models.Order;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface IOrder
    {
        /// <summary>
        /// 取得訂單資料
        /// </summary>
        List<OrderDataDtoResponse> GetOrderData(OrderConditionDataDto conditionData);

        /// <summary>
        /// 新增訂單資料
        /// </summary>
        bool InsertOrderData(InsertOrderDataDto insertData);

        /// <summary>
        /// 編輯訂單資料
        /// </summary>
        bool UpdateOrderData(UpdateOrderDataDto updateData);

        /// <summary>
        /// 刪除訂單資料
        /// </summary>
        bool DeleteOrderData();

        /// <summary>
        /// 檢查搜尋條件的傳入參數
        /// </summary>
        bool CheckConditionInputData(OrderConditionDataDto conditionData);

        /// <summary>
        /// 檢查新增訂單資料的傳入參數
        /// </summary>
        bool CheckInsertInputData(InsertOrderDataDto insertData);

        /// <summary>
        /// 檢查編輯訂單資料的傳入參數
        /// </summary>
        bool CheckUpdateInputData(UpdateOrderDataDto updateData);

        /// <summary>
        /// 檢查刪除訂單資料的傳入參數
        /// </summary>
        bool CheckDeleteInputData(DeleteOrderDataDto deleteData);

        /// <summary>
        /// 取得訂單管理頁面所需的選項
        /// </summary>
        List<OrderOptionDataDtoResponse> GetOrderOptionData();
    }
}
