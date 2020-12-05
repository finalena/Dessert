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
    public partial class aDessertCategory_New : System.Web.UI.Page
    {
        DataTable dt = null;
        string sStore_no = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            sStore_no = Request["store_no"] ?? "";

            if (!IsPostBack)
            {
                GetData();
            }

            this.divNotify.Style.Add("display", "none");            
            
            HtmlGenericControl li = (HtmlGenericControl)Master.FindControl("aStore");
            li.Attributes.Add("class", "active");
            
            this.GridView1.RowCommand += new GridViewCommandEventHandler(GridView1_RowCommand);
            this.GridView1.RowEditing += new GridViewEditEventHandler(GridView1_RowEditing);
            this.GridView1.RowCancelingEdit += new GridViewCancelEditEventHandler(GridView1_RowCancelingEdit);
            this.GridView1.RowUpdating += new GridViewUpdateEventHandler(GridView1_RowUpdating);
            this.GridView1.RowDeleting += new GridViewDeleteEventHandler(GridView1_RowDeleting);
        }
        // 取得類別並綁定GridView1
        private DataTable GetData()
        {
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand("Select store_name From store Where store_no =? ", conn);
            cmd.Parameters.AddWithValue("store_no", sStore_no);
            
            conn.Open();
            string sStoreName = (string)cmd.ExecuteScalar();
            
            if (sStoreName == null)
            {
                Server.Transfer("~/ErrorPages/aOops.aspx");
            }

            Page.Title = sStoreName + "：類別管理┃下午茶後台";
            this.lblStoreName.Text = sStoreName;

            dt = new DataTable();
            cmd = new OleDbCommand("Select * From item_category Where store_no = ? Order By itemCategory_no", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("store_no", sStore_no);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dt);
            
            if (dt.Rows.Count > 0)
            {
                this.GridView1.DataSource = dt;
                this.GridView1.DataBind();
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
            }
            return dt;
        }
        // 新增餐點類別至資料庫
        private void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                string sCategoryName = (this.GridView1.FooterRow.FindControl("txtCategoryNameFooter") as TextBox).Text.Trim();
                
                if (sCategoryName == "")
                {
                    GetData();
                    this.divNotify.Style.Add("display", "block");
                    this.lblNotify.Text = "您忘記輸入類別名稱了。";
                    return;
                } 
                    
                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand("Insert Into item_category(itemCategory_name, store_no) Values(?,?)", conn);
                cmd.Parameters.AddWithValue("itemCategory_name", sCategoryName);
                cmd.Parameters.AddWithValue("store_no", sStore_no);
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Cancel();
                conn.Close();

                GetData();
            }
        }
        // 刪除餐點類別
        private void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand("Delete From item_category Where itemCategory_no = ?", conn);
            cmd.Parameters.AddWithValue("itemCategory_no", this.GridView1.DataKeys[e.RowIndex].Value.ToString());
            
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Cancel();
                conn.Close();
                this.GridView1.EditIndex = -1;
                GetData();
            }
            catch (Exception ex)
            {
                this.divAlert.Style.Add("display", "block");
                this.lblAlert.Text = "Error : " + ex.Message;
            }
        }
        // 編輯類別
        private void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.GridView1.EditIndex = e.NewEditIndex;
            GetData();
        }
        // 取消編輯
        private void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView1.EditIndex = -1;
            GetData();
        }
        // 更新類別
        private void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string sCategoryName = (this.GridView1.Rows[e.RowIndex].FindControl("txtCategoryName") as TextBox).Text.Trim();
            if (sCategoryName == "")
            {
                this.divNotify.Style.Add("display", "block");
                this.lblNotify.Text = "您忘記輸入類別名稱了。";
                return;
            } 
                    
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand("Update item_category Set itemCategory_name= ? Where itemCategory_no = ?", conn);
            cmd.Parameters.AddWithValue("itemCategory_name", sCategoryName);
            cmd.Parameters.AddWithValue("itemCategory_no", this.GridView1.DataKeys[e.RowIndex].Value.ToString());
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.Cancel();
            conn.Close();
            
            this.GridView1.EditIndex = -1;
            GetData();
        }
    }
}