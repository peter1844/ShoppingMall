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
        public List<AdminUserData> TestSp(LoginData loginData)
        {
            List<AdminUserData> adminUserData = new List<AdminUserData>();
            HttpContext context = HttpContext.Current;

            try
            {
                DataTable loginResult = ExcuteQueryBySp(loginData.Acc, loginData.Pwd);

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

        public List<AdminUserData> CheckLoginByAccountPassword(LoginData loginData)
        {
            try
            {
                string sql = $"SELECT f_id,f_name FROM t_adminUser WHERE f_acc = '{loginData.Acc}' AND f_pwd = HASHBYTES('MD5','{loginData.Pwd}') AND f_enabled = 1";
                List<AdminUserData> adminUserData = new List<AdminUserData>();
                HttpContext context = HttpContext.Current;

                // 调用父类的 ExecuteQuery 方法执行查询
                SqlDataReader adminUserReader = ExecuteQuery(sql);

                // 处理查询结果
                while (adminUserReader.Read())
                {
                    string randCode = GenerateRandomBytes(32);
                    string originToken = $"{adminUserReader["f_id"].ToString()},{randCode}";
                    string token = AesEncrypt(originToken);

                    // 将对象添加到列表中
                    adminUserData.Add(new AdminUserData
                    {
                        Name = adminUserReader["f_name"].ToString(),
                        Token = token
                    });

                    RedisConnection().GetDatabase().StringSet($"{adminUserReader["f_id"].ToString()}_token", token, TimeSpan.FromMinutes(10));
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
