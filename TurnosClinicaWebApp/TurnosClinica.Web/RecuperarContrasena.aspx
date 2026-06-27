<%@ Page Title="Recuperar contrasena" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="RecuperarContrasena.aspx.cs" Inherits="TurnosClinica.Web.RecuperarContrasena" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-12 col-md-7 col-lg-5">
            <h2>Recuperar contrasena</h2>
            <p class="text-muted">Ingresa el email asociado a la cuenta y te enviaremos una clave temporal nueva.</p>

            <asp:Panel ID="PnlResultado" runat="server" Visible="false" CssClass="alert alert-info">
                <asp:Label ID="LblResultado" runat="server" />
            </asp:Panel>

            <div class="mb-3">
                <asp:Label ID="LblEmail" runat="server" Text="Email" CssClass="form-label" />
                <asp:TextBox ID="TxtEmail" runat="server" TextMode="Email" CssClass="form-control" />
            </div>

            <asp:Button ID="BtnEnviar" runat="server" CssClass="btn btn-primary" Text="Enviar clave temporal" OnClick="BtnEnviar_Click" />
            <a href="Ingresar.aspx" class="btn btn-outline-secondary ms-2">Volver al ingreso</a>
        </div>
    </div>
</asp:Content>
