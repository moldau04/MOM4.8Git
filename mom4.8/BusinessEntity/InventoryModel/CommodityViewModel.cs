using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class CommodityViewModel
    {


        #region ::StoreProc::
        public static string GET_ALL_COMMODITY = "spGetCommodity";
        public static string GET_ALL_COMMODITY_BY_ID = "spGetCommodity";
        public static string CREATE_COMMODITY = "spCreateCommodity";
        public static string UPDATE_COMMODITY = "spUpdateCommodity";

        #endregion

        #region ::Private Property Variable Declaration::
        private int _ID;
        private string _CommodityCode;
        private string _CommodityDesc;
        private bool _CommodityIsActive;
        #endregion

        #region::Public Property Declaration::
        public string ConnConfig;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public string CommodityCode
        {
            get
            {
                return _CommodityCode;
            }
            set
            {
                _CommodityCode = value;
            }
        }
        public string CommodityDesc
        {
            get
            {
                return _CommodityDesc;
            }
            set
            {
                _CommodityDesc = value;
            }
        }

        public bool CommodityIsActive
        {
            get
            {
                return _CommodityIsActive;
            }
            set
            {
                _CommodityIsActive = value;
            }
        }
        #endregion
    

}
}
