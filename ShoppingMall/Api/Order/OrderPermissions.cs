using ShoppingMall.App_Code;
using ShoppingMall.Models.Order;
using System;
using System.Collections.Generic;

namespace ShoppingMall.Api.Order
{
    public class OrderPermissions
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
                    InsertPermission = Tools.CheckPermission((int)Permissions.OrderInsert),
                    UpdatePermission = Tools.CheckPermission((int)Permissions.OrderUpdate)
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
