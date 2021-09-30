using System;
using System.Web.UI;
using DistrictObjects;

namespace DistrictOne
{
    public partial class ScheduleResults : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Game theGame = (Game)Session["selectedGame"];
                if (theGame.team1.division.isBaseball)
                    lblDivision.Text = "Baseball";
                else
                    lblDivision.Text = "Softball";
                lblGameDateTime.Text = theGame.gameDate.ToString("MM/dd/yyyy hh:mm tt");
                lblHome.Text = theGame.team2.displayName;
                lblLevelOfPlay.Text = theGame.team1.division.name;
                lblVisitor.Text = theGame.team1.displayName;
                if (theGame.isPool())
                {
                    if (((PoolGame)theGame).pool.ToUpper().Equals("A"))
                        lblPoolBracket.Text = "East";
                    else
                        lblPoolBracket.Text = "West";
                    //                    lblPoolBracket.Text = ((PoolGame)theGame).pool;

                }
                else if (theGame.isBracket())
                {
                    lblPoolBracket.Text = ((BracketGame)theGame).gameNumber.ToString();
                } else
                {
                    lblPoolBracket.Text = "Interleague";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Results aResults = new Results();

            aResults.theGame = (Game)Session["selectedGame"];
            aResults.score1 = Convert.ToInt32(txtVisitorScore.Text);
            aResults.score2 = Convert.ToInt32(txtHomeScore.Text);
            if (aResults.theGame.isPool())
            {
                aResults.fieldInnings1 = 6;
                aResults.fieldInnings2 = 6;
            }
            aResults.dateReceived = DateTime.Now;
            aResults.recordedBy = txtReportedBy.Text;
            aResults.phoneContact = txtPhoneContact.Text;
            aResults.comments = txtComments.Text;

            if (aResults.save())
            {
                ((masterMain)Page.Master).setInformationMessage("Results successfully saved.  Thanks");
                txtComments.Text = "";
                txtHomeScore.Text = "";
                txtVisitorScore.Text = "";
                string emailBody = "Results were entered for: " + aResults.theGame.ToString() +".";
                DistrictMailer.SendEmail("jbooch@booch.net", "District Game Results", emailBody);
            }
            else
                ((masterMain)Page.Master).setErrorMessage("There was a problem and your results were not saved.");

        }
    }
}
