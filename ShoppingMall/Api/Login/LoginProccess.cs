using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Models.Login;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using ShoppingMall.Interface;

namespace ShoppingMall.Api.Login
{
    public class LoginProccess : ILogin
    {
        private IContextHelper _contextHelper;
        private IDbHelper _dbHelper;
        private ITools _tools;

        public LoginProccess(IContextHelper contextHelper = null, IDbHelper dbHelper = null, ITools tools = null)
        {
            _contextHelper = contextHelper ?? new ContextHelper();
            _dbHelper = dbHelper ?? new DbHelper();
            _tools = tools ?? new Tools();
        }

        /// <summary>
        /// 用帳號密碼檢查登入
        /// </summary>
        public List<AdminUserDataDtoResponse> CheckLoginByAccountPassword(LoginDataDto loginData)
        {
            List<AdminUserDataDtoResponse> adminUserData = new List<AdminUserDataDtoResponse>();
           

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = _dbHelper.MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getLoginData @acc,@pwd";
                command.Parameters.AddWithValue("@acc", loginData.Acc);
                command.Parameters.AddWithValue("@pwd", loginData.Pwd);
                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string randCode = _tools.GenerateRandomBytes(32);
                    string originToken = $"{dt.Rows[0]["f_id"].ToString()},{randCode}";
                    string token = _tools.AesEncrypt(originToken);

                    // 将对象添加到列表中
                    adminUserData.Add(new AdminUserDataDtoResponse
                    {
                        AdminId = Convert.ToInt32(dt.Rows[0]["f_id"]),
                        Name = dt.Rows[0]["f_name"].ToString(),
                        Token = token
                    });

                    _dbHelper.RedisConnection().GetDatabase().StringSet($"{dt.Rows[0]["f_id"].ToString()}_token", token, TimeSpan.FromMinutes(20));
                    _contextHelper.GetContext().Session["id"] = dt.Rows[0]["f_id"].ToString();
                    _contextHelper.GetContext().Session["token"] = token;
                }

                return adminUserData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }

        /// <summary>
        /// 設定登入者的權限
        /// </summary>
        public void SetLoginAdminPermissions(int adminId)
        {
            HttpContext context = HttpContext.Current;

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = _dbHelper.MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getLoginPermissionsData @adminId";
                command.Parameters.AddWithValue("@adminId", adminId);
                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    List<string> permissionsList = new List<string>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        permissionsList.Add(dt.Rows[i]["f_permissionsId"].ToString());
                    }

                    context.Session["permissions"] = string.Join(",", permissionsList.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }

        /// <summary>
        /// 檢查登入的傳入參數
        /// </summary>
        public bool CheckInputData(LoginDataDto loginData)
        {
            string rule = @"^[a-zA-Z0-9]+$";

            // 檢查帳號、密碼是否為空
            if (string.IsNullOrEmpty(loginData.Acc) || string.IsNullOrEmpty(loginData.Pwd)) return false;
            // 檢查帳號、密碼長度是否合法
            if (loginData.Acc.Length > 16 || loginData.Pwd.Length > 16) return false;
            // 檢查帳號、密碼是否有非法字元
            if (!Regex.IsMatch(loginData.Acc, rule) || !Regex.IsMatch(loginData.Pwd, rule)) return false;

            return true;
        }
    }
}
