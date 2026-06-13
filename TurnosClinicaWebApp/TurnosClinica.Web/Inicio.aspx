<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="TurnosClinica.Web.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Turnos Clinica</h1>
    <p>Sistema de turnos medicos online.</p>

    <div class="row row-cols-1 row-cols-md-3 g-4">
        <asp:Repeater runat="server" ID="RptResumen">
            <ItemTemplate>
                <div class="col">
                    <div class="card">
                        <div class="card-body">
                            <h5 class="card-title"><%# Eval("Titulo") %></h5>
                            <p class="card-text"><%# Eval("Descripcion") %></p>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
