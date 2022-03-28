﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace MOMWebApp.WS_TrxDetail {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WS_TrxDetailSoap", Namespace="http://tempuri.org/")]
    public partial class WS_TrxDetail : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetOpenBatchSummaryOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetCardTrxOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetCardTrxSummaryOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetCheckTrxOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WS_TrxDetail() {
            this.Url = global::MOMWebApp.Properties.Settings.Default.MOMWebApp_WS_TrxDetail_WS_TrxDetail;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetOpenBatchSummaryCompletedEventHandler GetOpenBatchSummaryCompleted;
        
        /// <remarks/>
        public event GetCardTrxCompletedEventHandler GetCardTrxCompleted;
        
        /// <remarks/>
        public event GetCardTrxSummaryCompletedEventHandler GetCardTrxSummaryCompleted;
        
        /// <remarks/>
        public event GetCheckTrxCompletedEventHandler GetCheckTrxCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetOpenBatchSummary", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetOpenBatchSummary(string GlobalUserName, string GlobalPassword, string rpNum, string beginDt, string endDt, string extData) {
            object[] results = this.Invoke("GetOpenBatchSummary", new object[] {
                        GlobalUserName,
                        GlobalPassword,
                        rpNum,
                        beginDt,
                        endDt,
                        extData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetOpenBatchSummaryAsync(string GlobalUserName, string GlobalPassword, string rpNum, string beginDt, string endDt, string extData) {
            this.GetOpenBatchSummaryAsync(GlobalUserName, GlobalPassword, rpNum, beginDt, endDt, extData, null);
        }
        
        /// <remarks/>
        public void GetOpenBatchSummaryAsync(string GlobalUserName, string GlobalPassword, string rpNum, string beginDt, string endDt, string extData, object userState) {
            if ((this.GetOpenBatchSummaryOperationCompleted == null)) {
                this.GetOpenBatchSummaryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetOpenBatchSummaryOperationCompleted);
            }
            this.InvokeAsync("GetOpenBatchSummary", new object[] {
                        GlobalUserName,
                        GlobalPassword,
                        rpNum,
                        beginDt,
                        endDt,
                        extData}, this.GetOpenBatchSummaryOperationCompleted, userState);
        }
        
        private void OnGetOpenBatchSummaryOperationCompleted(object arg) {
            if ((this.GetOpenBatchSummaryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetOpenBatchSummaryCompleted(this, new GetOpenBatchSummaryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetCardTrx", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetCardTrx(
                    string GlobalUserName, 
                    string GlobalPassword, 
                    string RPNum, 
                    string PNRef, 
                    string BeginDt, 
                    string EndDt, 
                    string PaymentType, 
                    string ExcludePaymentType, 
                    string TransType, 
                    string ExcludeTransType, 
                    string ApprovalCode, 
                    string Result, 
                    string ExcludeResult, 
                    string NameOnCard, 
                    string CardNum, 
                    string CardType, 
                    string ExcludeCardType, 
                    string ExcludeVoid, 
                    string User, 
                    string invoiceId, 
                    string SettleFlag, 
                    string SettleMsg, 
                    string SettleDt, 
                    string TransformType, 
                    string Xsl, 
                    string ColDelim, 
                    string RowDelim, 
                    string IncludeHeader, 
                    string ExtData) {
            object[] results = this.Invoke("GetCardTrx", new object[] {
                        GlobalUserName,
                        GlobalPassword,
                        RPNum,
                        PNRef,
                        BeginDt,
                        EndDt,
                        PaymentType,
                        ExcludePaymentType,
                        TransType,
                        ExcludeTransType,
                        ApprovalCode,
                        Result,
                        ExcludeResult,
                        NameOnCard,
                        CardNum,
                        CardType,
                        ExcludeCardType,
                        ExcludeVoid,
                        User,
                        invoiceId,
                        SettleFlag,
                        SettleMsg,
                        SettleDt,
                        TransformType,
                        Xsl,
                        ColDelim,
                        RowDelim,
                        IncludeHeader,
                        ExtData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetCardTrxAsync(
                    string GlobalUserName, 
                    string GlobalPassword, 
                    string RPNum, 
                    string PNRef, 
                    string BeginDt, 
                    string EndDt, 
                    string PaymentType, 
                    string ExcludePaymentType, 
                    string TransType, 
                    string ExcludeTransType, 
                    string ApprovalCode, 
                    string Result, 
                    string ExcludeResult, 
                    string NameOnCard, 
                    string CardNum, 
                    string CardType, 
                    string ExcludeCardType, 
                    string ExcludeVoid, 
                    string User, 
                    string invoiceId, 
                    string SettleFlag, 
                    string SettleMsg, 
                    string SettleDt, 
                    string TransformType, 
                    string Xsl, 
                    string ColDelim, 
                    string RowDelim, 
                    string IncludeHeader, 
                    string ExtData) {
            this.GetCardTrxAsync(GlobalUserName, GlobalPassword, RPNum, PNRef, BeginDt, EndDt, PaymentType, ExcludePaymentType, TransType, ExcludeTransType, ApprovalCode, Result, ExcludeResult, NameOnCard, CardNum, CardType, ExcludeCardType, ExcludeVoid, User, invoiceId, SettleFlag, SettleMsg, SettleDt, TransformType, Xsl, ColDelim, RowDelim, IncludeHeader, ExtData, null);
        }
        
        /// <remarks/>
        public void GetCardTrxAsync(
                    string GlobalUserName, 
                    string GlobalPassword, 
                    string RPNum, 
                    string PNRef, 
                    string BeginDt, 
                    string EndDt, 
                    string PaymentType, 
                    string ExcludePaymentType, 
                    string TransType, 
                    string ExcludeTransType, 
                    string ApprovalCode, 
                    string Result, 
                    string ExcludeResult, 
                    string NameOnCard, 
                    string CardNum, 
                    string CardType, 
                    string ExcludeCardType, 
                    string ExcludeVoid, 
                    string User, 
                    string invoiceId, 
                    string SettleFlag, 
                    string SettleMsg, 
                    string SettleDt, 
                    string TransformType, 
                    string Xsl, 
                    string ColDelim, 
                    string RowDelim, 
                    string IncludeHeader, 
                    string ExtData, 
                    object userState) {
            if ((this.GetCardTrxOperationCompleted == null)) {
                this.GetCardTrxOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCardTrxOperationCompleted);
            }
            this.InvokeAsync("GetCardTrx", new object[] {
                        GlobalUserName,
                        GlobalPassword,
                        RPNum,
                        PNRef,
                        BeginDt,
                        EndDt,
                        PaymentType,
                        ExcludePaymentType,
                        TransType,
                        ExcludeTransType,
                        ApprovalCode,
                        Result,
                        ExcludeResult,
                        NameOnCard,
                        CardNum,
                        CardType,
                        ExcludeCardType,
                        ExcludeVoid,
                        User,
                        invoiceId,
                        SettleFlag,
                        SettleMsg,
                        SettleDt,
                        TransformType,
                        Xsl,
                        ColDelim,
                        RowDelim,
                        IncludeHeader,
                        ExtData}, this.GetCardTrxOperationCompleted, userState);
        }
        
        private void OnGetCardTrxOperationCompleted(object arg) {
            if ((this.GetCardTrxCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCardTrxCompleted(this, new GetCardTrxCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetCardTrxSummary", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetCardTrxSummary(
                    string GlobalUserName, 
                    string GlobalPassword, 
                    string RPNum, 
                    string BeginDt, 
                    string EndDt, 
                    string ApprovalCode, 
                    string Register, 
                    string NameOnCard, 
                    string CardNum, 
                    string CardType, 
                    string ExcludeVoid, 
                    string User, 
                    string SettleFlag, 
                    string SettleMsg, 
                    string SettleDt, 
                    string TransformType, 
                    string Xsl, 
                    string ColDelim, 
                    string RowDelim, 
                    string IncludeHeader, 
                    string ExtData) {
            object[] results = this.Invoke("GetCardTrxSummary", new object[] {
                        GlobalUserName,
                        GlobalPassword,
                        RPNum,
                        BeginDt,
                        EndDt,
                        ApprovalCode,
                        Register,
                        NameOnCard,
                        CardNum,
                        CardType,
                        ExcludeVoid,
                        User,
                        SettleFlag,
                        SettleMsg,
                        SettleDt,
                        TransformType,
                        Xsl,
                        ColDelim,
                        RowDelim,
                        IncludeHeader,
                        ExtData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetCardTrxSummaryAsync(
                    string GlobalUserName, 
                    string GlobalPassword, 
                    string RPNum, 
                    string BeginDt, 
                    string EndDt, 
                    string ApprovalCode, 
                    string Register, 
                    string NameOnCard, 
                    string CardNum, 
                    string CardType, 
                    string ExcludeVoid, 
                    string User, 
                    string SettleFlag, 
                    string SettleMsg, 
                    string SettleDt, 
                    string TransformType, 
                    string Xsl, 
                    string ColDelim, 
                    string RowDelim, 
                    string IncludeHeader, 
                    string ExtData) {
            this.GetCardTrxSummaryAsync(GlobalUserName, GlobalPassword, RPNum, BeginDt, EndDt, ApprovalCode, Register, NameOnCard, CardNum, CardType, ExcludeVoid, User, SettleFlag, SettleMsg, SettleDt, TransformType, Xsl, ColDelim, RowDelim, IncludeHeader, ExtData, null);
        }
        
        /// <remarks/>
        public void GetCardTrxSummaryAsync(
                    string GlobalUserName, 
                    string GlobalPassword, 
                    string RPNum, 
                    string BeginDt, 
                    string EndDt, 
                    string ApprovalCode, 
                    string Register, 
                    string NameOnCard, 
                    string CardNum, 
                    string CardType, 
                    string ExcludeVoid, 
                    string User, 
                    string SettleFlag, 
                    string SettleMsg, 
                    string SettleDt, 
                    string TransformType, 
                    string Xsl, 
                    string ColDelim, 
                    string RowDelim, 
                    string IncludeHeader, 
                    string ExtData, 
                    object userState) {
            if ((this.GetCardTrxSummaryOperationCompleted == null)) {
                this.GetCardTrxSummaryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCardTrxSummaryOperationCompleted);
            }
            this.InvokeAsync("GetCardTrxSummary", new object[] {
                        GlobalUserName,
                        GlobalPassword,
                        RPNum,
                        BeginDt,
                        EndDt,
                        ApprovalCode,
                        Register,
                        NameOnCard,
                        CardNum,
                        CardType,
                        ExcludeVoid,
                        User,
                        SettleFlag,
                        SettleMsg,
                        SettleDt,
                        TransformType,
                        Xsl,
                        ColDelim,
                        RowDelim,
                        IncludeHeader,
                        ExtData}, this.GetCardTrxSummaryOperationCompleted, userState);
        }
        
        private void OnGetCardTrxSummaryOperationCompleted(object arg) {
            if ((this.GetCardTrxSummaryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCardTrxSummaryCompleted(this, new GetCardTrxSummaryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetCheckTrx", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetCheckTrx(
                    string GlobalUserName, 
                    string GlobalPassword, 
                    string RPNum, 
                    string PNRef, 
                    string BeginDt, 
                    string EndDt, 
                    string PaymentType, 
                    string ExcludePaymentType, 
                    string TransType, 
                    string ExcludeTransType, 
                    string ApprovalCode, 
                    string Result, 
                    string ExcludeResult, 
                    string NameOnCheck, 
                    string CheckNum, 
                    string AcctNum, 
                    string RouteNum, 
                    string ExcludeVoid, 
                    string User, 
                    string invoiceId, 
                    string SettleFlag, 
                    string SettleMsg, 
                    string SettleDt, 
                    string TransformType, 
                    string Xsl, 
                    string ColDelim, 
                    string RowDelim, 
                    string IncludeHeader, 
                    string ExtData) {
            object[] results = this.Invoke("GetCheckTrx", new object[] {
                        GlobalUserName,
                        GlobalPassword,
                        RPNum,
                        PNRef,
                        BeginDt,
                        EndDt,
                        PaymentType,
                        ExcludePaymentType,
                        TransType,
                        ExcludeTransType,
                        ApprovalCode,
                        Result,
                        ExcludeResult,
                        NameOnCheck,
                        CheckNum,
                        AcctNum,
                        RouteNum,
                        ExcludeVoid,
                        User,
                        invoiceId,
                        SettleFlag,
                        SettleMsg,
                        SettleDt,
                        TransformType,
                        Xsl,
                        ColDelim,
                        RowDelim,
                        IncludeHeader,
                        ExtData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetCheckTrxAsync(
                    string GlobalUserName, 
                    string GlobalPassword, 
                    string RPNum, 
                    string PNRef, 
                    string BeginDt, 
                    string EndDt, 
                    string PaymentType, 
                    string ExcludePaymentType, 
                    string TransType, 
                    string ExcludeTransType, 
                    string ApprovalCode, 
                    string Result, 
                    string ExcludeResult, 
                    string NameOnCheck, 
                    string CheckNum, 
                    string AcctNum, 
                    string RouteNum, 
                    string ExcludeVoid, 
                    string User, 
                    string invoiceId, 
                    string SettleFlag, 
                    string SettleMsg, 
                    string SettleDt, 
                    string TransformType, 
                    string Xsl, 
                    string ColDelim, 
                    string RowDelim, 
                    string IncludeHeader, 
                    string ExtData) {
            this.GetCheckTrxAsync(GlobalUserName, GlobalPassword, RPNum, PNRef, BeginDt, EndDt, PaymentType, ExcludePaymentType, TransType, ExcludeTransType, ApprovalCode, Result, ExcludeResult, NameOnCheck, CheckNum, AcctNum, RouteNum, ExcludeVoid, User, invoiceId, SettleFlag, SettleMsg, SettleDt, TransformType, Xsl, ColDelim, RowDelim, IncludeHeader, ExtData, null);
        }
        
        /// <remarks/>
        public void GetCheckTrxAsync(
                    string GlobalUserName, 
                    string GlobalPassword, 
                    string RPNum, 
                    string PNRef, 
                    string BeginDt, 
                    string EndDt, 
                    string PaymentType, 
                    string ExcludePaymentType, 
                    string TransType, 
                    string ExcludeTransType, 
                    string ApprovalCode, 
                    string Result, 
                    string ExcludeResult, 
                    string NameOnCheck, 
                    string CheckNum, 
                    string AcctNum, 
                    string RouteNum, 
                    string ExcludeVoid, 
                    string User, 
                    string invoiceId, 
                    string SettleFlag, 
                    string SettleMsg, 
                    string SettleDt, 
                    string TransformType, 
                    string Xsl, 
                    string ColDelim, 
                    string RowDelim, 
                    string IncludeHeader, 
                    string ExtData, 
                    object userState) {
            if ((this.GetCheckTrxOperationCompleted == null)) {
                this.GetCheckTrxOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCheckTrxOperationCompleted);
            }
            this.InvokeAsync("GetCheckTrx", new object[] {
                        GlobalUserName,
                        GlobalPassword,
                        RPNum,
                        PNRef,
                        BeginDt,
                        EndDt,
                        PaymentType,
                        ExcludePaymentType,
                        TransType,
                        ExcludeTransType,
                        ApprovalCode,
                        Result,
                        ExcludeResult,
                        NameOnCheck,
                        CheckNum,
                        AcctNum,
                        RouteNum,
                        ExcludeVoid,
                        User,
                        invoiceId,
                        SettleFlag,
                        SettleMsg,
                        SettleDt,
                        TransformType,
                        Xsl,
                        ColDelim,
                        RowDelim,
                        IncludeHeader,
                        ExtData}, this.GetCheckTrxOperationCompleted, userState);
        }
        
        private void OnGetCheckTrxOperationCompleted(object arg) {
            if ((this.GetCheckTrxCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCheckTrxCompleted(this, new GetCheckTrxCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void GetOpenBatchSummaryCompletedEventHandler(object sender, GetOpenBatchSummaryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetOpenBatchSummaryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetOpenBatchSummaryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void GetCardTrxCompletedEventHandler(object sender, GetCardTrxCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCardTrxCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCardTrxCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void GetCardTrxSummaryCompletedEventHandler(object sender, GetCardTrxSummaryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCardTrxSummaryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCardTrxSummaryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void GetCheckTrxCompletedEventHandler(object sender, GetCheckTrxCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCheckTrxCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCheckTrxCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591