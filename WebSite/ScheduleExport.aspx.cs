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
using CarlosAg.ExcelXmlWriter;

public partial class ScheduleExport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataView gamesView = (DataView)Session["gamesView"];

            createScheduleWorksheetFor(gamesView);
        }
    }
    private void createScheduleWorksheetFor(DataView gamesView)
    {
        Workbook workbook = new Workbook();
        workbook.Properties.Author = "John Bucciarelli";
        workbook.Properties.Title = "Schedule Export";
        workbook.Properties.Created = DateTime.Now;
        WorksheetStyle style = workbook.Styles.Add("HeaderStyle");
        style.Font.FontName = "Arial";
        style.Font.Size = 12;
        style.Font.Bold = true;
        style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        style.Font.Color = "White";
        style.Interior.Color = "Black";
        style.Interior.Pattern = StyleInteriorPattern.ThickDiagCross;

        Worksheet aWorkSheet = workbook.Worksheets.Add("Tournament Schedule");

        /*
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
        */

        aWorkSheet.Table.Columns.Add(new WorksheetColumn(100));  // Game Date
        aWorkSheet.Table.Columns.Add(new WorksheetColumn(200));  // Division
        aWorkSheet.Table.Columns.Add(new WorksheetColumn(200));  // Visitor
        aWorkSheet.Table.Columns.Add(new WorksheetColumn(200));  // Home
        aWorkSheet.Table.Columns.Add(new WorksheetColumn(300));  // Location
        aWorkSheet.Table.Columns.Add(new WorksheetColumn(300));  // Timestamp

        WorksheetRow row = aWorkSheet.Table.Rows.Add();
        row.Cells.Add("Game Date", DataType.String, "HeaderStyle");
        row.Cells.Add("Division", DataType.String, "HeaderStyle");
        row.Cells.Add("Visitor", DataType.String, "HeaderStyle");
        row.Cells.Add("Home", DataType.String, "HeaderStyle");
        row.Cells.Add("Location", DataType.String, "HeaderStyle");
        row.Cells.Add("Last Update", DataType.String, "HeaderStyle");

        foreach (DataRowView aRow in gamesView)
        {
            WorksheetRow row2 = aWorkSheet.Table.Rows.Add();
            row2.Cells.Add((string)aRow["GameDate"]);
            row2.Cells.Add((string)aRow["Division"]);
            row2.Cells.Add((string)aRow["Visitor"]);
            row2.Cells.Add((string)aRow["Home"]);
            row2.Cells.Add((string)aRow["Location"]);
            row2.Cells.Add(((DateTime)aRow["updateTimestamp"]).ToString("MM/dd/yyyy hh:mm:ss tt"));
        }
        base.Response.ContentType = "application/vnd.ms-excel";
        workbook.Save(base.Response.OutputStream);
    }

}
