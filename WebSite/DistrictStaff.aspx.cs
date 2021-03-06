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


public partial class DistrictStaff : System.Web.UI.Page
{
    Staff selectStaff = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable pastDaTable = new DataTable();
            pastDaTable.Columns.Add(new DataColumn("PastDa"));

            string tempPastDa = DistrictObject.getDistrictData("dlPastDA");
            string[] items = tempPastDa.Split('|');

            for (int x= items.Length;x>0;x--)
            {
                DataRow aRow = pastDaTable.NewRow();
                aRow["PastDa"] = items[x-1];
                pastDaTable.Rows.Add(aRow);
            }

            rptPastDA.DataSource = pastDaTable;
            rptPastDA.DataBind();

            grdStaff.DataSource = Staff.allStaff;
            if (Page.User.Identity.IsAuthenticated)
            {
                grdStaff.Columns[0].Visible = true;
            }
            else
            {
                grdStaff.Columns[0].Visible = false;
            }
            grdStaff.DataBind();
            MultiView1.ActiveViewIndex = 0;
        }
        else if (MultiView1.ActiveViewIndex >0)
        {
            selectStaff = (Staff)Session["selectedStaff"];
        }
    }
    protected void grdStaff_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnPastDA.Enabled = false;
        btnStaff.Enabled = false;
        selectStaff = Staff.find(Convert.ToInt32(((Label)grdStaff.Rows[e.NewEditIndex].FindControl("lblId")).Text));
        Session["selectedStaff"] = selectStaff;

        txtPosition.Text = selectStaff.position;
        if (selectStaff.person != null)
        {
            txtCellPhone.Text = selectStaff.person.CellPhone;
            txtHomePhone.Text = selectStaff.person.HomePhone;
            txtEmailAddress.Text = selectStaff.person.EmailAddress;
            txtFirstName.Text = selectStaff.person.FirstName;
            txtLastName.Text = selectStaff.person.LastName;
        }
        else
        {
            txtCellPhone.Text = "";
            txtHomePhone.Text = "";
            txtEmailAddress.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
        }
        MultiView1.ActiveViewIndex = 1;
        e.Cancel = true;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        btnPastDA.Enabled = true;
        btnStaff.Enabled = true;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool okToGo = true;
        selectStaff.position = txtPosition.Text;
        if (txtFirstName.Text.Trim().Length > 0 && txtLastName.Text.Trim().Length > 0)
        {
            if (selectStaff.person == null)
                selectStaff.person = new Person();
            selectStaff.person.FirstName = txtFirstName.Text;
            selectStaff.person.LastName = txtLastName.Text;
            selectStaff.person.HomePhone = txtHomePhone.Text;
            selectStaff.person.CellPhone = txtCellPhone.Text;
            selectStaff.person.EmailAddress = txtEmailAddress.Text;
        }
        else if (selectStaff.person != null && txtFirstName.Text.Trim().Length == 0 && txtLastName.Text.Trim().Length == 0)
        {
            if (selectStaff.person.remove())
                selectStaff.person = null;
            else
                okToGo = false;
        }
        if (okToGo && selectStaff.save())
        {
            grdStaff.DataSource = Staff.allStaff;
            grdStaff.DataBind();
            MultiView1.ActiveViewIndex = 0;
            ((masterMain)Page.Master).setInformationMessage("Staff was successfully saved");
            btnPastDA.Enabled = true;
            btnStaff.Enabled = true;
        }
        else
        {
            ((masterMain)Page.Master).setErrorMessage("There was a problem saving the staff. " + DistrictObject.sqlMessages);
        }
    }

    protected void btnStaff_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        btnStaff.CssClass = "btn  btn-outline-dark active";
        btnPastDA.CssClass = "btn  btn-outline-dark";
    }

    protected void btnPastDA_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 2;
        btnStaff.CssClass = "btn  btn-outline-dark";
        btnPastDA.CssClass = "btn  btn-outline-dark active";
    }
}
