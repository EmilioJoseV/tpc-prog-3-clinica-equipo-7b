<%@ Page Title="Formulario de Especialidad" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioEspecialidad.aspx.cs" Inherits="TurnosClinica.Web.FormularioEspecialidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <asp:HiddenField ID="hfIdEspecialidad" runat="server" />
        <h1 class="h3 mb-3">
            <asp:Label ID="lblTitulo" runat="server" Text="Nueva Especialidad" />
        </h1>

        <div class="row">
            <div class="col-12 col-md-6">
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtNombre" Text="Nombre" CssClass="form-label" />
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                </div>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtDescripcion" Text="Descripcion" CssClass="form-label" />
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                </div>
                <div class="mb-3">
                    <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" />
                </div>
                <div class="mb-3">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
