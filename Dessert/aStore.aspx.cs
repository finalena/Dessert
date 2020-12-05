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
    public partial class aStore : System.Web.UI.Page
    {
        DataTable dt = null;
        string sSearchStore = "";
                
        protected void Page_Load(object sender, EventArgs e)
        {
            sSearchStore = Request["q"] ?? "";
            
            if (!IsPostBack)
            {
                GetData();
            }

            this.divNotify.Style.Add("display", "none");
            
            HtmlGenericControl li = (HtmlGenericControl)Master.FindControl("aStore");
            li.Attributes.Add("class", "active");
            
            this.GridView1.RowCommand += new GridViewCommandEventHandler(GridView1_RowCommand);
            this.GridView1.RowDeleting +=new GridViewDeleteEventHandler(GridView1_RowDeleting);
            this.btnNewStore.Click += new EventHandler(btnNewStore_Click);
            this.btnSearch.Click += new EventHandler(btnNewStore_Click);
        }

        private DataTable GetData()  
        { 
            try
            {
                string sSql = @"Select count(store_no) 
                               From store
                               Where store_name Like '%" + sSearchStore + "%'";

                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand(sSql, conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("store_name", sSearchStore);
                conn.Open();
                // 共有多少筆資料
                int iRecordCount = (int)cmd.ExecuteScalar();

                Pagination Page = new Pagination(iRecordCount, Request["page"]);
                            
                // 輸出資料並綁定GridView 
                sSql = @"Select * 
                         From store 
                         Where store_name Like '%" + sSearchStore + @"%'" + @"
                            And store_no 
                            Between (Select min(store_no) 
                                From (Select Top {0} store_no From store Where store_name Like '%" + sSearchStore + @"%' Order By store_no Desc))
                            And ( Select min(store_no) 
                                From (Select Top {1} store_no From store Where store_name Like '%" + sSearchStore + @"%' Order By store_no Desc))
                         Order By store_no Desc";

                sSql = Page.FormatSql(sSql);

                dt = new DataTable();

                cmd = new OleDbCommand(sSql, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                
                ViewState["dt_sotre"] = dt;

                if (dt.Rows.Count > 0 )
                {
                    this.GridView1.DataSource = ViewState["dt_sotre"];
                    this.GridView1.DataBind();

                    this.lblPagination.Text = Page.PaginationOut("aStore.aspx", sSearchStore).ToString();
                    this.lblResultStats.Text = "共" + Page.TotalPage + "頁，" + iRecordCount.ToString() + "筆結果";
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    this.GridView1.DataSource = ViewState["dt_sotre"];
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

        private void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // 新增店家
                if (e.CommandName.Equals("AddNew"))
                {
                    string sStoreName = ((TextBox)this.GridView1.FooterRow.Cells[0].FindControl("txtStoreNameFooter")).Text.Trim();
                    string sStoreAddress = ((TextBox)this.GridView1.FooterRow.Cells[0].FindControl("txtAddressFooter")).Text.Trim();
                    string sStorePhone = ((TextBox)this.GridView1.FooterRow.Cells[0].FindControl("txtPhoneFooter")).Text.Trim();
                    string sStoreRemark = ((TextBox)this.GridView1.FooterRow.Cells[0].FindControl("txtRemarkFooter")).Text.Trim();
                    
                    // 驗證資料
                    if (sStoreName == "")
                    {
                        GetData();
                        this.GridView1.FooterRow.Visible = true;
                        this.divNotify.Style.Add("display", "block");
                        this.lblNotify.Text = "您忘記輸入店家名稱了。";
                        return;
                    }
                    // 新增至資料庫
                    string sSql = "Insert Into store(store_name, store_address, store_phone, store_remark) Values (?,?,?,?);";
                    OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    OleDbCommand cmd = new OleDbCommand(sSql, conn);
                    cmd.Parameters.AddWithValue("store_name", sStoreName);
                    cmd.Parameters.AddWithValue("store_address", sStoreAddress);
                    cmd.Parameters.AddWithValue("store_phone", sStorePhone);
                    cmd.Parameters.AddWithValue("store_remark", sStoreRemark);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Cancel();
                    conn.Close();

                    DataTable dt = (DataTable)ViewState["dt_sotre"];
                    // ViewState 新增餐點
                    DataRow row = dt.NewRow();
                    row["store_name"] = sStoreName;
                    row["store_phone"] = sStorePhone;
                    row["store_address"] = sStoreAddress;
                    row["store_remark"] = sStoreRemark;
                    dt.Rows.InsertAt(row, 0);

                    ViewState["dt_sotre"] = dt;

                    GetData();

                } // 取消新增店家
                else if (e.CommandName.Equals("CancelAddNew"))
                {
                    this.GridView1.FooterRow.Visible = false;
                    GetData();
                }
            }
            catch (Exception ex)
            {
                this.divAlert.Style.Add("display", "block");
                this.lblAlert.Text = "Error : " + ex.Message;
            }
        }
        // 刪除店家
        private void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand("Delete From store Where store_no = ?", conn);
                cmd.Parameters.AddWithValue("store_no", this.GridView1.DataKeys[e.RowIndex].Value.ToString());
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Cancel();
                conn.Close();

                GetData();
            }
            catch (Exception ex)
            {
                this.divAlert.Style.Add("display", "block");
                this.lblAlert.Text = "Error : " + ex.Message;
            }
        }
        private void btnNewStore_Click(object sender, EventArgs e)
        {
            if (sender.Equals(this.btnSearch))
            {
                if (this.txtSearchStore.Text.Trim() != "")
                {
                    Response.Redirect("aStore.aspx?page=1&q=" + this.txtSearchStore.Text.Trim());
                }
                else
                {
                    Response.Redirect("aStore.aspx?page=1");
                }
            }// 新增店家，顯示footer
            else if (sender.Equals(this.btnNewStore))
            {
                GetData();
                this.GridView1.FooterRow.Visible = true;
               
            }
        }
    }

}