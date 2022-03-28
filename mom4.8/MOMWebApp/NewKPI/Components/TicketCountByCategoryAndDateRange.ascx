<%@ Control Language="C#" AutoEventWireup="true" Inherits="TicketCountByCategoryAndDateRange" CodeBehind="TicketCountByCategoryAndDateRange.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Panel runat="server" ID="SelectCompanyContent" Style="display: inline-block; margin-bottom: 10px;" CssClass="ticket-count-filter">
    <div class="srchtitle">
        Category:
    </div>
    <div class="srchinputwrap">
        <telerik:RadComboBox ID="SelectCategoryDd" Width="200" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
            CssClass="browser-default selectst category-dropdown" AutoPostBack="True" DropDownAutoWidth="Enabled" OnClientDropDownClosed="OnClientDropDownClosedHandler">
        </telerik:RadComboBox>
    </div>

    <div class="srchtitle">
        Date Range:
    </div>
    <div class="srchinputwrap">
        <telerik:RadComboBox ID="FilterDateRange" Width="130" runat="server" DropDownAutoWidth="Enabled"
            CssClass="browser-default selectst top-daterange-filter" AutoPostBack="True" OnSelectedIndexChanged="FilterDateRange_SelectedIndexChanged">
            <Items>
                <telerik:RadComboBoxItem Value="7" Text="Last 7 days" />
                <telerik:RadComboBoxItem Value="14" Text="Last 14 days" />
                <telerik:RadComboBoxItem Value="30" Text="Last 30 days" />
                <telerik:RadComboBoxItem Value="60" Text="Last 60 days" />
                <telerik:RadComboBoxItem Value="90" Text="Last 90 days" />
                <telerik:RadComboBoxItem Value="180" Text="Last 180 days" />
                <telerik:RadComboBoxItem Value="365" Text="Last 365 days" />
            </Items>
        </telerik:RadComboBox>
    </div>
</asp:Panel>

<div id="TicketCountByCategoryAndDateRangeContent" runat="server">
    <div style="overflow: hidden">
        <div class="card-move-up">
            <telerik:RadHtmlChart runat="server" ID="TicketCountByCategoryAndDateRangeChart" Height="390px" Width="100%" Skin="Silk" EnableViewState="false">
                <Pan Enabled="true" />
                <Zoom Enabled="true" >
                    <MouseWheel Enabled="true" Lock="Y" />
                    <Selection Enabled="true" Lock="Y" ModifierKey="Shift" />
                </Zoom>
                <PlotArea>
                    <Series>
                        <telerik:ColumnSeries DataFieldY="Count">
                            <TooltipsAppearance Color="White" DataFormatString="{0} ticket(s)" />
                        </telerik:ColumnSeries>
                    </Series>
                    <XAxis DataLabelsField="Category">
                        <TitleAppearance Text="Category">
                            <TextStyle Margin="20" />
                        </TitleAppearance>
                        <MajorGridLines Visible="false" />
                    </XAxis>
                    <YAxis>
                        <TitleAppearance Text="Ticket Count">
                            <TextStyle Margin="5" FontSize="15" />
                        </TitleAppearance>
                        <MinorGridLines Visible="false" />
                    </YAxis>
                </PlotArea>
                <ChartTitle Text="">
                    <Appearance Position="Bottom" Visible="true">
                        <TextStyle FontSize="13" />
                    </Appearance>
                </ChartTitle>
            </telerik:RadHtmlChart>
        </div>
    </div>
</div>
