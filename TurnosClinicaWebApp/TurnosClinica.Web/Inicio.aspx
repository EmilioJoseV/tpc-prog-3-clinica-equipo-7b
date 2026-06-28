<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="TurnosClinica.Web.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="imagen-inicio text-white d-flex align-items-center">
        <div class="container">
            <div class="col-12 col-md-8 col-lg-6 bg-dark bg-opacity-75 rounded-4 p-4 p-md-5 shadow">
                <h1 class="display-5 fw-semibold mb-3">Turnos Clinica</h1>
                <p class="lead mb-0">
                    Sistema para administrar pacientes, medicos y turnos
                </p>
            </div>
        </div>
    </div>
</asp:Content>
