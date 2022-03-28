<%@ Page Language="C#" MasterPageFile="~/MOM.master" AutoEventWireup="true" Inherits="CustomReports" Codebehind="CustomReports.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register assembly="Stimulsoft.Report.Web" namespace="Stimulsoft.Report.Web" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <div class="divbutton-container">
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">
                    <header>
                        <div class="container row-color-grey">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <div class="page-title">
                                            <i class="mdi-social-people"></i>&nbsp;
                                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Locations Custom Report</asp:Label>
                                        </div>
                                        
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click">
                                                <i class="mdi-content-clear"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </header>
                </div>
            </div>
        </div>
    </div>
        <div class="row" style="margin-bottom:10px;background:#fff;padding:20px 10px;">                                                            
            <div class="row"> 
                <div class="col-lg-2" style="float:left;">
                    <label>Select &ensp;</label>
                    <telerik:RadDropDownList runat="server" ID="drpModule" Visible="false" OnSelectedIndexChanged="drpModule_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:DropDownListItem Text="Select One" />
                            <telerik:DropDownListItem Text="Customers" />
                            <telerik:DropDownListItem Text="Locations" Selected="true" />
                        </Items>
                    </telerik:RadDropDownList>
                </div>
                <%--<div class="col-lg-2" style="float:left;">
                    <telerik:RadListBox RenderMode="Auto" runat="server" ID="lstParent" Height="130px" Width="230px" AutoPostBack="true"
                        ButtonSettings-AreaWidth="35px" Visible="false" OnSelectedIndexChanged="lstParent_SelectedIndexChanged">               
                    </telerik:RadListBox>
                </div>--%>
                <div class="col-lg-2" style="float:left;">
                    <telerik:RadListBox RenderMode="Auto" runat="server" ID="lstChild" Height="130px" Width="230px" AutoPostBack="true" SelectionMode="Multiple"
                         ButtonSettings-AreaWidth="35px" Visible="true" OnSelectedIndexChanged="lstChild_SelectedIndexChanged">
                    </telerik:RadListBox>
                </div>
                <div class="col-lg-2" style="float:left;">
                     <telerik:RadListBox RenderMode="Auto" runat="server" ID="lstColumns" Height="130px" Width="230px" SelectionMode="Multiple" AutoPostBack="true"
                         ButtonSettings-AreaWidth="35px" Visible="false" OnSelectedIndexChanged="lstColumns_SelectedIndexChanged">
                    </telerik:RadListBox>
                </div>
                <div class="col-lg-2" style="float:left;">
                    <asp:LinkButton ID="lnkSearch"  CssClass="btn submit" CommandName="GenerateBudget" OnClick="lnkSearch_Click" runat="server" Visible="true">
                        <i class="fa fa-search"></i>
                    </asp:LinkButton>
                </div>  
                <div class="col-lg-2" style="float:left;width:11%;margin-left: 6px;margin-top: 6px;">
                    <asp:LinkButton runat="server" ID="lnkStartOver" OnClick="lnkStartOver_Click" Text="Start Over" Visible="false" />
                </div>
                <div class="col-lg-2" style="float:left;"></div>
            </div> 
            <div runat="server" id="CriteriaPanel" class="row" style="margin-top:10px;margin-bottom:20px;">
                <div class="col-lg-2" style="float:left;margin-left:40px;width:15%;">
                    <asp:Label runat="server" ID="lblSelect" Text="Select Criteria" />
                    <telerik:RadDropDownList runat="server" ID="drpSelectedColumns" OnSelectedIndexChanged="drpSelectedColumns_SelectedIndexChanged" AutoPostBack="true" >
                    </telerik:RadDropDownList>
                </div>
                <div class="col-lg-2" style="float:left;width:15%; ">
                    <telerik:RadDropDownList runat="server" ID="drpFilter" OnSelectedIndexChanged="drpFilter_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:DropDownListItem Text="Select One" />
                            <telerik:DropDownListItem Text="Contains" />
                            <telerik:DropDownListItem Text="Equals" />
                        </Items>
                    </telerik:RadDropDownList>
                </div>
                <div class="col-lg-2" style="float:left;width:15%;">
                    <asp:TextBox runat="server" ID="txtfilterText" CssClass="form-control input-sm input-small" Height="32px" />
                </div>
                <div class="col-lg-2" style="float:left;width:5%;margin-top: 6px;">
                    <asp:LinkButton runat="server" ID="lnkFilter" OnClick="lnkFilter_Click" Text="Filter" />
                </div>
                <div class="col-lg-2" style="float:left;">
                     <telerik:RadListBox RenderMode="Auto" runat="server" ID="lstMemo" Height="130px" Width="230px" SelectionMode="Multiple" 
                         ButtonSettings-AreaWidth="35px" Visible="false">
                    </telerik:RadListBox>
                </div>
                 <div class="col-lg-2" style="float:left;width:11%;margin-left: 6px;margin-top: 6px;">
                    <asp:LinkButton runat="server" ID="lnkDelete" OnClick="lnkDelete_Click" Text="Remove Criteria" Visible="false" />
                </div>
            </div>
             
            <asp:Repeater Visible="false" ID="ConditionsRepeater" runat="server" OnItemDataBound="ConditionsRepeater_ItemDataBound" OnItemCommand="ConditionsRepeater_ItemCommand">
                <ItemTemplate>
                <div class="row" style="margin-top:10px;margin-bottom:20px;">
                <div class="col-lg-2" style="float:left;margin-left:40px;width:18%;">
                    <asp:Label runat="server" ID="lblSelect" Text="Select Criteria" />
                    <telerik:RadDropDownList runat="server" ID="drpSelectedColumns" OnSelectedIndexChanged="drpSelectedColumns_SelectedIndexChanged" AutoPostBack="true" Visible="false">
                    </telerik:RadDropDownList>
                </div>
                <div class="col-lg-2" style="float:left;width:11%;">
                    <telerik:RadDropDownList runat="server" ID="drpFilter" OnSelectedIndexChanged="drpFilter_SelectedIndexChanged" AutoPostBack="true" >
                        <Items>
                            <telerik:DropDownListItem Text="Select One" />
                            <telerik:DropDownListItem Text="Contains" />
                            <telerik:DropDownListItem Text="Equals" />
                        </Items>
                    </telerik:RadDropDownList>
                </div>
                <div class="col-lg-2" style="float:left;width:11%;">
                    <asp:TextBox runat="server" ID="txtfilterText" CssClass="form-control input-sm input-small" Height="32px" />
                </div>
                <div class="col-lg-2" style="float:left;width:11%;margin-top: 6px;">
                    <asp:LinkButton runat="server" ID="lnkFilter" CommandName="Filter"  Text="Add" />
                </div>
            </div>
                    </ItemTemplate>
                </asp:Repeater>
            <cc1:StiWebViewer ID="StiWebViewerCustomReport" runat="server" ScrollbarsMode="true" Visible="false" OnGetReport="StiWebViewerCustomReport_GetReport" 
               OnGetReportData="StiWebViewerCustomReport_GetReportData" />
        </div>
           
        

        <telerik:RadAjaxManager ID="RadAjaxManager_Budget" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="drpModule" EventName="SelectedIndexChanged">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lstChild"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="ConditionsRepeater" EventName="ItemCommand">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ConditionsRepeater"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lstParent" EventName="SelectedIndexChanged">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lstChild"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lstChild" EventName="SelectedIndexChanged">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lstColumns"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSearch" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblSelect"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpSelectedColumns"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpSelectedColumns" EventName="SelectedIndexChanged">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpFilter"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpFilter" EventName="SelectedIndexChanged">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtfilterText"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lnkFilter"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkFilter" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="StiWebViewerCustomReport"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        </telerik:RadAjaxManager>

</asp:Content>