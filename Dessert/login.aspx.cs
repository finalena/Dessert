using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Data.OleDb;
using System.Configuration;

namespace Dessert
{
    public partial class WebForm11 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnLogin.Click += new EventHandler(btnLogin_Click);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand("Select * From employee  Where employee_id = ? and employee_pwd =?", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("employee_id", this.txtId.Text);
            cmd.Parameters.AddWithValue("employee_pwd", this.txtPwd.Text);

            conn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();

            //管理員登入
            if ((this.txtId.Text.Trim() == "admin" && this.txtPwd.Text == "123456"))
            {
                Session["admin_id"] = this.txtId.Text;
                Response.Redirect("aOrder_List.aspx");             
            }
            //一般使用者登入
            else if (dr.Read())
            {
                Session["employee_no"] = dr[0];
                Session["employee_name"] = dr[3];
                cmd.Cancel();
                dr.Close();
                conn.Close();
                conn.Dispose();
                Response.Redirect("uOrder_List.aspx");
            }
            else
            {   this.lblRemind.Text = "帳號或密碼有誤，請重新輸入。";
                return;
            }
        }
    }
}