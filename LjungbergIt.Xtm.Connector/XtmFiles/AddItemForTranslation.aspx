<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddItemForTranslation.aspx.cs" Inherits="LjungbergIt.Xtm.Connector.XtmFiles.AddItemForTranslation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/XtmFiles/XtmStyles/Xtm.css" />
</head>
<body>
  <script type="text/javascript">
    
  </script>
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
        <div class="container errordiv">
            <asp:Label ID="labelErrorMessage" runat="server" />
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
            <div class="divspace">
                <asp:DropDownList ID="ddSourceLanguage" runat="server" OnSelectedIndexChanged="ddSourceLanguage_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown" />
            </div>
            <div class="divspace">
                Project Name (Leave blank if you want to add to existing project):
            </div>
            <div class="divspace">
                <asp:TextBox runat="server" ID="txtProjectName" CssClass="textbox" />
            </div>
            <div runat="server" id="divExistingProjects">
                <div class="divspace">
                    Choose existing project to add translation to:
                </div>
                <div>
                    <asp:RadioButtonList ID="rblExistingProjects" runat="server" />
                </div>
            </div>
            <div class="divspace">
                Choose the target languages (target languages will override any languages specified on an XTM Template)
            </div>
            <div>
                <asp:CheckBoxList runat="server" ID="cbTargetLanguages"></asp:CheckBoxList>
            </div>
            <div class="divspace">
                Choose the XTM Template (Do not choose any if not needed)
            </div>
            <div class="divspace">
                <asp:DropDownList ID="ddXtmTemplate" runat="server" CssClass="dropdown" />
            </div>

            <div class="divspace">
                Include related content:
                <asp:CheckBoxList CssClass="addsubitemstd" runat="server" ID="cblIncludeRelatedContentItems"></asp:CheckBoxList>
            </div>

            <div class="divspace">
                Include all sub-items:
                <!--OnClick="showElement(this, 'includerelatedcontentfromsubitems')"-->
                <asp:CheckBox  runat="server" ID="cbAllSubItems" CssClass="checkbox jsIncludeAllSubItems" OnCheckedChanged="cbAllSubItems_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
            </div>

            <div class="divspace">
                <asp:CheckBoxList CssClass="addsubitemstd" ID="cblIncludeAllSubitems" runat="server"></asp:CheckBoxList>
            </div>

            <div class="divspace">
                <span style="visibility: hidden" class="includerelatedcontentfromsubitems" id="spanIncludeRelatedContentItemsFromSubItems">Also include related content of sub-items:
                    <%--<asp:CheckBox runat="server" ID="cbIncludeRelatedContentItemsFromSubItems" CssClass="checkbox cbIncludeRelatedContentItemsFromSubItems" />--%>
                  <input type="checkbox" id="inputchbIncludeAllSubItemsRelatedItems" onchange="showElementById(this, 'divIncludeAllSubitemsRelatedItems'); ToggleAllSubitemsRelatedItems(this)" />
                </span>
            </div>

            <div class="divspace includeAllSubitemsRelatedItems" id="divIncludeAllSubitemsRelatedItems" style="display: none" >
                <asp:CheckBoxList CssClass="addsubitemstd" ID="cblIncludeAllSubitemsRelatedItems" runat="server"></asp:CheckBoxList>                
            </div>

            <div class="divspace">
                <asp:Button runat="server" ID="btnAddForTranslation" Text="Add" OnClick="btnAddForTranslation_Click" CssClass="button" Width="40px" />
            </div>
        </div>
        <div class="container divspacebottom divspace">
            <asp:Label ID="labelResult" runat="server" />
        </div>
    </form>
</body>
    <script src="/XtmFiles/XtmJs/XtmJs.js"></script>
    <script> 
      showElementGlobal('cbAllSubItems', 'spanIncludeRelatedContentItemsFromSubItems');      
    </script>
</html>
