using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DistrictObjects;

public partial class UmpireDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Umpire anUmp = Umpire.find(Convert.ToInt32(Request.Params["id"]));
        if (anUmp.image != null)
            imgUmpire.ImageUrl = System.Configuration.ConfigurationManager.AppSettings.Get("umpirePhotoDir") + "\\" + anUmp.image;
        imgUmpire.Height = Unit.Pixel(250);
        imgUmpire.Width = Unit.Pixel(250);
        lblFirstName.Text = anUmp.firstName;
        lblLastName.Text = anUmp.lastName;
        lblHomeLeague.Text = anUmp.homeLeague;
        lblYears.Text =  anUmp.umpireSince;
        lblHistory.Text = anUmp.credits;
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close()", true);
    }
}
