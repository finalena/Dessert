<%@ Page Title="下午茶清單┃下午茶" Language="C#" MasterPageFile="~/Site_user.Master" AutoEventWireup="true" CodeBehind="uOrder_List.aspx.cs" Inherits="Dessert.dessert_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    td:nth-child(3)
    {
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden;
        max-width: 195px;
    }   
    td:nth-child(4)
    {
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden;
        max-width: 200px;
    }   
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="form1" runat="server">    
    <div class="alert alert-danger text-center " id="divAlert" role="alert"  style="display:none" runat="server">
        <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
    </div>
    <section>
        <div class="container">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item active" aria-current="page">下午茶清單</li>
                </ol>
            </nav>
            <asp:GridView ID="GridView1" DataKeyNames="order_no" AutoGenerateColumns="False" CssClass="table table-hover table-light table-borderless " runat="server" >
                <Columns>
                    <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                         <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="order_date" HeaderText="訂單期限"  HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"/>
                    <asp:BoundField DataField="store_name" HeaderText="店家名稱" />
                    <asp:BoundField DataField="order_remark" HeaderText="訂單備註" HeaderStyle-Width="200px"/>
                    <asp:BoundField DataField="quantity" HeaderText="總數量" HeaderStyle-Width="80px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="total" HeaderText="總金額" HeaderStyle-Width="80px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="狀態"  HeaderStyle-Width="70px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblstatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("status") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-Width="135px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnView" runat="server" Text="查看" PostBackUrl='<%# Eval("order_no", "uOrdered.aspx?order_no={0}") %>' cssClass="btn btn-sm btn-user-primary" /></button>
                            <asp:Button ID="btnOrder" runat="server" Text="馬上訂" PostBackUrl='<%# Eval("order_no", "uOrder.aspx?order_no={0}") %>' cssClass="btn btn-sm btn-user-primary" /></button>
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
