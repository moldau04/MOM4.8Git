<%@ Control Language="C#" AutoEventWireup="true" Inherits="Demo_Components_EquipmentTypeChart" Codebehind="EquipmentTypeChart.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="" id="EquipmentTypeChartContent" runat="server">
    <div style="overflow: hidden">
        <div class="card-move-up">
            <telerik:RadHtmlChart runat="server" ID="EquipmentTypeChart" Width="100%" Height="437px" Skin="Silk" EnableViewState="false">            
                <PlotArea>
                    <Series>
                        <telerik:DonutSeries DataFieldY="value" NameField="category">
                            <LabelsAppearance Position="OutsideEnd" Visible="true">
                                 <ClientTemplate>
                                     #=dataItem.category#: #=kendo.format(\'{0:N0}\', dataItem.value)#
                               </ClientTemplate>
                            </LabelsAppearance>
                            <TooltipsAppearance Color="White">
                                <ClientTemplate>
                                     #=dataItem.category#: #=kendo.format(\'{0:N0}\', dataItem.value)#
                               </ClientTemplate>
                            </TooltipsAppearance>
                        </telerik:DonutSeries>
                    </Series>
                </PlotArea>
                <ChartTitle Text="">
                    <Appearance Align="Center" Position="Top">
                    </Appearance>
                </ChartTitle>
                <Legend>
                    <Appearance Position="Right" Visible="false" Align="Start" Orientation="Horizontal" Width="150">
                        <TextStyle FontSize="18" />
                        <ClientTemplate>
                             #=wrapText(text)#: #=kendo.format(\'{0:N0}\', dataItem.value)#
                        </ClientTemplate>
                    </Appearance>
                </Legend>
            </telerik:RadHtmlChart>
            <telerik:RadLabel ID="TotalEquipmentLabel" Text="" runat="server" CssClass="total-equipment-label"></telerik:RadLabel>
        </div>
    </div>
</div>

