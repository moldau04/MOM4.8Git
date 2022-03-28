using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessEntity.Recurring
{
    public class SafetyTest
    {
        #region::Private Declaration::
        private int _lid;
        private int _typeid;
        private int _locid;
        private int _equipid;
        private DateTime? _last;
        private DateTime? _next;
        private int _status;
        private int? _ticket;
        private string _remarks;
        private DateTime? _lastdate;
        private int _oldStatus;
        private int _oldtypeid;
        private string _startdate;
        private string _enddate;
        private int? _jobid;
        private int? _workerid;
        private string _worker;

        private int _testDueBy;
        private int _charge;
        private int _thirdParty;
        private double _amount;
        private double _overrideAmount;

        private string _thirdPartyName;
        private string _thirdPartyPhone;
        private string _custom1;
        private string _custom2;
        private string _custom3;
        private string _custom4;

       
        #endregion

        #region::Public Declaration::
        public DataTable Cus_TestItemValue;
        public List<NotificationCustomChange> Cus_EmailToTeamMember;
        public List<NotificationCustomChange> Cus_CreateTask;
        public string ConnConfig;
        public string UserName;
        public string Statusstr;
        public int FlagEN;
        public int UserID { get; set; }
        public int LID
        {
            get
            {
                return _lid;
            }
            set
            {
                _lid = value;
            }
        }
        public int Typeid
        {
            get
            {
                return _typeid;
            }
            set
            {
                _typeid = value;
            }
        }
        public int Locid
        {
            get
            {
                return _locid;
            }
            set
            {
                _locid = value;
            }
        }
        public int Equipid
        {
            get
            {
                return _equipid;
            }
            set
            {
                _equipid = value;
            }
        }
        public DateTime? Last
        {
            get
            {
                return _last;
            }
            set
            {
                _last = value;
            }
        }
        public DateTime? Next
        {
            get
            {
                return _next;
            }
            set
            {
                _next = value;
            }
        }
        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public int? Ticket
        {
            get
            {
                return _ticket;
            }
            set
            {
                _ticket = value;
            }
        }
        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                _remarks = value;
            }
        }

        public DateTime? Lastdate
        {
            get
            {
                return _lastdate;
            }
            set
            {
                _lastdate = value;
            }
        }

        public int OldTypeid
        {
            get
            {
                return _oldtypeid;
            }
            set
            {
                _oldtypeid = value;
            }
        }

        public int OldStatus
        {
            get
            {
                return _oldStatus;
            }
            set
            {
                _oldStatus = value;
            }
        }

        public string Startdate
        {
            get
            {
                return _startdate;
            }
            set
            {
                _startdate = value;
            }
        }
        public string Enddate
        {
            get
            {
                return _enddate;
            }
            set
            {
                _enddate = value;
            }
        }

        public int? Job
        {
            get
            {
                return _jobid;
            }
            set
            {
                _jobid = value;
            }
        }

        public int? Workerid
        {
            get
            {
                return _workerid;
            }
            set
            {
                _workerid = value;
            }
        }

        public string Worker
        {
            get
            {
                return _worker;
            }
            set
            {
                _worker = value;
            }
        }
       

        public bool CreateTestHistory { get; set; }

        public int TestDueBy
        {
            get { return _testDueBy; }
            set { _testDueBy = value; }
        }
        public int Charge
        {
            get { return _charge; }
            set { _charge = value; }
        }
        public int ThirdParty
        {
            get { return _thirdParty; }
            set { _thirdParty = value; }
        }
        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
        public double OverrideAmount
        {
            get { return _overrideAmount; }
            set { _overrideAmount = value; }
        }
        public String ThirdPartyName
        {
            get { return _thirdPartyName; }
            set { _thirdPartyName = value; }
        }
        public String ThirdPartyPhone
        {
            get { return _thirdPartyPhone; }
            set { _thirdPartyPhone = value; }
        }
        public String Custom1
        {
            get { return _custom1; }
            set { _custom1 = value; }
        }
        public String Custom2
        {
            get { return _custom2; }
            set { _custom2 = value; }
        }
        public String Custom3
        {
            get { return _custom3; }
            set { _custom3 = value; }
        }
        public String Custom4
        {
            get { return _custom4; }
            set { _custom4 = value; }
        }
        public String Proposal { get; set; }
        public int ProposalId { get; set; }

        public int PriceYear { get; set; }
      
        public Boolean CreateTicketForAll { get; set; }
        public String ScheduleDate { get; set; }
        public String ScheduleWorker { get; set; }
        public String ScheduleStatusName { get; set; }
        public int ScheduleStatusID { get; set; }
        public int ScheduleID { get; set; }

        public String ServiceDate { get; set; }
        public String ServiceWorker { get; set; }
        public String ServiceStatusName { get; set; }
        public int ServiceStatusID { get; set; }
        public int isDefautTest { get; set; }
        public Boolean UpdateThirdPartyForAll { get; set; }
        public string Classification { get; set; }
        #endregion
    }
    public class NotificationCustomChange
    {
        public String SubjectEmail;
        public String UserName;
        public String lsTeamMember;
        public String label;
        public String EquipmentName;
        public String EquipmentDesc;
        public String lsRole;
        public String EmailContent()
        {
            return UserName + " has edited " + label + " for " + EquipmentName +  " and " + EquipmentDesc;
        }
    }
}
