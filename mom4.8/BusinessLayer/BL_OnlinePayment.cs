using System;
using DataLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;

namespace BusinessLayer
{
    public class BL_OnlinePayment
    {
        DL_OnlinePayment objDL_OnlinePayment = new DL_OnlinePayment();

        public DataSet OnlinePaymentInsert(OnlinePayment objOnlinePayment)
        {
            return objDL_OnlinePayment.OnlinePaymentInsert(objOnlinePayment);
        }


        public DataSet OnlinePaymentSelect(OnlinePayment objOnlinePayment, List<RetainFilter> filters, int intEN)
        {
            return objDL_OnlinePayment.OnlinePaymentSelect(objOnlinePayment, filters, intEN);
        }


        public void OnlinePaymentDelete(OnlinePayment objOnlinePayment)
        {
            objDL_OnlinePayment.OnlinePaymentDelete(objOnlinePayment);
        }


        public void OnlinePaymentApprove(OnlinePayment objOnlinePayment)
        {
            objDL_OnlinePayment.OnlinePaymentApprove(objOnlinePayment);
        }

    }
}
