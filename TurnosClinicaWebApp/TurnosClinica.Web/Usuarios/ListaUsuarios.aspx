<%@ Page Title="Lista de Usuarios" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListaUsuarios.aspx.cs" Inherits="TurnosClinica.Web.ListaUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h1 class="h3 mb-3">Lista de Usuarios</h1>
                <p class="text-secondary">Esta página está pensada para administrar usuarios.</p>
            </div>
            <div class="col text-end">
                <a href="FormularioUsuario.aspx" class="btn btn-primary">
                    + Nuevo Usuario
                </a>
            </div>
        </div>

        <table class="table table-striped table-hover table-bordered align-middle">
            <thead class="table-dark">
                <tr>
                    <th scope="col"># ID</th>
                    <th scope="col">Nombre de Usuario</th>
                    <th scope="col">Email</th>
                    <th scope="col">Rol (ID)</th>
                    <th scope="col">Médico (ID)</th>
                    <th scope="col">Estado</th>
                    <th scope="col">Acciones</th>
                </tr>
            </thead>
            <tbody>
                <% foreach (TurnosClinica.Dominio.Entidades.Usuario user in ListaUsuariosProp) { %>
                    <tr>
                        <td><%: user.IdUsuario %></td>
                        <td class="fw-bold"><%: user.NombreUsuario %></td>
                        <td><%: user.Email %></td>
                        <td>
                            <%: user.Rol != null ? user.Rol.IdRol.ToString() : "Sin Rol" %>
                        </td>
                        <td>
                            <%: user.Medico != null ? user.Medico.IdMedico.ToString() : "No Asignado" %>
                        </td>
                        <td>
                            <% if (user.Activo) { %>
                                <span class="badge bg-success">Activo</span>
                            <% } else { %>
                                <span class="badge bg-danger">Inactivo</span>
                            <% } %>
                        </td>
                        <td>
                            <a href="FormularioUsuario.aspx?id=<%: user.IdUsuario %>" class="btn btn-warning btn-sm">
                                Modificar
                            </a>
                        </td>
                    </tr>
                <% } %>
            </tbody>
        </table>
    </div>
</asp:Content>