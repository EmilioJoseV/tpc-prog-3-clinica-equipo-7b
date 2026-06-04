<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AltaPaciente.aspx.cs" Inherits="TurnosClinica.Web.AltaPaciente" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Alta de Paciente</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Alta de Paciente</h1>
        <p>Esta pagina esta pensada para cargar todos los datos necesarios para dar de alta un paciente.</p>

        <div>
            <asp:Label ID="LblDni" runat="server" AssociatedControlID="TxtDni" Text="DNI" />
            <br />
            <asp:TextBox ID="TxtDni" runat="server" />
        </div>
        <div>
            <asp:Label ID="LblNombre" runat="server" AssociatedControlID="TxtNombre" Text="Nombre" />
            <br />
            <asp:TextBox ID="TxtNombre" runat="server" />
        </div>
        <div>
            <asp:Label ID="LblApellido" runat="server" AssociatedControlID="TxtApellido" Text="Apellido" />
            <br />
            <asp:TextBox ID="TxtApellido" runat="server" />
        </div>
        <div>
            <asp:Label ID="LblFechaNacimiento" runat="server" AssociatedControlID="TxtFechaNacimiento" Text="Fecha de nacimiento" />
            <br />
            <asp:TextBox ID="TxtFechaNacimiento" runat="server" TextMode="Date" />
        </div>
        <div>
            <asp:Label ID="LblTelefono" runat="server" AssociatedControlID="TxtTelefono" Text="Telefono" />
            <br />
            <asp:TextBox ID="TxtTelefono" runat="server" />
        </div>
        <div>
            <asp:Label ID="LblEmail" runat="server" AssociatedControlID="TxtEmail" Text="Email" />
            <br />
            <asp:TextBox ID="TxtEmail" runat="server" TextMode="Email" />
        </div>
        <div>
            <asp:Label ID="LblDireccion" runat="server" AssociatedControlID="TxtDireccion" Text="Direccion" />
            <br />
            <asp:TextBox ID="TxtDireccion" runat="server" />
        </div>
        <div>
            <asp:CheckBox ID="ChkActivo" runat="server" Text="Activo" Checked="true" />
        </div>
    </form>
</body>
</html>
