using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DistrictObjects;
using System.Collections.Generic;

public partial class SponsorMaintenance : System.Web.UI.Page
{
    System.Collections.ArrayList activeSponsors = null;
    string serverPath = null;
    DistrictSponsor currentSponsor = null;
    string SponsorImageDirectory = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        activeSponsors = (System.Collections.ArrayList)Session["activeCollection"];
        currentSponsor = (DistrictSponsor)Session["CurrentSponsor"];

        SponsorImageDirectory = System.Configuration.ConfigurationManager.AppSettings.Get("SponsorImageDirectory");

        serverPath = Server.MapPath(SponsorImageDirectory);
        if (!serverPath.EndsWith("\\"))
            serverPath += "\\";
        if (!Page.User.Identity.IsAuthenticated)
        {
            Response.Redirect("default.aspx");
        }
        else
        {
            if (!Page.IsPostBack)
            {
                activeSponsors = new System.Collections.ArrayList();

                SponsorCollection.Reset();

                foreach(DistrictSponsor aSponsor in SponsorCollection.Sponsors)
                {
                    activeSponsors.Add(aSponsor);
                }
 
                lstSponsors.DataTextField = "companyName";
                lstSponsors.DataValueField = "id";

                lstSponsors.DataSource = activeSponsors;
                lstSponsors.DataBind();
                lstSponsors.Items.Insert(0, new ListItem("<Add a Sponsor>", "-1"));
                lstSponsors.SelectedIndex = 0;
                dlState.DataSource = DistrictObject.States;
                dlState.DataBind();
                dlState.Items.FindByValue("NJ").Selected = true;
                Session["activeCollection"] = activeSponsors;
                Session["CurrentSponsor"] = null;
            }
        }
    }

    protected void lstSponsors_TextChanged(object sender, EventArgs e)
    {
        if (currentSponsor != null)
            retrieveCurrentSponsor();

        if (lstSponsors.SelectedIndex > 0)
        {
            btnSave.CausesValidation = true;
            currentSponsor = findCurrentSponsor(Int32.Parse(lstSponsors.SelectedValue));
            txtCoName.Enabled = true;

            txtContactName.Text = currentSponsor.contactName;
            txtCoName.Text = currentSponsor.companyName;

            if (currentSponsor.image != null)
            {
                imgSponsor.Visible = true;
                ckRemove.Visible = true;
                imgSponsor.ImageUrl = SponsorImageDirectory + currentSponsor.image;
                imgSponsor.AlternateText = currentSponsor.image;
            }
            else
            {
                imgSponsor.Visible = false;
                ckRemove.Visible = false;
                imgSponsor.ImageUrl = null;
            }

            txtEmail.Text = currentSponsor.emailAddress;
            txtPhone.Text = currentSponsor.businessPhone;
            txtStreet1.Text = currentSponsor.street1;
            txtStreet2.Text = currentSponsor.street2;
            txtCity.Text = currentSponsor.city;
            txtZipCode.Text = currentSponsor.zipCode;
            dlState.SelectedIndex = -1;
            dlState.Items.FindByValue(currentSponsor.state).Selected = true;
            txtWebSite.Text = currentSponsor.webSite;

            btnDelete.Enabled = true;

        }
        else
        {
            currentSponsor = null;
            txtCoName.Enabled = true;
            txtContactName.Text = "";
            txtCoName.Text = "";
            txtContactName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtStreet1.Text = "";
            txtStreet2.Text = "";
            txtCoName.Text = "";
            txtZipCode.Text = "";
            txtCity.Text = "";
            dlState.SelectedIndex = -1;
            dlState.Items.FindByText("NJ").Selected = true;
            imgSponsor.Visible = false;
            imgSponsor.ImageUrl = null;
            ckRemove.Visible = false;
            txtWebSite.Text = "";

            btnDelete.Enabled = false;

        }
        Session["CurrentSponsor"] = currentSponsor;

    }

    private DistrictSponsor findCurrentSponsor(int Id)
    {
        DistrictSponsor theSponsor = null;
        foreach(DistrictSponsor aSponsor in activeSponsors)
        {
            if (aSponsor.id.Equals(Id))
            {
                theSponsor = aSponsor;
                break;
            }
        }
        return theSponsor;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (lstSponsors.SelectedIndex >0 || currentSponsor != null)
            retrieveCurrentSponsor();

        SponsorCollection.Sponsors.Clear();
        foreach(DistrictSponsor aSponsor in activeSponsors)
        {
            SponsorCollection.Sponsors.Add(aSponsor);
        }

        if (SponsorCollection.Save())
        {
            ((masterMain)Page.Master).setInformationMessage("Sponsors were successfully saved.");
            lstSponsors.DataMember = "BusinessName";
            lstSponsors.DataSource = SponsorCollection.Sponsors;
            lstSponsors.DataBind();
            lstSponsors.Items.Insert(0, new ListItem("<Add a Sponsor>", "-1"));
            lstSponsors.SelectedIndex = 0;
            btnDelete.Enabled = false;

            ((masterMain)Page.Master).setInformationMessage("Sponsor Save was successful");
        }
        else
        {
            ((masterMain)Page.Master).setErrorMessage("Sponsors weren't saved.");
        }
    }


    protected void retrieveCurrentSponsor()
    {
        currentSponsor.companyName = txtCoName.Text;
        currentSponsor.contactName = txtContactName.Text;
        currentSponsor.emailAddress = txtEmail.Text;
        currentSponsor.businessPhone = txtPhone.Text;
        currentSponsor.street1 = txtStreet1.Text;
        currentSponsor.street2 = txtStreet2.Text;
        currentSponsor.city = txtCity.Text;
        currentSponsor.zipCode = txtZipCode.Text;
        currentSponsor.state = dlState.SelectedValue;
        currentSponsor.webSite = txtWebSite.Text;

        currentSponsor.LastTmp = DateTime.Now;
        currentSponsor.lastUser = User.Identity.Name;
        if (imgSponsorFL.FileName != null && imgSponsorFL.FileName.Trim().Length >0)
        {
            saveNewImage();
        }

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if(currentSponsor.image != null)
        {
            // remove image

            System.IO.FileInfo oldPicture = new System.IO.FileInfo(serverPath + currentSponsor.image);
            if (oldPicture.Exists)
                oldPicture.Delete();
        }

        activeSponsors.RemoveAt(lstSponsors.SelectedIndex - 1);

        lstSponsors.DataSource = activeSponsors;
        lstSponsors.DataValueField = "id";
        lstSponsors.DataTextField = "companyName";
        lstSponsors.DataBind();
        lstSponsors.Items.Insert(0, new ListItem("<Add a Sponsor>", "-1"));
        lstSponsors.SelectedIndex = 0;

        currentSponsor = null;
        txtCoName.Enabled = true;
        txtContactName.Text = "";
        txtCoName.Text = "";
        txtContactName.Text = "";
        txtEmail.Text = "";
        txtPhone.Text = "";
        txtStreet1.Text = "";
        txtStreet2.Text = "";
        txtCoName.Text = "";
        txtZipCode.Text = "";
        txtCity.Text = "";
        dlState.Items.FindByValue("NJ").Selected = true;
        imgSponsor.ImageUrl = null;
        ckRemove.Visible = false;
        txtWebSite.Text = "";

        btnDelete.Enabled = false;

        Session["CurrentSponsor"] = null;
        Session["activeCollection"] = activeSponsors;
        // remove validations when "Add is the only list item"
        if (lstSponsors.Items.Count == 1)
            btnSave.CausesValidation = false;

    }
    private string getFileName(string p)
    {
        System.IO.FileInfo temp = new System.IO.FileInfo(serverPath + p);
        if (!temp.Exists)
            return p;
        else
            return getFileName(p, 0);
    }

    private string getFileName(string p,int i)
    {
        p = p.Substring(0,p.LastIndexOf('.')-1) + " " + i.ToString() + p.Substring(p.LastIndexOf('.'));
        System.IO.FileInfo temp = new System.IO.FileInfo(serverPath + p);
        if (!temp.Exists)
            return p;
        else
            return getFileName(p, i++);
    }

    protected void txtCoName_TextChanged(object sender, EventArgs e)
    {
        if (currentSponsor == null)
        {
            currentSponsor = new DistrictSponsor();
            activeSponsors.Add(currentSponsor);
            Session["activeCollection"] = activeSponsors;
        }

        currentSponsor.companyName = txtCoName.Text;
        lstSponsors.DataSource = activeSponsors;
        lstSponsors.DataTextField = "companyName";
        lstSponsors.DataValueField = "id";
        lstSponsors.DataBind();
        lstSponsors.Items.Insert(0, new ListItem("<Add a Sponsor>", "-1"));
        lstSponsors.Items.FindByValue(currentSponsor.id.ToString()).Selected = true;

        Session["currentSponsor"] = currentSponsor;

        Page.SetFocus(txtContactName);
    }

    protected void saveNewImage()
    {
        // new image added
        // save new image
        string saveFileName = getFileName(imgSponsorFL.FileName);
        imgSponsorFL.SaveAs(serverPath + saveFileName);

        // remove old image (if any)
        if (currentSponsor.image != null)
        {
            System.IO.FileInfo oldPicture = new System.IO.FileInfo(serverPath + currentSponsor.image);
            if (oldPicture.Exists)
                oldPicture.Delete();
        }
        // save image file name to card
        currentSponsor.image = saveFileName;
        imgSponsor.ImageUrl = serverPath + currentSponsor.image;
        imgSponsor.Visible = true;
        imgSponsor.AlternateText = currentSponsor.image;
        ckRemove.Visible = true;
    }

    protected void imgSponsorFL_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        saveNewImage();
    }


    protected void ckRemove_CheckedChanged(object sender, EventArgs e)
    {
        if (currentSponsor.image != null && ckRemove.Checked)
        {
            // remove image
                System.IO.FileInfo oldPicture = new System.IO.FileInfo(serverPath + currentSponsor.image);
                if (oldPicture.Exists)
                    oldPicture.Delete();

            currentSponsor.image = null;
            imgSponsor.ImageUrl = null;
            imgSponsor.AlternateText = null;
            imgSponsor.Visible = false;
            ckRemove.Visible = false;
        }
    }
}