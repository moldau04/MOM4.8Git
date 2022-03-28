using BusinessEntity;
using DataLayer;
using System;
using System.Data;

namespace BusinessLayer
{
    public class BL_TimeCard
    {
        DL_TimeCard objTimeCard = new DL_TimeCard();
        public DataSet GetInputCard(TimeCard timeCard)
        {
            return objTimeCard.GetInputCard(timeCard);
        }

        public DataSet GetTimeCardJob(TimeCard timeCard)
        {
            return objTimeCard.GetTimeCardJob(timeCard);
        }

        public int SaveInputCard(TimeCard timeCard, string super,string worker, string category,
            int markedReview, string username, DataTable dts,int timesheet)
        {
            return objTimeCard.SaveTimeCardJob(timeCard,super,worker,category, markedReview, username, dts,timesheet);
        }

        public DataSet GetInvoiceByJobID(string prefixText,string Conn)
        {
            return objTimeCard.GetInvoiceByJobID(prefixText, Conn);
        }
    }
}
