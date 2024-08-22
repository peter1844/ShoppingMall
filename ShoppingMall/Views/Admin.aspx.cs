using ShoppingMall.Models.Enum;
using System;
using System.Linq;

namespace ShoppingMall.Views
{
    public partial class Admin : BasePage
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