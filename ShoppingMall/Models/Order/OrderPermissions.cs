﻿namespace ShoppingMall.Models.Order
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class OrderPermissionsDtoResponse
    {
        /// <summary>
        /// 新增權限
        /// </summary>
        public bool InsertPermission { get; set; }
        /// <summary>
        /// 編輯權限
        /// </summary>
        public bool UpdatePermission { get; set; }
    }
}