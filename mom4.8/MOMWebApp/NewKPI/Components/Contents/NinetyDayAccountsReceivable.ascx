<%@ Control Language="C#" AutoEventWireup="true" Inherits="NewKPI_Components_Contents_NinetyDayAccountsReceivable" Codebehind="NinetyDayAccountsReceivable.ascx.cs" %>

<div class="kpi-card">
    <div class="card-content white white-text" style="min-height: 140px;">
        <div class="editbtns-sml">
            <a class="btn-floating waves-effect waves-light blue darken-2 right"><i class="mdi-content-add activator" style="color: #fff !important;"></i></a>
        </div>
        <div class="blue-text text-darken-2">
            <p class="card-stats-title icn-stats"><i class="mdi-action-account-balance"></i></p>
            <p class="card-stats-title">90+ Accounts Receivable</p>
            <h4 class="card-stats-number"><span ID="CountsNinetyPlus" runat="server"></span>
                <%--<i ID="ImgPlusUp" runat="server" class="mdi-hardware-keyboard-arrow-up red-text" style="font-size: 2rem !important; font-weight: bold !important;"></i>
                <i ID="ImgPlusDown" runat="server" class="mdi-hardware-keyboard-arrow-down green-text" style="font-size: 2rem !important; font-weight: bold !important;"></i>--%>
            </h4>
        </div>
    </div>
</div>
