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

public partial class ScheduleImport : System.Web.UI.Page
{
    ArrayList importList = new ArrayList();
    ArrayList importedGames = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
        }
        else
        {
            importList = (ArrayList)Session["importList"];
            importedGames = (ArrayList)Session["importedGames"];
        }
    }
    protected void btnStep1Next_Click(object sender, EventArgs e)
    {
        importList = new ArrayList();
        GetFile(importList);

        Session.Add("importList", importList);

        int x = 0;

        System.Text.StringBuilder aBuilder = new System.Text.StringBuilder();

        aBuilder.Append("<tr style=\"font-weight:bold\">");

        int numberToShow = 6;

        if (ckHeader.Checked)
            numberToShow = 7;

        int attributeSize = ((string[])importList[0]).Length;

        poolExtender.Maximum = attributeSize;
        visitorExtender.Maximum = attributeSize;
        homeExtender.Maximum = attributeSize;
        gameDateExtender.Maximum = attributeSize;
        gameTimeExtender.Maximum = attributeSize;
        locationExtender.Maximum = attributeSize;


        for (int t = 1; t <= attributeSize; t++)
        {
            aBuilder.Append("<td>");
            aBuilder.Append(t.ToString("##"));
            aBuilder.Append("</td>");
        }

        aBuilder.Append("</tr>");

        foreach (string[] aStringArray in importList)
        {
            if (x < numberToShow)
            {
                aBuilder.Append("<tr>");
                foreach (string aString in aStringArray)
                {
                    aBuilder.Append("<td>");
                    aBuilder.Append(aString);
                    aBuilder.Append("</td>");
                }
                aBuilder.Append("</tr>");
                x++;
            }
            else
            {
                break;
            }
        }

        lblImportData.Text = aBuilder.ToString();

        MultiView1.ActiveViewIndex ++;
    }
    protected void btnStep2Next_Click(object sender, EventArgs e)
    {
        importedGames = new ArrayList();
        for (int x=0;x<importList.Count;x++)
        {
            string[] aStringArray = (string[])importList[x];
            if (ckHeader.Checked && x > 0)
            {
                try
                {
                    Game aGame = null;
                    if (rbCategory.SelectedIndex == 0)
                    {
                        aGame = new PoolGame();
                        aGame.team1 = DistrictTeam.find(Convert.ToInt32(aStringArray[Convert.ToInt32(txtVisitorNumber.Text) - 1]));
                        aGame.team2 = DistrictTeam.find(Convert.ToInt32(aStringArray[Convert.ToInt32(txtHomeNumber.Text) - 1]));
                    }
                    else
                    {
                        aGame = new InterleagueGame();
                        aGame.team1 = InterleagueTeam.find(Convert.ToInt32(aStringArray[Convert.ToInt32(txtVisitorNumber.Text) - 1]));
                        aGame.team2 = InterleagueTeam.find(Convert.ToInt32(aStringArray[Convert.ToInt32(txtHomeNumber.Text) - 1]));
                    }

                    if (txtGameTimeNumber.Text == "0")
                        aGame.gameDate = Convert.ToDateTime(aStringArray[Convert.ToInt32(txtGameDateNumber.Text) - 1]);
                    else
                    {
                       aGame.gameDate = Convert.ToDateTime(aStringArray[Convert.ToInt32(txtGameDateNumber.Text) - 1] + " " + aStringArray[Convert.ToInt32(txtGameTimeNumber.Text) - 1]);
                    }
                    if (txtLocationNumber.Text == "0")
                        aGame.location = "";
                    else
                        aGame.location = aStringArray[Convert.ToInt32(txtLocationNumber.Text) - 1];
                    if (aGame.isPool())
                        if (txtPoolNumber.Text == "0")
                            ((PoolGame)aGame).pool = ((DistrictTeam)aGame.team2).pool;
                        else
                            ((PoolGame)aGame).pool = aStringArray[Convert.ToInt32(txtPoolNumber.Text) - 1];
                    importedGames.Add(aGame);
                }
                catch (Exception)
                {
                }
            }
        }

        Session.Add("importedGames", importedGames);
        grdImportedGames.DataSource = importedGames;
        grdImportedGames.DataBind();

        MultiView1.ActiveViewIndex ++;
    }

    private void GetFile(ArrayList importList)
    {
        if (gameUpload.HasFile)
        {
            System.IO.StreamReader aReader = new System.IO.StreamReader(gameUpload.FileContent);
            while (!aReader.EndOfStream)
            {
                string aString = aReader.ReadLine();
                importList.Add(aString.Split(rbDelimiter.SelectedValue[0]));
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ckPurge.Checked)
        {
            foreach (DistrictObjects.Game aGame in DistrictObjects.Game.AllPoolGames)
            {
                aGame.delete();
            }
        }
        int gamesLoaded = 0;
        int gamesField = 0;
        foreach (Game aGame in importedGames)
        {
            if (aGame.save())
                gamesLoaded++;
            else
                gamesField++;
        }

        ((masterMain)Page.Master).setInformationMessage(gamesLoaded.ToString("#,###") + " were successfully imported and " + gamesField.ToString("#,###") + " failed.");

        Response.Redirect("Schedule.aspx");
    }
    protected void btnBack3_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex--;
    }

    protected void gameUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {

    }
}
