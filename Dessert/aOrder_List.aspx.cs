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
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace Dessert
{
    public partial class aOrderDetail : System.Web.UI.Page
    {
        DataTable dt = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Inititle();                
            }
            
            HtmlGenericControl li = (HtmlGenericControl)Master.FindControl("aOrderList");
            li.Attributes.Add("class", "active");
            
            this.btnNewOrder.Click += new EventHandler(btnNewOrder_Click);
            this.GridView1.RowDeleting += new GridViewDeleteEventHandler(GridView1_RowDeleting);
        }
        
        private DataTable Inititle()
        {
            try
            {
                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand("Select count(order_no) From dessert_order ", conn);
                conn.Open();
                // 共有多少筆資料
                int iRecordCount = (int)cmd.ExecuteScalar();

                Pagination Page = new Pagination(iRecordCount, Request["page"] ); 
                
                // 輸出資料並綁定GridView
                string sSql = @"Select A.order_no, (A.start_date + ' ~ ' + A.end_date) As order_date, A.start_date, A.end_date, C.store_name, SUM(B.quantity*B.item_price) As Total ,SUM(B.quantity) As Quantity 
                                From ((dessert_order A
                                Left Join filledIn B On A.order_no = B.order_no)
                                Left Join store C On A.store_no = C.store_no)
                                Where A.order_no 
                                    Between (Select min(order_no) 
                                        From (Select Top {0} order_no From dessert_order Order By order_no Desc))
                                    And (Select min(order_no) 
                                        From (Select Top {1} order_no From dessert_order Order By order_no Desc))
                                Group By A.order_no, A.start_date, A.end_date, C.store_name
                                Order By A.order_no Desc";

                sSql = Page.FormatSql(sSql);

                dt = new DataTable();

                cmd = new OleDbCommand(sSql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // 每筆訂單的狀態
                    dt.Columns.Add("status", Type.GetType("System.String"));

                    string sStatus = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        // 判斷訂單是否超過期限
                        DateTime dtStartDate = Convert.ToDateTime(row["start_date"]);
                        DateTime dtEndDate = Convert.ToDateTime(row["end_date"]);

                        if (DateTime.Now.CompareTo(dtStartDate) >= 0 && DateTime.Now.CompareTo(dtEndDate) <= 0)
                        {
                            sStatus = "開放中";
                        }
                        else
                        {
                            sStatus = "已截止";
                        }
                        row["status"] = sStatus;
                    }

                    this.GridView1.DataSource = dt;
                    this.GridView1.DataBind();

                    // 設定GridView1狀態欄位的Css樣式
                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        if (((Label)this.GridView1.Rows[i].Cells[5].FindControl("lblstatus")).Text == "開放中")
                        {
                            ((Label)this.GridView1.Rows[i].Cells[5].FindControl("lblstatus")).ControlStyle.CssClass = "badge badge-pill badge-info";
                        }
                        else
                        {
                            ((Label)this.GridView1.Rows[i].Cells[5].FindControl("lblstatus")).ControlStyle.CssClass = "badge badge-pill badge-secondary";
                        }
                    }

                    // 輸出頁碼
                    this.lblPagination.Text = Page.PaginationOut("aOrder_List.aspx").ToString();
                    this.lblResultStats.Text = "共" + Page.TotalPage.ToString() + "頁，" + iRecordCount.ToString() + "筆結果";
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    this.GridView1.DataSource = dt;
                    this.GridView1.DataBind();
                    this.GridView1.Rows[0].Cells.Clear();
                    this.GridView1.Rows[0].Cells.Add(new TableCell());
                    this.GridView1.Rows[0].Cells[0].ColumnSpan = this.GridView1.Columns.Count;
                    this.GridView1.Rows[0].Cells[0].Text = "查無資料...馬上新增!";
                    this.GridView1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

                    this.lblResultStats.Text = "";
                    this.lblPagination.Text = "";
                }
            }
            catch (Exception ex)
            {
                this.divAlert.Style.Add("display", "block");
                this.lblAlert.Text = "Error : " + ex.Message;
            }
            return dt;
        }
        // 刪除訂單
        private void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand("Delete From dessert_order Where order_no = ?", conn);
                cmd.Parameters.AddWithValue("order_no", this.GridView1.DataKeys[e.RowIndex].Value.ToString());
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Cancel();
                conn.Close();

                Inititle(); 
            }
            catch (Exception ex)
            {
                this.divAlert.Style.Add("display", "block");
                this.lblAlert.Text = "Error : " + ex.Message;
            }
        }

        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            if (sender.Equals(this.btnNewOrder))
            {
                Response.Redirect("aOrder_New.aspx");
            }
        }
    }
}