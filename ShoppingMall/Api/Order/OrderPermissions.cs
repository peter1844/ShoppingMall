using ShoppingMall.App_Code;
using ShoppingMall.Models.Order;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Order
{
    public class OrderPermissions : ShoppingMall.Base.Base
    {
        /// <summary>
        /// 取得商品頁面所有權限
        /// </summary>
        public List<OrderPermissionsDtoResponse> GetAllOrderPermissions()
        {
            List<OrderPermissionsDtoResponse> orderPermissions = new List<OrderPermissionsDtoResponse>();

            try
            {
                orderPermissions.Add(new OrderPermissionsDtoResponse
                {
                    InsertPermission = this.CheckPermission((int)Permissions.OrderInsert),
                    UpdatePermission = this.CheckPermission((int)Permissions.OrderUpdate)
                });

                return orderPermissions;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString(), ex);
            }
        }
    }
}
