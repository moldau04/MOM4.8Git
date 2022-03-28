namespace BusinessEntity
{
    public class Commodity
    {

        #region ::StoreProc::
        public static string GET_ALL_COMMODITY = "spGetCommodity";
        public static string GET_ALL_COMMODITY_BY_ID = "spGetCommodity";
        public static string CREATE_COMMODITY = "spCreateCommodity";
        public static string UPDATE_COMMODITY = "spUpdateCommodity";

        #endregion

        #region ::Private Property Variable Declaration::
        private int _ID;
        private string _Code;
        private string _Desc;
        private bool _IsActive;
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
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
            }
        }
        public string Desc
        {
            get
            {
                return _Desc;
            }
            set
            {
                _Desc = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                _IsActive = value;
            }
        }
        #endregion
    }

    public class ReadAllCommodityParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string GET_ALL_COMMODITY_BY_ID = "spGetCommodity";


    }
}
