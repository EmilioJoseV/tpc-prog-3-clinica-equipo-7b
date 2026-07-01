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
            <asp:TextBox ID="TxtMatricula" runat="server" CssClass="form-control" placeholder="Ej: MN 123456" required="required" />
        </div>
        <div class="col-12 col-md-4">
            <asp:Label ID="LblDni" runat="server" Text="DNI" CssClass="form-label" />
            <asp:TextBox ID="TxtDni" runat="server" CssClass="form-control" TextMode="Number" placeholder="Sin puntos (Ej: 30123456)" required="required" min="1" step="1" />
        </div>
        <div class="col-12 col-md-4">
            <asp:Label ID="LblTelefono" runat="server" Text="Telefono" CssClass="form-label" />
            <asp:TextBox ID="TxtTelefono" runat="server" CssClass="form-control" placeholder="Ej: 11-1234-5678" required="required" />
        </div>
        <div class="col-12 col-md-6">
            <asp:Label ID="LblNombre" runat="server" Text="Nombre" CssClass="form-label" />
            <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control" placeholder="Ej: Juan" required="required" />
        </div>
        <div class="col-12 col-md-6">
            <asp:Label ID="LblApellido" runat="server" Text="Apellido" CssClass="form-label" />
            <asp:TextBox ID="TxtApellido" runat="server" CssClass="form-control"  placeholder="Ej: Favaloro" required="required"/>
        </div>
        <div class="col-12">
            <asp:Label ID="LblEmail" runat="server" Text="Email" CssClass="form-label" />
            <asp:TextBox ID="TxtEmail" runat="server" TextMode="Email" CssClass="form-control" placeholder="correo@clinica.com" required="required"/>
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
                    <div class="table-responsive">
                        <asp:CheckBoxList ID="CblEspecialidades" runat="server"
                            CssClass="table table-sm table-borderless align-middle mb-0"
                            RepeatColumns="3"
                            RepeatDirection="Horizontal"
                            RepeatLayout="Table" />
                    </div>
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
                            <asp:TextBox ID="TxtHoraDesde" runat="server" TextMode="Time" CssClass="form-control" />
                        </div>
                        <div class="col-12 col-md-3">
                            <asp:Label ID="LblHoraHasta" runat="server" Text="Hora hasta" CssClass="form-label" />
                            <asp:TextBox ID="TxtHoraHasta" runat="server" TextMode="Time" CssClass="form-control" />
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
                                            <th>Acciones</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="HfIdHorario" runat="server" Value='<%# Eval("IdHorarioDisponibilidadMedico") %>' />
                                                <asp:DropDownList ID="DdlDiaSemanaFila" runat="server" CssClass="form-control"
                                                    SelectedValue='<%# Convert.ToInt32(Eval("DiaSemana")).ToString() %>'>
                                                    <asp:ListItem Text="Lunes" Value="1" />
                                                    <asp:ListItem Text="Martes" Value="2" />
                                                    <asp:ListItem Text="Miercoles" Value="3" />
                                                    <asp:ListItem Text="Jueves" Value="4" />
                                                    <asp:ListItem Text="Viernes" Value="5" />
                                                    <asp:ListItem Text="Sabado" Value="6" />
                                                    <asp:ListItem Text="Domingo" Value="7" />
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtHoraDesdeFila" runat="server" TextMode="Time"
                                                    CssClass="form-control" Text='<%# FormatearHora(Eval("HoraDesde")) %>' />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtHoraHastaFila" runat="server" TextMode="Time"
                                                    CssClass="form-control" Text='<%# FormatearHora(Eval("HoraHasta")) %>' />
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
        <asp:Button ID="BtnCancelar" runat="server" CssClass="btn btn-outline-primary" Text="Cancelar"
            OnClick="BtnCancelar_Click" CausesValidation="false" UseSubmitBehavior="false" />
    </div>
</asp:Content>
