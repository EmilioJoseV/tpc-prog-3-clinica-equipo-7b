<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionPacientes.aspx.cs" Inherits="TurnosClinica.Web.GestionPacientes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gestion de Pacientes</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Gestion de Pacientes</h1>
        <p>Esta pagina esta pensada para la carga, consulta y mantenimiento de pacientes.</p>
        <asp:Button ID="BtnAltaPaciente" runat="server" Text="Dar de alta paciente" OnClick="BtnAltaPaciente_Click" />
    </form>
</body>
</html>
