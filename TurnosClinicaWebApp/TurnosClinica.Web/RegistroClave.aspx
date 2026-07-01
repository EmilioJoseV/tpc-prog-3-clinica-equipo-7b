<%@ Page Title="Registro de Clave" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="RegistroClave.aspx.cs" Inherits="TurnosClinica.Web.RegistroClave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="mb-4 mt-4">
        <h1 class="h3 mb-1">Registro de Clave</h1>
        <p class="text-muted">Ingresa tu correo para generar tus credenciales por primera vez.</p>
    </div>

    <div class="row">
        <div class="col-12 col-md-8">
            <asp:Panel ID="PnlMensaje" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show" role="alert">
                <asp:Label ID="LblMensajeTexto" runat="server" />
            </asp:Panel>
        </div>
    </div>

    <div class="row mb-3">
        <div class="col-12 col-md-6 col-lg-5">
            <div class="mb-3">
                <asp:Label ID="LblEmail" runat="server" Text="Correo Electrónico" CssClass="form-label" Style="font-weight: bold;" />
                <div class="d-flex gap-2">
                    <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" TextMode="Email"
                        placeholder="correo registrado en la clinica" required="required" />
                    <asp:Button ID="BtnContinuar" runat="server" Text="Continuar" CssClass="btn btn-outline-primary" OnClick="BtnContinuar_Click" />
                    <asp:Button ID="BtnCancelarEmail" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary"
                        OnClick="BtnVolver_Click" CausesValidation="false" UseSubmitBehavior="false" />
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-12 col-md-6 col-lg-4">
            <asp:Panel ID="PnlFormularioClave" runat="server" Visible="false" CssClass="card bg-light p-3 mb-4">
                <h5 class="text-primary mb-3" style="font-weight: bold;">Establecer Credenciales para:<br />
                    <small>
                        <asp:Label ID="LblUsuarioSeleccionado" runat="server" CssClass="text-dark fs-6" /></small>
                </h5>

                <div class="mb-3">
                    <asp:Label ID="LblNombreUsuario" runat="server" Text="Nombre de Usuario (Login)" CssClass="form-label" Style="font-weight: bold;" />
                    <asp:TextBox ID="TxtNombreUsuario" runat="server" CssClass="form-control"
                        placeholder="Ej: nombre.apellido" required="required" />
                </div>

                <div class="mb-3">
                    <asp:Label ID="LblClave" runat="server" Text="Nueva Contraseña" CssClass="form-label" />
                    <asp:TextBox ID="TxtClave" runat="server" TextMode="Password" CssClass="form-control"
                        placeholder="Minimo 8 caracteres, mayuscula, minuscula y numero" required="required" />
                </div>

                <div class="mb-3">
                    <asp:Label ID="LblClaveConfirmar" runat="server" Text="Confirmar Contraseña" CssClass="form-label" />
                    <asp:TextBox ID="TxtClaveConfirmar" runat="server" TextMode="Password" CssClass="form-control"
                        placeholder="Repeti la contraseña" required="required" />
                </div>

                <div class="mt-2">
                    <asp:Button ID="BtnActivar" runat="server" CssClass="btn btn-success" Text="Activar Cuenta" OnClick="BtnActivar_Click" />
                    <asp:Button ID="BtnVolver" runat="server" CssClass="btn btn-outline-secondary ms-2" Text="Cancelar"
                        OnClick="BtnVolver_Click" CausesValidation="false" UseSubmitBehavior="false" />
                </div>
            </asp:Panel>
        </div>
    </div>

</asp:Content>
