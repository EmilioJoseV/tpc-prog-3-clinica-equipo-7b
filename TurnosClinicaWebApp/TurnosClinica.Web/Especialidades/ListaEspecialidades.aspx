<%@ Page Title="Lista de Especialidades" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListaEspecialidades.aspx.cs" Inherits="TurnosClinica.Web.ListaEspecialidades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h1 class="h3 mb-3">Administracion de Especialidades</h1>
                <p class="text-secondary">Listado de especialidades registradas</p>
            </div>
            <div class="col text-end">
                <a href="FormularioEspecialidad.aspx" class="btn btn-primary">+ Nueva Especialidad</a>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-12 col-md-6">
                <asp:Label runat="server" Text="Filtrar" CssClass="form-label" />
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged" />
            </div>
            <div class="col-12 col-md-6 d-flex align-items-end gap-2">
                <asp:CheckBox ID="chkAvanzado" runat="server" Text="Filtro avanzado" AutoPostBack="true" OnCheckedChanged="chkAvanzado_CheckedChanged" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-primary" OnClick="btnLimpiar_Click" />
            </div>
        </div>

        <% if (chkAvanzado.Checked)
           { %>
        <div class="row mb-3">
            <div class="col-12 col-md-3">
                <asp:Label runat="server" Text="Campo" CssClass="form-label" />
                <asp:DropDownList ID="ddlCampo" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Nombre" />
                    <asp:ListItem Text="Descripcion" />
                </asp:DropDownList>
            </div>
            <div class="col-12 col-md-3">
                <asp:Label runat="server" Text="Criterio" CssClass="form-label" />
                <asp:DropDownList ID="ddlCriterio" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Contiene" />
                    <asp:ListItem Text="Igual a" />
                    <asp:ListItem Text="Comienza con" />
                    <asp:ListItem Text="Termina con" />
                </asp:DropDownList>
            </div>
            <div class="col-12 col-md-3">
                <asp:Label runat="server" Text="Filtro" CssClass="form-label" />
                <asp:TextBox ID="txtFiltroAvanzado" runat="server" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-3">
                <asp:Label runat="server" Text="Estado" CssClass="form-label" />
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Todos" />
                    <asp:ListItem Text="Activo" />
                    <asp:ListItem Text="Inactivo" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="row mb-3">
            <div class="col-12">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
            </div>
        </div>
        <% } %>

        <asp:GridView ID="dgvEspecialidades" runat="server" AutoGenerateColumns="false"
            CssClass="table table-striped table-hover table-bordered align-middle"
            GridLines="None" UseAccessibleHeader="true" HeaderStyle-CssClass="table-dark"
            OnRowCommand="dgvEspecialidades_RowCommand">
            <Columns>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("Activo")) ? "<span class='badge bg-success'>Activo</span>" : "<span class='badge bg-danger'>Inactivo</span>" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ver">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnVer" runat="server" CommandName="Ver"
                            CommandArgument='<%# Eval("IdEspecialidad") + "|" + Eval("Activo") %>'
                            CssClass="btn btn-warning btn-sm">Ver</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cambiar estado">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnToggle" runat="server" CommandName="Toggle"
                            CommandArgument='<%# Eval("IdEspecialidad") + "|" + Eval("Activo") %>'
                            CssClass='<%# Convert.ToBoolean(Eval("Activo")) ? "btn btn-sm btn-outline-danger" : "btn btn-sm btn-outline-success" %>'>
                            <%# Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Activar" %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
