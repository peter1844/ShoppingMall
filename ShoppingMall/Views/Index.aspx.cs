using System;

namespace ShoppingMall.Views
{
    public partial class Index : BasePage
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