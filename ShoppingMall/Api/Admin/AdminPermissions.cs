using ShoppingMall.App_Code;
using ShoppingMall.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Admin
{
    public class AdminPermissions : ShoppingMall.Base.Base
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
                    InsertPermission = this.CheckPermission((int)Permissions.AdminInsert),
                    UpdatePermission = this.CheckPermission((int)Permissions.AdminUpdate),
                    DeletePermission = this.CheckPermission((int)Permissions.AdminDelete)
                });

                return adminPermissions;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString(), ex);
            }
        }
    }
}
