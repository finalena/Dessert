using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dessert
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["employee_no"] != null)
            {
                this.lblSessionId.Text = Session["employee_name"].ToString();
            }
            else
            {
                Response.Redirect("login.aspx");
            }

            this.logout.ServerClick += Logout_ServerClick;
        }

        private void Logout_ServerClick(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("login.aspx");
        }
    }
}