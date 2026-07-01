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
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej: Cardiologia" required="required" />
                </div>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtDescripcion" Text="Descripcion" CssClass="form-label" />
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Escribe una breve descripcion" required="required" />
                </div>
                <div class="mb-3">
                    <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" />
                </div>
                <div class="mb-3">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                        OnClick="btnCancelar_Click" CausesValidation="false" UseSubmitBehavior="false" />
                </div>
            </div>

            <div class="col-12 col-md-6">
                <h2 class="h5 mb-3">Medicos asociados</h2>
                <asp:GridView ID="dgvMedicosAsociados" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered align-middle"
                    GridLines="None" UseAccessibleHeader="true" HeaderStyle-CssClass="table-dark"
                    EmptyDataText="Sin medicos asociados">
                    <Columns>
                        <asp:TemplateField HeaderText="Medico">
                            <ItemTemplate>
                                <%# Eval("Persona.Apellido") + ", " + Eval("Persona.Nombre") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <%# Convert.ToBoolean(Eval("Activo")) ? "Activo" : "Inactivo" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
