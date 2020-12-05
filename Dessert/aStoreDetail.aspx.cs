
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.UI.HtmlControls;

namespace Dessert
{
    public partial class aStoreDetail : System.Web.UI.Page
    {
        DataTable dt = null;
        DataTable dt2 = null;
        string sStore_no = null;

        protected void Page_Load(object sender, EventArgs e)
        {   
            sStore_no = Request["store_no"] ?? "";
            
            if (!IsPostBack)
            {
                GetStoreData();
                GetStoreItem();
                GetCategory();
            }

            this.divNotify.Style.Add("display", "none");
            
            HtmlGenericControl li = (HtmlGenericControl)Master.FindControl("aStore");
            li.Attributes.Add("class", "active");
            
            Page.Title = this.txtStore.Text + "：店家管理┃下午茶後台";
            
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
            this.btnCategoryMgt.Click += new EventHandler(btnSubmit_Click);
            this.ddlCategory.SelectedIndexChanged += new EventHandler(ddlCategory_SelectedIndexChanged);
            this.GridView1.RowCommand += new GridViewCommandEventHandler(GridView1_RowCommand);
            this.GridView1.RowDeleting += new GridViewDeleteEventHandler(GridView1_RowDeleting);
            this.GridView1.RowEditing += new GridViewEditEventHandler(GridView1_RowEditing);
            this.GridView1.RowCancelingEdit += new GridViewCancelEditEventHandler(GridView1_RowCancelingEdit);
            this.GridView1.RowUpdating += new GridViewUpdateEventHandler(GridView1_RowUpdating);
            this.GridView1.RowDataBound += new GridViewRowEventHandler(GridView1_RowDataBound);
        }
        // 取得店家基本資料
        private void GetStoreData()
        {   
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand("Select * From store Where store_no = ?", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("aStore_no", sStore_no);
            conn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            dr.Read();

            if (dr.HasRows)
            {
                this.txtStore.Text = dr.GetString(1);
                this.txtPhone.Text = dr.GetString(2);
                this.txtAddress.Text = dr.GetString(3);
                this.txtRemark.Text = dr.GetString(4);
            }
            else
            {
                Server.Transfer("~/ErrorPages/aOops.aspx");
            }

            cmd.Cancel();
            dr.Close();

        }
        // 抓取店家產品
        private DataTable GetStoreItem()
        { 
            dt = new DataTable();

            string sSql = @"Select A.item_no, B.itemCategory_no, B.itemCategory_name, a.item_name, a.item_price
                             From ((item AS A 
                             Left Join item_category AS B ON A.itemCategory_no = B.itemCategory_no) 
                             Left Join store AS C ON A.store_no = C.store_no) 
                             Where A.store_no = ?
                             Order By A.item_no Desc";

            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand(sSql, conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("store_no", sStore_no);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dt);

            ViewState["dt"] = dt;
            BindItem();

            return dt;
        }        
        // 綁定店家餐點資料
        private void BindItem()
        {
            DataTable dt = (DataTable)ViewState["dt"];

            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
                this.GridView1.DataSource = ViewState["dt"];
                this.GridView1.DataBind();
                this.GridView1.Rows[0].Cells.Clear();
                this.GridView1.Rows[0].Cells.Add(new TableCell());
                this.GridView1.Rows[0].Cells[0].ColumnSpan = this.GridView1.Columns.Count;
                this.GridView1.Rows[0].Cells[0].Text = "查無資料...馬上新增!";
                this.GridView1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                this.GridView1.DataSource = ViewState["dt"];
                this.GridView1.DataBind();
            }
        }
        private void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 下拉選單不是選到'全'
            if (this.ddlCategory.SelectedValue.ToString() != "0")
            {
                DataTable dt = (DataTable)ViewState["dt"];
                dt.Clear();

                this.ddlCategory.Text = this.ddlCategory.SelectedValue.ToString();

                string sSql = @"Select A.item_no, B.itemCategory_no, B.itemCategory_name, A.item_name, A.item_price 
                                From ((item AS A 
                                Left Join item_category AS B ON A.itemCategory_no = B.itemCategory_no) 
                                Left Join store AS C ON A.store_no = C.store_no) 
                                Where A.store_no = ? And B.itemCateGory_no = ?
                                Order By A.item_no Desc";

                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand(sSql, conn);
                cmd.Parameters.AddWithValue("store_no", sStore_no);
                cmd.Parameters.AddWithValue("itemCategory_no", this.ddlCategory.SelectedValue.ToString());

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                ViewState["dt"] = dt;
                BindItem();

                GetCategory();
            } 
            else
            {
                GetStoreItem();
                GetCategory();
            }
        }
        // 取得餐點類別，綁定dropdownlist
        private DataTable GetCategory() 
        {
            dt2 = new DataTable();
            string sSql2 = "Select itemCategory_no, itemCategory_name From item_category Where store_no = ? Order By itemCategory_no";

            try
            {
                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand(sSql2, conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("store_no", sStore_no);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt2);

                this.ddlCategory.DataSource = dt2;
                this.ddlCategory.DataTextField = "itemCategory_name";
                this.ddlCategory.DataValueField = "itemCategory_no";
                this.ddlCategory.DataBind();
                this.ddlCategory.Items.Insert(0, "全");
                this.ddlCategory.Items[0].Value = "0";

                ((DropDownList)this.GridView1.FooterRow.FindControl("ddlCategoryFooter")).DataSource = dt2;
                ((DropDownList)this.GridView1.FooterRow.FindControl("ddlCategoryFooter")).DataTextField = "itemCategory_name";
                ((DropDownList)this.GridView1.FooterRow.FindControl("ddlCategoryFooter")).DataValueField = "itemCategory_no";
                ((DropDownList)this.GridView1.FooterRow.FindControl("ddlCategoryFooter")).DataBind();
            
            }
            catch (Exception ex)
            {
                this.divAlert.Style.Add("display", "block");
                this.lblAlert.Text = "Error : " + ex.Message;
            }

            return dt2;
        }
        // 編輯餐點的dropdownlist塞入餐點類別
        private void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState.ToString().Contains("Edit"))
                {
                    GetCategory();
                    ((DropDownList)e.Row.FindControl("ddlCategoryEdit")).DataSource = dt2;
                    ((DropDownList)e.Row.FindControl("ddlCategoryEdit")).DataTextField = "itemCategory_name";
                    ((DropDownList)e.Row.FindControl("ddlCategoryEdit")).DataValueField = "itemCategory_no";                    
                    ((DropDownList)e.Row.FindControl("ddlCategoryEdit")).DataBind();
                    // 編輯前的類別(lblCategory_no)與下拉選單連動
                    ((DropDownList)e.Row.FindControl("ddlCategoryEdit")).SelectedValue = ((Label)e.Row.FindControl("lblCategory_no")).Text;
                } 
            }
        }
        // 刪除資料
        private void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string sSql = "Delete From item Where item_no = ?";

            try
            {
                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand(sSql, conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("item_no", this.GridView1.DataKeys[e.RowIndex].Value.ToString());
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Cancel();
                conn.Close();
                
                // 重新綁定資料
                DataTable dt = (DataTable)ViewState["dt"];
                dt.Rows.RemoveAt(e.RowIndex);
              
                ViewState["dt"] = dt;
                BindItem();
                
                GetCategory();
            }
            catch (Exception ex)
            {
                this.divAlert.Style.Add("display", "block");
                this.lblAlert.Text = "Error : " + ex.Message;
            } 
        }
        // 編輯資料
        private void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.GridView1.EditIndex = e.NewEditIndex;

            BindItem();
            GetCategory();
        }       
        // 取消編輯
        private void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView1.EditIndex = -1;

            BindItem();
            GetCategory();
        }
        // 更新餐點資料
        private void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string sSql = "Update item Set itemCategory_no = ?, item_name = ?, item_price = ? where item_no = ?";
           
            try
            {   // 抓取修改後的值
                string sCategoryNo = ((DropDownList)this.GridView1.Rows[e.RowIndex].Cells[1].FindControl("ddlCategoryEdit")).Text.Trim();
                string sCategoryName = ((DropDownList)this.GridView1.Rows[e.RowIndex].Cells[1].FindControl("ddlCategoryEdit")).SelectedItem.Text;
                string sItemName = ((TextBox)this.GridView1.Rows[e.RowIndex].Cells[2].FindControl("txtItemName")).Text.Trim();
                string sItemPrice = ((TextBox)this.GridView1.Rows[e.RowIndex].Cells[3].FindControl("txtItemPrice")).Text.Trim();
                sItemPrice = Regex.Replace(sItemPrice, @"\b\s+\b", " ");        //字串中間多餘的空格去掉，只保留一個空格
                
                // 驗證資料
                string sErrMsg = Verify(sItemName, sItemPrice);
                
                if (sErrMsg != "")
                {
                    this.divNotify.Style.Add("display", "block");
                    this.lblNotify.Text = sErrMsg;
                    return;
                }
                // 更新資料庫
                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand(sSql, conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("itemCategory_no", sCategoryNo);
                cmd.Parameters.AddWithValue("item_name", sItemName);
                cmd.Parameters.AddWithValue("item_price", sItemPrice);
                cmd.Parameters.AddWithValue("item_no", this.GridView1.DataKeys[e.RowIndex].Value.ToString());       //取得被選取列的主鍵
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Cancel();
                conn.Close();
                // 更新ViewState的dt
                DataTable dt = (DataTable)ViewState["dt"];
                GridViewRow row = this.GridView1.Rows[e.RowIndex];
                dt.Rows[row.DataItemIndex]["itemCategory_no"] = sCategoryNo;
                dt.Rows[row.DataItemIndex]["itemCategory_name"] = sCategoryName;
                dt.Rows[row.DataItemIndex]["item_name"] = sItemName;
                dt.Rows[row.DataItemIndex]["item_price"] = sItemPrice;
                // 離開編輯模式
                this.GridView1.EditIndex = -1;
                
                ViewState["dt"] = dt;
                BindItem();
                GetCategory();
            }
            catch (Exception ex)
            {
                this.divAlert.Style.Add("display", "block");
                this.lblAlert.Text = "Error : " + ex.Message;
            }
        }
        // 新增餐點
        private void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNewItem"))
            {
                try
                {
                    string sCategory_no = ((DropDownList)this.GridView1.FooterRow.FindControl("ddlCategoryFooter")).Text;
                    string sItemName = ((TextBox)this.GridView1.FooterRow.FindControl("txtItemNameFooter")).Text.Trim();
                    string sItemPrice = ((TextBox)this.GridView1.FooterRow.FindControl("txtItemPriceFooter")).Text.Trim();
                    sItemPrice = Regex.Replace(sItemPrice, @"\b\s+\b", " ");        // 字串中間多餘的空格去掉，只保留一個空格
                    
                    DataTable dt = (DataTable)ViewState["dt"];

                    // 防止dt在無資料時，GridView1產生一行空白行
                    if (this.GridView1.Rows[0].Cells[0].Text == "查無資料...馬上新增!")
                    {
                        dt.Clear();
                        ViewState["dt"] = dt;
                    }

                    // 驗證資料
                    string sErrMsg = Verify(sItemName, sItemPrice);
                
                    if (sCategory_no == "")
                    {
                        BindItem();
                        GetCategory();
                        this.divNotify.Style.Add("display", "block");
                        this.lblNotify.Text = "請先新增餐點類別。";
                        return;
                    }
                    else if (sErrMsg != "")
                    {
                        BindItem();
                        GetCategory();
                        this.divNotify.Style.Add("display", "block");
                        this.lblNotify.Text = sErrMsg;
                        return;
                    }

                    // 新增至資料庫
                    string sSql = "Insert Into item(store_no, itemCategory_no, item_name, item_price) Values(?,?,?,?)";
                    OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    OleDbCommand cmd = new OleDbCommand(sSql, conn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("store_no", sStore_no);
                    cmd.Parameters.AddWithValue("itemCategory_no", sCategory_no);
                    cmd.Parameters.AddWithValue("item_name", sItemName);
                    cmd.Parameters.AddWithValue("item_price", sItemPrice);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    cmd = new OleDbCommand("Select item_no From item Order By item_no Desc", conn);
                    int iItem_no = (int)cmd.ExecuteScalar();

                    cmd.Cancel();
                    conn.Close();

                    // ViewState 新增餐點
                    DataRow row = dt.NewRow();
                    row["item_no"] = iItem_no;
                    row["itemCategory_no"] = sCategory_no;
                    row["itemCategory_name"] = ((DropDownList)this.GridView1.FooterRow.FindControl("ddlCategoryFooter")).SelectedItem.Text;
                    row["item_name"] = sItemName;
                    row["item_price"] = sItemPrice;
                    dt.Rows.InsertAt(row, 0);

                    ViewState["dt"] = dt;

                    BindItem();
                    GetCategory();
                }
                catch (Exception ex)
                {
                    this.divAlert.Style.Add("display", "block");
                    this.lblAlert.Text = "Error : " + ex.Message;
                }
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // 更新店家基本資料
            if (sender.Equals(this.btnSubmit))
            {
                string sSql = "Update store Set store_name = ?, store_phone = ?, store_address = ?, store_remark = ? Where store_no = ?";

                try
                {
                    OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    OleDbCommand cmd = new OleDbCommand(sSql, conn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("store_name", this.txtStore.Text.Trim());
                    cmd.Parameters.AddWithValue("store_phone", this.txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("store_address", this.txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("store_remark", this.txtRemark.Text.Trim());
                    cmd.Parameters.AddWithValue("store_no", sStore_no);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Cancel();
                    conn.Close();

                    BindItem();
                    GetCategory();
                }
                catch (Exception ex)
                {
                    this.divAlert.Style.Add("display", "block");
                    this.lblAlert.Text = "Error : " + ex.Message;
                }
            }
            // 跳轉至餐點類別頁面
            else if (sender.Equals(this.btnCategoryMgt))
            {
                Response.Redirect("aItemCategory_Mgt.aspx?store_no=" + sStore_no);
            }
        }
        
        /// <summary>
        /// 驗證餐點資料 
        /// </summary>
        /// <param name="sItemName"></param>
        /// <param name="sItemPrice"></param>
        /// <returns></returns>
        private string Verify(string sItemName, string sItemPrice)
        {
            string[] arrItemPrice = sItemPrice.Split(' ');

            if (sItemName == "" || sItemPrice == "")
            {
                return "欄位不可為空。";
            }
            else if (arrItemPrice.Length == 1)
            {
                if (!IsNumeric(arrItemPrice[0]))
                {
                    return "請檢查金額欄位是否少打產品細項或價錢。";
                }
                return "";
            }
            else
            {
                if (arrItemPrice.Length % 2 != 0)
                {
                    return "請檢查金額欄位是否少打產品細項或價錢。";
                }
                else
                {
                    // 陣列奇數欄須為正整數
                    for (int intA = 1; intA <= arrItemPrice.Length; intA += 2)
                    {
                        if (!IsNumeric(arrItemPrice[intA]))
                        {
                            return "價錢須為大於0的數字。";
                        }
                    }
                }
            }

            return "";
        }
        // 判斷是否為正整數
        public bool IsNumeric(String strNumber)
        {
            Regex NumberPattern = new Regex("^[0-9]*[1-9][0-9]*$");
            return NumberPattern.IsMatch(strNumber);
        }
    }
}