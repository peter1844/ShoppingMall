using ShoppingMall.Api.Order;
using ShoppingMall.Helper;
using System;
using System.Timers;

namespace ShoppingMall.Runtime
{
    public static class DeleteOrder
    {
        /// <summary>
        /// 每隔1天執行一次刪除訂單
        /// </summary>
        public static void DeleteOrderTimer()
        {
            Timer timer = new Timer(TimeSpan.FromDays(1).TotalMilliseconds);

            timer.Elapsed += (s, ea) =>
            {
                try
                {
                    OrderProccess.DeleteOrderData();
                }
                catch (Exception ex)
                {
                    LogHelper.Warn(ex.Message);
                }
            };

            timer.Start();
        }
    }
}
