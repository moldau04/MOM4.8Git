using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

 
    public partial class SendAppointment : System.Web.UI.Page
    {
    protected void Page_Load(object sender, EventArgs e)
    {

        List<string> LocationList = new List<string>();


        if (!Page.IsPostBack)
        {
            if (Session["SafetyTestAppointLocationList"] == null)
            {
                Response.Redirect("SafetyTest?fil=1");
            }

            LocationList = (List<string>)Session["SafetyTestAppointLocationList"]  ;

            Session["SafetyTestAppointLocationList"] = null;

            ViewState["LocationList"] = LocationList;

            if (LocationList.Count > 0)
            {
                string _loc = "";

                foreach (var item in LocationList)
                {
                    _loc = item;     break;

                }

                SendApoinment(_loc);

            }
        }
    }


    protected void LinkButton1_Click1(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtEmailTo.Text))
        {
            Mail mail = new Mail();
            try
            {

                mail.To = txtEmailTo.Text.Split(';', ',').OfType<string>().ToList();

                mail.From = txtEmailFrom.Text;

                mail.Title = txtSubject.Text;

                mail.Text = txtBody.Text;

                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

                var apStart = Convert.ToDateTime(txtStart.Text);

                var apEnd = Convert.ToDateTime(TextEnd.Text);

                StringBuilder sb = (StringBuilder) ViewState["stringBuilderToString"];

                StringBuilder apBody = new StringBuilder();
               
                apBody.AppendFormat(sb.ToString());

                var icsAttachmentContentsStr = WebBaseUtility.CreateICSAttachmentCalendarStr(txtSubject.Text
                    , apBody.ToString()
                    , txtLocation.Text
                    , apStart
                    , apEnd
                    , 60
                    );
                var myByteArray = System.Text.Encoding.UTF8.GetBytes(icsAttachmentContentsStr);
                mail.attachmentBytes = myByteArray;
                mail.FileName = txtLocation.Text + "-SafetyTest-Appointment.ics";
                mail.Send();


                string str1 = "Safety Test added to the calendar successfully for " + txtSubject.Text + " .";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str1 + "',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);

            }

            catch (Exception ex)
            {

                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            }
        }
         

        List<string> LocationList = new List<string>();

        LocationList = (List<string>)ViewState["LocationList"];

        if (LocationList.Count > 0)
        {
            LocationList.RemoveAt(0);
        }

        if (LocationList.Count > 0)
        {
            string _loc = "";

            foreach (var item in LocationList)
            {
                _loc = item;

                break;

            }
            ViewState["LocationList"] = LocationList;

            SendApoinment(_loc.ToString());
        }
        else {

            Response.Redirect("SafetyTest?fil=1");
        }
       
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {

        Response.Redirect("SafetyTest?fil=1");
    }

  
    public void SendApoinment(string Loc)
    {

        #region NK: Send email with a appointment to login user 

        // Create


        DataSet ds = GetTestDetails(Loc);

        string TestName = "";

        string StartDate = "";

        string EndDate = "";

        string LocationName = "";

        DataTable dt = new DataTable();

        if (ds.Tables[0].Rows.Count > 0)
        {

            TestName = ds.Tables[0].Rows[0]["TestName"].ToString();

            LocationName = ds.Tables[0].Rows[0]["LocationName"].ToString();

            dt = ds.Tables[0];

        }

        if (ds.Tables[1].Rows.Count == 1)
        {
            StartDate = ds.Tables[1].Rows[0]["StartDate"].ToString();

            EndDate = ds.Tables[1].Rows[0]["EndDate"].ToString();
        }

        if (StartDate == EndDate)
        {
            StartDate = ds.Tables[1].Rows[0]["StartDate"].ToString();

            DateTime _EndDate = Convert.ToDateTime(EndDate);

            EndDate = _EndDate.AddHours(23).ToString();
        }

        string MailTitle = TestName + " - " + LocationName;


        var mailAddress = WebBaseUtility.GetFromEmailAddress();




        if (!string.IsNullOrEmpty(mailAddress))
        {

            try
            {



                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append("<br/><b>Subject: </b> " + MailTitle + " <br/>");

                string ddd = Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy");

                stringBuilder.Append("<br/><b>When: </b> " + ddd);


                if (StartDate != EndDate)
                {
                    string ttt = Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy");

                    stringBuilder.Append(" - " + ttt);
                }

                stringBuilder.Append("<br/>");

                stringBuilder.Append("<br/><b>Where: </b> " + LocationName + " <br/>");

                List<string> UNITList = new List<string>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string UnitNEW = dt.Rows[i]["Unit"].ToString();
                    string TestNameNEW = dt.Rows[i]["TestName"].ToString();
                    string ScheduledDateNew = Convert.ToDateTime(dt.Rows[i]["ScheduledDate"].ToString()).ToString("MM/dd/yyyy");
                    string WorkerNew = dt.Rows[i]["Worker"].ToString();

                    List<string> ScheduledDateList = new List<string>();

                    ScheduledDateList.Add(ScheduledDateNew);

                    List<string> WorkerList = new List<string>();

                    WorkerList.Add(WorkerNew);

                    for (int j = i + 1; j < dt.Rows.Count; j++)
                    {
                        if (UnitNEW == dt.Rows[j]["Unit"].ToString() && TestNameNEW == dt.Rows[j]["TestName"].ToString())
                        {
                            var v = Convert.ToDateTime(dt.Rows[j]["ScheduledDate"].ToString()).ToString("MM/dd/yyyy")  ;

                            if (!ScheduledDateList.Contains(v))
                            {
                                var z = Convert.ToDateTime(dt.Rows[j]["ScheduledDate"].ToString()).ToString("MM/dd/yyyy")  ;
                                ScheduledDateList.Add(z);
                            }

                            if (!WorkerList.Contains(dt.Rows[j]["Worker"].ToString()))
                            {
                                WorkerList.Add(dt.Rows[j]["Worker"].ToString());
                            }
                        }
                    }


                    if (!UNITList.Contains(UnitNEW + TestNameNEW))
                    {
                        string ScheduledDateNew1 = "";
                        string WorkerNew1 = "";

                        foreach (string item in ScheduledDateList)
                        {
                            if (ScheduledDateNew1 != "") { ScheduledDateNew1 += ","; }
                            ScheduledDateNew1 += item;
                        }

                        foreach (string item in WorkerList)
                        {
                            if (WorkerNew1 != "") { WorkerNew1 += ","; }
                            WorkerNew1 += item;
                        }


                        stringBuilder.Append("<br/> <b>Unit:- </b>  " + UnitNEW + "<b> Test:- </b> " + TestNameNEW + " <b> ScheduledDate: </b>- " + ScheduledDateNew1 + " - " + "<b> Worker:- </b> " + WorkerNew1 + " <br/>");

                        UNITList.Add(UnitNEW + TestNameNEW);
                    } 
                }
                 
                var apSubject = string.Format("Test name:- " + MailTitle); 

                var apStart = Convert.ToDateTime(StartDate);

                var apEnd = Convert.ToDateTime(EndDate);



                txtEmailFrom.Text = WebBaseUtility.GetFromEmailAddress();
                txtEmailTo.Text = WebBaseUtility.GetFromEmailAddress();
                txtSubject.Text = MailTitle;
                txtStart.Text = apStart.ToString("MM/dd/yyyy") + " 12:00 AM";
                TextEnd.Text = apEnd.ToString("MM/dd/yyyy") + " 11:00 PM";
                txtLocation.Text = LocationName.ToString();
                txtBody.Text = stringBuilder.ToString();

                ViewState["stringBuilderToString"] = stringBuilder;

                //string script = "function f(){$find(\"" + RadWindowAppointment.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);  ";
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key11245", script, true);
            }

            catch (Exception ex)
            {

                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        else
        {


            string str = "Please Configure Mail settings ";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr44444", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }

        #endregion
    }

    public DataSet GetTestDetails(string Loc)
    {
        try
        {


            StringBuilder stringQuery = new StringBuilder();

            stringQuery.Append("select");
            stringQuery.Append(" LoadTest.Name as TestName  , L.Tag  LocationName");
            stringQuery.Append(" , Elev.Unit");
            stringQuery.Append(" , Cast( sc.ScheduledDate as date) ScheduledDate ");
            stringQuery.Append(" , sc.Worker   from loc l");
            stringQuery.Append(" inner join LoadTestItem LTI on LTI.Loc = l.Loc");
            stringQuery.Append(" inner join Elev   on Elev.ID = LTI.Elev");
            stringQuery.Append(" inner join LoadTest   on LoadTest.ID = LTI.ID");
            stringQuery.Append(" inner join LoadTestItemSchedule sc on sc.LID = LTI.LID");
            stringQuery.Append(" where isnull(sc.ScheduledDate, '') <> ''  and isnull(sc.Worker, '') <> ''  and l.Tag =   '" + Loc + "' ");


            stringQuery.Append(" select");
            stringQuery.Append(" min( Cast( sc.ScheduledDate as date)) StartDate , ");
            stringQuery.Append(" max( Cast( sc.ScheduledDate as date)) EndDate ");
            stringQuery.Append(" from loc l");
            stringQuery.Append(" inner join LoadTestItem LTI on LTI.Loc = l.Loc");
            stringQuery.Append(" inner join Elev   on Elev.ID = LTI.Elev");
            stringQuery.Append(" inner join LoadTest   on LoadTest.ID = LTI.ID");
            stringQuery.Append(" inner join LoadTestItemSchedule sc on sc.LID = LTI.LID");
            stringQuery.Append(" where isnull(sc.ScheduledDate, '') <> ''  and isnull(sc.Worker, '') <> ''  and l.Tag =   '" + Loc + "' ");


            return SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, stringQuery.ToString());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
 