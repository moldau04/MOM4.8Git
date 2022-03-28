using BusinessEntity;
using BusinessLayer;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
//using System.DirectoryServices;

/// <summary>
/// Summary description for WebBaseUtility
/// </summary>
public static class WebBaseUtility
{
    public static string ConnectionString
    {
        get
        {
            return HttpContext.Current.Session["config"] != null ? Convert.ToString(HttpContext.Current.Session["config"]) : string.Empty;
        }
    }

    public static void UpdateMailConfigurationFromLoginUser(Mail mail)
    {
        if (mail == null)
            mail = new Mail();
        mail.RequireAutentication = false;
        General _objGeneral = new General();
        _objGeneral.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objGeneral.userid = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        BL_General _objBL_General = new BL_General();
        //DataSet dsEmailacc = _objBL_General.GetEmailAccounts(_objGeneral);
        DataSet dsEmailacc = _objBL_General.GetEmailAcc(_objGeneral);
        if (dsEmailacc.Tables[0].Rows.Count > 0)
        {
            mail.RequireAutentication = true;
            mail.Username = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();
            mail.Password = dsEmailacc.Tables[0].Rows[0]["OutPassword"].ToString();
            mail.SMTPHost = dsEmailacc.Tables[0].Rows[0]["OutServer"].ToString();
            mail.SMTPPort = Convert.ToInt32(dsEmailacc.Tables[0].Rows[0]["OutPort"].ToString());
            //mail.From = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();

            mail.InUsername = dsEmailacc.Tables[0].Rows[0]["InUsername"].ToString();
            mail.InPassword = dsEmailacc.Tables[0].Rows[0]["InPassword"].ToString();
            mail.InHost = dsEmailacc.Tables[0].Rows[0]["InServer"].ToString();
            mail.InPort = Convert.ToInt32(dsEmailacc.Tables[0].Rows[0]["InPort"].ToString());
            //mail.SSL = dsEmailacc.Tables[0].Rows[0]["SSL"].ToString() == "1" ? true : false;
            if (string.IsNullOrEmpty(dsEmailacc.Tables[0].Rows[0]["SSL"].ToString()))
            {
                mail.SSL = false;
            }
            else
            {
                mail.SSL = Convert.ToBoolean(dsEmailacc.Tables[0].Rows[0]["SSL"]);
            }
            if (string.IsNullOrEmpty(dsEmailacc.Tables[0].Rows[0]["TakeASentEmailCopy"].ToString()))
            {
                mail.TakeASentEmailCopy = false;
            }
            else
            {
                mail.TakeASentEmailCopy = Convert.ToBoolean(dsEmailacc.Tables[0].Rows[0]["TakeASentEmailCopy"]);
            }

            // Bcc email
            var bccStr = dsEmailacc.Tables[0].Rows[0]["BccEmail"].ToString();
            if (!string.IsNullOrEmpty(bccStr))
            {
                foreach (var bccaddress in bccStr.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (WebBaseUtility.IsValidEmailAddress1(bccaddress) && !mail.Bcc.Contains(bccaddress))
                    {
                        mail.Bcc.Add(bccaddress);
                    }
                }
            }
        }
        else
        {
            System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            mail.RequireAutentication = !mailSettings.Smtp.Network.DefaultCredentials;
            mail.Username = mailSettings.Smtp.Network.UserName;
            mail.Password = mailSettings.Smtp.Network.Password;
            mail.SMTPHost = mailSettings.Smtp.Network.Host;
            mail.SMTPPort = mailSettings.Smtp.Network.Port;
            //mail.From = mailSettings.Smtp.Network.UserName;
            mail.TakeASentEmailCopy = false;
        }
    }

    public static void UpdateMailConfiguration(Mail mail, string connConfig, int userId)
    {
        if (mail == null)
            mail = new Mail();
        mail.RequireAutentication = false;
        General _objGeneral = new General();
        _objGeneral.ConnConfig = connConfig;
        _objGeneral.userid = userId;
        BL_General _objBL_General = new BL_General();
        //DataSet dsEmailacc = _objBL_General.GetEmailAccounts(_objGeneral);
        DataSet dsEmailacc = _objBL_General.GetEmailAcc(_objGeneral);
        if (dsEmailacc.Tables[0].Rows.Count > 0)
        {
            mail.RequireAutentication = true;
            mail.Username = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();
            mail.Password = dsEmailacc.Tables[0].Rows[0]["OutPassword"].ToString();
            mail.SMTPHost = dsEmailacc.Tables[0].Rows[0]["OutServer"].ToString();
            mail.SMTPPort = Convert.ToInt32(dsEmailacc.Tables[0].Rows[0]["OutPort"].ToString());
            //if(string.IsNullOrEmpty(mail.From)) 
            //    mail.From = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();

            mail.InUsername = dsEmailacc.Tables[0].Rows[0]["InUsername"].ToString();
            mail.InPassword = dsEmailacc.Tables[0].Rows[0]["InPassword"].ToString();
            mail.InHost = dsEmailacc.Tables[0].Rows[0]["InServer"].ToString();
            mail.InPort = Convert.ToInt32(dsEmailacc.Tables[0].Rows[0]["InPort"].ToString());
            //mail.SSL = dsEmailacc.Tables[0].Rows[0]["SSL"].ToString() == "1" ? true : false;
            if (string.IsNullOrEmpty(dsEmailacc.Tables[0].Rows[0]["SSL"].ToString()))
            {
                mail.SSL = false;
            }
            else
            {
                mail.SSL = Convert.ToBoolean(dsEmailacc.Tables[0].Rows[0]["SSL"]);
            }
            if (string.IsNullOrEmpty(dsEmailacc.Tables[0].Rows[0]["TakeASentEmailCopy"].ToString()))
            {
                mail.TakeASentEmailCopy = false;
            }
            else
            {
                mail.TakeASentEmailCopy = Convert.ToBoolean(dsEmailacc.Tables[0].Rows[0]["TakeASentEmailCopy"]);
            }

        }
        else
        {
            System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            mail.RequireAutentication = !mailSettings.Smtp.Network.DefaultCredentials;
            mail.Username = mailSettings.Smtp.Network.UserName;
            mail.Password = mailSettings.Smtp.Network.Password;
            mail.SMTPHost = mailSettings.Smtp.Network.Host;
            mail.SMTPPort = mailSettings.Smtp.Network.Port;
            //mail.From = mailSettings.Smtp.Network.UserName;
            mail.TakeASentEmailCopy = false;
        }
    }

    //public static string GetFromEmailAddress()
    //{
    //    string strFrom = string.Empty;
    //    General _objGeneral = new General();
    //    _objGeneral.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //    _objGeneral.userid = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
    //    BL_General _objBL_General = new BL_General();
    //    DataSet dsEmailacc = _objBL_General.GetEmailAcc(_objGeneral);
    //    if (dsEmailacc.Tables[0].Rows.Count > 0)
    //    {
    //        strFrom = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();
    //    }
    //    else
    //    {
    //        System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
    //        MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
    //        string username = mailSettings.Smtp.Network.UserName;
    //        strFrom = username;
    //    }

    //    return strFrom;
    //}

    public static string GetFromEmailAddress()
    {
        string fromEmail = "";
        //General objPropUser = new General();
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        BL_User objBL_User = new BL_User();
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        //objPropUser.Username = HttpContext.Current.Session["username"].ToString();
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        try
        {
            //fromEmail = objBL_User.getUserEmail(objPropUser);
            fromEmail = objBL_User.getUserEmailByUserId(objPropUser);

            if (fromEmail == string.Empty)
            {
                System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
                string username = mailSettings.Smtp.Network.UserName;
                fromEmail = username;
                ////txtFrom.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = RemoveSpecCharsForJsScriptNotification(ex.Message);
        }
        return fromEmail;
    }

    public static string RemoveSpecCharsForJsScriptNotification(string strMessage)
    {
        if (!string.IsNullOrEmpty(strMessage))
            strMessage = strMessage.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r\n", "<br>").Replace("\n", "<br>");
        return strMessage;
    }

    public static void ShowEmailErrorMessageBox(System.Web.UI.Page page, Type pagetype, Exception ex)
    {
        string str = RemoveSpecCharsForJsScriptNotification(ex.Message);
        var notyType = "error";
        if (str.IndexOf("Get SentItems Error") == 0) notyType = "warning";
        System.Web.UI.ScriptManager.RegisterStartupScript(page, pagetype, "keyErr", "noty({text: '" + str + "',  type : '" + notyType + "', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    }

    public static string[] CreateICSAttachmentCalendar(string strSubject, string strDesc, string strLocation, DateTime schBeginDate, DateTime schEndDate, int minsReminder)
    {
        string[] contents = { "BEGIN:VCALENDAR",
            "PRODID:-//Flo Inc.//FloSoft//EN",
            "BEGIN:VEVENT",
            "SUMMARY:" + strSubject,
            "DTSTART:" + schBeginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
            "DTEND:" + schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
            "LOCATION:" + strLocation,
            "DESCRIPTION;ENCODING=ESCAPED-CHAR:" + strDesc,
            "STATUS:CONFIRMED",
            "SEQUENCE:3",
            "PRIORITY:3",
            "BEGIN:VALARM",
            string.Format("TRIGGER:-PT{0}M", minsReminder),
            "ACTION:DISPLAY",
            "DESCRIPTION:Reminder",
            "END:VALARM",
            "END:VEVENT",
            "END:VCALENDAR"
        };

        return contents;
        //System.IO.File.WriteAllLines(System.Web.HttpContext.Current.Server.MapPath("FileName.ics"), contents);
    }

    public static string CreateICSAttachmentCalendarStr(string strSubject, string strDesc, string strLocation, DateTime schBeginDate, DateTime schEndDate, int minsReminder)
    {
        StringBuilder contents = new StringBuilder();
        contents.Append("BEGIN:VCALENDAR").AppendLine();
        contents.Append("PRODID:-//Flo Inc.//FloSoft//EN").AppendLine();
        contents.Append("BEGIN:VEVENT").AppendLine();
        contents.Append("SUMMARY:" + strSubject).AppendLine();
        contents.Append("DTSTART:" + schBeginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")).AppendLine();
        contents.Append("DTEND:" + schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")).AppendLine();
        contents.Append("LOCATION:" + strLocation).AppendLine();
        //contents.Append("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + strDesc).AppendLine();
        contents.Append("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + strDesc).AppendLine();
        contents.Append("STATUS:CONFIRMED").AppendLine();
        contents.Append("SEQUENCE:3").AppendLine();
        contents.Append("PRIORITY:3").AppendLine();
        contents.Append("BEGIN:VALARM").AppendLine();
        contents.Append(string.Format("TRIGGER:-PT{0}M", minsReminder)).AppendLine();
        contents.Append("ACTION:DISPLAY").AppendLine();
        contents.Append("DESCRIPTION:Reminder").AppendLine();
        contents.Append("END:VALARM").AppendLine();
        contents.Append("END:VEVENT").AppendLine();
        contents.Append("END:VCALENDAR").AppendLine();

        return contents.ToString();
        //System.IO.File.WriteAllLines(System.Web.HttpContext.Current.Server.MapPath("FileName.ics"), contents);
    }

    public static DataTable GetContactListOnExchangeServer()
    {
        DataTable distributionList = new DataTable();
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if (HttpContext.Current.Session["DistributionList"] != null && HttpContext.Current.Session["DistributionList"].ToString() == "1")
        {
            var ds = objBL_User.GetUserExchangeContacts(objPropUser);
            if (ds.Tables.Count > 0)
                distributionList = ds.Tables[0];
        }
        else
        {
            string domainName = ConfigurationManager.AppSettings["ExchangeDomainName"].ToString();
            var ewsUrl = ConfigurationManager.AppSettings["ExchangeWebserviceURL"].ToString();
            if (!string.IsNullOrEmpty(ewsUrl))
            {
                try
                {
                    Mail mail = new Mail();
                    UpdateMailConfigurationFromLoginUser(mail);

                    var serverURI = new Uri(ewsUrl);
                    //new Uri("https://mail.teigroup.com/ews/exchange.asmx");
                    var exch = new ExchangeService();
                    exch.Url = serverURI;
                    exch.UseDefaultCredentials = false;

                    if (string.IsNullOrEmpty(domainName))
                    {
                        exch.Credentials = new System.Net.NetworkCredential(mail.Username, mail.Password);
                    }
                    else
                    {
                        exch.Credentials = new System.Net.NetworkCredential(mail.Username, mail.Password, domainName);
                    }

                    distributionList = GetContactsFromExContactFolder(exch);

                    var exGAL = GetGALFromExServer(exch);
                    // Merge two contacts list
                    distributionList.Merge(exGAL);
                    // Distinct datatable
                    distributionList = distributionList.DefaultView.ToTable(true);

                    objPropUser.ContactData = distributionList;
                    objBL_User.UpdateUserExchangeContacts(objPropUser);
                    HttpContext.Current.Session["DistributionList"] = "1";
                }
                catch (Exception ex)
                {
                    distributionList = new DataTable();
                }
            }
        }
        return distributionList;
    }

    private static DataTable GetContactsFromExContactFolder(ExchangeService exch)
    {
        DataTable emails = new DataTable();
        emails.Columns.Add("MemberName");
        emails.Columns.Add("MemberEmail");
        emails.Columns.Add("GroupName");
        emails.Columns.Add("Type");
        // Get the number of items in the contacts folder. To limit the size of the response, request only the TotalCount property.
        ContactsFolder contactsfolder = ContactsFolder.Bind(exch,
                                                            WellKnownFolderName.Contacts,
                                                            new PropertySet(BasePropertySet.IdOnly, FolderSchema.TotalCount));

        // Set the number of items to the number of items in the Contacts folder or 50, whichever is smaller.
        int numItems = contactsfolder.TotalCount < 50 ? contactsfolder.TotalCount : 50;

        // Instantiate the item view with the number of items to retrieve from the Contacts folder.
        ItemView view = new ItemView(numItems);

        // To keep the response smaller, request only the display name.
        //view.PropertySet = new PropertySet(BasePropertySet.IdOnly, ContactSchema.DisplayName);

        var folders = GetAllContactsFolders(exch);
        //var emailValidation = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        foreach (var folder in folders)
        {

            // Request the items in the Contacts folder that have the properties that you selected.
            FindItemsResults<Item> contactItems = exch.FindItems(folder.Id, view);
            Console.WriteLine("Loaded a folder named: {0}", folder.DisplayName);
            // Display the list of contacts. (Note that there can be a large number of contacts in the Contacts folder.)

            foreach (Item item in contactItems)
            {
                if (item is Contact)
                {
                    Contact contact = item as Contact;
                    try
                    {
                        DataRow _dr = emails.NewRow();
                        _dr["MemberName"] = contact.DisplayName;
                        _dr["MemberEmail"] = contact.EmailAddresses[EmailAddressKey.EmailAddress1];
                        _dr["GroupName"] = "";
                        _dr["Type"] = "Personal";
                        //if (emailValidation.IsValid(_dr["MemberEmail"]))
                        if (IsValidEmailAddress(_dr["MemberEmail"]))
                            emails.Rows.Add(_dr);
                    }
                    catch (Exception)
                    {
                    }

                }
                else
                {
                    ContactGroup cg = item as ContactGroup;
                    ExpandContactGroup(exch, cg, emails);
                }
            }
        }

        //distributionList = distributionList.DefaultView.ToTable(true);
        return emails;
    }

    private static DataTable GetGALFromExServer(ExchangeService exch)
    {
        DataTable emails = new DataTable();
        emails.Columns.Add("MemberName");
        emails.Columns.Add("MemberEmail");
        emails.Columns.Add("GroupName");
        emails.Columns.Add("Type");
        //var emailValidation = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        // A -> Z
        for (int c = 65; c <= 90; c++)
        {
            NameResolutionCollection nrCol = exch.ResolveName("SMTP:" + Convert.ToChar(c), ResolveNameSearchLocation.DirectoryOnly, true);
            foreach (NameResolution nr in nrCol)
            {
                if (nr.Mailbox.RoutingType == "SMTP")
                {
                    DataRow _dr = emails.NewRow();
                    _dr["MemberName"] = nr.Mailbox.Name;
                    _dr["MemberEmail"] = nr.Mailbox.Address;
                    _dr["GroupName"] = "";
                    _dr["Type"] = "Company-wide";
                    //if (emailValidation.IsValid(_dr["MemberEmail"]))
                    if (IsValidEmailAddress(_dr["MemberEmail"]))
                        emails.Rows.Add(_dr);
                }
            }
        }

        for (int n = 0; n <= 9; n++)
        {
            NameResolutionCollection nrCol = exch.ResolveName("SMTP:" + n.ToString(), ResolveNameSearchLocation.DirectoryOnly, true);
            foreach (NameResolution nr in nrCol)
            {
                if (nr.Mailbox.RoutingType == "SMTP")
                {
                    DataRow _dr = emails.NewRow();
                    _dr["MemberName"] = nr.Mailbox.Name;
                    _dr["MemberEmail"] = nr.Mailbox.Address;
                    _dr["GroupName"] = "";
                    _dr["Type"] = "Company-wide";
                    //if (emailValidation.IsValid(_dr["MemberEmail"]))
                    if (IsValidEmailAddress(_dr["MemberEmail"]))
                        emails.Rows.Add(_dr);
                }
            }
        }
        return emails;
    }

    private static Dictionary<String, String> ResolvedEmailAddressCache = new Dictionary<String, String>();
    private static String GetResolvedEmailAddress(string address, ExchangeService svc)
    {
        if (ResolvedEmailAddressCache.ContainsKey(address))
            return ResolvedEmailAddressCache[address];

        NameResolutionCollection nd = svc.ResolveName(address);
        foreach (NameResolution nm in nd)
        {
            if (nm.Mailbox.RoutingType == "SMTP")
            {
                ResolvedEmailAddressCache.Add(address, nm.Mailbox.Address);
                return nm.Mailbox.Address;
            }
        }

        ResolvedEmailAddressCache.Add(address, address);
        return address;
    }


    private static Collection<Folder> GetAllContactsFolders(ExchangeService service)
    {
        // Collection will contain all contact folders. 
        Collection<Folder> folders = new Collection<Folder>();

        // Get the root Contacts folder and load all properties. This results in a GetFolder operation call to EWS.
        ContactsFolder rootContactFolder = ContactsFolder.Bind(service, WellKnownFolderName.Contacts);
        folders.Add(rootContactFolder);
        //Console.WriteLine("Added the default Contacts folder to the collection of contact folders.");

        // Find all child folders of the root Contacts folder.
        int initialFolderSearchOffset = 0;
        const int folderSearchPageSize = 100;
        bool AreMoreFolders = true;
        FolderView folderView = new FolderView(folderSearchPageSize, initialFolderSearchOffset);
        folderView.Traversal = FolderTraversal.Deep;
        folderView.PropertySet = new PropertySet(BasePropertySet.IdOnly);

        while (AreMoreFolders)
        {
            try
            {
                // Find all the child folders of the default Contacts folder. This results in a FindFolder operation call to EWS.
                FindFoldersResults childrenOfContactsFolderResults = rootContactFolder.FindFolders(folderView);
                if (folderView.Offset == 0)
                {
                    Console.WriteLine("Found {0} child folders of the default Contacts folder.", childrenOfContactsFolderResults.TotalCount);
                }

                foreach (Folder f in childrenOfContactsFolderResults.Folders)
                {
                    ContactsFolder contactFolder = (ContactsFolder)f;
                    // Loads all the properties for the folder. This results in a GetFolder operation call to EWS.
                    contactFolder.Load();
                    // Add the folder to the collection of contact folders.
                    folders.Add(contactFolder);
                }

                // Turn off paged searches if there are no more folders to return.
                if (childrenOfContactsFolderResults.MoreAvailable == false)
                {
                    AreMoreFolders = false;
                }
                else // Increment the paging offset.
                {
                    folderView.Offset = folderView.Offset + folderSearchPageSize;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        return folders;
    }

    private static void ExpandContactGroup(ExchangeService service, ContactGroup contactGroup, DataTable distribulist)
    {
        // Return the expanded group.
        ExpandGroupResults myGroupMembers = service.ExpandGroup(contactGroup.Id);
        Console.WriteLine("Group members count: {0}", myGroupMembers.Count);
        //var emailValidation = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        // Display the group members.
        foreach (EmailAddress address in myGroupMembers.Members)
        {
            if (address.MailboxType == MailboxType.PublicGroup)
            {
                ExpandDistributionLists(service, address.Address, contactGroup, distribulist);
            }
            else
            {
                DataRow _dr = distribulist.NewRow();
                _dr["MemberName"] = address.Name;
                _dr["MemberEmail"] = address.Address;
                _dr["GroupName"] = contactGroup.DisplayName;
                _dr["Type"] = "Personal";
                //if (emailValidation.IsValid(_dr["MemberEmail"]))
                if (IsValidEmailAddress(_dr["MemberEmail"]))
                    distribulist.Rows.Add(_dr);
                //Console.WriteLine("Email Address: {0}", address);
            }
        }
    }

    private static void ExpandDistributionLists(ExchangeService service, string Mailbox, ContactGroup contactGroup, DataTable distribulist)
    {
        // Return the expanded group.
        ExpandGroupResults myGroupMembers = service.ExpandGroup(Mailbox);
        //var emailValidation = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        // Display the group members.
        foreach (EmailAddress address in myGroupMembers.Members)
        {
            // Check to see if the mailbox is a public group
            if (address.MailboxType == MailboxType.PublicGroup)
            {
                // Call the function again to expand the contained
                // distribution group.
                ExpandDistributionLists(service, address.Address, contactGroup, distribulist);
            }
            else
            {
                // Output the address of the mailbox.
                //Console.WriteLine("Email Address: {0}", address);
                DataRow _dr = distribulist.NewRow();
                _dr["MemberName"] = address.Name;
                _dr["MemberEmail"] = address.Address;
                _dr["GroupName"] = contactGroup.DisplayName;
                _dr["Type"] = "Personal";
                //if (emailValidation.IsValid(_dr["MemberEmail"]))
                if (IsValidEmailAddress(_dr["MemberEmail"]))
                    distribulist.Rows.Add(_dr);
            }
        }
    }

    public static bool IsValidEmailAddress(object value)
    {
        if (value == null)
        {
            return false;
        }

        string emailAddress = value as string;
        emailAddress = emailAddress.Trim();
        if (string.IsNullOrEmpty(emailAddress))
        {
            return false;
        }

        Regex _regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        //Regex _regex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}$", RegexOptions.IgnoreCase);
        return _regex.Match(emailAddress).Length > 0;
    }

    public static bool IsValidEmailAddress1(object value)
    {
        if (value == null)
        {
            return false;
        }

        string emailAddress = value as string;
        emailAddress = emailAddress.Trim();
        if (string.IsNullOrEmpty(emailAddress))
        {
            return false;
        }

        //Regex _regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        Regex _regex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}$", RegexOptions.IgnoreCase);
        return _regex.Match(emailAddress).Length > 0;
    }

    public static bool IsPasswordPassedPwPolicy(string strUserName, string strFirstName, string strLastName, string strPassword, ref string errorMessage)
    {
        bool isValidPassword = true;
        StringBuilder error = new StringBuilder();
        if (!string.IsNullOrEmpty(strPassword))
        {
            //Minimum character length should be 6 and maximum character length should be 10 for the password when privacy policy applied.
            if (strPassword.Length < 6 || strPassword.Length > 10)
            {
                isValidPassword = false;
                error.AppendLine("&#8226; Must contain minimum 6 characters upto maximum 10 characters");
                //error.AppendLine("At least six characters in length");
            }
            // TODO: need to confirm again about this policy

            //Should contain characters from three of the following four categories:
            //Must contain English uppercase characters(A through Z)
            //Must contain English lowercase characters(a through z)
            //Must contain Numerical digits(0 through 9)
            //Must contain a Non - alphabetic / Special Characters symbols(for example, !, $, #, %)
            //var arrayChar = strPassword.();
            var containUppercase = false;
            var containLowercase = false;
            var containNumerical = false;
            var containNonAlphabetic = false;
            var strUpperCase = "ABCDEFGHIJKLMNOPQRSTUVXYZW";
            var strLowerCase = strUpperCase.ToLower();
            var strNumberical = "123456789";
            foreach (var ch in strPassword)
            {
                if (strUpperCase.Contains(ch)) containUppercase = true;
                if (strLowerCase.Contains(ch)) containLowercase = true;
                if (strNumberical.Contains(ch)) containNumerical = true;
                if (!char.IsLetterOrDigit(ch)) containNonAlphabetic = true;
            }

            if (!containUppercase)
            {
                isValidPassword = false;
                error.AppendLine("&#8226; At Least 1 Uppercase letter");
            }

            if (!containLowercase)
            {
                isValidPassword = false;
                error.AppendLine("&#8226; At Least 1 Lowercase letter");
            }

            if (!containNonAlphabetic)
            {
                isValidPassword = false;
                error.AppendLine("&#8226; Must contain 1 special character");
            }

            if (!containNumerical)
            {
                isValidPassword = false;
                error.AppendLine("&#8226; Must contain 1 numeric character");
            }

            //Should not contain the user's account name or parts of the user's full name that exceed two consecutive characters
            var sub3UserName = strUserName.Length > 3 ? strUserName.Substring(0, 3).ToLower() : strUserName;
            var sub3FirstName = strFirstName.Length > 3 ? strFirstName.Substring(0, 3).ToLower() : strFirstName;
            var sub3LastName = strLastName.Length > 3 ? strLastName.Substring(0, 3).ToLower() : strLastName;
            if (strPassword.ToLower().Contains(sub3UserName)
                || strPassword.ToLower().Contains(sub3FirstName)
                || strPassword.ToLower().Contains(sub3LastName)
                )
            {
                isValidPassword = false;
                error.AppendLine("&#8226; Should not have user&#39;s first three letters of his/her First Name, Last Name and/or Username");
            }
        }
        else
        {
            isValidPassword = false;
            error.AppendLine("&#8226; Must contain minimum 6 characters upto maximum 10 characters");
            //error.AppendLine("At least six characters in length");
        }
        if (error.Length > 0)
        {
            errorMessage = error.ToString();
        }
        return isValidPassword;
    }

    public static void UpdatePageTitle(Page page, string pageName, string uid, string iscopy)
    {
        if (string.IsNullOrEmpty(uid))
        {
            page.Title = "Add " + pageName + " || MOM";
        }
        else
        {
            if (!string.IsNullOrEmpty(iscopy) && iscopy.ToLower() == "c")
            {
                page.Title = "Copy " + pageName + " || MOM";
            }
            else
            {
                page.Title = "Edit " + pageName + " || MOM";
            }
        }
    }

    public static string GetCompanySignature()
    {
        DataSet dsC = new DataSet();
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        dsC = objBL_User.getControl(objPropUser);
        string address = string.Empty;
        if (dsC.Tables[0].Rows.Count > 0)
        {

            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["name"])))
            {
                address += dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;// + "</br>";
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Address"])))
            {
                address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["city"])))
            {
                address += dsC.Tables[0].Rows[0]["city"].ToString() + ", ";
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["state"])))
            {
                address += dsC.Tables[0].Rows[0]["state"].ToString() + ", ";
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["zip"])))
            {
                address += dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;// + "</br>";
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Phone"])))
            {
                address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;// + "</br>";
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["fax"])))
            {
                address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;// + "</br>";
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["email"])))
            {
                if (!ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("QAE"))
                    address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;// + "<br/>";
            }
            //string mailBody = "Please review the attached Ticket Report.";
            //address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

            //txtBody.Text = address;
        }
        return address;
    }

    public static string GetSignature_BK()
    {
        DataSet dsC = new DataSet();
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //DataSet dsEmailacc = _objBL_General.GetEmailAccounts(_objGeneral);
        var signature = objBL_User.GetDefaultUserEmailSignature(objPropUser);

        if (string.IsNullOrEmpty(signature))
        {
            dsC = objBL_User.getControl(objPropUser);
            //string signature = string.Empty;
            if (dsC.Tables[0].Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["name"])))
                {
                    signature += dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;// + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Address"])))
                {
                    signature += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["city"])))
                {
                    signature += dsC.Tables[0].Rows[0]["city"].ToString() + ", ";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["state"])))
                {
                    signature += dsC.Tables[0].Rows[0]["state"].ToString() + ", ";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["zip"])))
                {
                    signature += dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;// + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Phone"])))
                {
                    signature += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;// + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["fax"])))
                {
                    signature += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;// + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["email"])))
                {
                    if (!ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("QAE"))
                        signature += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;// + "<br />";
                }
                //string mailBody = "Please review the attached Ticket Report.";
                //address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

                //txtBody.Text = address;
            }
        }
        return signature;
    }

    public static string GetSignature()
    {
        DataSet dsC = new DataSet();
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //DataSet dsEmailacc = _objBL_General.GetEmailAccounts(_objGeneral);
        var signature = objBL_User.GetDefaultUserEmailSignature(objPropUser);

        if (string.IsNullOrEmpty(signature))
        {
            //dsC = objBL_User.getControl(objPropUser);
            if (HttpContext.Current.Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                dsC = objBL_User.getControlBranch(objPropUser);
            }
            //string signature = string.Empty;
            if (dsC.Tables[0].Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["name"])))
                {
                    signature += dsC.Tables[0].Rows[0]["name"].ToString() +  "<br/>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Address"])))
                {
                    signature += dsC.Tables[0].Rows[0]["Address"].ToString() + "<br/>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["city"])))
                {
                    signature += dsC.Tables[0].Rows[0]["city"].ToString() + ", ";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["state"])))
                {
                    signature += dsC.Tables[0].Rows[0]["state"].ToString() + ", ";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["zip"])))
                {
                    signature += dsC.Tables[0].Rows[0]["zip"].ToString() + "<br/>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Phone"])))
                {
                    signature += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString()  + "<br/>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["fax"])))
                {
                    signature += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString()  + "<br/>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["email"])))
                {
                    signature += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + "<br/>";
                    //if (!ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("QAE"))
                    //    signature += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString()  + "<br />";
                }
            }
        }
        return signature;
    }

    //public static void SendEmailByEWS()
    //{

    //    string domainName = ConfigurationManager.AppSettings["ExchangeDomainName"].ToString();
    //    var ewsUrl = ConfigurationManager.AppSettings["ExchangeWebserviceURL"].ToString();
    //    if (!string.IsNullOrEmpty(ewsUrl))
    //    {
    //        try
    //        {
    //            Mail mail = new Mail();
    //            UpdateMailConfigurationFromLoginUser(mail);

    //            var serverURI = new Uri(ewsUrl);
    //            //new Uri("https://mail.teigroup.com/ews/exchange.asmx");
    //            var exch = new ExchangeService();
    //            exch.Url = serverURI;
    //            exch.UseDefaultCredentials = false;

    //            if (string.IsNullOrEmpty(domainName))
    //            {
    //                exch.Credentials = new System.Net.NetworkCredential(mail.Username, mail.Password);
    //            }
    //            else
    //            {
    //                exch.Credentials = new System.Net.NetworkCredential(mail.Username, mail.Password, domainName);
    //            }

    //            EmailMessage message = new EmailMessage(exch);

    //            // Set properties on the email message.
    //            message.Subject = "Test";
    //            message.Body = "Test sending email?";
    //            message.ToRecipients.Add("thomas@esssoft.com");
    //            message.Sender = mail.From;
    //            // Send the email message and save a copy.
    //            // This method call results in a CreateItem call to EWS.
    //            message.Send();
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //}

    //public static DataTable GetContactsForExchServer()
    //{
    //    DataTable distributionList = new DataTable();
    //    User objPropUser = new User();
    //    BL_User objBL_User = new BL_User();
    //    objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
    //    objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //    if (HttpContext.Current.Session["DistributionList"] != null && HttpContext.Current.Session["DistributionList"].ToString() == "1")
    //    {
    //        //distributionList = (DataTable)Session["DistributionList"];
    //        var ds = objBL_User.GetUserExchangeContacts(objPropUser);
    //        if (ds.Tables.Count > 0)
    //            distributionList = ds.Tables[0];
    //    }
    //    else
    //    {
    //        //DataTable distributionList1 = new DataTable();

    //        distributionList = WebBaseUtility.GetContactListOnExchangeServer("0", "");

    //        //distributionList.Merge(distributionList1);
    //        //Session["DistributionList"] = distributionList;
    //        HttpContext.Current.Session["DistributionList"] = "1";

    //        //User objPropUser = new User();
    //        //objBL_User = new BL_User();

    //        objPropUser.ContactData = distributionList;
    //        objBL_User.UpdateUserExchangeContacts(objPropUser);
    //    }

    //    return distributionList;
    //}
}

//public class ActiveDirectoryConnection
//{
//    public DirectoryEntry GetLdapDirectoryEntry(string path)
//    {
//        return GetDirectoryEntry(path, "LDAP");
//    }

//    public DirectoryEntry GetGCDirectoryEntry(string path)
//    {
//        return GetDirectoryEntry(path, "GC");
//    }

//    private DirectoryEntry GetDirectoryEntry(string path, string protocol)
//    {
//        var ldapPath = string.IsNullOrEmpty(path) ? string.Format("{0}:", protocol) : string.Format("{0}://{1}", protocol, path);
//        return new DirectoryEntry(ldapPath);
//    }
//}

//public class ExchangeAddressListService
//{
//    private readonly ActiveDirectoryConnection _Connection;

//    public ExchangeAddressListService(ActiveDirectoryConnection connection)
//    {
//        if (connection == null) throw new ArgumentNullException("connection");
//        _Connection = connection;
//    }

//    public IEnumerable<AddressList> GetGlobalAddressLists()
//    {
//        return GetAddressLists("CN=All Global Address Lists");
//    }

//    public IEnumerable<AddressList> GetAllAddressLists()
//    {
//        return GetAddressLists("CN=All Address Lists");
//    }
//    public IEnumerable<AddressList> GetSystemAddressLists()
//    {
//        return GetAddressLists("CN=All System Address Lists");
//    }

//    private IEnumerable<AddressList> GetAddressLists(string containerName)
//    {
//        string exchangeRootPath;
//        using (var root = _Connection.GetLdapDirectoryEntry("RootDSE"))
//        {
//            exchangeRootPath = string.Format("CN=Microsoft Exchange, CN=Services, {0}", root.Properties["configurationNamingContext"].Value);
//        }
//        string companyRoot;
//        using (var exchangeRoot = _Connection.GetLdapDirectoryEntry(exchangeRootPath))
//        using (var searcher = new DirectorySearcher(exchangeRoot, "(objectclass=msExchOrganizationContainer)"))
//        {
//            companyRoot = (string)searcher.FindOne().Properties["distinguishedName"][0];
//        }

//        var globalAddressListPath = string.Format(containerName + ",CN=Address Lists Container, {0}", companyRoot);
//        var addressListContainer = _Connection.GetLdapDirectoryEntry(globalAddressListPath);

//        using (var searcher = new DirectorySearcher(addressListContainer, "(objectClass=addressBookContainer)"))
//        {
//            searcher.SearchScope = SearchScope.OneLevel;
//            using (var searchResultCollection = searcher.FindAll())
//            {
//                foreach (SearchResult addressBook in searchResultCollection)
//                {
//                    yield return
//                        new AddressList((string)addressBook.Properties["distinguishedName"][0], _Connection);
//                }
//            }
//        }
//    }
//}

//public class AddressList
//{
//    private readonly ActiveDirectoryConnection _Connection;
//    private readonly string _Path;

//    private DirectoryEntry _DirectoryEntry;

//    internal AddressList(string path, ActiveDirectoryConnection connection)
//    {
//        _Path = path;
//        _Connection = connection;
//    }

//    private DirectoryEntry DirectoryEntry
//    {
//        get
//        {
//            if (_DirectoryEntry == null)
//            {
//                _DirectoryEntry = _Connection.GetLdapDirectoryEntry(_Path);
//            }
//            return _DirectoryEntry;
//        }
//    }

//    public string Name
//    {
//        get { return (string)DirectoryEntry.Properties["name"].Value; }
//    }

//    public IEnumerable<SearchResult> GetMembers(params string[] propertiesToLoad)
//    {
//        var rootDse = _Connection.GetGCDirectoryEntry(string.Empty);
//        var searchRoot = rootDse.Children.Cast<DirectoryEntry>().First();
//        using (var searcher = new DirectorySearcher(searchRoot, string.Format("(showInAddressBook={0})", _Path)))
//        {
//            if (propertiesToLoad != null)
//            {
//                searcher.PropertiesToLoad.AddRange(propertiesToLoad);
//            }
//            searcher.SearchScope = SearchScope.Subtree;
//            searcher.PageSize = 512;
//            do
//            {
//                using (var result = searcher.FindAll())
//                {
//                    foreach (SearchResult searchResult in result)
//                    {
//                        yield return searchResult;
//                    }
//                    if (result.Count < 512) break;
//                }
//            } while (true);
//        }
//    }
//}