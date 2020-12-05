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
    public partial class dessert_list : System.Web.UI.Page
    {
        DataTable dt = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Inititle();
            }

            HtmlGenericControl li = (HtmlGenericControl)Master.FindControl("uOrderList");
            li.Attributes.Add("class", "active");
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

                Pagination Page = new Pagination(iRecordCount, Request["page"]);

                string sSql = @"Select T1.order_no, (T1.start_date + ' ~ ' + T1.end_date) As order_date, T1.start_date, T1.end_date, T1.store_name, T1.order_remark, SUM(T2.quantity * T2.item_price) As total ,SUM(T2.quantity) As quantity
                                From (Select * 
                                      From dessert_order A 
                                      Left Join store B On A.store_no = B.store_no) As T1
                                Left Join (Select quantity, item_price, order_no, employee_no
                                           From filledIn
                                           Where employee_no = '"+ Session["employee_no"].ToString() + @"'
                                          ) As T2
                                On T1.order_no = T2.order_no
                                Where A.order_no 
                                    Between (Select min(order_no) 
                                        From (Select Top {0} order_no From dessert_order Order By order_no Desc))
                                    And (Select min(order_no) 
                                        From (Select Top {1} order_no From dessert_order Order By order_no Desc))
                                Group By T1.order_no, T1.start_date, T1.end_date, T1.store_name, T1.order_remark
                                Order By T1.order_no Desc;";

                sSql = Page.FormatSql(sSql);

                dt = new DataTable();

                cmd = new OleDbCommand(sSql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // 設定每筆訂單的狀態
                    dt.Columns.Add("status", Type.GetType("System.String"));

                    string sStatus = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime dtStartDate = Convert.ToDateTime(row["start_date"]);
                        DateTime dtEndDate = Convert.ToDateTime(row["end_date"]);
                        // 判斷訂單是否超過期限
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
                            ((Button)this.GridView1.Rows[i].Cells[6].FindControl("btnOrder")).Enabled = true;
                        }
                        else
                        {
                            ((Label)this.GridView1.Rows[i].Cells[5].FindControl("lblstatus")).ControlStyle.CssClass = "badge badge-pill badge-secondary";
                            ((Button)this.GridView1.Rows[i].Cells[6].FindControl("btnOrder")).Enabled = false;
                        }
                    }

                    // 輸出頁碼
                    this.lblPagination.Text = Page.PaginationOut("uOrder_List.aspx").ToString();
                    this.lblResultStats.Text = "共" + Page.TotalPage + "頁，" + iRecordCount.ToString() + "筆結果";
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    this.GridView1.DataSource = dt;
                    this.GridView1.DataBind();
                    this.GridView1.Rows[0].Cells.Clear();
                    this.GridView1.Rows[0].Cells.Add(new TableCell());
                    this.GridView1.Rows[0].Cells[0].ColumnSpan = this.GridView1.Columns.Count;
                    this.GridView1.Rows[0].Cells[0].Text = "查無資料...";
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
    }
}