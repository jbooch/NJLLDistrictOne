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
    public partial class Schedule : System.Web.UI.Page
    {
        private string sortValue = null;
        private DataView gamesView = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            gamesView = (DataView)Session["gamesView"];
            sortValue = (string)Session["sortValue"];
            if (Page.User.Identity.IsAuthenticated)
            {
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
            }
            if (!Page.IsPostBack)
            {
                gamesView = null;
                lstDivision.DataSource = Division.allDivisions;
                lstDivision.DataValueField = "id";
                lstDivision.DataTextField = "DisplayName";
                lstDivision.DataBind();

                lstLeague.DataSource = League.allLeagues;
                lstLeague.DataValueField = "id";
                lstLeague.DataTextField = "Name";
                lstLeague.DataBind();

                grdSchedule.DataSource = BuildDisplaySchedule(Game.AllGames);
                grdSchedule.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("schedulePageSize"));
                grdSchedule.DataBind();
                setStartDate(DateTime.Now);
                lblPage.Text = "Page: " + ((grdSchedule.PageIndex + 1)).ToString("###") + " of " + grdSchedule.PageCount.ToString("###");
            }

        }
        private DataView BuildDisplaySchedule(ArrayList aList)
        {
            if (gamesView == null)
            {
                gamesView = new DataView();
                DataTable gamesTable = new DataTable("gamesTable");
                gamesTable.Columns.Add(new DataColumn("id", Type.GetType("System.String")));
                gamesTable.Columns.Add(new DataColumn("GameDate", Type.GetType("System.String")));
                gamesTable.Columns.Add(new DataColumn("Division", Type.GetType("System.String")));
                gamesTable.Columns.Add(new DataColumn("Visitor", Type.GetType("System.String")));
                gamesTable.Columns.Add(new DataColumn("Home", Type.GetType("System.String")));
                gamesTable.Columns.Add(new DataColumn("resultAllowed", Type.GetType("System.Boolean")));
                gamesTable.Columns.Add(new DataColumn("PoolPlay", Type.GetType("System.String")));
                gamesTable.Columns.Add(new DataColumn("Location", Type.GetType("System.String")));
                gamesTable.Columns.Add(new DataColumn("HomeScore", Type.GetType("System.String")));
                gamesTable.Columns.Add(new DataColumn("VisitorScore", Type.GetType("System.String")));
                gamesTable.Columns.Add(new DataColumn("updateComment", typeof(string)));
                gamesTable.Columns.Add(new DataColumn("updateTimestamp",Type.GetType("System.DateTime")));
                gamesView.Table = gamesTable;
            }
            else
            {
                gamesView.Table.Clear();
            }

            aList.Sort();

            foreach (Game aGame in aList)
            {
                if (aGame.isPool() || aGame.isBracket())
                {
                    DataRow aRow = gamesView.Table.NewRow();
                    aRow["id"] = aGame.id.ToString();
                    aRow["GameDate"] = aGame.gameDate.ToString("MM/dd/yyyy hh:mm:tt");
                    aRow["Division"] = aGame.team1.division.displayName;
                    aRow["Visitor"] = aGame.team1.displayName;
                    aRow["Home"] = aGame.team2.displayName;
                    aRow["resultAllowed"] = (aGame.personVerified == null && aGame.gameDate.Date <= DateTime.Now.Date);
                    aRow["updateComment"] = aGame.updateComment;
                    if (aGame.isPool())
                    {
                        if (((PoolGame)aGame).pool.ToUpper().Equals("A"))
                            aRow["PoolPlay"] = "East";
                        else
                            aRow["PoolPlay"] = "West";
                    }
                    else
                        aRow["PoolPlay"] = ((BracketGame)aGame).gameNumber;
                    aRow["Location"] = aGame.location;
                    if (aGame.personVerified != null)
                    {
                        aRow["HomeScore"] = aGame.score2.ToString("#0");
                        aRow["VisitorScore"] = aGame.score1.ToString("#0");
                    }
                    aRow["updateTimestamp"] = aGame.LastTmp;
                    gamesView.Table.Rows.Add(aRow);
                }
            }
            if (gamesView.Count > 0)
            {
                Session["gamesView"] = gamesView;
                if (sortValue == null)
                {
                    Session["sortValue"] = "gameDate";
                    sortValue = "gameDate";
                    gamesView.Sort = "gameDate, PoolPlay, Home";
                }
                else
                {
                    if (sortValue.Equals("gameDate"))
                        gamesView.Sort = "gameDate, PoolPlay, Home";
                    else
                        gamesView.Sort = sortValue + ", gameDate";
                }
                populateDatesList();
            }
            else
            {
                ((masterMain)Page.Master).setInformationMessage("No Games entered yet");
            }
            return gamesView;
        }

        protected void grdSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game aGame = Game.find(Convert.ToInt32(((Label)grdSchedule.SelectedRow.FindControl("RecordId")).Text));
            Session.Add("selectedGame", aGame);
            Response.Redirect("ScheduleResults.aspx");
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            ArrayList gameList = new ArrayList();
            foreach (Game aGame in Game.AllGames)
            {
                if ((lstDivision.Items.FindByValue(aGame.team1.division.id.ToString()).Selected
                    || lstDivision.SelectedIndex == -1)
                    && (lstLeague.Items.FindByValue(aGame.team1.league.id.ToString()).Selected
                    || lstLeague.Items.FindByValue(aGame.team2.league.id.ToString()).Selected
                    || lstLeague.SelectedIndex == -1))
                {
                    gameList.Add(aGame);
                }
            }
            grdSchedule.DataSource = BuildDisplaySchedule(gameList);
            grdSchedule.PageIndex = 0;
            grdSchedule.DataBind();
            lblPage.Text = "Page: " + ((grdSchedule.PageIndex + 1)).ToString("###") + " of " + grdSchedule.PageCount.ToString("###");
        }

        protected void dlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gamesView.Count; i++)
            {
                if (((string)(gamesView[i])[sortValue]).StartsWith(dlPage.SelectedValue))
                {
                    grdSchedule.DataSource = gamesView;
                    grdSchedule.DataBind();
                    grdSchedule.PageIndex = i / grdSchedule.PageSize;
                    grdSchedule.DataBind();
                    break;
                }
            }
            lblPage.Text = "Page: " + ((grdSchedule.PageIndex + 1)).ToString("###") + " of " + grdSchedule.PageCount.ToString("###");
        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            grdSchedule.DataSource = gamesView;
            grdSchedule.PageIndex = 0;
            grdSchedule.DataBind();
            dlPage.SelectedIndex = -1;
            if (sortValue.Equals("gameDate"))
                dlPage.Items.FindByValue(((string)gamesView[0]["GameDate"]).Substring(0, 10)).Selected = true;
            else
                dlPage.Items.FindByValue(((string)gamesView[0][sortValue])).Selected = true;

            lblPage.Text = "Page: " + ((grdSchedule.PageIndex + 1)).ToString("###") + " of " + grdSchedule.PageCount.ToString("###");
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            grdSchedule.DataSource = gamesView;
            grdSchedule.DataBind();
            grdSchedule.PageIndex = grdSchedule.PageCount - 1;
            grdSchedule.DataBind();
            dlPage.SelectedIndex = -1;
            if (sortValue.Equals("gameDate"))
                dlPage.Items.FindByText(((string)gamesView[grdSchedule.PageIndex * grdSchedule.PageSize]["gameDate"]).Substring(0, 10)).Selected = true;
            else
                dlPage.Items.FindByText(((string)gamesView[grdSchedule.PageIndex * grdSchedule.PageSize][sortValue])).Selected = true;


            lblPage.Text = "Page: " + ((grdSchedule.PageIndex + 1)).ToString("###") + " of " + grdSchedule.PageCount.ToString("###");
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (grdSchedule.PageIndex < (grdSchedule.PageCount - 1))
            {
                grdSchedule.DataSource = gamesView;
                grdSchedule.DataBind();
                grdSchedule.PageIndex++;
                grdSchedule.DataBind();
                dlPage.SelectedIndex = -1;
                if (sortValue.Equals("gameDate"))
                    dlPage.Items.FindByValue(((string)gamesView[grdSchedule.PageIndex * grdSchedule.PageSize]["GameDate"]).Substring(0, 10)).Selected = true;
                else
                    dlPage.Items.FindByValue(((string)gamesView[grdSchedule.PageIndex * grdSchedule.PageSize][sortValue])).Selected = true;

                lblPage.Text = "Page: " + ((grdSchedule.PageIndex + 1)).ToString("###") + " of " + grdSchedule.PageCount.ToString("###");
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (grdSchedule.PageIndex > 0)
            {
                grdSchedule.DataSource = gamesView;
                grdSchedule.DataBind();
                grdSchedule.PageIndex--;
                grdSchedule.DataBind();
                dlPage.SelectedIndex = -1;
                if (sortValue.Equals("gameDate"))
                    dlPage.Items.FindByValue(((string)gamesView[grdSchedule.PageIndex * grdSchedule.PageSize]["gameDate"]).Substring(0, 10)).Selected = true;
                else
                    dlPage.Items.FindByValue(((string)gamesView[grdSchedule.PageIndex * grdSchedule.PageSize][sortValue])).Selected = true;

                lblPage.Text = "Page: " + ((grdSchedule.PageIndex + 1)).ToString("###") + " of " + grdSchedule.PageCount.ToString("###");
            }
        }

        private void populateDatesList()
        {
            dlPage.Items.Clear();
            foreach (DataRow aRow in gamesView.Table.Rows)
            {
                if (sortValue.Equals("gameDate"))
                {
                    if (dlPage.Items.FindByText(((string)aRow["GameDate"]).Substring(0, 10)) == null)
                    {
                        dlPage.Items.Add(((string)aRow["GameDate"]).Substring(0, 10));
                    }
                }
                else
                {
                    if (dlPage.Items.FindByText((string)aRow[sortValue]) == null)
                        dlPage.Items.Add(((string)aRow[sortValue]));
                }
            }
        }

        private void setStartDate(DateTime aDate)
        {
            if (dlPage.Items.Count > 0)
            {
                bool found = false;
                dlPage.SelectedIndex = -1;
                for (int x = 0; x < dlPage.Items.Count; x++)
                {
                    if (dlPage.Items.Contains(new ListItem(aDate.ToString("MM/dd/yyyy"))))
                    {
                        dlPage.Items.FindByText(aDate.ToString("MM/dd/yyyy")).Selected = true;
                        found = true;
                    }
                }
                if (!found)
                    dlPage.Items[dlPage.Items.Count - 1].Selected = true;
                this.dlPage_SelectedIndexChanged(dlPage, new EventArgs());
            }
        }
        protected void grdSchedule_Sorting(object sender, GridViewSortEventArgs e)
        {
            gamesView.Sort = e.SortExpression + ", gameDate, PoolPlay, Home";

            grdSchedule.DataSource = gamesView;
            grdSchedule.PageIndex = 0;
            grdSchedule.DataBind();
            sortValue = e.SortExpression;
            Session["sortValue"] = e.SortExpression;
            lblPage.Text = "Page: " + ((grdSchedule.PageIndex + 1)).ToString("###") + " of " + grdSchedule.PageCount.ToString("###");
        }
        protected void grdSchedule_Sorted(object sender, EventArgs e)
        {
            populateDatesList();
            dlPage.SelectedIndex = -1;
            if (sortValue.Equals("gameDate"))
                dlPage.Items.FindByValue(((string)gamesView[0]["GameDate"]).Substring(0, 10)).Selected = true;
            else
                dlPage.Items.FindByValue(((string)gamesView[0][sortValue])).Selected = true;            
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScheduleExport.aspx");
            // Set up Spresdsheet

            // Extract currently filtered Games 
            // Save the spreadsheet
        }
}
}
