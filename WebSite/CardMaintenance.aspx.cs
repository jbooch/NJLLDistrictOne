using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DistrictObjects;

public partial class CardMaintenance : System.Web.UI.Page
{
    string serverPath = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        serverPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings.Get("umpirePhotoDir"));
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
                lstCards.DataMember = "cardTitle";
                lstCards.DataSource = DistrictCard.allCards.Cards;
                RangeValidator1.MinimumValue = (DistrictCard.allCards.Cards.Count + 1).ToString();
                lstCards.DataBind();
                lstCards.Items.Insert(0, new ListItem("<Add a Card>", "Add"));
                lstCards.SelectedIndex = 0;
                btnSave.Text = "Add";
            }
        }
    }

    protected void lstCards_TextChanged(object sender, EventArgs e)
    {
        if (lstCards.SelectedIndex > 0)
        {
            DistrictCard aCard = DistrictCard.find(lstCards.SelectedValue);
            txtBody.Text = aCard.cardBody.Replace("<br />","\n");
            txtTitle.Text = aCard.cardTitle;
            if (aCard.image != null)
            {
                cdImg.ImageUrl = System.Configuration.ConfigurationManager.AppSettings.Get("umpirePhotoDir") + aCard.image;
                cdImg.Visible = true;
                ckRemoveImage.Visible = true;
            }
            else
            {
                ckRemoveImage.Visible = false;
                cdImg.Visible = false;
            }
            txtCardNumber.Text = aCard.cardNumber.ToString();
            rbPriority.SelectedValue = aCard.priority;
            RangeValidator1.MinimumValue = aCard.cardNumber.ToString();
            RangeValidator1.MaximumValue = aCard.cardNumber.ToString();
            txtCardNumber.Enabled = false;

            btnSave.Text = "Change";
            btnDelete.Enabled = true;
        }
        else
        {
            txtBody.Text = "";
            cdImg.Visible = false;
            ckRemoveImage.Visible = false;
            txtTitle.Text = "";
            txtCardNumber.Text = "";
            rbPriority.SelectedValue = "";
            RangeValidator1.MinimumValue = (DistrictCard.allCards.Cards.Count + 1).ToString();
            RangeValidator1.MaximumValue = "99";
            txtCardNumber.Enabled = true;

            btnSave.Text = "Add";
            btnDelete.Enabled = false;
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DistrictCard aCard = null;

        if (lstCards.SelectedIndex == 0)
        {
            aCard = new DistrictCard();
        }
        else
        {
            aCard = DistrictCard.find(lstCards.SelectedValue);
        }

        aCard.cardTitle = txtTitle.Text;
        aCard.cardBody = txtBody.Text.Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("\r", "<br />");
        aCard.cardNumber = Convert.ToInt32(txtCardNumber.Text);
        if (flImg.PostedFile != null && flImg.PostedFile.FileName.Trim().Length > 0)
        {
            // new image added
            // save new image
            string saveFileName = getFileName(flImg.PostedFile.FileName);
            flImg.PostedFile.SaveAs(serverPath + saveFileName);

            // remove old image (if any)
            if (aCard.image != null)
            {
                System.IO.FileInfo oldPicture = new System.IO.FileInfo(serverPath + aCard.image);
                if (oldPicture.Exists)
                    oldPicture.Delete();
            }
            // save image file name to card
            aCard.image = saveFileName;
        } else
        {
            // Just remove image
            if (ckRemoveImage.Checked)
            {
                System.IO.FileInfo oldPicture = new System.IO.FileInfo(serverPath + aCard.image);
                if (oldPicture.Exists)
                    oldPicture.Delete();
                aCard.image = null;
            }
        }
        aCard.priority = rbPriority.SelectedValue;
        aCard.LastTmp = DateTime.Now;
        aCard.lastUser = User.Identity.Name;

        if (aCard.Save())
        {
            ((masterMain)Page.Master).setInformationMessage("Cards were successfully saved.");
            lstCards.DataMember = "cardTitle";
            lstCards.DataSource = DistrictCard.allCards.Cards;
            lstCards.DataBind();
            lstCards.Items.Insert(0, new ListItem("<Add a Card>", "Add"));
            lstCards.SelectedIndex = 0;
            btnSave.Text = "Add";
            btnDelete.Enabled = false;
        }
        else
        {
            ((masterMain)Page.Master).setErrorMessage("Cards weren't saved.");
        }

        lstCards.DataMember = "cardTitle";
        lstCards.DataSource = DistrictCard.allCards.Cards;
        lstCards.DataBind();
        lstCards.Items.Insert(0, new ListItem("<Add a Card>", "Add"));
        lstCards.SelectedIndex = 0;
        btnSave.Text = "Add";
        btnDelete.Enabled = false;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DistrictCard aCard = null;
        if (lstCards.SelectedIndex > 0)
        {
            aCard = DistrictCard.find(lstCards.SelectedValue);
            DistrictCard.Remove(aCard);
            lstCards.DataMember = "cardTitle";
            lstCards.DataSource = DistrictCard.allCards.Cards;
            lstCards.DataBind();
            lstCards.Items.Insert(0, new ListItem("<Add a Card>", "Add"));
            lstCards.SelectedIndex = 0;
            btnSave.Text = "Add";
            btnDelete.Enabled = false;
        }
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
}