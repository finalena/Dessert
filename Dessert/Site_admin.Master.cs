using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Dessert
{
    public partial class Site_Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin_id"] == null)
            {
                Response.Redirect("login.aspx");
            }
            this.logout.ServerClick += new EventHandler(logout_ServerClick);
        }

        private void logout_ServerClick(object sender, EventArgs e)
        {
            Session.Abandon();              
            Response.Redirect("login.aspx");    
        }

       
    }
}