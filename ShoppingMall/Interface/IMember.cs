using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Login;
using ShoppingMall.Models.Member;
using System;
using System.Collections.Generic;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface IMember
    {
        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        List<MemberDataDtoResponse> GetAllMemberData();

        /// <summary>
        /// 編輯會員資料
        /// </summary>
        bool UpdateMemberData(UpdateMemberDataDto updateData);

        /// <summary>
        /// 檢查編輯會員的傳入參數
        /// </summary>
        bool CheckUpdateInputData(UpdateMemberDataDto updateData);
    }
}
