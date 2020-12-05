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
using HtmlAgilityPack;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Dessert
{
    public partial class uOrder : System.Web.UI.Page
    {
        DataSet ds = null;
        string sOrder_no = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            sOrder_no = Request["order_no"] ?? "";

            if (!IsPostBack)
            {
                Inititle();
                GetMenu(); 
            }

            Page.Title = this.lblStore.Text + "：菜單┃下午茶";
        }
        // 載入訂單資訊
        private void Inititle()
        {
            string sSql = @"Select B.store_name, A.start_date, A.end_date, A.order_remark
                            From dessert_order A 
                            Left Join store B On A.store_no = B.store_no 
                            where order_no = ? ";

            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbCommand cmd = new OleDbCommand(sSql, conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("order_no",sOrder_no);
            conn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                DateTime dtStartDate = Convert.ToDateTime(dr.GetString(1));
                DateTime dtEndDate = Convert.ToDateTime(dr.GetString(2));
                // 訂單沒超過期限才輸出資料
                if (DateTime.Now.CompareTo(dtStartDate) >= 0 && DateTime.Now.CompareTo(dtEndDate) <= 0)
                {
                    this.lblStore.Text = dr.GetString(0);
                    this.lblDate.Text = dr.GetString(1) + " ~ " + dr.GetString(2);
                    if (dr.GetString(3).Trim().Length != 0)
                    {
                        this.lblAddOrderRemark.Text = "<span class=\"order-remark\">" + dr.GetString(3) + "</span>";
                        this.lblAddIcon.Text = "<i class=\"icon-bullhorn\" style=\"cursor: pointer;\" data-toggle=\"modal\" data-target=\"#OrderRemarkModal\"></i>";
                    }
                }
                else
                {
                    Response.Write("<script language='JavaScript'>alert('訂單已截止!');location.href='uOrder_List.aspx';</script>"); 
                }
            }
            else
            {
                Server.Transfer("~/ErrorPages/uOops.aspx");
            }

            cmd.Cancel();
            dr.Close();
        }
        // 載入店家商品
        private DataSet GetMenu()
        {
            string sSql = @"Select A.itemCategory_no, A.itemCategory_name
                            From item_category A
                            Left Join dessert_order B On A.store_no = B.store_no 
                            Where B.order_no =" + sOrder_no + @"
                            Order By A.itemCategory_no;";

            string sSql2 = @"Select A.itemCategory_no, A.item_no, A.item_name, A.item_price
                            From item A
                            Left Join dessert_order B On A.store_no = B.store_no 
                            Where B.order_no =" + sOrder_no + @"
                            Order By A.item_no;";

            ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            StringBuilder sbCat = new StringBuilder();

            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbDataAdapter da = new OleDbDataAdapter(sSql, conn);
            OleDbDataAdapter da2 = new OleDbDataAdapter(sSql2, conn);

            da.Fill(ds, "itemCategory");
            da2.Fill(ds, "item");

            ds.Relations.Add("Relation_item", ds.Tables["itemCategory"].Columns["itemCategory_no"], ds.Tables["item"].Columns["itemCategory_no"]);
            int iItemCount = 0;
            int iSer = 0;
            
            sbCat.AppendLine("<div class=\"menu-control-holder\">");
            sbCat.AppendLine("<ul>");
            // 輸出類別
            foreach (DataRow dRow in ds.Tables["itemcategory"].Rows)
            {
                sb.Append("<div id=\"cat" + dRow["itemCategory_no"].ToString() + "\" class=\"category-items scrollspy \">");
                sb.Append("<h5 class=\"category-title text-center\">" + dRow["itemCategory_name"].ToString() + "</h5>");
                sb.Append("<ul class=\"item-list\">");
                
                sbCat.AppendFormat("<li><a href=\"#cat{0}\">{1}</a></li>", dRow["itemCategory_no"].ToString(), dRow["itemCategory_name"].ToString());
                // 輸出品名
                foreach (DataRow dRow2 in dRow.GetChildRows(ds.Relations["Relation_item"]))
                {
                    iItemCount++;
                    iSer = 0;
                    sb.Append("<li class=\"dish\">");
                    sb.Append("<div class=\"dish-react-root\">");
                    sb.Append("<h6 class=\"dish-name\">" + dRow2["item_name"].ToString() + "</h6>");
                    sb.Append("<div class=\"dish-price-add \">");
                    sb.Append("<apan class=\"product-price\">");
                    string[] arrPrice = dRow2["item_price"].ToString().Split(' ');

                    // 商品無選項只有價錢 <input type="hidden" />
                    if (arrPrice.Length == 1)
                    {
                        sb.Append("<label><input type=\"hidden\" name=\"" + iItemCount + "\" value=\"" + arrPrice[0] + "\" data-serial=\"" + iSer + "\" data-item-name=\"" + dRow2["item_name"].ToString() + "\" data-other=\"\"/>" + arrPrice[0] + "元</label>");
                        sb.Append("<button type=\"button\" id=\"" + iItemCount + "\" class=\"btn btn-basket btn-sm\" onclick='getName(this)'><i class=\"icon-basket-1\"></i></button>");
                    }// 商品有一個細項及價錢 <input type="hidden" />
                    else if (arrPrice.Length == 2)
                    {
                        sb.Append("<label><input type=\"hidden\" name=\"" + iItemCount + "\" value=\"" + arrPrice[1] + "\" data-serial=\"" + iSer + "\" data-item-name=\"" + dRow2["item_name"].ToString() + "\" data-other=\"" + arrPrice[0] + "\"/>" + arrPrice[0] + " " + arrPrice[1] + "元</label>");
                        sb.Append("<button type=\"button\" id=\"" + iItemCount + "\" class=\"btn btn-basket btn-sm\" onclick='getName(this)'><i class=\"icon-basket-1\"></i></button>");
                    }// 商品有一個以上選項<input type="radio" />
                    else
                    {
                        for (int intA = 0; intA < arrPrice.Length; intA += 2)
                        {
                            iSer++;
                            sb.Append("<label><input type=\"radio\" name=\"" + iItemCount + "\" value=\"" + arrPrice[intA + 1] + "\" data-serial=\"" + iSer + "\" data-item-name=\"" + dRow2["item_name"].ToString() + "\" data-other=\"" + arrPrice[intA] + "\"/>" + arrPrice[intA] + " " + arrPrice[intA + 1] + "元</label>");
                        }
                        sb.Append("<button type=\"button\" id=\"" + iItemCount + "\" class=\"btn btn-basket btn-sm\" onclick='getName(this)'><i class=\"icon-basket-1\"></i></button>");
                    }
                       
                    sb.Append("</span>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append("</div>");

            }
            sbCat.AppendLine("</ul>");
            sbCat.Append("</div>");

            this.lblAddMenu.Text = sb.ToString();
            this.lblAddCategorylist.Text = sbCat.ToString();

            return ds;
        }
    }
}