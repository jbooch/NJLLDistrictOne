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

public partial class DistrictCalendar : System.Web.UI.Page
{
    CalendarItem anItem;
    protected void Page_Load(object sender, EventArgs e)
    {
        anItem = (CalendarItem)Session["anItem"];
        if (!Page.IsPostBack)
        {
            grdCalendar.DataSource = CalendarItem.allItema;
            mvCalendar.ActiveViewIndex = 0;
            if (Page.User.Identity.IsAuthenticated)
            {
                btnAdd.Visible = true;
                grdCalendar.Columns[0].Visible = true;
            }
            else
            {
                btnAdd.Visible = false;
                grdCalendar.Columns[0].Visible = false;
            }
            grdCalendar.DataBind();
        }
    }
    protected void grdCalendar_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnSave.Text = "Save";
        anItem = (CalendarItem)CalendarItem.allItema[e.NewEditIndex];

        populateCalendarEntry();
        mvCalendar.ActiveViewIndex = 1;
        e.Cancel = true;
    }

    private void populateCalendarEntry()
    {
        txtDescription.Text = anItem.description;
        txtPlace.Text = anItem.place;
        tstSubject.Text = anItem.subject;
        txtDate.Text = anItem.date.ToString("MMMM dd,yyyy");
        txtTime.Text = anItem.date.ToString("hh:mm tt");
        Session["anItem"] = anItem;
    }

    private void getCalendarEntry()
    {
        anItem.description = txtDescription.Text;
        anItem.place = txtPlace.Text;
        anItem.subject = tstSubject.Text;
        DateTime eventDate = Convert.ToDateTime(txtDate.Text);
        anItem.date = Convert.ToDateTime(eventDate.ToString("MM/dd/yyyy") + " " + txtTime.Text); // + rbAP.SelectedValue);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (btnSave.Text.Equals("Delete"))
        {
            if (anItem.delete())
            {
                grdCalendar.DataSource = CalendarItem.allItema;
                grdCalendar.DataBind();
                mvCalendar.ActiveViewIndex = 0;
                btnSave.Text = "Save";
            }
        }
        else
        {
            getCalendarEntry();
            if (anItem.save())
            {
                grdCalendar.DataSource = CalendarItem.allItema;
                grdCalendar.DataBind();
                mvCalendar.ActiveViewIndex = 0;
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Save";
        mvCalendar.ActiveViewIndex = 0;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        anItem = new CalendarItem();
        populateCalendarEntry();
        btnSave.Text = "Save";
        mvCalendar.ActiveViewIndex = 1;
    }
    protected void grdCalendar_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        anItem = (CalendarItem)CalendarItem.allItema[e.RowIndex];

        populateCalendarEntry();
        btnSave.Text = "Delete";
        mvCalendar.ActiveViewIndex = 1;
    }
}
