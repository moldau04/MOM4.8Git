using System;
using DataLayer;
using System.Data;
using BusinessEntity;


namespace BusinessLayer
{

    public class BL_EstimateForm
    {
        DL_EstimateForm dlef = new DL_EstimateForm();

        public DataSet GetEstimateFormsByEstimateId(EstimateForm ef)
        {
            return dlef.GetEstimateFormsByEstimateId(ef);
        }

        public DataSet GetEstimateLastProposalByEstimateId(EstimateForm ef)
        {
            return dlef.GetEstimateLastProposalByEstimateId(ef);
        }

        public void GetEstimateFormById(EstimateForm ef)
        {
            dlef.GetEstimateFormById(ef);
            PopulateFields(ef, ef.ds.Tables[0].Rows[0]);
        }

        public void AddEstimateForm(EstimateForm ef)
        {
            dlef.AddEstimateForm(ef);
        }

        public void DeleteEstimateForm(EstimateForm ef)
        {
            dlef.DeleteEstimateForm(ef);
        }

        public void UpdateEstimateForm(EstimateForm ef, String sendTO, String sendFrom, string sendBy)
        {
            dlef.UpdateEstimateForm(ef, sendTO, sendFrom, sendBy);
        }


        public void PopulateFields(EstimateForm ef, DataRow dr)
        {
            ef.Estimate = dr.Field<int>("Estimate");
            ef.JobTID = dr.Field<int>("JobTID");
            ef.Name = dr.Field<string>("Name");
            ef.FileName = dr.Field<string>("FileName");
            ef.FilePath = dr.Field<string>("FilePath");
            ef.PdfFilePath = dr.Field<string>("PdfFilePath");
            ef.Remark = dr.Field<string>("Remark");
            ef.MIME = dr.Field<string>("MIME");
            ef.AddedOn =Convert.ToString(dr["AddedOn"]);
        }

        //public void GenerateForms(DataSet estimate, EstimateTemplate et, EstimateForm ef, String formsPath)
        //{
        //    string guid = System.Guid.NewGuid().ToString();
        //    ef.FilePath = formsPath + guid + "." + et.MIME; //docx
        //    ef.PdfFilePath = formsPath + guid + ".pdf";

        //    File.Copy(et.FilePath, ef.FilePath);

        //    DataRow dr;
        //    using (DocX document = DocX.Load(ef.FilePath))
        //    {
        //        dr = estimate.Tables[0].Rows[0];
        //        document.ReplaceText("{Id}", Convert.ToString(dr["Id"]), false, RegexOptions.IgnoreCase);
        //        document.ReplaceText("{Name}", Convert.ToString(dr["Name"]), false, RegexOptions.IgnoreCase);
        //        document.ReplaceText("{Name}", Convert.ToString(dr["Name"]), false, RegexOptions.IgnoreCase);
        //        document.ReplaceText("{field}", "This is value of Field 2", false, RegexOptions.IgnoreCase);
        //        document.Save();
        //    }

        //    //Document doc = new Document(); //Free version of Spire.Doc has limitations of first three pages more details at https://www.e-iceblue.com/Introduce/free-doc-component.html
        //    //doc.LoadFromFile(ef.FilePath);
        //    //doc.SaveToFile(ef.PdfFilePath, FileFormat.PDF);

        //    #region Convert Docx file into PDF
        //    Word2Pdf objWorPdf = new Word2Pdf();
        //    object FromLocation = ef.FilePath;
        //    string FileExtension = Path.GetExtension(ef.FileName);
        //    if (FileExtension == ".doc" || FileExtension == ".docx")
        //    {
        //        object ToLocation = ef.PdfFilePath;
        //        objWorPdf.InputLocation = FromLocation;
        //        objWorPdf.OutputLocation = ToLocation;
        //        objWorPdf.Word2PdfCOnversion();
        //    }
        //    #endregion

        //    AddEstimateForm(ef);
        //}
    }
}
