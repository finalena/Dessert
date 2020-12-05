<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Dessert.WebForm11" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>下午茶</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous"/>
    
    <%--Data Source : https://codepen.io/Albzi/pen/abkAn--%>
    <style type="text/css">
    @import url(https://fonts.googleapis.com/css?family=Karla);
    @import url(https://fonts.googleapis.com/css?family=Ubuntu:300);
    
    .wrap{
        padding: 120px 0;
        font-size: 62px;
        color: #888;
        width: 400px;
        font-family: 'Karla';
        margin: 0 auto;
        text-align: center;
    }
    input{
        font-family: 'Ubuntu';
        font-weight: 300;
        border: 0;
        border-bottom: 1px solid #ff5407;
        width: 100%;
        height: 36px;
        font-size: 26px;
    }
    input:focus{
        outline: none;
        box-shadow: none;
        background: #ffeae2;
    }
    button{
        border: 0;
        background: transparent;
        padding: 5px;
        margin-top: 50px;
        position: relative;
        outline: 0;
    } 
    .buttonafter:after{
        content: '';
        display: block;
        position: absolute;
        top: 8px;
        left: 100%; /* should be set to 100% */
        width: 0;
        height: 0;
        border-color: transparent transparent transparent #14a03d; /* border color should be same as div div background color */
        border-style: solid;
        border-width: 5px;
    }
    .login{
        background: #a0a0a0;
        color: #fff;
        float: right;
        border: 0;
    }
    .login.buttonafter {
        background: #14a03d;
    }
    #lblRemind {
        color: #ff5407;
    }
    </style>

</head>
<body>
    <div class="wrap">
        Login
        <form id="form1" runat="server">
            <div><asp:TextBox ID="txtId" placeholder="Username" runat="server" required="required">admin</asp:TextBox></div>
            <div><asp:TextBox ID="txtPwd" placeholder="Password" runat="server" required="required">123456</asp:TextBox></div>
            <div><h6><asp:Label ID="lblRemind" runat="server" Text=""></asp:Label></h6></div> 
            <asp:Button ID="btnLogin" class="login" runat="server" Text="LOG IN" />
        </form>    

    </div>
</body>
</html>
