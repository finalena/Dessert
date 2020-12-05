<%@ Page Title="類別管理" Language="C#" MasterPageFile="~/Site_admin.Master" AutoEventWireup="true" CodeBehind="aItemCategory_Mgt.aspx.cs" Inherits="Dessert.aDessertCategory_New" MaintainScrollPositionOnPostback ="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    table
    {
        margin: 20px 0;    
    }
</style>
<script type="text/javascript">
    function handler() {
        var getParameters = location.search.split("=");
        window.location.href = "aStoreDetail.aspx?store_no=" + getParameters[1];
    }
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="alert alert-danger text-center " id="divAlert" role="alert" style="display:none" runat="server">
      <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
    </div>
    <div class="alert alert-warning text-center divNotify" id="divNotify" role="alert" style="display:none" runat="server">
      <asp:Label ID="lblNotify" runat="server" Text=""></asp:Label>
    </div>
    <form id="form1" runat="server">
    <section>
        <div class="container">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="aStore.aspx">店家列表</a></li>
                    <li class="breadcrumb-item"><a href="javascript: handler();">管理店家</a></li>
                <li class="breadcrumb-item active" aria-current="page">類別管理</li>
                </ol>
            </nav>
            <h3 class="text-center"><asp:Label ID="lblStoreName" runat="server" Text=""></asp:Label>：餐點類別</h3>
                <asp:GridView ID="GridView1" DataKeyNames="itemCategory_no" AutoGenerateColumns="False" ShowFooter="True" ShowHeaderWhenEmpty="True" CssClass="table table-hover table-borderless text-center" runat="server">
                    <Columns>
                        <asp:TemplateField HeaderText="#" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="類別名稱" >
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("itemCategory_name") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCategoryName" class="text-center" Text='<%# Eval("itemCategory_name") %>' Width="300px" runat="server" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtCategoryNameFooter" class="text-center" Text='<%# Eval("itemCategory_name") %>' Width="300px" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作" HeaderStyle-Width="130px" HeaderStyle-CssClass="text-left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" CommandName="Edit" Text="編輯"  CssClass="btn btn-dark btn-sm" runat="server"/>
                                <asp:Button ID="btnDelete" CommandName="Delete" Text="刪除"  onclientclick="return confirm('刪除類別也會將屬於該類別的商品刪除，您確定要刪除嗎?')" CssClass="btn btn-secondary btn-sm" runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnSave" CommandName="Update" Text="儲存" CssClass="btn btn-info btn-sm" runat="server"/>
                                <asp:Button ID="btnCancel" CommandName="Cancel" Text="取消"  CssClass="btn btn-secondary btn-sm" runat="server"/>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnAddNew" CommandName="AddNew" Text="新增" CssClass="btn btn-info btn-sm" runat="server"/>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </section>
    </form>

</asp:Content>
