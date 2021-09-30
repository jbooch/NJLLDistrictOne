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

public partial class PastChampions : System.Web.UI.Page
{
    PastChampionSeason selectedSeason;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["selectedSeason"] != null)
            selectedSeason = (PastChampionSeason)Session["selectedSeason"];

        if (Page.User.Identity.IsAuthenticated)
        {
            btnMaintenance.Visible = true;
            btnPastChamps.Visible = true;
            txtYearValidator.MinimumValue = "1950";
            txtYearValidator.MaximumValue = DateTime.Now.Year.ToString();
        }
        else
        {
            btnMaintenance.Visible = false;
            btnPastChamps.Visible = false;
        }
        if (!Page.IsPostBack)
        {
            mvPastChamps.ActiveViewIndex = 0;
            rpChampions.DataSource = PastChampionSeason.allChampions;
            rpChampions.DataBind();
        }
    }
    protected void rpChampions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem anItem = e.Item;

        for (int x = 0; x < 10; x++)
        {
            string cellIdString = "td" + x.ToString();
            string champIdString = "champId" + x.ToString();
            HtmlTableCell tc = (HtmlTableCell)anItem.FindControl(cellIdString);
            if (tc != null)
            {
                Label aLabel = (Label)tc.FindControl(champIdString);
                if (aLabel != null)
                {
                    int tId = Convert.ToInt32(aLabel.Text);
                    PastChampion aChamp = PastChampion.find(tId);
                    if (aChamp != null)
                    {
                        tc.Style.Add("color",aChamp.fontColor);
                        tc.Style.Add("background-color", aChamp.cellColor);
                    }
                }
            }
        }
    }
    protected void btnMaintenance_Click(object sender, EventArgs e)
    {
        dlChampSeason.DataSource = PastChampionSeason.allChampions;
        dlChampSeason.DataTextField = "Year";
        dlChampSeason.DataBind();
        dlChampSeason.Items.Insert(0,new ListItem("[New]","New"));
        dlChampSeason.SelectedIndex = 0;
        dlChampSeason_SelectedIndexChanged(sender, e);
        mvPastChamps.ActiveViewIndex = 1;
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["selectedSeason"] = selectedSeason;
        loadSelectedSeason();
    }
    protected void dlChampSeason_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dlChampSeason.SelectedValue.Equals("New"))
        {
            selectedSeason = null;
        }
        else
        {
            selectedSeason =  PastChampionSeason.find(Convert.ToInt32(dlChampSeason.SelectedValue));
            Session["selectedSeason"] = selectedSeason;
        }
        loadSelectedSeason();
        Session["selectedSeason"] = selectedSeason;
    }

    private void loadSelectedSeason()
    {
        if (selectedSeason != null)
        {
            txtYear.Text = selectedSeason.year.ToString("####");
            if (selectedSeason.year>0)
                txtYear.Enabled = false;
            txtbbMinorsTeam.Text = selectedSeason.bbMinors.team;
            txtbbMinorsOtherText.Text = selectedSeason.bbMinors.otherText;
            ckbbMinorsWinnings.Items.FindByText("Sections").Selected = selectedSeason.bbMinors.sections;
            ckbbMinorsWinnings.Items.FindByText("Regions").Selected = selectedSeason.bbMinors.regions;
            ckbbMinorsWinnings.Items.FindByText("States").Selected = selectedSeason.bbMinors.states;
            ckbbMinorsWinnings.Items.FindByText("National").Selected = selectedSeason.bbMinors.nationals;

            txtbbMinors2Team.Text = selectedSeason.bbMinors2.team;
            txtbbMinors2OtherText.Text = selectedSeason.bbMinors2.otherText;
            ckbbMinors2Winnings.Items.FindByText("Sections").Selected = selectedSeason.bbMinors2.sections;
            ckbbMinors2Winnings.Items.FindByText("Regions").Selected = selectedSeason.bbMinors2.regions;
            ckbbMinors2Winnings.Items.FindByText("States").Selected = selectedSeason.bbMinors2.states;
            ckbbMinors2Winnings.Items.FindByText("National").Selected = selectedSeason.bbMinors2.nationals;

            txtbbMajorsTeam.Text = selectedSeason.bbMajors.team;
            txtbbMajorsOtherText.Text = selectedSeason.bbMajors.otherText;
            ckbbMajorsWinnings.Items.FindByText("Sections").Selected = selectedSeason.bbMajors.sections;
            ckbbMajorsWinnings.Items.FindByText("Regions").Selected = selectedSeason.bbMajors.regions;
            ckbbMajorsWinnings.Items.FindByText("States").Selected = selectedSeason.bbMajors.states;
            ckbbMajorsWinnings.Items.FindByText("National").Selected = selectedSeason.bbMajors.nationals;

            txtbbJuniorsTeam.Text = selectedSeason.bbJuniors.team;
            txtbbJuniorsOtherText.Text = selectedSeason.bbJuniors.otherText;
            ckbbJuniorsWinnings.Items.FindByText("Sections").Selected = selectedSeason.bbJuniors.sections;
            ckbbJuniorsWinnings.Items.FindByText("Regions").Selected = selectedSeason.bbJuniors.regions;
            ckbbJuniorsWinnings.Items.FindByText("States").Selected = selectedSeason.bbJuniors.states;
            ckbbJuniorsWinnings.Items.FindByText("National").Selected = selectedSeason.bbJuniors.nationals;

            txtbbIntermediateTeam.Text = selectedSeason.bbIntermediate.team;
            txtbbIntermediateOtherText.Text = selectedSeason.bbIntermediate.otherText;
            ckbbIntermediateWinnings.Items.FindByText("Sections").Selected = selectedSeason.bbIntermediate.sections;
            ckbbIntermediateWinnings.Items.FindByText("Regions").Selected = selectedSeason.bbIntermediate.regions;
            ckbbIntermediateWinnings.Items.FindByText("States").Selected = selectedSeason.bbIntermediate.states;
            ckbbIntermediateWinnings.Items.FindByText("National").Selected = selectedSeason.bbIntermediate.nationals;


            txtbbSeniorsTeam.Text = selectedSeason.bbSeniors.team;
            txtbbSeniorsOtherText.Text = selectedSeason.bbSeniors.otherText;
            ckbbSeniorsWinnings.Items.FindByText("Sections").Selected = selectedSeason.bbSeniors.sections;
            ckbbSeniorsWinnings.Items.FindByText("Regions").Selected = selectedSeason.bbSeniors.regions;
            ckbbSeniorsWinnings.Items.FindByText("States").Selected = selectedSeason.bbSeniors.states;
            ckbbSeniorsWinnings.Items.FindByText("National").Selected = selectedSeason.bbSeniors.nationals;

            txtsbMinorsTeam.Text = selectedSeason.sbMinors.team;
            txtsbMinorsOtherText.Text = selectedSeason.sbMinors.otherText;
            cksbMinorsWinnings.Items.FindByText("Sections").Selected = selectedSeason.sbMinors.sections;
            cksbMinorsWinnings.Items.FindByText("Regions").Selected = selectedSeason.sbMinors.regions;
            cksbMinorsWinnings.Items.FindByText("States").Selected = selectedSeason.sbMinors.states;
            cksbMinorsWinnings.Items.FindByText("National").Selected = selectedSeason.sbMinors.nationals;

            txtsbMinors2Team.Text = selectedSeason.sbMinors2.team;
            txtsbMinors2OtherText.Text = selectedSeason.sbMinors2.otherText;
            cksbMinors2Winnings.Items.FindByText("Sections").Selected = selectedSeason.sbMinors2.sections;
            cksbMinors2Winnings.Items.FindByText("Regions").Selected = selectedSeason.sbMinors2.regions;
            cksbMinors2Winnings.Items.FindByText("States").Selected = selectedSeason.sbMinors2.states;
            cksbMinors2Winnings.Items.FindByText("National").Selected = selectedSeason.sbMinors2.nationals;

            txtsbMajorsTeam.Text = selectedSeason.sbMajors.team;
            txtsbMajorsOtherText.Text = selectedSeason.sbMajors.otherText;
            cksbMajorsWinnings.Items.FindByText("Sections").Selected = selectedSeason.sbMajors.sections;
            cksbMajorsWinnings.Items.FindByText("Regions").Selected = selectedSeason.sbMajors.regions;
            cksbMajorsWinnings.Items.FindByText("States").Selected = selectedSeason.sbMajors.states;
            cksbMajorsWinnings.Items.FindByText("National").Selected = selectedSeason.sbMajors.nationals;

            txtsbJuniorsTeam.Text = selectedSeason.sbJuniors.team;
            txtsbJuniorsOtherText.Text = selectedSeason.sbJuniors.otherText;
            cksbJuniorsWinnings.Items.FindByText("Sections").Selected = selectedSeason.sbJuniors.sections;
            cksbJuniorsWinnings.Items.FindByText("Regions").Selected = selectedSeason.sbJuniors.regions;
            cksbJuniorsWinnings.Items.FindByText("States").Selected = selectedSeason.sbJuniors.states;
            cksbJuniorsWinnings.Items.FindByText("National").Selected = selectedSeason.sbJuniors.nationals;

            txtsbSeniorsTeam.Text = selectedSeason.sbSeniors.team;
            txtsbSeniorsOtherText.Text = selectedSeason.sbSeniors.otherText;
            cksbSeniorsWinnings.Items.FindByText("Sections").Selected = selectedSeason.sbSeniors.sections;
            cksbSeniorsWinnings.Items.FindByText("Regions").Selected = selectedSeason.sbSeniors.regions;
            cksbSeniorsWinnings.Items.FindByText("States").Selected = selectedSeason.sbSeniors.states;
            cksbSeniorsWinnings.Items.FindByText("National").Selected = selectedSeason.sbSeniors.nationals;
        }
        else
        {
            txtYear.Text = "";
            txtYear.Enabled = true;

            txtbbMinorsTeam.Text = "";
            txtbbMinorsOtherText.Text = "";
            foreach (ListItem anItem in ckbbMinorsWinnings.Items)
            {
                anItem.Selected = false;
            }

            txtbbMinors2Team.Text = "";
            txtbbMinors2OtherText.Text = "";
            foreach (ListItem anItem in ckbbMinors2Winnings.Items)
            {
                anItem.Selected = false;
            }

            txtbbMajorsTeam.Text = "";
            txtbbMajorsOtherText.Text = "";
            foreach (ListItem anItem in ckbbMajorsWinnings.Items)
            {
                anItem.Selected = false;
            }

            txtbbIntermediateTeam.Text = "";
            txtbbIntermediateOtherText.Text = "";
            foreach (ListItem anItem in ckbbIntermediateWinnings.Items)
            {
                anItem.Selected = false;
            }

            txtbbJuniorsTeam.Text = "";
            txtbbJuniorsOtherText.Text = "";
            foreach (ListItem anItem in ckbbJuniorsWinnings.Items)
            {
                anItem.Selected = false;
            }

            txtbbSeniorsTeam.Text = "";
            txtbbSeniorsOtherText.Text = "";
            foreach (ListItem anItem in ckbbSeniorsWinnings.Items)
            {
                anItem.Selected = false;
            }

            txtsbMinorsTeam.Text = "";
            txtsbMinorsOtherText.Text = "";
            foreach (ListItem anItem in cksbMinorsWinnings.Items)
            {
                anItem.Selected = false;
            }

            txtsbMinors2Team.Text = "";
            txtsbMinors2OtherText.Text = "";
            foreach (ListItem anItem in cksbMinors2Winnings.Items)
            {
                anItem.Selected = false;
            }

            txtsbMajorsTeam.Text = "";
            txtsbMajorsOtherText.Text = "";
            foreach (ListItem anItem in cksbMajorsWinnings.Items)
            {
                anItem.Selected = false;
            }

            txtsbJuniorsTeam.Text = "";
            txtsbJuniorsOtherText.Text = "";
            foreach (ListItem anItem in cksbJuniorsWinnings.Items)
            {
                anItem.Selected = false;
            }

            txtsbSeniorsTeam.Text = "";
            txtsbSeniorsOtherText.Text = "";
            foreach (ListItem anItem in cksbSeniorsWinnings.Items)
            {
                anItem.Selected = false;
            }
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        getChampSeason();
        if (selectedSeason.save())
        {
            rpChampions.DataSource = PastChampionSeason.allChampions;
            rpChampions.DataBind();
            mvPastChamps.ActiveViewIndex = 0;
        }
        else
        {
        }
    }

    private void getChampSeason()
    {
        if (selectedSeason == null)
        {
            selectedSeason = new PastChampionSeason();
            selectedSeason.year = Convert.ToInt32(txtYear.Text);
        }
        selectedSeason.bbMinors.team = txtbbMinorsTeam.Text;
        selectedSeason.bbMinors.otherText = txtbbMinorsOtherText.Text;
        selectedSeason.bbMinors.sections = ckbbMinorsWinnings.Items.FindByText("Sections").Selected;
        selectedSeason.bbMinors.regions = ckbbMinorsWinnings.Items.FindByText("Regions").Selected;
        selectedSeason.bbMinors.states = ckbbMinorsWinnings.Items.FindByText("States").Selected;
        selectedSeason.bbMinors.nationals = ckbbMinorsWinnings.Items.FindByText("National").Selected;

        selectedSeason.bbMinors2.team = txtbbMinors2Team.Text;
        selectedSeason.bbMinors2.otherText = txtbbMinors2OtherText.Text;
        selectedSeason.bbMinors2.sections = ckbbMinors2Winnings.Items.FindByText("Sections").Selected;
        selectedSeason.bbMinors2.regions = ckbbMinors2Winnings.Items.FindByText("Regions").Selected;
        selectedSeason.bbMinors2.states = ckbbMinors2Winnings.Items.FindByText("States").Selected;
        selectedSeason.bbMinors2.nationals = ckbbMinors2Winnings.Items.FindByText("National").Selected;

        selectedSeason.bbMajors.team = txtbbMajorsTeam.Text;
        selectedSeason.bbMajors.otherText = txtbbMajorsOtherText.Text;
        selectedSeason.bbMajors.sections = ckbbMajorsWinnings.Items.FindByText("Sections").Selected;
        selectedSeason.bbMajors.regions = ckbbMajorsWinnings.Items.FindByText("Regions").Selected;
        selectedSeason.bbMajors.states = ckbbMajorsWinnings.Items.FindByText("States").Selected;
        selectedSeason.bbMajors.nationals = ckbbMajorsWinnings.Items.FindByText("National").Selected;

        selectedSeason.bbIntermediate.team = txtbbIntermediateTeam.Text;
        selectedSeason.bbIntermediate.otherText = txtbbIntermediateOtherText.Text;
        selectedSeason.bbIntermediate.sections = ckbbIntermediateWinnings.Items.FindByText("Sections").Selected;
        selectedSeason.bbIntermediate.regions = ckbbIntermediateWinnings.Items.FindByText("Regions").Selected;
        selectedSeason.bbIntermediate.states = ckbbIntermediateWinnings.Items.FindByText("States").Selected;
        selectedSeason.bbIntermediate.nationals = ckbbIntermediateWinnings.Items.FindByText("National").Selected;

        selectedSeason.bbJuniors.team = txtbbJuniorsTeam.Text;
        selectedSeason.bbJuniors.otherText = txtbbJuniorsOtherText.Text;
        selectedSeason.bbJuniors.sections = ckbbJuniorsWinnings.Items.FindByText("Sections").Selected;
        selectedSeason.bbJuniors.regions = ckbbJuniorsWinnings.Items.FindByText("Regions").Selected;
        selectedSeason.bbJuniors.states = ckbbJuniorsWinnings.Items.FindByText("States").Selected;
        selectedSeason.bbJuniors.nationals = ckbbJuniorsWinnings.Items.FindByText("National").Selected;

        selectedSeason.bbSeniors.team = txtbbSeniorsTeam.Text;
        selectedSeason.bbSeniors.otherText = txtbbSeniorsOtherText.Text;
        selectedSeason.bbSeniors.sections = ckbbSeniorsWinnings.Items.FindByText("Sections").Selected;
        selectedSeason.bbSeniors.regions = ckbbSeniorsWinnings.Items.FindByText("Regions").Selected;
        selectedSeason.bbSeniors.states = ckbbSeniorsWinnings.Items.FindByText("States").Selected;
        selectedSeason.bbSeniors.nationals = ckbbSeniorsWinnings.Items.FindByText("National").Selected;

        selectedSeason.sbMinors.team = txtsbMinorsTeam.Text;
        selectedSeason.sbMinors.otherText = txtsbMinorsOtherText.Text;
        selectedSeason.sbMinors.sections = cksbMinorsWinnings.Items.FindByText("Sections").Selected;
        selectedSeason.sbMinors.regions = cksbMinorsWinnings.Items.FindByText("Regions").Selected;
        selectedSeason.sbMinors.states = cksbMinorsWinnings.Items.FindByText("States").Selected;
        selectedSeason.sbMinors.nationals = cksbMinorsWinnings.Items.FindByText("National").Selected;

        selectedSeason.sbMinors2.team = txtsbMinors2Team.Text;
        selectedSeason.sbMinors2.otherText = txtsbMinors2OtherText.Text;
        selectedSeason.sbMinors2.sections = cksbMinors2Winnings.Items.FindByText("Sections").Selected;
        selectedSeason.sbMinors2.regions = cksbMinors2Winnings.Items.FindByText("Regions").Selected;
        selectedSeason.sbMinors2.states = cksbMinors2Winnings.Items.FindByText("States").Selected;
        selectedSeason.sbMinors2.nationals = cksbMinors2Winnings.Items.FindByText("National").Selected;

        selectedSeason.sbMajors.team = txtsbMajorsTeam.Text;
        selectedSeason.sbMajors.otherText = txtsbMajorsOtherText.Text;
        selectedSeason.sbMajors.sections = cksbMajorsWinnings.Items.FindByText("Sections").Selected;
        selectedSeason.sbMajors.regions = cksbMajorsWinnings.Items.FindByText("Regions").Selected;
        selectedSeason.sbMajors.states = cksbMajorsWinnings.Items.FindByText("States").Selected;
        selectedSeason.sbMajors.nationals = cksbMajorsWinnings.Items.FindByText("National").Selected;

        selectedSeason.sbJuniors.team = txtsbJuniorsTeam.Text;
        selectedSeason.sbJuniors.otherText = txtsbJuniorsOtherText.Text;
        selectedSeason.sbJuniors.sections = cksbJuniorsWinnings.Items.FindByText("Sections").Selected;
        selectedSeason.sbJuniors.regions = cksbJuniorsWinnings.Items.FindByText("Regions").Selected;
        selectedSeason.sbJuniors.states = cksbJuniorsWinnings.Items.FindByText("States").Selected;
        selectedSeason.sbJuniors.nationals = cksbJuniorsWinnings.Items.FindByText("National").Selected;

        selectedSeason.sbSeniors.team = txtsbSeniorsTeam.Text;
        selectedSeason.sbSeniors.otherText = txtsbSeniorsOtherText.Text;
        selectedSeason.sbSeniors.sections = cksbSeniorsWinnings.Items.FindByText("Sections").Selected;
        selectedSeason.sbSeniors.regions = cksbSeniorsWinnings.Items.FindByText("Regions").Selected;
        selectedSeason.sbSeniors.states = cksbSeniorsWinnings.Items.FindByText("States").Selected;
        selectedSeason.sbSeniors.nationals = cksbSeniorsWinnings.Items.FindByText("National").Selected;
    }

    protected void btnPastChamps_Click(object sender, EventArgs e)
    {
        rpChampions.DataSource = PastChampionSeason.allChampions;
        rpChampions.DataBind();
        mvPastChamps.ActiveViewIndex = 0;
    }
}
