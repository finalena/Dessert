<%@ Page Title="新增訂單┃下午茶後台" Language="C#" MasterPageFile="~/Site_admin.Master" AutoEventWireup="true" CodeBehind="aOrder_New.aspx.cs" Inherits="Dessert.aOrder_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    #form1
    {
        padding: 0 25%;    
    }
    .form1-control
    {
        width: 80px;    
    }
    .form1-input
    {
       width: 350px;    
    }
    .txtRemark
    {
        height: 150px;   
    }
    #ContentPlaceHolder1_btnSubmit
    {
        margin: 0 40%; 
    }
</style>
<script type="text/javascript">
    $(function () {
        $('.datetimes').daterangepicker({
            startDate: moment().startOf('hour'),
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
        <div class="alert alert-primary alert-dismissible fade show text-center divNotify" id="divNotify" role="alert" style="display: none" runat="server">
            成功開放一筆訂單~<a href="aOrder_List.aspx" class="alert-link">Check It Out!</a>
        </div>
        <div class="alert alert-danger text-center " id="divAlert" role="alert"  style="display:none" runat="server">
          <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
        </div>
        <section>   
            <div class="container">
                <nav aria-label="breadcrumb">
                  <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="aOrder_List.aspx">訂單列表</a></li>
                    <li class="breadcrumb-item active" aria-current="page">新增訂單</li>
                  </ol>
                </nav>
                <form id="form1" class="" runat="server">
                    <div class="form1-group">
                        <div class="form1-control">訂購期間：</div>
                        <asp:TextBox ID="txtStartDate" runat="server" class="datetimes"></asp:TextBox> ~ <asp:TextBox ID="txtEndDate" runat="server" class="datetimes"></asp:TextBox>
                    </div>
                    <div class="form1-group">
                        <div class="form1-control">選擇店家：</div>
                        <asp:DropDownList ID="ddlStore" class="form1-input" runat="server"></asp:DropDownList>
                    </div>
                    <div class="form1-group">
                        <div class="form1-control">備註：</div>
                        <asp:TextBox ID="txtRemark" class="form1-input txtRemark" runat="server"  TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="form1-group">
                        <asp:Button ID="btnSubmit" class="btn-admin-dark btn-size-4x" runat="server" Text="開放訂購" />
                    </div>
                </form>
            </div> 
    </section>
</asp:Content>