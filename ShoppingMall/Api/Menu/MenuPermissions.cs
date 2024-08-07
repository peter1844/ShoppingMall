using ShoppingMall.App_Code;
using ShoppingMall.Models.Menu;
using System;
using System.Collections.Generic;

namespace ShoppingMall.Api.Menu
{
    public class MenuPermissions
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
                    MemberPermission = Tools.CheckPermission((int)Permissions.Member),
                    CommodityPermission = Tools.CheckPermission((int)Permissions.Commodity),
                    OrderPermission = Tools.CheckPermission((int)Permissions.Order),
                    AdminPermission = Tools.CheckPermission((int)Permissions.Admin),
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
