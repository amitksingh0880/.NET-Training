<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDetail.aspx.cs" Inherits="CountryCodeApplication.UserDetail" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Details</title>
    <style>
        .details-container {
            width: 80%;
            margin: 20px auto;
            padding: 20px;
            font-family: Arial, sans-serif;
        }
        .grid-view {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }
        .grid-view th, .grid-view td {
            border: 1px solid #999;
            padding: 8px;
            text-align: left;
        }
        .grid-view th {
            background-color: #f2f2f2;
            font-weight: bold;
        }
        .error {
            color: red;
            margin-bottom: 10px;
        }
        .back-link {
            display: block;
            margin-top: 10px;
            color: #0066cc;
            text-decoration: none;
        }
        .back-link:hover {
            text-decoration: underline;
        }
        .edit-button {
            background-color: #4CAF50;
            color: white;
            border: none;
            padding: 5px 10px;
            cursor: pointer;
        }
        .edit-button:hover {
            background-color: #45a049;
        }
        .profile-image {
            width: 50px;
            height: 50px;
            object-fit: cover;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="details-container">
            <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
            <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns="False" 
                CssClass="grid-view" DataKeyNames="Id">
                <Columns>
                    <asp:TemplateField HeaderText="Profile Picture">
                        <ItemTemplate>
                            <asp:Image ID="imgProfile" runat="server" CssClass="profile-image" 
                                ImageUrl='<%# Eval("ProfilePicture") %>' 
                                AlternateText="Profile Picture" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FullName" HeaderText="FullName" ReadOnly="True" />
                    <asp:BoundField DataField="Address" HeaderText="Address" ReadOnly="True" />
                    <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber" ReadOnly="True" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" ReadOnly="True" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="edit-button"
                                CommandName="EditUser" CommandArgument='<%# Eval("UserId") %>'
                                OnCommand="btnEdit_Command" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <a href="UsersData.aspx" class="back-link">Back to Users Data</a>
        </div>
    </form>
</body>
</html>