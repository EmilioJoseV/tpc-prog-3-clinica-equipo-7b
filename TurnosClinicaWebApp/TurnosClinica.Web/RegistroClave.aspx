<%@ Page Title="Registro de Clave" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="RegistroClave.aspx.cs" Inherits="TurnosClinica.Web.RegistroClave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="mb-4 mt-4">
        <h1 class="h3 mb-1">Registro de Clave</h1>
        <p class="text-muted">Ingresá tu correo para generar tu clave de acceso por primera vez.</p>
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
                    <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" placeholder="ejemplo@correo.com"
                        AutoPostBack="true" OnTextChanged="TxtEmail_TextChanged" />
                    <asp:Button ID="BtnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-primary" OnClick="BtnLimpiar_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <div class="row mb-4">
        <div class="col-12 col-md-8">
            <asp:GridView ID="DgvUsuarios" runat="server" AutoGenerateColumns="false" Visible="false"
                CssClass="table table-striped table-hover table-bordered align-middle"
                GridLines="None" UseAccessibleHeader="true" HeaderStyle-CssClass="table-dark"
                OnRowCommand="DgvUsuarios_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Nombre">
                        <ItemTemplate><%#: Eval("Persona.Nombre") %></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Apellido">
                        <ItemTemplate><%#: Eval("Persona.Apellido") %></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate><%#: Eval("Persona.Email") %></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Rol">
                        <ItemTemplate><%#: Eval("Rol.Nombre") %></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <span class='<%# ObtenerClaseEstado(Eval("EstadoUsuario.Nombre")) %>'>
                                <%#: Eval("EstadoUsuario.Nombre") %>
                        </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Acción">
                        <ItemTemplate>
                            <asp:LinkButton ID="BtnSeleccionar" runat="server" CommandName="Seleccionar"
                                CommandArgument='<%# Eval("Persona.Email") + "|" + Eval("EstadoUsuario.Nombre") %>'
                                CssClass="btn btn-warning btn-sm">Seleccionar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
                    <asp:Label ID="LblClave" runat="server" Text="Nueva Contraseña" CssClass="form-label" />
                    <asp:TextBox ID="TxtClave" runat="server" TextMode="Password" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <asp:Label ID="LblClaveConfirmar" runat="server" Text="Confirmar Contraseña" CssClass="form-label" />
                    <asp:TextBox ID="TxtClaveConfirmar" runat="server" TextMode="Password" CssClass="form-control" />
                </div>

                <div class="mt-2">
                    <asp:Button ID="BtnActivar" runat="server" CssClass="btn btn-success" Text="Activar Cuenta" OnClick="BtnActivar_Click" />
                    <asp:Button ID="BtnVolver" runat="server" CssClass="btn btn-outline-secondary ms-2" Text="Cancelar" OnClick="BtnVolver_Click" CausesValidation="false" />
                </div>
            </asp:Panel>
        </div>
    </div>

</asp:Content>
