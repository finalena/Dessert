<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="Dessert.ErrorPages.ErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Error:</h2>
        <p><asp:Label ID="FriendlyErrorMsg" runat="server" Text="Label" Font-Size="Large" style="color: red"></asp:Label></p>

        <asp:Panel ID="DetailedErrorPanel" runat="server" Visible="false">
            <h4>Detailed Error:</h4>
            <p><asp:Label ID="lblErrorDetailedMsg" runat="server"/></p>

        </asp:Panel>
    </form>
</body>
</html>
