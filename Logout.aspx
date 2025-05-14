<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="CountryCodeApplication.Logout" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logout</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f6fa;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }
        .logout-container {
            text-align: center;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,00,0,0.1);
        }
        .logout-container h2 {
            color: #333;
            margin-bottom: 10px;
        }
        .logout-container p {
            color: #666;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="logout-container">
            <h2>Logged Out</h2>
            <p>You have been successfully logged out. Redirecting to login page...</p>
        </div>
    </form>
</body>
</html>