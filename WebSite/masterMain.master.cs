using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DistrictObjects;


public partial class masterMain : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.User.Identity.IsAuthenticated)
        {
            lblAdminNav.Text = "";
        }
        else
        {
            lblAdminNav.Text = setAdminNavigation();
        }

        if (!Page.IsPostBack)
        {
            lblCarouselSponsors.Text = buildSponsorsCarosole();
        }

        copyRiteYear.Text = DateTime.Now.Year.ToString("####");
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
    }

    private string buildSponsorsCarosole()
    {
        string imagePath = System.Configuration.ConfigurationManager.AppSettings.Get("SponsorImageDirectory");
        bool firstRec = true;
        string tSponsorHTML = "";

        foreach(DistrictSponsor aSponsor in SponsorCollection.Sponsors)
        {
            if (firstRec)
            {
                tSponsorHTML += "<div class = \"carousel-item active\" />";
                firstRec = false;
            }
            else
            {
                tSponsorHTML += "<div class = \"carousel-item \" />";
            }
            tSponsorHTML += "<div class = \"background-dark\">";
            tSponsorHTML += "<h5 class=\"card-title\">" + aSponsor.companyName+ "</h5>";
            tSponsorHTML += "<p class=\"card-body\">" + aSponsor.bodyDisplay(imagePath) + "</p>";
            tSponsorHTML += "<p class=\"card-footer\">" + aSponsor.footerDisplay() + "</p>";
            tSponsorHTML += "</div></div>";
        }

        if (firstRec)
        {
            // must not be any games scheduled for today
            tSponsorHTML += "<div class = \"carousel-item active\" />";
            tSponsorHTML += "<div class = \"background-dark\">";
            tSponsorHTML += "<h5 class=\"card-title text-center\">Your Business Can Be here</h5>";
            tSponsorHTML += "<p class=\"card-body\">If you would like to become a District One sponsor, please contact</p>";
            tSponsorHTML += "<p class=\"card-footer\"><a href=\"mailto: sponsors@llnjone.org\">Email Us</a> </p>";
            tSponsorHTML += "</div></div>";
        }

        return tSponsorHTML;
    }

    protected void loginAdmin_Authenticate(object sender, AuthenticateEventArgs e)
    {
        Login aLogin = (Login)adminLoginView.FindControl("loginAdmin");

        e.Authenticated = false;

        Staff adminPerson = Staff.findByEmail(aLogin.UserName);
        if (adminPerson != null)
        {
            if (aLogin.Password == System.Configuration.ConfigurationManager.AppSettings.Get("adminpwd"))
            {
                Session.Add("AdminPerson", adminPerson);
                
                e.Authenticated = true;
             }
        }
    }

    private string setAdminNavigation()
    {
        string navString = "";

        navString = "<li class=\"nav-item dropdown\">";
        navString += "<a class=\"nav-link dropdown-toggle\" href=\"AdminMain.aspx\" id=\"navbarDropdownMenuLink\" role=\"button\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">Administrators</a>";
        navString += "<div class=\"dropdown-menu\" aria-labelledby=\"navbarDropdownMenuLink\">";
        navString += "<a class=\"dropdown-item\" href=\"HomePageMaintenance.aspx\">Home Page Maintenance</a>";
        navString += "<a class=\"dropdown-item\" href=\"TeamMaintenance.aspx\">District Team Maintenance</a>";
        navString += "<a class=\"dropdown-item\" href=\"InterleagueTeamMaintenance.aspx\">Interleague Team Maintenance</a>";
        navString += "<a class=\"dropdown-item\" href=\"ScheduleMaintenance.aspx\">Schedule Maintenance</a>";
        navString += "<a class=\"dropdown-item\" href=\"ApproveGameResults.aspx\">Approve Game Results</a>";
        navString += "<a class=\"dropdown-item\" href=\"CardMaintenance.aspx\">Card Maintenance</a>";
        navString += "<a class=\"dropdown-item\" href=\"SponsorMaintenance.aspx\">Sponsor Maintenance</a>";
        navString += "</div></li>";

        return navString;
    }

public void setErrorMessage(string aText)
    {
        lblMessage.Text = aText;
        lblMessage.CssClass = "text-danger";
    }
    public void setInformationMessage(string aText)
    {
        lblMessage.Text = aText;
        lblMessage.CssClass = "text-info";
    }
    public void resetMessage()
    {
        lblMessage.Text = "";
    }
    public void resetObjects()
    {
        Staff.reset();
        League.reset();
        Team.reset();
        SpecialGame.reset();
        DistrictObjects.CalendarItem.reset();
        Division.reset();
        Game.reset();
        Person.reset();
        Umpire.reset();
        PastChampion.reset();
        DistrictCard.Reset();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        resetObjects();
    }
}
