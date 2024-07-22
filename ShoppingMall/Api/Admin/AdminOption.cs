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

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getAllAdminOptionData";
                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt); 

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        adminOptionData.Add(new AdminOptionDataDtoResponse
                        {
                            RoleId = Convert.ToInt32(dt.Rows[i]["f_id"]),
                            RoleName = dt.Rows[i]["f_name"].ToString(),
                        });
                    }
                }

                return adminOptionData;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString(), ex);
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }
    }
}
