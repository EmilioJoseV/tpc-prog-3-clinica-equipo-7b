<%@ Page Title="Registro de Clave" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="RegistroClave.aspx.cs" Inherits="TurnosClinica.Web.RegistroClave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container mt-4">
        <h1 style="color: blue; font-weight: bold; border-bottom: 2px solid blue; padding-bottom: 10px;">
            PANTALLA: REGISTRO DE CLAVE
        </h1>
    </div>

    <div class="row mt-4">
        <div class="col-12 col-md-6 col-lg-4">
            <h2>Activar mi Cuenta</h2>
            <p class="text-muted">Ingresá tu correo para generar tu clave de acceso por primera vez.</p>
            
            <div class="mb-3">
                <asp:Label ID="LblEmail" runat="server" Text="Correo Electrónico" CssClass="form-label" />
                <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" placeholder="ejemplo@correo.com" />
            </div>
            <div class="mb-3">
                <asp:Label ID="LblClave" runat="server" Text="Nueva Contraseña" CssClass="form-label" />
                <asp:TextBox ID="TxtClave" runat="server" TextMode="Password" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <asp:Label ID="LblClaveConfirmar" runat="server" Text="Confirmar Contraseña" CssClass="form-label" />
                <asp:TextBox ID="TxtClaveConfirmar" runat="server" TextMode="Password" CssClass="form-control" />
            </div>
            
            <asp:Button ID="BtnActivar" runat="server" CssClass="btn btn-success" Text="Activar Cuenta" OnClick="BtnActivar_Click" />
            <asp:Button ID="BtnVolver" runat="server" CssClass="btn btn-outline-secondary ms-2" Text="Cancelar" OnClick="BtnVolver_Click" CausesValidation="false" />
        </div>
    </div>
</asp:Content>