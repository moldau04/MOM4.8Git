using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetEmailLogsViewModel
    {
        public string Username { get; set; }
        public DateTime EmailDate { get; set; }
        public string Status { get; set; }
        public int Ref { get; set; }
        public string EmailFunction { get; set; }
        public string SessionNo { get; set; }
        public string UsrErrMessage { get; set; }
        public string SysErrMessage { get; set; }
    }
}
