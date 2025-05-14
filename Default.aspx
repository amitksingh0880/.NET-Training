<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CountryCodeApplication.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Phone Entry Form</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f6fa;
            margin: 0;
            padding: 0;
        }

        .form-container {
            max-width: 500px;
            margin: 60px auto;
            background-color: #ffffff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        }

        h2 {
            margin-bottom: 20px;
            color: #333;
        }

        select, input[type="text"], .aspNet-Button {
            width: 100%;
            padding: 10px;
            margin-top: 5px;
            margin-bottom: 20px;
            font-size: 1rem;
            border: 1px solid #ccc;
            border-radius: 6px;
        }

        .aspNet-Button {
            background-color: #4169e1;
            color: white;
            border: none;
            cursor: pointer;
            font-weight: bold;
            transition: background-color 0.3s ease;
        }

        .aspNet-Button:hover {
            background-color: #3558c8;
        }

        .message-label {
            font-weight: bold;
            margin-top: 10px;
        }

        .grid-container {
            margin-top: 30px;
        }

        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }

        .gridview th {
            background-color: #4169e1;
            color: white;
            padding: 10px;
        }

        .gridview td {
            border: 1px solid #ddd;
            padding: 8px;
        }

        .gridview tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .gridview tr:hover {
            background-color: #eef;
        }

        .link-button {
            display: inline-block;
            margin-top: 30px;
            text-decoration: none;
            color: #4169e1;
            font-weight: bold;
        }

        .link-button:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>Phone Entry Form</h2>
            <asp:DropDownList ID="ddlCountries" runat="server" AppendDataBoundItems="true" />
            
            <asp:TextBox ID="txtPhoneNumber" runat="server" placeholder="Enter phone number" />
            
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="aspNet-Button" OnClick="btnSubmit_Click" />
            
            <asp:Label ID="lblMessage" runat="server" CssClass="message-label" ForeColor="Green" />

            <div class="grid-container">
                <asp:GridView ID="gvPhoneNumbers" runat="server" AutoGenerateColumns="false"
                    CssClass="gridview"
                    AllowPaging="true" PageSize="5"
                    BorderStyle="Solid" BorderWidth="1" BorderColor="#999"
                    OnPageIndexChanging="gvPhoneNumbers_PageIndexChanging"
                    OnSelectedIndexChanged="gvPhoneNumbers_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" />
                        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
                        <asp:BoundField DataField="CountryName" HeaderText="Country" />
                    </Columns>
                </asp:GridView>
            </div>

            <a class="link-button" href="DataLinkLibraryImplementation.aspx">→ Age Calculator through DLL</a>
        </div>
    </form>
    <a class="link-button" href="Dashboard.aspx">→ Navigate to Dashboard...</a>
</body>
</html>
