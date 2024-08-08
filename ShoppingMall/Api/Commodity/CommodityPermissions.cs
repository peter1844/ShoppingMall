using ShoppingMall.App_Code;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;

namespace ShoppingMall.Api.Commodity
{
    public class CommodityPermissions
    {
        /// <summary>
        /// 取得商品頁面所有權限
        /// </summary>
        public List<CommodityPermissionsDtoResponse> GetAllCommodityPermissions()
        {
            List<CommodityPermissionsDtoResponse> commodityPermissions = new List<CommodityPermissionsDtoResponse>();

            try
            {
                commodityPermissions.Add(new CommodityPermissionsDtoResponse
                {
                    InsertPermission = Tools.CheckPermission((int)Permissions.CommodityInsert),
                    UpdatePermission = Tools.CheckPermission((int)Permissions.CommodityUpdate)
                });

                return commodityPermissions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
