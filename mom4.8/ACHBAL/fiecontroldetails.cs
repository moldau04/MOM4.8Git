using System.Collections;
using System.Text;

namespace ACHBAL
{
    class fiecontroldetails
    {
    }

    /// <summary>
    /// Static Class filecontrolvariables
    /// </summary>

    public static class filecontrolvariables
    {
        // Holds the FileControl when file has content starts with 9

        public static string oldFileControl=string.Empty;

        // Used to increment the Batch Count for FileControl

        public static int batchcount=0;

        // To increment Entry Count for FileControl

        public static int entrycount=0;

        // Holds the EntryHash for FileControl

        public static double Entryhash=0;

        // Holds the Debit Amount for FileControl

        public static double debitamt=0;

        // Holds the Credit Amount for FileControl

        public static double creditamt=0;

        // To define the RecordType Code for the Added Record

        public static Hashtable rectype=new Hashtable();

        // Check whether Grid Cell Content Was Clicked or Not

        public static bool m_gridclick=false;

        //public static string stroldValue = string.Empty;

        // Check for Old Batch Control

        public static string oldData=string.Empty;

        //public static StringBuilder sbOld = new StringBuilder();

        // Check whether a File was Opened or Not

        public static bool m_fileOpen=false;

        // To check while Entry was adding with out Batch

        public static bool m_entryclick=false;

        // Removes the unnecessary characters

        public static char[] charRemove= { '\n','\r' };


        public static void ResetValues()
        {

            // Holds the FileControl when file has content starts with 9

            oldFileControl=string.Empty;

            // Used to increment the Batch Count for FileControl

            batchcount=0;

            // To increment Entry Count for FileControl

            entrycount=0;

            // Holds the EntryHash for FileControl

            Entryhash=0;

            // Holds the Debit Amount for FileControl

            debitamt=0;

            // Holds the Credit Amount for FileControl

            creditamt=0;

            // To define the RecordType Code for the Added Record

             rectype=new Hashtable();

            // Check whether Grid Cell Content Was Clicked or Not

            m_gridclick=false;

            //  string stroldValue = string.Empty;

            // Check for Old Batch Control

            oldData=string.Empty;

            //  StringBuilder sbOld = new StringBuilder();

            // Check whether a File was Opened or Not

            m_fileOpen=false;

            // To check while Entry was adding with out Batch

            m_entryclick=false;

            // Removes the unnecessary characters

           // char[] charRemove= { '\n','\r' };
        }
    }

    #region staticlass Append

    public static class Append
    {
        // FileName where the data is stored

        public static string FileName=string.Empty;

        // Check for the ODFI in Batch Control

        public static string OriginalOdfi=string.Empty;

        //Check for the existing Batch Control

        public static string oldbatchcontrol=string.Empty;

        // Holds the BatchControl Data 
        public static StringBuilder sb=new StringBuilder();


        public static int Batchcnt=0;
        public static int Batchnum_edit=0;
        public static int Traceno=0;
        public static int Entrycnt=0;
        public static bool m_newBatch=false;
        public static double EntryHash=0;

        // Debit Amount for Batch Control

        public static long debitAmnt=0;

        // Credit Amount for Batch Control

        public static long creditAmnt=0;


        public static int Seccode=0;
        public static bool m_flag=false;
        public static string Companyidentification=string.Empty;
        public static string StandEntryCode=string.Empty;
        public static string oldFileControl=string.Empty;


        public static void ResetValues()
        {

            // FileName where the data is stored
            FileName=string.Empty;
            // Check for the ODFI in Batch Control
            OriginalOdfi=string.Empty;
            //Check for the existing Batch Control
            oldbatchcontrol=string.Empty;
            // Holds the BatchControl Data 
             sb=new StringBuilder();
            Batchcnt=0;
            Batchnum_edit=0;
            Traceno=0;
            Entrycnt=0;
            m_newBatch=false;
            EntryHash=0;
            // Debit Amount for Batch Control
            debitAmnt=0;
            // Credit Amount for Batch Control
            creditAmnt=0;
            Seccode=0;
            m_flag=false;
            Companyidentification=string.Empty;
            StandEntryCode=string.Empty;
            oldFileControl=string.Empty;

        }
    }

    #endregion
}
