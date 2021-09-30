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

public partial class DistrictLeagues : System.Web.UI.Page
{
    League selectedLeague;
    protected void Page_Load(object sender, EventArgs e)
    {
        selectedLeague = (League)Session["selectedLeague"];
        if (!Page.IsPostBack)
        {
            ((masterMain)Page.Master).setInformationMessage("");
            mViewLeagues.ActiveViewIndex = 0;
            grdLeagues.DataSource = League.allLeagues;

            if (Page.User.Identity.IsAuthenticated)
            {
                btnAddLeague.Visible = true;
                grdLeagues.Columns[0].Visible = true;
                rgvYear.MaximumValue = DateTime.Now.Year.ToString();
            }
            else
            {
                btnAddLeague.Visible = false;
                grdLeagues.Columns[0].Visible = false;
            }
            grdLeagues.DataBind();
        }
    }
    protected void grdLeagues_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewRow selectedRow = grdLeagues.Rows[e.NewEditIndex];

        selectedLeague = League.find(Convert.ToInt32(((Label)selectedRow.FindControl("lblLeagueId")).Text));

        loadLeague();
        btnSave.Visible = true;
        btnRemove.Visible = false;
        mViewLeagues.ActiveViewIndex = 1;
        Session["selectedLeague"] = selectedLeague;
        e.Cancel = true;
        btnAddLeague.Enabled = false;
    }

    protected void grdLeagues_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow selectedRow = grdLeagues.Rows[e.RowIndex];

        selectedLeague = League.find(Convert.ToInt32(((Label)selectedRow.FindControl("lblLeagueId")).Text));

        loadLeague();
        Session["selectedLeague"] = selectedLeague;
        btnSave.Visible = false;
        btnRemove.Visible = true;
        mViewLeagues.ActiveViewIndex = 1;
        e.Cancel = true;
        btnAddLeague.Enabled = false;
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        getLeague();

        if (selectedLeague.validate() == null)
        {
            if (selectedLeague.save())
            {
                ((masterMain)Page.Master).setInformationMessage("League was successfully saved");
                mViewLeagues.ActiveViewIndex = 0;
                grdLeagues.DataSource = League.allLeagues;
                grdLeagues.SelectedIndex = -1;
                grdLeagues.DataBind();
                btnAddLeague.Enabled = true;
            }
            else
            {
                ((masterMain)Page.Master).setErrorMessage("League was not saved! " + DistrictObject.sqlMessages);
            }
        }
        else
            ((masterMain)Page.Master).setErrorMessage(selectedLeague.validate());
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        grdLeagues.SelectedIndex = -1;
        mViewLeagues.ActiveViewIndex = 0;
        btnAddLeague.Enabled = true;
        ((masterMain)Page.Master).setInformationMessage("League Update Cancelled");
    }

    private void loadLeague()
    {
        txtLeagueName.Text = selectedLeague.Name;
        txtTown.Text = selectedLeague.town;
        txtWebSite.Text = selectedLeague.WebSite;
        txtCharterYear.Text = selectedLeague.charterYear;
        txtLeagueId.Text = selectedLeague.leagueId;
        if (selectedLeague.directions != null)
            txtDirections.Text = selectedLeague.directions;
        else
            txtDirections.Text = "";

        if (selectedLeague.President != null)
        {
            txtCellPhone.Text = selectedLeague.President.CellPhone;
            txtEmailAddress.Text = selectedLeague.President.EmailAddress;
            txtFirstName.Text = selectedLeague.President.FirstName;
            txtHomePhone.Text = selectedLeague.President.HomePhone;
            txtLastName.Text = selectedLeague.President.LastName;
        }
        else
        {
            txtCellPhone.Text = "";
            txtEmailAddress.Text = "";
            txtFirstName.Text = "";
            txtHomePhone.Text = "";
            txtLastName.Text = "";
        }
    }

    private void getLeague()
    {
        selectedLeague.charterYear = txtCharterYear.Text;
        selectedLeague.Name = txtLeagueName.Text;
        selectedLeague.town = txtTown.Text;
        selectedLeague.WebSite = txtWebSite.Text;
        if (selectedLeague.President == null)
            selectedLeague.President = new Person();
        selectedLeague.President.LastName = txtLastName.Text;
        selectedLeague.President.FirstName = txtFirstName.Text;
        selectedLeague.President.HomePhone = txtHomePhone.Text;
        selectedLeague.President.CellPhone = txtCellPhone.Text;
        selectedLeague.President.EmailAddress = txtEmailAddress.Text;
        selectedLeague.leagueId = txtLeagueId.Text;
        selectedLeague.directions = txtDirections.Text;
    }
    protected void btnAddLeague_Click(object sender, EventArgs e)
    {
        selectedLeague = new League();
        Session["selectedLeague"] = selectedLeague;
        loadLeague();
        btnSave.Visible = true;
        btnRemove.Visible = false;
        mViewLeagues.ActiveViewIndex = 1;
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (selectedLeague.delete())
        {
            mViewLeagues.ActiveViewIndex = 0;
            ((masterMain)Page.Master).setInformationMessage("League was removed");
            grdLeagues.DataSource = League.allLeagues;
            grdLeagues.SelectedIndex = -1;
            grdLeagues.DataBind();
        }
        else
        {
            ((masterMain)Page.Master).setErrorMessage("Remove was unsuccessful");
        }
    }
}
