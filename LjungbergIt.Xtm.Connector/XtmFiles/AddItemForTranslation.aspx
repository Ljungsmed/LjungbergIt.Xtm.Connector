<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddItemForTranslation.aspx.cs" Inherits="LjungbergIt.Xtm.Connector.XtmFiles.AddItemForTranslation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/XtmFiles/XtmStyles/Xtm.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="blueline"></div>
        <div class="identity">
            <div class="heading">
                <h1>Add to Queue</h1>
            </div>
            <div class="logo">
                <img src="/XtmFiles/XtmStyles/xtmlogo.png" alt="xtm logo" height="40" />
            </div>
        </div>
        <div style="clear: both;"></div>
        <div runat="server" id="divChooseTranslationOptions" class="container">
            <div class="headingarea">
                <h2>
                    <asp:Literal runat="server" ID="litHeading" />
                </h2>
            </div>
            <div>
                <asp:Literal runat="server" ID="litSourceLanguage" />
            </div>
            <br />
            <div>
                <asp:DropDownList ID="ddSourceLanguage" runat="server" OnSelectedIndexChanged="ddSourceLanguage_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown" />
            </div>
            <br />
            <div>
                Choose the target languages (target languages will override any languages specified on an XTM Template)
            </div>
            <br />
            <div>
                <asp:CheckBoxList runat="server" ID="cbTargetLanguages"></asp:CheckBoxList>
            </div>
            <br />
            <div>
                Choose the XTM Template (Do not choose any if not needed)
            </div>
            <br />
            <div>
                <asp:DropDownList ID="ddXtmTemplate" runat="server" CssClass="dropdown" />
            </div>
            <br />
            <div>
                Include all sub-items:
                <asp:CheckBox runat="server" ID="cbAllSubItems" CssClass="checkbox"></asp:CheckBox>
            </div>
            <br />
            <div>
                <asp:Button runat="server" ID="btnAddForTranslation" Text="Add" OnClick="btnAddForTranslation_Click" CssClass="button" />
            </div>
        </div>
        <br />
        <div>
            <asp:Literal ID="litResult" runat="server" />
        </div>
    </form>
</body>
</html>
