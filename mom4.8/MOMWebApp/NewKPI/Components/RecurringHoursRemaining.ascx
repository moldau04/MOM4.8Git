<%@ Control Language="C#" AutoEventWireup="true" Inherits="Demo_Components_RecurringHoursRemaining" Codebehind="RecurringHoursRemaining.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div id="RecurringHoursRemainingChartContent" runat="server">
    <div style="overflow: hidden">
        <div class="card-move-up">
            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
                <telerik:RadHtmlChart runat="server" ID="RecurringHoursRemainingChart" Height="437px" Transitions="true" Skin="Silk" EnableViewState="false">
                    <Pan Enabled="true" />
                    <Zoom Enabled="true">
                        <MouseWheel Enabled="true" Lock="Y" />
                        <Selection Enabled="true" Lock="Y" ModifierKey="Shift" />
                    </Zoom>

                    <PlotArea>
                        <Series>
                            <telerik:ColumnSeries Name="Total Hours" DataFieldY="TotalHours">
                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0} hours" />
                                <LabelsAppearance Visible="false">
                                </LabelsAppearance>
                            </telerik:ColumnSeries>
                            <telerik:ColumnSeries Name="Completed" DataFieldY="Completed">
                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0} hours" />
                                <LabelsAppearance Visible="false">
                                </LabelsAppearance>
                            </telerik:ColumnSeries>
                            <telerik:ColumnSeries Name="Open" DataFieldY="Open">
                                <TooltipsAppearance BackgroundColor="Orange" DataFormatString="{0} hours" />
                                <LabelsAppearance Visible="false">
                                </LabelsAppearance>
                            </telerik:ColumnSeries>
                        </Series>
                        <XAxis AxisCrossingValue="0" Color="Black" MajorTickType="Outside" MinorTickType="Outside" DataLabelsField="Category" MinValue="0" MaxValue="6"
                            Reversed="false">                          
                        </XAxis>
                        <YAxis AxisCrossingValue="0" Color="Black" MajorTickSize="1" MajorTickType="Outside"
                            MinorTickSize="1" MinorTickType="Outside" Reversed="false">
                            <TitleAppearance Text="Hours">
                                <TextStyle Margin="5" FontSize="15"/>
                            </TitleAppearance>
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
        </div>
    </div>
</div>
