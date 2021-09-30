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
using System.IO;
using DistrictObjects;

public partial class MeetingMinutes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                minutesAdmin.Visible = false;
            }
            else
            {
                ArrayList yearList = new ArrayList();
                for (int x = DateTime.Now.Year; x >= 2018 ; x--)
                {
                    yearList.Add(x.ToString());
                }
                string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames;
                dlMonth.DataSource = monthNames;
                dlMonth.SelectedValue = DateTime.Now.ToString("MMMM");
                dlMonth.DataBind();
                
                dlYear.DataSource = yearList;
                dlYear.SelectedValue = DateTime.Now.Year.ToString();
                dlYear.DataBind();
            }

            LoadGrid();
            ((masterMain)Page.Master).setInformationMessage("Click on the month to display the minutes");
        }
    }

    private void LoadGrid()
    {
        DataTable tMinutes = new DataTable();
        tMinutes.Columns.Add(new DataColumn("year"));
        tMinutes.Columns.Add(new DataColumn("months", tMinutes.GetType()));

        for (int x = DateTime.Now.Year; x >= 2018; x--)
        {
            DataRow newRow = tMinutes.NewRow();
            newRow["year"] = x.ToString();

            newRow["months"] = new DataTable("months");
            ((DataTable)newRow["months"]).Columns.Add(new DataColumn("month"));

            for (int y = 1; y < 13; y++)
            {
                DataRow newMinutesRow = ((DataTable)newRow["months"]).NewRow();
                DistrictMinutes aMinutes = DistrictMinutes.find(y.ToString(), newRow["year"].ToString());
                if (aMinutes != null)

                {
                    newMinutesRow["month"] = "<a href='" + System.Configuration.ConfigurationManager.AppSettings.Get("minutesPath") + "/" + aMinutes.fileName + "' target='_blank'>" + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(y) + "</a>";
                }
                else
                {
                    newMinutesRow["month"] = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(y);
                }
                ((DataTable)newRow["months"]).Rows.Add(newMinutesRow);
            }
            tMinutes.Rows.Add(newRow);
        }

        rptMinutesYear.DataSource = tMinutes;
        rptMinutesYear.DataBind();
    }

    private string fileName(string aPathName, string aFileName,int tries)
    {
        DirectoryInfo aDirectoryInfo = new DirectoryInfo(aPathName);

        if (tries > 0)
        {

            aFileName = aFileName.Substring(0,aFileName.LastIndexOf('.')) + "_" + tries.ToString() + aFileName.Substring(aFileName.LastIndexOf('.'));
        }
        FileInfo[] files = aDirectoryInfo.GetFiles(aFileName);

        string answer;
        if (files.Length > 0)
        {
            answer = fileName(aPathName, aFileName, ++tries);
        }
        else
        {
            answer = aFileName;
        }
        return answer;   
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        if (fleMinutes.FileName.Trim().Length > 0)
        {
            string filePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings.Get("minutesPath"));
            DistrictMinutes aMinutes = new DistrictMinutes();
            aMinutes.month = DateTime.ParseExact(dlMonth.SelectedValue, "MMMM", null).Month.ToString();
            aMinutes.year = dlYear.SelectedValue;
            aMinutes.fileName = fileName(filePath, fleMinutes.FileName, 0);

            if (aMinutes.Save())
            {
                ((masterMain)Page.Master).setInformationMessage("Minutes for " + dlMonth.SelectedValue + " " + dlYear.SelectedValue + " has been successfully saved.");
                LoadGrid();
            }
        }
        else
        {
            ((masterMain)Page.Master).setErrorMessage("Need a file to Upload first.");
        }
    }

    protected void fleMinutes_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        string filePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings.Get("minutesPath"));
        System.IO.FileInfo existingFile = new FileInfo(filePath + e.FileName);
        if (existingFile.Exists)
            existingFile.Delete();
        fleMinutes.SaveAs(filePath + e.FileName);
     }

    protected void fleMinutes_UploadedFileError(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        ((masterMain)Page.Master).setErrorMessage("Error in File Upload.");
    }
}
