<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddRoute.aspx.cs" Inherits="CountryCodeApplication.AddRoute" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Route</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 400px; margin: 20px auto; padding: 20px; border: 1px solid #ccc;">
            <h2>Add New Route</h2>

            <asp:Label ID="lblRouteName" runat="server" Text="Route Name:" /><br />
            <asp:TextBox ID="txtRouteName" runat="server" Width="100%" /><br /><br />

            <asp:Label ID="lblRouteURL" runat="server" Text="Route URL:" /><br />
            <asp:TextBox ID="txtRouteURL" runat="server" Width="100%" /><br /><br />

            <asp:Label ID="lblPageHandler" runat="server" Text="Page Handler:" /><br />
            <asp:TextBox ID="txtPageHandler" runat="server" Width="100%" /><br /><br />

            <asp:Label ID="lblParentRouteID" runat="server" Text="Parent Route ID (optional):" /><br />
            <asp:TextBox ID="txtParentRouteID" runat="server" Width="100%" /><br /><br />

            <asp:Button ID="btnSubmit" runat="server" Text="Add Route" OnClick="btnSubmit_Click" /><br /><br />

            <asp:Label ID="lblRoles" runat="server" Text="Assign Roles:" Visible="false" /><br />
            <asp:CheckBoxList ID="chkRoles" runat="server" RepeatDirection="Horizontal" Visible="false">
                <asp:ListItem Text="Admin" Value="1" />
                <asp:ListItem Text="User" Value="2" />
                <asp:ListItem Text="Manager" Value="3" />
            </asp:CheckBoxList>
            <br />
            <asp:Button ID="btnAssignRoles" runat="server" Text="Assign Roles to Route" OnClick="btnAssignRoles_Click" Enabled="false" /><br /><br />

            <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
        </div>
        <a class="link-button" href="Dashboard.aspx">→ Navigate to Dashboard...</a>
    </form>
</body>
</html>
