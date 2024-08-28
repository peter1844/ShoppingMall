using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Interface;

namespace ShoppingMall.Api.Logout
{
    public class Logout : ILogout
    {
        private IContextHelper _contextHelper;
        private IDbHelper _dbHelper;
        private ITools _tools;

        public Logout(IContextHelper contextHelper, IDbHelper dbHelper, ITools tools)
        {
            _contextHelper = contextHelper;
            _dbHelper = dbHelper;
            _tools = tools;
        }

        /// <summary>
        /// 登出功能
        /// </summary>
        public void LogoutProccess()
        {
            string sessionToken = _tools.AesDecrypt((string)_contextHelper.GetContext().Session["token"]);
            string[] realTokenData = sessionToken.Split(',');

            _contextHelper.ClearContextSession();
            _dbHelper.RedisConnection().GetDatabase().KeyDelete($"{realTokenData[0]}_token");
        }
    }
}
