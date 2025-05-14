<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CountryLayering.aspx.cs" Inherits="CountryCodeApplication.CountryLayering" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Country State District Selection</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f6fa;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 900px;
            margin: 40px auto;
            padding: 25px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        }

        h2 {
            color: #333;
            margin-top: 0;
        }

        .form-table {
            width: 100%;
            margin: 20px 0;
        }

        .form-table td {
            padding: 10px;
        }

        select, input[type="submit"], .button, .aspNet-Button {
            width: 100%;
            padding: 10px;
            font-size: 1rem;
            border: 1px solid #ccc;
            border-radius: 6px;
        }

        .button, .aspNet-Button {
            background-color: #4169e1;
            color: white;
            border: none;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .button:hover, .aspNet-Button:hover {
            background-color: #3558c8;
        }

        .error {
            color: #d9534f;
            font-weight: bold;
        }

        .success {
            color: #28a745;
            font-weight: bold;
        }

        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin-top: 30px;
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

        .footer-link {
            display: inline-block;
            margin-top: 25px;
            color: #4169e1;
            text-decoration: none;
        }

        .footer-link:hover {
            text-decoration: underline;
        }

        .progress-overlay {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: #fff;
            padding: 20px 40px;
            border: 1px solid #ccc;
            box-shadow: 0 0 10px rgba(0,0,0,0.2);
            z-index: 1000;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container">
                    <h2>Select Location</h2>
                    <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />

                    <table class="form-table">
                        <tr>
                            <td>Country:</td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>State:</td>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlState_SelectedIndexChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>District:</td>
                            <td>
                                <asp:DropDownList ID="ddlDistrict" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save Selection" CssClass="aspNet-Button"
                                    OnClick="btnSave_Click" />
                            </td>
                        </tr>
                    </table>

                    <h2>Your Selections</h2>
                    <asp:GridView ID="gvUserSelections" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                        CssClass="gridview" DataKeyNames="UserId"
                        OnPageIndexChanging="gvUserSelections_PageIndexChanging"
                        OnRowEditing="gvUserSelections_RowEditing"
                        OnRowCancelingEdit="gvUserSelections_RowCancelingEdit"
                        OnRowUpdating="gvUserSelections_RowUpdating">
                        <Columns>
                            <asp:BoundField DataField="UserId" HeaderText="User ID" ReadOnly="true" />
                            <asp:BoundField DataField="Username" HeaderText="Username" />
                            <asp:TemplateField HeaderText="Country">
                                <ItemTemplate><%# Eval("CountryName") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlCountryEdit" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlCountryEdit_SelectedIndexChanged" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="State">
                                <ItemTemplate><%# Eval("StateName") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlStateEdit" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlStateEdit_SelectedIndexChanged" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="District">
                                <ItemTemplate><%# Eval("DistrictName") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlDistrictEdit" runat="server" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                    </asp:GridView>

                    <a href="Dashboard.aspx" class="footer-link">← Back to Dashboard</a>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="100">
            <ProgressTemplate>
                <div class="progress-overlay">Please wait...</div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
