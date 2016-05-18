<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="LjungbergIt.Xtm.Connector.Test.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="test get project" OnClick="Button1_Click" />
        
    </div>
    <div>
        <asp:Button ID="Button2" runat="server" Text="test import XML" OnClick="Button2_Click" />
        <asp:Literal ID="Literal1" runat="server" />
    </div>
        <div>
        <asp:Button ID="Button3" runat="server" Text="test scheduled task code" OnClick="Button3_Click" />
        <asp:Literal ID="litXtmResponse" runat="server" />
    </div>
    <div>
        <asp:Literal ID="litInfo" runat="server" />
    </div>
    </form>
</body>
</html>
