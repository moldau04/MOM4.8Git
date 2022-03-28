using System;
using System.Data;

namespace BusinessEntity
{
    public class TestSetupForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string MIME { get; set; }
        public int Type { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public string ConnConfig { get; set; }


    }

    public class ProposalForm
    {
        public int Id { get; set; }

        public int LocID { get; set; }

        public string Classification { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string PdfFilePath { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int Type { get; set; }
        public String Status { get; set; }
        public String AlertEmail { get; set; }
        public string ListEquipment { get; set; }
        public int YearProposal { get; set; }
        public string ConnConfig { get; set; }

        public Boolean Chargable { get; set; }
        public int TestTypeID { get; set; }

    }
    public class ProposalFormDetail
    {
        public int Id { get; set; }

        public int ProposalID { get; set; }
       
        public int  EquipmentID { get; set; }

        public int TestID { get; set; }

        public String Status { get; set; }

        public string ConnConfig { get; set; }
        public int YearProposal { get; set; }

        public Boolean Chargable { get; set; }

        public int TestTypeID { get; set; }

    }

    public class TestSetupEmailForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }       
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public string ConnConfig { get; set; }


    }
}
