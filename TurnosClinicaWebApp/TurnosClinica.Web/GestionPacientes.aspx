<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionPacientes.aspx.cs" Inherits="TurnosClinica.Web.GestionPacientes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Registro de Nuevo Paciente<br />
            <br />
        </div>
        <br />
        <br />
&nbsp;<br />
        <br />
        <br />
        <br />
        <table class="auto-style1cellpadding="10" cellspacing="5"">
            <tr>
                <td>Nombre<asp:TextBox ID="TextNombre" runat="server" OnTextChanged="TextBox1_TextChanged" Width="133px"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Apellido<asp:TextBox ID="TextApellido" runat="server" OnTextChanged="TextBox2_TextChanged"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Dni<asp:TextBox ID="TextDni" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Correo Electronico<asp:TextBox ID="TextCorreoElectronico" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        <p>
            <asp:Button ID="Button1" runat="server" BackColor="#99CCFF" Font-Bold="True" ForeColor="Black" Text="Guardar Paciente" />
        </p>
    </form>
</body>
</html>
