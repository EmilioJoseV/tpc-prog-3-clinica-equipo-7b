<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="TurnosClinica.Web.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div class="row justify-content-center mt-5">
        <div class="col-12 col-md-8 col-lg-6">
            <div class="card shadow-sm">
                <div class="card-body p-4">
                    <h2 class="text-center mb-4">Registro de Nuevo Paciente</h2>

                    <div class="row">

                        <div class="col-md-6 mb-3">
                            <asp:Label ID="LblNombre" runat="server" Text="Nombre" CssClass="form-label" />
                            <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control" placeholder="Ej: Juan" />
                            <asp:RequiredFieldValidator ID="ReqNombre" runat="server"
                                ControlToValidate="TxtNombre"
                                ErrorMessage="Debe completar el nombre"
                                ForeColor="Red"
                                Display="Dynamic" />
                        </div>


                        <div class="col-md-6 mb-3">
                            <asp:Label ID="LblApellido" runat="server" Text="Apellido" CssClass="form-label" />
                            <asp:TextBox ID="TxtApellido" runat="server" CssClass="form-control" placeholder="Ej: Perez" />
                        </div>
                    </div>


                    <div class="mb-3">
                        <asp:Label ID="LblDni" runat="server" Text="DNI" CssClass="form-label" />
                        <asp:TextBox ID="TxtDni" runat="server" CssClass="form-control" placeholder="Sin puntos" />
                    </div>


                    <div class="mb-3">
                        <asp:Label ID="LblEmail" runat="server" Text="Correo Electrónico" CssClass="form-label" />
                        <asp:TextBox ID="TxtEmail" runat="server" TextMode="Email" CssClass="form-control" placeholder="juan@ejemplo.com" />
                    </div>


                    <div class="mb-4">
                        <asp:Label ID="LblContrasena" runat="server" Text="Contraseña" CssClass="form-label" />
                        <asp:TextBox ID="TxtContrasena" runat="server" TextMode="Password" CssClass="form-control" />
                    </div>


                    <div class="d-grid gap-2">
                        <asp:Button ID="BtnCrearCuenta" runat="server" CssClass="btn btn-primary" Text="Crear Cuenta" OnClick="BtnCrearCuenta_Click" />
                        <a href="Ingresar.aspx" class="btn btn-outline-secondary">Ya tengo cuenta, quiero ingresar</a>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>



