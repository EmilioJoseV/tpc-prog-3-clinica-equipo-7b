<%@ Page Title="Detalle de Paciente" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioPaciente.aspx.cs" Inherits="TurnosClinica.Web.FormularioPaciente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h1 class="h3 mb-3">Paciente</h1>
                <p class="text-secondary">Completa los datos y guarda los cambios.</p>
            </div>
        </div>

        <asp:HiddenField ID="HfIdPaciente" runat="server" />
        <asp:HiddenField ID="HfIdPersona" runat="server" />

        <div class="row g-3">
            <div class="col-12 col-md-6">
                <label class="form-label">DNI</label>
                <asp:TextBox ID="TxtDni" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-6">
                <label class="form-label">Nombre</label>
                <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-6">
                <label class="form-label">Apellido</label>
                <asp:TextBox ID="TxtApellido" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-6">
                <label class="form-label">Fecha de nacimiento</label>
                <asp:TextBox ID="TxtFechaNacimiento" runat="server" CssClass="form-control" placeholder="yyyy-MM-dd" />
            </div>
            <div class="col-12 col-md-6">
                <label class="form-label">Telefono</label>
                <asp:TextBox ID="TxtTelefono" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-6">
                <label class="form-label">Email</label>
                <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12">
                <label class="form-label">Direccion</label>
                <asp:TextBox ID="TxtDireccion" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12">
                <asp:CheckBox ID="ChkActivo" runat="server" Text="Activo" Checked="true" />
            </div>
        </div>

        <div class="mt-4">
            <asp:Button ID="BtnGuardar" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="BtnGuardar_Click" />
            <asp:Button ID="BtnCancelar" runat="server" CssClass="btn btn-outline-primary" Text="Cancelar" OnClick="BtnCancelar_Click" />
        </div>
    </div>
</asp:Content>
