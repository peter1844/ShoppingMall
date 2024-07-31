using ShoppingMall.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShoppingMall.Views
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)Session["token"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if ((string)Session["permissions"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else 
            {
                int permission = (int)Permissions.Admin;
                string[] allPermissions = Session["permissions"].ToString().Split(',');

                if (!allPermissions.Contains(permission.ToString())) Response.Redirect("Index.aspx");
            }
        }
    }
}