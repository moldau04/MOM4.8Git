<%@ Control Language="C#" AutoEventWireup="true" Inherits="Demo_Components_RecurringHoursChart" Codebehind="RecurringHoursChart.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div id="RecurringHoursChartContent" runat="server">
    <div style="overflow:hidden">
        <div class="card-move-up">
            <telerik:RadHtmlChart runat="server" ID="RecurringHoursChart" Height="437px" Width="100%" Skin="Silk" EnableViewState="false">
                <PlotArea>
                    <Series>
                        <telerik:ColumnSeries DataFieldY="Avg">
                            <TooltipsAppearance Color="White" DataFormatString="{0} hours" />
                        </telerik:ColumnSeries>
                    </Series>
                    <XAxis DataLabelsField="SalesPerson">
                        <TitleAppearance Text="">
                            <TextStyle Margin="20" />
                        </TitleAppearance>
                        <MajorGridLines Visible="false" />
                        <MinorGridLines Visible="false" />
                    </XAxis>
                    <YAxis>
                        <TitleAppearance Text="Hours">
                            <TextStyle Margin="5" FontSize="15"/>
                        </TitleAppearance>
                        <MinorGridLines Visible="false" />
                    </YAxis>
                </PlotArea>
                <ChartTitle Text="Current Period">
                    <Appearance Position="Bottom" Visible="true">
                        <TextStyle FontSize="13"/>
                    </Appearance>
                </ChartTitle>
            </telerik:RadHtmlChart>
        </div>
    </div>
</div>
