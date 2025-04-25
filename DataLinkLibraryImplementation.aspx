<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataLinkLibraryImplementation.aspx.cs" Inherits="CountryCodeApplication.DataLinkLibraryImplementation" %>

<!DOCTYPE html>
 
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
</head>
<body>
<form id="form2" runat="server">
<div>
    
<asp:TextBox ID="UserName" runat="server" placeholder="Enter Your Name" />
    <br />
<asp:TextBox ID="BirthDate" runat="server" placeholder="Enter Your DOB" /> <br /> <br />
<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
<br />
<br />
    <asp:Label ID="ldlName" runat="server" ForeColor="Red" />
    <br />
<asp:Label ID="lblAge" runat="server" ForeColor="Green" />
<br />
    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
<br />
<a href="Default.aspx">Country</a>
</div>
</form>
</body>
</html>
