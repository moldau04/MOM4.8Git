<%@ Page Title="Home Demo || MOM" Language="C#" MasterPageFile="~/MOM.master" AutoEventWireup="true" Inherits="HomeDemo" Codebehind="HomeDemo.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="NewKPI/Style/style.css" type="text/css" rel="stylesheet" media="screen,projection">
    <script>        
        function wrapText(value){
            if (value.length > 14){
                value = value.replace(" ", "\n");
            };
            return value;
        };

        function enterEvent(e) {
            if (e.keyCode == 13) {
                $('#<%=btnSaveDashboardName.ClientID%>').click();
            }
        }

        $(document).mouseup(function (e) {
            var container = $(".graph-list");

            // if the target of the click isn't the container nor a descendant of the container
            if (!container.is(e.target) && container.has(e.target).length === 0) {
                container.hide();
            }
        });

        $(document).ready(function(){
           $('#<%=txtDashboardName.ClientID%>').dblclick(function() {
               $('#<%=txtDashboardName.ClientID%>').removeAttr("readonly");
            });
        });

        dataviz = kendo.dataviz;
        deepExtend = kendo.deepExtend;
        ShapeElement = dataviz.ShapeElement;
        kendo.dataviz.LegendItem.fn.createMarker = function () {
                    var item = this,
                        options = item.options,
                        markerColor = options.markerColor,
                        markers = options.markers,
                        markerType = "rect", // "triangle" "circle" "rect"
                        markerWidth = 18,
                        markerHeight = 18,
                        markerMargin = { top: 0, left: 10, right: 0, bottom: 0 };

            if (options.series.legend) {
                if (options.series.legend.marker) {
                    if (options.series.legend.marker.type) {
                        markerType = options.series.legend.marker.type;
                    }
                    if (options.series.legend.marker.width) {
                        markerWidth = options.series.legend.marker.width;
                    }
                    if (options.series.legend.marker.height) {
                        markerHeight = options.series.legend.marker.height;
                    }
                    if (options.series.legend.marker.margin) {
                        markerMargin = options.series.legend.marker.margin;
                    }
                }
            }

            var markerOptions = deepExtend({}, markers, {
                background: markerColor,
                border: {
                    color: markerColor
                },
                type: markerType,
                width: markerWidth,
                height: markerHeight,
                margin: markerMargin,
                vAlign: "start"
            });

            item.container.append(new ShapeElement(markerOptions));
        }
    </script>
    <section id="content">
        <!--start container-->
        <div class="container">
            <div class="row">
                <!--KPI dashboard start-->
                <div class="col s12 m12 l8 dashboard-main">

                    <asp:UpdatePanel runat="server" ID="ConfigurationPanel1" EnableViewState="true">
                        <ContentTemplate>
                            <div class="dashboard-nav">

                                <%--<h4 class="dashboard-title">KPI</h4>--%>
                                <telerik:RadTextBox  ID="txtDashboardName" runat="server" MaxLength="100" CssClass="dashboard-title" ReadOnly="true" onkeypress="enterEvent(event)"></telerik:RadTextBox>
                                <asp:Button ID="btnSaveDashboardName" runat="server" style="display:none" OnClick="btnSaveDashboardName_Click" />

                                <div class="select-graph">
                                    <telerik:RadComboBox RenderMode="Auto" ID="RadComboBox1" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="250" DropDownAutoWidth="Enabled" CssClass="select-graph-combobox" 
                                        AutoPostBack="true" OnItemChecked="RadComboBox1_OnItemChecked" OnCheckAllCheck="RadComboBox1_CheckAllCheck">
                                        <%--<Items>
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/Contents/OneTwentyDayAccountsReceivable.ascx" Text="120+ Accounts Receivable" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/Contents/NinetyDayAccountsReceivable.ascx" Text="90+ Accounts Receivable" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/Contents/SixtyDayAccountsReceivable.ascx" Text="60+ Accounts Receivable" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/Contents/AvgEstimateConversionRate.ascx" Text="Avg Estimate Conversion Rate" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/EquipmentTypeChart.ascx" Text="Equipment by Type" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/EquipmentBuildingChart.ascx" Text="Equipment By Building" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/RecurringHoursChart.ascx" Text="Converted Estimates By Salesperson Avg. Days" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/TicketRecurringChart.ascx" Text="Monthly Default Recurring Open vs Completed" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/ActualBudgetedRevenueChart.ascx" Text="Actual vs Budgeted Revenue" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/RecurringHoursRemaining.ascx" Text="Recurring Hours Remaining for Current Month by Route" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/MonthlyRevenueByDeptChart.ascx" Text="Monthly Revenue by Department" />
                                            <telerik:RadComboBoxItem Value="~/NewKPI/Components/TroubleCallsByEquipment.ascx" Text="Trouble Calls by Equipment" />
                                        </Items>--%>
                                    </telerik:RadComboBox>
                                </div>
                            </div>

                            <div class="dashboard-content row">
                                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" CssClass="col s12 m12 l12">
                                    <telerik:RadDockLayout runat="server" ID="RadDockLayout1" OnSaveDockLayout="RadDockLayout1_SaveDockLayout"
                                        OnLoadDockLayout="RadDockLayout1_LoadDockLayout">
                                        <%--<telerik:RadDockZone runat="server" ID="RadDockZoneContent" CssClass="row" Orientation="Horizontal">
                                        </telerik:RadDockZone>--%>
                                        <telerik:RadDockZone runat="server" ID="RadDockZoneGraph" MinHeight="500" CssClass="row" Orientation="Horizontal">
                                        </telerik:RadDockZone>
                                    </telerik:RadDockLayout>
                                </telerik:RadAjaxPanel>

                                <div class="hidden-container">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSaveDashboardName" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
                <!--KPI dashboard end-->

                <div class="col s12 m12 l4">
                </div>
            </div>
        </div>
        <!--end container-->
    </section>

</asp:Content>
