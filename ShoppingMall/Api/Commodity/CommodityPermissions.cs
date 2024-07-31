using ShoppingMall.App_Code;
using ShoppingMall.Models.Commodity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Commodity
{
    public class CommodityPermissions : ShoppingMall.Base.Base
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
                    InsertPermission = this.CheckPermission((int)Permissions.CommodityInsert),
                    UpdatePermission = this.CheckPermission((int)Permissions.CommodityUpdate)
                });

                return commodityPermissions;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString(), ex);
            }
        }
    }
}
