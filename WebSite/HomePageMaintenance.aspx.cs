using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DistrictObjects;

public partial class HomePageMaintenance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.User.Identity.IsAuthenticated)
        {
            Response.Redirect("default.aspx");
        }
        else
        {
            DateTime countdownDate = DateTime.MinValue;
            if (!Page.IsPostBack)
            {
                countdownDate = Convert.ToDateTime(DistrictObject.getDistrictData("txtCountdownDate"));
                txtDefaultMessage.Text = DistrictObject.getDistrictData("txtDefaultMessage");
                txtCountdownMessage.Text = DistrictObject.getDistrictData("txtCountdownMessage");
                txtCountdownDate.Text = countdownDate.ToString("MM/dd/yyyy");
                txtCountdownTime.Text = countdownDate.ToString("hh:mm tt");
            }
            else
            {
/*
                countdownDate = (DateTime)Session["countdownDate"];
                txtCountdownDate.Text = countdownDate.ToString("MM/dd/yyyy");
                txtCountdownTime.Text = countdownDate.ToString("hh:mm tt");
                txtDefaultMessage.Text = (String)Session["defaultMessage"];
                txtCountdownMessage.Text = (String)Session["countdownMessage"];
*/
            }
        }

    }
    protected void btnDefaultSave_Click(object sender, EventArgs e)
    {
        string defaultMessageString = txtDefaultMessage.Text;
        while (defaultMessageString.LastIndexOf("<br/>") >= 0)
        {
            defaultMessageString = defaultMessageString.Remove(defaultMessageString.LastIndexOf("<br/>"));
        }
        txtDefaultMessage.Text = defaultMessageString;
        DistrictObject.saveDistrictData("txtDefaultMessage", defaultMessageString);
        DateTime countdownDate = Convert.ToDateTime(txtCountdownDate.Text + " " + txtCountdownTime.Text);
        DistrictObject.saveDistrictData("txtCountdownDate", countdownDate.ToString());
        DistrictObject.saveDistrictData("txtCountdownMessage", txtCountdownMessage.Text);

        Session["countdownDate"] = countdownDate;
        Session["countdownMessage"] = txtCountdownMessage.Text;
        Session["defaultMessage"] = defaultMessageString;
    }
}
