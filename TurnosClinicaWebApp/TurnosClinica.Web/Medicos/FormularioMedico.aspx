<%@ Page Title="Formulario de Medico" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioMedico.aspx.cs" Inherits="TurnosClinica.Web.FormularioMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="h3 mb-3">Formulario de Medico</h1>
    <p class="text-secondary">Complete los datos requeridos. La cuenta de acceso se genera automaticamente en estado pendiente.</p>

    <asp:HiddenField ID="HfIdMedico" runat="server" />
    <asp:HiddenField ID="HfIdPersona" runat="server" />

    <div class="row g-3">
        <div class="col-12 col-md-4">
            <asp:Label ID="LblMatricula" runat="server" Text="Matricula" CssClass="form-label" />
            <asp:TextBox ID="TxtMatricula" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-4">
            <asp:Label ID="LblDni" runat="server" Text="DNI" CssClass="form-label" />
            <asp:TextBox ID="TxtDni" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-4">
            <asp:Label ID="LblTelefono" runat="server" Text="Telefono" CssClass="form-label" />
            <asp:TextBox ID="TxtTelefono" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-6">
            <asp:Label ID="LblNombre" runat="server" Text="Nombre" CssClass="form-label" />
            <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-6">
            <asp:Label ID="LblApellido" runat="server" Text="Apellido" CssClass="form-label" />
            <asp:TextBox ID="TxtApellido" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12">
            <asp:Label ID="LblEmail" runat="server" Text="Email" CssClass="form-label" />
            <asp:TextBox ID="TxtEmail" runat="server" TextMode="Email" CssClass="form-control" />
        </div>
        <div class="col-12">
            <asp:CheckBox ID="ChkMedicoActivo" runat="server" Text="Activo" Checked="true" />
        </div>
    </div>

    <div class="row mt-4">
        <div class="col-12">
            <div class="card">
                <div class="card-body">
                    <h2 class="h5 mb-3">Especialidades</h2>
                    <asp:CheckBoxList ID="CblEspecialidades" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" />
                </div>
            </div>
        </div>
    </div>

    <div class="row mt-4">
        <div class="col-12">
            <div class="card">
                <div class="card-body">
                    <h2 class="h5 mb-3">Horarios de disponibilidad</h2>
                    <div class="row g-3">
                        <div class="col-12 col-md-3">
                            <asp:Label ID="LblDiaSemana" runat="server" Text="Dia" CssClass="form-label" />
                            <asp:DropDownList ID="DdlDiaSemana" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-3">
                            <asp:Label ID="LblHoraDesde" runat="server" Text="Hora desde" CssClass="form-label" />
                            <asp:TextBox ID="TxtHoraDesde" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-3">
                            <asp:Label ID="LblHoraHasta" runat="server" Text="Hora hasta" CssClass="form-label" />
                            <asp:TextBox ID="TxtHoraHasta" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-1">
                            <asp:Button ID="BtnAgregarHorario" runat="server" CssClass="btn btn-outline-primary" Text="Agregar" OnClick="BtnAgregarHorario_Click" />
                        </div>
                    </div>

                    <div class="mt-3">
                        <asp:Repeater ID="RptHorarios" runat="server" OnItemCommand="RptHorarios_ItemCommand">
                            <HeaderTemplate>
                                <table class="table table-striped align-middle">
                                    <thead>
                                        <tr>
                                            <th>Dia</th>
                                            <th>Desde</th>
                                            <th>Hasta</th>
                                            <th>Accion</th>
                                            <th>Acciones</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                        <tr>
                                            <td><%# ObtenerDiaSemanaTexto(Eval("DiaSemana")) %></td>
                                            <td><%# FormatearHora(Eval("HoraDesde")) %></td>
                                            <td><%# FormatearHora(Eval("HoraHasta")) %></td>
                                            <td>
                                                <asp:LinkButton ID="BtnEditarHorario" runat="server" CssClass="btn btn-outline-secondary" CommandName="EditarHorario" CommandArgument='<%# Container.ItemIndex %>'>Editar</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="BtnEliminarHorario" runat="server" CssClass="btn btn-outline-danger" CommandName="EliminarHorario" CommandArgument='<%# Container.ItemIndex %>'>Quitar</asp:LinkButton>
                                            </td>
                                        </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="mt-4">
        <asp:Button ID="BtnGuardar" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="BtnGuardar_Click" />
        <asp:Button ID="BtnCancelar" runat="server" CssClass="btn btn-outline-primary" Text="Cancelar" OnClick="BtnCancelar_Click" />
    </div>
</asp:Content>
