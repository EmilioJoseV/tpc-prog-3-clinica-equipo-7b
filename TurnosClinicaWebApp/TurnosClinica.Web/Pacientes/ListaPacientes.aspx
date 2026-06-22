<%@ Page Title="Lista de Pacientes" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListaPacientes.aspx.cs" Inherits="TurnosClinica.Web.ListaPacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h1 class="h3 mb-3">Administracion de Pacientes</h1>
                <p class="text-secondary">Listado de pacientes registrados</p>
            </div>
            <div class="col text-end">
                <a href="FormularioPaciente.aspx" class="btn btn-primary">+ Nuevo Paciente</a>
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
                <asp:DropDownList ID="ddlCampo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged">
                    <asp:ListItem Text="Nombre" />
                    <asp:ListItem Text="Apellido" />
                    <asp:ListItem Text="DNI" />
                    <asp:ListItem Text="Email" />
                    <asp:ListItem Text="Direccion" />
                </asp:DropDownList>
            </div>
            <div class="col-12 col-md-3">
                <asp:Label runat="server" Text="Criterio" CssClass="form-label" />
                <asp:DropDownList ID="ddlCriterio" runat="server" CssClass="form-control">
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

        <asp:GridView ID="dgvPacientes" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover table-bordered align-middle"
            GridLines="None" UseAccessibleHeader="true" HeaderStyle-CssClass="table-dark" OnRowCommand="dgvPacientes_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="DNI"><ItemTemplate><%#: Eval("Persona.DNI") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre"><ItemTemplate><%#: Eval("Persona.Nombre") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Apellido"><ItemTemplate><%#: Eval("Persona.Apellido") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Telefono"><ItemTemplate><%#: Eval("Persona.Telefono") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Email"><ItemTemplate><%#: Eval("Persona.Email") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("Activo")) ? "<span class='badge bg-success'>Activo</span>" : "<span class='badge bg-danger'>Inactivo</span>" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ver">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnVer" runat="server" CommandName="Ver"
                            CommandArgument='<%# Eval("IdPaciente") + "|" + Eval("Activo") %>'
                            CssClass="btn btn-warning btn-sm">Ver</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cambiar estado">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnToggle" runat="server" CommandName="Toggle"
                            CommandArgument='<%# Eval("IdPaciente") + "|" + Eval("Activo") %>'
                            CssClass='<%# Convert.ToBoolean(Eval("Activo")) ? "btn btn-sm btn-outline-danger" : "btn btn-sm btn-outline-success" %>'>
                            <%# Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Activar" %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
