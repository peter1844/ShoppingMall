﻿namespace ShoppingMall.Interface
{
    public interface IDeleteOrder
    {
        /// <summary>
        /// 每隔1天執行一次刪除訂單
        /// </summary>
        void DeleteOrderTimer();
    }
}
