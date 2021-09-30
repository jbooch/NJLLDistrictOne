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

namespace DistrictOne
{
    public partial class ApproveGameResults : System.Web.UI.Page
    {
        private Staff adminPerson = null;
        private ArrayList resultsGames = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            adminPerson = (Staff)Session["adminPerson"];
            if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    resultsGames = new ArrayList();
                    foreach (Game aGame in Game.AllGames)
                    {
                        if (aGame.personVerified == null && aGame.results.Count > 0)
                        {
                            resultsGames.Add(aGame);
                        }
                    }
                    rptApproveResults.DataSource = resultsGames;
                    rptApproveResults.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem anItem in rptApproveResults.Items)
            {
                int tScore1 = Convert.ToInt32(((TextBox)anItem.FindControl("txtScore1")).Text);
                int tScore2 = Convert.ToInt32(((TextBox)anItem.FindControl("txtScore2")).Text);
                string updateComment = ((TextBox)anItem.FindControl("txtUpdateComment")).Text;
                if (tScore1 + tScore2 > 0)
                {
                    Game theGame = Game.find(Convert.ToInt32(((Label)anItem.FindControl("recordId")).Text));
                    theGame.score1 = tScore1;
                    theGame.score2 = tScore2;
                    theGame.fieldInnings1 = theGame.team1.division.defaultInnings;
                    theGame.fieldInnings2 = theGame.team1.division.defaultInnings;
                    theGame.personVerified = adminPerson.person;
                    theGame.dateVerified = DateTime.Now;
                    theGame.updateComment = updateComment;
                    theGame.save();
                }
            }
            ((masterMain)Page.Master).setInformationMessage("Game Results successfully recorded.");
            ckIncludComple_CheckedChanged(sender, e);
        }
        protected void ckIncludComple_CheckedChanged(object sender, EventArgs e)
        {
            resultsGames = new ArrayList();
            if (ckIncludComple.Checked)
            {
                foreach (Game aGame in Game.AllGames)
                {
                    if (aGame.results.Count > 0)
                    {
                        resultsGames.Add(aGame);
                    }
                }
            }
            else
            {
                foreach (Game aGame in Game.AllGames)
                {
                    if (aGame.personVerified == null && aGame.results.Count > 0)
                    {
                        resultsGames.Add(aGame);
                    }
                }
            }
            rptApproveResults.DataSource = resultsGames;
            rptApproveResults.DataBind();
        }
}
}
