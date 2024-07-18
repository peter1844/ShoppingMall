using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace ShoppingMall.App_Code
{
    public enum StateCode
    {
        InvaildInputData = 100,
        Success = 200,
        DbError = 300,
        InvaildToken = 401,
        NoHeaderToken = 4011,
        InvaildLogin = 4012
    }

    public class Tools
    {
    }
}