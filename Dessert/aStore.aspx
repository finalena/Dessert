<%@ Page Title="店家列表┃下午茶後台" Language="C#" MasterPageFile="~/Site_admin.Master" AutoEventWireup="true" CodeBehind="aStore.aspx.cs" Inherits="Dessert.aStore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .new-store
    {
        float: left;    
    }
    .search-store
    {
        float: right;    
    }
    #ContentPlaceHolder1_btnSearch
    {
        margin: 0 5px;
    }
    /* Table */
    td:nth-child(2),
    td:nth-child(3),
    td:nth-child(4),
    td:nth-child(5)
    {
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden;
    } 
    td:nth-child(2)
    {
        max-width: 250px;
    }
    td:nth-child(3)
    {
       max-width: 120px;
    }
    td:nth-child(4)
    {
        max-width: 150px;
    }
    td:nth-child(5)
    {
        max-width: 350px;
    }    

</style>
<script type="text/javascript">

</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="alert alert-danger text-center " id="divAlert" role="alert"  style="display:none" runat="server">
      <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
    </div>
    <div class="alert alert-warning text-center divNotify" id="divNotify" role="alert"  style="display:none" runat="server">
      <asp:Label ID="lblNotify" runat="server" Text=""></asp:Label>
    </div>
    <div class="container">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item active" aria-current="page">店家列表</li>
          </ol>
        </nav>
        <form id="form1" runat="server">
        <div class="table-control">
            <div class="new-store">
                <asp:Button ID="btnNewStore" class="btn-admin-dark btn-size-4x" runat="server" Text="新增店家" />
            </div>
            <div class="search-store">
                <asp:TextBox ID="txtSearchStore" runat="server" placeholder="請輸入店家名稱"></asp:TextBox><asp:Button ID="btnSearch" class="btn-admin-dark btn-size-2x" runat="server" Text="查詢" />
            </div>
        </div>
        <div class="nullValue text-center">
            <asp:Label ID="lblNullValue" runat="server" Text=""></asp:Label>
        </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-hover table-borderless " DataKeyNames="store_no" ShowHeaderWhenEmpty="True" >
            <Columns>
               <asp:TemplateField HeaderText="#" HeaderStyle-Width="50px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="店家" SortExpression="store_name" HeaderStyle-Width="250px">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("store_no", "aStoreDetail.aspx?store_no={0}") %>' Text='<%# Eval("store_name") %>'></asp:HyperLink>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("store_name") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtStoreNameFooter" runat="server" Width="100%" Height="100%" TextMode="MultiLine"  ></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="電話" SortExpression="store_phone" HeaderStyle-Width="120px">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("store_phone") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("store_phone") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtPhoneFooter" Width="100%" Height="100%" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="地址" SortExpression="store_address" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("store_address") %>'></asp:Label>
                    </ItemTemplate>
                     <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" Text='<%# Bind("store_address") %>' Width="100%" Height="100%" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtAddressFooter" runat="server" Height="100%" TextMode="MultiLine" Width="100%" ></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="備註" SortExpression="store_remark" HeaderStyle-Width="350px">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("store_remark") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Height="100%" Text='<%# Bind("store_remark") %>' TextMode="MultiLine" Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtRemarkFooter" Width="100%" Height="100%" TextMode="MultiLine" runat="server" ></asp:TextBox>
                    </FooterTemplate>
                    </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" HeaderStyle-Width="70px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" CommandName="Delete" Text="刪除" onclientclick="return confirm('刪除店家也會刪除該店家的所有餐點，您確定要刪除嗎??')" runat="server" CssClass='btn btn-secondary btn-sm' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button ID="btnAdd" CommandName="AddNew" CssClass='btn btn-info btn-sm' runat="server"  Text="新增" />
                        <asp:Button ID="btnCancel" CommandName="CancelAddNew" CssClass='btn btn-secondary btn-sm' runat="server" Text="取消" />
                    </FooterTemplate>
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
        </form>
    </div>
</asp:Content>
