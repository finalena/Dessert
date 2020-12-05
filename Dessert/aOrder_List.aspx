<%@ Page Title="訂單列表┃下午茶後台" Language="C#" MasterPageFile="~/Site_admin.Master" AutoEventWireup="true" CodeBehind="aOrder_List.aspx.cs" Inherits="Dessert.aOrderDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css"> 

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    <div class="alert alert-danger text-center " id="divAlert" role="alert"  style="display:none" runat="server">
        <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
    </div>
    <section >
        <div class="container"> 
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item active" aria-current="page">訂單列表</li>
                </ol>
            </nav>
           <div class="table-control"><asp:Button ID="btnNewOrder" class="btn-admin-dark btn-size-4x" runat="server" Text="新增訂單" /></div>
           <asp:GridView ID="GridView1" runat="server" DataKeyNames="order_no" AutoGenerateColumns="False" CssClass="table table-hover table-borderless" ShowHeaderWhenEmpty="True" PageSize="1" >
                <Columns>
                    <asp:TemplateField HeaderText="#" HeaderStyle-Width="50px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="訂單期限" SortExpression="order_date" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Bind("order_date") %>' NavigateUrl='<%# Eval("order_no", "aOrderDetail.aspx?order_no={0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("order_date") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="store_name" HeaderText="店家名稱" />
                    <asp:BoundField DataField="Quantity" HeaderText="總數量" HeaderStyle-Width="100px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"  />
                    <asp:BoundField DataField="Total" HeaderText="總金額" HeaderStyle-Width="100px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="狀態" SortExpression="status" HeaderStyle-Width="80px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:Label ID="lblstatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                            </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("status") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-Width="70px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" CommandName="Delete" Text="刪除" onclientclick="return confirm('刪除訂單也會將訂單資料刪除，您確定要刪除嗎?')" CssClass="btn btn-secondary btn-sm" runat="server"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="pagination-bar">  
                <div class="search-status">
                    <div class="resultStats">
                        <asp:Label ID="lblResultStats" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <asp:Label ID="lblPagination" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </section>
</form>
</asp:Content>
