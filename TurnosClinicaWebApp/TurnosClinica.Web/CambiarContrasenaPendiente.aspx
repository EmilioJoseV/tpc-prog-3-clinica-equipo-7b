<%@ Page Title="Cambiar contrasena" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CambiarContrasenaPendiente.aspx.cs" Inherits="TurnosClinica.Web.CambiarContrasenaPendiente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-12 col-md-7 col-lg-5">
            <h2>Cambiar contrasena</h2>
            <p class="text-muted">Debes definir una nueva contrasena para continuar.</p>

            <asp:Panel ID="PnlCuenta" runat="server" Visible="false" CssClass="alert alert-secondary">
                <strong>Cuenta:</strong>
                <asp:Label ID="LblCuenta" runat="server" />
            </asp:Panel>

            <div class="mb-3">
                <asp:Label ID="LblNuevaContrasena" runat="server" Text="Nueva contrasena" CssClass="form-label" />
                <asp:TextBox ID="TxtNuevaContrasena" runat="server" TextMode="Password" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <asp:Label ID="LblConfirmacionContrasena" runat="server" Text="Confirmar contrasena" CssClass="form-label" />
                <asp:TextBox ID="TxtConfirmacionContrasena" runat="server" TextMode="Password" CssClass="form-control" />
            </div>

            <asp:Button ID="BtnGuardar" runat="server" CssClass="btn btn-primary" Text="Guardar nueva contrasena" OnClick="BtnGuardar_Click" />
        </div>
    </div>
</asp:Content>
