using System;
using System.IO;
using System.Text;

namespace ACHBAL
{
    public class Fileentry
    {
        bool m_flag=false;

        #region CreateFileControl
        /// <summary>
        /// FileControlCreation
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sbFileControl"></param>
        /// <returns></returns>
        public bool createFileEntry(string filename,out StringBuilder sbFileControl)
        {
            sbFileControl=null;
            if(!(File.Exists(filename)))
            {
                m_flag=false;
            }
            else
            {
                m_flag=true;
                StringBuilder sb=new StringBuilder();
                string strRectypecode="9";
                sb.Append(strRectypecode);

                //Batchcount 

                string Batchcnt=filecontrolvariables.batchcount.ToString().PadLeft(6,(char)48);
                sb.Append(Batchcnt);

                StreamReader sr=new StreamReader(filename);
                string srRead=sr.ReadToEnd();

                //Block count

                int blockcnt=0;
                if(srRead.Length%940==0)
                {
                    blockcnt=Math.Abs(srRead.Length/940);
                }
                else
                {
                    blockcnt=Math.Abs(srRead.Length/940)+1;
                }


                sb.Append(Convert.ToString(blockcnt).PadLeft(6,(char)48));
                sr.Close();

                //Entry/Addenda count

                string Entrycnt=filecontrolvariables.entrycount.ToString().PadLeft(8,(char)48);
                sb.Append(Entrycnt);

                //Hash Count

                string Entryhash=filecontrolvariables.Entryhash.ToString().PadLeft(10,(char)48);
                sb.Append(Entryhash);

                //Debit Amount

                string debtamnt=filecontrolvariables.debitamt.ToString().PadLeft(12,(char)48);
                sb.Append(debtamnt);

                //Credit AMount

                string credamnt=filecontrolvariables.creditamt.ToString().PadLeft(12,(char)48);
                sb.Append(credamnt);

                //Reserved

                string FileControlReserved="".PadLeft(39,(char)32).ToString();
                sb.Append(FileControlReserved);
                sbFileControl=sb;

            }
            return m_flag;
        }
        #endregion

        #region CreateFileControl
        /// <summary>
        /// FileControlCreation
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sbFileControl"></param>
        /// <returns></returns>
        public bool createFileEntryMidwest(string filename, out StringBuilder sbFileControl)
        {
            sbFileControl = null;
            if (!(File.Exists(filename)))
            {
                m_flag = false;
            }
            else
            {
                m_flag = true;
                StringBuilder sb = new StringBuilder();
                string strRectypecode = "9";
                sb.Append(strRectypecode);

                //Batchcount 

                string Batchcnt = filecontrolvariables.batchcount.ToString().PadLeft(6, (char)48);
                sb.Append(Batchcnt);

                StreamReader sr = new StreamReader(filename);
                string srRead = sr.ReadToEnd();

                //Block count

                int blockcnt = 0;
                if (srRead.Length % 940 == 0)
                {
                    blockcnt = Math.Abs(srRead.Length / 940);
                }
                else
                {
                    blockcnt = Math.Abs(srRead.Length / 940) + 1;
                }


                sb.Append(Convert.ToString(blockcnt).PadLeft(6, (char)48));
                sr.Close();

                //Entry/Addenda count

                string Entrycnt = filecontrolvariables.entrycount.ToString().PadLeft(8, (char)48);
                sb.Append(Entrycnt);

                //Hash Count

                string Entryhash = filecontrolvariables.Entryhash.ToString().PadLeft(10, (char)48);
                sb.Append(Entryhash);

                //Debit Amount

                string debtamnt = filecontrolvariables.debitamt.ToString().PadLeft(12, (char)48);
                sb.Append(debtamnt);

                //Credit AMount

                string credamnt = filecontrolvariables.creditamt.ToString().PadLeft(12, (char)48);
                sb.Append(credamnt);

                //Reserved

                string FileControlReserved = "".PadLeft(39, (char)32).ToString();
                sb.Append(FileControlReserved);
                sbFileControl = sb;

            }
            return m_flag;
        }
        #endregion


        #region WriteEntry

        /// <summary>
        /// Writing FileControl
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sb"></param>
        /// <returns></returns>

        public bool writeFileeEntry(string filename,StringBuilder sb)
        {
            if(!(File.Exists(filename)))
            {
                m_flag=false;
            }
            else
            {
                StreamWriter sw=File.AppendText(filename);
                sw.Write(sb);
                m_flag=true;
                sw.Close();
            }
            return m_flag;
        }
        #endregion
    }
}
