using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QBFC12Lib;

public partial class qbtest : System.Web.UI.Page
{
    private bool booSessionBegun;
    protected void Page_Load(object sender, EventArgs e)
    {
        //QBSYNC();
    }

    private void QBSYNC()
    {
        QBSessionManager sessionManager = null;
        booSessionBegun = false;

        try
        {
            #region Connection to QB

            string path = "F:\\KUNAL\\QuickBooks Softwares\\Company Files\\Elevator Refurbishing Corp.QBW";

            IMsgSetRequest requestMsgSet;
            IMsgSetResponse responseMsgSet;
            sessionManager = new QBSessionManager();
            sessionManager.CommunicateOutOfProcess(true);
            sessionManager.QBAuthPreferences.PutIsReadOnly(false);
            sessionManager.QBAuthPreferences.PutUnattendedModePref(ENUnattendedModePrefType.umptRequired);
            sessionManager.QBAuthPreferences.PutPersonalDataPref(ENPersonalDataPrefType.pdptRequired);
            sessionManager.OpenConnection2("", "MSMSDK", ENConnectionType.ctLocalQBD);
            sessionManager.BeginSession(path, ENOpenMode.omDontCare);

            if (sessionManager.QBAuthPreferences.WasAuthPreferencesObeyed() != true)
            {
                throw new Exception("Auth Not Obeyed!!");
            }
            booSessionBegun = true;
            requestMsgSet = getLatestMsgSetRequest(sessionManager);
            requestMsgSet.Attributes.OnError = ENRqOnError.roeStop;
            #endregion

            #region Sync Customer type

            requestMsgSet.ClearRequests();
            IItemServiceQuery Custtype = requestMsgSet.AppendItemServiceQueryRq();
            //Custtype.ORListQuery.ListFilter.ORNameFilter.NameFilter.Name.SetValue("Mobile Service Manager");

            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            IResponse responses = responseMsgSet.ResponseList.GetAt(0);
            IItemServiceRetList invoiceRets = responses.Detail as IItemServiceRetList;

            if (invoiceRets != null)
            {
                if (!(invoiceRets.Count == 0))
                {
                    int rowcount = invoiceRets.Count;
                    int fCount = 0;
                    for (int ndx = 0; ndx < rowcount; ndx++)
                    {
                        IItemServiceRet invoiceRet1 = invoiceRets.GetAt(ndx);
                        if (invoiceRet1.Name.GetValue().ToString() == "Mileage")
                        {
                            Response.Write(invoiceRet1.ListID.GetValue().ToString());
                        }
                    }
                }
            }

            #endregion

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "alert('" + str + "')", true);
        }
        finally
        {
            #region Close QB Connection

            if (booSessionBegun)
            {
                sessionManager.EndSession();
            }
            booSessionBegun = false;
            sessionManager.CloseConnection();

            #endregion
        }
    }

    public IMsgSetRequest getLatestMsgSetRequest(QBSessionManager sessionManager)
    {
        // Find and adapt to supported version of QuickBooks
        double supportedVersion = QBFCLatestVersion(sessionManager);

        short qbXMLMajorVer = 0;
        short qbXMLMinorVer = 0;

        if (supportedVersion >= 6.0)
        {
            qbXMLMajorVer = 6;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 5.0)
        {
            qbXMLMajorVer = 5;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 4.0)
        {
            qbXMLMajorVer = 4;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 3.0)
        {
            qbXMLMajorVer = 3;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 2.0)
        {
            qbXMLMajorVer = 2;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 1.1)
        {
            qbXMLMajorVer = 1;
            qbXMLMinorVer = 1;
        }
        else
        {
            qbXMLMajorVer = 1;
            qbXMLMinorVer = 0;
            //Response.Write("It seems that you are running QuickBooks 2002 Release 1. We strongly recommend that you use QuickBooks' online update feature to obtain the latest fixes and enhancements");
        }

        // Create the message set request object
        IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", qbXMLMajorVer, qbXMLMinorVer);
        return requestMsgSet;
    }

    private double QBFCLatestVersion(QBSessionManager SessionManager)
    {
        // Use oldest version to ensure that this application work with any QuickBooks (US)
        IMsgSetRequest msgset = SessionManager.CreateMsgSetRequest("US", 1, 0);
        msgset.AppendHostQueryRq();
        IMsgSetResponse QueryResponse = SessionManager.DoRequests(msgset);


        IResponse response = QueryResponse.ResponseList.GetAt(0);

        // Please refer to QBFC Developers Guide for details on why 
        // "as" clause was used to link this derrived class to its base class
        IHostRet HostResponse = response.Detail as IHostRet;
        IBSTRList supportedVersions = HostResponse.SupportedQBXMLVersionList as IBSTRList;

        int i;
        double vers;
        double LastVers = 0;
        string svers = null;

        for (i = 0; i <= supportedVersions.Count - 1; i++)
        {
            svers = supportedVersions.GetAt(i);
            vers = Convert.ToDouble(svers);
            if (vers > LastVers)
            {
                LastVers = vers;
            }
        }
        return LastVers;
    }
    
}
