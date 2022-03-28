using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class OnlinePayment
    {
        private string _ConnConfig;
        //public string ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int OnlinePaymentTransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int GatewayId { get; set; }
        public int InvoiceId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public int LocId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public string BankNameOnAccount { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Bin { get; set; }
        public string FirstName { get; set; }
        public string LastDate { get; set; }
        public string Token { get; set; }
        public string AuthCode { get; set; }
        public string GatewayTransactionId { get; set; }
        public string NetworkTransactionId { get; set; }
        public string transHashSha2 { get; set; }
        public string GatewayResponseDump { get; set; }
        public bool TransactionStatus { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorText { get; set; }
        public DateTime StartDate;
        public DateTime EndDate;
        public int UserID { get; set; }
    }

    public class DeleteOnlinePaymentParam
    {
        public int ID;
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetAllOnlinePaymentParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<RetainFilter> filters { get; set; }
        public int intEN { get; set; }
    }

    public class UpdateOnlinePaymentParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int OnlinePaymentTransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int GatewayId { get; set; }
        public int InvoiceId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public int LocId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public string BankNameOnAccount { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Bin { get; set; }
        public string FirstName { get; set; }
        public string LastDate { get; set; }
        public string Token { get; set; }
        public string AuthCode { get; set; }
        public string GatewayTransactionId { get; set; }
        public string NetworkTransactionId { get; set; }
        public string transHashSha2 { get; set; }
        public string GatewayResponseDump { get; set; }
        public bool TransactionStatus { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }

    public class AddOnlinePaymentParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime TransactionDate { get; set; }
        public int GatewayId { get; set; }
        public int InvoiceId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public int LocId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public string BankNameOnAccount { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Bin { get; set; }
        public string FirstName { get; set; }
        public string LastDate { get; set; }
        public string Token { get; set; }
        public string AuthCode { get; set; }
        public string GatewayTransactionId { get; set; }
        public string NetworkTransactionId { get; set; }
        public string transHashSha2 { get; set; }
        public string GatewayResponseDump { get; set; }
        public bool TransactionStatus { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }

    public class GetOnlinePaymentByIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID { get; set; }
    }

}
