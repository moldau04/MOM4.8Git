<%@ Control Language="C#" AutoEventWireup="true" Inherits="NewKPI_Components_MonthlyRevenueByDeptChart" Codebehind="MonthlyRevenueByDeptChart.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Panel runat="server" ID="SelectCompanyContent" style="display: inline-block;">
    <div class="srchtitle" style="padding-left: 15px;">
        Select Company:
    </div>
    <div class="srchinputwrap">
        <asp:DropDownList ID="SelectCompanyDd" Width="210" runat="server"
            CssClass="browser-default selectst" AutoPostBack="True" OnSelectedIndexChanged="SelectCompanyDd_SelectedIndexChanged">           
        </asp:DropDownList>
    </div>
</asp:Panel>

<div id="MonthlyRevenueByDeptChartContent" runat="server">
    <div style="overflow: hidden">
        <div class="card-move-up">
            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" EnableViewState="false">
                <telerik:RadHtmlChart runat="server" ID="MonthlyRevenueByDeptChart" Height="400px" Transitions="true" Skin="Silk">
                    <PlotArea>
                    <Series>
                        <telerik:DonutSeries DataFieldY="Revenue" NameField="Department">
                            <LabelsAppearance Position="Center" DataFormatString="{0:C2}" Visible="true" Color="White"></LabelsAppearance>
                            <TooltipsAppearance Color="White">
                                <ClientTemplate>
                                    #=dataItem.Department#: #= kendo.format(\'{0:C2}\', dataItem.Revenue)#
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
                    <Appearance Position="Right" Visible="true" Align="Start" Orientation="Horizontal" Width="150">
                        <TextStyle FontSize="18" />
                        <%--<ClientTemplate>
                             #=wrapText(text)#: #=value#
                        </ClientTemplate>--%>
                    </Appearance>
                </Legend>
                </telerik:RadHtmlChart>
                <telerik:RadLabel ID="TotalRevenueLabel" Text="" runat="server" CssClass="total-revenue-label"></telerik:RadLabel>
            </telerik:RadAjaxPanel>
        </div>
    </div>
</div>