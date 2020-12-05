using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Web.UI.HtmlControls;

namespace Dessert
{
    public partial class aOrderDetail_People : System.Web.UI.Page
    {
        string sOrder_no = null;
        DataTable dtPeople = null;
        DataTable dtProduct = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            sOrder_no = Request["order_no"] ?? "";

            if (!IsPostBack)
            {
                Inititle();
                GetFilledInPeople();
                GetFilledInProduct();
                GetOrderTotal();
            }

            HtmlGenericControl li = (HtmlGenericControl)Master.FindControl("aOrderList");
            li.Attributes.Add("class", "active");
            
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
            this.btnExportExcelPeople.Click += new EventHandler(btnSubmit_Click);
            this.btnExportExcelProduct.Click += new EventHandler(btnSubmit_Click);
        }
        // 輸出訂單資料
        private void Inititle() 
        {
            string sSql = "Select * From dessert_order A Left Join store B On A.store_no = B.store_no Where A.order_no = ?";

            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand(sSql, conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("order_no", sOrder_no);
           
            conn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                this.lblStoreName.Text = dr["store_name"].ToString();
                this.lblPhone.Text = dr["store_phone"].ToString();
                this.lblAddress.Text = dr["store_address"].ToString();
                this.lblRemark.Text = dr["store_remark"].ToString();
                this.txtStartDate.Text = dr["start_date"].ToString();
                this.txtEndDate.Text = dr["end_date"].ToString();
                this.txtOrderRemark.Text = dr["order_remark"].ToString();
            }
            else
            {
                Server.Transfer("~/ErrorPages/aOops.aspx");
            }
                
            cmd.Cancel();
            dr.Close();
        }

        // 按人統計
        private DataTable GetFilledInPeople() 
        {
            string sSql = @"Transform SUM(A.item_price*A.quantity) as subprice
                            Select B.employee_name, B.ext, SUM(A.quantity) As quantity, SUM(subprice) As subtotal
                            From filledIn A Left Join employee B On A.employee_no = B.employee_no
                            Where A.order_no = ?
                            Group by B.employee_name, B.ext
                            Pivot A.item_name";
            
            dtPeople = new DataTable();
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand(sSql, conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("order_no", sOrder_no);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dtPeople);

            string item = "";
            dtPeople.Columns.Add("#", System.Type.GetType("System.Int32")).SetOrdinal(0);
            dtPeople.Columns.Add("items", Type.GetType("System.String")).SetOrdinal(5);   
            
            // 如items後面欄位有資料，將欄位名稱(品名)儲存到字串中
            for (int intA = 0; intA < dtPeople.Rows.Count; intA++)
            {
                item = "";
                for (int intB = 6; intB < dtPeople.Columns.Count ; intB++)
                {
                    if (!dtPeople.Rows[intA].IsNull(dtPeople.Columns[intB]))
                    {
                        item += dtPeople.Columns[intB].ColumnName + ", ";
                    }
                }
                // 去除最後的, 符號 ，並將字串新增到items下的儲存格
                dtPeople.Rows[intA]["items"] = item.Substring(0, item.Length - 2);    
                // 序號
                dtPeople.Rows[intA]["#"] = intA + 1;  
            }
            // 刪除items後面的欄位，從最後一欄開始刪除
            for (int intA = dtPeople.Columns.Count -1 ; intA > 5; intA--)
            {
                dtPeople.Columns.RemoveAt(intA);
            }
            
            dtPeople.Columns[1].ColumnName = "訂購人";
            dtPeople.Columns[2].ColumnName = "分機";
            dtPeople.Columns[3].ColumnName = "數量";
            dtPeople.Columns[4].ColumnName = "總共";
            dtPeople.Columns[5].ColumnName = "品名";
    
            if (dtPeople.Rows.Count > 0)
            {
                DataRow row = dtPeople.NewRow();
                row[5] = GetOrderTotal();
                dtPeople.Rows.Add(row);

                this.GridView1.DataSource = dtPeople;
                this.GridView1.DataBind();
            }
            else
            {
                this.lblProductNullValue.Text = "查無資料";
            }
           
            return dtPeople;
        }
        // 按產品統計
        private DataTable GetFilledInProduct()
        {
            string sSql = @"Select Distinct A.item_name, SUM(A.quantity) As quantity, A.item_price 
                            From filledIn A
                            Where A.order_no = " + sOrder_no + @"
                            Group By A.item_name, A.item_price";
            
            string sSql2 = @"Select A.item_name, B.employee_name, A.filledIn_remark
                             From filledIn A Left Join employee B On A.employee_no = B.employee_no 
                             Where order_no = " + sOrder_no + @"
                             Order By A.item_name DESC;";

            dtProduct = new DataTable();
            DataTable dtTemp = new DataTable();
            
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbDataAdapter da = new OleDbDataAdapter(sSql, conn);
            OleDbDataAdapter da2 = new OleDbDataAdapter(sSql2, conn);
            da.Fill(dtProduct);
            da2.Fill(dtTemp);
            dtProduct.Columns.Add("#", System.Type.GetType("System.Int32")).SetOrdinal(0);
            dtProduct.Columns.Add("items", Type.GetType("System.String"));
            

            string sItems = "";
            for (int intA = 0; intA < dtTemp.Rows.Count; intA++)
            {
                // 使用者如有填備註，以 姓名(XXXX) 呈現
                if (dtTemp.Rows[intA]["filledIn_remark"].ToString() == "")
                {
                    sItems = dtTemp.Rows[intA]["employee_name"].ToString();
                }
                else
                {
                    sItems = dtTemp.Rows[intA]["employee_name"].ToString() + "(" + dtTemp.Rows[intA]["filledIn_remark"].ToString() + ")";
                }
                // 加到dt_Product的items欄位，產品有多位使用者訂購以空白鍵區隔
                for (int intB = 0; intB < dtProduct.Rows.Count; intB++)
                {
                    if (dtProduct.Rows[intB]["item_name"].ToString() == dtTemp.Rows[intA]["item_name"].ToString())
                    {
                        dtProduct.Rows[intB]["items"] += sItems + "  ";
                    }
                    // 序號
                    dtProduct.Rows[intB]["#"] = intB + 1;  
                }
            }

            dtProduct.Columns[1].ColumnName = "品名";
            dtProduct.Columns[2].ColumnName = "數量";
            dtProduct.Columns[3].ColumnName = "單價";
            dtProduct.Columns[4].ColumnName = "訂購人(備註)";

            if (dtProduct.Rows.Count > 0)
            {
                // 表尾訂單資料
                DataRow row = dtProduct.NewRow();
                row[4] =  GetOrderTotal();
                dtProduct.Rows.Add(row);

                this.GridView2.DataSource = dtProduct;
                this.GridView2.DataBind();
            }
            else
            {
                this.lblPeopleNullValue.Text = "查無資料";
            }
            return dtProduct;
        }

        // 統計訂單資料
        private string GetOrderTotal()
        {
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand("Select Count(employee_no) From (Select Distinct employee_no From filledIn Where order_no = " + sOrder_no + ")", conn);
            conn.Open();
            string sOrderTotal = "訂購人數 : " + cmd.ExecuteScalar();

            cmd = new OleDbCommand("Select Sum(quantity) As num, Sum(quantity*item_price) As total From filledIn Where order_no = " + sOrder_no + " Group By order_no", conn);
            OleDbDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                sOrderTotal += " 總數量 : " + dr["num"].ToString() + " 總金額 : " + dr["total"].ToString();
            }

            cmd.Cancel();
            dr.Close();

            return sOrderTotal;
        }

        private void ExportExcel(DataTable dt)
        {
            // 1.匯出到Excel
            Excel.Application MyExcel = new Excel.Application();
            Excel.Workbook MyBook = MyExcel.Workbooks.Add();
            Excel.Worksheet MySheet = MyBook.Worksheets[1];

            int iRowCnt = 1;

            // 2.EXCEL欄位
            for (int intA = 0; intA < dt.Columns.Count; intA++)
            {
                MySheet.Cells[iRowCnt, intA + 1].Value = dt.Columns[intA].ColumnName;
            }

            // 3.EXCEL資料
            for (int intA = 0; intA < dt.Rows.Count; intA++)
            {
                iRowCnt++;
                for (int intB = 0; intB < dt.Columns.Count; intB++)
                {
                    MySheet.Cells[iRowCnt, intB + 1].Value = dt.Rows[intA][intB];
                }
            }

            // 4.EXCEL格式
            MySheet.Name = "下午茶";
            MySheet.Cells.Font.Name = "Calibri";
            Excel.Range Range = MySheet.Range[MySheet.Cells[1, 1], MySheet.Cells[iRowCnt, dt.Columns.Count]];
            Range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            Range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;     
            Range.EntireColumn.AutoFit();          

            MyExcel.Visible = true;
            Marshal.ReleaseComObject(MyExcel);
            MyExcel = null;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // 更新訂單
            if (sender.Equals(this.btnSubmit))
            {
                string sSql = "Update dessert_order Set start_date=?, end_date=?, order_remark=? Where order_no =?";
                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                OleDbCommand cmd = new OleDbCommand(sSql, conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("start_date", this.txtStartDate.Text);
                cmd.Parameters.AddWithValue("end_date", this.txtEndDate.Text);
                cmd.Parameters.AddWithValue("order_remark", this.txtOrderRemark.Text);
                cmd.Parameters.AddWithValue("order_no", sOrder_no);
                
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Cancel();
                    conn.Close();

                }
                catch (Exception ex)
                {
                    this.divAlert.Style.Add("display", "block");
                    this.lblAlert.Text = "Error : " + ex.Message;
                }
            }
            else if (sender.Equals(this.btnExportExcelPeople))
            {
                GetFilledInPeople();
                ExportExcel(dtPeople);
            }
            else if (sender.Equals(this.btnExportExcelProduct))
            {
                GetFilledInProduct();
                ExportExcel(dtProduct);
            }
        }
        
    }
}