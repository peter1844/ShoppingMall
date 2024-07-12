using ShoppingMall.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ShoppingMall.Api.Admin
{
    public class AdminProccess : ShoppingMall.Base.Base
    {
        public List<AdminUserDataDtoResponse> GetAllAdminUserData()
        {
            List<AdminUserDataDtoResponse> adminUserData = new List<AdminUserDataDtoResponse>();
            List<AdminUserDataDtoResponse> groupAdminUserData = new List<AdminUserDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getAllAdminData";
                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        adminUserData.Add(new AdminUserDataDtoResponse
                        {
                            Id = Convert.ToInt32(dt.Rows[i]["f_id"]),
                            Acc = dt.Rows[i]["f_acc"].ToString(),
                            Name = dt.Rows[i]["f_name"].ToString(),
                            Enabled = Convert.ToInt32(dt.Rows[i]["f_enabled"]),
                            Role = new List<AdminUserRoleData> { new AdminUserRoleData() { RoleId = Convert.ToInt32(dt.Rows[i]["f_roleId"]) } }
                        });
                    }

                    groupAdminUserData = adminUserData
                        .GroupBy(u => u.Id)
                        .Select(g => new AdminUserDataDtoResponse
                        {
                            Id = g.First().Id,
                            Acc = g.First().Acc,
                            Name = g.First().Name,
                            Enabled = g.First().Enabled,
                            Role = g.SelectMany(u => u.Role).ToList()  // 将每个组内的 Role 合并到一个列表中
                        }).ToList();

                }

                return groupAdminUserData;
            }
            catch (Exception ex)
            {
                throw new Exception("A102");
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }
        public int InsertAdminData(InsertAdminDataDto insertData)
        {
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_insertAdminData @name,@acc,@pwd,@enabled";
                command.Parameters.AddWithValue("@name", insertData.Name);
                command.Parameters.AddWithValue("@acc", insertData.Acc);
                command.Parameters.AddWithValue("@pwd", insertData.Pwd);
                command.Parameters.AddWithValue("@enabled", insertData.Enabled);
                command.Connection.Open();

                int iExecuteCount = command.ExecuteNonQuery();



                return iExecuteCount;
            }
            catch (Exception ex)
            {
                throw new Exception("A102");
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
            return 1;
        }
        public bool CheckInputData(InsertAdminDataDto insertData)
        {
            string rule = @"^[a-zA-Z0-9]+$";

            if (insertData.Acc.Length > 16 || insertData.Pwd.Length > 16) return false;
            if (!Regex.IsMatch(insertData.Acc, rule) || !Regex.IsMatch(insertData.Pwd, rule)) return false;

            return true;
        }
    }
}
