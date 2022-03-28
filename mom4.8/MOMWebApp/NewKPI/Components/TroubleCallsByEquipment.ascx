<%@ Control Language="C#" AutoEventWireup="true" Inherits="NewKPI_Components_TroubleCallsByEquipment" Codebehind="TroubleCallsByEquipment.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Panel runat="server" ID="SelectCompanyContent" style="display: inline-block; margin-bottom: 10px;" CssClass="trouble-calls-filter">
    <div class="srchtitle">
        Category:
    </div>
    <div class="srchinputwrap">
        <telerik:RadComboBox ID="SelectCategoryDd" Width="135" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
            CssClass="browser-default selectst category-dropdown" AutoPostBack="True" DropDownAutoWidth="Enabled" OnClientDropDownClosed="OnClientDropDownClosedHandler">           
        </telerik:RadComboBox>
    </div>

    <div class="srchtitle">
        Filter:
    </div>
    <div class="srchinputwrap">
        <telerik:RadComboBox ID="FilterTopAndDays" Width="240" runat="server" DropDownAutoWidth="Enabled"
            CssClass="browser-default selectst top-date-filter" AutoPostBack="True" OnSelectedIndexChanged="FilterTopAndDays_SelectedIndexChanged">  
             <%--<Items>
                <telerik:RadComboBoxItem Value="530" Text="5 trouble calls in the past 30 days" />
                <telerik:RadComboBoxItem Value="590" Text="5 trouble calls in the past 90 days" />
                <telerik:RadComboBoxItem Value="10180" Text="10 trouble calls in the past 180 days" />
                <telerik:RadComboBoxItem Value="20360" Text="20 trouble calls in the past 360 days" />
            </Items>--%>
        </telerik:RadComboBox>
    </div>
</asp:Panel>

<div id="TroubleCallsByEquipmentContent" runat="server">
    <div style="overflow: hidden">
        <div class="card-move-up">
            <telerik:RadScriptBlock runat="server">
                <script type="text/javascript">
                    function OnClientDropDownClosedHandler(sender, eventArgs) {
                        document.getElementById("<%= btnLoadData.ClientID %>").click();
                    }
                </script>
            </telerik:RadScriptBlock>
            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" EnableViewState="false" CssClass="RadGrid_Material trouble-calls-equipment">
                <telerik:RadGrid RenderMode="Lightweight" ID="TroubleCallsEquipmentGrid" AllowFilteringByColumn="true" ShowFooter="True" Height="390px"
                    PagerStyle-AlwaysVisible="true" OnNeedDataSource="TroubleCallsEquipmentGrid_NeedDataSource"
                    ShowStatusBar="true" runat="server" AllowPaging="False" AllowSorting="true"  AllowCustomPaging="False">
                    <CommandItemStyle />
                    <GroupingSettings CaseSensitive="false" />
                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                        <Columns>
                           <telerik:GridBoundColumn FilterDelay="5" DataField="Unit" HeaderText="Equipment Name" SortExpression="Count" HeaderStyle-Wrap="true" HeaderStyle-Width="20%"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterDelay="5" DataField="Count" HeaderText="Count" SortExpression="Count"  HeaderStyle-Width="15%"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterDelay="5" DataField="Type" HeaderText="Type" SortExpression="Count" HeaderStyle-Width="15%"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterDelay="5" DataField="Tag" HeaderText="Location" SortExpression="Count"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterDelay="5" DataField="CType" HeaderText="Contract Type" SortExpression="Count"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterDelay="5" DataField="ContractNumber" HeaderText="Contract #" SortExpression="Count"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    </telerik:RadGrid>
            </telerik:RadAjaxPanel>
            <asp:Button runat="server" ID="btnLoadData" Text="" style="display:none;" OnClick="btnLoadData_Click" />
        </div>
    </div>
</div>