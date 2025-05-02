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
        .gridview { width: 100%; border-collapse: collapse; margin: 20px; }
        .gridview th, .gridview td { border: 1px solid #ddd; padding: 8px; text-align: left; }
    </style>

        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setTimeout(function () {

                }, 7000);
            });
         </script>
</head>
<body>
   
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
            <h2>Your Selections</h2>
            <asp:GridView ID="gvUserSelections" runat="server" AutoGenerateColumns="False" AllowPaging="true" 
                CssClass="gridview" OnPageIndexChanging="gvUserSelections_PageIndexChanging" 
                OnRowEditing="gvUserSelections_RowEditing" OnRowCancelingEdit="gvUserSelections_RowCancelingEdit" 
                OnRowUpdating="gvUserSelections_RowUpdating" DataKeyNames="UserId">
                <Columns>
                    <asp:BoundField DataField="UserId" HeaderText="User ID" ReadOnly="true" />
                    <asp:BoundField DataField="Username" HeaderText="Username" />
                    <asp:TemplateField HeaderText="Country">
                        <ItemTemplate>
                            <%# Eval("CountryName") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlCountryEdit" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlCountryEdit_SelectedIndexChanged">
                                <asp:ListItem Value="0">Select Country</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="State">
                        <ItemTemplate>
                            <%# Eval("StateName") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlStateEdit" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlStateEdit_SelectedIndexChanged">
                                <asp:ListItem Value="0">Select State</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="District">
                        <ItemTemplate>
                            <%# Eval("DistrictName") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlDistrictEdit" runat="server">
                                <asp:ListItem Value="0">Select District</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" />
                </Columns>
            </asp:GridView>
        </div>
        <a href="Dashboard.aspx">Navigate to Dashboard</a>
                  </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="100">
    <ProgressTemplate>
        <div style="position:fixed; top:50%; left:50%; transform:translate(-50%,-50%); background:#fff; padding:20px; border:1px solid #ccc; box-shadow:0 0 10px rgba(0,0,0,0.2); z-index:1000;">
      
            <span style="margin-left:10px;">Please wait...</span>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
    </form>
  

</body>
</html>