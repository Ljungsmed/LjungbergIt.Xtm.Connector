<%@ page language="C#" autoeventwireup="true" codebehind="Admin.aspx.cs" inherits="LjungbergIt.Xtm.Test.XtmAdmin.Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>
            Uncheck the "XTM_In Translation" checkbox from an item and all children 
        </h2>
        <div>
            <span>Insert ID of parent item</span>
            <asp:textbox runat="server" id="txtSitecoreId"></asp:textbox>
            <asp:button id="btnClearInTranslation" runat="server" text="Clear checked values" onclick="btnClearInTranslation_click" />
        </div>
        <div>
            <asp:literal runat="server" id="litClearInTranslation"></asp:literal>
        </div>
    </form>
</body>
</html>
