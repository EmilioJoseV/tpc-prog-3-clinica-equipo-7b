<%@ Page Title="Alta de Paciente" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioPaciente.aspx.cs" Inherits="TurnosClinica.Web.FormularioPaciente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="h3 mb-3">Alta de Paciente</h1>
    <p class="text-secondary">Esta pagina esta pensada para cargar todos los datos necesarios para dar de alta un paciente.</p>

    <div class="row g-3">
        <div class="col-12 col-md-6">
            <asp:Label ID="LblDni" runat="server" Text="DNI" CssClass="form-label" />
            <asp:TextBox ID="TxtDni" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-6">
            <asp:Label ID="LblNombre" runat="server" Text="Nombre" CssClass="form-label" />
            <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-6">
            <asp:Label ID="LblApellido" runat="server" Text="Apellido" CssClass="form-label" />
            <asp:TextBox ID="TxtApellido" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-6">
            <asp:Label ID="LblFechaNacimiento" runat="server" Text="Fecha de nacimiento" CssClass="form-label" />
            <asp:TextBox ID="TxtFechaNacimiento" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-6">
            <asp:Label ID="LblTelefono" runat="server" Text="Telefono" CssClass="form-label" />
            <asp:TextBox ID="TxtTelefono" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-6">
            <asp:Label ID="LblEmail" runat="server" Text="Email" CssClass="form-label" />
            <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12">
            <asp:Label ID="LblDireccion" runat="server" Text="Direccion" CssClass="form-label" />
            <asp:TextBox ID="TxtDireccion" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12">
            <asp:CheckBox ID="ChkActivo" runat="server" Text="Activo" Checked="true" />
        </div>
    </div>
</asp:Content>
