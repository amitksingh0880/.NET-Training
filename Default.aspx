<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CountryCodeApplication.Default" %>
 
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Phone Entry Form</title>
</head>
<body>
<form id="form1" runat="server">
<div style="padding:20px; max-width:400px;">
<asp:DropDownList ID="ddlCountries" runat="server" AppendDataBoundItems="true">
<asp:ListItem Text="--Select Country--" Value="" />
</asp:DropDownList>
<br /><br />
<asp:TextBox ID="txtPhoneNumber" runat="server" placeholder="Enter phone number" />
<br /><br />
<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
<br /><br />
<asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
<br /><br />
<asp:GridView ID="gvPhoneNumbers" runat="server" AutoGenerateColumns="false" 
    AllowPaging="true" PageSize="5" BorderStyle="Solid" BorderWidth="1" 
    BorderColor="#999" OnPageIndexChanging="gvPhoneNumbers_PageIndexChanging">
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" />
        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
        <asp:BoundField DataField="CountryName" HeaderText="Country" />
    </Columns>
</asp:GridView>

      <a href="DataLinkLibraryImplementation.aspx">Age Calculator through dll</a>
</div>
</form>
</body>
</html>