<%@ Page Title="Error" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="TurnosClinica.Web.ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="alert alert-danger" role="alert">
        <h1 class="h4 alert-heading">Ocurrio un error en el sistema</h1>
        <p>No se pudo completar la operacion, intentee de nuevo mas tarde.</p>
        <hr />
        <a href="Inicio.aspx" class="btn btn-primary">Volver al inicio</a>
    </div>
</asp:Content>
