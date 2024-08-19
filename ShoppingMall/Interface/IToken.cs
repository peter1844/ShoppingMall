﻿using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Login;
using System;
using System.Collections.Generic;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface IToken
    {
        /// <summary>
        /// 用Token檢查登入
        /// </summary>
        bool CheckLoginByToken();

        /// <summary>
        /// 檢查是否為有效Token
        /// </summary>
        bool IsValidToken(string token);

        /// <summary>
        /// token驗證通過時延長時間
        /// </summary>
        void ExtendRedisLoginToken(string token);
    }
}
