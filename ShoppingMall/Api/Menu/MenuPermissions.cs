using ShoppingMall.App_Code;
using ShoppingMall.Models.Menu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Menu
{
    public class MenuPermissions : ShoppingMall.Base.Base
    {
        /// <summary>
        /// 取得會員頁面所有權限
        /// </summary>
        public List<MenuPermissionsDtoResponse> GetAllMenuPermissions()
        {
            List<MenuPermissionsDtoResponse> menuPermissions = new List<MenuPermissionsDtoResponse>();

            try
            {
                menuPermissions.Add(new MenuPermissionsDtoResponse
                {
                    MemberPermission = this.CheckPermission((int)Permissions.Member),
                    CommodityPermission = this.CheckPermission((int)Permissions.Commodity),
                    OrderPermission = this.CheckPermission((int)Permissions.Order),
                    AdminPermission = this.CheckPermission((int)Permissions.Admin),
                });

                return menuPermissions;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString(), ex);
            }
        }
    }
}
