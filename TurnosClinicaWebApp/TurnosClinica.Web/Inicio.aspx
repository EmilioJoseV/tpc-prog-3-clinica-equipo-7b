<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="TurnosClinica.Web.Inicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inicio</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Inicio</h1>
        <p>Esta pagina esta pensada para que esten el inicio de la aplicacion cuando entre el usuario y puede ver todo sobre la app e ir al login</p>
        <asp:Button ID="BtnIrLogin" runat="server" Text="Ir al Login" OnClick="BtnIrLogin_Click" />
    </form>
</body>
</html>
