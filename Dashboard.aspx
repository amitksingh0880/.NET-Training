<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CountryCodeApplication.Dashboard" %>

<!DOCTYPE html>
<html>
<head>
    <title>Dashboard</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f6fa;
            margin: 0;
            padding: 0;
        }

        .dashboard-container {
            width: 80%;
            margin: 50px auto;
            padding: 20px;
            background-color: #ffffff;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            border-radius: 10px;
        }

        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background-color: #4169e1;
            color: white;
            padding: 20px;
            border-radius: 8px;
        }

        .btn-logout {
            background-color: #dc3545;
            color: white;
            padding: 10px 25px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-weight: bold;
            transition: background-color 0.3s ease;
        }

            .btn-logout:hover {
                background-color: #c82333;
            }

        .btn-deactivate {
            background-color: #ffc107;
            color: black;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-weight: bold;
            margin-top: 10px;
            transition: background-color 0.3s ease;
        }

            .btn-deactivate:hover {
                background-color: #e0a800;
            }

        .error-container {
            margin-top: 20px;
            margin-bottom: 10px;
        }

        #lblError {
            color: red;
            font-family:sans-serif;
            font-size: 0.95rem;
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
    <form id="form2" runat="server">
        <div class="dashboard-container">
            <div class="header">
                <h2>Welcome,
                    <asp:Label ID="lblUsername" runat="server"></asp:Label>
                    (<asp:Label ID="lblRoleName" runat="server"></asp:Label>)</h2>
                <asp:Button ID="Button1" runat="server" Text="Logout" CssClass="btn-logout" OnClick="btnLogout_Click" />
            </div>

            <asp:Button ID="Button2" runat="server" Text="Deactivate My Account" CssClass="btn-deactivate"
                OnClick="btnDeactivate_Click" OnClientClick="return confirm('Are you sure you want to deactivate your account?');" />
            <div class="links">
                <h2>Welcome to your dashboard...</h2>
                <div class="error-container">
                    <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
                </div>
                <asp:Label ID="CurrentUrl" runat="server"></asp:Label>
                <asp:Repeater ID="rptMenu" runat="server">
                    <ItemTemplate>
                        <a href='<%# Eval("RouteURL") %>'><%# Eval("RouteName") %></a>
                    </ItemTemplate>
                </asp:Repeater>
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
