using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class TroubleCallsByEquipmentGraphRequest
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TopSelect { get; set; }

        public int CallTimes { get; set; }

        public int UserID { get; set; }

        public int EN { get; set; }

        public string Categories { get; set; }
        public string ConnConfig { get; set; }
    }
}
