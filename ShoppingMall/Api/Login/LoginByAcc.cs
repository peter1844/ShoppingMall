using ShoppingMall.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web;
using ShoppingMall.Models;

namespace ShoppingMall.Api.Login
{
    public class LoginByAcc : ShoppingMall.Base.Base
    {
        public List<AdminUserData> CheckLoginByAccountPassword(LoginData loginData)
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

                REDIS.GetDatabase().StringSet($"{adminUserReader["f_id"].ToString()}_token", token, TimeSpan.FromMinutes(10));
                context.Session["token"] = token;
            }

            return adminUserData;
        }
    }
}
