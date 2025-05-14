<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MasterRoleHandler.aspx.cs" Inherits="CountryCodeApplication.MasterRoleHandler" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>View Routes by Role</title>
    <style>
        body {
            font-family: Arial;
            background-color: #f9f9f9;
        }
        .container {
            max-width: 900px;
            margin: 50px auto;
            padding: 30px;
            background: #fff;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
            border-radius: 10px;
        }
       
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>View Routes by Role</h2>
            <asp:Button ID="Button1" runat="server" Text="Add New Route" OnClick="AddNewRoute_btn" />
            <asp:DropDownList ID="ddlRoles" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged" />
            <asp:GridView ID="gvRoutes" runat="server" AutoGenerateColumns="False" CssClass="grid">
                <Columns>
                    <asp:BoundField DataField="RouteName" HeaderText="Route Name" />
                    <asp:BoundField DataField="RouteURL" HeaderText="Route URL" />
                    <asp:BoundField DataField="PageHandler" HeaderText="Page Handler" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
    <a class="link-button" href="Dashboard.aspx">→ Navigate to Dashboard...</a>
</body>
</html>
