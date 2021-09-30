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
    public partial class TournamentHome : System.Web.UI.Page
    {
        private Staff adminPerson = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    lblUpdateTime.Text = DistrictObject.getDistrictData("lblUpdateTime");
                    lblAnnouncement.Text = DistrictObject.getDistrictData("lblAnnouncement");
                    txtAnnouncement.Text = lblAnnouncement.Text;
                }
                catch (Exception ex)
                {
                    lblAnnouncement.Text = "Welcome";
                    ((masterMain)Page.Master).setErrorMessage(ex.Message);
                }
            }
            if (Page.User.Identity.IsAuthenticated)
            {
                adminPerson = (Staff)Session["adminPerson"];
                pnlAdminAnnouncement.Visible = true;
            }
            else
            {
                pnlAdminAnnouncement.Visible = false;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string defaultMessageString = txtAnnouncement.Text;
            while (defaultMessageString.LastIndexOf("<br>") >= 0)
            {
                defaultMessageString = defaultMessageString.Remove(defaultMessageString.LastIndexOf("<br>"));
            }

            DistrictObject.saveDistrictData("lblAnnouncement", defaultMessageString);
            txtAnnouncement.Text = defaultMessageString;
            DistrictObject.saveDistrictData("lblUpdateTime",DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " by " + adminPerson.person.fullName);
            //            theAnnouncement.lastUpdateUser = ((Person)Session["adminPerson"]);
//            theAnnouncement.lastUpdate = DateTime.Now;
//            theAnnouncement.save();
//            lblUpdateTime.Text = theAnnouncement.lastUpdate.ToString("MM/dd/yyyy hh:mm tt") + " by " + theAnnouncement.lastUpdateUser.fullName;
            lblAnnouncement.Text = txtAnnouncement.Text;
        }
    }
}
