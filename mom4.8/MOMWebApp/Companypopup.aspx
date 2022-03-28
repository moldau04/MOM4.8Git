<%@ Page Language="C#" AutoEventWireup="true" Inherits="Companypopup" Codebehind="Companypopup.aspx.cs" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Company</title>
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/css/style.css" type="text/css" rel="stylesheet" media="screen,projection" />


    <script type="text/javascript">
       
        function SelectAllCheckboxes(chkboxSelectAll) {         
            var numChecked = 0;
            var grid = $find("RadGrid_Company");
            var masterTable = grid.get_masterTableView();
            for (var i = 0; i < masterTable.get_dataItems().length; i++) {

                var gridItemElement = masterTable.get_dataItems()[i].findElement("chkBranchSelect");
                gridItemElement.checked = chkboxSelectAll.checked
                if (gridItemElement.checked == true) {
                    numChecked = numChecked + 1;
                }
            }

            document.getElementById("lblRecordCount").innerHTML = "" + numChecked + "" + " Company(s) Selected.";
        }

        function CheckBoxCount(chkSelect) {
            debugger;

            var gv = document.getElementById("RadGrid_Company");
            var inputList = gv.getElementsByTagName("input");
            var numChecked = 0;
            if (inputList.length > 0) {
                for (var i = 0; i < inputList.length; i++) {
                    if (inputList[i].type == "checkbox" && inputList[i].checked) {
                        numChecked = numChecked + 1;
                    }
                    if (chkSelect.checked)
                        chkSelect.parentNode.parentNode.style.backgroundColor = "#01A1DC";
                    else
                        chkSelect.parentNode.parentNode.style.backgroundColor = "";
                }
            }
            else {
                numChecked = 0;
            }
            document.getElementById("lblRecordCount").innerHTML = "" + numChecked + "" + " Company(s) Selected.";
        }
        function RadioCheck(rb) {

            debugger;
            var gv = document.getElementById("RadGrid_Company");       
            var rbs = gv.getElementsByTagName("input");
            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }
                }
            }
        }

    </script>

    <script>
        function RefreshParentPage() {
            top.location.reload();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxManager ID="RadAjaxManager_Company" runat="server">
        </telerik:RadAjaxManager>
        <div style="margin-top: 15px;">

            <div class="form-section-row">
                <div class="btnlinks">
                    <asp:LinkButton ID="lnkSaveCompany" Style="text-decoration: none;" runat="server" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                </div>
                <div style="float: right">
                    <span class="tro trost">
                        <asp:Label ID="lblRecordCount" runat="server">0 Company(s) Selected.</asp:Label>
                    </span>
                </div>

            </div>
            <div class="form-section-row">
                <div class="grid_container">
                    <div class="form-section-row" style="margin-bottom: 0 !important;">

                        <div class="RadGrid RadGrid_Material  FormGrid">
                            <telerik:RadCodeBlock ID="RadCodeBlock13" runat="server">
                                <script type="text/javascript">
                                    function pageLoad() {
                                        var grid = $find("<%= RadGrid_Company.ClientID %>");
                                        var columns = grid.get_masterTableView().get_columns();
                                        for (var i = 0; i < columns.length; i++) {
                                            columns[i].resizeToFit(false, true);
                                        }
                                    }

                                    var requestInitiator = null;
                                    var selectionStart = null;

                                    function requestStart(sender, args) {
                                        requestInitiator = document.activeElement.id;
                                        if (document.activeElement.tagName == "INPUT") {
                                            selectionStart = document.activeElement.selectionStart;
                                        }
                                    }

                                    function responseEnd(sender, args) {
                                        var element = document.getElementById(requestInitiator);
                                        if (element && element.tagName == "INPUT") {
                                            element.focus();
                                            element.selectionStart = selectionStart;
                                        }
                                    }

                                </script>
                            </telerik:RadCodeBlock>
                            <telerik:RadAjaxPanel ID="RadAjaxPanel_Company" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Company" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Company" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                    ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                    AllowCustomPaging="True"
                                    OnNeedDataSource="RadGrid_Company_NeedDataSource" OnPreRender="RadGrid_Company_PreRender">
                                    <CommandItemStyle />
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="True" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="true" ShowFooter="True">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="40" AllowFiltering="false">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkboxBranchSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnSelected" runat="server" />
                                                    <asp:CheckBox ID="chkBranchSelect" runat="server" onclick="javascript:CheckBoxCount(this);" Checked='<%# Convert.ToBoolean(Eval("IsSel"))%>' />
                                                </ItemTemplate>

                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn FilterDelay="5" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Default" SortExpression="CompanyID" CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false">

                                                <ItemTemplate>
                                                    <asp:RadioButton ID="rbDefaultCompany" runat="server" GroupName="Common" onclick="RadioCheck(this);" />
                                                    <asp:Label runat="server" ID="lblrbDefaultCompany" AssociatedControlID="rbDefaultCompany">&nbsp;</asp:Label>
                                                    <asp:HiddenField ID="hdnDefaultCompanyID" runat="server" Value='<%#Eval("CompanyID")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="lblID" HeaderStyle-Width="50" FilterDelay="5" HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="lblRemark" HeaderStyle-Width="120" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterDelay="5" DataField="CompanyID" HeaderText="Company ID" SortExpression="CompanyID"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompanyID" runat="server" Text='<%# Bind("CompanyID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>


                                            <telerik:GridTemplateColumn UniqueName="lblName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterDelay="5" DataField="Name" HeaderText="Company" SortExpression="Name"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadAjaxPanel>
                        </div>
                    </div>
                </div>

            </div>
            <div style="clear: both;"></div>

        </div>
    </form>
</body>
</html>
