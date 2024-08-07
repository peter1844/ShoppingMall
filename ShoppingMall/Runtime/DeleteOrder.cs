using ShoppingMall.Api.Order;
using ShoppingMall.Helper;
using System;
using System.Timers;

namespace ShoppingMall.Runtime
{
    public static class DeleteOrder
    {
        // 每隔1天執行一次刪除訂單
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
                    LogHelper.logger.Warn(ex.Message);
                }
            };

            timer.Start();
        }
    }
}
