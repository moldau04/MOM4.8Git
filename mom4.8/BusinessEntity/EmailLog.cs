using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class EmailLog
    {
        public EmailLog()
        {
            EmailDate = DateTime.Now;
        }
        public string ConnConfig { get; set; }
        public int Id { get; set; }
        public string Username{ get; set; }
        public DateTime EmailDate{ get; set; }
        public int Status{ get; set; }
        public string UsrErrMessage { get; set; }
        public string SysErrMessage { get; set; }
        public string Sender{ get; set; } 
        public string From{ get; set; } 
        public string To{ get; set; } 
        public string Screen{ get; set; } 
        public int Ref { get; set; }
        public List<int> Refs { get; set; }
        public string Function { get; set; }
        public string SessionNo { get; set; }
    }

    public class AddEmailLogParam
    {
        public DateTime EmailDate { get; set; }
        public string From { get; set; }
        public int Ref { get; set; }
        public string Screen { get; set; }
        public string Sender { get; set; }
        public int Status { get; set; }
        public string SysErrMessage { get; set; }
        public string To { get; set; }
        public string Username { get; set; }
        public string UsrErrMessage { get; set; }
        public string Function { get; set; }
        public string SessionNo { get; set; }
        public string ConnConfig { get; set; }

    }

    public class GetEmailLogsParam
    {
        public string Screen { get; set; }
        public string Function { get; set; }
        public string ConnConfig { get; set; }
    }
}
