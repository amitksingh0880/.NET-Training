<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsersData.aspx.cs" Inherits="CountryCodeApplication.UsersData" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Users Data</title>
    <style>
        .grid-container {
            width: 80%;
            margin: 20px auto;
        }
        .grid-view {
            border-collapse: collapse;
            width: 100%;
            font-family: Arial, sans-serif;
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
            font-family: Arial, sans-serif;
        }
        .grid-view a {
            color: #0066cc;
            text-decoration: none;
        }
        .grid-view a:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="grid-container">
            <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
            <asp:GridView ID="userDatas" runat="server" AutoGenerateColumns="false" 
                CssClass="grid-view" BorderStyle="Solid" BorderWidth="1" BorderColor="#999">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="ID" />
                    <asp:HyperLinkField DataTextField="Username" DataNavigateUrlFields="Username" 
                        DataNavigateUrlFormatString="UserDetail.aspx?username={0}" HeaderText="UserName" />
                    <asp:BoundField DataField="IsActive" HeaderText="IsActive" />
                </Columns>
            </asp:GridView>
            
        </div>
    </form>
</body>
</html>