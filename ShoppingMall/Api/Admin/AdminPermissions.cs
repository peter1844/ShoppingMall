using ShoppingMall.App_Code;
using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;

namespace ShoppingMall.Api.Admin
{
    public class AdminPermissions
    {
        /// <summary>
        /// 取得商品頁面所有權限
        /// </summary>
        public List<AdminPermissionsDtoResponse> GetAllAdminPermissions()
        {
            List<AdminPermissionsDtoResponse> adminPermissions = new List<AdminPermissionsDtoResponse>();

            try
            {
                adminPermissions.Add(new AdminPermissionsDtoResponse
                {
                    InsertPermission = Tools.CheckPermission((int)Permissions.AdminInsert),
                    UpdatePermission = Tools.CheckPermission((int)Permissions.AdminUpdate),
                    DeletePermission = Tools.CheckPermission((int)Permissions.AdminDelete)
                });

                return adminPermissions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
