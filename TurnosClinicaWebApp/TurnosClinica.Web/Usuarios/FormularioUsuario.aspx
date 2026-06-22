<%@ Page Title="Formulario de Usuario" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioUsuario.aspx.cs" Inherits="TurnosClinica.Web.FormularioUsuario" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <%-- Título dinámico compatible con id e Id --%>
        <h2><%= Request.QueryString["id"] != null || Request.QueryString["Id"] != null ? "Modificar Usuario" : "Nuevo Usuario" %></h2>
        <p class="text-secondary">Ingresa los datos correspondientes para el usuario.</p>
        <hr />

        <div class="row">
            <div class="col-md-4">
                <h5 class="text-primary mb-3">Datos Personales</h5>

                <div class="mb-3">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="txtApellido" class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label">Subir nueva foto</label>
                    <div class="input-group">
                        <%-- Eliminamos el onchange de JavaScript --%>
                        <asp:FileUpload ID="fileImagen" runat="server" CssClass="form-control" />
                        <%-- Este botón procesa y actualiza la previsualización desde el servidor --%>
                        <asp:Button ID="btnPrevisualizar" runat="server" Text="Cargar" CssClass="btn btn-outline-secondary" OnClick="btnPrevisualizar_Click" />
                    </div>
                    <small class="text-muted">Formatos aceptados: .jpg, .png</small>
                </div>
            </div>

            <div class="col-md-4">
                <h5 class="text-primary mb-3">Acceso y Seguridad</h5>

                <div class="mb-3">
                    <label for="txtNombreUsuario" class="form-label">Nombre de Usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Correo Electronico</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Contraseña (Dejar vacío para no cambiar)</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="ddlRol" class="form-label">Rol Asignado</label>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <div class="mb-3">
                    <label for="ddlMedico" class="form-label">Medico Asociado (Opcional)</label>
                    <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
            </div>

            <div class="col-md-4 text-center d-flex flex-column align-items-center pt-4">
                <h5 class="text-primary mb-3">Foto de Perfil Actual</h5>

                <%-- Control de imagen estándar: si tiene foto se muestra acá --%>
                <asp:Image ID="imgPerfil" runat="server" CssClass="img-thumbnail rounded-circle" Style="width: 180px; height: 180px; object-fit: cover;" Visible="false" />

                <%-- Control de panel estándar: si no tiene foto se usa para la inicial o el texto por defecto --%>
                <asp:Panel ID="pnlInicial" runat="server" CssClass="bg-secondary text-white rounded-circle d-flex align-items-center justify-content-center" Style="width: 180px; height: 180px; font-size: 48px;" Visible="false">
                    <asp:Literal ID="litInicial" runat="server"></asp:Literal>
                </asp:Panel>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-12">
                <div class="mb-3 d-flex gap-2">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-primary" OnClick="btnAceptar_Click" />
                    <asp:Button ID="btnInactivar" runat="server" Text="Inactivar" CssClass="btn btn-warning" OnClick="btnInactivar_Click" />
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click" OnClientClick="return confirm('¿Estas seguro de que deseas eliminar permanentemente a este usuario?');" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
