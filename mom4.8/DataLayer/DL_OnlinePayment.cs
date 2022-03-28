using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer
{
    public class DL_OnlinePayment
    {
        public DataSet OnlinePaymentInsert(OnlinePayment objOnlinePayment)
        {
            SqlParameter[] para = new SqlParameter[25];

            para[0] = new SqlParameter();
            para[0].ParameterName = "GatewayId";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objOnlinePayment.GatewayId;

            para[1] = new SqlParameter();
            para[1].ParameterName = "InvoiceID";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objOnlinePayment.InvoiceId;

            para[2] = new SqlParameter();
            para[2].ParameterName = "InvoiceDate";
            para[2].SqlDbType = SqlDbType.SmallDateTime;
            para[2].Value = objOnlinePayment.InvoiceDate;

            para[3] = new SqlParameter();
            para[3].ParameterName = "CustomerId";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objOnlinePayment.CustomerId;

            para[4] = new SqlParameter();
            para[4].ParameterName = "LocId";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objOnlinePayment.LocId;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Amount";
            para[5].SqlDbType = SqlDbType.Decimal;
            para[5].Value = objOnlinePayment.Amount;

            para[6] = new SqlParameter();
            para[6].ParameterName = "PaymentMode";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objOnlinePayment.PaymentMode;
         
            para[7] = new SqlParameter();
            para[7].ParameterName = "BankNameOnAccount";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objOnlinePayment.BankNameOnAccount;

            para[8] = new SqlParameter();
            para[8].ParameterName = "RoutingNumber";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objOnlinePayment.RoutingNumber;

            para[9] = new SqlParameter();
            para[9].ParameterName = "AccountNumber";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objOnlinePayment.AccountNumber;

            para[10] = new SqlParameter();
            para[10].ParameterName = "AccountType";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objOnlinePayment.AccountType;

            para[11] = new SqlParameter();
            para[11].ParameterName = "CardNumber";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objOnlinePayment.CardNumber;

            para[12] = new SqlParameter();
            para[12].ParameterName = "ExpiryDate";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objOnlinePayment.ExpiryDate;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Bin";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objOnlinePayment.Bin;

            para[14] = new SqlParameter();
            para[14].ParameterName = "FirstName";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objOnlinePayment.FirstName;

            para[15] = new SqlParameter();
            para[15].ParameterName = "LastName";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objOnlinePayment.LastDate;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Token";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objOnlinePayment.Token;

            para[17] = new SqlParameter();
            para[17].ParameterName = "AuthCode";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objOnlinePayment.AuthCode;

            para[18] = new SqlParameter();
            para[18].ParameterName = "GatewayTransactionId";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objOnlinePayment.GatewayTransactionId;

            para[19] = new SqlParameter();
            para[19].ParameterName = "NetworkTransactionId";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objOnlinePayment.NetworkTransactionId;

            para[20] = new SqlParameter();
            para[20].ParameterName = "transHashSha2";
            para[20].SqlDbType = SqlDbType.NVarChar;
            para[20].Value = objOnlinePayment.transHashSha2;

            para[21] = new SqlParameter();
            para[21].ParameterName = "GatewayResponseDump";
            para[21].SqlDbType = SqlDbType.NVarChar;
            para[21].Value = objOnlinePayment.GatewayResponseDump;

            para[22] = new SqlParameter();
            para[22].ParameterName = "TransactionStatus";
            para[22].SqlDbType = SqlDbType.Bit;
            para[22].Value = objOnlinePayment.TransactionStatus;

            para[23] = new SqlParameter();
            para[23].ParameterName = "ErrorCode";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objOnlinePayment.ErrorCode;

            para[24] = new SqlParameter();
            para[24].ParameterName = "ErrorText";
            para[24].SqlDbType = SqlDbType.NVarChar;
            para[24].Value = objOnlinePayment.ErrorText;

            try
            {
                return SqlHelper.ExecuteDataset(objOnlinePayment.ConnConfig, CommandType.StoredProcedure, "spOnlinePaymentInsert", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet OnlinePaymentSelect(OnlinePayment objOnlinePayment, List<RetainFilter> filters, int intEN)
        {
            try
            {
                objOnlinePayment.ConnConfig = HttpContext.Current.Session["config"].ToString();
                objOnlinePayment.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());

                SqlParameter[] para = new SqlParameter[12];

                para[0] = new SqlParameter();
                para[0].ParameterName = "intEN";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = intEN;

                para[1] = new SqlParameter();
                para[1].ParameterName = "UserId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objOnlinePayment.UserID;

                para[2] = new SqlParameter();
                para[2].ParameterName = "StartDate";
                para[2].SqlDbType = SqlDbType.DateTime;
                para[2].Value = objOnlinePayment.StartDate;

                para[3] = new SqlParameter();
                para[3].ParameterName = "EndDate";
                para[3].SqlDbType = SqlDbType.DateTime;
                para[3].Value = objOnlinePayment.EndDate;

                para[4] = new SqlParameter();
                para[4].ParameterName = "ID";
                para[4].SqlDbType = SqlDbType.Int;

                para[5] = new SqlParameter();
                para[5].ParameterName = "TransactionDate";
                para[5].SqlDbType = SqlDbType.DateTime;

                para[6] = new SqlParameter();
                para[6].ParameterName = "GatewayTransactionId";
                para[6].SqlDbType = SqlDbType.VarChar;

                para[7] = new SqlParameter();
                para[7].ParameterName = "CustomerName";
                para[7].SqlDbType = SqlDbType.VarChar;

                para[8] = new SqlParameter();
                para[8].ParameterName = "Location";
                para[8].SqlDbType = SqlDbType.VarChar;

                para[9] = new SqlParameter();
                para[9].ParameterName = "PaymentMode";
                para[9].SqlDbType = SqlDbType.VarChar;

                para[10] = new SqlParameter();
                para[10].ParameterName = "TransactionStatus";
                para[10].SqlDbType = SqlDbType.VarChar;

                para[11] = new SqlParameter();
                para[11].ParameterName = "Amount";
                para[11].SqlDbType = SqlDbType.Decimal;

                // Filter
                if (filters.Count > 0)
                {
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            para[4].Value = filter.FilterValue;
                        }
                        else
                        {
                            para[4].Value = DBNull.Value;
                        }

                        if (filter.FilterColumn == "TransactionDate")
                        {
                            para[5].Value = filter.FilterValue;
                        }
                        else
                        {
                            para[5].Value = DBNull.Value;
                        }

                        if (filter.FilterColumn == "GatewayTransactionId")
                        {
                            para[6].Value = filter.FilterValue;
                        }
                        else
                        {
                            para[6].Value = DBNull.Value;
                        }

                        if (filter.FilterColumn == "CustomerName")
                        {
                            para[7].Value = filter.FilterValue;
                        }
                        else
                        {
                            para[7].Value = DBNull.Value;
                        }

                        if (filter.FilterColumn == "Location")
                        {
                            para[8].Value = filter.FilterValue;
                        }
                        else
                        {
                            para[8].Value = DBNull.Value;
                        }

                        if (filter.FilterColumn == "PaymentMode")
                        {
                            para[9].Value = filter.FilterValue;
                        }
                        else
                        {
                            para[9].Value = DBNull.Value;
                        }

                        if (filter.FilterColumn == "TransactionStatus")
                        {
                            para[10].Value = filter.FilterValue;
                        }
                        else
                        {
                            para[10].Value = DBNull.Value;
                        }

                        if (filter.FilterColumn == "Amount")
                        {
                            para[11].Value = filter.FilterValue;
                        }
                        else
                        {
                            para[11].Value = DBNull.Value;
                        }

                    }
                }

                return SqlHelper.ExecuteDataset(objOnlinePayment.ConnConfig, CommandType.StoredProcedure, "spOnlinePaymentSelect", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void OnlinePaymentDelete(OnlinePayment objOnlinePayment)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objOnlinePayment.ConnConfig, "spOnlinePaymentDelete", objOnlinePayment.OnlinePaymentTransactionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void OnlinePaymentApprove(OnlinePayment objOnlinePayment)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objOnlinePayment.ConnConfig, "spOnlinePaymentApprove", objOnlinePayment.OnlinePaymentTransactionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
