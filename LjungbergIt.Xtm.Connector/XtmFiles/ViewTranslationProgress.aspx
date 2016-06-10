<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTranslationProgress.aspx.cs" Inherits="LjungbergIt.Xtm.Connector.XtmFiles.ViewTranslationProgress" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Xtm</title>
    <link rel="stylesheet" type="text/css" href="/XtmFiles/XtmStyles/Xtm.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="headingarea">
            <h1>View Translation Progress
            </h1>
        </div>

        <asp:Literal ID="litTest" runat="server" />

        <div>
            <asp:ListView ID="lwProgress" runat="server" ItemType="LjungbergIt.Xtm.Webservice.XtmProject">
                <LayoutTemplate>
                    <table class="tablestyle">
                        <tr class="greybc whitetext">
                            
                            <th class="whitetext">
                                Project Name
                            </th>
                            <th>
                                Project ID
                            </th>
                            <th>
                                Customer
                            </th>
                            <th>
                                Source Language
                            </th>
                            <th>
                                Target Language
                            </th>
                            <th>
                                Created
                            </th>
                            <th>
                                Due Date
                            </th>
                            <th>
                                Status
                            </th>
                            <th>
                                Start import
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                
                    <tr>
                        
                        <td>
                            <%# Item.ProjectName %>
                        </td>
                        <td>
                            <%# Item.ProjectId %>
                        </td>
                        <td>
                            <%# Item.Customer %>
                        </td>
                        <td>
                            <%# Item.SourceLanguage %>
                        </td>
                        <td>
                            <%# Item.TargetLanguage %>
                        </td>
                        <td>
                            <%# Item.CreatedDate.ToString("dd-MM-yyyy HH:mm") %>
                        </td>
                        <td>
                            <%# Item.DueDate.ToString("dd-MM-yyyy HH:mm") %>
                        </td>
                        <td>
                            <%# Item.WorkflowStatus %>
                        </td>
                        <td>                                                        
                            <asp:Button runat="server" Visible='<%# RenderImportButton(Item.WorkflowStatus) %>' Text="Import now" CommandArgument='<%# Item.ProjectId %>' />                             
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <div>
            <asp:Literal ID="litInfo" runat="server" />
        </div>
    </form>
</body>
</html>
