<%@ Page Title="Gestion de Pacientes" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GestionPacientes.aspx.cs" Inherits="TurnosClinica.Web.GestionPacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="h3 mb-3">Gestion de Pacientes</h1>
    <p class="text-secondary">Esta pagina esta pensada para la carga, consulta y mantenimiento de pacientes.</p>
    <asp:Button ID="BtnAltaPaciente" runat="server" CssClass="btn btn-primary" Text="Dar de alta paciente" OnClick="BtnAltaPaciente_Click" />
</asp:Content>
