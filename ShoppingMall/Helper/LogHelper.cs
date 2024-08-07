using NLog;

namespace ShoppingMall.Helper
{
    public static class LogHelper
    {
        public static Logger logger;

        static LogHelper()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
    }
}
