using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;


namespace LjungbergIt.Xtm.Test.XtmAdmin
{
  public partial class Admin : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnClearInTranslation_click(object sender, EventArgs e)
    {
      Database master = Database.GetDatabase("master");
      string strSitecoreId = txtSitecoreId.Text;
      if (!strSitecoreId.Equals(""))
      {
        ID parentId = null;
        bool parentIdOk;
        try
        {
          parentId = new ID(strSitecoreId);
          parentIdOk = true;
        }
        catch (Exception)
        {
          parentIdOk = false;
        }

        if (parentIdOk)
        {

          Item parentItem = master.GetItem(parentId);
          using (new SecurityDisabler())
          {
            parentItem.Editing.BeginEdit();
            parentItem["XTM_In Translation"] = string.Empty;
            parentItem.Editing.EndEdit();
          }
          foreach (Item child in parentItem.GetChildren())
          {
            using (new SecurityDisabler())
            {
              child.Editing.BeginEdit();
              child["XTM_In Translation"] = string.Empty;
              child.Editing.EndEdit();
            }
          }
          litClearInTranslation.Text = "Succesfully cleared all checkboxes";
        }
        else
          litClearInTranslation.Text = "ID is not in correct format";
      }
      else
        litClearInTranslation.Text = "no ID in field";

    }
  }
}