<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CountryCodeApplication.Dashboard" %>

<!DOCTYPE html>
<html>
<head>
    <title>Dashboard</title>
    <style>
        .dashboard-container {
            width: 80%;
            margin: 50px auto;
            padding: 20px;
        }
        .header {
            background-color: #f8f8f8;
            padding: 15px;
            border-radius: 5px;
            display:flex;
            flex-direction:row;
            justify-content:space-between;
        }
        .btn-logout {
            background-color: #ff4444;
            color: white;
            padding: 0px 30px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn-logout:hover {
            background-color: #cc0000;
        }
        .btn-deactivate {
            background-color: #ffd800;
            color: black;
            padding: 8px 15px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            margin-top: 10px;
        }
        .btn-deactivate:hover {
            background-color: #e6c200;
        }
        .error {
            color: red;
            margin-bottom: 10px;
            font-family: Arial, sans-serif;
        }
        .links {
            margin-top: 20px;
        }
        .links a {
            display: block;
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboard-container">
            <div class="header">
                <h2>Welcome, <asp:Label ID="lblUsername" runat="server"></asp:Label></h2>
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn-logout" OnClick="btnLogout_Click" />
            </div>
            <h3>Dashboard</h3>
            <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
            <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate My Account" CssClass="btn-deactivate"
                OnClick="btnDeactivate_Click" OnClientClick="return confirm('Are you sure you want to deactivate your account?');" />
            <div class="links">
                <p>Welcome to your dashboard...</p>
                <a href="Default.aspx">Country Input Form</a>
                <a href="UsersData.aspx">Users Data</a>
                <a href="CountryLayering.aspx">Country layering Selection</a>
                <a href="DataLinkLibraryImplementation.aspx">Age Calculation through dll reference</a>
                <a href="Contact.aspx">Contact Me</a>
                <a href="About.aspx">Wanna know me?</a>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        function showAlert(message) {
            alert(message);
        }
    </script>
</body>
</html>