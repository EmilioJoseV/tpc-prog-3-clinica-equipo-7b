<%@ Page Title="Registro de Clave" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="RegistroClave.aspx.cs" Inherits="TurnosClinica.Web.RegistroClave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="mb-4 mt-4">
        <h1 class="h3 mb-1">Registro de Clave</h1>
        <p class="text-muted">Ingresá tu correo para generar tu clave de acceso por primera vez.</p>
    </div>
    <div class="row">
        <div class="col-12 col-md-6 col-lg-4">
            <asp:Panel ID="PnlMensaje" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show" role="alert">
                <asp:Label ID="LblMensajeTexto" runat="server" />
            </asp:Panel>
        </div>
    </div>

    <div class="row">
        <div class="col-12 col-md-6 col-lg-4">
            <div class="mb-3">
                <asp:Label ID="LblEmail" runat="server" Text="Correo Electrónico" CssClass="form-label" Style="font-weight: bold;" />

                <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" placeholder="ejemplo@correo.com"
                    AutoPostBack="true" OnTextChanged="TxtEmail_TextChanged" />

                <asp:ListBox ID="LstCorreosSugeridos" runat="server" CssClass="form-control mt-1"
                    Visible="false" AutoPostBack="true" OnSelectedIndexChanged="LstCorreosSugeridos_SelectedIndexChanged"
                    Style="max-height: 150px;" />
            </div>

            <asp:Panel ID="PnlCuadroInfoUsuario" runat="server" Visible="false" CssClass="card mb-3 bg-light">
                <div class="card-body">
                    <h5 class="card-title text-primary mb-3" style="font-weight: bold;">Datos Asociados Detectados</h5>
                    <div class="row g-2">
                        <div class="col-6">
                            <strong>Nombre completo:</strong>
                            <asp:Label ID="LblInfoNombreCompleto" runat="server" />
                        </div>
                        <div class="col-6">
                            <strong>Rol en Clínica:</strong>
                            <asp:Label ID="LblInfoRol" runat="server" CssClass="badge bg-secondary" />
                        </div>
                        <div class="col-6">
                            <strong>Correo:</strong>
                            <asp:Label ID="LblInfoCorreo" runat="server" />
                        </div>
                        <div class="col-6">
                            <strong>Estado Actual:</strong>
                            <asp:Label ID="LblInfoEstado" runat="server" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

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
