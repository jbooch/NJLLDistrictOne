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
    public partial class ScheduleMaintenance : System.Web.UI.Page
    {
        private Game theGame = null;
        private Staff adminPerson = null;

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
                    dlDivision.DataSource = Division.allDivisions;
                    dlDivision.DataTextField = "DisplayName";
                    dlDivision.DataValueField = "id";
                    dlDivision.DataBind();
                    dlDivision.Items.Insert(0, new ListItem("[Select Division]", "-1"));
                }
                else
                {
                    theGame = (Game)Session["theGame"];
                }
            }
        }

        protected void dlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            ckPools.Items.Clear();
            ArrayList divisionGames = new ArrayList();
            foreach (Game aGame in Game.AllGames)
            {
                if (aGame.team1.division.id == Convert.ToInt32(dlDivision.SelectedValue))
                {
                    divisionGames.Add(aGame);
                    if (aGame.isPool())
                    {

                        if (ckPools.Items.FindByValue(((PoolGame)aGame).pool) == null)
                            ckPools.Items.Add(new ListItem(((PoolGame)aGame).pool));
                    }
                    else if (aGame.isBracket())
                    {
                        if (ckPools.Items.FindByValue(((BracketGame)aGame).gameNumber.ToString()) == null)
                            ckPools.Items.Add(new ListItem(((BracketGame)aGame).gameNumber.ToString()));
                    }
                    else
                    {
                        // must be an interleague game
                    }
                }
            }
            if (dlDivision.Items[0].Value == "-1")
                dlDivision.Items.RemoveAt(0);
            grdSchedule.DataSource = BuildDisplaySchedule(divisionGames);
            grdSchedule.DataBind();
        }

        protected void btnAddPool_Click(object sender, EventArgs e)
        {
            if (dlDivision.SelectedValue != "-1")
            {
                btnAddBracket.Visible = false;
                btnAddPool.Visible = false;
                dlDivision.Enabled = false;
                pnlGameDetail.Visible = true;
                grdSchedule.Enabled = false;
                displayGameDetailsFor(new PoolGame());
                pnlGameDisplay.Visible = false;
            }
            else
            {
                ((masterMain)Page.Master).setInformationMessage("You need to select a Division first");
            }
        }

        protected void btnAddInterleague_Click(object sender,EventArgs e)
        {
            if (dlDivision.SelectedValue != "-1")
            {
                btnAddBracket.Visible = false;
                btnAddPool.Visible = false;
                dlDivision.Enabled = false;
                pnlGameDetail.Visible = true;
                grdSchedule.Enabled = false;
                displayGameDetailsFor(new InterleagueGame());
                pnlGameDisplay.Visible = false;
            }
            else
            {
                ((masterMain)Page.Master).setInformationMessage("You need to select a Division first");
            }
        }

        protected void btnAddBracket_Click(object sender, EventArgs e)
        {
            if (dlDivision.SelectedValue != "-1")
            {
                btnAddBracket.Visible = false;
                btnAddPool.Visible = false;
                dlDivision.Enabled = false;
                pnlGameDetail.Visible = true;
                grdSchedule.Enabled = false;
                displayGameDetailsFor(new BracketGame(Division.find(Convert.ToInt32(dlDivision.SelectedValue))));
                pnlGameDisplay.Visible = false;
            }
            else
            {
                ((masterMain)Page.Master).setErrorMessage("You need to select a Division first");
            }
        }

        protected void calGameDate_SelectionChanged(object sender, EventArgs e)
        {
/*
            lblGameDate.Text = calGameDate.SelectedDate.ToString("MM/dd/yyyy");
            if ((calGameDate.SelectedDate.DayOfWeek.Equals(DayOfWeek.Saturday)
                || calGameDate.SelectedDate.DayOfWeek.Equals(DayOfWeek.Sunday))
                && txtHour.Text.Trim().Length == 0)
            {
                txtHour.Text = "06";
                txtMin.Text = "00";
            }
*/
        }

        private DataTable BuildDisplaySchedule(ArrayList gameList)
        {
            DataTable aTable = new DataTable();
            aTable.Columns.Add(new DataColumn("id", Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("GameDate", Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("Division", Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("Visitor", Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("Home", Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("resultAllowed", Type.GetType("System.Boolean")));
            aTable.Columns.Add(new DataColumn("PoolPlay", Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("Location", Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("updateComment", typeof(string)));

            foreach (Game aGame in gameList)
            {
                DataRow aRow = aTable.NewRow();
                aRow["id"] = aGame.id.ToString();
                aRow["GameDate"] = aGame.gameDate.ToString("MM/dd/yyyy hh:mm:tt");
                aRow["Division"] = aGame.team1.division.displayName;
                aRow["Visitor"] = aGame.team1.league.Name;
                aRow["Home"] = aGame.team2.league.Name;
                aRow["resultAllowed"] = aGame.personVerified == null;
                if (aGame.isPool())
                    aRow["PoolPlay"] = ((PoolGame)aGame).pool;
                else if (aGame.isBracket())
                    aRow["PoolPlay"] = ((BracketGame)aGame).gameNumber;
                aRow["Location"] = aGame.location;
                aRow["updateComment"] = aGame.updateComment;
                aTable.Rows.Add(aRow);
            }
            return aTable;
        }

        protected void grdSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game theGame = Game.find(Convert.ToInt32((((Label)grdSchedule.SelectedRow.FindControl("RecordId")).Text)));
            displayGameDetailsFor(theGame);
            btnDelete.Enabled = theGame.personVerified == null;
            pnlGameDetail.Visible = true;
            grdSchedule.Enabled = false;
        }

        private void displayGameDetailsFor(Game theGame)
        {
            if (theGame.gameDate != DateTime.MinValue)
            {
                txtGameDate.Text = theGame.gameDate.ToString("MM/dd/yyyy");
                txtGameTime.Text = theGame.gameDate.ToString("hh:mm tt");
            }
            else
            {
                txtGameDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                if (DateTime.Now.DayOfWeek.Equals(DayOfWeek.Saturday) || DateTime.Now.DayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    txtGameTime.Text = "12:00PM";
                }
                else
                {
                    txtGameTime.Text = "06:00PM";
                }
            }
            txtLocation.Text = theGame.location;
            txtUpdateComment.Text = theGame.updateComment;
            if (theGame.isPool())
            {
                PoolGame aPoolGame = (PoolGame)theGame;
                txtGameNumber.Visible = false;
                dlPool.Visible = true;
                lblTeam1.Text = "Visitor";
                lblTeam2.Text = "Home";
                dlPool.Visible = true;
                dlPool.DataSource = DistrictTeam.getPoolsFor(Division.find(Convert.ToInt32(dlDivision.SelectedItem.Value)));
                dlPool.DataBind();
                if (aPoolGame.pool != null)
                {
                    dlPool.Enabled = false;
                    dlPool.Items.FindByText(aPoolGame.pool).Selected = true;
                }
                else 
                {
                    dlPool.Enabled = true;
                }
                ArrayList teamList = Team.getTeamsFor(Division.find(Convert.ToInt32(dlDivision.SelectedItem.Value)), dlPool.SelectedValue);
                ArrayList districtTeams = new ArrayList();
                foreach(Team aTeam in teamList)
                {
                    if (aTeam.isDistrictTeam())
                        districtTeams.Add(aTeam);
                }
                dlTeam1.DataSource = districtTeams;
                dlTeam1.DataTextField = "displayName";
                dlTeam1.DataValueField = "id";
                dlTeam1.DataBind();
                dlTeam2.DataSource = districtTeams;
                dlTeam2.DataTextField = "displayName";
                dlTeam2.DataValueField = "id";
                dlTeam2.DataBind();
                if (aPoolGame.team1 != null)
                {
                        dlTeam1.Items.FindByValue(aPoolGame.team1.id.ToString()).Selected = true;
                        dlTeam2.Items.FindByValue(aPoolGame.team2.id.ToString()).Selected = true;
                }
            }
            else if (theGame.isBracket())
            {
                ArrayList teamList = Team.getTeamsFor(Division.find(Convert.ToInt32(dlDivision.SelectedItem.Value)));
                ArrayList districtTeams = new ArrayList();
                foreach (Team aTeam in teamList)
                {
                    if (aTeam.isDistrictTeam())
                        districtTeams.Add(aTeam);
                }
                BracketGame aBracketGame = (BracketGame)theGame;
                lblPoolBracket.Text = "Bracket Number";
                dlPool.Visible = false;
                txtGameNumber.Visible = true;
                txtGameNumber.Text = aBracketGame.gameNumber.ToString();
                txtGameNumber.MaxLength = 2;
                lblTeam1.Text = "Team 1";
                lblTeam2.Text = "Team 2";
                dlTeam1.DataSource = districtTeams;
                dlTeam1.DataTextField = "displayName";
                dlTeam1.DataValueField = "id";
                dlTeam1.DataBind();
                dlTeam2.DataSource = districtTeams;
                dlTeam2.DataTextField = "displayName";
                dlTeam2.DataValueField = "id";
                dlTeam2.DataBind();
                if (aBracketGame.team1 != null)
                {
                        dlTeam1.Items.FindByValue(aBracketGame.team1.id.ToString()).Selected = true;
                        dlTeam2.Items.FindByValue(aBracketGame.team2.id.ToString()).Selected = true;
                 }
            }
            else
            {
                ArrayList InterleagueTeams = new ArrayList();
                ArrayList teamList = Team.getTeamsFor(Division.find(Convert.ToInt32(dlDivision.SelectedItem.Value)));
                foreach(Team aTeam in teamList)
                {
                    if (aTeam.isInterleagueTeam())
                        InterleagueTeams.Add(aTeam);
                }
                lblTeam1.Text = "Visitor";
                dlTeam1.DataSource = InterleagueTeams;
                dlTeam1.DataTextField = "displayName";
                dlTeam1.DataValueField = "id";
                dlTeam1.DataBind();

                dlTeam2.DataSource = InterleagueTeams;
                dlTeam2.DataTextField = "displayName";
                dlTeam2.DataValueField = "id";
                dlTeam2.DataBind();
                lblTeam2.Text = "Home";
                dlPool.Visible = false;
                txtGameNumber.Visible = false;

            }

            Session["theGame"] = theGame;
        }

        protected void btnCancelGame_Click(object sender, EventArgs e)
        {
            pnlGameDetail.Visible = false;
            pnlGameDisplay.Visible = true;
            grdSchedule.Enabled = true;
            btnAddBracket.Visible = true;
            btnAddPool.Visible = true;
            dlDivision.Enabled = true;
        }

        protected void btnSaveGame_Click(object sender, EventArgs e)
        {
            theGame.gameDate = Convert.ToDateTime(txtGameDate.Text + " " + txtGameTime.Text);
            theGame.location = txtLocation.Text;
            theGame.team1 = Team.find(Convert.ToInt32(dlTeam1.SelectedValue));
            theGame.team2 = Team.find(Convert.ToInt32(dlTeam2.SelectedValue));
            theGame.updateComment = txtUpdateComment.Text;
            if (theGame.isPool())
            {
                ((PoolGame)theGame).pool = ((DistrictTeam)theGame.team1).pool;
                if (ckPools.Items.FindByValue(((PoolGame)theGame).pool) == null)
                    ckPools.Items.Add(new ListItem(((PoolGame)theGame).pool));
            }
            else if (theGame.isBracket())
            {
                ((BracketGame)theGame).gameNumber = Convert.ToInt32(txtGameNumber.Text);
                if (ckPools.Items.FindByValue(((BracketGame)theGame).gameNumber.ToString())==null)
                    ckPools.Items.Add(new ListItem(((BracketGame)theGame).gameNumber.ToString()));
            }
            if (theGame.save())
            {
                ((masterMain)Page.Master).setInformationMessage("Game was successfully saved");
                pnlGameDetail.Visible = false;
                pnlGameDisplay.Visible = true;
                ckPools_SelectedIndexChanged(sender, e);
                grdSchedule.Enabled = true;
                btnAddPool.Visible = true;
                btnAddBracket.Visible = true;
                dlDivision.Enabled = true;
            }
            else
            {
                ((masterMain)Page.Master).setErrorMessage("There was a problem saving the game");
            }
        }

        protected void ckPools_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList divisionGames = new ArrayList();
            foreach (Game aGame in Game.AllGames)
            {
                if (aGame.team1.division.id == Convert.ToInt32(dlDivision.SelectedValue))
                {
                    if ((ckPools.SelectedIndex == -1)
                        || (aGame.team1.division.id == Convert.ToInt32(dlDivision.SelectedValue) &&
                        aGame.isPool() && ckPools.Items.FindByValue(((PoolGame)aGame).pool).Selected == true))
                    {
                        divisionGames.Add(aGame);
                    }
                }
            }
            grdSchedule.DataSource = BuildDisplaySchedule(divisionGames);
            grdSchedule.DataBind();
        }
        protected void dlPool_SelectedIndexChanged(object sender, EventArgs e)
        {
            dlTeam1.DataSource = Team.getTeamsFor(Division.find(Convert.ToInt32(dlDivision.SelectedValue)), dlPool.SelectedValue);
            dlTeam1.DataBind();
            dlTeam2.DataSource = Team.getTeamsFor(Division.find(Convert.ToInt32(dlDivision.SelectedValue)), dlPool.SelectedValue);
            dlTeam2.DataBind();
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScheduleImport.aspx");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            theGame.delete();
        }
}
}
