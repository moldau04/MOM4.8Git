using System;

namespace BusinessEntity
{
    [Serializable]
    public class MailSender
    {
        public Int32 ID { get; set; }

        public String Name { get; set; }

        public String FileName { get; set; }

        public String PDFFilePath { get; set; }

    }
}
