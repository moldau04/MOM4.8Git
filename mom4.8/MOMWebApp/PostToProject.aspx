<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostToProject.aspx.cs" Inherits="PostToProject"  MasterPageFile="~/Mom.master"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>--%>
    <script type="text/javascript" src="js/Signature/jquery.signaturepad.js"></script>
    <link rel="stylesheet" href="js/Signature/jquery.signaturepad.css" />
    <script src="Scripts/jquery.timeentry.min.js"></script>
    <style>
        .highlight {
            background-color: Yellow;
        }

        .highlighted {
            background-color: Yellow;
        }

        .ui-state-hover, .ui-state-active {
            text-decoration: none !important;
            background-color: transparent !important;
            border-radius: 4px !important;
            -webkit-border-radius: 4px !important;
            -moz-border-radius: 4px !important;
            background-image: none !important;
            width: 100%;
            border: none !important;
        }

        .signature {
            float: left;
            width: 100%;
            margin-top: 25px;
        }

            .signature #signbg {
                width: 100%;
                height: 100px;
                border: 1px solid #000;
                margin-top: 7px;
            }

                .signature #signbg img {
                    width: 100%;
                    height: 100%;
                }

        .sigPad {
            float: left;
            margin-top: 15px;
            width: 100%;
        }

        .sign-title {
            float: left;
            width: 100%;
            padding: 5px 10px;
            background: #316b9d;
        }

            .sign-title .sign-title-l {
                float: left;
                color: #fff;
                cursor: pointer;
            }

            .sign-title .sign-title-r {
                float: right;
                color: #fff;
                cursor: pointer;
            }

        .sigPad .pad {
            width: 100%;
        }

        .RadGrid_Material .rgHeader {
            color: #2e6b89 !important;
            font-weight: bold !important;
        }

        ul.anchor-links li a {
            border-bottom: 1px groove !important;
        }

        .dropdown-content.po-report-dropdown {
            width: auto !important;
        }

        #overlay {
            position: fixed; /* Sit on top of the page content */
            display: none; /* Hidden by default */
            width: 100%; /* Full width (cover the whole page) */
            height: 100%; /* Full height (cover the whole page) */
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5); /* Black background with opacity */
            z-index: 1000000; /* Specify a stack order in case you're using a different order for other elements */
            cursor: pointer; /* Add a pointer on hover */
        }
          input[type=text], input[type=password], input[type=email], input[type=url], input[type=time], input[type=date], input[type=datetime-local], input[type=tel], input[type=number], input[type=search], textarea.materialize-textarea
        {
            margin: 0 0 1px 0;
           height: 1.5rem;
        }
    </style>
    <script>
        function itemJSON() {
            var rawData = $('#<%=RadGrid_VendorItems.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnItemJSON.ClientID%>').val(formData);
        }
        function KeyPressed(sender, eventArgs) {
            if (eventArgs.get_keyCode() == 40) {
                document.getElementById('<%=btnAddNewLines.ClientID%>').click();
                return false;
            }
        }
        function resetIndexF6() {
            var hdnSelectedInvIndex = document.getElementById('<%=hdnSelectedInvIndex.ClientID%>');
            $(hdnSelectedInvIndex).val(-1);  
        }
        function CalTotalVal(obj) {
            var txt = obj.id;
            var txtGvQuan;

            if (txt.indexOf("Quan") >= 0) {
                txtGvQuan = document.getElementById(txt);
            }

            if (isNaN(parseFloat($(txtGvQuan).val()))) {
                $(txtGvQuan).val('0.00');
            } 

            var totalQty = 0.00;
            $("[id*=txtGvQuan]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totalQty = totalQty + parseFloat($(this).val());
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $('[id*=lblTotalQty]').text(totalQty.toFixed(2));

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        
        $(window.document).keydown(function (event) {
            if (event.which == 117) {
                document.getElementById('<%=btnCopyPrevious.ClientID%>').click();
                return false;
            }
        });

        $(document).ready(function () {
            InitializeGrids('<%=RadGrid_VendorItems.ClientID%>');
            (function ($) {
                $.extend({
                    toDictionary: function (query) {
                        var parms = {};
                        var items = query.split("&");
                        for (var i = 0; i < items.length; i++) {
                            var values = items[i].split("=");
                            var key1 = decodeURIComponent(values.shift().replace(/\+/g, '%20'));
                            var key = key1.split('$')[key1.split('$').length - 1];
                            var value = values.join("=");
                            parms[key] = decodeURIComponent(value.replace(/\+/g, '%20'));
                        }
                        return (parms);
                    }
                });
            })(jQuery);
            (function ($) {
                $.fn.serializeFormJSON = function () {
                    var o = [];
                    $(this).find('tr:not(:first, :last)').each(function () {
                        var elements = $(this).find('input, textarea, select');
                        if (elements.size() > 0) {
                            var serialized = $(this).find('input, textarea, select').serialize();
                            var item = $.toDictionary(serialized);
                            o.push(item);
                        }
                    });
                    return o;
                };
            })(jQuery);

            function InitializeGrids(Gridview) {

                var rowone = $("#" + Gridview).find('tr').eq(1);
                $("input", rowone).each(function () {
                    $(this).blur();
                });
            }
        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_gvPO" runat="server">
        <AjaxSettings>
            <%--<telerik:AjaxSetting AjaxControlID="btnAddNewLines">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_VendorItems"/>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
            <%--<telerik:AjaxSetting AjaxControlID="btnCopyPrevious">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_AddPO" LoadingPanelID="RadAjaxLoadingPanel_gvPO" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnSubmit" LoadingPanelID="RadAjaxLoadingPanel_gvPO"/>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <asp:HiddenField ID="hdnItemJSON" runat="server" />
    <asp:HiddenField runat="server" ID="hdnSelectedInvIndex" />
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-communication-contacts"></i>&nbsp;
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Post To Project</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"
                                                 OnClientClick="itemJSON();" ValidationGroup="po">Save</asp:LinkButton>
                                        </div>
                                        
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>

    <div class="container accordian-wrap">
        <div class="row">
            <div class="grid_container">
                <div class="RadGrid RadGrid_Material FormGrid">
                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                        <script type="text/javascript">
                            function pageLoad() {
                                try {
                                    var grid = $find("<%= RadGrid_VendorItems.ClientID %>");
                                    var columns = grid.get_masterTableView().get_columns();
                                    for (var i = 0; i < columns.length; i++) {
                                        columns[i].resizeToFit(false, true);
                                    }
                                } catch (e) {

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
                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvPO" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_VendorItems" AllowFilteringByColumn="false" ShowFooter="True" PageSize="50"
                            ShowStatusBar="true" runat="server" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" OnItemCommand="RadGrid_VendorItems_ItemCommand"
                            AllowCustomPaging="True" onblur="resetIndexF6()"  >
                            <CommandItemStyle />
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowKeyboardNavigation="true">
                                <Selecting AllowRowSelect="True"></Selecting>
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                <ClientEvents OnKeyPress="KeyPressed" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                <Columns>
                                    <telerik:GridTemplateColumn DataField="JobName" AutoPostBackOnFilter="true" HeaderStyle-Width="130" UniqueName="ProjectJob"
                                        CurrentFilterFunction="Contains" HeaderText="Project" ShowFilterIcon="false" AllowFiltering="false" ItemStyle-Width="130">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvJob" runat="server" CssClass="texttransparent psearchinput"
                                                Text='<%# Bind("JobName") %>'></asp:TextBox>
                                            <asp:HiddenField ID="hdnJobID" Value='<%# Eval("JobID") != DBNull.Value ? Eval("JobID") : "" %>' runat="server" />
                                            <asp:HiddenField ID="hdnIndex" Value='<%#Container.ItemIndex%>' runat="server" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" Text="Total" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Phase" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="120"
                                        UniqueName="Code"  CurrentFilterFunction="Contains" HeaderText="Code" ShowFilterIcon="false"  ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvPhase" runat="server" CssClass="texttransparent phsearchinput"
                                                Text='<%# Bind("Phase") %>'></asp:TextBox> <%-- onchange="clearPhase(this)"--%>
                                            <asp:HiddenField ID="hdnPID" Value='<%# Eval("PhaseID") != DBNull.Value ? Eval("PhaseID") : "" %>' runat="server" />                                           
                                            <asp:HiddenField ID="hdntxtGvPhase" Value='<%# Bind("Phase") %>' runat="server" />
                                        </ItemTemplate>
                                        
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="ItemDesc" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="120"
                                      UniqueName="ItemDesc"  CurrentFilterFunction="Contains" HeaderText="Item" ShowFilterIcon="false"  ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvItem" runat="server" CssClass="texttransparent pisearchinput"
                                                Text='<%# Bind("ItemDesc") %>'></asp:TextBox>
                                            <asp:HiddenField ID="hdnItemID" Value='<%# Eval("Inv") != DBNull.Value ? Eval("Inv") : "" %>' runat="server" />
                                            <%--<asp:HiddenField ID="hdOpSq" Value='<%# Eval("OpSq")%>' runat="server" />--%>
                                            <asp:HiddenField ID="hdnTypeId" Value='<%# Eval("TypeID") != DBNull.Value ? Eval("TypeID") : "" %>' runat="server" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="fDesc" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="250"
                                        CurrentFilterFunction="Contains" HeaderText="Item Description" ShowFilterIcon="false"  ItemStyle-Width="250">
                                        <ItemTemplate>
                                            <%--<asp:TextBox ID="txtGvDesc" runat="server" CssClass="texttransparent"
                                                Text='<%# Bind("fDesc") %>' MaxLength="255"></asp:TextBox>--%>
                                            <asp:TextBox ID="txtGvDesc" Style="padding: 0px!important;" runat="server" Text='<%#Eval("fDesc")%>' TextMode="MultiLine"
                                                                                                MaxLength="8000" CssClass="materialize-textarea"></asp:TextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Quan" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="100"
                                        CurrentFilterFunction="Contains" HeaderText="Quan" ShowFilterIcon="false"  ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvQuan" runat="server" CssClass="texttransparent" autocomplete="off"
                                                MaxLength="15" Text='<%# Bind("Quan") %>'
                                                onchange="CalTotalVal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalQty" runat="server" Style="text-align: left;"></asp:Label>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="120"
                                        CurrentFilterFunction="Contains" HeaderText="Warehouse" ShowFilterIcon="false"  ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvWarehouse"
                                                Text='<%# Bind("Warehousefdesc") %>'
                                                runat="server" CssClass="texttransparent Warehousesearchinput"></asp:TextBox>
                                            <asp:HiddenField ID="hdnWarehouse" runat="server" Value='<%# Bind("WarehouseID") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="160"
                                        CurrentFilterFunction="Contains" HeaderText="Warehouse Location" ShowFilterIcon="false"  ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvWarehouseLocation"
                                                Text='<%# Bind("Locationfdesc") %>'
                                                runat="server" CssClass="texttransparent WarehouseLocationsearchinput "></asp:TextBox>
                                            <asp:HiddenField ID="hdnWarehouseLocationID" runat="server" Value='<%# Bind("WHLocID") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    

                                    <%--<telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="100"
                                      UniqueName="Ticket"  CurrentFilterFunction="Contains" HeaderText="Ticket" ShowFilterIcon="false"  ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvTicket" runat="server" CssClass="texttransparent" Text='<%# Bind("Ticket") %>' autocomplete="off"></asp:TextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>

                                    <telerik:GridTemplateColumn DataField="AcctNo" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="100"
                                       UniqueName ="AcctGL" CurrentFilterFunction="Contains" HeaderText="Acct No." ShowFilterIcon="false"  ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvAcctNo" runat="server" CssClass="texttransparent searchinput"
                                                Text='<%# Bind("AcctNo") %>'></asp:TextBox>
                                            <%--<asp:HiddenField ID="hdnId" runat="server" Value='<%# Bind("RowID") %>'></asp:HiddenField>--%>
                                            <%--<asp:HiddenField ID="hdnTID" runat="server" Value='<%# Eval("ID") != DBNull.Value ? Eval("ID") : "" %>'></asp:HiddenField>--%>
                                            <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Bind("Line") %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnAcctID" runat="server" Value='<%# Eval("AcctID") != DBNull.Value ? Eval("AcctID") : "" %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="Loc" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="150"
                                        CurrentFilterFunction="Contains" HeaderText="Location Name" ShowFilterIcon="false"  ItemStyle-Width="150">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvLoc" runat="server" CssClass="texttransparent jsearchinput"
                                                Text='<%# Bind("Loc") %>' onchange="clearJob(this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="fDate" SortExpression="fDate" HeaderStyle-Width="120"
                                        HeaderText="Date" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="datepicker_mom" Text='<%# Bind("fDate") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="fTime" SortExpression="fTime" HeaderStyle-Width="140"
                                        HeaderText="Time" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTime" Width="80%" runat="server" Text='<%# Bind("fTime") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="worker" SortExpression="worker" HeaderStyle-Width="120"
                                        HeaderText="Worker" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <%--<asp:TextBox ID="txtWorker" runat="server" Text='<%# Bind("worker") %>'></asp:TextBox>--%>
                                            <asp:DropDownList ID="ddlWorker" runat="server" DataTextField="fdesc"
                                                SelectedValue='<%# Eval("worker") == DBNull.Value ? "" : Eval("worker") %>'
                                                CssClass="form-control input-sm input-small-num browser-default preventdownrow" AppendDataBoundItems="true"
                                                DataValueField="fdesc" DataSource='<%#dtWorker%>'>
                                                <asp:ListItem Value="">Select Worker</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="Billed" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="90"
                                        CurrentFilterFunction="Contains" HeaderText="Chargeable" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkBill" runat="server" Checked='<%# (Convert.ToString(Eval("Billed")) == "1") ? true : false %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false"
                                        CurrentFilterFunction="Contains" HeaderText="Action" ShowFilterIcon="false" HeaderStyle-Width="60"  ItemStyle-Width="60"> 
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibDelete" runat="server" CommandName="DeleteTransaction"
                                                CommandArgument="<%#Container.ItemIndex%>"
                                                ImageUrl="~/images/glyphicons-17-bin.png" Width="13px" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                </div>
            </div>
        </div>
        <div class="row mt-10">
            <div class="btnlinks">
                <asp:LinkButton ID="btnAddNewLines" runat="server" CausesValidation="false" OnClientClick="itemJSON();"
                    OnClick="btnAddNewLines_Click" Text="Add New Lines"></asp:LinkButton>
                <asp:LinkButton ID="btnCopyPrevious" runat="server" CausesValidation="false" OnClientClick="itemJSON();"
                    OnClick="btnCopyPrevious_Click" Text="Copy Previous" Style="display: none;"></asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <asp:Label ID="lblTC" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function dtaa() {
            this.prefixText = null;
            this.con = null;
            this.Acct = null;
        }

        function pageLoad(sender, args) {
            $("[id*=txtTime]").timeEntry();

            $("#<%=RadGrid_VendorItems.ClientID%> tbody tr input:text, #<%=RadGrid_VendorItems.ClientID%> tbody tr input:checkbox, #<%=RadGrid_VendorItems.ClientID%> tbody tr select").on("focus", function (e) {
                // For F6
                var ctr = $(e)[0].target;
                var currRow = $(ctr).closest('tbody>tr');
                var hdnIndexVal = $(currRow).find("[id*=hdnIndex]").val();
                $('#<%=hdnSelectedInvIndex.ClientID%>').val(hdnIndexVal);
                $(ctr).select();
                // Work around Chrome's little problem
                //$(ctr).onmouseup = function() {
                //    // Prevent further mouseup intervention
                //    $(ctr).onmouseup = null;
                //    return false;
                //};
            });

            $("[id*=txtGvJob]").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetJobLocations",
                        data: '{"prefixText": "' + dtaaa.prefixText + '", "IsJob": "' + true + '", "con": "' + dtaaa.con + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load project details");
                        }
                    });
                },
                select: function (event, ui) {
                    var txtGvJob = this.id;
                    var txtGvLoc = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvLoc'));
                    var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));
                    var txtGvAcctNo = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvAcctNo'));
                    var hdnAcctID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnAcctID'));

                    $(hdnJobID).val(ui.item.ID);
                    var jobStr = ui.item.ID + ", " + ui.item.fDesc;
                    $(this).val(jobStr);
                    $(txtGvLoc).val(ui.item.Tag);
                    $(hdnAcctID).val(ui.item.GLExp);
                    var strAcct = ui.item.Acct + ' - ' + ui.item.DefaultAcct;
                    $(txtGvAcctNo).val(strAcct);
                    $('#hdnIsAutoCompleteSelected').val('1');
                    return false;
                },
                change: function (event, ui) {
                    var txtGvJob = this.id;
                    var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));
                    var strJob = document.getElementById(txtGvJob).value;

                    if (strJob == '') {
                        $(hdnJobID).val('');
                    }
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.fDesc);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                //  $(this).autocomplete('search', $(this).val())
            });
            $.each($(".psearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.fDesc;
                    var result_desc = item.Tag;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>';
                    });

                    //if (result_value != null) {
                    //    result_value = result_value.toString().replace(x, function (FullMatch, n) {
                    //        return '<span class="highlight">' + FullMatch + '</span>'
                    //    });
                    //}

                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>';
                        });
                    }
                    if (result_value == 0) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a><b> Project: </b> " + result_value + ", " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });

            $("[id*=txtGvPhase]").autocomplete({

                source: function (request, response) {
                    var curr_control = $("[id*=txtGvPhase]").attr('id');
                    var hdnProjectId = document.getElementById(curr_control.replace('txtGvPhase', 'hdnJobID'));
                    var job = hdnProjectId.value;
                    //var job = "20";
                    var prefixText = "Materials";

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPhase",
                        data: '{"jobID": "' + job + '", "prefixText": "' + prefixText + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var phase = {};
                            var phase1 = [];
                            phase = $.parseJSON(data.d);
                            $.each(phase, function (index) {
                                if (phase[index].TypeName == "Materials") {
                                    phase1[0] = phase[index];
                                }
                            });
                            var jsonConvertedData = JSON.stringify(phase1);
                            response($.parseJSON(jsonConvertedData));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load type.");
                        },
                        complete: function () {
                            $(this).data('requestRunning', false);
                        }
                    });

                },
                deferRequestBy: 200,
                select: function (event, ui) {
                    var txtGvPhase = this.id;
                    var hdnTypeId = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnTypeId'));
                    var hdntxtGvPhase = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdntxtGvPhase'));
                    var str = ui.item.TypeName;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {

                        $(hdnTypeId).val(ui.item.Type);
                        console.log(hdnTypeId.value);
                        $(this).val(ui.item.TypeName);
                        $(hdntxtGvPhase).val(ui.item.TypeName);
                        console.log(hdntxtGvPhase.value);

                    }
                    return false;
                },
                focus: function (event, ui) {

                    $(this).val(ui.item.TypeName);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val());
            });
            $.each($(".phsearchinput"), function (index, item) {

                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.Type;
                    var result_item = item.TypeName;

                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                };
            });

            $("[id*=txtGvItem]").autocomplete({
                source: function (request, response) {
                    var curr_control = this.element.attr('id');
                    var job = "0";
                    var typeId = "8";
                    //var typeId = document.getElementById(curr_control.replace('txtGvItem', 'hdnTypeId')).value; 
                    var prefixText = request.term;
                    query = request.term;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPhaseByInventoryItem",
                        data: '{"typeId": "' + typeId + '", "jobId": "' + job + '", "prefixText": "' + prefixText + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var result = {};
                            var result1 = [];
                            result = $.parseJSON(data.d);
                            var i = 0;
                            $.each(result, function (index) {
                                if (result[index].INVtype == "0") {
                                    result1[i] = result[index];
                                    i = i + 1;
                                }
                            });
                            var jsonConvertedData = JSON.stringify(result1);
                            response($.parseJSON(jsonConvertedData));
                            //response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load item.");
                        },
                        complete: function () {
                            $(this).data('requestRunning', false);
                        }
                    });

                    return false;
                },
                deferRequestBy: 50,
                select: function (event, ui) {

                    var curr_control = this.id;


                    var hdnItemID = document.getElementById(curr_control.replace('txtGvItem', 'hdnItemID'));
                    var txtGvDesc = document.getElementById(curr_control.replace('txtGvItem', 'txtGvDesc'));

                    var txtGvWarehouse = document.getElementById(curr_control.replace('txtGvItem', 'txtGvWarehouse'));
                    var hdnWarehouseID = document.getElementById(curr_control.replace('txtGvItem', 'hdnWarehouse'));

                    var txtGvWarehouseLocation = document.getElementById(curr_control.replace('txtGvItem', 'txtGvWarehouseLocation'));
                    var hdnWarehouseLocationID = document.getElementById(curr_control.replace('txtGvItem', 'hdnWarehouseLocationID'));

                    $(txtGvWarehouse).val("");
                    $(hdnWarehouseID).val("");
                    $(txtGvWarehouseLocation).val("");
                    $(hdnWarehouseLocationID).val("0");


                    var hdnPID = document.getElementById(curr_control.replace('txtGvItem', 'hdnPID'));
                    var job = "";
                    var str = ui.item.ItemDesc;
                    var strId = ui.item.ItemID;


                    if (strId == "0") {
                        $(this).val("");
                        $(hdnItemID).val("");
                        $(hdnPID).val("");

                    }
                    else {
                        if (ui.item.ItemID) {
                            var result = $(this);
                            if (ui.item.OnHand > 0) {
                                $(txtGvDesc).val(ui.item.fDesc);
                                $(hdnItemID).val(ui.item.ItemID);
                                $(hdnPID).val(ui.item.Line);
                                $(this).val(ui.item.ItemDesc1);
                                //$.ajax({
                                //    type: "POST",
                                //    contentType: "application/json; charset=utf-8",
                                //    url: "AccountAutoFill.asmx/IsItemOnHand",
                                //    data: '{"INVitemID": "' + ui.item.ItemID + '","WareHouseID": "0","WHLocationID": "0"}',
                                //    dataType: "json",
                                //    async: true,
                                //    success: function (data) {
                                //        if (data.d == "false") {
                                //            alert("Item selected is not on hand, please choose another one.");
                                //            $(hdnItemID).val("");
                                //            $(result).val("");
                                //            $(txtGvDesc).val("");
                                //        }
                                //        else {
                                //            var txtGvPrice = document.getElementById(result[0].id.replace('txtGvItem', 'txtGvPrice'));
                                //            $(txtGvPrice).val(parseFloat(data.d).toFixed(2));
                                //        }
                                //    },
                                //    error: function (result) {
                                //        alert("Due to unexpected errors we were unable to load item.");
                                //    },
                                //});
                                var txtGvPrice = document.getElementById(result[0].id.replace('txtGvItem', 'txtGvPrice'));
                                $(txtGvPrice).val(parseFloat(ui.item.Price).toFixed(2));
                                // Get warehouse 
                                var dtaaa = new dtaa();
                                dtaaa.prefixText = '';
                                dtaaa.InvID = ui.item.ItemID;
                                dtaaa.isShowAll = "no";
                                $.ajax({
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    url: "AccountAutoFill.asmx/GetWarehouseName",
                                    data: JSON.stringify(dtaaa),
                                    dataType: "json",
                                    async: true,
                                    success: function (data) {
                                        var warehouse = $.parseJSON(data.d);
                                        if (warehouse != null && warehouse.length == 1) {
                                            $(txtGvWarehouse).val(warehouse[0]["WarehouseName"]);
                                            $(hdnWarehouseID).val(warehouse[0]["WarehouseID"]);
                                        }
                                    },
                                    error: function (result) {
                                        // alert("Due to unexpected errors we were unable to load account name");
                                    }
                                });
                            } else {
                                alert("Item selected is not on hand, please choose another one.");
                                $(hdnItemID).val("");
                                $(result).val("");
                                $(txtGvDesc).val("");
                            }

                        }
                        else {
                            $(this).val("");
                            $(hdnPID).val(ui.item.Line);
                            $(txtGvDesc).val(ui.item.ItemDesc1);
                        }

                    }
                    return false;
                },
                focus: function (event, ui) {
                    if (ui.item) {
                        $(this).val(ui.item.ItemDesc1);
                    }
                    return false;
                },

                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val());
            });
            $.each($(".pisearchinput"), function (index, item) {

                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ItemID;
                    var result_item = item.ItemDesc1;
                    var result_line = item.Line;
                    var result_itemfdesc = item.fDesc;
                    var OnHand = item.OnHand;


                    var x = new RegExp('\\b' + query, 'ig');

                    try {
                        if (result_item != null) {
                            result_item = result_item.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>';
                            });
                        }

                        if (result_itemfdesc != null) {
                            result_itemfdesc = result_itemfdesc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>';
                            });
                        }

                    } catch{ }

                    if (result_line == "0") {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>  " + result_item + ', Qty :' + OnHand + ", <span style='color:Gray;'><b>  </b>" + result_itemfdesc + "</span></a>")
                            .appendTo(ul);
                    }
                    else {

                        if (result_item == undefined) { result_item = 'No Record Found!'; }
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item'><i style='display:inline-block; margin-right:8px;width:auto;color:#1565C0 !important;' class='fas fa-check-square' title=''></i>" + result_item + "</span>")
                            .appendTo(ul);

                    }
                };
            });

            $("[id*=txtGvWarehouse]").autocomplete({
                source: function (request, response) {
                    debugger;
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var str = request.term;
                    var txtGvWarehouse_GetID = $(this.element).attr("id");
                    var hdnInvID = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'hdnItemID'));

                    var hdntxtGvPhase = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'hdntxtGvPhase'));
                    //  if (hdntxtGvPhase.value != "Inventory") { return; }
                    console.log(hdntxtGvPhase.value);

                    var ID = $(hdnInvID).val();
                    dtaaa.InvID = ID;
                    dtaaa.isShowAll = "no";
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetWarehouseName",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            // alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },
                select: function (event, ui) {
                    try {
                        var txtGvWarehouse = this.id;
                        var hdnWarehouse = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnWarehouse'));
                        var hdnInvID = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnItemID'));
                        var txtGvWarehouseLocation = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'txtGvWarehouseLocation'));
                        var hdnWarehouseLocationID = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnWarehouseLocationID'));

                        $(txtGvWarehouseLocation).val("");
                        $(hdnWarehouseLocationID).val("0");


                        var Str = ui.item.WarehouseID + ", " + ui.item.WarehouseName;
                        //$(this).val(Str);
                        //$(txtGvWarehouse).val(Str);
                        $(this).val(ui.item.WarehouseName);
                        $(txtGvWarehouse).val(ui.item.WarehouseName);
                        $(hdnWarehouse).val(ui.item.WarehouseID);

                        var locationID = 0;
                        var warehouseID = $(hdnWarehouse).val();
                        var invID = $(hdnInvID).val();

                        //// Check ON Hand
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/IsItemOnHand",
                            data: '{"INVitemID": "' + invID + '","WareHouseID": "' + ui.item.WarehouseID + '","WHLocationID": "0"}',
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                if (data.d == "false") {
                                    alert("Item selected is not on hand this warehouse, please choose another one.");
                                    $(txtGvWarehouse).val('');
                                    $(hdnWarehouse).val('');
                                }
                            },
                            error: function (result) {
                                // alert("Due to unexpected errors we were unable to load item.");
                            },

                        });

                        ////

                    } catch{ }
                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $(this).val(ui.item.WarehouseName);
                    } catch{ }
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val());
            });
            $.each($(".Warehousesearchinput"), function (index, item) {
                if (item && typeof item == "object")
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        debugger;
                        var ula = ul;
                        var itema = item;
                        var result_value = item.ID;
                        var result_item = item.WarehouseName;
                        var result_desc = item.WarehouseID;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>';
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>';
                            });
                        }

                        if (result_value == 0) {
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                        else {
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a style='color:blue;'>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                    };
            });
            //warehouselocation

            //txtGvWarehouseLocation
            $("[id*=txtGvWarehouseLocation]").autocomplete({
                source: function (request, response) {
                    debugger;
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var str = request.term;

                    var txtGvWarehouseLocation_GetID = $(this.element).attr("id");
                    var hdnWarehouse = document.getElementById(txtGvWarehouseLocation_GetID.replace('txtGvWarehouseLocation', 'hdnWarehouse'));
                    var ID = $(hdnWarehouse).val();
                    var hdntxtGvPhase = document.getElementById(txtGvWarehouseLocation_GetID.replace('txtGvWarehouseLocation', 'hdntxtGvPhase'));
                    dtaaa.WarehouseID = ID;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetWarehouseLocation",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {

                            response($.parseJSON(data.d));

                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },
                select: function (event, ui) {
                    try {
                        var txtGvWarehouseLocation = this.id;
                        var hdnWarehouseLocationID = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnWarehouseLocationID'));
                        var hdnInvID = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnItemID'));
                        var hdnWarehouse = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnWarehouse'));

                        var Str = ui.item.ID + ", " + ui.item.Name;
                        $(this).val(Str);
                        $(txtGvWarehouseLocation).val(Str);
                        $(hdnWarehouseLocationID).val(ui.item.ID);

                        var locationID = $(hdnWarehouseLocationID).val();
                        // alert(ui.item.ID);
                        var warehouseID = $(hdnWarehouse).val();
                        var invID = $(hdnInvID).val();


                        //// Check ON Hand
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/IsItemOnHand",
                            data: '{"INVitemID": "' + invID + '","WareHouseID": "' + warehouseID + '","WHLocationID": "' + ui.item.ID + '"}',
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                if (data.d == "false") {
                                    alert("Item selected is not on hand this location, please choose another one.");
                                    $(txtGvWarehouseLocation).val('');
                                    $(hdnWarehouseLocationID).val('');
                                }
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load item.");
                            }
                        });
                    } catch{ }

                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $(this).val(ui.item.ID);
                    } catch{ }
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val());
            });
            $.each($(".WarehouseLocationsearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    debugger;
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.Name;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>';
                    });


                    if (result_value == 0) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a style='color:blue;'>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                };
            });
        }
    </script>
</asp:Content>