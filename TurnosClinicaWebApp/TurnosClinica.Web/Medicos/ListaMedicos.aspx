<%@ Page Title="Lista de Medicos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListaMedicos.aspx.cs" Inherits="TurnosClinica.Web.ListaMedicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="h3 mb-3">Lista de Medicos</h1>
    <p class="text-secondary">Alta, edicion y desactivacion de medicos.</p>
    <asp:Button ID="BtnNuevoMedico" runat="server" CssClass="btn btn-primary mb-3" Text="Nuevo medico" OnClick="BtnNuevoMedico_Click" />

    <asp:Repeater ID="RptMedicos" runat="server" OnItemCommand="RptMedicos_ItemCommand">
        <HeaderTemplate>
            <table class="table table-striped align-middle">
                <thead>
                    <tr>
                        <th>Matricula</th>
                        <th>Nombre</th>
                        <th>DNI</th>
                        <th>Email</th>
                        <th>Estado</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
                    <tr>
                        <td><%# Eval("Matricula") %></td>
                        <td><%# Eval("Apellido") %>, <%# Eval("Nombre") %></td>
                        <td><%# Eval("DNI") %></td>
                        <td><%# Eval("Email") %></td>
                        <td><%# ((bool)Eval("Activo")) ? "Activo" : "Inactivo" %></td>
                        <td>
                            <asp:LinkButton ID="BtnEditar" runat="server" CssClass="btn btn-outline-primary" CommandName="Editar" CommandArgument='<%# Eval("IdMedico") %>'>Editar</asp:LinkButton>
                            <asp:LinkButton ID="BtnDesactivar" runat="server" CssClass="btn btn-outline-danger" CommandName="Desactivar" CommandArgument='<%# Eval("IdMedico") %>'>Desactivar</asp:LinkButton>
                        </td>
                    </tr>
        </ItemTemplate>
        <FooterTemplate>
                </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <asp:Panel ID="PnlVacio" runat="server" CssClass="alert alert-info mt-3" Visible="false">
        No hay medicos cargados.
    </asp:Panel>
</asp:Content>
