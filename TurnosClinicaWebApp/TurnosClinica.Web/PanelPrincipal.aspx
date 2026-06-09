<%@ Page Title="Panel Principal" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="PanelPrincipal.aspx.cs" Inherits="TurnosClinica.Web.PanelPrincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="h3 mb-3">Panel Principal</h1>
    <p class="text-secondary">Esta pagina esta pensada para ser el acceso principal de la aplicacion.</p>
    <div class="d-flex flex-wrap gap-2">
        <asp:Button ID="BtnInicio" runat="server" CssClass="btn btn-outline-primary" Text="Inicio" OnClick="BtnInicio_Click" />
        <asp:Button ID="BtnLogin" runat="server" CssClass="btn btn-outline-primary" Text="Login" OnClick="BtnLogin_Click" />
        <asp:Button ID="BtnPacientes" runat="server" CssClass="btn btn-outline-primary" Text="Pacientes" OnClick="BtnPacientes_Click" />
        <asp:Button ID="BtnMedicos" runat="server" CssClass="btn btn-outline-primary" Text="Medicos" OnClick="BtnMedicos_Click" />
        <asp:Button ID="BtnEspecialidades" runat="server" CssClass="btn btn-outline-primary" Text="Especialidades" OnClick="BtnEspecialidades_Click" />
        <asp:Button ID="BtnHorariosMedicos" runat="server" CssClass="btn btn-outline-primary" Text="Horarios Medicos" OnClick="BtnHorariosMedicos_Click" />
        <asp:Button ID="BtnUsuarios" runat="server" CssClass="btn btn-outline-primary" Text="Usuarios" OnClick="BtnUsuarios_Click" />
        <asp:Button ID="BtnRoles" runat="server" CssClass="btn btn-outline-primary" Text="Roles" OnClick="BtnRoles_Click" />
        <asp:Button ID="BtnConfiguracionTurnos" runat="server" CssClass="btn btn-outline-primary" Text="Configuracion Turnos" OnClick="BtnConfiguracionTurnos_Click" />
        <asp:Button ID="BtnTurnos" runat="server" CssClass="btn btn-outline-primary" Text="Turnos" OnClick="BtnTurnos_Click" />
        <asp:Button ID="BtnDetalleTurno" runat="server" CssClass="btn btn-outline-primary" Text="Detalle Turno" OnClick="BtnDetalleTurno_Click" />
        <asp:Button ID="BtnMisTurnos" runat="server" CssClass="btn btn-outline-primary" Text="Mis Turnos" OnClick="BtnMisTurnos_Click" />
    </div>
</asp:Content>
