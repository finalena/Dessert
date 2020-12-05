<%@ Page Title="" Language="C#" MasterPageFile="~/Site_user.Master" AutoEventWireup="true" CodeBehind="uOrdered.aspx.cs" Inherits="Dessert.uOrdered" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
:root{
  --font-color: #666 ; 
}
h3{
    color: #fff;
    padding: 15px;
    background-color: #ffeae2;  
}
h4{
  padding: 0 12px;
  color: var(--font-color);
}
h6{
  color: var(--font-color);
}
#form1 > div.container > hr{
  border-top: 3px solid #ED502E; 
  margin: 24px 0;
}
/* Form */
.invoice-content{
    padding: 0 12px;
}
.form1-group
{
  margin: 16px 0px;
}
.total-field{     /* 垂直置中對齊*/    
  position: relative;
  top: 50%;
  transform: translateY(-50%);
}
#ContentPlaceHolder1_lblStore{
  margin-bottom: 16px;
  font-weight: bold;
  font-size: 24px;
}
/* Tabel */
.filledin-remark{
  color: #707070;
}
.table thead th {
  border: 0;
  border-bottom: 0;
  color: var(--font-color);
}
td:nth-child(1), th:nth-child(1){
  width: 50px;
  text-align: center;
}
td:nth-child(3), th:nth-child(3),
td:nth-child(4), th:nth-child(4){
  width: 100px;
  text-align: center;
}
table > tfoot > tr > td:nth-child(1){
  text-align: right;
}
table > tfoot > tr > td:nth-child(2){
  text-align: center;
}
/* Tabel End */
.nullValue{
  margin: 150px;    
}
.clear-filledin{
    margin-bottom: 32px;
}
#ContentPlaceHolder1_btnClear{
  background-color: transparent;
  border: 0;
  color: #333;
}
#ContentPlaceHolder1_btnClear:hover{
  color: #ED502E;
  text-decoration: none;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="form1" runat="server">
    <div class="container">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="uOrder_List.aspx">下午茶清單</a></li>
                <li class="breadcrumb-item active" aria-current="page">訂單明細</li>
            </ol>
        </nav>
        <!-- Order Info -->   
        <h3><asp:Label ID="lblStore" runat="server" Text=""></asp:Label></h3>
        <div class="invoice-content row">
            <div class="col-9 order-info">
                <div class="form1-group">
                    <h6 class="form1-control">訂購期間</h6>
                    <div class="form1-input">
                        <asp:Label ID="lblStart_date" runat="server" Text=""></asp:Label> ~ <asp:Label ID="lblEnd_date" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="form1-group">
                    <h6 class="form1-control">訂單備註</h6>
                    <div class="form1-input">
                        <asp:Label ID="lblRemark" runat="server" Text=""></asp:Label>
                    </div>
                </div>    
            </div>
            <div class="col-3 text-right order-total">
                <div class="total-field">
                    <h6 class="form1-control">Total</h6>
                    <h2>$<asp:Label ID="lblTotal" runat="server" Text=""></asp:Label></h2>
                </div>
            </div>
        </div>
        <hr />
        <!-- Order Details -->
        <section class="order-detail">
            <asp:Label ID="lblAddFilledIn" runat="server" Text=""></asp:Label>
            <div class="clear-filledin text-center">
                <asp:Button ID="btnClear" runat="server" Text="清除訂購資料" onclientclick="return confirm('您確定要清除所有餐點嗎?')" />         
            </div>
        </section>
    </div>
</form>

</asp:Content>
