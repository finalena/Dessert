using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Dessert
{
    public partial class uOrdered : System.Web.UI.Page
    {
        string sOrder_no = null;
        DataSet ds = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            sOrder_no = Request["order_no"] ?? "";

            if (!IsPostBack)
            {
                Inititle();
                GetFilledIn();
            }

            HtmlGenericControl li = (HtmlGenericControl)Master.FindControl("uOrderList");
            li.Attributes.Add("class", "active");

            this.btnClear.Click += new EventHandler(btnClear_Click);
        }

        private void Inititle()
        {
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand("Select A.*, B.store_name From dessert_order A Left Join store B On A.store_no = B.store_no Where A.order_no = ?", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("order_no", sOrder_no);
            conn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            dr.Read();

            if (dr.HasRows)
            {
                this.lblStore.Text = dr.GetString(5);
                this.lblStart_date.Text = dr.GetString(1);
                this.lblEnd_date.Text = dr.GetString(2);
                this.lblRemark.Text = dr.GetString(3);
            }
            else
            {
                Server.Transfer("~/ErrorPages/uOops.aspx");
            }

            cmd.Cancel();
            dr.Close();

            DateTime dtStartDate = Convert.ToDateTime(this.lblStart_date.Text);
            DateTime dtEndDate = Convert.ToDateTime(this.lblEnd_date.Text);
            // 如訂單截止隱藏btnClear
            if (DateTime.Now.CompareTo(dtStartDate) >= 0 && DateTime.Now.CompareTo(dtEndDate) <= 0)
            {
                this.btnClear.Visible = true;
            }
            else
            {
                this.btnClear.Visible = false;
            }
        }

        private DataSet GetFilledIn() 
        {
            string sSql = @"Select distinct filledin_date
                            From filledIn 
                            Where order_no = "+ sOrder_no +" And employee_no = '" + Session["employee_no"].ToString() + @"'
                            Order By filledIn_date Desc";

            string sSql2 = @"Select filledin_date, item_name, quantity, item_price, filledin_remark
                            From filledIn
                            Where order_no = " + sOrder_no + " And employee_no = '" + Session["employee_no"].ToString() + @"'
                            Order By filledIn_date Desc";

            ds = new DataSet();
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbDataAdapter da = new OleDbDataAdapter(sSql, conn);
            OleDbDataAdapter da2 = new OleDbDataAdapter(sSql2, conn);
            da.Fill(ds, "filledInDate");
            da2.Fill(ds, "filledInDetail");
            ds.Relations.Add("Relation_filledin", ds.Tables["filledInDate"].Columns["filledin_date"], ds.Tables["filledInDetail"].Columns["filledin_date"]);
            
            StringBuilder sb = new StringBuilder();
            int iTemp = 0;
            int iSubTotal = 0;
            int iTotal = 0;
            
            if (ds.Tables["filledinDate"].Rows.Count > 0 )
            {
                foreach (DataRow dRow in ds.Tables["filledIndate"].Rows)
                {
                    iTemp = 0;
                    iSubTotal = 0;

                    sb.AppendLine("<table class=\"table \">");
                    sb.AppendLine("<h4 >" + dRow["filledin_date"].ToString() + "</h4>");
                    sb.AppendLine("<thead>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<th scope=\"col=\" >#</th>");
                    sb.AppendLine("<th scope=\"col\" >品名</th>");
                    sb.AppendLine("<th scope=\"col\" >數量</th>");
                    sb.AppendLine("<th scope=\"col\" >單價</th>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("</thead>");
                    sb.AppendLine("<tbody>");

                    foreach (DataRow dRow2 in dRow.GetChildRows(ds.Relations["Relation_filledin"]))
                    {
                        iTemp++;
                        iSubTotal += int.Parse(dRow2["quantity"].ToString()) * int.Parse(dRow2["item_price"].ToString());

                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td>" + iTemp + "</td>");
                        sb.AppendLine("<td>" + dRow2["item_name"].ToString() + "<div class=\"filledin-remark\">" + dRow2["filledin_remark"].ToString() + "</div></td>");
                        sb.AppendLine("<td>" + dRow2["quantity"].ToString() + "</td>");
                        sb.AppendLine("<td>" + dRow2["item_price"].ToString() + "</td>");
                        sb.AppendLine("</tr>");
                    }

                    iTotal += iSubTotal;

                    sb.AppendLine("</tbody>");
                    sb.AppendLine("<tfoot>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td colspan=\"3\">SubTotal</td><td>" + iSubTotal + "</td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("</tfoot>");
                    sb.AppendLine("</table>");
                }        
            }
            else
            {
                sb.AppendLine("<table class=\" table table-borderless \">");
                sb.AppendLine("<tr>");
                sb.AppendLine("<th>查無資料...</th>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</table>");
                
            }

            this.lblAddFilledIn.Text = sb.ToString();
            this.lblTotal.Text = iTotal.ToString();

            return ds;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (sender.Equals(this.btnClear))
            {
                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand("Delete From filledIn Where order_no = " + sOrder_no + "and employee_no ='" + Session["employee_no"].ToString() + "'", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Cancel();
                conn.Close();

                GetFilledIn();
            }
        }
    }
}