using System;
using System.IO;
using System.Text;

namespace ACHBAL
{
    public class ACHBatchHeader
    {
        // ODFI used while Editing a Batch if ODFI changes

        string strnewOdfi=string.Empty;

        //To get the Previous ODFI value from the file.

        string stroldOdfi=string.Empty;

        // Will get the Number of Entry Details exists in the file.

        string strentryLine=string.Empty;

        // will get the respective Batch from the file

        string strbtchline=string.Empty;

        // Get the Batch Control from the file

        string strBtchcntrl=string.Empty;

        //Get the Trace Number from the file.

        string stroldTaceno=string.Empty;

        //Get the Company Identification of the Batch Header from the File.

        string strcompIdentification=string.Empty;

        //Get the ServiceClassCode from the file.

        string strbtchcntrlSrvcClsCode=string.Empty;

        //Get the Company Identification of Batch Control from the file

        string strbtchcntrlCompIdentification=string.Empty;

        //Get the ODFI of Batch control from the file

        string strbtchcntrlODFI=string.Empty;

        // Stores the RecordTypeCode

        private static string m_strRecordTypeCode;

        // Stores the ServiceClassCode

        private static string m_strServiceClassCode;

        //Stores the CompanyName

        private static string m_strCompanyName;

        //Stroes CompanyDiscretionaryData

        private static string m_strCompanyDiscretionaryData;

        //Stores CompanyIdentification

        private static string m_strCompanyIdentification;

        //Stores StandardEntryClassCode

        private static string m_strStandardEntryClassCode;

        // Stores CompanyEntryDescription

        private static string m_strCompanyEntryDescription;

        // Stores CompanyDescriptiveDate

        private static string m_strCompanyDescriptiveDate;

        //Stores EffectiveEntryDate

        private static string m_strEffectiveEntryDate;

        //Stores OriginatorStatusCode

        private static string m_strOriginatorStatusCode;

        //Stores ODFI
        private static string m_strOriginationDFIIdentification;

        //Stores BatchNumber

        private static string m_strBatchNumber;

        //Stores JulianDate

        private static string m_strJulianDate;

        //Bool Value 

        private bool m_flag=false;

        /// <summary>
        /// Get and Set JulianDate
        /// </summary>

        public string JulianDate
        {
            get { return m_strJulianDate; }
            set { m_strJulianDate=value; }
        }

        /// <summary>
        /// Get and Set Flag
        /// </summary>

        public bool Flag
        {
            get { return m_flag; }
            set { m_flag=value; }
        }

        /// <summary>
        /// Get and Set RecordType
        /// </summary>

        public string RecordTypeCode
        {
            get { return m_strRecordTypeCode; }
            set { m_strRecordTypeCode=value; }
        }

        /// <summary>
        /// Get and Set ServiceClassCode
        /// </summary>

        public string ServiceClassCode
        {
            get { return m_strServiceClassCode; }
            set { m_strServiceClassCode=value; }
        }

        /// <summary>
        /// Get and Set CompanyName
        /// </summary>


        public string CompanyName
        {
            get { return m_strCompanyName; }
            set { m_strCompanyName=value; }
        }

        /// <summary>
        /// Get and Set CompanyDiscretionaryData
        /// </summary>

        public string CompanyDiscretionaryData
        {
            get { return m_strCompanyDiscretionaryData; }
            set { m_strCompanyDiscretionaryData=value; }
        }

        /// <summary>
        /// Get and Set CompanyIdentification
        /// </summary>

        public string CompanyIdentification
        {
            get { return m_strCompanyIdentification; }
            set { m_strCompanyIdentification=value; }
        }

        /// <summary>
        /// Get and Set StandardEntryClassCode
        /// </summary>

        public string StandardEntryClassCode
        {
            get { return m_strStandardEntryClassCode; }
            set { m_strStandardEntryClassCode=value; }
        }

        /// <summary>
        /// Get and Set CompanyEntryDescription
        /// </summary>

        public string CompanyEntryDescription
        {
            get { return m_strCompanyEntryDescription; }
            set { m_strCompanyEntryDescription=value; }
        }

        /// <summary>
        /// Get and Set CompanyDescriptiveDate
        /// </summary>

        public string CompanyDescriptiveDate
        {
            get { return m_strCompanyDescriptiveDate; }
            set { m_strCompanyDescriptiveDate=value; }
        }

        /// <summary>
        /// Get and Set EffectiveEntryDate
        /// </summary>

        public string EffectiveEntryDate
        {
            get { return m_strEffectiveEntryDate; }
            set { m_strEffectiveEntryDate=value; }
        }

        /// <summary>
        /// Get and Set OriginatorStatusCode
        /// </summary>

        public string OriginatorStatusCode
        {
            get { return m_strOriginatorStatusCode; }
            set { m_strOriginatorStatusCode=value; }
        }

        /// <summary>
        /// Get and Set OriginatingDFIIdentification
        /// </summary>

        public string OriginatingDFIIdentification
        {
            get { return m_strOriginationDFIIdentification; }
            set { m_strOriginationDFIIdentification=value; }
        }

        /// <summary>
        /// Get and Set BatchNumber
        /// </summary>

        public string BatchNumber
        {
            get { return m_strBatchNumber; }
            set { m_strBatchNumber=value; }
        }

        #region AddBatch

        /// <summary>
        /// Adding Batch Header
        /// </summary>
        /// <returns></returns>


        public bool AddBatchHeader()
        {
            string m_strpath=null;
            if(m_strServiceClassCode!=string.Empty&&m_strCompanyName!=string.Empty&&
                m_strCompanyIdentification!=string.Empty&&m_strCompanyEntryDescription!=string.Empty
                &&m_strEffectiveEntryDate!=string.Empty&&m_strOriginatorStatusCode!=string.Empty&&m_strOriginationDFIIdentification!=string.Empty
                &&m_strBatchNumber!=string.Empty)
            {
                m_flag=true;
                saveBatchHeader(m_strpath);
            }
            else
            {
                m_flag=false;
            }
            return m_flag;
        }

        #endregion

        #region compareBatch

        /// <summary>
        /// Comparing Old Batch with New
        /// </summary>
        /// <retruns></returns>

        public bool compareBatch(string strOld,string strNew)
        {
            StringBuilder sbold=new StringBuilder();

            //Holds the Service Class Code if service class code changes while editing

            string strnewSrvcClsCode=string.Empty;

            //Holds the Company Name if Company Name changes while editing.

            string strCompname=string.Empty;

            //Holds the CompanyDiscretionaryData if CompanyDiscretionaryData changes while editing.

            string strCompanyDiscretionaryData=string.Empty;

            //Holds the CompanyID if CompanyID changes while editing.

            string strCompanyID=string.Empty;

            //Holds the StandardEntryClassCode if StandardEntryClassCode changes while editing

            string strStndrdEntryClscode=string.Empty;

            //Holds the CompanyEntryDescription if CompanyEntryDescription changes while editing

            string strCompEntryDescription=string.Empty;

            //Holds the CompanyDescriptveDate if CompanyDescriptveDate changes while editing

            string strCompanyDescdate=string.Empty;

            //Holds the Effectiveentrydate if Effectiveentrydate changes while editing

            string strEffectiveentrydate=string.Empty;

            //Holds the OriginatorStatusCode if OriginatorStatusCode changes while editing.

            string strOriginatorStatusCode=string.Empty;

            //Used to check for appropriate BatchNumber while editing 
            string strBatchno=string.Empty;

            //Flag value to check whether change in data occurs or not

            bool m_flag=false;

            if(strOld==strNew)
            {
                m_flag=true;
            }
            else
            {
                strnewSrvcClsCode=strNew.Substring(1,3);
                strCompname=strNew.Substring(4,16);
                strCompanyDiscretionaryData=strNew.Substring(20,20);
                strcompIdentification=strNew.Substring(40,10);
                strCompEntryDescription=strNew.Substring(53,10);
                strCompanyDescdate=strNew.Substring(63,6);
                strEffectiveentrydate=strNew.Substring(69,6);
                strOriginatorStatusCode=strNew.Substring(78,1);
                stroldOdfi=strOld.Substring(79,8).ToString();
                strnewOdfi=strNew.Substring(79,8).ToString();

                strbtchcntrlSrvcClsCode=strNew.Substring(1,3);
                strbtchcntrlCompIdentification=strNew.Substring(40,10);
                strbtchcntrlODFI=strNew.Substring(79,8);
                //strbtchcntrlBacthno = strNew.Substring(87, 7);

                StreamReader sr=new StreamReader(Append.FileName);
                while(sr.Peek()>=0)
                {
                    string strRead=sr.ReadLine();
                    if(strRead.StartsWith("1"))
                        sbold.Append(strRead);
                    if(strRead.StartsWith("5"))
                    {
                        strBatchno=strRead.Substring(87,7);
                        if(filecontrolvariables.oldData.Substring(87,7)==strBatchno)
                        {
                            strbtchline=strRead;

                            // ServiceClassCode

                            strbtchline=strbtchline.Substring(0,1)+strnewSrvcClsCode+strbtchline.Substring(4,(strbtchline.Length-4));

                            //CompanyName

                            strbtchline=strbtchline.Substring(0,4)+strCompname+strbtchline.Substring(20,(strbtchline.Length-20));

                            //CompanyDiscretionaryData

                            strbtchline=strbtchline.Substring(0,20)+strCompanyDiscretionaryData+strbtchline.Substring(40,(strbtchline.Length-40));

                            //CompanyIdentification

                            strbtchline=strbtchline.Substring(0,40)+strcompIdentification+strbtchline.Substring(50,(strbtchline.Length-50));

                            //CompanyEntryDescription

                            strbtchline=strbtchline.Substring(0,53)+strCompEntryDescription+strbtchline.Substring(63,(strbtchline.Length-63));

                            //CompanyDescriptiveDate

                            strbtchline=strbtchline.Substring(0,63)+strCompanyDescdate+strbtchline.Substring(69,(strbtchline.Length-69));

                            //EffectiveEntryDate

                            strbtchline=strbtchline.Substring(0,69)+strEffectiveentrydate+strbtchline.Substring(75,(strbtchline.Length-75));

                            //OriginatorStatusCode

                            strbtchline=strbtchline.Substring(0,78)+strOriginatorStatusCode+strbtchline.Substring(79,(strbtchline.Length-79));

                            // ODFI

                            strbtchline=strbtchline.Substring(0,79)+strnewOdfi+strbtchline.Substring((strbtchline.Length-7),7);

                            sbold.AppendLine();
                            sbold.Append(strbtchline);
                        }
                        else
                        {
                            sbold.AppendLine();
                            sbold.Append(strRead);
                        }
                    }
                    if(strRead.StartsWith("6")&&(filecontrolvariables.oldData.Substring(87,7)==strBatchno))
                    {
                        stroldTaceno=strRead.Substring(79,8);
                        strentryLine=strRead;
                        strentryLine=strentryLine.Substring(0,79)+strnewOdfi+strentryLine.Substring((strentryLine.Length-7),7);
                        sbold.AppendLine();
                        sbold.Append(strentryLine);
                    }
                    else if(strRead.StartsWith("6")&&(filecontrolvariables.oldData.Substring(87,7)!=strBatchno))
                    {
                        sbold.AppendLine();
                        sbold.Append(strRead);
                    }

                    if(strRead.StartsWith("8")&&(filecontrolvariables.oldData.Substring(87,7)==strBatchno))
                    {
                        strBtchcntrl=strRead;

                        //ServiceClassCode

                        strBtchcntrl=strBtchcntrl.Substring(0,1)+strbtchcntrlSrvcClsCode+strBtchcntrl.Substring(4,(strBtchcntrl.Length-4));

                        //CompanyIdentification

                        strBtchcntrl=strBtchcntrl.Substring(0,44)+strbtchcntrlCompIdentification+strBtchcntrl.Substring(54,(strBtchcntrl.Length-54));

                        //ODFI

                        strBtchcntrl=strBtchcntrl.Substring(0,79)+strnewOdfi+strBtchcntrl.Substring((strBtchcntrl.Length-7),7);


                        sbold.AppendLine();
                        sbold.Append(strBtchcntrl);
                    }
                    else if(strRead.StartsWith("8")&&(filecontrolvariables.oldData.Substring(87,7)!=strBatchno))
                    {
                        sbold.AppendLine();
                        sbold.Append(strRead);
                    }
                    if(strRead.StartsWith("9"))
                    {
                        sbold.AppendLine();
                        sbold.Append(strRead);
                    }
                }
                sr.Close();

                StringBuilder sbNew=new StringBuilder(sbold.ToString());

                using(StreamWriter sw=new StreamWriter(Append.FileName))
                {
                    sw.Write(sbNew.ToString());
                }

            }
            return m_flag;
        }

        #endregion

        #region editedBatchHeader

        ///<summary>
        /// Batch Edited details
        ///</summary>
        ///<returns></returns>

        public string editedBatchHeader()
        {
            StringBuilder sb=new StringBuilder();
            sb.Append(m_strRecordTypeCode.PadLeft(1,(char)48));
            sb.Append(m_strServiceClassCode.PadLeft(3,(char)48));
            sb.Append(m_strCompanyName.PadRight(16,(char)32));
            sb.Append(m_strCompanyDiscretionaryData.PadRight(20,(char)32));
            sb.Append(m_strCompanyIdentification.PadRight(10,(char)32));
            sb.Append(m_strStandardEntryClassCode.PadRight(3,(char)32));
            sb.Append(m_strCompanyEntryDescription.PadRight(10,(char)32));
            string m_strCompanyDescripDate=m_strCompanyDescriptiveDate.Replace("/","");
            sb.Append(m_strCompanyDescripDate.PadLeft(6,(char)48));
            string m_strEffDate=m_strEffectiveEntryDate.Replace("/","");
            sb.Append(m_strEffDate.PadLeft(6,(char)48));
            sb.Append(m_strJulianDate.PadRight(3,(char)32));
            sb.Append(m_strOriginatorStatusCode.PadRight(1,(char)32));
            sb.Append(m_strOriginationDFIIdentification.PadLeft(8,(char)48));
            sb.Append(m_strBatchNumber.PadLeft(7,(char)48));
            return sb.ToString();

        }

        #endregion

        #region BatchValid

        /// <summary>
        /// To check whether Batch is valid or not
        /// </summary>
        /// <returns></returns>


        public bool IsBatchValid()
        {
            StringBuilder sb=new StringBuilder();
            sb.AppendLine();
            sb.Append(m_strRecordTypeCode.PadLeft(1,(char)48));
            sb.Append(m_strServiceClassCode.PadLeft(3,(char)48));
            sb.Append(m_strCompanyName.PadRight(16,(char)32));
            sb.Append(m_strCompanyDiscretionaryData.PadRight(20,(char)32));
            sb.Append(m_strCompanyIdentification.PadRight(10,(char)32));
            sb.Append(m_strStandardEntryClassCode.PadRight(3,(char)32));
            sb.Append(m_strCompanyEntryDescription.PadRight(10,(char)32));
            string m_strCompanyDescripDate=m_strCompanyDescriptiveDate.Replace("/","");
            sb.Append(m_strCompanyDescripDate.PadLeft(6,(char)48));
            string m_strEffDate=m_strEffectiveEntryDate.Replace("/","");
            sb.Append(m_strEffDate.PadLeft(6,(char)48));
            sb.Append(m_strJulianDate.PadRight(3,(char)32));
            sb.Append(m_strOriginatorStatusCode.PadRight(1,(char)32));
            sb.Append(m_strOriginationDFIIdentification.PadLeft(8,(char)48));
            sb.Append(m_strBatchNumber.PadLeft(7,(char)48));
            sb.Replace("\r\n",String.Empty);
            int len=sb.Length+Append.sb.Length;
            if(len<95)
            {
                m_flag=false;
            }
            else
            {
                m_flag=true;
            }
            return m_flag;
        }

        #endregion

        #region Saving Batch
        /// <summary>
        /// Save Batch Header
        /// </summary>
        /// <param name="m_strPath"></param>
        /// <returns></returns>

        public bool saveBatchHeader(string m_strPath)
        {
            StringBuilder sb=new StringBuilder();
            sb.AppendLine();
            sb.Append(m_strRecordTypeCode.PadLeft(1,(char)48));
            sb.Append(m_strServiceClassCode.PadLeft(3,(char)48));
            sb.Append(m_strCompanyName.PadRight(16,(char)32));
            sb.Append(m_strCompanyDiscretionaryData.PadRight(20,(char)32));
            sb.Append(m_strCompanyIdentification.PadRight(10,(char)32));
            sb.Append(m_strStandardEntryClassCode.PadRight(3,(char)32));
            sb.Append(m_strCompanyEntryDescription.PadRight(10,(char)32));
            string m_strCompanyDescripDate=m_strCompanyDescriptiveDate.Replace("/","");
            sb.Append(m_strCompanyDescripDate.PadLeft(6,(char)48));
            string m_strEffDate=m_strEffectiveEntryDate.Replace("/","");
            sb.Append(m_strEffDate.PadLeft(6,(char)48));
            sb.Append(m_strJulianDate.PadRight(3,(char)32));
            sb.Append(m_strOriginatorStatusCode.PadRight(1,(char)32));
            sb.Append(m_strOriginationDFIIdentification.PadLeft(8,(char)48));
            sb.Append(m_strBatchNumber.PadLeft(7,(char)48));
            sb.Append(Append.sb);

            Append.sb.Remove(0,Append.sb.Length);

            //Batch Control Record

            /// RecordTypeCode 

            string strRecordtype="8";

            ///ServiceClassCode 

            string strServcclasscode=m_strServiceClassCode.PadLeft(3,(char)48).ToString();

            ///Entry/Addenda count(Based on the Number of Entys/Addenda this will be generated

            string Entrycnt=Append.Entrycnt.ToString().PadLeft(6,(char)48);

            filecontrolvariables.entrycount+=Append.Entrycnt;

            ///EntryHash(Sum of all RDFI's)

            string EntryHash=Append.EntryHash.ToString().PadLeft(10,(char)48);
            filecontrolvariables.Entryhash+=Append.EntryHash;

            ///Debit Amount

            string debitAmnt=Append.debitAmnt.ToString().PadLeft(12,(char)48);
            filecontrolvariables.debitamt+=Append.debitAmnt;

            ///Credit Amount

            string creditAmnt=Append.creditAmnt.ToString().PadLeft(12,(char)48);
            filecontrolvariables.creditamt+=Append.creditAmnt;

            ///CompanyIdentification

            string CompIdentification=m_strCompanyIdentification.PadRight(10,(char)32);

            ///Message Authentication Code(This is Padded by Blank)

            string MessageAuthenticationCode="".PadRight(19,(char)32).ToString();

            /// Reserved(This one also padded with Blank)

            string Reserved="".PadRight(6,(char)32).ToString();

            /// ODFI(Which we can get from Batch Entry)

            string Odfi=m_strOriginationDFIIdentification.PadLeft(8,(char)48);

            ///BatchNumber

            string batchno=m_strBatchNumber.PadLeft(7,(char)48);

            sb.AppendLine();
            sb.Append(strRecordtype);
            sb.Append(strServcclasscode);
            sb.Append(Entrycnt);
            sb.Append(EntryHash);
            sb.Append(debitAmnt);
            sb.Append(creditAmnt);
            sb.Append(CompIdentification);
            sb.Append(MessageAuthenticationCode);
            sb.Append(Reserved);
            sb.Append(Odfi);
            sb.Append(batchno);

            filecontrolvariables.batchcount++;

            using(StreamWriter sw=File.AppendText(m_strPath))
            {
                sw.Write(sb);
            }
            //sw.Close();
            return m_flag;
        }

        public bool saveBatchHeaderMidwest(string m_strPath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.Append(m_strRecordTypeCode.PadLeft(1, (char)48));
            sb.Append(m_strServiceClassCode.PadLeft(3, (char)48));
            sb.Append(m_strCompanyName.PadRight(16, (char)32));
            sb.Append(m_strCompanyDiscretionaryData.PadRight(20, (char)32));
            sb.Append(m_strCompanyIdentification.PadRight(10, (char)32));
            sb.Append(m_strStandardEntryClassCode.PadRight(2, (char)32));
            sb.Append(m_strCompanyEntryDescription.PadRight(7, (char)32));
            string m_strCompanyDescripDate = m_strCompanyDescriptiveDate.Replace("/", "");
            //sb.Append(m_strCompanyDescripDate.PadLeft(6,(char)48));
            string m_strEffDate = m_strEffectiveEntryDate.Replace("/", "");
            //sb.Append(m_strEffDate.PadLeft(6, (char)48));
            sb.Append(m_strEffDate.PadRight(9, (char)32));
            //sb.Append(m_strJulianDate.PadRight(3, (char)32));
            //sb.Append(m_strOriginatorStatusCode.PadRight(1, (char)32));
            //sb.Append(m_strOriginationDFIIdentification.PadLeft(8, (char)48));
            sb.Append(m_strBatchNumber.PadLeft(7, (char)48));
            sb.Append(Append.sb);

            Append.sb.Remove(0, Append.sb.Length);

            //Batch Control Record

            /// RecordTypeCode 

            string strRecordtype = "8";

            ///ServiceClassCode 

            string strServcclasscode = m_strServiceClassCode.PadLeft(3, (char)48).ToString();

            ///Entry/Addenda count(Based on the Number of Entys/Addenda this will be generated

            string Entrycnt = Append.Entrycnt.ToString().PadLeft(6, (char)48);

            filecontrolvariables.entrycount += Append.Entrycnt;

            ///EntryHash(Sum of all RDFI's)
            /// CHAMGE HERE FOR 0008100098
            string EntryHash = Append.EntryHash.ToString().PadLeft(10, (char)48);
            filecontrolvariables.Entryhash += Append.EntryHash;

            ///Debit Amount

            string debitAmnt = Append.debitAmnt.ToString().PadLeft(12, (char)48);
            filecontrolvariables.debitamt += Append.debitAmnt;

            ///Credit Amount

            string creditAmnt = Append.creditAmnt.ToString().PadLeft(12, (char)48);
            filecontrolvariables.creditamt += Append.creditAmnt;

            ///CompanyIdentification

            string CompIdentification = m_strCompanyIdentification.Replace("PPDPAYROLL","").PadRight(10, (char)32);

            ///Message Authentication Code(This is Padded by Blank)

            string MessageAuthenticationCode = "".PadRight(19, (char)32).ToString();

            /// Reserved(This one also padded with Blank)

            string Reserved = "".PadRight(6, (char)32).ToString();

            /// ODFI(Which we can get from Batch Entry)

            //string Odfi = m_strOriginationDFIIdentification.PadLeft(8, (char)48);

            ///BatchNumber

            string batchno = m_strBatchNumber.Remove(0,1).PadLeft(7, (char)48);

            sb.AppendLine();
            sb.Append(strRecordtype);
            sb.Append(strServcclasscode);
            sb.Append(Entrycnt);
            sb.Append(EntryHash);
            sb.Append(debitAmnt);
            sb.Append(creditAmnt);
            sb.Append(CompIdentification);
            sb.Append(MessageAuthenticationCode);
            sb.Append(Reserved);
            //sb.Append(Odfi);
            sb.Append(batchno);

            filecontrolvariables.batchcount++;

            using (StreamWriter sw = File.AppendText(m_strPath))
            {
                sw.Write(sb);
            }
            //sw.Close();
            return m_flag;
        }

        #endregion
    }
}
