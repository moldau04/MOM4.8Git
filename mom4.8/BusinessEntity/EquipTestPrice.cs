using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class EquipTestPrice
    {

        #region::Private Declaration::
        public int Id { get; set; }      
        public string TestTypeName { get; set; }
        public int TestTypeId { get; set; }       
        public double Amount { get; set; }
        public double OverrideAmount { get; set; }
        public int LastUpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Classification { get; set; }
        public int UpdateType { get; set; }
        public string Remarks { get; set; }
        public double DefaultHour { get; set; }

        public int PriceYear { get; set; }

        public Boolean IsThirdPartyRequired { get; set; }
        #endregion

    }

}
