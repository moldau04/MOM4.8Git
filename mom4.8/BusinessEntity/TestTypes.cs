namespace BusinessEntity
{
    public class TestTypes
    {

        #region::Private Declaration::
        private int _id;
        private string _name;
        private string _Authority;
        private int _Frequency;
        private string _Remarks;
        private int _Count;
        private int _Level;
        private string _Cat;
        private string _fDesc;
        private int _NextDateCalcMode;
        private int _Charge;
        private int _ThirdParty;
        public int Status;
        public string TestTypeCover;
        public bool TicketCovered;
        #endregion

        #region::Public Declaration::
        public string ConnConfig;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string Authority
        {
            get
            {
                return _Authority;
            }
            set
            {
                _Authority = value;
            }
        }
        public int Frequency
        {
            get
            {
                return _Frequency;
            }
            set
            {
                _Frequency = value;
            }
        }
        public string Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                _Remarks = value;
            }
        }
        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
                _Count = value;
            }
        }
        public int Level
        {
            get
            {
                return _Level;
            }
            set
            {
                _Level = value;
            }
        }
        public string Cat
        {
            get
            {
                return _Cat;
            }
            set
            {
                _Cat = value;
            }
        }
        public string fDesc
        {
            get
            {
                return _fDesc;
            }
            set
            {
                _fDesc = value;
            }
        }
        public int NextDateCalcMode
        {
            get
            {
                return _NextDateCalcMode;
            }
            set
            {
                _NextDateCalcMode = value;
            }
        }

        public int Charge
        {
            get
            {
                return _Charge;
            }
            set
            {
                _Charge = value;
            }
        }
        public int ThirdParty
        {
            get
            {
                return _ThirdParty;
            }
            set
            {
                _ThirdParty = value;
            }
        }
        #endregion

    }
}
