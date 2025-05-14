<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BulkDataInsert.aspx.cs" Inherits="CountryCodeApplication.BulkDataInsert" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:FileUpload ID="FileUpload1" runat="server" />
<asp:Button ID="btnUpload" runat="server" Text="Upload CSV" OnClick="btnUpload_Click" />
<asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>

        </div>
    </form>
    <a class="link-button" href="Dashboard.aspx">→ Navigate to Dashboard...</a>
</body>
</html>
