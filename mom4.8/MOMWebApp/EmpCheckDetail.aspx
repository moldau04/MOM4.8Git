<%@ Page Title="Edit Check || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="EmpCheckDetail" CodeBehind="EmpCheckDetail.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />


    <style type="text/css">
        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        .rptSti tr:nth-child(2n+1) {
            background: none !important;
        }

        ul.anchor-links li a {
            border-bottom: 1px groove !important;
        }


        @media screen and (max-width: 2048px) {

            .rgDataDiv {
                height: 50vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 2304px) {

            .rgDataDiv {
                height: 52vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 1920px) {

            .rgDataDiv {
                height: 47vh !important;
            }
        }

        @media screen and (max-width: 1706px) {

            .rgDataDiv {
                height: 42vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 1688px) {

            .rgDataDiv {
                height: 42vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 1366px) {

            .rgDataDiv {
                height: 30vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }
    </style>

    <script type="text/javascript">
       <%-- function cancel() {
            window.parent.document.getElementById('<%=mpeTemplate.ClientID%>').click();
        }

        function cancel() {
            window.parent.document.getElementById('btnCancel').click();

        }--%>
        function isNumberKey(evt, txt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function dtaa() {
            this.checkno = null;
            this.bank = null;
            this.cdId = null;
        }
        function IsExistCheckNo() {

            var valCheckNo = $("#<%=txtCheck.ClientID%>").val();
            var valBank = $("#<%=hdnBankID.ClientID%>").val();
            var valCD = $("#<%=hdnCDID.ClientID%>").val();
            var PaymentType = $("#<%=hdnPaymentType.ClientID%>").val();
            var dtaaa = new dtaa();
            dtaaa.checkno = valCheckNo;
            dtaaa.bank = valBank;
            dtaaa.cdId = valCD;
            dtaaa.paytype = PaymentType;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "AccountAutoFill.asmx/CheckNumValidOnEdit",
                data: JSON.stringify(dtaaa),
                dataType: "json",
                async: true,
                success: function (data) {

                    if (data.d == true) {
                        noty({
                            text: 'Check #' + valCheckNo + ' is already in exists in bank account. Since duplicate check numbers are not allowed, the check generation process cannot continue.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 15000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                        $("#<%=txtCheck.ClientID%>").val('');
                        ValidatorEnable($('#<%=rfvCheckNo.ClientID %>')[0], true);
                    }
                },
                failure: function (response) {
                    alert(response);
                },
                error: function (result) {
                    alert("Due to unexpected errors we were unable to load availability");
                }
            });
        }

        function IsExistCheckNoSave() {
            debugger;
            var result = false;
            var valCheckNo = $("#<%=txtCheck.ClientID%>").val();
            var valBank = $("#<%=hdnBankID.ClientID%>").val();
            var valCD = $("#<%=hdnCDID.ClientID%>").val();
            var dtaaa = new dtaa();
            dtaaa.checkno = valCheckNo;
            dtaaa.bank = valBank;
            dtaaa.cdId = valCD;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "AccountAutoFill.asmx/CheckNumValidOnEdit",
                data: JSON.stringify(dtaaa),
                dataType: "json",
                async: true,
                success: function (data) {

                    if (data.d == true) {

                        noty({
                            text: 'Check #' + valCheckNo + ' is already in exists in bank account. Since duplicate check numbers are not allowed, the check generation process cannot continue.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 15000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                        $("#<%=txtCheck.ClientID%>").val('');
                        ValidatorEnable($('#<%=rfvCheckNo.ClientID %>')[0], true);
                        result = false;
                        return result;
                    }
                    else {
                        result = true;
                        return result;
                    }
                },
                failure: function (response) {
                    alert(response);
                },
                error: function (result) {
                    alert("Due to unexpected errors we were unable to load availability");
                }
            });

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_WC" runat="server">
        <AjaxSettings>


            <telerik:AjaxSetting AjaxControlID="btnPrintCheck">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowTemplates" LoadingPanelID="RadAjaxLoadingPanel_WC" />
                </UpdatedControls>
            </telerik:AjaxSetting>



        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_WC" runat="server">
    </telerik:RadAjaxLoadingPanel>
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
                                        <asp:Label ID="lblHeader" runat="server" CssClass="title_text">Edit Check</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <%--<asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkChangeCheckDate_Click" CausesValidation="true" ValidationGroup="check" OnClientClick="disableButton(this,'check'); javascript:return IsExistCheckNoSave();">Save</asp:LinkButton>--%>
                                            <asp:LinkButton ID="lnkChangeCheckDate" runat="server" OnClick="lnkChangeCheckDate_Click" CausesValidation="true" ValidationGroup="check">Change Check Date</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkChangeCheckNo" runat="server" OnClick="lnkChangeCheckNo_Click" CausesValidation="true" ValidationGroup="check">Change Check #</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnPrintCheck" runat="server" OnClick="btnPrintCheck_Click">Reprint Check</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <span id="chkNumber">
                                                <asp:Label ID="lblCheck" runat="server" Text="Check# " Visible="False"></asp:Label>
                                                <asp:Label ID="lblCheckNo" runat="server" Visible="False" Style="font-weight: bold; font-size: 15px;"></asp:Label>
                                            </span>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>

            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div class="tblnks">
                                <ul class="anchor-links">
                                    <li><a href="#accrdEditCheck">Check Detail</a></li>
                                    <li id="libreak" runat="server"><a href="#accrdbreak">Wage Category Breakdown</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <asp:Panel ID="pnlNext" runat="server" class="nextprev" Style="display: block;" Visible="False">
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False"
                                            OnClick="lnkFirst_Click"><i class="fa fa-angle-double-left"></i></asp:LinkButton>
                                    </span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False"
                                            OnClick="lnkPrevious_Click"><i class="fa fa-angle-left"></i></asp:LinkButton>
                                    </span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False"
                                            OnClick="lnkNext_Click"><i class="fa fa-angle-right"></i></asp:LinkButton>
                                    </span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False"
                                            OnClick="lnkLast_Click"><i class="fa fa-angle-double-right"></i></asp:LinkButton>
                                    </span>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="srchpane-advanced" id="accrdEditCheck">

                <div class="form-content-wrap">
                    <div class="form-content-pd">
                        <div class="form-section3">
                            <div class="input-field col s12">
                                <div class="row">
                                    <asp:HiddenField ID="hdnCDID" runat="server" />
                                    <asp:HiddenField ID="hdnEmpID" runat="server" />
                                    <asp:HiddenField ID="hdnVendorAcct" runat="server" />
                                    <asp:TextBox ID="txtPayee" runat="server" CssClass="materialize-textarea remit-ht" TextMode="MultiLine" MaxLength="255"></asp:TextBox>
                                    <asp:Label runat="server" ID="lbltxtPayee" AssociatedControlID="txtPayee">Payee</asp:Label>
                                </div>
                            </div>


                            <div class="input-field col s12">
                                <div class="row">
                                    <asp:TextBox ID="txtMemo" runat="server" CssClass="materialize-textarea remit-ht" TextMode="MultiLine" MaxLength="255"></asp:TextBox>
                                    <asp:Label runat="server" ID="lbltxtMemo" AssociatedControlID="txtMemo">Memo</asp:Label>
                                </div>
                            </div>


                        </div>
                        <div class="form-section3-blank">
                            &nbsp;
                        </div>
                        <div class="form-section3">
                            <div class="input-field col s12">
                                <div class="row">
                                    <label></label>
                                    <asp:RequiredFieldValidator ID="rfvCheckDate"
                                        runat="server" ControlToValidate="txtCheckDate" Display="None" ErrorMessage="Check date is required"
                                        SetFocusOnError="True" ValidationGroup="check"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender
                                        ID="vceCheckDate" runat="server" Enabled="True"
                                        PopupPosition="Right" TargetControlID="rfvCheckDate" />
                                    <asp:TextBox ID="txtCheckDate" runat="server" CssClass="datepicker_mom" onkeypress="return false;" autocomplete="off"></asp:TextBox>
                                    <asp:Label runat="server" ID="lbltxtCheckDate" AssociatedControlID="txtCheckDate">Check Date</asp:Label>

                                </div>
                            </div>
                            <div class="input-field col s12">
                                <div class="row">
                                    <asp:TextBox ID="txtCheck" runat="server" autocomplete="off" MaxLength="19"
                                        onkeypress="return isNumberKey(event,this)" onchange="IsExistCheckNo();"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCheckNo"
                                        runat="server" ControlToValidate="txtCheck" Display="None" ErrorMessage="Check no is required"
                                        SetFocusOnError="True" ValidationGroup="check"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender
                                        ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                        PopupPosition="Right" TargetControlID="rfvCheckNo" />
                                    <asp:Label runat="server" ID="lbltxtCheck" AssociatedControlID="txtCheck">Check#</asp:Label>
                                </div>
                            </div>
                            <div class="input-field col s12">
                                <div class="row">
                                    <asp:HiddenField ID="hdnBankID" runat="server" />
                                    <asp:TextBox ID="txtBank" runat="server"></asp:TextBox>
                                    <asp:Label runat="server" ID="lbltxtBank" AssociatedControlID="txtBank">Bank</asp:Label>
                                </div>
                            </div>
                            <div class="input-field col s12">
                                <div class="row">
                                    <asp:TextBox ID="txtPaymentType" runat="server"></asp:TextBox>
                                    <asp:Label runat="server" ID="lbltxtPaymentType" AssociatedControlID="txtPaymentType"> Payment type</asp:Label>
                                    <asp:HiddenField ID="hdnPaymentType" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-section3-blank">
                            &nbsp;
                        </div>
                        <div class="form-section3">

                            <div class="text-field col s12">
                                <div class="row">
                                    <label style="font-size: 0.9em;">
                                        <asp:Label ID="lblTotlIncome" runat="server" Text="Income:"></asp:Label></label>
                                    <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                        <asp:Label ID="lblTotlIncomeVal" runat="server"></asp:Label></span>
                                </div>
                            </div>
                            <div class="text-field col s12">
                                <div class="row">
                                    <label style="font-size: 0.9em;">
                                        <asp:Label ID="lblTotalDeduction" runat="server" Text="Deduction:"></asp:Label></label>
                                    <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                        <asp:Label ID="lblTotalDeductionVal" runat="server"></asp:Label></span>
                                </div>
                            </div>
                            <div class="text-field col s12">
                                <div class="row">
                                    <label style="font-size: 0.9em;">
                                        <asp:Label ID="lblTotal" runat="server" Text="Net Check:"></asp:Label></label>
                                    <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                        <asp:Label ID="lblTotalVal" runat="server"></asp:Label></span>
                                </div>
                            </div>
                            <div class="input-field col s12">
                                <div class="row">
                                    <asp:Image ID="imgCleared" runat="server" ImageUrl="~/images/icons/Cleared.png" />
                                    <asp:Image ID="imgVoid" runat="server" ImageUrl="~/images/icons/void.png" Style="height: 35px;" />
                                    <asp:HiddenField ID="hdnBatch" runat="server" />
                                </div>
                            </div>
                        </div>




                        <%--<div class="form-section3">
                                        <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                            <div class="row">
                                                <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                                <asp:Label runat="server" ID="lbltxtCompany" AssociatedControlID="txtCompany">Company</asp:Label>
                                            </div>
                                        </div>
                                    </div>--%>
                    </div>
                </div>

            </div>

            <div class="grid_container" style="width: 100% !important">
                <div class="form-section2" style="margin-bottom: 0 !important;">
                    <div style="font-weight: bold; padding-left: 295px; background: #dcddf5;">Wages</div>
                    <div class="grid_container">
                        <div class="RadGrid RadGrid_Material FormGrid">
                            <telerik:RadCodeBlock ID="RadCodeBlock_Bills" runat="server">
                                <script type="text/javascript">
                                    function pageLoad() {
                                        var grid = $find("<%= gvIncome.ClientID %>");
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
                                        try {
                                            var element = document.getElementById(requestInitiator);
                                            if (element && element.tagName == "INPUT") {
                                                element.focus();
                                                element.selectionStart = selectionStart;
                                            }
                                        } catch (e) {

                                        }
                                        Materialize.updateTextFields();
                                    }
                                </script>
                            </telerik:RadCodeBlock>

                            <telerik:RadAjaxPanel ID="RadAjaxPanel_CheckDetails" runat="server" LoadingPanelID="RadAjaxLoadingPanel_WC" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                <telerik:RadGrid RenderMode="Auto" ID="gvIncome" ShowFooter="True" PageSize="50"
                                    PagerStyle-AlwaysVisible="true" OnItemDataBound="gvIncome_ItemDataBound"
                                    ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                    <CommandItemStyle />
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" ShowFooter="True">
                                        <Columns>

                                            <telerik:GridTemplateColumn HeaderText="ID" HeaderStyle-Width="10" DataField="ID" SortExpression="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Bind("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Desc" HeaderStyle-Width="130" DataField="fDesc" SortExpression="fDesc">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("fDesc") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Quan" HeaderStyle-Width="50" DataField="Quan" SortExpression="Quan" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum" FooterAggregateFormatString="{0:N2}">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuan" runat="server" Text='<%# Bind("Quan") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Rate" HeaderStyle-Width="60" DataField="Rate" SortExpression="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rate", "{0:c}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="80" DataField="Amount" SortExpression="Amount" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum" FooterAggregateFormatString="{0:C}">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="YTD" HeaderStyle-Width="80" DataField="YTD" SortExpression="YTD" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum" FooterAggregateFormatString="{0:C}">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYTD" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "YTD", "{0:c}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>

                            </telerik:RadAjaxPanel>
                        </div>
                    </div>
                </div>
                <div class="form-section2-blank">
                    &nbsp;
                </div>
                <div class="form-section2" style="margin-bottom: 0 !important;">
                    <div style="font-weight: bold; padding-left: 295px; background: #f5dedc;">Deduction</div>
                    <div class="grid_container">
                        <div class="RadGrid RadGrid_Material FormGrid">
                            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                <script type="text/javascript">
                                    function pageLoad() {
                                        var grid = $find("<%= gvDeduction.ClientID %>");
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
                                        try {
                                            var element = document.getElementById(requestInitiator);
                                            if (element && element.tagName == "INPUT") {
                                                element.focus();
                                                element.selectionStart = selectionStart;
                                            }
                                        } catch (e) {

                                        }
                                        Materialize.updateTextFields();
                                    }
                                </script>
                            </telerik:RadCodeBlock>

                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_WC" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                <telerik:RadGrid RenderMode="Auto" ID="gvDeduction" ShowFooter="True" PageSize="50"
                                    PagerStyle-AlwaysVisible="true" OnItemDataBound="gvDeduction_ItemDataBound"
                                    ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                    <CommandItemStyle />
                                    <GroupingSettings CaseSensitive="false" />
                                    <FooterStyle CssClass="footer" />
                                    <ClientSettings AllowColumnsReorder="true" EnableAlternatingItems="false" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" ShowFooter="True">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="ID" HeaderStyle-Width="10" DataField="ID" SortExpression="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Bind("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Desc" HeaderStyle-Width="130" DataField="fDesc" SortExpression="fDesc">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("fDesc") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="80" DataField="Amount" SortExpression="Amount" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum" FooterAggregateFormatString="{0:C}">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="YTD" HeaderStyle-Width="80" DataField="YTD" SortExpression="YTD" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum" FooterAggregateFormatString="{0:C}">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYTD" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "YTD", "{0:c}")%>'></asp:Label>
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

            <div class="container accordian-wrap">
                <div class="col s12 m12 l12">
                    <div class="row">
                        <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                            <li id="tbLogs" runat="server" style="display: none">
                                <div id="accrdbreak" class="collapsible-header accrd active accordian-text-custom"><i class="mdi-content-content-paste"></i>Wage Category Breakdown</div>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="grid_container">
                                                <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                    <div class="RadGrid RadGrid_Material">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                            <script type="text/javascript">
                                                                function pageLoad() {
                                                                    try {
                                                                        var grid = $find("<%= gvBreakdown.ClientID %>");
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
                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_gvBreakdown" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvBreakdown" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                            <telerik:RadGrid RenderMode="Auto" ID="gvBreakdown" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true" OnItemCreated="gvBreakdown_ItemCreated"
                                                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True" OnNeedDataSource="gvBreakdown_NeedDataSource">
                                                                <CommandItemStyle />
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="false" ShowFooter="True" AllowNaturalSort="false" DataKeyNames="ID">
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn DataField="ID" SortExpression="ID" HeaderText="ID" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="WageID" SortExpression="WageID" HeaderText="ID" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblWageID" runat="server" Text='<%# Eval("WageID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="WageCategory" SortExpression="WageCategory" HeaderText="Wage Category">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblWageCategory" runat="server" Text='<%# Eval("WageCategory") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="WageType" SortExpression="WageType" HeaderText="Type">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblWageType" runat="server" Text='<%# Eval("WageType") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Quan" SortExpression="Quan" HeaderText="Quan">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblQuan" runat="server" Text='<%# Eval("Quan") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Rate" SortExpression="Rate" HeaderText="Rate">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rate", "{0:c}")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Amount" SortExpression="Amount" HeaderText="Amount">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>




                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </telerik:RadAjaxPanel>
                                                    </div>

                                                </div>
                                            </div>

                                            <div class="cf"></div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

        </div>
    </div>


    <telerik:RadWindowManager ID="RadWindowManagerWC" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowTemplates" Skin="Material" VisibleTitlebar="true" Title="Check Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1100" OnClientClose="OnClientCloseHandler">
                <ContentTemplate>
                    <div>
                        <div class='col s5' style="width: 100%; float: left;">
                            <div class='cr-title' style="padding-top: 5px; font-size: initial; padding-bottom: 5px;">Select a check template. Please note checks will be saved after you exit this screen. </div>
                        </div>
                        <div class='col s5' style="width: 30%; float: left; padding-top: 5px; margin-bottom: 15px; margin-right: 30px;">
                            <div class='cr-box'>
                                <div class='cr-title'>AP – check top </div>
                                <%--<div class='cr-img'>
                                    <asp:Label ID="lbltopcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                    <asp:Label ID="lbltopdd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                    <asp:Label ID="lbltopemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                            
                                </div>
                                <div class='cr-img'>
                                    <img src='images/ReportImages/ApTopCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                                </div>--%>
                                <div class='cr-date'>
                                    <div class='cr-iocn'>
                                        <asp:DropDownList ID="ddlApTopCheckForLoad" runat="server" CssClass="browser-default">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="imgPrintTemp1" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp1_Click" ToolTip="Export to PDF" />
                                        <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="25px" Width="25px" OnClick="ImageButton7_Click" ToolTip="Edit Template" />
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkSaveDefault_Click" ToolTip="Set as Default" />
                                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClick="ImageButton3_Click" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" />

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class='col s5' style="width: 30%; float: left; padding-top: 5px; margin-bottom: 15px; margin-right: 30px;">
                            <div class='cr-box'>
                                <div class='cr-title'>AP – check middle </div>
                                <%--<div class='cr-img'>
                                    <asp:Label ID="lblmidcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                    <asp:Label ID="lblmidadd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                    <asp:Label ID="lblmidemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                                </div>
                                <div class='cr-img'>
                                    <img src='images/ReportImages/MidCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                                </div>--%>
                                <div class='cr-date'>

                                    <asp:DropDownList ID="ddlApMiddleCheckForLoad" runat="server" CssClass="browser-default">
                                    </asp:DropDownList>
                                    <div class='cr-iocn'>
                                        <asp:ImageButton ID="imgPrintTemp2" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp2_Click" ToolTip="Export to PDF" />
                                        <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="30px" Width="30px" OnClick="ImageButton8_Click" ToolTip="Edit Template" />
                                        <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkSaveApMiddleCheck_Click" ToolTip="Set as Default" />
                                        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClick="ImageButton6_Click" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" />

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class='col s5' style="width: 30%; float: left; padding-top: 5px; margin-bottom: 15px; margin-right: 30px;">
                            <div class='cr-box'>
                                <div class='cr-title'>AP – Detailed check top </div>
                                <%--<div class='cr-img'>
                                    <asp:Label ID="lbldetailcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                    <asp:Label ID="lbldetailadd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                    <asp:Label ID="lbldetailemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                               </div>
                               <div class='cr-img'>
                                   <img src='images/ReportImages/TopDetailCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                               </div>--%>
                                <div class='cr-date'>
                                    <asp:DropDownList ID="ddlTopChecksForLoad" runat="server" CssClass="browser-default">
                                    </asp:DropDownList>

                                    <div class='cr-iocn'>
                                        <asp:ImageButton ID="imgPrintTemp6" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp6_Click" ToolTip="Export to PDF" />
                                        <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="30px" Width="30px" OnClick="ImageButton9_Click" ToolTip="Edit Template" />
                                        <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkTopChecks_Click" />
                                        <asp:ImageButton ID="ImageButton14" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClick="ImageButton14_Click" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" />

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowFirstReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" OnClientClose="OnClientCloseHandler">
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner1" runat="server" OnSaveReport="StiWebDesigner1_SaveReport" Width="100%" Height="700" OnSaveReportAs="StiWebDesigner1_SaveReportAs" OnExit="StiWebDesigner1_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowSecondReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" OnClientClose="OnClientCloseHandler">
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner2" runat="server" OnSaveReport="StiWebDesigner2_SaveReport" Width="100%" Height="700" OnSaveReportAs="StiWebDesigner2_SaveReportAs" OnExit="StiWebDesigner2_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowThirdReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" OnClientClose="OnClientCloseHandler">
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner3" runat="server" OnSaveReport="StiWebDesigner3_SaveReport" Width="100%" Height="700" OnSaveReportAs="StiWebDesigner3_SaveReportAs" OnExit="StiWebDesigner3_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

        });
        function pageLoad(sender, args) {

            var firstReport = '<%=Session["wc_first_edit"] %>';
            if (firstReport == "true") {
                showFirstWindow();
                <%Session["wc_first_edit"] = null; %>
            }

            var secondReport = '<%=Session["wc_second_edit"] %>';
            if (secondReport == "true") {
                showSecondWindow();
                <%Session["wc_second_edit"] = null; %>
            }

            var thirdReport = '<%=Session["wc_third_edit"] %>';
            if (thirdReport == "true") {
                showThirdWindow();
                <%Session["wc_third_edit"] = null; %>
            }

            Materialize.updateTextFields();
        }

        function showFirstWindow() {
            Sys.Application.remove_load(showFirstWindow);
            var oWindowCust = $find('<%= RadWindowFirstReport.ClientID %>');
            oWindowCust.show();
        }

        function showSecondWindow() {
            Sys.Application.remove_load(showSecondWindow);
            var oWindowCust = $find('<%= RadWindowSecondReport.ClientID %>');
            oWindowCust.show();
        }

        function showThirdWindow() {
            Sys.Application.remove_load(showThirdWindow);
            var oWindowCust = $find('<%= RadWindowThirdReport.ClientID %>');
            oWindowCust.show();
        }

        function OnClientCloseHandler(sender, args) {
            <%Session["wc_first_edit"] = null; %>
            <%Session["wc_second_edit"] = null; %>
            <%Session["wc_third_edit"] = null; %>
            var id = getUrlParameter('id');
            var frm = getUrlParameter('frm');
            if (frm.trim() != "" && frm != null) {
                window.location = "EditCheck.aspx?id=" + id + "&frm=" + frm;
            }
            else {
                window.location = "EditCheck.aspx?id=" + id;
            }
        }

        function getUrlParameter(name) {
            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(location.search);
            return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
        };

    </script>
</asp:Content>

