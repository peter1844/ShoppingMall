using ShoppingMall.Models.Common;
using ShoppingMall.Models.Login;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;

namespace ShoppingMall.Api.Login
{
    public class LoginByAcc : ShoppingMall.Base.Base
    {
        public List<AdminUserDataDtoResponse> CheckLoginByAccountPassword(LoginDataDto loginData)
        {
            List<AdminUserDataDtoResponse> adminUserData = new List<AdminUserDataDtoResponse>();
            HttpContext context = HttpContext.Current;

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXE1C pro_bkg_getLoginData @acc,@pwd";
                command.Parameters.AddWithValue($"@acc", loginData.Acc);
                command.Parameters.AddWithValue($"@pwd", loginData.Pwd);
                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt); 

                if (dt.Rows.Count > 0)
                {
                    string randCode = GenerateRandomBytes(32);
                    string originToken = $"{dt.Rows[0]["f_id"].ToString()},{randCode}";
                    string token = AesEncrypt(originToken);

                    // 将对象添加到列表中
                    adminUserData.Add(new AdminUserDataDtoResponse
                    {
                        Name = dt.Rows[0]["f_name"].ToString(),
                        Token = token
                    });

                    RedisConnection().GetDatabase().StringSet($"{dt.Rows[0]["f_id"].ToString()}_token", token, TimeSpan.FromMinutes(20));
                    context.Session["token"] = token;
                }

                return adminUserData;
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
        public bool CheckInputData(LoginDataDto loginData)
        {
            string rule = @"^[a-zA-Z0-9]+$";

            if (loginData.Acc.Length > 16 || loginData.Pwd.Length > 16) return false;
            if (!Regex.IsMatch(loginData.Acc, rule) || !Regex.IsMatch(loginData.Pwd, rule)) return false;

            return true;
        }
    }
}
