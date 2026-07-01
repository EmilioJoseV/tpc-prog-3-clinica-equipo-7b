<%@ Page Title="Mi Perfil" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="TurnosClinica.Web.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2>Mi Perfil</h2>
        <div class="row">      
          
            <div class="col-md-4">
                <div class="mb-3">
                    <label class="form-label">Email</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtEmail" TextMode="Email" placeholder="correo@ejemplo.com" required="required" />
                </div>
                <div class="mb-3">
                    <label class="form-label">Nombre</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtNombre"  placeholder="Ej: Pepe" required="required"/>
                </div>
                <div class="mb-3">
                    <label class="form-label">Apellido</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtApellido" placeholder="Ej: Argento" required="required" />
                </div>
                
               <div class="mb-3">
                    <label class="form-label">Nombre de Usuario</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtNombreUsuario" placeholder="Ej: Admin" required="required" />
                </div>
                <div class="mb-3">
                    <label class="form-label">Nueva Contraseña (dejar en blanco para no cambiar)</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtPassword"  autocomplete="new-password" TextMode="Password" />
                </div>
                
                <div class="mb-3 mt-4">
                    <asp:Button Text="Guardar Cambios" CssClass="btn btn-primary" OnClick="btnGuardar_Click" ID="btnGuardar" runat="server" />
                    <asp:Button Text="Cancelar" CssClass="btn btn-outline-secondary ms-2" OnClick="btnCancelar_Click" ID="btnCancelar" runat="server" CausesValidation="false" />
                </div>
            </div>
            
            <div class="col-md-4 ms-md-5">
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="fileImagen" Text="Imagen de Perfil" CssClass="form-label" />
                    <div class="input-group">
                        <asp:FileUpload ID="fileImagen" runat="server" CssClass="form-control" />
                        <asp:Button ID="btnPrevisualizar" runat="server" Text="Previsualizar"
                            CssClass="btn btn-outline-secondary" OnClick="btnPrevisualizar_Click" />
                    </div>
                </div>
                <div class="mt-4 text-center">
                    <asp:Label runat="server" Text="Vista previa" CssClass="form-label d-block text-start mb-3" />
                    
                    
                    <asp:Image ID="imgPerfil" runat="server" Visible="false"
                        Width="150" Height="150"
                        CssClass="img-thumbnail rounded-circle object-fit-cover" />
                    
                    
                    <asp:Panel ID="pnlInicial" runat="server"
                        Width="150" Height="150"
                        CssClass="bg-secondary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto fs-1">
                        <asp:Literal ID="litInicial" runat="server" Text="U" />
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
