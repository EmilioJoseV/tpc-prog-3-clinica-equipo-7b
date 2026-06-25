<%@ Page Title="Detalle de Turno" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="DetalleTurno.aspx.cs" Inherits="TurnosClinica.Web.DetalleTurno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h1 class="h3 mb-3">Detalle de Turno</h1>
                <p class="text-secondary">Consulta, edicion y ciclo de vida del turno</p>
            </div>
        </div>

        <asp:Panel ID="pnlContenido" runat="server" Visible="false">
            <div class="row g-3 mb-4">
                <div class="col-12 col-md-3">
                    <asp:Label runat="server" Text="Numero" CssClass="form-label" />
                    <asp:TextBox ID="txtNumeroTurno" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-12 col-md-3">
                    <asp:Label runat="server" Text="Estado" CssClass="form-label" />
                    <asp:TextBox ID="txtEstado" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-12 col-md-3">
                    <asp:Label runat="server" Text="Fecha" CssClass="form-label" />
                    <asp:TextBox ID="txtFechaActual" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-12 col-md-3">
                    <asp:Label runat="server" Text="Horario" CssClass="form-label" />
                    <asp:TextBox ID="txtHorarioActual" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-12 col-md-6">
                    <asp:Label runat="server" Text="Paciente" CssClass="form-label" />
                    <asp:TextBox ID="txtPaciente" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-12 col-md-6">
                    <asp:Label runat="server" Text="Medico" CssClass="form-label" />
                    <asp:TextBox ID="txtMedico" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-12 col-md-6">
                    <asp:Label runat="server" Text="Especialidad" CssClass="form-label" />
                    <asp:TextBox ID="txtEspecialidad" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-12 col-md-6">
                    <asp:Label runat="server" Text="Email paciente" CssClass="form-label" />
                    <asp:TextBox ID="txtEmailPaciente" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
            </div>

            <div class="row g-3 mb-4">
                <div class="col-12">
                    <asp:Label runat="server" Text="Observaciones" CssClass="form-label" />
                    <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Rows="4"
                        MaxLength="500" CssClass="form-control" />
                </div>
                <div class="col-12">
                    <asp:Label runat="server" Text="Diagnostico medico" CssClass="form-label" />
                    <asp:TextBox ID="txtDiagnosticoMedico" runat="server" TextMode="MultiLine" Rows="4"
                        MaxLength="500" CssClass="form-control" />
                </div>
            </div>

            <div class="mt-4">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary"
                    OnClick="btnGuardar_Click" />
                <asp:Button ID="btnReprogramar" runat="server" Text="Reprogramar turno"
                    CssClass="btn btn-outline-primary" OnClick="btnReprogramar_Click" />
                <asp:Button ID="btnCancelarTurno" runat="server" Text="Cancelar turno"
                    CssClass="btn btn-outline-danger" OnClick="btnCancelarTurno_Click" />
                <asp:Button ID="btnNoAsistio" runat="server" Text="Marcar no asistio"
                    CssClass="btn btn-outline-secondary" OnClick="btnNoAsistio_Click" />
                <asp:Button ID="btnCerrarTurno" runat="server" Text="Cerrar turno"
                    CssClass="btn btn-outline-success" OnClick="btnCerrarTurno_Click" />
                <asp:Button ID="btnVolver" runat="server" Text="Cancelar"
                    CssClass="btn btn-outline-primary" OnClick="btnVolver_Click" CausesValidation="false" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
