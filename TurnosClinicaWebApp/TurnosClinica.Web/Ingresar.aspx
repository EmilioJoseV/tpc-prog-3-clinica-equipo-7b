<%@ Page Title="Ingresar" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Ingresar.aspx.cs" Inherits="TurnosClinica.Web.Ingresar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-12 col-md-6 col-lg-4">
            <h2>Ingresar</h2>
            <div class="mb-3">
                <asp:Label ID="LblUsuario" runat="server" Text="Usuario" CssClass="form-label" />
                <asp:TextBox ID="TxtUsuario" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <asp:Label ID="LblContrasena" runat="server" Text="Password" CssClass="form-label" />
                <asp:TextBox ID="TxtContrasena" runat="server" TextMode="Password" CssClass="form-control" />
            </div>
            <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-primary" Text="Ingresar" OnClick="BtnIngresar_Click" />
            <asp:Button ID="BtnRegistrarme" runat="server" CssClass="btn btn-outline-primary ms-2"
                Text="Registrarme" CausesValidation="false" />
            <asp:Button ID="BtnRecuperarContrasena" runat="server" CssClass="btn btn-outline-primary ms-2"
                Text="Recuperar contrasena" CausesValidation="false" />
        </div>
    </div>
</asp:Content>
