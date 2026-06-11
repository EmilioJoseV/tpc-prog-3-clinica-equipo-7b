<%@ Page Title="Panel Principal" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="PanelPrincipal.aspx.cs" Inherits="TurnosClinica.Web.PanelPrincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dropdown:hover > .dropdown-menu {
            display: block;
            margin-top: 0;
        }

        .dropdown:hover > .dropdown-toggle {
            border-bottom-left-radius: 0;
            border-bottom-right-radius: 0;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="h3 mb-3">Panel Principal</h1>
    <p class="text-secondary">Esta pagina esta pensada para ser el acceso principal de la aplicacion.</p>
    <div class="d-flex flex-wrap gap-2 align-items-start">
        <div class="dropdown">
            <asp:Button ID="BtnPacientes" runat="server" CssClass="btn btn-outline-primary dropdown-toggle" Text="Pacientes" UseSubmitBehavior="false" CausesValidation="false" data-bs-toggle="dropdown" aria-expanded="false" />
            <ul class="dropdown-menu">
                <li>
                    <asp:LinkButton ID="LnkAltaPaciente" runat="server" CssClass="dropdown-item" OnClick="LnkAltaPaciente_Click">Alta de paciente</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LnkListaPacientes" runat="server" CssClass="dropdown-item" OnClick="LnkListaPacientes_Click">Ver pacientes</asp:LinkButton>
                </li>
            </ul>
        </div>

        <div class="dropdown">
            <asp:Button ID="BtnMedicos" runat="server" CssClass="btn btn-outline-primary dropdown-toggle" Text="Medicos" UseSubmitBehavior="false" CausesValidation="false" data-bs-toggle="dropdown" aria-expanded="false" />
            <ul class="dropdown-menu">
                <li>
                    <asp:LinkButton ID="LnkAltaMedico" runat="server" CssClass="dropdown-item" OnClick="LnkAltaMedico_Click">Alta de medico</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LnkListaMedicos" runat="server" CssClass="dropdown-item" OnClick="LnkListaMedicos_Click">Ver medicos</asp:LinkButton>
                </li>
            </ul>
        </div>

        <div class="dropdown">
            <asp:Button ID="BtnEspecialidades" runat="server" CssClass="btn btn-outline-primary dropdown-toggle" Text="Especialidades" UseSubmitBehavior="false" CausesValidation="false" data-bs-toggle="dropdown" aria-expanded="false" />
            <ul class="dropdown-menu">
                <li>
                    <asp:LinkButton ID="LnkAltaEspecialidad" runat="server" CssClass="dropdown-item" OnClick="LnkAltaEspecialidad_Click">Alta de especialidad</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LnkListaEspecialidades" runat="server" CssClass="dropdown-item" OnClick="LnkListaEspecialidades_Click">Ver especialidades</asp:LinkButton>
                </li>
            </ul>
        </div>

        <div class="dropdown">
            <asp:Button ID="BtnUsuarios" runat="server" CssClass="btn btn-outline-primary dropdown-toggle" Text="Usuarios" UseSubmitBehavior="false" CausesValidation="false" data-bs-toggle="dropdown" aria-expanded="false" />
            <ul class="dropdown-menu">
                <li>
                    <asp:LinkButton ID="LnkAltaUsuario" runat="server" CssClass="dropdown-item" OnClick="LnkAltaUsuario_Click">Alta de usuario</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LnkListaUsuarios" runat="server" CssClass="dropdown-item" OnClick="LnkListaUsuarios_Click">Ver usuarios</asp:LinkButton>
                </li>
            </ul>
        </div>

        <div class="dropdown">
            <asp:Button ID="BtnTurnos" runat="server" CssClass="btn btn-outline-primary dropdown-toggle" Text="Turnos" UseSubmitBehavior="false" CausesValidation="false" data-bs-toggle="dropdown" aria-expanded="false" />
            <ul class="dropdown-menu">
                <li>
                    <asp:LinkButton ID="LnkConfiguracionTurnos" runat="server" CssClass="dropdown-item" OnClick="LnkConfiguracionTurnos_Click">Configuracion global de turnos</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LnkAltaTurno" runat="server" CssClass="dropdown-item" OnClick="LnkAltaTurno_Click">Alta de turnos</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LnkMisTurnos" runat="server" CssClass="dropdown-item" OnClick="LnkMisTurnos_Click">Mis turnos asignados</asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
