<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TurnosClinica.Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Login</h1>
        <div>
            <asp:Label ID="LblUsuario" runat="server" AssociatedControlID="TxtUsuario" Text="Usuario" />
            <br />
            <asp:TextBox ID="TxtUsuario" runat="server" />
        </div>
        <div>
            <asp:Label ID="LblContrasena" runat="server" AssociatedControlID="TxtContrasena" Text="Password" />
            <br />
            <asp:TextBox ID="TxtContrasena" runat="server" TextMode="Password" />
        </div>
        <div>
            <asp:Button ID="BtnAcceder" runat="server" Text="Acceder" OnClick="BtnAcceder_Click" />
        </div>
        <div>
            <asp:HyperLink ID="HlkOlvideMiPassword" runat="server" NavigateUrl="#" Text="Olvide mi password" />
        </div>
    </form>
</body>
</html>
