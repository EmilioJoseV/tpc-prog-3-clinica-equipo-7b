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
                    <p class="text-muted">Alta y listado de pacientes.</p>
                    <asp:LinkButton ID="LnkAltaPaciente" runat="server" CssClass="btn btn-primary btn-sm me-2" OnClick="LnkAltaPaciente_Click">Alta</asp:LinkButton>
                    <asp:LinkButton ID="LnkListaPacientes" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkListaPacientes_Click">Ver lista</asp:LinkButton>
                </div>
            </div>
        </div>

        <div id="CardMedicos" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Medicos</h2>
                    <p class="text-muted">Alta y listado de medicos.</p>
                    <asp:LinkButton ID="LnkAltaMedico" runat="server" CssClass="btn btn-primary btn-sm me-2" OnClick="LnkAltaMedico_Click">Alta</asp:LinkButton>
                    <asp:HyperLink ID="LnkListaMedicos" runat="server" CssClass="btn btn-outline-primary btn-sm" NavigateUrl="Medicos/ListaMedicos.aspx">Ver lista</asp:HyperLink>
                </div>
            </div>
        </div>

        <div id="CardTurnos" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Turnos</h2>
                    <p class="text-muted">Gestion y consulta de turnos.</p>
                    <asp:LinkButton ID="LnkAltaTurno" runat="server" CssClass="btn btn-primary btn-sm me-2" OnClick="LnkAltaTurno_Click">Alta</asp:LinkButton>
                    <asp:LinkButton ID="LnkListaTurnos" runat="server" CssClass="btn btn-outline-primary btn-sm me-2" OnClick="LnkListaTurnos_Click">Ver lista</asp:LinkButton>
                    <asp:LinkButton ID="LnkMisTurnos" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkMisTurnos_Click">Mis turnos</asp:LinkButton>
                </div>
            </div>
        </div>

        <div id="CardEspecialidades" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Especialidades</h2>
                    <p class="text-muted">Alta y listado de especialidades.</p>
                    <asp:LinkButton ID="LnkAltaEspecialidad" runat="server" CssClass="btn btn-primary btn-sm me-2" OnClick="LnkAltaEspecialidad_Click">Alta</asp:LinkButton>
                    <asp:LinkButton ID="LnkListaEspecialidades" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkListaEspecialidades_Click">Ver lista</asp:LinkButton>
                </div>
            </div>
        </div>

        <div id="CardUsuarios" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Usuarios</h2>
                    <p class="text-muted">Alta y listado de usuarios.</p>
                    <asp:LinkButton ID="LnkAltaUsuario" runat="server" CssClass="btn btn-primary btn-sm me-2" OnClick="LnkAltaUsuario_Click">Alta</asp:LinkButton>
                    <asp:LinkButton ID="LnkListaUsuarios" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkListaUsuarios_Click">Ver lista</asp:LinkButton>
                </div>
            </div>
        </div>

        <div id="CardConfiguracion" runat="server" class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5">Configuracion</h2>
                    <p class="text-muted">Configuracion global de turnos.</p>
                    <asp:LinkButton ID="LnkConfiguracionTurnos" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="LnkConfiguracionTurnos_Click">Configurar</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
