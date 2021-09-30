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
    public partial class TeamMaintenance : System.Web.UI.Page
    {
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

            txtDivRule.Text = aDivision.poolAdvanceRules;

            DataTable aTable = new DataTable();
            aTable.Columns.Add(new DataColumn("isTeam", Type.GetType("System.Boolean")));
            aTable.Columns.Add(new DataColumn("recordId",Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("existingTeam", Type.GetType("System.Boolean")));
            aTable.Columns.Add(new DataColumn("league", Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("leagueId",Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("name", Type.GetType("System.String")));
            aTable.Columns.Add(new DataColumn("pool", Type.GetType("System.String")));

            foreach (League aLeague in League.allLeagues)
            {
                DataRow aRow = aTable.NewRow();
                Team aTeam = Team.find(aLeague,aDivision);
                if (aTeam != null && aTeam.isDistrictTeam())
                {
                    DistrictTeam theTeam = (DistrictTeam)aTeam;
                    aRow["recordId"] = theTeam.id.ToString();
                    aRow["isTeam"] = true;
                    aRow["existingTeam"] = false;
                    aRow["name"] = theTeam.name;
                    aRow["pool"] = theTeam.pool;
                }
                else {
                    aRow["isTeam"] = false;
                    aRow["existingTeam"] = true;
                    aRow["name"] = "";
                    aRow["pool"] = "";
                }
                aRow["leagueId"] = aLeague.id;
                aRow["league"] = aLeague.Name; ;
                aTable.Rows.Add(aRow);       
            }
            grdTeams.Columns[0].Visible = ckShowTeamId.Checked;
            grdTeams.DataSource = aTable;
            grdTeams.DataBind();
        }

        protected bool validatePool()
        {
            bool ans = true;
            foreach (GridViewRow aRow in grdTeams.Rows)
            {
                if (((CheckBox)aRow.FindControl("ckIsTEam")).Checked)
                {
                    if (((TextBox)aRow.FindControl("txtPool")).Text.Trim().Length == 0)
                    {
                        ArrayList gamesList = Game.find(Team.find(Convert.ToInt32(((Label)aRow.FindControl("lblRecordID")).Text)));
                        if (gamesList.Count > 0)
                        {
                            ((Label)aRow.FindControl("lblError")).Visible = true;
                            ans = false;
                        }
                        else
                        {
                            ((Label)aRow.FindControl("lblError")).Visible = false;
                        }
                    }
                    else
                    {
                        ((Label)aRow.FindControl("lblError")).Visible = false;
                    }
                }
            }
            return ans;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (validatePool())
            {
                bool saveSuccess = true;

                Division aDivision = Division.find(Convert.ToInt32(dlDivision.SelectedValue));

                aDivision.poolAdvanceRules = txtDivRule.Text;

                aDivision.save();

                foreach (GridViewRow aRow in grdTeams.Rows)
                {
                    if (((CheckBox)aRow.FindControl("ckIsTEam")).Checked)
                    {
                        if (((CheckBox)aRow.FindControl("ckIsTEam")).Enabled == false)
                        {
                            DistrictTeam theTeam = (DistrictTeam)Team.find(Convert.ToInt32(((Label)aRow.FindControl("lblRecordId")).Text));
                            if (((TextBox)aRow.FindControl("txtPool")).Text.Trim().Length == 0)
                            {
                                // must be a delete
                                theTeam.deleted = true;
                            }
                            else
                            {
                                theTeam.pool = ((TextBox)aRow.FindControl("txtPool")).Text.ToUpper();
                                theTeam.name = ((TextBox)aRow.FindControl("txtName")).Text;
                            }
                            if (!theTeam.save())
                            {
                                ((Label)aRow.FindControl("lblError")).Visible = true;
                                saveSuccess = false;
                            }
                            else
                            {
                                ((Label)aRow.FindControl("lblError")).Visible = false;
                            }

                        }
                        else
                        {
                            DistrictTeam theTeam = new DistrictTeam();
                            theTeam.league = League.find(Convert.ToInt32(((Label)aRow.FindControl("lblLeagueId")).Text));
                            theTeam.division = Division.find(Convert.ToInt32(dlDivision.SelectedValue));
                            theTeam.name = ((TextBox)aRow.FindControl("txtName")).Text;
                            theTeam.pool = ((TextBox)aRow.FindControl("txtPool")).Text.ToUpper();
                            if (!theTeam.save())
                            {
                                ((Label)aRow.FindControl("lblError")).Visible = true;
                                saveSuccess = false;
                            }
                            else
                            {
                                ((Label)aRow.FindControl("lblError")).Visible = false;
                            }
                        }
                    }
                }
                if (saveSuccess)
                {
                    Team.reset();
                    ((masterMain)Page.Master).setInformationMessage("Teams were saved.");
                    dlDivision_SelectedIndexChanged(sender, e);
                }
                else
                {
                    ((masterMain)Page.Master).setErrorMessage("Highlighted teams were not saved.");
                }
            }
            else
                ((masterMain)Page.Master).setErrorMessage("Pool is required for each team. Teams were not saved.");
        }
}
}
