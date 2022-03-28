using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessEntity
{
    public class ProposalData
    {
        public DateTime FromDate;
        public DateTime ToDate;
        public List<TestProposal> lsTestProposal;
        public List<TestProposalDetail> lsTestProposalDetail;
    }
    public class TestProposalDetail
    {
        public int YearProposal;
        public DataTable LocationInfo;
        public DataTable CustomerInfo;
        public List<ProposalEquipment> ProposalEquipment;
        public String Remark;
        public Double DefaultHour;
        public Double DefaultAmount;
        public String TestType;
        public String TestTypeCoverName;
        public String CoveredByTestTypeName;
        public int TestTypeID;
        public String Classification { get; set; }
    }
    public class ProposalEquipment
    {
        public int ID { get; set; }
        public String unit { get; set; }
        public String Classification { get; set; }
        public int TestID { get; set; }
        public Double Amount { get; set; }
        public Double OverrideAmount { get; set; }
        public String ThirdPartyName { get; set; }
        public String ThirdPartyPhone { get; set; }
        public Boolean Chargeable { get; set; }
        public string getChargeable()
        {
            if (Chargeable)
            {
                return "not covered";
            }
            else
            {
                return "covered";
            }
        }

        public String Remarks;
        public Boolean ThirdPartyRequired { get; set; }
    }
    public class TestProposal
    {
        public int YearProposal;
        public DataTable LocationInfo;
        public List<ProEquimentClassification> lsEquimentClassification;
        public String Remark;
        public Double DefaultHour;
        public Double DefaultAmount;
        public String TestType;
        public String TestTypeCoverName;
        public String CoveredByTestTypeName;
        public int TestTypeID;
    }
    public class ProEquimentClassification
    {
        public String EquipmentClassificationName;
        public List<ProEquiment> lsEquiment;
    }
    public class ProEquiment
    {
        public DataTable EquimentInfo;
        public List<ProEquimentTest> lsEquimentTest;
        public int ID;
        public String unit;
    }
    public class ProEquimentTest
    {
        public int TestID { get; set; }
        public Double Amount { get; set; }
        public Double OverrideAmount { get; set; }
        public String ThirdPartyName { get; set; }
        public Boolean Chargeable { get; set; }  
        public string getChargeable()
        {
            if (Chargeable)
            {
                return "not covered";
            }
            else
            {
                return "covered";
            }
        }
       
        public String Remarks;
        public Boolean ThirdPartyRequired { get; set; }

    }
}
