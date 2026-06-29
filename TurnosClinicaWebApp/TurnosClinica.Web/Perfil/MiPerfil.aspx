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
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtEmail" TextMode="Email" />
                </div>
                <div class="mb-3">
                    <label class="form-label">Nombre</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtNombre" />
                </div>
                <div class="mb-3">
                    <label class="form-label">Apellido</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtApellido" />
                </div>
                
                <div class="mb-3 mt-4">
                    <asp:Button Text="Guardar Cambios" CssClass="btn btn-primary" OnClick="btnGuardar_Click" ID="btnGuardar" runat="server" />
                    <a href="Default.aspx" class="ms-3">Regresar</a>
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
                        CssClass="img-thumbnail rounded-circle"
                        Style="width: 150px; height: 150px; object-fit: cover;" />
                    
                    
                    <asp:Panel ID="pnlInicial" runat="server"
                        CssClass="bg-secondary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto"
                        Style="width: 150px; height: 150px; font-size: 50px;">
                        <asp:Literal ID="litInicial" runat="server" Text="U" />
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
