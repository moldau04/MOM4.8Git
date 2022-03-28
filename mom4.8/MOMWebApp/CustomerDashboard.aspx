<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="CustomerDashboard" Codebehind="CustomerDashboard.aspx.cs" %>
<%@ Register Src="~/KPI/CMKPI1.ascx" TagPrefix="uc" TagName="EquipmentKPI" %>  
<%@ Register Src="~/KPI/EquipmentByBuilding.ascx" TagPrefix="uc" TagName="EquipmentByBuildingKPI" %>  
<%@ Register Src="~/KPI/EquipmentStatusHistory.ascx" TagPrefix="uc" TagName="EquipmentStatusHistory" %>  
<%@ Register Src="~/KPI/SixtyPlusAR.ascx" TagPrefix="uc" TagName="SixtyPlusAR" %>  
<%@ Register Src="~/KPI/NinetyPlusAR.ascx" TagPrefix="uc" TagName="NinetyPlusAR" %>  
<%@ Register Src="~/KPI/ActualvsBudgetedRevenue.ascx" TagPrefix="uc" TagName="ActualvsBudgetedRevenueKPI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col s12">
        <div class="row">
            <div class="col s4" style="align-content: center;">
                <uc:EquipmentKPI runat="server"/>                
            </div>
            <div class="col s4">
                <uc:EquipmentByBuildingKPI runat="server" />              
            </div>
            <div class="col s4">               
                <uc:EquipmentStatusHistory runat="server" />
            </div>
            <div class="col s4">               
                <uc:ActualvsBudgetedRevenueKPI runat="server" />
            </div>
        </div>        
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
</asp:Content>

