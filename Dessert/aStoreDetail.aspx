<%@ Page Title="" Language="C#" MasterPageFile="~/Site_admin.Master" AutoEventWireup="true" CodeBehind="aStoreDetail.aspx.cs" Inherits="Dessert.aStoreDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
   .form1-input
    {
        width: 400px;    
    }
    /* section 店家基本資訊*/
    .store-detail
    {
        margin: 30px 0;    
    }
    #ContentPlaceHolder1_txtRemark
    {
        height: 100px;    
    }
    #ContentPlaceHolder1_btnSubmit
    {
        margin-left: 220px;
    }        
    /* section 餐點清單 */
    section.store-item > div > h3
    {
        margin: 16px 0;
    }
    .category-search > span
    {
        margin: 0 5px;
    }
    .category-search
    {
        float: left;    
    }
    .category-mgt
    {
        float: right;    
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
        max-width: 350px;
    }
    td:nth-child(3)
    {
        max-width: 300px;
    }
    td:nth-child(4)
    {
        max-width: 300px;
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
    <form id="form2" runat="server">
    <section class="store-info">   
        <div class="container">
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="aStore.aspx">店家列表</a></li>
                <li class="breadcrumb-item active" aria-current="page">管理店家</li>
              </ol>
            </nav>
            <h3>店家基本資訊</h3>  
            <div class="store-detail">
                <div class="form1-group">
                    <div class="form1-control" >店家：</div>
                    <asp:TextBox ID="txtStore" class="form1-input" runat="server" required="required"></asp:TextBox>
                </div>
                <div class="form1-group">
                    <div class="form1-control">電話：</div>
                    <asp:TextBox ID="txtPhone" class="form1-input" runat="server"></asp:TextBox>
                </div>
                <div class="form1-group">
                    <div class="form1-control">地址：</div>
                    <asp:TextBox ID="txtAddress" class="form1-input" runat="server"></asp:TextBox>
                </div>
                <div class="form1-group">
                    <div class="form1-control">備註：</div>
                    <asp:TextBox ID="txtRemark" class="form1-input" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
                <asp:Button ID="btnSubmit" class="btn-admin-dark btn-size-2x" runat="server" Text="儲存" />
            </div>
        <hr />
        </div>
    </section>
    <section class="store-item">
        <div class="container">
            <h3>餐點清單</h3>
            <div class="table-control">
                <div class="category-search">
                    <span>請選擇欲查詢的類別</span><asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True"></asp:DropDownList>
                </div>
                <div class="category-mgt">
                    <asp:Button ID="btnCategoryMgt" class="btn-admin-dark btn-size-4x" runat="server" Text="類別管理" />
                </div>
            </div>
            <div class="product">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="item_no" CssClass="table table-hover table-borderless table-sm " ShowFooter="True" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:TemplateField HeaderText="#"  HeaderStyle-Width="50px" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate><%#Container.DataItemIndex+1 %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="類別" HeaderStyle-Width="350px">
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlCategoryFooter" Width="90%" runat="server">
                                </asp:DropDownList>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCategory_name" Text='<%# Bind("itemCategory_name") %>' Width="90%" runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlCategoryEdit" Width="90%" runat="server"></asp:DropDownList>
                                <br />
                                <asp:Label ID="lblCategory_no" runat="server" Text='<%# Bind("itemCategory_no") %>' Visible="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="品名" HeaderStyle-Width="325px" >
                            <FooterTemplate>
                                <asp:TextBox ID="txtItemNameFooter"  Width="90%" runat="server"></asp:TextBox>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("item_name") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtItemName" Text='<%# Bind("item_name") %>' Width="90%" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金額" HeaderStyle-Width="325px">
                            <FooterTemplate>
                                <asp:TextBox ID="txtItemPriceFooter" Width="90%" runat="server"></asp:TextBox>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblItemPrice"  runat="server" Text='<%# Bind("item_price") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtItemPrice" Text='<%# Bind("item_price") %>' Width="90%" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作" HeaderStyle-Width="100px" HeaderStyle-CssClass="text-left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" CausesValidation="False" CommandName="Edit"  CssClass="btn btn-dark btn-sm" Text="編輯" />
                                <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Delete" onclientclick="return confirm('您確定要刪除該餐點嗎?')"  CssClass="btn btn-secondary btn-sm" Text="刪除" />
                            </ItemTemplate>
                            <EditItemTemplate> 
                                <asp:Button ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update" CssClass="btn btn-info btn-sm" Text="儲存"  />
                                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"  CssClass="btn btn-secondary btn-sm" Text="取消" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnAddItem" Text="新增" CommandName="AddNewItem" CssClass="btn btn-info btn-sm" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </section>
    </form>
</asp:Content>
