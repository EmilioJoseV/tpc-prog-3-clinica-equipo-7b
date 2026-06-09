<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="TurnosClinica.Web.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mb-4">
        <h1 class="display-6 fw-semibold">Turnos Clinica</h1>
        <p class="text-secondary mb-3">
            Sistema de turnos medicos online.
        </p>
        <asp:Button ID="BtnIrLogin" runat="server" CssClass="btn btn-primary" Text="Ir al login" OnClick="BtnIrLogin_Click" />
    </div>

    <div class="row row-cols-1 row-cols-md-3 g-4">
        <asp:Repeater runat="server" ID="RptResumen">
            <ItemTemplate>
                <div class="col">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h2 class="h5 card-title"><%# Eval("Titulo") %></h2>
                            <p class="card-text text-secondary mb-0"><%# Eval("Descripcion") %></p>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
