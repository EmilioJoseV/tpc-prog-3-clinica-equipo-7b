<%@ Page Title="Lista de Medicos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListaMedicos.aspx.cs" Inherits="TurnosClinica.Web.ListaMedicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Lista de Medicos</h1>
    <div class="row">
        <div class="col-6">
            <div class="mb-3">
                <asp:Label Text="Filtrar" runat="server" />
                <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control" AutoPostBack="true" OnTextChanged="filtro_TextChanged" />
            </div>
        </div>
        <div class="col-6" style="display: flex; flex-direction: column; justify-content: flex-end;">
            <div class="mb-3">
                <asp:CheckBox Text="Filtro Avanzado"
                    ID="chkAvanzado" runat="server"
                    AutoPostBack="true"
                    OnCheckedChanged="chkAvanzado_CheckedChanged" />
            </div>
        </div>

        <% if (chkAvanzado.Checked)
            { %>
        <div class="row">
            <div class="col-3">
                <div class="mb-3">
                    <asp:Label Text="Campo" ID="lblCampo" runat="server" />
                    <asp:DropDownList runat="server" AutoPostBack="true" CssClass="form-control" ID="ddlCampo" OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged">
                        <asp:ListItem Text="Nombre" />
                        <asp:ListItem Text="Matricula" />
                        <asp:ListItem Text="DNI" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-3">
                <div class="mb-3">
                    <asp:Label Text="Criterio" runat="server" />
                    <asp:DropDownList runat="server" ID="ddlCriterio" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-3">
                <div class="mb-3">
                    <asp:Label Text="Filtro" runat="server" />
                    <asp:TextBox runat="server" ID="txtFiltroAvanzado" CssClass="form-control" />
                </div>
            </div>
            <div class="col-3">
                <div class="mb-3">
                    <asp:Label Text="Estado" runat="server" />
                    <asp:DropDownList runat="server" ID="ddlEstado" CssClass="form-control">
                        <asp:ListItem Text="Todos" />
                        <asp:ListItem Text="Activo" />
                        <asp:ListItem Text="Inactivo" />
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-3">
                <div class="mb-3">
                    <asp:Button Text="Buscar" runat="server" CssClass="btn btn-primary" ID="btnBuscar" OnClick="btnBuscar_Click" />
                </div>
            </div>
            <div class="col-3">
                <div class="mb-3">
                    <asp:Button Text="Limpiar" runat="server" CssClass="btn btn-outline-primary" ID="btnLimpiar" OnClick="btnLimpiar_Click" />
                </div>
            </div>
        </div>
        <% } %>
    </div>
    <asp:GridView ID="dgvMedicos" runat="server" DataKeyNames="IdMedico"
        CssClass="table" AutoGenerateColumns="false"
        OnRowCommand="dgvMedicos_RowCommand">
        <Columns>
            <asp:BoundField HeaderText="Matricula" DataField="Matricula" />
            <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
            <asp:BoundField HeaderText="Apellido" DataField="Apellido" />
            <asp:BoundField HeaderText="DNI" DataField="DNI" />
            <asp:BoundField HeaderText="Email" DataField="Email" />
            <asp:TemplateField HeaderText="Activo">
                <ItemTemplate>
                    <%# Convert.ToBoolean(Eval("Activo")) ? "Activo" : "Inactivo" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Accion">
                <ItemTemplate>
                    <asp:LinkButton ID="BtnModificar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("IdMedico") %>'>Modificar</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Baja">
                <ItemTemplate>
                    <asp:LinkButton ID="BtnBaja" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("IdMedico") %>'>Baja</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="mt-3">
        <asp:Button Text="Limpiar" runat="server" CssClass="btn btn-outline-primary" ID="btnLimpiarRapido" OnClick="btnLimpiar_Click" />
    </div>
    <a href="FormularioMedico.aspx" class="btn btn-primary">Agregar</a>
</asp:Content>
