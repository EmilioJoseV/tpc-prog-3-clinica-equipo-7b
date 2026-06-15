<%@ Page Title="Error" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="TurnosClinica.Web.ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>    <div class="alert alert-danger" role="alert">
        <h4 class="alert-heading"> Ha ocurrido un error</h4>
        <hr>
        <asp:Label ID="lblMensajeError" runat="server" Text="Error desconocido"></asp:Label>
        <br /><br />
        <a href="Default.aspx" class="btn btn-primary">Volver al Inicio</a>
    </div>

</asp:Content>
