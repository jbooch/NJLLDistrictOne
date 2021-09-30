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
    public partial class InterleagueTeamMaintenance : System.Web.UI.Page
    {
        public DataTable teamTable = null;
        public int newId = -1;
        private Staff adminPerson = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            adminPerson = (Staff)Session["adminPerson"];
            if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("default.aspx");
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
                    teamTable = (DataTable)Session["teamtable"];
                    if (Session["newID"] != null)
                        newId = (int)Session["newId"];
                }
            }
        }

        protected void dlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlTeamGrid.Visible = true;
            if (dlDivision.Items[0].Value == "-1")
            {
                dlDivision.Items.RemoveAt(0);
            }
            Division aDivision = Division.find(Convert.ToInt32(dlDivision.SelectedValue));

            teamTable= new DataTable();
 //           aTable.Columns.Add(new DataColumn("isTeam", Type.GetType("System.Boolean")));
            teamTable.Columns.Add(new DataColumn("recordId",Type.GetType("System.String")));
            teamTable.Columns.Add(new DataColumn("deleted", Type.GetType("System.Boolean")));
            teamTable.Columns.Add(new DataColumn("league", Type.GetType("System.String")));
            teamTable.Columns.Add(new DataColumn("leagueId",Type.GetType("System.String")));
            teamTable.Columns.Add(new DataColumn("name", Type.GetType("System.String")));
            teamTable.Columns.Add(new DataColumn("existingTeam", Type.GetType("System.Boolean")));
            teamTable.PrimaryKey = new DataColumn[] { teamTable.Columns["recordId"] };

            dlLeague.DataSource = League.allLeagues;
            dlLeague.DataTextField = "Name";
            dlLeague.DataValueField = "id";
            dlLeague.DataBind();
            dlLeague.Items.Insert(0, "[Select League]");

            foreach (Team aTeam in Team.getTeamsFor(aDivision))
            {
                if (aTeam != null && aTeam.isInterleagueTeam())
                {
                    DataRow aRow = teamTable.NewRow();
                    aRow["recordId"] = aTeam.id.ToString();
                    aRow["existingTeam"] = true;
                    aRow["deleted"] = false;
                    aRow["name"] = aTeam.name;
                    aRow["leagueId"] = aTeam.league.id;
                    aRow["league"] = aTeam.league.Name; ;
                    teamTable.Rows.Add(aRow);
                }
            }
            grdTeams.Columns[0].Visible = ckShowTeamId.Checked;
            grdTeams.DataSource = teamTable;
            grdTeams.DataBind();
            Session["teamtable"] = teamTable;
            Session["newId"] = newId;
        }

        protected void populateTeamList()
        {
            foreach (GridViewRow aRow in grdTeams.Rows)
            {
                string teamId = ((Label)aRow.FindControl("lblRecordID")).Text;
                DataRow teamRow = teamTable.Rows.Find(teamId);

                teamRow["deleted"] = ((CheckBox)aRow.FindControl("ckDelete")).Checked;

                teamRow["name"] = ((TextBox)aRow.FindControl("txtName")).Text;
            }
        }

        protected bool validateTeams() 
        {
            bool ans = true;
            foreach (GridViewRow aRow in grdTeams.Rows)
            {
                if (Convert.ToInt32(((Label)aRow.FindControl("lblRecordID")).Text) >=0 && ((CheckBox)aRow.FindControl("ckDelete")).Checked)
                {
                    ArrayList gamesList = Game.find(Team.find(Convert.ToInt32(((Label)aRow.FindControl("lblRecordID")).Text)));
                    if (gamesList.Count > 0)
                    {
                        ((Label)aRow.FindControl("lblError")).Visible = true;
                        ans = false;
                    }
                } else if (((TextBox)aRow.FindControl("txtName")).Text.Length ==0)
                {
                    ((Label)aRow.FindControl("lblError")).Visible = true;
                    ans = false;
                }
            }
            return ans;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (validateTeams())
            {
                populateTeamList();

                bool saveSuccess = true;

                foreach (DataRow teamRow in teamTable.Rows)
                {
                    int recordId = Convert.ToInt32((string)teamRow["recordId"]);
                    if (recordId >= 0 && (bool)teamRow["deleted"])
                    {
                        Team theTeam = Team.find(recordId);
                        theTeam.deleted = true;
                        saveSuccess = theTeam.save();
                    }
                    else if (recordId >= 0)
                    {
                        Team theTeam = Team.find(recordId);
                        theTeam.name = (string)teamRow["name"];
                        saveSuccess = theTeam.save();
                    }
                    else
                    {
                        // must be a new team nothing to do if new team is also deleted

                        if (!(bool)teamRow["deleted"])
                        {
                            Team theTeam = new InterleagueTeam();
                            theTeam.name = (string)teamRow["name"];
                            theTeam.league = League.find(Convert.ToInt32(teamRow["leagueId"]));
                            theTeam.division = Division.find(Convert.ToInt32(dlDivision.SelectedValue));

                            saveSuccess = theTeam.save();
                        }
                    }
                 }
                if (saveSuccess)
                {
                    Team.reset();
                    ((masterMain)Page.Master).setInformationMessage("Teams were saved.");
                    dlDivision_SelectedIndexChanged(sender, e);
                    Response.Redirect("default.aspx");
                }
                else
                {
                    ((masterMain)Page.Master).setErrorMessage("Highlighted teams were not saved.");
                }
            }
            else
                ((masterMain)Page.Master).setErrorMessage("Team Name is required for each team. Teams were not saved.");
        }

        protected void btnAddTeam_Click(object sender, EventArgs e)
        {
            League theLeague = League.find(Convert.ToInt32(dlLeague.SelectedValue));
            DataRow aRow = teamTable.NewRow();
            aRow["recordId"] = newId--;
            aRow["existingTeam"] = false;
            aRow["deleted"] = false;
            aRow["name"] = txtTeamName.Text;
            aRow["leagueId"] = theLeague.id;
            aRow["league"] = theLeague.Name;
            teamTable.Rows.Add(aRow);

            grdTeams.Columns[0].Visible = ckShowTeamId.Checked;
            grdTeams.DataSource = teamTable;
            grdTeams.DataBind();
            Session["teamtable"] = teamTable;
            Session["newId"] = newId;
        }

        protected void ckShowTeamId_changed(object sender, EventArgs e)
        {
            grdTeams.Columns[0].Visible = ckShowTeamId.Checked;
            grdTeams.DataSource = teamTable;
            grdTeams.DataBind();
            Session["teamtable"] = teamTable;
        }
    }
}
