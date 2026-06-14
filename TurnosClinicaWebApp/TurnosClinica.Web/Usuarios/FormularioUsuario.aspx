<%@ Page Title="Formulario de Usuario" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioUsuario.aspx.cs" Inherits="TurnosClinica.Web.FormularioUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2 id="lblTitulo" runat="server">Nuevo Usuario</h2>
        <p class="text-secondary">Ingresa los datos correspondientes para el usuario.</p>
        
        <div class="row">
            <div class="col-md-6">
                <div class="mb-3">
                    <label for="txtNombreUsuario" class="form-label">Nombre de Usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Correo Electrónico</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Contraseña</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="ddlRol" class="form-label">Rol Asignado</label>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <div class="mb-3">
                    <label for="ddlMedico" class="form-label">Médico Asociado (Opcional)</label>
                    <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <div class="mb-3 form-check form-switch mt-4">
                    <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" Checked="true" />
                    <label class="form-check-label" for="<%= chkActivo.ClientID %>">Usuario Activo</label>
                </div>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-md-6">
                <div class="mb-3">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-primary me-2" OnClick="btnAceptar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>