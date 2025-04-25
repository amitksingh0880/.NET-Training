<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUserDetails.aspx.cs" Inherits="CountryCodeApplication.AddUserDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Details</title>
    <style>
        .Hype {
            display: flex;
            justify-content: center;
            gap: 20px;
        }
        .Hype a {
            padding: 12px 25px;
            background-color: #007bff;
            color: #fff;
            text-decoration: none;
            border-radius: 50px;
            font-size: 16px;
            font-weight: 500;
            transition: background-color 0.3s ease, transform 0.3s ease;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        .Hype a:hover {
            background-color: #0056b3;
            transform: scale(1.05);
        }
        .profile-image {
            max-width: 150px;
            max-height: 150px;
            border-radius: 8px;
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 20px; max-width: 600px; margin: auto; background-color: #f9f9f9; border-radius: 8px;">
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
            <h2 style="text-align: center;">User Details</h2>
            <table style="width: 100%;">
                <tr>
                    <td><strong>Profile Image:</strong></td>
                    <td>
                        <asp:Image ID="imgProfile" runat="server" CssClass="profile-image" Visible="false" />
                        <asp:FileUpload ID="fuProfileImage" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td><strong>Username:</strong></td>
                    <td><asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" /></td>
                </tr>
                <tr>
                    <td><strong>Full Name:</strong></td>
                    <td><asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" /></td>
                </tr>
                <tr>
                    <td><strong>Address:</strong></td>
                    <td><asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" /></td>
                </tr>
                <tr>
                    <td><strong>Phone Number:</strong></td>
                    <td><asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" /></td>
                </tr>
                <tr>
                    <td><strong>Gender:</strong></td>
                    <td><asp:TextBox ID="txtGender" runat="server" CssClass="form-control" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center; padding-top: 15px;">
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" CssClass="btn btn-primary" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="Hype">
            <a href="Dashboard.aspx">Dashboard</a>
        </div>
    </form>
</body>
</html>