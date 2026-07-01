<%@ Page Title="Panel Principal" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="PanelPrincipal.aspx.cs" Inherits="TurnosClinica.Web.PanelPrincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mb-4">
        <h1 class="h3 mb-1">Panel Principal</h1>
        <asp:Label ID="LblBienvenida" runat="server" CssClass="text-muted" />
    </div>

    <div class="row g-3">
        <div id="CardPacientes" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Pacientes</h2>
                    <p class="text-muted">Registrar y consultar pacientes.</p>
                    <asp:LinkButton ID="LnkAltaPaciente" runat="server" CssClass="btn btn-primary btn-sm me-2" OnClick="LnkAltaPaciente_Click">Nuevo paciente</asp:LinkButton>
                    <asp:LinkButton ID="LnkListaPacientes" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkListaPacientes_Click">Ver pacientes</asp:LinkButton>
                </div>
            </div>
        </div>

        <div id="CardMedicos" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Medicos</h2>
                    <p class="text-muted">Registrar y consultar medicos.</p>
                    <asp:LinkButton ID="LnkAltaMedico" runat="server" CssClass="btn btn-primary btn-sm me-2" OnClick="LnkAltaMedico_Click">Nuevo medico</asp:LinkButton>
                    <asp:LinkButton ID="LnkListaMedicos" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkListaMedicos_Click">Ver medicos</asp:LinkButton>
                </div>
            </div>
        </div>

        <div id="CardTurnos" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Turnos</h2>
                    <p class="text-muted">Registrar, listar o revisar turnos asignados.</p>
                    <asp:LinkButton ID="LnkAltaTurno" runat="server" CssClass="btn btn-primary btn-sm me-2" OnClick="LnkAltaTurno_Click">Nuevo turno</asp:LinkButton>
                    <asp:LinkButton ID="LnkListaTurnos" runat="server" CssClass="btn btn-outline-primary btn-sm me-2" OnClick="LnkListaTurnos_Click">Ver turnos</asp:LinkButton>
                    <asp:LinkButton ID="LnkMisTurnos" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkMisTurnos_Click">Mis turnos</asp:LinkButton>
                </div>
            </div>
        </div>

        <div id="CardEspecialidades" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Especialidades</h2>
                    <p class="text-muted">Administrar especialidades disponibles.</p>
                    <asp:LinkButton ID="LnkAltaEspecialidad" runat="server" CssClass="btn btn-primary btn-sm me-2" OnClick="LnkAltaEspecialidad_Click">Nueva especialidad</asp:LinkButton>
                    <asp:LinkButton ID="LnkListaEspecialidades" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkListaEspecialidades_Click">Ver especialidades</asp:LinkButton>
                </div>
            </div>
        </div>

        <div id="CardUsuarios" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Usuarios</h2>
                    <p class="text-muted">Crear y consultar usuarios del sistema.</p>
                    <asp:LinkButton ID="LnkAltaUsuario" runat="server" CssClass="btn btn-primary btn-sm me-2" OnClick="LnkAltaUsuario_Click">Nuevo usuario</asp:LinkButton>
                    <asp:LinkButton ID="LnkListaUsuarios" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkListaUsuarios_Click">Ver usuarios</asp:LinkButton>
                </div>
            </div>
        </div>

        <div id="CardConfiguracion" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Configuracion</h2>
                    <p class="text-muted">Definir parametros generales de turnos.</p>
                    <asp:LinkButton ID="LnkConfiguracionTurnos" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkConfiguracionTurnos_Click">Configurar turnos</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
