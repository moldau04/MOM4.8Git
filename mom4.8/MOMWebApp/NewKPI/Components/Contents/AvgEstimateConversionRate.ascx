<%@ Control Language="C#" AutoEventWireup="true" Inherits="Demo_Components_Contents_AvgEstimateConversionRate" Codebehind="AvgEstimateConversionRate.ascx.cs" %>

<div class="kpi-card">
    <div class="card-content white white-text" style="min-height: 140px;">
        <div class="editbtns-sml">
            <a class="btn-floating waves-effect waves-light blue darken-2 right"><i class="mdi-content-add activator" style="color: #fff !important;"></i></a>
        </div>
        <div class="blue-text text-darken-2">
            <p class="card-stats-title icn-stats"><i class="mdi-action-description"></i></p>
            <p class="card-stats-title"><span class="blue-text text-darken-2" style="font-size: 0.8em !important">Avg Estimate Conversion Rate (Days)</span></p>
            <h4 class="card-stats-number">
                <span ID="AvgEstimateValue" runat="server"></span>
                <i ID="ImgUpAvgEstimate" runat="server" class="mdi-hardware-keyboard-arrow-up green-text" style="font-size: 2rem !important; font-weight: bold !important;"></i>
                <i ID="ImgDownAvgEstimate" runat="server" class="mdi-hardware-keyboard-arrow-down red-text" style="font-size: 2rem !important; font-weight: bold !important;"></i>
            </h4>
        </div>
    </div>
</div>