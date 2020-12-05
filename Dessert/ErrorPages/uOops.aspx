<%@ Page Title="" Language="C#" MasterPageFile="~/Site_user.Master" AutoEventWireup="true" CodeBehind="uOops.aspx.cs" Inherits="Dessert.ErrorPages.uOops" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    a
    {
        color: #ED502E;     
    }
    a:hover 
    {
       color: #ED502E;     
    }
    .container
    {
        margin-top: 50px;
        font-size: 21px;
        color: #676767;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container text-center">
    我們找不到您想去的那一頁...<br />
    請回上一頁重新選擇<br />
    <br />
    您也可以<br />
    <a href="/uOrder_List.aspx">回首頁</a>
</div>
</asp:Content>
