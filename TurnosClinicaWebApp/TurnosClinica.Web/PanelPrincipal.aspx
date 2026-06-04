<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PanelPrincipal.aspx.cs" Inherits="TurnosClinica.Web.PanelPrincipal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Panel Principal</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Panel Principal</h1>
        <p>Esta pagina esta pensada para ser el acceso principal de la aplicacion.</p>

        <h2>Accesos</h2>
        <asp:Button ID="BtnInicio" runat="server" Text="Inicio" OnClick="BtnInicio_Click" />
        <asp:Button ID="BtnLogin" runat="server" Text="Login" OnClick="BtnLogin_Click" />
        <asp:Button ID="BtnPacientes" runat="server" Text="Pacientes" OnClick="BtnPacientes_Click" />
        <asp:Button ID="BtnMedicos" runat="server" Text="Medicos" OnClick="BtnMedicos_Click" />
        <asp:Button ID="BtnEspecialidades" runat="server" Text="Especialidades" OnClick="BtnEspecialidades_Click" />
        <asp:Button ID="BtnHorariosMedicos" runat="server" Text="Horarios Medicos" OnClick="BtnHorariosMedicos_Click" />
        <asp:Button ID="BtnUsuarios" runat="server" Text="Usuarios" OnClick="BtnUsuarios_Click" />
        <asp:Button ID="BtnRoles" runat="server" Text="Roles" OnClick="BtnRoles_Click" />
        <asp:Button ID="BtnConfiguracionTurnos" runat="server" Text="Configuracion Turnos" OnClick="BtnConfiguracionTurnos_Click" />
        <asp:Button ID="BtnTurnos" runat="server" Text="Turnos" OnClick="BtnTurnos_Click" />
        <asp:Button ID="BtnDetalleTurno" runat="server" Text="Detalle Turno" OnClick="BtnDetalleTurno_Click" />
        <asp:Button ID="BtnMisTurnos" runat="server" Text="Mis Turnos" OnClick="BtnMisTurnos_Click" />
    </form>
</body>
</html>
