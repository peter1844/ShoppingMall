using NLog;

namespace ShoppingMall.Helper
{
    public static class LogHelper
    {
        private static Logger logger;

        static LogHelper()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// 將訊息紀錄到Info等級的log
        /// </summary>
        public static void Info(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// 將訊息紀錄到Warn等級的log
        /// </summary>
        public static void Warn(string message) 
        {
            logger.Warn(message);
        }
    }
}
