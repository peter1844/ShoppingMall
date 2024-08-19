using NLog;
using ShoppingMall.Interface;

namespace ShoppingMall.Helper
{
    public class LogHelper : ILogHelper
    {
        private Logger logger;

        public LogHelper()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// 將訊息紀錄到Info等級的log
        /// </summary>
        public void Info(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// 將訊息紀錄到Warn等級的log
        /// </summary>
        public void Warn(string message)
        {
            logger.Warn(message);
        }

        /// <summary>
        /// 將訊息紀錄到Error等級的log
        /// </summary>
        public void Error(string message) 
        {
            logger.Error(message);
        }
    }
}
