using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DistrictObjects;


public partial class iFrameMaster : System.Web.UI.MasterPage
{
//    private readonly Control tmUpdate_Tick;

    protected void Page_Load(object sender, EventArgs e)
    {
        copyRiteYear.Text = DateTime.Now.Year.ToString("####");
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
    }
}
