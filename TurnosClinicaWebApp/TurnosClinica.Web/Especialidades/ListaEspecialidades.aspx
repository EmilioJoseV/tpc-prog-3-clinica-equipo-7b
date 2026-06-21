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
            <div class="row">
        <div class="col-6">
            <div class="mb-3">
                <asp:Label Text="Filtrar" runat="server" />
                <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged" />
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

        <% if (FiltroAvanzado)
            { %>
        <div class="row">
            
                              <div class="col-4">
                    <div class="mb-3">
                        <asp:Label Text="Especialidad" runat="server" />
                        <asp:DropDownList runat="server" ID="ddlFiltroNombre" CssClass="form-control">
                        </asp:DropDownList>
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
 
        <table class="table table-striped table-hover table-bordered align-middle">
            <thead class="table-dark">
                <tr>
                    <th scope="col"># ID</th>
                    <th scope="col">Nombre</th>
                    <th scope="col">Descripción</th>
                    <th scope="col">Estado</th>
                    <th scope="col">Acciones</th>
                    <th scope="col">Baja</th>
                   <th scope ="col">Médicos Asociados</th> 
                    
                          

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
                        
                        <td>
                            
                            <a href="ListaEspecialidades.aspx?idBaja=<%: esp.IdEspecialidad %>" class="btn btn-danger" onclick="return confirm('¿Estás seguro que deseas dar de baja esta especialidad?');">Dar de baja</a>
                        </td>
                        <td><%: ObtenerMedicosPorEspecialidad(esp.IdEspecialidad) %> </td>


                    </tr>
                <% } %>
                
            </tbody>
        </table>
    

</asp:Content>
