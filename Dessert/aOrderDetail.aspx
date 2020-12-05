<%@ Page Title="訂單明細┃下午茶後台" Language="C#" MasterPageFile="~/Site_admin.Master" AutoEventWireup="true" CodeBehind="aOrderDetail.aspx.cs" Inherits="Dessert.aOrderDetail_People" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    .form1-input 
    {
        display: inline-block;
        width: 490px;    
    }
    .order-info > div > .form1-control
    {   
        width: 80px;   
    }
    .txtOrderRemark
    {
       width: 400px;
       height: 100px;
    }
    .tabpanel
    {
        margin: 20px 0;    
    }
    /* Table 按人統計 */
    #ContentPlaceHolder1_GridView1 > tbody > tr:nth-child(1) > th:nth-child(2){ width: 80px; }
    #ContentPlaceHolder1_GridView1 > tbody > tr:nth-child(1) > th:nth-child(3),
    #ContentPlaceHolder1_GridView1 > tbody > tr:nth-child(1) > th:nth-child(4),
    #ContentPlaceHolder1_GridView1 > tbody > tr:nth-child(1) > th:nth-child(5){ width: 60px; }
    /* Table 按產品統計 */
    #ContentPlaceHolder1_GridView2 > tbody > tr:nth-child(1) > th:nth-child(3),
    #ContentPlaceHolder1_GridView2 > tbody > tr:nth-child(1) > th:nth-child(4){ width: 60px; }
    #ContentPlaceHolder1_GridView2 > tbody > tr:nth-child(1) > th:nth-child(5){ width: 600px;}
    
    .null-value {
        margin: 24px 10px;
        display: inline-block;
    }
    .export-excel
    {
        text-decoration: underline; 
    }
</style>
<script type="text/javascript">
    $(function () {
        $('.datetimes').daterangepicker({
            timePickerIncrement: 15,        //  每15分鐘為一個選取單位
            minYear: 2019,
            showDropdown: true,
            timePicker: true,
            singleDatePicker: true,
            timePicker24Hour: true,
            locale: {
                format: 'YYYY/MM/DD HH:mm ',
                applyLabel: "確定",
                cancelLabel: "取消",
                daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
                monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
                firstDay: 0
            }
        });
    });        
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="alert alert-danger text-center " id="divAlert" role="alert"  style="display:none" runat="server">
      <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
    </div>
    <form id="form1" runat="server">
        <section>   
            <div class="container">
                <nav aria-label="breadcrumb">
                  <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="aOrder_List.aspx">訂單列表</a></li>
                    <li class="breadcrumb-item active" aria-current="page">訂單明細</li>
                  </ol>
                </nav>
                <h3><asp:Label ID="lblStoreName" runat="server" Text=""></asp:Label></h3>
			    <div class="row">
				    <div class="col-sm-6">
                        <div class="store-info">
                            <div class="form1-group">
                                <div class="form1-control">電話：</div>
                                <asp:Label ID="lblPhone" class="form1-input" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="form1-group">
                                <div class="form1-control">地址：</div>
                                <asp:Label ID="lblAddress" class="form1-input" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="form1-group">
                                <div class="form1-control">備註：</div>
                                <asp:Label ID="lblRemark" class="form1-input" runat="server" Text=""></asp:Label>
                            </div>
				        </div>
                    </div>
			        <div class="col-sm-6 ">
                        <div class="order-info">
                            <div class="form1-group">
                                <div class="form1-control">訂購期間：</div>
                                <asp:TextBox ID="txtStartDate" class="datetimes" runat="server"  required="required"></asp:TextBox> ~ <asp:TextBox ID="txtEndDate" runat="server" class="datetimes" required="required"></asp:TextBox>
                            </div>
                            <div class="form1-group">
                                <div class="form1-control">備註：</div>
                                <asp:TextBox ID="txtOrderRemark" class="form1-input txtOrderRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="text-center"><asp:Button ID="btnSubmit" class="btn-admin-dark btn-size-2x " runat="server" Text="儲存" /></div>
			            </div>
                    </div>
                </div>
		    </div>	
        </section>
        <section class="tabpanel">
            <div class="container"> 
                <div class="orderDetail-tab">
                    <ul class="nav nav-tabs">
                        <li class="nav-item" class="active">
                            <a class="nav-link active" href="#people" data-toggle="tab">按人統計</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#product" data-toggle="tab">按產品統計</a>
                        </li>
                    </ul>
                </div>
                <div class="tab-content">
                    <div id="people" class="tab-pane fade show active">
                        <asp:Label ID="lblPeopleNullValue" class="null-value" runat="server" Text=""></asp:Label>
                        <asp:GridView ID="GridView1" CssClass="table table-striped table-borderless " runat="server" ShowHeaderWhenEmpty="True"></asp:GridView>
                        <asp:Button ID="btnExportExcelPeople" class="btn-admin btn-size-4x export-excel" runat="server" Text="匯出Excel" />
                    </div>
                    <div id="product" class="tab-pane fade">
                        <asp:Label ID="lblProductNullValue" class="null-value" runat="server" Text=""></asp:Label>
                        <asp:GridView ID="GridView2" CssClass="table table-striped table-borderless " runat="server"></asp:GridView>
                        <asp:Button ID="btnExportExcelProduct" class="btn-admin btn-size-4x export-excel" runat="server" Text="匯出Excel" />
                    </div>
                </div>
            </div>
        </section>
    </form>
</asp:Content>
