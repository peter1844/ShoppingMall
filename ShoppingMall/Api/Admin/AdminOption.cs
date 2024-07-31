using ShoppingMall.App_Code;
using ShoppingMall.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Admin
{
    public class AdminOption : ShoppingMall.Base.Base
    {
        /// <summary>
        /// 取得管理者帳號頁面所需的選項
        /// </summary>
        public List<AdminOptionDataDtoResponse> GetAllAdminOptionData()
        {
            List<AdminOptionDataDtoResponse> adminOptionData = new List<AdminOptionDataDtoResponse>();

            try 
            {
                Array rolesArray = Enum.GetValues(typeof(Roles));

                foreach (Roles role in rolesArray) 
                {
                    // 排除超級管理者
                    if ((int)role != 1)
                    {
                        adminOptionData.Add(new AdminOptionDataDtoResponse()
                        {
                            RoleId = (int)role,
                            RoleName = role.ToString()
                        });
                    }
                }

                return adminOptionData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
