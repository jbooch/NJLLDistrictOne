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

public partial class Deafult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        { 
            string temp = DistrictObject.getDistrictData("lblDefaultMessage");
            if (temp.Trim().Length > 0)
            {
                lblDefaultHeaderMessage.Visible = true;
                lblDefaultHeaderMessage.Text = temp;
            }
            else
            {
                lblDefaultHeaderMessage.Visible = false;
                lblDefaultHeaderMessage.Text = null;
            }
            CardHTML.Text = setupCards();

            lblUpcomingGames.Text = setupUpcomingGames();
        }

    }

    private string setupUpcomingGames()
    {
        string tUpcomingGamesString = "";

        bool firstRec = true;

        ArrayList gameList = Game.AllGames;

        foreach(Game aGame in gameList)
        {
            if (aGame.IsGameDateEqual(DateTime.Now))
            {
                if (firstRec)
                {
                    tUpcomingGamesString += "<div class = \"carousel-item active\" />";
                    firstRec = false;
                }
                else
                {
                    tUpcomingGamesString += "<div class = \"carousel-item \" />";
                }
                tUpcomingGamesString += "<div class = \"background-dark\">";
                tUpcomingGamesString += "<h5 class=\"card-title\">" + aGame.gameDateDisplay + "</h5>";
                tUpcomingGamesString += "<p class=\"card-body\">" + aGame.team1.displayName + "<br/>" + "vs<br/>" + aGame.team2.displayName + "<br/>" + aGame.location + "</p>";
                tUpcomingGamesString += "<p class=\"card-footer\"></p>";
                tUpcomingGamesString += "</div></div>";
            }
        }
        if (firstRec)
        {
            // must not be any games scheduled for today
            tUpcomingGamesString += "<div class = \"carousel-item active\" />";
            tUpcomingGamesString += "<div class = \"background-dark\">";
            tUpcomingGamesString += "<h5 class=\"card-title\">No Games Today</h5>";
            tUpcomingGamesString += "<p class=\"card-body\">No Games</p>";
            tUpcomingGamesString += "<p class=\"card-footer\"></p>";
            tUpcomingGamesString += "</div></div>";
        }
        return tUpcomingGamesString;
    }

    private string setupCards()
    {
        string tCardHTML = "";
        ArrayList CardList = DistrictCard.cardCountArray;
        if (CardList.Count > 0)
        {
            // build first card as active
            tCardHTML += "<div class = \"carousel-item active\" />";
            tCardHTML += "<div class = \"" + ((DistrictCard)CardList[0]).CardColor + "\">";
            tCardHTML += "<h4 class=\"card-title\">" + ((DistrictCard)CardList[0]).cardTitle + "</h4>";
            if (((DistrictCard)CardList[0]).image != null && ((DistrictCard)CardList[0]).image.Trim().Length > 0)
                tCardHTML += "<img src='" + System.Configuration.ConfigurationManager.AppSettings.Get("umpirePhotoDir") + ((DistrictCard)CardList[0]).image + "' class='img-fluid' alt='card image' width='250px'>";
            tCardHTML += "<p class=\"card-body\">" + ((DistrictCard)CardList[0]).cardBody + "</p>";
            tCardHTML += "<p class=\"card-footer\">" + ((DistrictCard)CardList[0]).cardFooter + "</p>";
            tCardHTML += "</div></div>";
        }
        for (int x = 1; x < CardList.Count; x++)
        {
            // build remainder of cards
            tCardHTML += "<div class = \"carousel-item \" />";
            tCardHTML += "<div class = \"" + ((DistrictCard)CardList[x]).CardColor + "\">";
            tCardHTML += "<h4 class=\"card-title\">" + ((DistrictCard)CardList[x]).cardTitle + "</h4>";
            if (((DistrictCard)CardList[x]).image != null && ((DistrictCard)CardList[x]).image.Trim().Length > 0)
                tCardHTML += "<img src=\"" + System.Configuration.ConfigurationManager.AppSettings.Get("umpirePhotoDir") + ((DistrictCard)CardList[x]).image + "\" class='img-fluid' alt='card image' width='250px'>";
            tCardHTML += "<p class=\"card-body\">" + ((DistrictCard)CardList[x]).cardBody + "</p>";
            tCardHTML += "<p class=\"card-footer\">" + ((DistrictCard)CardList[x]).cardFooter + "</p>";
            tCardHTML += "</div></div>";
        }
        return tCardHTML;
    }

    protected void tmUpdate_Tick(object sender, EventArgs e)
    {
        string countdownMessage = null;
        DateTime countdownDate = DateTime.MinValue;
        if (Session["countdownMessage"] != null)
            countdownMessage = (string)Session["countdownMessage"];
        else
        {
            countdownMessage = DistrictObject.getDistrictData("lblCountdownMessage");
            Session["countdownMessage"] = countdownMessage;
        }

        if (Session["countdownDate"] != null)
        {
            countdownDate = (DateTime)Session["countdownDate"];
        }
        else
        {
            countdownDate = Convert.ToDateTime(DistrictObject.getDistrictData("lblCountdownDate"));
            Session["countdownDate"] = countdownDate;
        }

        DateTime currentDate = DateTime.Now;

        countdownDate = countdownDate.AddHours(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("timeDiff")));

        TimeSpan aSpan = (TimeSpan)countdownDate.Subtract(currentDate);
        if (aSpan.Ticks > 0)
        {
            countdownPanel.Visible = true;
            lblDays.Text = aSpan.Days.ToString("##0");
            lblhours.Text = aSpan.Hours.ToString("#0");
            lblMinutes.Text = aSpan.Minutes.ToString("#0");
            lblSeconds.Text = aSpan.Seconds.ToString("#0");
        }
        else
        {
            countdownPanel.Visible = false;
            lblDays.Text = "0";
            lblhours.Text = "0";
            lblMinutes.Text = "0";
            lblSeconds.Text = "0";
        }
        lblCountdown.Text = countdownMessage;
    }

    protected void btnAdminRefresh_Click(object sender, EventArgs e)
    {
        ((masterMain)Page.Master).resetObjects();
    }


    protected void btnCards_Click(object sender, EventArgs e)
    {
        ((masterMain)Page.Master).resetMessage();
        Response.Redirect("CardMaintenance.aspx");
    }
}