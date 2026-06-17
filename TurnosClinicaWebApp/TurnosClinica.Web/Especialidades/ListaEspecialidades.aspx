<%@ Page Title="Lista de Especialidades" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListaEspecialidades.aspx.cs" Inherits="TurnosClinica.Web.ListaEspecialidades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container mt-4">
        
        <div class="row mb-3">
            <div class="col">
                <h1 class="h3 mb-3">Administración de Especialidades</h1>
                <p class="text-secondary">Desde aquí puedes ver todas las especialidades registradas en la clínica.</p>
            </div>
            <div class="col text-end">
                <a href="FormularioEspecialidad.aspx" class="btn btn-primary">
                    + Nueva Especialidad
                </a>
            </div>
        </div>

       
        <div class="row mb-3">
            <div class="col-md-4 d-flex align-items-center">
                <asp:Label ID="lblFiltrar" runat="server" Text="Filtrar:" CssClass="me-2 fw-bold"></asp:Label>
                
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" 
                             AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged" 
                             placeholder="Buscar especialidad...">
                </asp:TextBox>
            </div>
        </div>
        
 
        <table class="table table-striped table-hover table-bordered align-middle">
            <thead class="table-dark">
                <tr>
                    <th scope="col"># ID</th>
                    <th scope="col">Nombre</th>
                    <th scope="col">Descripción</th>
                    <th scope="col">Estado</th>
                    <th scope="col">Acciones</th>
                </tr>
            </thead>
            <tbody>
                
                <% foreach (TurnosClinica.Dominio.Entidades.Especialidad esp in ListaEspecialidad) { %>
                    <tr>
                        <td><%: esp.IdEspecialidad %></td>
                        <td class="fw-bold"><%: esp.Nombre %></td>
                        <td><%: esp.Descripcion %></td>
                        <td>
                            <% if (esp.Activo) { %>
                                <span class="badge bg-success">Activo</span>
                            <% } else { %>
                                <span class="badge bg-danger">Inactivo</span>
                            <% } %>
                        </td>
                        <td>
                            <a href="FormularioEspecialidad.aspx?id=<%: esp.IdEspecialidad %>" class="btn btn-warning btn-sm">
                                Modificar
                            </a>
                        </td>
                    </tr>
                <% } %>
                
            </tbody>
        </table>
    </div>

</asp:Content>
