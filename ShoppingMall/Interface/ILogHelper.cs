namespace ShoppingMall.Interface
{
    public interface ILogHelper
    {
        /// <summary>
        /// 將訊息紀錄到Info等級的log
        /// </summary>
        void Info(string message);

        /// <summary>
        /// 將訊息紀錄到Warn等級的log
        /// </summary>
        void Warn(string message);

        /// <summary>
        /// 將訊息紀錄到Error等級的log
        /// </summary>
        void Error(string message);
    }
}
