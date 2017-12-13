<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ExcludedFields.aspx.cs" Inherits="LjungbergIt.Xtm.Connector.XtmFiles.ExcludedFields" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>XTM</title>
  <link rel="stylesheet" type="text/css" href="/XtmFiles/XtmStyles/Xtm.css" />
</head>
<body>
    <form id="form1" runat="server">
      <div class="blueline"></div>
        <div class="identity">
            <div class="heading">
                <h1>Excluded Fields</h1>
            </div>
            <div class="logo">
                <img src="/XtmFiles/XtmStyles/xtmlogo.png" alt="xtm logo" height="40" />
            </div>
        </div>
        <div style="clear: both;"></div>
        <br />
        <div class="container">
          <asp:Repeater ID="rptExlcudedFields" runat="server" ItemType="LjungbergIt.Xtm.Connector.Helpers.ExcludedFieldObject">
            <HeaderTemplate>
                <table class="tablestyle">
                        <tr class="greybc whitetext">                            
                            <th class="whitetext">
                                Template Path
                            </th>
                            <%--<th>
                                Template Name
                            </th>--%>
                            <th>
                                Field Name
                            </th>
                            <th>
                                In Template Section
                            </th>
                          <th>
                                Remove Excluded Field
                            </th>
                        </tr>
            </HeaderTemplate>
            <ItemTemplate>
              <tr>
                <td>
                  <%# Item.TemplatePath %>
                </td>
                <%--<td>
                  <%# Item.TemplateName %>
                </td>--%>
                <td>
                  <b>
                    <%# Item.FieldName %>
                  </b>
                </td>
                <td>
                  <%# Item.TemplateSection %>
                </td>
                <td>

                </td>
              </tr>
            </ItemTemplate>
            <FooterTemplate>
              </table>
            </FooterTemplate>
          </asp:Repeater>
          </div>
    </form>
</body>
</html>
