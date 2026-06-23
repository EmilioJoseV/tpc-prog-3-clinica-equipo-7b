<%@ Page Title="Formulario de Usuario" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioUsuario.aspx.cs" Inherits="TurnosClinica.Web.FormularioUsuario" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h1 class="h3 mb-3">
                    <asp:Label ID="lblTitulo" runat="server" Text="Nuevo Usuario" />
                </h1>
                <p class="text-secondary">Usuarios Administrador y Recepcionista</p>
            </div>
        </div>

        <asp:HiddenField ID="hfIdUsuario" runat="server" />
        <asp:HiddenField ID="hfIdPersona" runat="server" />

        <div class="row g-3">
            <div class="col-12 col-md-6">
                <asp:Label runat="server" AssociatedControlID="txtDni" Text="DNI" CssClass="form-label" />
                <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-6">
                <asp:Label runat="server" AssociatedControlID="txtNombre" Text="Nombre" CssClass="form-label" />
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-6">
                <asp:Label runat="server" AssociatedControlID="txtApellido" Text="Apellido" CssClass="form-label" />
                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-6">
                <asp:Label runat="server" AssociatedControlID="txtTelefono" Text="Telefono" CssClass="form-label" />
                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-6">
                <asp:Label runat="server" AssociatedControlID="txtEmail" Text="Email" CssClass="form-label" />
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
            </div>
            <div class="col-12 col-md-6">
                <asp:Label runat="server" AssociatedControlID="ddlRol" Text="Rol" CssClass="form-label" />
                <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Administrador" />
                    <asp:ListItem Text="Recepcionista" />
                </asp:DropDownList>
            </div>
            <div class="col-12 col-md-6">
                <asp:Label runat="server" AssociatedControlID="fileImagen" Text="Imagen" CssClass="form-label" />
                <div class="input-group">
                    <asp:FileUpload ID="fileImagen" runat="server" CssClass="form-control" />
                    <asp:Button ID="btnPrevisualizar" runat="server" Text="Previsualizar"
                        CssClass="btn btn-outline-secondary" OnClick="btnPrevisualizar_Click" />
                </div>
            </div>
            <div class="col-12 col-md-6">
                <asp:Label runat="server" Text="Estado" CssClass="form-label d-block" />
                <asp:Label ID="lblEstado" runat="server" Text="Pendiente" CssClass="badge bg-warning text-dark" />
            </div>
            <div class="col-12">
                <asp:Label runat="server" Text="Vista previa" CssClass="form-label d-block" />
                <asp:Image ID="imgPerfil" runat="server" Visible="false"
                    CssClass="img-thumbnail rounded-circle"
                    Style="width: 100px; height: 100px; object-fit: cover;" />
                <asp:Panel ID="pnlInicial" runat="server"
                    CssClass="bg-secondary text-white rounded-circle d-flex align-items-center justify-content-center"
                    Style="width: 140px; height: 140px; font-size: 40px;">
                    <asp:Literal ID="litInicial" runat="server" Text="U" />
                </asp:Panel>
            </div>
            <div class="col-12">
                <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" Checked="true" />
            </div>
        </div>

        <div class="mt-4">
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar"
                CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                CssClass="btn btn-outline-primary" OnClick="btnCancelar_Click" CausesValidation="false" />
        </div>
    </div>
</asp:Content>
