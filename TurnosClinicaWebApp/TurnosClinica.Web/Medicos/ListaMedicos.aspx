<%@ Page Title="Lista de Medicos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListaMedicos.aspx.cs" Inherits="TurnosClinica.Web.ListaMedicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h1 class="h3 mb-3">Administracion de Medicos</h1>
                <p class="text-secondary">Listado de medicos registrados</p>
            </div>
            <div class="col text-end">
                <a href="FormularioMedico.aspx" class="btn btn-primary">+ Nuevo Medico</a>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-12 col-md-6">
                <asp:Label runat="server" Text="Filtrar" CssClass="form-label" />
                <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control" AutoPostBack="true" OnTextChanged="filtro_TextChanged" />
            </div>
            <div class="col-12 col-md-6 d-flex align-items-end gap-2">
                <asp:CheckBox Text="Filtro avanzado" ID="chkAvanzado" runat="server"
                    AutoPostBack="true" OnCheckedChanged="chkAvanzado_CheckedChanged" />
                <asp:Button Text="Limpiar" runat="server" CssClass="btn btn-outline-primary" ID="btnLimpiar" OnClick="btnLimpiar_Click" />
            </div>
        </div>

        <% if (chkAvanzado.Checked)
           { %>
        <div class="row mb-3">
            <div class="col-12 col-md-3">
                <asp:Label Text="Campo" ID="lblCampo" runat="server" CssClass="form-label" />
                <asp:DropDownList runat="server" AutoPostBack="true" CssClass="form-control" ID="ddlCampo" OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged">
                    <asp:ListItem Text="Nombre" />
                    <asp:ListItem Text="Apellido" />
                    <asp:ListItem Text="Matricula" />
                    <asp:ListItem Text="DNI" />
                    <asp:ListItem Text="Email" />
                </asp:DropDownList>
            </div>
            <div class="col-12 col-md-3">
                <asp:Label Text="Criterio" runat="server" CssClass="form-label" />
                <asp:DropDownList runat="server" ID="ddlCriterio" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-3">
                <asp:Label Text="Filtro" runat="server" CssClass="form-label" />
                <asp:TextBox runat="server" ID="txtFiltroAvanzado" CssClass="form-control" />
            </div>
            <div class="col-12 col-md-3">
                <asp:Label Text="Estado" runat="server" CssClass="form-label" />
                <asp:DropDownList runat="server" ID="ddlEstado" CssClass="form-control">
                    <asp:ListItem Text="Todos" />
                    <asp:ListItem Text="Activo" />
                    <asp:ListItem Text="Inactivo" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="row mb-3">
            <div class="col-12">
                <asp:Button Text="Buscar" runat="server" CssClass="btn btn-primary" ID="btnBuscar" OnClick="btnBuscar_Click" />
            </div>
        </div>
        <% } %>

        <asp:GridView ID="dgvMedicos" runat="server" AutoGenerateColumns="false"
            CssClass="table table-striped table-hover table-bordered align-middle"
            GridLines="None" UseAccessibleHeader="true" HeaderStyle-CssClass="table-dark"
            OnRowCommand="dgvMedicos_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Matricula" DataField="Matricula" />
                <asp:TemplateField HeaderText="Nombre"><ItemTemplate><%#: Eval("Persona.Nombre") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Apellido"><ItemTemplate><%#: Eval("Persona.Apellido") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DNI"><ItemTemplate><%#: Eval("Persona.DNI") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Email"><ItemTemplate><%#: Eval("Persona.Email") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("Activo")) ? "<span class='badge bg-success'>Activo</span>" : "<span class='badge bg-danger'>Inactivo</span>" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ver">
                    <ItemTemplate>
                        <asp:LinkButton ID="BtnVer" runat="server" CommandName="Ver"
                            CommandArgument='<%# Eval("IdMedico") + "|" + Eval("Activo") %>'
                            CssClass="btn btn-warning btn-sm">Ver</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cambiar estado">
                    <ItemTemplate>
                        <asp:LinkButton ID="BtnToggle" runat="server" CommandName="Toggle"
                            CommandArgument='<%# Eval("IdMedico") + "|" + Eval("Activo") %>'
                            CssClass='<%# Convert.ToBoolean(Eval("Activo")) ? "btn btn-sm btn-outline-danger" : "btn btn-sm btn-outline-success" %>'>
                            <%# Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Activar" %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
