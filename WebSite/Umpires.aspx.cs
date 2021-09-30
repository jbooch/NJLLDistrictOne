using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DistrictObjects;

public partial class Umpires : System.Web.UI.Page
{
    Umpire theUmp = null;
    string umpireDirectory = System.Configuration.ConfigurationManager.AppSettings.Get("umpirePhotoDir");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["selectedUmp"] != null)
            theUmp = (Umpire)Session["selectedUmp"];

        if (Page.User.Identity.IsAuthenticated)
        {
            btnAdd.Visible = true;
            grdUmps.Columns[0].Visible = true;
        }
        else
        {
            btnAdd.Visible = false;
            grdUmps.Columns[0].Visible = false;
        }
        if (!Page.IsPostBack)
        {
            mvUmps.ActiveViewIndex = 0;
            grdUmps.DataSource = Umpire.allUmps;
            grdUmps.DataBind();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        theUmp = new Umpire();
        populateUmpire();
        mvUmps.ActiveViewIndex = 1;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        // first determine if there is an image then save the image. If Image save is successful, then save the updates.

        if (flDefaultImage.PostedFile.FileName.Trim().Length > 0)
        {
            // must have added a new image.  Have to save it
            string serverPath = Server.MapPath(umpireDirectory);
            if (!serverPath.EndsWith("\\"))
                serverPath += "\\";
            flDefaultImage.PostedFile.SaveAs(serverPath + getFileName(flDefaultImage.PostedFile.FileName));

            if (theUmp.image != null)
            {
                // need to remove old image.
                System.IO.FileInfo oldPicture = new System.IO.FileInfo(Server.MapPath(umpireDirectory + theUmp.image));
                if (oldPicture.Exists)
                    oldPicture.Delete();
            }
            theUmp.image = flDefaultImage.PostedFile.FileName;
        }
        else if (ckRemove.Checked)
        {
            // need to remove old image and replace with nothing
            System.IO.FileInfo oldPicture = new System.IO.FileInfo(Server.MapPath(umpireDirectory + theUmp.image));
            if (oldPicture.Exists)
                oldPicture.Delete();
            theUmp.image = null;
        }

        theUmp.firstName = txtFirstName.Text;
        theUmp.lastName = txtLastName.Text;
        theUmp.homeLeague = txtHomeLeague.Text;
        theUmp.credits = txtCredits.Text;
        theUmp.umpireSince = txtUmpireSince.Text;

        if (theUmp.save())
        {
            grdUmps.DataSource = Umpire.allUmps;
            grdUmps.DataBind();
            mvUmps.ActiveViewIndex = 0;
        }
        else
        {
            ((masterMain)Page.Master).setErrorMessage("There was a problem Saving the Umpire");
        }
    }

    private string getFileName(string p)
    {
        return p.Substring(p.LastIndexOf("\\")+1);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvUmps.ActiveViewIndex = 0;
    }

    protected void grdUmps_RowEditing(object sender, GridViewEditEventArgs e)
    {
        theUmp = (Umpire)Umpire.allUmps[e.NewEditIndex];
        populateUmpire();
        mvUmps.ActiveViewIndex = 1;
        e.Cancel = true;
    }
    private void populateUmpire()
    {
        Session["selectedUmp"] = theUmp;
        txtCredits.Text = theUmp.credits;
        txtFirstName.Text = theUmp.firstName;
        txtLastName.Text = theUmp.lastName;
        txtUmpireSince.Text = theUmp.umpireSince;
        txtHomeLeague.Text = theUmp.homeLeague;
        if (theUmp.image != null)
        {
            imgUmpire.ImageUrl = System.Configuration.ConfigurationManager.AppSettings.Get("umpirePhotoDir") + theUmp.image;
            imgUmpire.Visible = true;
            ckRemove.Visible = true;
            imgUmpire.Height = Unit.Pixel(250);
            imgUmpire.Width = Unit.Pixel(250);
        }
        else
        {
            ckRemove.Visible = false;
            imgUmpire.Visible = false;
        }
    }
}
