<%@ Page Title="Nuevo Turno" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioTurno.aspx.cs" Inherits="TurnosClinica.Web.FormularioTurno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h1 class="h3 mb-2">Nuevo Turno</h1>
                <p class="text-secondary">Selecciona el paciente y busca un horario disponible.</p>
            </div>
        </div>

        <div class="row g-3">
            <div class="col-12 col-md-6">
                <asp:Label runat="server" AssociatedControlID="DdlPaciente" Text="Paciente" CssClass="form-label" />
                <asp:DropDownList ID="DdlPaciente" runat="server" CssClass="form-select" />
            </div>
            <div class="col-12 col-md-6">
                <asp:Label runat="server" AssociatedControlID="DdlEspecialidad" Text="Especialidad" CssClass="form-label" />
                <asp:DropDownList ID="DdlEspecialidad" runat="server" CssClass="form-select" />
            </div>
        </div>

        <div class="mt-4">
            <asp:Label runat="server" Text="Tipo de busqueda" CssClass="form-label d-block" />
            <div class="d-flex gap-4">
                <asp:CheckBox ID="ChkBuscarPorFecha" runat="server" Text="Buscar por fecha"
                    Checked="true" AutoPostBack="true"
                    OnCheckedChanged="ChkBuscarPorFecha_CheckedChanged" />
                <asp:CheckBox ID="ChkBuscarProximo" runat="server" Text="Buscar proximo disponible"
                    AutoPostBack="true" OnCheckedChanged="ChkBuscarProximo_CheckedChanged" />
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-12 col-md-6">
                <asp:Label runat="server" AssociatedControlID="TxtFecha" Text="Fecha" CssClass="form-label" />
                <asp:TextBox ID="TxtFecha" runat="server" TextMode="Date" CssClass="form-control" />
            </div>
        </div>

        <div class="mt-3">
            <asp:Button ID="BtnBuscar" runat="server" Text="Buscar"
                CssClass="btn btn-outline-primary" OnClick="BtnBuscar_Click" />
        </div>

        <asp:Panel ID="PnlDisponibilidad" runat="server" Visible="false" CssClass="mt-4">
            <h2 class="h5 mb-3">Horarios disponibles</h2>
            <asp:GridView ID="DgvDisponibilidad" runat="server" AutoGenerateColumns="false"
                CssClass="table table-striped table-hover table-bordered align-middle"
                GridLines="None" UseAccessibleHeader="true" HeaderStyle-CssClass="table-dark"
                EmptyDataText="No hay horarios disponibles para la seleccion."
                OnRowCommand="DgvDisponibilidad_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Medico">
                        <ItemTemplate>
                            <%#: Eval("Medico.Persona.Apellido") %>, <%#: Eval("Medico.Persona.Nombre") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha">
                        <ItemTemplate><%#: ObtenerFechaConDia(Convert.ToDateTime(Eval("FechaTurno"))) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Desde">
                        <ItemTemplate><%# ((TimeSpan)Eval("HoraInicio")).ToString(@"hh\:mm") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hasta">
                        <ItemTemplate><%# ((TimeSpan)Eval("HoraFin")).ToString(@"hh\:mm") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <ItemTemplate>
                            <asp:LinkButton ID="BtnSeleccionar" runat="server" Text="Seleccionar"
                                CssClass="btn btn-sm btn-primary" CommandName="Seleccionar"
                                CommandArgument='<%# Eval("Medico.IdMedico") + "|" + Eval("HoraInicio") + "|" + Eval("HoraFin") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>

        <asp:HiddenField ID="HfIdMedicoSeleccionado" runat="server" />
        <asp:HiddenField ID="HfHoraInicio" runat="server" />
        <asp:HiddenField ID="HfHoraFin" runat="server" />

        <asp:Panel ID="PnlSeleccion" runat="server" Visible="false" CssClass="alert alert-info mt-4">
            <strong>Horario seleccionado</strong>
            <asp:Label ID="LblSeleccion" runat="server" CssClass="d-block mt-1" />
        </asp:Panel>

        <div class="mt-3">
            <asp:Label runat="server" AssociatedControlID="TxtObservaciones"
                Text="Motivo de la consulta" CssClass="form-label" />
            <asp:TextBox ID="TxtObservaciones" runat="server" TextMode="MultiLine" Rows="4"
                MaxLength="500" CssClass="form-control" />
        </div>

        <div class="mt-4">
            <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary"
                OnClick="BtnGuardar_Click" />
            <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-primary"
                OnClick="BtnCancelar_Click" CausesValidation="false" />
        </div>
    </div>
</asp:Content>
