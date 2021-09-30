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
    public partial class InterleagueStandings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dlDivision.DataSource = Division.allDivisions;
                dlDivision.DataTextField = "displayName";
                dlDivision.DataValueField = "id";
                dlDivision.DataBind();
                dlDivision.Items.Insert(0,new ListItem("[Please Select a Division]","-1"));
            }
        }

        protected void dlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((masterMain)Page.Master).setInformationMessage("");

            Division selectedDivision = Division.find(Convert.ToInt32(dlDivision.SelectedValue));
            ArrayList teamList = null;

            if (dlDivision.SelectedValue != "-1")
            {
                teamList = InterleagueTeam.getTeamsFor(selectedDivision);

                if (teamList.Count > 0)
                {
                    grdTeams.DataSource = checkStandings(teamList);
                    grdTeams.DataBind();
                }
                else
                {
                    ((masterMain)Page.Master).setInformationMessage("No Teams in Division");
                }
            }
        }

        private ArrayList checkStandings(ArrayList standingList)
        {
            ArrayList sortedKeys = new ArrayList();
            Hashtable aHash = new Hashtable();
            ArrayList finalList = new ArrayList();

            foreach (Team aTeam in standingList)
            {
                if (aHash.ContainsKey(aTeam.wins))
                    ((ArrayList)aHash[aTeam.wins]).Add(aTeam);
                else
                {
                    sortedKeys.Add(aTeam.wins);
                    ArrayList temp = new ArrayList();
                    temp.Add(aTeam);
                    aHash.Add(aTeam.wins, temp);
                }
            }

            sortedKeys.Sort();
            sortedKeys.Reverse();

            foreach(int aWinsKey in sortedKeys)
            {
                // no tie breakers
                ArrayList teamsInList = (ArrayList)aHash[aWinsKey];
                if (teamsInList.Count == 1)
                    finalList.Add(teamsInList[0]);
                else if (teamsInList.Count == 2)
                {
                    // head to head tie breaker
                    if (Game.HeadToHead((Team)teamsInList[0], (Team)teamsInList[1]) > 0)
                    {
                        finalList.Add(teamsInList[0]);
                        finalList.Add(teamsInList[1]);
                    }
                    else
                    {
                        finalList.Add(teamsInList[1]);
                        finalList.Add(teamsInList[0]);
                    }
                }
                else
                {
                    // RPI    
                    teamsInList.Sort(new teamRPIComparer());
                    foreach (Team aTeam in teamsInList)
                    {
                        finalList.Add(aTeam);
                    }
                }
            }
            
            return finalList;
        }

    }
}
