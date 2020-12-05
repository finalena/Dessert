<%@ Page Title="" Language="C#" MasterPageFile="~/Site_admin.Master" AutoEventWireup="true" CodeBehind="aOops.aspx.cs" Inherits="Dessert.Oops" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    .container > a
    {
        color: #2C3E50;
        text-decoration: underline;
    }
    .container > a:hover 
    {
       color: #3a5269;     
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
    <a href="/aOrder_List.aspx">回首頁</a>
</div>
</asp:Content>
