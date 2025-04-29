<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CountryLayering.aspx.cs" Inherits="CountryCodeApplication.CountryLayering" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Country State District Selection</title>
    <style>
        .error { color: red; }
        .success { color: green; }
        table { margin: 20px; }
        td { padding: 5px; }
        .button { margin-top: 10px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Select Location</h2>
            <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false"></asp:Label>
            <table>
                <tr>
                    <td>Country:</td>
                    <td>
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" 
                            OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                            <asp:ListItem Value="0">Select Country</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>State:</td>
                    <td>
                        <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" 
                            OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                            <asp:ListItem Value="0">Select State</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>District:</td>
                    <td>
                        <asp:DropDownList ID="ddlDistrict" runat="server">
                            <asp:ListItem Value="0">Select District</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save Selection" CssClass="button" 
                            OnClick="btnSave_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <label id="country"></label>
        </div>
        <a href="Dashboard.aspx">Navigate to Dashboard</a>
    </form>
</body>
</html>