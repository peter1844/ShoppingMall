using ShoppingMall.Api.Order;
using ShoppingMall.Helper;
using ShoppingMall.Interface;
using System;
using System.Timers;

namespace ShoppingMall.Runtime
{
    public class DeleteOrder : IDeleteOrder
    {
        private IOrder _order;
        private ILogHelper _logHelper;

        public DeleteOrder(IOrder order = null, ILogHelper logHelper = null)
        {
            _order = order ?? new OrderProccess();
            _logHelper = logHelper ?? new LogHelper();
        }

        /// <summary>
        /// 每隔1天執行一次刪除訂單
        /// </summary>
        public void DeleteOrderTimer()
        {
            Timer timer = new Timer(TimeSpan.FromDays(1).TotalMilliseconds);

            timer.Elapsed += (s, ea) =>
            {
                try
                {
                    _order.DeleteOrderData();
                }
                catch (Exception ex)
                {
                    _logHelper.Error(ex.Message);
                }
            };

            timer.Start();
        }
    }
}
