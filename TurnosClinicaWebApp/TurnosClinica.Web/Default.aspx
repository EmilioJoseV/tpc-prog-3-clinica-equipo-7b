<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TurnosClinica.Web.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Clinica Medica</h1>
        </div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Gestion de Pacientes" />
        <asp:Button ID="Button2" runat="server" Text="Gestion de Medicos" />
        <asp:Button ID="Button3" runat="server" Text="Asignacion de Turnos" />
    </form>
</body>
</html>
