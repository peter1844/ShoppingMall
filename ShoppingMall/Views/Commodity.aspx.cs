﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShoppingMall.Views
{
    public partial class Ccommodity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)Session["token"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}