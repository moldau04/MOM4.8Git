using System;
using System.IO;
using System.Text;

namespace ACHBAL
{
    public class EntryDetail
    {

        int[] debits= { 25,27,28,29,35,37,38,39,47,48,48,49,55,81,84,86,88 };
        int[] credits= { 20,22,23,24,30,32,33,34,40,42,43,44,45,50,52,53,54,82,83,85,87 };
        //int[] addenda ={ 21, 26, 31, 36, 41, 46, 51, 56 };

        // Will check for the Credit and Debit Lengths

        int length=0;

        //Holds the FileControl if changes occured

        string newfileControl=string.Empty;

        // Holds the Old FileControl

        string oldfileControl=string.Empty;

        // Debit Amount Field

        double AmtforFileControl_debit=0;

        // Credit Amount Field

        double AmtforFileControl_credit=0;

        Fileentry oFileEntry=new Fileentry();

        // Holds the New Batch Control If changes in Entry occurs

        string strnewBatchcntrl=string.Empty;

        // Holds the Transaction Code when changed

        string strnewTransactioncode=string.Empty;

        // Holds the RDFI if changed

        string strnewRDFI=string.Empty;

        // Holds the DFIAccountNumber if changed

        string strnewDFIAccno=string.Empty;

        // Holds the OldTransactionCode

        string strOldTranscode=string.Empty;

        //Holds the Amount if changed

        string strnewAmnt=string.Empty;

        //Holds the Identification Number if changed

        string strnewIdentificationnumber=string.Empty;

        // Holds RecievingCompanyName while editing

        string strnewReceivingcompname=string.Empty;

        // Holds DiscretionaryData while editing

        string strnewDiscretionaryData=string.Empty;

        //Holds AddendaRecordIndicator while editing

        string strnewAddendarecordindicator=string.Empty;

        // Holds the TraceNumber when file opened

        string strTracenumber=string.Empty;

        // To Read the Entry Detail

        string strEntryread=string.Empty;

        // Stores strEntryRead if any changes will replace the content

        string strEntryLine=string.Empty;

        // Get the BatchControl

        string stroldBtchcntrl=string.Empty;

        // Stores RecordTypeCode

        private string m_strRecordTypeCode;

        // Stores TransactionCode

        private string m_strTransactionCode;

        // Stores RecievingDFIIdentification

        private string m_strRecievingDFIIdentification;

        // Stores CheckDigit

        private string m_strCheckDigit;

        // Stores DFIAccountNumber

        private string m_strDFIAccountNumber;

        // Stores Amount

        private string m_strAmount;

        // Stores Identification Number

        private string m_strIdentificationNumber;

        // Stores RecievingcompanyName

        private string m_strRecievingcompanyName;

        // Stores DiscretionaryData

        private string m_strDiscretionaryData;

        //Stores AddendaRecordIndicator

        private string m_strAddendaRecordIndicator;

        //Stores TraceNumber

        private string m_strTraceNumber;

        // Stores the Path of the file

        private static string m_path=string.Empty;

        private bool m_flag=false;

        /// <summary>
        /// Get and Set RecordTypeCode
        /// </summary>

        public string RecordTypeCode
        {
            get { return m_strRecordTypeCode; }
            set { m_strRecordTypeCode=value; }
        }

        /// <summary>
        ///  Get and Set TransactioCode
        /// </summary>

        public string TransactioCode
        {
            get { return m_strTransactionCode; }
            set { m_strTransactionCode=value; }
        }

        /// <summary>
        /// Get and Set RecievingDFIIdentification
        /// </summary>

        public string RecievingDFIIdentification
        {
            get { return m_strRecievingDFIIdentification; }
            set { m_strRecievingDFIIdentification=value; }
        }

        /// <summary>
        /// Get and Set CheckDigit
        /// </summary>

        public string CheckDigit
        {
            get { return m_strCheckDigit; }
            set { m_strCheckDigit=value; }
        }

        /// <summary>
        ///  Get and Set DFIAccountNumber
        /// </summary>

        public string DFIAccountNumber
        {

            get { return m_strDFIAccountNumber; }
            set { m_strDFIAccountNumber=value; }
        }

        /// <summary>
        /// Get and Set Amount
        /// </summary>

        public string Amount
        {
            get { return m_strAmount; }
            set { m_strAmount=value; }
        }

        /// <summary>
        /// Get and Set IdentificationNumber
        /// </summary>

        public string IdentificationNumber
        {
            get { return m_strIdentificationNumber; }
            set { m_strIdentificationNumber=value; }
        }

        /// <summary>
        /// Get and Set RecievingCompanyName
        /// </summary>

        public string RecievingCompanyName
        {
            get { return m_strRecievingcompanyName; }
            set { m_strRecievingcompanyName=value; }
        }

        /// <summary>
        /// Get and Set DiscretionaryData
        /// </summary>

        public string DiscretionaryData
        {
            get { return m_strDiscretionaryData; }
            set { m_strDiscretionaryData=value; }
        }

        /// <summary>
        /// Get and Set AddendaRecordIndicator
        /// </summary>

        public string AddendaRecordIndicator
        {
            get { return m_strAddendaRecordIndicator; }
            set { m_strAddendaRecordIndicator=value; }
        }

        /// <summary>
        /// Get and Set TraceNumber
        /// </summary>

        public string TraceNumber
        {
            get { return m_strTraceNumber; }
            set { m_strTraceNumber=value; }
        }

        #region AddEntryDetails
        /// <summary>
        /// Add Entry Details
        /// </summary>
        /// <returns></returns>
        public bool addEntrydetails()
        {
            this.m_flag=false;
            if(m_strRecordTypeCode!=string.Empty&&m_strTransactionCode!=string.Empty&&m_strRecievingDFIIdentification!=string.Empty&&
                m_strCheckDigit!=string.Empty&&m_strDFIAccountNumber!=string.Empty&&m_strAmount!=string.Empty&&m_strRecievingcompanyName!=string.Empty
                &&m_strAddendaRecordIndicator!=string.Empty)
            {
                saveEntry(m_path);
                m_flag=true;
            }
            else
            {
                m_flag=false;
            }
            return m_flag;
        }

        #endregion

        #region RDFIValidating

        /// <summary>
        /// RDFI Validation
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public bool BankRoutingNumberValidation(string sInput)
        {
            m_flag=false;

            bool bRule1=false;
            bool bRule2=false;

            int iResult=0;
            if(sInput.Length==9)
            {
                iResult=3*Convert.ToInt32(sInput.Substring(0,1))+
                7*Convert.ToInt32(sInput.Substring(1,1))+
                Convert.ToInt32(sInput.Substring(2,1))+
                3*Convert.ToInt32(sInput.Substring(3,1))+
                7*Convert.ToInt32(sInput.Substring(4,1))+
                Convert.ToInt32(sInput.Substring(5,1))+
                3*Convert.ToInt32(sInput.Substring(6,1))+
                7*Convert.ToInt32(sInput.Substring(7,1))+
                Convert.ToInt32(sInput.Substring(8,1));

                if((iResult%10)==0)
                {
                    bRule1=true;

                    iResult=7*Convert.ToInt32(sInput.Substring(0,1))+
                    3*Convert.ToInt32(sInput.Substring(1,1))+
                    9*Convert.ToInt32(sInput.Substring(2,1))+
                    7*Convert.ToInt32(sInput.Substring(3,1))+
                    3*Convert.ToInt32(sInput.Substring(4,1))+
                    9*Convert.ToInt32(sInput.Substring(5,1))+
                    7*Convert.ToInt32(sInput.Substring(6,1))+
                    3*Convert.ToInt32(sInput.Substring(7,1));

                    if((iResult%10)==Convert.ToInt32(sInput.Substring(8,1)))
                    {
                        bRule2=true;
                    }

                    m_flag=bRule1&bRule2;
                }
            }
            return m_flag;

        }

        #endregion

        //#region CheckforAddenda
        /////<summary>
        ///// To check for addenda
        /////</summary>
        /////<param name="strpath"></param>
        ///// <returns></returns>

        //public bool checkAddenda()
        //{
        //    m_flag = false;
        //    int addendaLength = addenda.Length;
        //    for (int l = 0; l < addendaLength; l++)
        //    {
        //        if (m_strTransactionCode == addenda[l].ToString())
        //        {
        //            m_flag = true;
        //        }
        //    }
        //    return m_flag;
        //}

        //#endregion


        /// <summary>
        /// Code for SaveEntry
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>

        public bool saveEntry(string strPath)
        {
            long Amnt=0;
            m_flag=true;
            StringBuilder nsb=new StringBuilder();
            if(Append.oldbatchcontrol==string.Empty)
                nsb.AppendLine();
            nsb.Append(m_strRecordTypeCode.PadLeft(1,(char)48));
            nsb.Append(m_strTransactionCode.PadLeft(2,(char)48));

            nsb.Append(m_strRecievingDFIIdentification.PadLeft(9,(char)48));
            nsb.Append(m_strDFIAccountNumber.PadRight(17,(char)32));
            nsb.Append(m_strAmount.PadLeft(10,(char)48));
            length=debits.Length;

            //Debit entry for Batch control and Filecontrol 

            for(int j=0;j<length;j++)
            {
                if(m_strTransactionCode==debits[j].ToString())
                {
                    Amnt=Convert.ToInt64(m_strAmount);
                    if(Append.oldbatchcontrol!=string.Empty)
                    {
                        AmtforFileControl_debit=Convert.ToDouble(m_strAmount);
                    }
                    Append.debitAmnt+=Amnt;
                    break;
                }
            }

            //Crebit entry for Batch control and Filecontrol 

            length=credits.Length;
            for(int k=0;k<length;k++)
            {
                if(m_strTransactionCode==credits[k].ToString())
                {
                    Amnt=Convert.ToInt64(m_strAmount);
                    if(Append.oldbatchcontrol!=string.Empty)
                    {
                        AmtforFileControl_credit=Convert.ToDouble(m_strAmount);
                    }
                    Append.creditAmnt+=Amnt;
                    break;
                }
            }
            nsb.Append(m_strIdentificationNumber.PadRight(15,(char)32));
            Append.EntryHash+=Convert.ToDouble(m_strRecievingDFIIdentification.Substring(0,8));
            nsb.Append(m_strRecievingcompanyName.PadRight(22,(char)32));
            nsb.Append(m_strDiscretionaryData.PadRight(2,(char)32));
            nsb.Append(m_strAddendaRecordIndicator.PadLeft(1,(char)48));
            nsb.Append(m_strTraceNumber.PadLeft(15,(char)48));

            if(Append.sb==null)
                Append.sb=nsb;
            else
                Append.sb.Append(nsb);
            //Entry Count for EntryDetail
            Append.Entrycnt++;

            if(Append.oldbatchcontrol!=string.Empty)
            {
                Append.sb.AppendLine();

                string strRecordtype="8";

                string strServcclasscode=Append.Seccode.ToString().PadLeft(3,(char)48);

                string Entrycnt=Append.Entrycnt.ToString().PadLeft(6,(char)48);

                filecontrolvariables.entrycount++;

                string EntryHash=Append.EntryHash.ToString().PadLeft(10,(char)48);

                filecontrolvariables.Entryhash+=Convert.ToDouble(m_strRecievingDFIIdentification.Substring(0,8));

                string debitAmnt=Append.debitAmnt.ToString().PadLeft(12,(char)48);

                filecontrolvariables.debitamt+=AmtforFileControl_debit;

                string creditAmnt=Append.creditAmnt.ToString().PadLeft(12,(char)48);

                filecontrolvariables.creditamt+=AmtforFileControl_credit;

                string CompIdentification=Append.Companyidentification.PadRight(10,(char)32);


                string MessageAuthenticationCode="".PadRight(19,(char)32).ToString();

                string Reserved="".PadRight(6,(char)32).ToString();

                string Odfi=Append.OriginalOdfi.PadLeft(8,(char)48);


                string batchno=Append.Batchnum_edit.ToString().PadLeft(7,(char)48);

                Append.sb.Append(strRecordtype);
                Append.sb.Append(strServcclasscode);
                Append.sb.Append(Entrycnt);
                Append.sb.Append(EntryHash);
                Append.sb.Append(debitAmnt);
                Append.sb.Append(creditAmnt);
                Append.sb.Append(CompIdentification);
                Append.sb.Append(MessageAuthenticationCode);
                Append.sb.Append(Reserved);
                Append.sb.Append(Odfi);
                Append.sb.Append(batchno);

                string Filedata=string.Empty;

                using(StreamReader forAddEntry=new StreamReader(strPath))
                {

                    while(forAddEntry.Peek()>=0)
                    {
                        Filedata=forAddEntry.ReadToEnd();
                       // Filedata=Filedata.Replace(Append.oldbatchcontrol,Append.sb.ToString());
                    }
                }
                //forAddEntry.Close();
                using(StreamWriter sw=new StreamWriter(strPath))
                {
                    sw.Write(Filedata);
                    //sw.Close();
                }
                Append.sb.Remove(0,Append.sb.Length);
            }
            return m_flag;
        }


        public bool saveEntryMidwest(string strPath)
        {
            long Amnt = 0;
            m_flag = true;
            StringBuilder nsb = new StringBuilder();
            if (Append.oldbatchcontrol == string.Empty)
                nsb.AppendLine();
            nsb.Append(m_strRecordTypeCode.PadLeft(1, (char)48));
            nsb.Append(m_strTransactionCode.PadLeft(2, (char)48));

            nsb.Append(m_strRecievingDFIIdentification.PadLeft(9, (char)48));
            nsb.Append(m_strDFIAccountNumber.PadRight(17, (char)32));
            nsb.Append(m_strAmount.PadLeft(10, (char)48));
            length = debits.Length;

            //Debit entry for Batch control and Filecontrol 

            for (int j = 0; j < length; j++)
            {
                if (m_strTransactionCode == debits[j].ToString())
                {
                    Amnt = Convert.ToInt64(m_strAmount);
                    if (Append.oldbatchcontrol != string.Empty)
                    {
                        AmtforFileControl_debit = Convert.ToDouble(m_strAmount);
                    }
                    Append.debitAmnt += Amnt;
                    break;
                }
            }

            //Crebit entry for Batch control and Filecontrol 

            length = credits.Length;
            for (int k = 0; k < length; k++)
            {
                if (m_strTransactionCode == credits[k].ToString())
                {
                    Amnt = Convert.ToInt64(m_strAmount);
                    if (Append.oldbatchcontrol != string.Empty)
                    {
                        AmtforFileControl_credit = Convert.ToDouble(m_strAmount);
                    }
                    Append.creditAmnt += Amnt;
                    break;
                }
            }
            nsb.Append("".PadRight(15, (char)32));
            //Append.EntryHash += Convert.ToDouble(m_strRecievingDFIIdentification.Substring(0, 8));
            nsb.Append(m_strRecievingcompanyName.PadRight(22, (char)32));
            nsb.Append(m_strDiscretionaryData.PadRight(2, (char)32));
            nsb.Append(m_strAddendaRecordIndicator.PadLeft(1, (char)48));
            nsb.Append(m_strTraceNumber.PadLeft(15, (char)48));

            if (Append.sb == null)
                Append.sb = nsb;
            else
                Append.sb.Append(nsb);
            //Entry Count for EntryDetail
            Append.Entrycnt++;

            if (Append.oldbatchcontrol != string.Empty)
            {
                Append.sb.AppendLine();

                string strRecordtype = "8";

                string strServcclasscode = Append.Seccode.ToString().PadLeft(3, (char)48);

                string Entrycnt = Append.Entrycnt.ToString().PadLeft(6, (char)48);

                filecontrolvariables.entrycount++;

                string EntryHash = Append.EntryHash.ToString().PadLeft(10, (char)48);

                filecontrolvariables.Entryhash += Convert.ToDouble(m_strRecievingDFIIdentification.Substring(0, 8));

                string debitAmnt = Append.debitAmnt.ToString().PadLeft(12, (char)48);

                filecontrolvariables.debitamt += AmtforFileControl_debit;

                string creditAmnt = Append.creditAmnt.ToString().PadLeft(12, (char)48);

                filecontrolvariables.creditamt += AmtforFileControl_credit;

                string CompIdentification = Append.Companyidentification.PadRight(10, (char)32);


                string MessageAuthenticationCode = "".PadRight(19, (char)32).ToString();

                string Reserved = "".PadRight(6, (char)32).ToString();

                string Odfi = Append.OriginalOdfi.PadLeft(8, (char)48);


                string batchno = Append.Batchnum_edit.ToString().PadLeft(7, (char)48);

                Append.sb.Append(strRecordtype);
                Append.sb.Append(strServcclasscode);
                Append.sb.Append(Entrycnt);
                Append.sb.Append(EntryHash);
                Append.sb.Append(debitAmnt);
                Append.sb.Append(creditAmnt);
                Append.sb.Append(CompIdentification);
                Append.sb.Append(MessageAuthenticationCode);
                Append.sb.Append(Reserved);
                Append.sb.Append(Odfi);
                Append.sb.Append(batchno);

                string Filedata = string.Empty;

                using (StreamReader forAddEntry = new StreamReader(strPath))
                {

                    while (forAddEntry.Peek() >= 0)
                    {
                        Filedata = forAddEntry.ReadToEnd();
                        // Filedata=Filedata.Replace(Append.oldbatchcontrol,Append.sb.ToString());
                    }
                }
                //forAddEntry.Close();
                using (StreamWriter sw = new StreamWriter(strPath))
                {
                    sw.Write(Filedata);
                    //sw.Close();
                }
                Append.sb.Remove(0, Append.sb.Length);
            }
            return m_flag;
        }

        #region CompareEntry
        /// <summary>
        /// To find whether changes in Entry Occured or not
        /// </summary>
        /// <returns></returns>

        public bool compareEntry(string stroldEntry,string strnewEntry)
        {
            bool m_flag=false;
            //bool m_batch = true;

            StringBuilder sbold=new StringBuilder();


            if(stroldEntry==strnewEntry)
            {
                m_flag=true;
            }
            else
            {
                StreamReader srt=new StreamReader(Append.FileName);
                while(srt.Peek()>=0)
                {
                    string line=srt.ReadLine();
                    if(line==stroldEntry)
                    {
                        while(srt.Peek()>=0)
                        {
                            line=srt.ReadLine();
                            if(line.StartsWith("8"))
                            {
                                Append.oldbatchcontrol=line;
                                Append.Seccode=Convert.ToInt16(line.Substring(1,3));
                                Append.Entrycnt=Convert.ToInt32(line.Substring(4,6));
                                Append.creditAmnt=Convert.ToInt64(line.Substring(32,12));
                                Append.debitAmnt=Convert.ToInt64(line.Substring(20,12));
                                Append.Batchnum_edit=Convert.ToInt32(line.Substring(87,7));
                                Append.EntryHash=0;
                                //m_batch = false;
                                break;
                            }
                            //if (line.StartsWith("9"))
                            //{
                            //    filecontrolvariables.oldFileControl = line;
                            //    filecontrolvariables.Entryhash = 0;

                            //}

                        }

                    }
                }
                srt.Close();
                StreamReader sr_for_BatchEntryHash=new StreamReader(Append.FileName);
                while(sr_for_BatchEntryHash.Peek()>=0)
                {
                    string line=sr_for_BatchEntryHash.ReadLine();
                    if(line.StartsWith("5")&&(Convert.ToInt32(line.Substring(87,7))==Append.Batchnum_edit))
                    {
                        while(!sr_for_BatchEntryHash.EndOfStream)
                        {
                            line=sr_for_BatchEntryHash.ReadLine();
                            if(line.StartsWith("6"))
                            {
                                Append.EntryHash+=Convert.ToDouble(line.Substring(3,8));
                            }
                            if(line.StartsWith("8"))
                            {
                                Append.oldbatchcontrol=line;
                                stroldBtchcntrl=line;
                                break;
                            }
                        }
                        break;
                    }
                }
                sr_for_BatchEntryHash.Close();

                strnewTransactioncode=strnewEntry.Substring(1,2);
                strnewRDFI=strnewEntry.Substring(3,9);
                strnewDFIAccno=strnewEntry.Substring(12,17);
                strnewAmnt=strnewEntry.Substring(29,10);
                strnewIdentificationnumber=strnewEntry.Substring(39,15);
                strnewReceivingcompname=strnewEntry.Substring(54,22);
                strnewDiscretionaryData=strnewEntry.Substring(76,2);
                strnewAddendarecordindicator=strnewEntry.Substring(78,1);

                strTracenumber=strnewEntry.Substring(79,15);

                StreamReader srEntry=new StreamReader(Append.FileName);
                while(srEntry.Peek()>=0)
                {
                    strEntryread=srEntry.ReadLine();
                    //if (strEntryread.StartsWith("1"))
                    //  sbold.Append(strEntryread);
                    //if (strEntryread.StartsWith("5"))
                    //{
                    //  sbold.AppendLine();
                    // sbold.Append(strEntryread);
                    //}
                    if(strEntryread.StartsWith("6"))
                    {
                        strTracenumber=strEntryread.Substring(79,15);
                        if(strTracenumber==filecontrolvariables.oldData.Substring(79,15))
                        {
                            strEntryLine=strEntryread;

                            //TransactionCode

                            strEntryLine=strEntryLine.Substring(0,1)+strnewTransactioncode+strEntryLine.Substring(3,(strEntryLine.Length-3));

                            //RDFI

                            strEntryLine=strEntryLine.Substring(0,3)+strnewRDFI+strEntryLine.Substring(12,(strEntryLine.Length-12));

                            Append.EntryHash=Append.EntryHash-Convert.ToDouble(stroldEntry.Substring(3,8))+Convert.ToDouble(strnewRDFI.Substring(0,8));
                            filecontrolvariables.Entryhash=filecontrolvariables.Entryhash-Convert.ToDouble(stroldEntry.Substring(3,8))+Convert.ToDouble(strnewRDFI.Substring(0,8));

                            //DFIAccountnumber

                            strEntryLine=strEntryLine.Substring(0,13)+strnewDFIAccno+strEntryLine.Substring(30,(strEntryLine.Length-30));

                            //Amount

                            strEntryLine=strEntryLine.Substring(0,29)+strnewAmnt+strEntryLine.Substring(39,(strEntryLine.Length-39));

                            //Debit entry for Batch control and Filecontrol 

                            strOldTranscode=stroldEntry.Substring(1,2);
                            length=debits.Length;
                            for(int j=0;j<length;j++)
                            {
                                if(strOldTranscode==debits[j].ToString())
                                {
                                    // Thread.Sleep(500);
                                    Append.debitAmnt-=Convert.ToInt64(stroldEntry.Substring(29,10));
                                    filecontrolvariables.debitamt-=Convert.ToInt64(stroldEntry.Substring(29,10));
                                    break;
                                }
                            }

                            //Crebit entry for Batch control and Filecontrol 

                            length=credits.Length;
                            for(int k=0;k<length;k++)
                            {
                                if(strOldTranscode==credits[k].ToString())
                                {
                                    //Thread.Sleep(500);
                                    Append.creditAmnt-=Convert.ToInt64(stroldEntry.Substring(29,10));
                                    filecontrolvariables.creditamt-=Convert.ToInt64(stroldEntry.Substring(29,10));
                                    break;
                                }
                            }

                            //strnewTransactioncode = strnew.Substring(1, 2);
                            length=debits.Length;
                            for(int j=0;j<length;j++)
                            {
                                if(strnewTransactioncode==debits[j].ToString())
                                {
                                    // Thread.Sleep(500);
                                    Append.debitAmnt+=Convert.ToInt64(strnewAmnt);
                                    filecontrolvariables.debitamt+=Convert.ToInt64(strnewAmnt);
                                    break;
                                }
                            }

                            //Credit entry for Batch control and Filecontrol 

                            length=credits.Length;
                            for(int k=0;k<length;k++)
                            {
                                if(strnewTransactioncode==credits[k].ToString())
                                {
                                    //Thread.Sleep(500);
                                    Append.creditAmnt+=Convert.ToInt64(strnewAmnt);
                                    filecontrolvariables.creditamt+=Convert.ToInt64(strnewAmnt);
                                    break;
                                }
                            }

                            //Identificationnumber(CCD)/IndividualIdentificationnumber(PPD)

                            strEntryLine=strEntryLine.Substring(0,39)+strnewIdentificationnumber+strEntryLine.Substring(54,(strEntryLine.Length-54));

                            //Receivingcompanyname(CCD) / Indiviudalname(PPD)

                            strEntryLine=strEntryLine.Substring(0,54)+strnewReceivingcompname+strEntryLine.Substring(76,(strEntryLine.Length-76));

                            //DiscretionaryData

                            strEntryLine=strEntryLine.Substring(0,76)+strnewDiscretionaryData+strEntryLine.Substring(78,(strEntryLine.Length-78));

                            //AddendaRecordIndicator

                            strEntryLine=strEntryLine.Substring(0,78)+strnewAddendarecordindicator+strEntryLine.Substring(79,(strEntryLine.Length-79));

                        }

                        StringBuilder sbnewbtchcntrl=new StringBuilder();

                        string strRecordtype="8";

                        string strServcclasscode=Append.Seccode.ToString().PadLeft(3,(char)48);

                        string Entrycnt=Append.Entrycnt.ToString().PadLeft(6,(char)48);

                        // filecontrolvariables.entrycount++;

                        string EntryHash=Append.EntryHash.ToString().PadLeft(10,(char)48);

                        // filecontrolvariables.Entryhash += Convert.ToDouble(m_strRecievingDFIIdentification.Substring(0, 8));

                        string debitAmnt=Append.debitAmnt.ToString().PadLeft(12,(char)48);

                        //filecontrolvariables.debitamt += AmtforFileControl_debit;

                        string creditAmnt=Append.creditAmnt.ToString().PadLeft(12,(char)48);

                        // filecontrolvariables.creditamt += AmtforFileControl_credit;

                        string CompIdentification=Append.Companyidentification.PadRight(10,(char)32);


                        string MessageAuthenticationCode="".PadRight(19,(char)32).ToString();

                        string Reserved="".PadRight(6,(char)32).ToString();

                        string Odfi=Append.OriginalOdfi.PadLeft(8,(char)48);


                        string batchno=Append.Batchnum_edit.ToString().PadLeft(7,(char)48);

                        Append.sb=new StringBuilder();

                        Append.sb.Append(strRecordtype);
                        Append.sb.Append(strServcclasscode);
                        Append.sb.Append(Entrycnt);
                        Append.sb.Append(EntryHash);
                        Append.sb.Append(debitAmnt);
                        Append.sb.Append(creditAmnt);
                        Append.sb.Append(CompIdentification);
                        Append.sb.Append(MessageAuthenticationCode);
                        Append.sb.Append(Reserved);
                        Append.sb.Append(Odfi);
                        Append.sb.Append(batchno);

                        strnewBatchcntrl=Append.sb.ToString();
                        Append.sb=null;
                    }
                }
                srEntry.Close();
                StringBuilder sb=new StringBuilder();
                oFileEntry.createFileEntry(Append.FileName,out sb);
                newfileControl=sb.ToString();
                StreamReader sroldfiledata=new StreamReader(Append.FileName);
                string oldfiledata=string.Empty;
                if(filecontrolvariables.oldFileControl==string.Empty)
                {
                    StreamReader srFilecntrl=new StreamReader(Append.FileName);
                    while(srFilecntrl.Peek()>=0)
                    {
                        string strFilcntrl=string.Empty;
                        strFilcntrl=srFilecntrl.ReadLine();
                        if(strFilcntrl.StartsWith("9"))
                            filecontrolvariables.oldFileControl=strFilcntrl;
                    }
                    srFilecntrl.Close();
                }
                while(sroldfiledata.Peek()>=0)
                    oldfiledata=sroldfiledata.ReadToEnd();
                sroldfiledata.Close();
                string newfiledata=string.Empty;
                if(filecontrolvariables.oldFileControl==string.Empty)
                {
                    //newfiledata = oldfiledata.Replace(stroldEntry, strEntryLine).Replace(stroldBtchcntrl, strnewBatchcntrl).Replace(filecontrolvariables.oldFileControl, newfileControl);
                    newfiledata=oldfiledata.Replace(stroldEntry,strEntryLine).Replace(stroldBtchcntrl,strnewBatchcntrl);
                }
                else
                    newfiledata=oldfiledata.Replace(stroldEntry,strEntryLine).Replace(stroldBtchcntrl,strnewBatchcntrl).Replace(filecontrolvariables.oldFileControl,newfileControl);
                StreamWriter sw=new StreamWriter(Append.FileName);
                sw.Write(newfiledata);
                sw.Close();
                filecontrolvariables.oldFileControl=string.Empty;
            }
            return m_flag;
        }

        #endregion

        #region EditedentryDetails
        /// <summary>
        /// When Entry Details Edited
        /// </summary>
        /// <returns></returns>
        public string editedEntry()
        {
            StringBuilder sb=new StringBuilder();
            sb.Append(m_strRecordTypeCode.PadLeft(1,(char)48));
            sb.Append(m_strTransactionCode.PadLeft(2,(char)48));
            sb.Append(m_strRecievingDFIIdentification.PadLeft(9,(char)48));
            sb.Append(m_strDFIAccountNumber.PadRight(17,(char)32));
            sb.Append(m_strAmount.PadLeft(10,(char)48));
            sb.Append(m_strIdentificationNumber.PadRight(15,(char)32));
            sb.Append(m_strRecievingcompanyName.PadRight(22,(char)32));
            sb.Append(m_strDiscretionaryData.PadRight(2,(char)32));
            sb.Append(m_strAddendaRecordIndicator.PadLeft(1,(char)48));
            sb.Append(m_strTraceNumber.PadLeft(15,(char)48));
            return sb.ToString();
        }

        #endregion
    }
}
