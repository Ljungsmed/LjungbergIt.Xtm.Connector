<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestImport.aspx.cs" Inherits="LjungbergIt.Xtm.Test.Test.TestImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Test Import</h1>
        <asp:Literal ID="litInfo" runat="server" />
    </div>
        <div>
            
            <asp:TextBox runat="server" ID="txtboxProjectId" />
        </div>
        <div>
            <asp:Button ID="btnCheckProjectId" runat="server" Text="Button" OnClick="btnCheckProjectId_click" />
        </div>
    </form>
</body>
</html>
