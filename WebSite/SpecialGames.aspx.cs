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

public partial class SpecialGames : System.Web.UI.Page
{
    SpecialGame theGame;
    protected void Page_Load(object sender, EventArgs e)
    {
        theGame = (SpecialGame)Session["specialGame"];
        if (Page.User.Identity.IsAuthenticated)
        {
            grdSpecialGames.Columns[0].Visible = true;
            btnAdd.Visible = true;
        }
        else
        {
            grdSpecialGames.Columns[0].Visible = false;
            btnAdd.Visible = false;
        }
        if (!Page.IsPostBack)
        {
            ((masterMain)Page.Master).setInformationMessage("");
            grdSpecialGames.DataSource = SpecialGame.allSpecialGames;
            grdSpecialGames.DataBind();
            mvSpecialGames.ActiveViewIndex = 0;
        }
    }
    protected void grdSpecialGames_RowEditing(object sender, GridViewEditEventArgs e)
    {
        theGame = (SpecialGame)SpecialGame.allSpecialGames[e.NewEditIndex];
        populateSpecialGame();
        mvSpecialGames.ActiveViewIndex = 1;
        e.Cancel = true;
    }

    private void populateSpecialGame()
    {
        dlLeague.DataSource = League.allLeagues;
        dlLeague.DataTextField = "name";
        dlLeague.DataValueField = "id";
        dlLeague.DataBind();
        if (theGame.hostLeague == null)
        {
            dlLeague.Items.Insert(0, "[Choose League]");
            dlLeague.Items[0].Selected = true;
        }
        else
            dlLeague.Items.FindByValue(theGame.hostLeague.id.ToString()).Selected = true;

        if (txtAgeRange.Text != null)
            txtAgeRange.Text = theGame.ageRange;
        else
            txtAgeRange.Text = "";
        if (txtName.Text != null)
            txtName.Text = theGame.tournament;
        else
            txtName.Text = "";
        if (theGame.tournamentRules != null)
        {
            txtRulesName.Text = theGame.tournamentRules.name;
            txtRulesLink.Text = theGame.tournamentRules.link;
        }
        else
        {
            txtRulesName.Text = "";
            txtRulesLink.Text = "";
        }
        if (theGame.type != null)
            rbType.Items.FindByValue(theGame.type).Selected = true;
        else
            rbType.Items[0].Selected = true;
        if (theGame.status != null)
            rbStatus.Items.FindByValue(theGame.status).Selected = true;
        else
            rbStatus.Items[0].Selected = true;
        if (theGame.webSite != null)
        {
            txtWebSiteLink.Text = theGame.webSite.link;
            txtWebSiteName.Text = theGame.webSite.name;
        }
        else
        {
            txtWebSiteName.Text = "";
            txtWebSiteLink.Text = "";
        }
        if (theGame.tournamentRules != null)
        {
            txtRegFormLink.Text = theGame.registrationForm.link;
            txtRegFormName.Text = theGame.registrationForm.name;
        }
        else
        {
            txtRegFormName.Text = "";
            txtRegFormLink.Text = "";
        }
        Session["specialGame"] = theGame;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvSpecialGames.ActiveViewIndex = 0;
        ((masterMain)Page.Master).setInformationMessage("Cancelled");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        GetData();
        theGame.save();
        grdSpecialGames.DataSource = SpecialGame.allSpecialGames;
        grdSpecialGames.DataBind();
        mvSpecialGames.ActiveViewIndex = 0;
        ((masterMain)Page.Master).setInformationMessage("Special Game Saved");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        theGame = new SpecialGame();
        populateSpecialGame();
        mvSpecialGames.ActiveViewIndex = 1;
    }
    protected void btnUploadeCancel_Click(object sender, EventArgs e)
    {
        mvSpecialGames.ActiveViewIndex = 1;
    }
    protected void btnRulesLink_Click(object sender, EventArgs e)
    {
        if (((Button)sender).ID.Equals(btnRegFormLink.ID))
        {
            lblUploadName.Text = "Registration Form";

        }
        else
        {
            lblUploadName.Text = "Registration Rules";
        }
        mvSpecialGames.ActiveViewIndex = 2;
    }
    protected void GetData()
    {
        if (dlLeague.SelectedIndex > 0)
            theGame.hostLeague = League.find(Convert.ToInt32(dlLeague.SelectedValue));

        theGame.ageRange = txtAgeRange.Text;
        theGame.tournament = txtName.Text;
        if (txtRulesLink.Text != null)
        {
            if (theGame.tournamentRules == null)
                theGame.tournamentRules = new DistrictLink();

            theGame.tournamentRules.name = txtRulesName.Text;
            theGame.tournamentRules.link = txtRulesLink.Text;
        }
        theGame.type = rbType.SelectedValue;
        theGame.status = rbStatus.SelectedValue;

        if (txtWebSiteName.Text != null)
        {
            if (theGame.webSite == null)
                theGame.webSite = new DistrictLink();
            theGame.webSite.link = txtWebSiteLink.Text;
            theGame.webSite.name = txtWebSiteName.Text;
        }
        if (txtRegFormName.Text != null)
        {
            if (theGame.registrationForm == null)
                theGame.registrationForm = new DistrictLink();
            theGame.registrationForm.link = txtRegFormLink.Text;
            theGame.registrationForm.name = txtRegFormName.Text;
        }
    }
    protected void btnContinue_Click(object sender, EventArgs e)
    {
        
    }
}
