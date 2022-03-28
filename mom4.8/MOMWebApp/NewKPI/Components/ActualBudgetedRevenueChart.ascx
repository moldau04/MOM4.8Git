<%@ Control Language="C#" AutoEventWireup="true" Inherits="Demo_Components_ActualBudgetedRevenueChart" Codebehind="ActualBudgetedRevenueChart.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<div style="display: inline-block;">
    <div class="srchtitle" style="padding-left: 15px;">
        Select Budget:
    </div>
    <div class="srchinputwrap">
        <asp:DropDownList ID="SelectBudgetDd" runat="server"
            CssClass="browser-default selectsml selectst" AutoPostBack="True" OnSelectedIndexChanged="SelectBudgetDd_SelectedIndexChanged">           
        </asp:DropDownList>
    </div>
</div>

<div id="ActualBudgetedRevenueChartContent" runat="server">
    <div style="overflow: hidden">
        <div class="card-move-up">
            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
                <telerik:RadHtmlChart runat="server" ID="ActualBudgetedRevenueChart" Height="400px" Transitions="true" Skin="Silk" EnableViewState="false">
                    <PlotArea>
                        <Series>
                            <telerik:ColumnSeries Name="Actual Revenues" DataFieldY="NTotal">
                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0:C}" />
                                <LabelsAppearance Visible="false">
                                </LabelsAppearance>
                            </telerik:ColumnSeries>
                            <telerik:ColumnSeries Name="Budgeted Revenues" DataFieldY="NBudget">
                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0:C}" />
                                <LabelsAppearance Visible="false">
                                </LabelsAppearance>
                            </telerik:ColumnSeries>
                        </Series>
                        <XAxis AxisCrossingValue="0" Color="Black" MajorTickType="Outside" MinorTickType="Outside" DataLabelsField="NMonth"
                            Reversed="false">
                        </XAxis>
                        <YAxis AxisCrossingValue="0" Color="Black" MajorTickSize="1" MajorTickType="Outside"
                            MinorTickSize="1" MinorTickType="Outside" Reversed="false">
                            <TitleAppearance Position="Center" RotationAngle="0" Text="" />
                            <LabelsAppearance DataFormatString="{0:N0}">
                            </LabelsAppearance>
                        </YAxis>
                    </PlotArea>
                    <ChartTitle Text="">
                    </ChartTitle>
                    <Legend>
                        <Appearance Position="Bottom">
                            <TextStyle FontSize="13" Bold="true"/>
                        </Appearance>
                    </Legend>
                </telerik:RadHtmlChart>
            </telerik:RadAjaxPanel>

            <%--<telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
            </telerik:RadAjaxLoadingPanel>--%>
        </div>
    </div>
</div>
