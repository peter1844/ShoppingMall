using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Login;
using System.Collections.Generic;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface ILogin
    {
        /// <summary>
        /// 用帳號密碼檢查登入
        /// </summary>
        List<AdminUserDataDtoResponse> CheckLoginByAccountPassword(LoginDataDto loginData);

        /// <summary>
        /// 設定登入者的權限
        /// </summary>
        void SetLoginAdminPermissions(int adminId);

        /// <summary>
        /// 檢查登入的傳入參數
        /// </summary>
        bool CheckInputData(LoginDataDto loginData);
    }
}
