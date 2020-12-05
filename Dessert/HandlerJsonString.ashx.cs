using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace Dessert
{
    /// <summary>
    ///HandlerJsonString 的摘要描述
    /// </summary>
    public class HandlerJsonString : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {   
        string sOrder_no = null;
    
        public void ProcessRequest(HttpContext context)
        {
            sOrder_no = context.Request.QueryString["order_no"];
            // 傳入的資料
            string order = context.Request["order"] ?? string.Empty;
            // 反序列化資料:將JSON格式轉換成物件
            List<Data> data = JsonConvert.DeserializeObject<List<Data>>(order);  

            // 訂單資料存入資料庫
            string sSql = @"Insert Into filledIn(order_no, employee_no, filledin_date, item_name, quantity, item_price, filledIn_remark) Values(?,?,?,?,?,?,?)";
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(sSql, conn);
            for (int intA = 0; intA < data.Count; intA++)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("order_no", sOrder_no);
                cmd.Parameters.AddWithValue("employee_no", context.Session["employee_no"].ToString());
                cmd.Parameters.AddWithValue("filledin_date", DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                cmd.Parameters.AddWithValue("item_name", data[intA].Name);
                cmd.Parameters.AddWithValue("quantity", data[intA].Quantity);
                cmd.Parameters.AddWithValue("item_price", data[intA].Price);
                cmd.Parameters.AddWithValue("filledIn_remark", data[intA].Remark);
                cmd.ExecuteNonQuery();
            }

            cmd.Cancel();
            conn.Close();

            // 回傳資料並序列化成JSON 
            context.Response.Write(JsonConvert.SerializeObject(data));
        }

        public class Data
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
            public string Price { get; set; }
            public string Remark { get; set; }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}