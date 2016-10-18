<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDocumentation.aspx.cs" Inherits="LjungbergIt.Xtm.Connector.XtmFiles.ViewDocumentation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Xtm</title>
    <link rel="stylesheet" type="text/css" href="/XtmFiles/XtmStyles/Xtm.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="blueline"></div>
        <div class="identity">
            <div class="heading">
                <h1>XTM Documentation</h1>
            </div>
            <div class="logo">
                <img src="/XtmFiles/XtmStyles/xtmlogo.png" alt="xtm logo" height="40" />
            </div>
        </div>
        <div style="clear: both;"></div>
        <div class="container">
        <asp:Repeater ID="rptDocumentation" runat="server" ItemType="LjungbergIt.Xtm.Connector.XtmFiles.ViewDocumentation">
            <ItemTemplate>
                <h2>
                    <%# Item.DocumentationHeading %>
                </h2>
                <div>
                    <%# Item.DocumentationText %>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        </div>
    </form>
</body>
</html>
