<%@ Page Title="Formulario de Especialidad" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioEspecialidad.aspx.cs" Inherits="TurnosClinica.Web.FormularioEspecialidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <h2>Nueva Especialidad</h2>
    <div class="row">
        <div class="col-md-6">
            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre de la Especialidad</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="txtDescripcion" class="form-label">Descripción</label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </div>
        </div>
    </div>

    <div class="row">
    <div class="col-md-6">
        <div class="mb-3">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-primary" OnClick="btnAceptar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" />
        </div>
    </div>
</div>
    <div class="row">
        <div class="col-6">
            <div class="mb-3">
            
                <asp:Button Text="Eliminar" ID="btnEliminar" OnClick="btnEliminar_Click" CssClass="btn btn-danger" runat="server" />


            </div>
            <%if (ConfirmaEliminacion)
                { %>

            <div class="mb-3">
                <asp:CheckBox Text="Confirmar Eliminacion" ID="chkConfirmaEliminacion" runat="server" />
                <asp:Button Text="Eliminar" ID="btnConfirmaEliminar" CssClass="btn btn-outline-danger" runat="server" />

            </div>
            <%  } %>
            
        </div
    </div>


</asp:Content>
