using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;
using BusinessEntity;
using BusinessLayer;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class TimeCardService : System.Web.Services.WebService
{
    public TimeCardService()
    {
    }
    string defaultDate = "12/30/1899";
    TimeCard timeCard = new TimeCard();
    BL_TimeCard objTimeCard = new BL_TimeCard();


    [System.Web.Services.WebMethod(EnableSession = true)]
    public List<TimeCardProject> GetTimeCardJob(string prefixText, int isJob, int loc = 0, int jobId=0, string worker="")
    {
        DataSet ds = new DataSet();
        timeCard.DBName = HttpContext.Current.Session["dbname"].ToString();
        timeCard.ConnConfig = Session["config"].ToString();
        timeCard.SearchText = prefixText;
        timeCard.IsJob = isJob;
        timeCard.Loc = loc;
        timeCard.JobId = jobId;
        timeCard.Worker = worker;

        #region Company check
        timeCard.Userid = Convert.ToInt32(Session["UserID"].ToString());
        if (Session["CmpChkDefault"].ToString() == "1") timeCard.EN = 1;
        #endregion

        ds = objTimeCard.GetTimeCardJob(timeCard);

        DataTable dt = ds.Tables[0];

        List<TimeCardProject> lstTimeCardProject = new List<TimeCardProject>();
        if (isJob == 1)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTimeCardProject.Add(
                    new TimeCardProject
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["ID"]),
                        fDesc = Convert.ToString(dt.Rows[i]["fDesc"]),
                        Loc = Convert.ToInt32(dt.Rows[i]["Loc"]),
                        Tag = Convert.ToString(dt.Rows[i]["Tag"])
                    });
            }
        }
        if (isJob == 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTimeCardProject.Add(
                    new TimeCardProject
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["id"]),
                        fDesc = Convert.ToString(dt.Rows[i]["fDesc"]),
                        Selected= Convert.ToString(dt.Rows[i]["Selected"])
                    });
            }
        }
        if (isJob == 2)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTimeCardProject.Add(
                    new TimeCardProject
                    {
                        State = Convert.ToString(dt.Rows[i]["state"]),
                        Cat = Convert.ToString(dt.Rows[i]["cat"]),
                        Category = Convert.ToString(dt.Rows[i]["category"]),
                        Id = Convert.ToInt32(dt.Rows[i]["id"]),
                        Unit = Convert.ToString(dt.Rows[i]["unit"]),
                        Type = Convert.ToString(dt.Rows[i]["type"]),
                        Fdesc = Convert.ToString(dt.Rows[i]["fdesc"]),
                        Status = Convert.ToString(dt.Rows[i]["status"]),
                        Building = Convert.ToString(dt.Rows[i]["building"])
                    });
            }
        }

        if (isJob == 3)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTimeCardProject.Add(
                    new TimeCardProject
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["Phase"]),
                        fDesc = Convert.ToString(dt.Rows[i]["fDesc"]),
                        Code = Convert.ToString(dt.Rows[i]["Code"]),
                        BType = Convert.ToString(dt.Rows[i]["BType"]),
                        Tag = Convert.ToString(dt.Rows[i]["CodeDesc"]),
                        Category = Convert.ToString(dt.Rows[i]["GroupName"])
                    });
            }

         
        }

        if (isJob == 4)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTimeCardProject.Add(
                    new TimeCardProject
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["GroupID"]),
                        fDesc = Convert.ToString(dt.Rows[i]["GroupName"]),
                    });
            }
        }

        if (isJob == 5)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTimeCardProject.Add(
                    new TimeCardProject
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["Code"]),
                        fDesc = Convert.ToString(dt.Rows[i]["CodeDesc"]),
                    });
            }
        }

        return lstTimeCardProject;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public TimeInputCardViewModel GetInputCardService()
    {
        TimeInputCardViewModel objGetTimeInput = new TimeInputCardViewModel();

        timeCard.ConnConfig = Session["config"].ToString();

        DataSet ds = objTimeCard.GetInputCard(timeCard);

        List<SuperVisor> lstSup = new List<SuperVisor>();
        List<Worker> lstWor = new List<Worker>();
        List<Category> lstCat = new List<Category>();

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            lstSup.Add(
                new SuperVisor
                {
                    FDesc = Convert.ToString(ds.Tables[0].Rows[i]["Super"])
                });
        }

        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
        {
            lstWor.Add(
                new Worker
                {
                    ID = Convert.ToInt32(ds.Tables[1].Rows[i]["id"]),
                    FDesc = Convert.ToString(ds.Tables[1].Rows[i]["fDesc"]),
                    Super = Convert.ToString(ds.Tables[1].Rows[i]["Super"]),
                });
        }

        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
        {
            lstCat.Add(
                new Category
                {
                    Type = Convert.ToString(ds.Tables[2].Rows[i]["type"]),
                });
        }

        objGetTimeInput.lstSuperVisor = lstSup;
        objGetTimeInput.lstWorker = lstWor;
        objGetTimeInput.lstCategory = lstCat;

        return objGetTimeInput;

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public bool SaveInputCardService(string super, string worker, string category, string[] date, string[] time, string[] desc, string[] reg, string[] ot,
       string[] nt, string[] dt, string[] travel, string[] miles, string[] zone, string[] reimb,
        string[] project, string[] type, string[] wage, string[] group, string[] equipment, string[] wo,string[] cate,
        int markedReview,int timesheet)
    {
        DataTable dts = new DataTable();
        int res = 0;
        try
        {
            dts.Clear();
            dts.Columns.Add("ID");
            dts.Columns.Add("date");
            dts.Columns.Add("time");
            dts.Columns.Add("desc");
            dts.Columns.Add("reg");
            dts.Columns.Add("ot");
            dts.Columns.Add("nt");
            dts.Columns.Add("dt");
            dts.Columns.Add("travel");
            dts.Columns.Add("miles");
            dts.Columns.Add("zone");
            dts.Columns.Add("reimb");
            dts.Columns.Add("project");
            dts.Columns.Add("type");
            dts.Columns.Add("wage");
            dts.Columns.Add("group");
            dts.Columns.Add("equipment");
            dts.Columns.Add("wo");
            dts.Columns.Add("cate");

            for (int i = 0; i < date.Length; i++)
            {
                DataRow dr = dts.NewRow();
                dr["ID"] = i + 1;
                dr["date"] = Convert.ToDateTime(date[i]);
                dr["time"] = Convert.ToDateTime(date[i] + " " + time[i]);
                dr["desc"] = desc[i];
                dr["reg"] = Convert.ToDouble(reg[i]);
                dr["ot"] = Convert.ToDouble(ot[i]);
                dr["nt"] = Convert.ToDouble(nt[i]);
                dr["dt"] = Convert.ToDouble(dt[i]);
                dr["travel"] = Convert.ToDouble(travel[i]);
                dr["miles"] = Convert.ToDouble(miles[i]);
                dr["zone"] = Convert.ToDouble(zone[i]);
                dr["reimb"] = Convert.ToDouble(reimb[i]);
                dr["project"] = project[i];
                dr["type"] = type[i];
                dr["wage"] = wage[i];
                dr["group"] = group[i];
                dr["equipment"] = equipment[i];
                dr["wo"] = wo[i];
                dr["cate"] = cate[i];
                dts.Rows.Add(dr);
            }
            string username = Convert.ToString(Session["username"]);
            timeCard.ConnConfig = Session["config"].ToString();
            res = objTimeCard.SaveInputCard(timeCard, super, worker, category, markedReview, username, dts, timesheet);

        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }


    [WebMethod(EnableSession = true)]
    public List<TimeCardProject> GetInvoiceByJobID(string prefixText)
    {
        string Conn = Session["config"].ToString();
        DataSet ds = objTimeCard.GetInvoiceByJobID(prefixText, Conn);

        DataTable dt = ds.Tables[0];

        List<TimeCardProject> lstProject = new List<TimeCardProject>();
        for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstProject.Add(
                    new TimeCardProject
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["ID"]),
                        fDesc = Convert.ToString(dt.Rows[i]["fDesc"]),
                        Loc = Convert.ToInt32(dt.Rows[i]["Loc"]),
                        Tag = Convert.ToString(dt.Rows[i]["Tag"]),
                        Name= Convert.ToString(dt.Rows[i]["Name"]),
                        Address = Convert.ToString(dt.Rows[i]["Address"]),
                        IsContract = Convert.ToString(dt.Rows[i]["IsContract"]) 
                    });
            }
             return lstProject;
        }
}

 