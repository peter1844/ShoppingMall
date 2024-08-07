using ShoppingMall.App_Code;
using System;
using System.Linq;

namespace ShoppingMall.Views
{
    public partial class Ccommodity : BasePage
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
                int permission = (int)Permissions.Commodity;
                string[] allPermissions = Session["permissions"].ToString().Split(',');

                if (!allPermissions.Contains(permission.ToString())) Response.Redirect("Index.aspx");
            }
        }
    }
}