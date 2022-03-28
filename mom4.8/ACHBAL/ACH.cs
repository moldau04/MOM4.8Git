using System;
using System.IO;
using System.Text;

namespace ACHBAL
{
    public class ACH
    {

        #region CreateFileHeader
       /// <summary>
        /// Create File Header
       /// </summary>
       /// <param name="FileName"></param>
       /// <param name="FileCreationTime"></param>
       /// <param name="FileCreationDate"></param>
        public void CreateFileHeader(string FileName,string FileCreationTime,string FileCreationDate)
        {
            ACHFileheader objBL=new ACHFileheader();
            string ImmediateDestination = "043000096";
            string ImmediateOrigin = "043000096";
            string PriorityCode="01";
            string FormatCode="1";
            string FileIdModifier="A";
            string ReferenceTypeCode="1";
            string RecordSize="094";
            string BlockingFactor="10";
            //string FileCreationTime="2246";
            string ImmediateDestinationName="PNC BANK";
            //string FileCreationDate="040520";
            string ImmediateOriginName="SOUTHERN ELEVATOR COMPA";
            string ReferenceCode="";
            Append.FileName=FileName;
            if(ImmediateDestination!=string.Empty&&ImmediateDestination.Length==9&&ImmediateOrigin.Length==9&&
                ImmediateOrigin!=string.Empty&&PriorityCode!=string.Empty&&
                FileIdModifier!=string.Empty&&RecordSize!=string.Empty&&BlockingFactor!=string.Empty&&
                FormatCode!=string.Empty)
            {
                objBL.ReferenceTypeCode=ReferenceTypeCode;
                objBL.PriorityCode=PriorityCode;
                objBL.ImmediateDestination=ImmediateDestination;
                objBL.ImmediateOrigin=ImmediateOrigin;
                objBL.FileCreationDate=Convert.ToString(FileCreationDate);//.ToString("yy/MM/dd"));
                objBL.FileCreationTime=FileCreationTime.Replace(":","");
                objBL.FileIdModifier=FileIdModifier;
                objBL.RecordSize=RecordSize;
                objBL.BlockingFactor=BlockingFactor;
                objBL.FormatCode=FormatCode;
                objBL.ImmediateDestinationName=ImmediateDestinationName;
                objBL.ImmediateOriginName=ImmediateOriginName;
                objBL.ReferenceCode=ReferenceCode;
                objBL.SaveFileHeader(Append.FileName);
            }

        }

        #endregion

        #region CreateBatchHeader
        /// <summary>
        /// CreateBatchHeader
        /// </summary>
        public void CreateBatchHeader()
        {
            bool m_flag=false;
            ACHBatchHeader objBatchHeader=new ACHBatchHeader();
            EntryDetail objEntry=new EntryDetail();
            int cmbServiceClassCode=200;//ddl 
            string CompanyName="SOUTHERNELEVATOR";
            string CompanyIdentification = "2561733838";
            int StandardEntryClassCode=0;//ddl
            string CompanyEntryDescription="CRED DEBIT";
            string OriginatorStatusCode="1";
            string OriginatingDFIIdentification = "043000096";
            string CompanyDiscretionaryData="CODES0/28/03";
            string RecordTypeCode="5";
            string BatchNumber="0000001";
            string CompanyDescriptiveDate="040521";
            string EffectiveEntryDate = DateTime.Now.ToString("yyMMdd");   //"131011";
            string JulianDate="000";
            if(cmbServiceClassCode!=-1&&CompanyName!=string.Empty&&CompanyIdentification!=string.Empty&&
               StandardEntryClassCode==0&&CompanyEntryDescription!=string.Empty&&OriginatorStatusCode!=string.Empty&&
                   OriginatingDFIIdentification!=string.Empty&&objEntry.BankRoutingNumberValidation(OriginatingDFIIdentification))
            {

                objBatchHeader.RecordTypeCode=RecordTypeCode;
                objBatchHeader.ServiceClassCode=cmbServiceClassCode.ToString();
                objBatchHeader.CompanyName=CompanyName;
                objBatchHeader.CompanyDiscretionaryData=CompanyDiscretionaryData;
                objBatchHeader.CompanyIdentification=CompanyIdentification;
                objBatchHeader.StandardEntryClassCode=StandardEntryClassCode.ToString()=="0"?"PPD":"CCD";
                objBatchHeader.CompanyEntryDescription=CompanyEntryDescription;
                objBatchHeader.CompanyDescriptiveDate=Convert.ToString(CompanyDescriptiveDate);//ToString("yy/MM/dd"));
                objBatchHeader.EffectiveEntryDate=Convert.ToString(EffectiveEntryDate);//ToString("yy/MM/dd"));
                objBatchHeader.JulianDate=JulianDate.ToString();
                objBatchHeader.OriginatorStatusCode=OriginatorStatusCode;
                objBatchHeader.OriginatingDFIIdentification=OriginatingDFIIdentification.Substring(0,8);
                objBatchHeader.BatchNumber=BatchNumber;
                //string FileName = frmmain._strPath;

                if(objBatchHeader.IsBatchValid())
                {
                    string filedata=string.Empty;
                    string strcontent=string.Empty;
                    string srEnd=string.Empty;

                    m_flag=true;
                    if(m_flag)
                    {
                        using(StreamReader sr=new StreamReader(Append.FileName))
                        {
                            while(sr.Peek()>=0)
                            {
                                srEnd=sr.ReadLine();
                                if(srEnd.StartsWith("9"))
                                {
                                    strcontent=srEnd;
                                }
                            }
                        }
                        //sr.Close();
                        using(StreamReader srNew=new StreamReader(Append.FileName))
                        {
                            while(srNew.Peek()>=0)
                            {
                                filedata=srNew.ReadToEnd();
                                if(strcontent!=string.Empty)
                                    filedata=filedata.Replace(strcontent,"").TrimEnd(filecontrolvariables.charRemove);
                            }
                        }
                        //srNew.Close();
                        using(StreamWriter swwrite=new StreamWriter(Append.FileName))
                        {
                            swwrite.Write(filedata);
                        }
                        //swwrite.Close();
                        m_flag=false;
                        //sr.Close();
                    }

                    objBatchHeader.saveBatchHeader(Append.FileName);
                    //("Batch for the record saved successfully","Message"

                }


            }
            else if(cmbServiceClassCode!=-1&&CompanyName!=string.Empty&&CompanyIdentification!=string.Empty&&
               StandardEntryClassCode==1&&CompanyEntryDescription!=string.Empty&&OriginatorStatusCode!=string.Empty&&
                   OriginatingDFIIdentification!=string.Empty&&objEntry.BankRoutingNumberValidation(OriginatingDFIIdentification))
            {


                {
                    objBatchHeader.RecordTypeCode=RecordTypeCode;
                    objBatchHeader.ServiceClassCode=cmbServiceClassCode.ToString();
                    objBatchHeader.CompanyName=CompanyName;
                    objBatchHeader.CompanyDiscretionaryData=CompanyDiscretionaryData;
                    objBatchHeader.CompanyIdentification=CompanyIdentification;
                    objBatchHeader.StandardEntryClassCode=StandardEntryClassCode.ToString();
                    objBatchHeader.CompanyEntryDescription=CompanyEntryDescription;
                    objBatchHeader.CompanyDescriptiveDate=Convert.ToString(CompanyDescriptiveDate);//ToString("yy/MM/dd"));
                    objBatchHeader.EffectiveEntryDate=Convert.ToString(EffectiveEntryDate);//ToString("yy/MM/dd"));
                    objBatchHeader.JulianDate="".PadRight(3,' ').ToString();
                    objBatchHeader.OriginatorStatusCode=OriginatorStatusCode;
                    objBatchHeader.OriginatingDFIIdentification=OriginatingDFIIdentification.Substring(0,8);
                    objBatchHeader.BatchNumber=BatchNumber;
                    if(objBatchHeader.IsBatchValid())
                    {
                        string filedata=string.Empty;
                        string strcontent=string.Empty;
                        string srEnd=string.Empty;

                        m_flag=true;
                        if(m_flag)
                        {
                            using(StreamReader sr=new StreamReader(Append.FileName))
                            {
                                while(sr.Peek()>=0)
                                {
                                    srEnd=sr.ReadLine();
                                    if(srEnd.StartsWith("9"))
                                    {
                                        strcontent=srEnd;
                                    }
                                }
                                //   sr.Close();
                            }
                            using(StreamReader srNew=new StreamReader(Append.FileName))
                            {
                                while(srNew.Peek()>=0)
                                {
                                    filedata=srNew.ReadToEnd();
                                    if(strcontent!=string.Empty)
                                        filedata=filedata.Replace(strcontent,"").TrimEnd(filecontrolvariables.charRemove);
                                }
                                //   srNew.Close();
                            }
                            using(StreamWriter swwrite=new StreamWriter(Append.FileName))
                            {
                                swwrite.Write(filedata);
                                //swwrite.Close();
                            }



                            m_flag=false;
                            // sr.Close();
                        }

                        objBatchHeader.saveBatchHeader(Append.FileName);
                        //"Batch for the record saved successfully"

                    }
                    else
                    {
                        //You must save atleast one entry" 
                    }
                }
            }
        }
        #endregion

        #region AddEntryDetails
        /// <summary>
        /// Add Entry Details
        /// </summary>
        /// <param name="BankRouting_OR_DFIAccountNumber"></param>
        /// <param name="AccountHolderName_OR_RecievingCompanyName"></param>
        /// <param name="BankAccount_OR_IdentificationNumber"></param>
        /// <param name="Amount"></param>

        public void AddEntryDetails(string BankRouting_OR_DFIAccountNumber,string AccountHolderName_OR_RecievingCompanyName,string BankAccount_OR_IdentificationNumber,string Amount)
        {
            string RecordTypeCode="6";
            string TransactioCode="27";
            string RecievingDFIIdentification = "043000096";
            string CheckDigit="6";
            string DiscretionaryData="";
            string AddendaRecordIndicator="0";
            string TraceNumber="043000090000001";
            EntryDetail objEntry=new EntryDetail();
            bool m_flag=false;
            if(objEntry.BankRoutingNumberValidation(RecievingDFIIdentification)&&TransactioCode!="Select"&&RecievingDFIIdentification!=string.Empty&&
                BankRouting_OR_DFIAccountNumber!=string.Empty&&Amount!=string.Empty&&AccountHolderName_OR_RecievingCompanyName!=string.Empty&&AddendaRecordIndicator!=string.Empty)
            {

                objEntry.RecordTypeCode=RecordTypeCode;
                objEntry.TransactioCode=TransactioCode.ToString().Substring(0,2);
                objEntry.RecievingDFIIdentification=RecievingDFIIdentification;
                objEntry.DFIAccountNumber=BankRouting_OR_DFIAccountNumber;
                objEntry.Amount=Amount.Replace("$","").Replace(".","");
                objEntry.IdentificationNumber=BankAccount_OR_IdentificationNumber;
                objEntry.RecievingCompanyName=AccountHolderName_OR_RecievingCompanyName;
                objEntry.DiscretionaryData=DiscretionaryData;
                objEntry.AddendaRecordIndicator=AddendaRecordIndicator;
                objEntry.TraceNumber=TraceNumber;
                objEntry.saveEntry(Append.FileName);
            }

        }
        #endregion

        #region CreateFileControle
        /// <summary>
        /// Create File Controle
        /// </summary>

        public void CreateFileControle()
        {
            Fileentry objfileentry=new Fileentry();
            StringBuilder sb=new StringBuilder();
            objfileentry.createFileEntry(Append.FileName,out sb);
            File.AppendAllText(Append.FileName,Environment.NewLine+sb.ToString());

        }

        #endregion
    }
}
