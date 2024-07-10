using ShoppingMall.Models;
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
        public List<AdminUserData> CheckLoginByAccountPassword(LoginData loginData)
        {
            List<AdminUserData> adminUserData = new List<AdminUserData>();
            HttpContext context = HttpContext.Current;

            try
            {
                Dictionary<string, object> spData= new Dictionary<string, object>
                {
                    { "acc", loginData.Acc },
                    { "pwd", loginData.Pwd }
                };

                DataTable loginResult = ExcuteSp("pro_bkg_getLoginData", spData);

                if (loginResult.Rows.Count > 0)
                {
                    string randCode = GenerateRandomBytes(32);
                    string originToken = $"{loginResult.Rows[0]["f_id"].ToString()},{randCode}";
                    string token = AesEncrypt(originToken);

                    // 将对象添加到列表中
                    adminUserData.Add(new AdminUserData
                    {
                        Name = loginResult.Rows[0]["f_name"].ToString(),
                        Token = token
                    });

                    RedisConnection().GetDatabase().StringSet($"{loginResult.Rows[0]["f_id"].ToString()}_token", token, TimeSpan.FromMinutes(10));
                    context.Session["token"] = token;
                }

                return adminUserData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool CheckInputData(LoginData loginData)
        {
            string rule = @"^[a-zA-Z0-9]+$";

            if (loginData.Acc.Length > 16 || loginData.Pwd.Length > 16) return false;
            if (!Regex.IsMatch(loginData.Acc, rule) || !Regex.IsMatch(loginData.Pwd, rule)) return false;

            return true;
        }
    }
}
