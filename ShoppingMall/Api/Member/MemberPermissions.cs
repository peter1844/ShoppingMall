using ShoppingMall.App_Code;
using ShoppingMall.Models.Member;
using System;
using System.Collections.Generic;

namespace ShoppingMall.Api.Member
{
    public class MemberPermissions
    {
        /// <summary>
        /// 取得會員頁面所有權限
        /// </summary>
        public List<MemberPermissionsDtoResponse> GetAllMemberPermissions()
        {
            List<MemberPermissionsDtoResponse> memberPermissions = new List<MemberPermissionsDtoResponse>();

            try
            {
                memberPermissions.Add(new MemberPermissionsDtoResponse
                {
                    UpdatePermission = Tools.CheckPermission((int)Permissions.MemberUpdate)
                });

                return memberPermissions;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString(), ex);
            }
        }
    }
}
