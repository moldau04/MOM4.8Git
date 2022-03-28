<%@ Page Language="C#" MasterPageFile="~/MOM.master" AutoEventWireup="true" Inherits="ReportDesignerLocations" Codebehind="ReportDesignerLocations.aspx.cs" %>

<%@ Register assembly="Stimulsoft.Report.Web" namespace="Stimulsoft.Report.Web" tagprefix="cc1" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function openForm() {
            window.radopen(null, "RadCreateWindow");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadCreateWindow" runat="server" Modal="true" Width="550" Height="460">
                <ContentTemplate>
                    <div class="budget-customize">
                        <div>

                            <p style="font-size: medium; font-weight: bold; text-align: center; margin-top: 10px;">Choose Columns for Report</p>
                            <br />
                            <br />
                            
                            <div style="margin-top: 10px; padding-left: 30px;">
                                <div>Select : </div>
                                <div>
                                    <telerik:RadListBox RenderMode="Auto" runat="server" ID="FromColumn" Height="200px" Width="230px"
                AllowTransfer="true" TransferToID="ToColumn" TransferMode="Move" 
                 ButtonSettings-AreaWidth="35px" >
                <Items>
                    <telerik:RadListBoxItem Text="Customer" Value="Customer"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="Location" Value="Location"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="Address" Value="Address"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="City" Value="City"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="State" Value="State"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="Zip" Value="Zip"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="Status" Value="Status"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="Type" Value="Type"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="LocationSTax" Value="LocationSTax"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="EquipmentCounts" Value="EquipmentCounts"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="Balance" Value="Balance"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="SalesPerson" Value="SalesPerson"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="DefaultWorker" Value="DefaultWorker"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="PreferredWorker" Value="PreferredWorker"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="TaxDesc" Value="TaxDesc"></telerik:RadListBoxItem>
                    <telerik:RadListBoxItem Text="TaxRate" Value="TaxRate"></telerik:RadListBoxItem>
                </Items>
            </telerik:RadListBox>
            <telerik:RadListBox RenderMode="Auto" runat="server" ID="ToColumn" Height="200px" Width="230px"
                 ButtonSettings-AreaWidth="35px" >
            </telerik:RadListBox>
                                </div>
                                <br />
                                
                                <div style="clear:both;"></div>
                            </div>
                            <div style="text-align: center; font-size: small; font-weight: bold; margin-top: 15px;">
                                <asp:LinkButton ID="lnkSearch" CommandName="GenerateBudget" runat="server" OnClick="lnkSearch_Click">Load</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div class="page-cont-top">
        <div class="page-bar-right">
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-lg-12 col-md-12">
            <div class="pc-title">
                <asp:Panel runat="server" ID="pnlGridButtons">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Locations Report</asp:Label></li>
                        <li> <asp:LinkButton CssClass="icon-closed" ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                    OnClick="lnkClose_Click"></asp:LinkButton></li>
                    </ul>
                </asp:Panel>
            </div>
        </div>
        <div class="col-lg-12 col-md-12">
            <div class="com-cont">
                <div>
                    <div class="title_bar">
                        <div id="divSpace" class="Close_button">
                        </div>

                    </div>
                    <div class="search-customer">
                      
                              
                        <asp:Panel class="sc-form" style="float: left; margin-right: 0px; font-size: 13px; font-weight: bold; width:900px;" ID="budgetSavePanel" runat="server"  >
                            
                     <asp:LinkButton ID="btnCreateBudget" runat="server" Text="Select Criteria" OnClientClick="openForm();return false" />&ensp;
                        <asp:DropDownList ID="drpReportType" runat="server" OnSelectedIndexChanged="drpReportType_SelectedIndexChanged"
                            AutoPostBack="true" Width="120px" CssClass="form-control input-sm input-small">
                            <asp:ListItem Text="-- Select Report Type --" Selected="True" Value="0"></asp:ListItem>
                            <asp:ListItem Text="All Columns" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Selected Columns" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                            <%--<div style="float:left;width:55%;margin-top: 15px;">
                        From Date:
                                  <asp:TextBox ID="txtFromDt" runat="server" Width="150px" CssClass="form-control" MaxLength="20" OnTextChanged="txtFromDt_TextChanged" AutoPostBack="true"
                                                        TabIndex="4"></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtFromDt_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtFromDt">
                                                    </asp:CalendarExtender>
                            <telerik:RadDatePicker ID="datePickerFromDate" runat="server" Visible="false" AutoPostBack="true" OnSelectedDateChanged="datePickerFromDate_SelectedDateChanged" DisplayDateFormat="MM/dd/yyyy"></telerik:RadDatePicker>
                        To Date: 
                        <telerik:RadDatePicker ID="datePickerToDate" runat="server" Visible="false" AutoPostBack="true" OnSelectedDateChanged="datePickerToDate_SelectedDateChanged" DisplayDateFormat="MM/dd/yyyy"></telerik:RadDatePicker>
                          <asp:TextBox ID="txtToDt" runat="server" Width="150px" CssClass="form-control" MaxLength="20" OnTextChanged="txtToDt_TextChanged" AutoPostBack="true"
                                                        TabIndex="4"></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtToDt_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtToDt">
                                                    </asp:CalendarExtender>
                                 </div>
                            <div style="float:left;width:45%;">
                                 &nbsp; <asp:DropDownList ID="drpBudgetsList" Visible="false" runat="server" OnSelectedIndexChanged="drpBudgetsList_SelectedIndexChanged" AutoPostBack="true" Width="120px" CssClass="form-control input-sm input-small"></asp:DropDownList>
                            </div>--%>
                        </asp:Panel>
                     
                        </div>
                   <%-- </div>--%>
                    
                    <div class="search-customer">
                        
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                        </div>

                    <div class="clearfix">
                    </div>
                    <div class="table-scrollable" style="padding-top: 15px; border: none">

                        
                        <input id="Hidden1" name="Hidden1" runat="server" type="hidden" /> 
                    </div>
                </div>
            </div>
        </div>
    </div>
 
    <cc1:StiWebViewer ID="StiWebViewerLocations" runat="server" ScrollbarsMode="true" Visible="true" OnGetReport="StiWebViewerLocations_GetReport" OnGetReportData="StiWebViewerLocations_GetReportData" />

      
</asp:Content>
