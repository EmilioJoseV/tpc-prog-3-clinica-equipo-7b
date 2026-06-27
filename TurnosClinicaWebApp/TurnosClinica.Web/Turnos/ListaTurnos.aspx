<%@ Page Title="Lista de Turnos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListaTurnos.aspx.cs" Inherits="TurnosClinica.Web.ListaTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h1 class="h3 mb-3">Administracion de Turnos</h1>
                <p class="text-secondary">Lista de turnos registrados</p>
            </div>
            <div class="col text-end">
                <a href="FormularioTurno.aspx" class="btn btn-primary">+ Nuevo Turno</a>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-12 col-md-4">
                <asp:Label runat="server" Text="Filtrar" CssClass="form-label" />
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control"
                    AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged" />
            </div>
            <div class="col-12 col-md-3">
                <asp:Label runat="server" Text="Fecha" CssClass="form-label" />
                <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-3">
                <asp:Label runat="server" Text="Estado" CssClass="form-label" />
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" />
            </div>
            <div class="col-12 col-md-2 d-flex align-items-end gap-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary"
                    OnClick="btnBuscar_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-primary"
                    OnClick="btnLimpiar_Click" />
            </div>
        </div>

        <asp:GridView ID="dgvTurnos" runat="server" AutoGenerateColumns="false"
            CssClass="table table-striped table-hover table-bordered align-middle"
            GridLines="None" UseAccessibleHeader="true" HeaderStyle-CssClass="table-dark"
            EmptyDataText="No se encontraron turnos."
            OnRowCommand="dgvTurnos_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Numero">
                    <ItemTemplate><%#: Eval("NumeroTurno") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fecha">
                    <ItemTemplate><%#: ObtenerFechaConDia(Convert.ToDateTime(Eval("FechaTurno"))) %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Horario">
                    <ItemTemplate>
                        <%# ((TimeSpan)Eval("HoraInicio")).ToString(@"hh\:mm") %> -
                        <%# ((TimeSpan)Eval("HoraFin")).ToString(@"hh\:mm") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Paciente">
                    <ItemTemplate>
                        <%#: Eval("Paciente.Persona.Apellido") %>, <%#: Eval("Paciente.Persona.Nombre") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Medico">
                    <ItemTemplate>
                        <%#: Eval("Medico.Persona.Apellido") %>, <%#: Eval("Medico.Persona.Nombre") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Especialidad">
                    <ItemTemplate><%#: Eval("Especialidad.Nombre") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <span class='<%# ObtenerClaseEstado(Eval("EstadoTurno.Nombre").ToString()) %>'>
                            <%#: Eval("EstadoTurno.Nombre") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ver">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnVer" runat="server" CommandName="Ver"
                            CommandArgument='<%# Eval("IdTurno") %>'
                            CssClass="btn btn-warning btn-sm">Ver</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
