using System;
using System.IO;
using System.Text;

namespace ACHBAL
{
    public class ACHFileheader
    {
        //Stores PriorityCode

        private string m_strPriorityCode;

        //Stores ImmediateDestination

        private string m_strImmediateDestination;

        // Stores ImmediateOrigin

        private string m_strImmediateOrigin;

        // Stores FileCreationDate

        private string m_strFileCreationDate;

        //Stores ReferenceTypeCode

        private string m_strReferenceTypeCode;

        // Stores FileCreationTime

        private string m_strFileCreationTime;

        // Stores FileIDModifier

        private string m_strFileIdModifier;

        // Stores RecordSize

        private string m_strRecordSize;

        // Stores BlockingFactor

        private string m_strBlockingFactor;

        //Stores FormatCode

        private string m_strFormatCode;

        //Stores ImmediateDestinationName

        private string m_strImmediateDestinationName;

        // Stores ImmediateOriginName

        private string m_strImmediateOriginName;

        // Stores ReferenceCode

        private string m_strReferenceCode;


        private bool m_bFlag=false;

        /// <summary>
        /// Get and Set ReferenceTypeCode
        /// </summary>

        public string ReferenceTypeCode
        {
            get { return m_strReferenceTypeCode; }
            set { m_strReferenceTypeCode=value; }
        }

        /// <summary>
        /// Get and Set PriorityCode
        /// </summary>

        public string PriorityCode
        {
            get { return m_strPriorityCode; }
            set { m_strPriorityCode=value; }
        }

        /// <summary>
        /// Get and Set ImmediateDestination
        /// </summary>

        public string ImmediateDestination
        {
            get { return m_strImmediateDestination; }
            set { m_strImmediateDestination=value; }
        }

        /// <summary>
        /// Get and Set ImmediateOrigin
        /// </summary>

        public string ImmediateOrigin
        {
            get { return m_strImmediateOrigin; }
            set { m_strImmediateOrigin=value; }

        }

        /// <summary>
        /// Get and Set FileCreationDate
        /// </summary>

        public string FileCreationDate
        {
            get { return m_strFileCreationDate; }
            set { m_strFileCreationDate=value; }

        }

        /// <summary>
        /// Get and Set FileCreationTime
        /// </summary>

        public string FileCreationTime
        {
            get { return m_strFileCreationTime; }
            set { m_strFileCreationTime=value; }
        }

        /// <summary>
        /// Get and Set FileIdModifier
        /// </summary>

        public string FileIdModifier
        {
            get { return m_strFileIdModifier; }
            set { m_strFileIdModifier=value; }

        }

        /// <summary>
        /// Get and Set RecordSize
        /// </summary>

        public string RecordSize
        {
            get { return m_strRecordSize; }
            set { m_strRecordSize=value; }
        }

        /// <summary>
        /// Get and Set BlockingFactor
        /// </summary>

        public string BlockingFactor
        {
            get { return m_strBlockingFactor; }
            set { m_strBlockingFactor=value; }
        }

        /// <summary>
        /// Get and Set FormatCode
        /// </summary>

        public string FormatCode
        {
            get { return m_strFormatCode; }
            set { m_strFormatCode=value; }
        }

        /// <summary>
        /// Get and Set ImmediateDestinationName
        /// </summary>

        public string ImmediateDestinationName
        {
            get { return m_strImmediateDestinationName; }
            set { m_strImmediateDestinationName=value; }

        }

        /// <summary>
        /// Get and Set ImmediateOriginName
        /// </summary>

        public string ImmediateOriginName
        {
            get { return m_strImmediateOriginName; }
            set { m_strImmediateOriginName=value; }
        }

        /// <summary>
        /// Get and Set ReferenceCode
        /// </summary>

        public string ReferenceCode
        {
            get { return m_strReferenceCode; }
            set { m_strReferenceCode=value; }
        }

        #region AddFileHeader

        /// <summary>
        /// Adding File Header
        /// </summary>
        /// <returns></returns>


        public bool AddAchFileHeader()
        {
            this.m_bFlag=false;
            string m_path=null;
            try
            {
                if(m_strPriorityCode!=string.Empty&&m_strImmediateDestination!=string.Empty&&m_strImmediateOrigin!=string.Empty
                     &&m_strFileIdModifier!=string.Empty&&m_strRecordSize!=string.Empty&&m_strBlockingFactor!=string.Empty
                     &&m_strFormatCode!=string.Empty)
                {
                    SaveFileHeader(m_path);
                    m_bFlag=true;
                }
                else
                {
                    m_bFlag=false;
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
            return m_bFlag;
        }

        #endregion

        #region Save FileHeader

        /// <summary>
        /// Save File Header
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>

        public bool SaveFileHeader(string strPath)
        {
            bool m_flag=true;
            using(TextWriter tw=new StreamWriter(strPath))
            {
                tw.Write(m_strReferenceTypeCode.PadLeft(1,(char)48));
                tw.Write(m_strPriorityCode.PadLeft(2,(char)48));
                tw.Write(m_strImmediateDestination.PadLeft(10,(char)32));
                tw.Write(m_strImmediateOrigin.PadLeft(10,(char)32));
                string m_strDate1=m_strFileCreationDate.Replace("/","");
                tw.Write(m_strDate1.PadLeft(6,(char)48));
                string m_strTime1=m_strFileCreationTime.Replace(":","");
                tw.Write(m_strTime1.PadLeft(4,(char)48));
                tw.Write(m_strFileIdModifier.PadLeft(1,(char)48));
                tw.Write(m_strRecordSize.PadLeft(3,(char)48));
                tw.Write(m_strBlockingFactor.PadLeft(2,(char)48));
                tw.Write(m_strFormatCode.PadLeft(1,(char)48));
                tw.Write(m_strImmediateDestinationName.PadRight(23,(char)32));
                tw.Write(m_strImmediateOriginName.PadRight(23,(char)32));
                tw.Write(m_strReferenceCode.PadRight(8,(char)32));
                tw.Flush();
                tw.Close();
            }
            return m_flag;
        }
        public bool SaveFileHeaderMidwest(string strPath)
        {
            bool m_flag = true;
            using (TextWriter tw = new StreamWriter(strPath))
            {
                //tw.Write(m_strReferenceTypeCode.PadLeft(1, (char)48));
                //tw.Write(m_strPriorityCode.PadLeft(2, (char)48));
                tw.Write(m_strImmediateDestination.PadLeft(10, (char)32));
                //tw.Write(m_strImmediateOrigin.PadLeft(10, (char)32));
                string m_strDate1 = m_strFileCreationDate.Replace("/", "");
                tw.Write(m_strDate1.PadLeft(6, (char)48));
                string m_strTime1 = m_strFileCreationTime.Replace(":", "");
                tw.Write(m_strTime1.PadLeft(4, (char)48));
                tw.Write(m_strFileIdModifier.PadLeft(1, (char)48));

                string m_strRecordSize1 = m_strRecordSize.PadRight(23, (char)32);
                m_strRecordSize1 += m_strImmediateOriginName;
                tw.Write(m_strRecordSize1);
                //tw.Write(m_strRecordSize.PadRight(23, (char)32));

                //tw.Write(m_strBlockingFactor.PadLeft(2, (char)48));
                //tw.Write(m_strFormatCode.PadLeft(1, (char)48));
                //tw.Write(m_strImmediateDestinationName.PadRight(23, (char)32));
                //tw.Write(m_strImmediateOriginName.PadLeft(37, (char)32));
                //tw.Write(m_strImmediateOriginName);
                //tw.Write(m_strReferenceCode.PadRight(8, (char)32));
                tw.Flush();
                tw.Close();
            }
            return m_flag;
        }

        #endregion

        #region ReplaceFile
        /// <summary>
        /// Replacing File
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strNewFile"></param>
        /// <returns></returns>
        public bool replaceFile(string strPath,string strNewFile)
        {

            bool m_flag=true;
            StreamReader srNew=new StreamReader(strPath);
            string strReplace=srNew.ReadToEnd();
            strReplace=strReplace.Replace(filecontrolvariables.oldData,strNewFile);
            srNew.Close();
            StreamWriter sw=new StreamWriter(strPath);
            sw.Write(strReplace);
            sw.Close();
            return m_flag;
        }
        #endregion

        #region EditFileHeader
        /// <summary>
        /// When File Header Values Edited
        /// </summary>
        /// <returns></returns>
        public string editedFileHeader()
        {
            StringBuilder sb=new StringBuilder();
            sb.Append(m_strReferenceTypeCode.PadLeft(1,(char)48));
            sb.Append(m_strPriorityCode.PadLeft(2,(char)48));
            sb.Append(m_strImmediateDestination.PadLeft(10,(char)32));
            sb.Append(m_strImmediateOrigin.PadLeft(10,(char)32));
            string m_strDate1=m_strFileCreationDate.Replace("/","");
            sb.Append(m_strDate1.PadLeft(6,(char)48));
            string m_strTime1=m_strFileCreationTime.Replace(":","");
            sb.Append(m_strTime1.PadLeft(4,(char)48));
            sb.Append(m_strFileIdModifier.PadLeft(1,(char)48));
            sb.Append(m_strRecordSize.PadLeft(3,(char)48));
            sb.Append(m_strBlockingFactor.PadLeft(2,(char)48));
            sb.Append(m_strFormatCode.PadLeft(1,(char)48));
            sb.Append(m_strImmediateDestinationName.PadRight(23,(char)32));
            sb.Append(m_strImmediateOriginName.PadRight(23,(char)32));
            sb.Append(m_strReferenceCode.PadRight(8,(char)32));
            return sb.ToString();
        }

        #endregion
    }
}
