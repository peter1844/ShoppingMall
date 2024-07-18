using ShoppingMall.App_Code;
using ShoppingMall.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.Results;
using System.Web.UI.WebControls;

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
                throw new Exception(StateCode.DbError.ToString());
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }
        public bool InsertAdminData(InsertAdminDataDto insertData)
        {
            SqlCommand command = MsSqlConnection();

            try
            {
                DataTable tempTable = new DataTable();
                tempTable.Columns.Add("roleId", typeof(int));

                foreach (int roleId in insertData.Roles)
                {
                    tempTable.Rows.Add(roleId);
                }
                
                command.CommandText = "EXEC pro_bkg_insertAdminData @name,@acc,@pwd,@enabled,@roleId";
                
                command.Parameters.AddWithValue("@name", insertData.Name);
                command.Parameters.AddWithValue("@acc", insertData.Acc);
                command.Parameters.AddWithValue("@pwd", insertData.Pwd);
                command.Parameters.AddWithValue("@enabled", insertData.Enabled);
                SqlParameter parameter = command.Parameters.AddWithValue("@roleId", tempTable);
                parameter.SqlDbType = SqlDbType.Structured;
                parameter.TypeName = "dbo.adminUserRoleTempType";

                command.Connection.Open();

                int statusMessage = Convert.ToInt32(command.ExecuteScalar());

                if(statusMessage != (int)StateCode.Success) throw new Exception(StateCode.DbError.ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString());
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
            
        }
        public bool UpdateAdminData(UpdateAdminDataDto updateData)
        {
            SqlCommand command = MsSqlConnection();
            
            try
            {
                DataTable tempTable = new DataTable();
                tempTable.Columns.Add("roleId", typeof(int));

                foreach (int roleId in updateData.Roles)
                {
                    tempTable.Rows.Add(roleId);
                }

                command.CommandText = "EXEC pro_bkg_updateAdminData @adminId,@name,@pwd,@enabled,@roleId";

                command.Parameters.AddWithValue("@adminId", updateData.AdminId);
                command.Parameters.AddWithValue("@name", updateData.Name);
                command.Parameters.AddWithValue("@pwd", updateData.Pwd);
                command.Parameters.AddWithValue("@enabled", updateData.Enabled);
                SqlParameter parameter = command.Parameters.AddWithValue("@roleId", tempTable);
                parameter.SqlDbType = SqlDbType.Structured;
                parameter.TypeName = "dbo.adminUserRoleTempType";

                command.Connection.Open();

                int statusMessage = Convert.ToInt32(command.ExecuteScalar());
                
                if (statusMessage != (int)StateCode.Success) throw new Exception(StateCode.DbError.ToString());
                if (!string.IsNullOrEmpty(updateData.Pwd)) RedisConnection().GetDatabase().KeyDelete($"{updateData.AdminId}_token");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString());
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }

        }
        public bool DeleteAdminData(DeleteAdminDataDto deleteData)
        {
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_deleteAdminData @adminId";

                command.Parameters.AddWithValue("@adminId", deleteData.AdminId);

                command.Connection.Open();

                int statusMessage = Convert.ToInt32(command.ExecuteScalar());

                if (statusMessage != (int)StateCode.Success) throw new Exception(StateCode.DbError.ToString());
                RedisConnection().GetDatabase().KeyDelete($"{deleteData.AdminId}_token");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString());
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }

        }
        public bool CheckInsertInputData(InsertAdminDataDto insertData)
        {
            string rule = @"^[a-zA-Z0-9]+$";

            if (string.IsNullOrEmpty(insertData.Acc) || string.IsNullOrEmpty(insertData.Pwd) || string.IsNullOrEmpty(insertData.Name) || insertData.Roles.Count == 0) return false;
            if (insertData.Enabled < 0 || insertData.Enabled > 1) return false;
            if (insertData.Acc.Length > 16 || insertData.Pwd.Length > 16 || insertData.Name.Length > 20) return false;
            if (!Regex.IsMatch(insertData.Acc, rule) || !Regex.IsMatch(insertData.Pwd, rule)) return false;

            return true;
        }
        public bool CheckUpdateInputData(UpdateAdminDataDto updateData)
        {
            string rule = @"^[a-zA-Z0-9]+$";

            if (string.IsNullOrEmpty(updateData.Name) || updateData.Roles.Count == 0) return false;
            if (updateData.Enabled < 0 || updateData.Enabled > 1) return false;
            if (updateData.Pwd.Length > 16 || updateData.Name.Length > 20) return false;
            if (!string.IsNullOrEmpty(updateData.Pwd) && !Regex.IsMatch(updateData.Pwd, rule)) return false;

            return true;
        }
        public bool CheckDeleteInputData(DeleteAdminDataDto deleteData)
        {
            if (deleteData.AdminId <= 0) return false;

            return true;
        }
    }
}
