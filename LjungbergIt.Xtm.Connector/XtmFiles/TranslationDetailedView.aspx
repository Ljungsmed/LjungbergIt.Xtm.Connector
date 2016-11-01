<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TranslationDetailedView.aspx.cs" Inherits="LjungbergIt.Xtm.Connector.XtmFiles.TranslationDetailedView" %>

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
                <h1>Content ready for translation</h1>
            </div>
            <div class="logo">
                <img src="/XtmFiles/XtmStyles/xtmlogo.png" alt="xtm logo" height="40" />
            </div>
        </div>
        <div style="clear: both;"></div>
    <div class="container">
        <asp:Repeater runat="server" ID="rptTranslationQueue" ItemType="LjungbergIt.Xtm.Connector.Helpers.ScQueueItem">
            <HeaderTemplate>
                <table class="tablestyle">
                        <tr class="greybc whitetext">                            
                            <th class="whitetext">
                                Name
                            </th>
                            <th>
                                Path
                            </th>
                            <th>
                                Source Language
                            </th>
                            <th>
                                Target Language
                            </th>
                            <th>
                                Xtm Template
                            </th>
                        </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Item.QueueItemName %>
                    </td>
                    <td>
                        <%# Item.QueueItemPath %>
                    </td>
                    <td>
                        <%# Item.SourceLanguage %>
                    </td>
                    <td>
                        <%# Item.TargetLanguage %>
                    </td>
                    <td>
                        <%# Item.XtmTemplate %>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>

            </FooterTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
