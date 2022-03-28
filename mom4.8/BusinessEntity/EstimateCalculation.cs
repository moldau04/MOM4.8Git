namespace BusinessEntity
{
    public class EstimateCalculation
    {


        #region ::StoreProc::
        public static string GET_ESTIMATETEMPLATE_CALCULATION = "spGetEstimateTemplateCalculation";
        public static string CREATE_ESTIMATETEMPLATE_CALCULATION = "spCreateEstimateTemplateCalculation";
        public static string CREATE_ESTIMATETEMPLATE_CALCULATION_TEMPLATE_INPUT_DATA_DERIVED_ITEMS = "spCreateEstimateCalculationTemplateInputDataDerivedItems";
        public static string UPDATE_ESTIMATETEMPLATE_SEQUENCE = "spUpdateEstimateTemplateCalculationSequence";
        public static string GET_ESTIMATETEMPLATE_CALCULATION_DERIVEDITEMS = "spGetEstimateTemplateInputDataDerivedItems";
        public static string GET_ESTIMATE_EXPENSE_CALCULATION = "spGetEstimateCalculation";

        #endregion


        public string ConnConfig { get; set; }
        #region ::Private Property Variable Declaration::
        private int _ID;
        private int _EstimateCalculationType;
        private int _EstimateCalculationTemplateSequence;
        private int _EstimateCalculationTemplateBOMTID;
        private bool _EstimateCalculationTemplateIsBOM;
        private bool _EstimateCalculationTemplateIsTax;
        private bool _EstimateCalculationInputBasedCalculation;
        private bool _EstimateCalculationTemplateInputDataDerived;
        private bool _EstimateCalculationTemplateShowClaculatedValue;

        private string _EstimateCalculationTemplateNo;
        private string _EstimateCalculationTemplateLableName;
        private decimal _EstimateCalculationTemplateInputBasedCalculationfactor;
        private string _EstimateCalculationTemplateInputBasedCalculationOperation;
        private EstiateInputDataDerivedItems[] _DerivedItems;
        private string _EstimateCalculationValue;
        private int _EstimateId;
        #endregion

        #region::Public Property Declaration::
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
        public int EstimateCalculationType
        {
            get
            {
                return _EstimateCalculationType;
            }
            set
            {
                _EstimateCalculationType = value;
            }
        }
        public int EstimateCalculationTemplateSequence
        {
            get
            {
                return _EstimateCalculationTemplateSequence;
            }
            set
            {
                _EstimateCalculationTemplateSequence = value;
            }
        }

        public int EstimateCalculationTemplateBOMTID
        {
            get
            {
                return _EstimateCalculationTemplateBOMTID;
            }
            set
            {
                _EstimateCalculationTemplateBOMTID = value;
            }
        }

        public bool EstimateCalculationTemplateIsBOM
        {
            get
            {
                return _EstimateCalculationTemplateIsBOM;
            }
            set
            {
                _EstimateCalculationTemplateIsBOM = value;
            }
        }
        public bool EstimateCalculationTemplateIsTax
        {
            get
            {
                return _EstimateCalculationTemplateIsTax;
            }
            set
            {
                _EstimateCalculationTemplateIsTax = value;
            }
        }
        public bool EstimateCalculationInputBasedCalculation
        {
            get
            {
                return _EstimateCalculationInputBasedCalculation;
            }
            set
            {
                _EstimateCalculationInputBasedCalculation = value;
            }
        }
        public bool EstimateCalculationTemplateInputDataDerived
        {
            get
            {
                return _EstimateCalculationTemplateInputDataDerived;
            }
            set
            {
                _EstimateCalculationTemplateInputDataDerived = value;
            }
        }
        public bool EstimateCalculationTemplateShowClaculatedValue
        {
            get
            {
                return _EstimateCalculationTemplateShowClaculatedValue;
            }
            set
            {
                _EstimateCalculationTemplateShowClaculatedValue = value;
            }
        }


        public string EstimateCalculationTemplateNo
        {
            get
            {
                return _EstimateCalculationTemplateNo;
            }
            set
            {
                _EstimateCalculationTemplateNo = value;
            }
        }
        public string EstimateCalculationTemplateLableName
        {
            get
            {
                return _EstimateCalculationTemplateLableName;
            }
            set
            {
                _EstimateCalculationTemplateLableName = value;
            }
        }
        public string EstimateCalculationTemplateInputBasedCalculationOperation
        {
            get
            {
                return _EstimateCalculationTemplateInputBasedCalculationOperation;
            }
            set
            {
                _EstimateCalculationTemplateInputBasedCalculationOperation = value;
            }
        }
        public decimal EstimateCalculationTemplateInputBasedCalculationfactor
        {
            get
            {
                return _EstimateCalculationTemplateInputBasedCalculationfactor;
            }
            set
            {
                _EstimateCalculationTemplateInputBasedCalculationfactor = value;
            }
        }

        public EstiateInputDataDerivedItems[] DerivedItems
        {
            get
            {
                return _DerivedItems;
            }
            set
            {
                _DerivedItems = value;
            }

        }

        #endregion

        #region GridHepler variables

        public string ExpenseType
        {
            get;
            set;


        }
        public string EstimateCalculationValue
        {
            get
            {
                return _EstimateCalculationValue;
            }
            set
            {
                _EstimateCalculationValue = value;
            }
        }

        public int EstimateId
        {
            get
            {
                return _EstimateId;
            }
            set
            {
                _EstimateId = value;
            }
        }
        #endregion

    }
    public class EstiateInputDataDerivedItems
    {
        #region ::Private Property Variable Declaration::
        private int _ID;
        private int _EstimateCalculationForTemplateId;
        private int _EstimateCalculationOnTemplateId;
        private string _Operation;
        #endregion

        #region::Public Property Declaration::
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
        public int EstimateCalculationForTemplateId
        {
            get
            {
                return _EstimateCalculationForTemplateId;
            }
            set
            {
                _EstimateCalculationForTemplateId = value;
            }
        }
        public int EstimateCalculationOnTemplateId
        {
            get
            {
                return _EstimateCalculationOnTemplateId;
            }
            set
            {
                _EstimateCalculationOnTemplateId = value;
            }
        }
        public string Operation
        {
            get
            {
                return _Operation;
            }
            set
            {
                _Operation = value;
            }
        }
        #endregion
    }
}
