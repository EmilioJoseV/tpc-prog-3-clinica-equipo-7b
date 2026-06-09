<%@ Page Title="Login" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TurnosClinica.Web.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row justify-content-center">
        <div class="col-12 col-md-8 col-lg-5">
            <div class="card shadow-sm">
                <div class="card-body">
                    <h1 class="h4 mb-3">Login</h1>
                    <div class="mb-3">
                        <asp:Label ID="LblUsuario" runat="server" AssociatedControlID="TxtUsuario" Text="Usuario" CssClass="form-label" />
                        <asp:TextBox ID="TxtUsuario" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="LblContrasena" runat="server" AssociatedControlID="TxtContrasena" Text="Password" CssClass="form-label" />
                        <asp:TextBox ID="TxtContrasena" runat="server" TextMode="Password" CssClass="form-control" />
                    </div>
                    <div class="d-grid gap-2">
                        <asp:Button ID="BtnAcceder" runat="server" CssClass="btn btn-primary" Text="Acceder" OnClick="BtnAcceder_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
