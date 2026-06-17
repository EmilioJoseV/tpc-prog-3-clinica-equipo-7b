<%@ Page Title="Configuracion de Turnos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ConfiguracionTurnos.aspx.cs" Inherits="TurnosClinica.Web.ConfiguracionTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="h3 mb-3">Configuracion de Turnos</h1>
    <p class="text-secondary">En esta seccion puedes definir la duracion global de los turnos en minutos.</p>

    <asp:HiddenField ID="HfIdConfiguracionTurno" runat="server" />

    <div class="row">
        <div class="col-12 col-md-6 col-lg-4">
            <asp:Label ID="LblDuracionMinutos" runat="server" Text="Duracion en minutos" CssClass="form-label" />
            <asp:TextBox ID="TxtDuracionMinutos" runat="server" CssClass="form-control" />
        </div>
    </div>

    <div class="mt-4">
        <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="BtnGuardar_Click" />
        <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-primary" OnClick="BtnCancelar_Click" />
    </div>
</asp:Content>
