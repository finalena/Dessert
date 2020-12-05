using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Web.UI.HtmlControls;

namespace Dessert
{
    public partial class aOrder_New : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               GetData();
            }
            
            this.divNotify.Style.Add("display", "none");

            HtmlGenericControl li = (HtmlGenericControl)Master.FindControl("aOrderList");
            li.Attributes.Add("class", "active");
            
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
        }
        //取得店家名稱綁定dropdown
        private DataTable GetData() 
        {
            DataTable dt = new DataTable();

            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand("Select store_no,store_name From store Order By store_no ", conn);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dt);

            this.ddlStore.DataSource = dt;
            this.ddlStore.DataTextField = "store_name";
            this.ddlStore.DataValueField = "store_no";
            this.ddlStore.DataBind();

            return dt;
        }
        
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string sSql = "Insert Into dessert_order(start_date, end_date, order_remark, store_no) values (?,?,?,?);";

            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand(sSql, conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("start_date", this.txtStartDate.Text.Trim());
            cmd.Parameters.AddWithValue("end_date", this.txtEndDate.Text.Trim());
            cmd.Parameters.AddWithValue("order_remark", this.txtRemark.Text.Trim());
            cmd.Parameters.AddWithValue("store_no", int.Parse(this.ddlStore.Text));
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.Cancel();
            conn.Close();

            this.divNotify.Style.Add("display", "block");
        }

    }
}