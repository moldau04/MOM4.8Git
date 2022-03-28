<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddInventory" EnableEventValidation="false" CodeBehind="AddInventory.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">


    <%--     <script src="js/jquery-ui-1.9.2.custom.js"></script>
    <link href="css/jquery-ui-1.9.2.custom.css" rel="stylesheet" />--%>


    <style type="text/css">
        #RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindowWarehouse {
            height: 235px !important;
        }

        #ctl00_ContentPlaceHolder1_RadWindowWarehouse_C {
            height: 180px !important;
        }
    </style>

    <script type="text/javascript">

        function checkAll(cb) {

            var ctrls = document.getElementsByTagName('input');
            for (var i = 0; i < ctrls.length; i++) {
                var cbox = ctrls[i];
                if (cbox.type == "checkbox") {
                    cbox.checked = cb.checked;
                }
            }

        }


        ///-Document permission

        function AddDocumentClick(hyperlink) {
            var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd == "Y") {
                ConfirmUpload(this.value)
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
        }

        function DeleteDocumentClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteDocument.ClientID%>').value;
            if (IsDelete == "Y") {
                return checkdelete();
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }


        function ViewDocumentClick(hyperlink) {
            var IsView = document.getElementById('<%= hdnViewDocument.ClientID%>').value;
            if (IsView == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }


        var ID = '<%=invID%>';

        $(document).ready(function () {


            //debugger;
            if (ID != "") {

                //$('#txtItemHeaderName').addClass("hide");
                //$('#txtEngineeringName').addClass("hide");
                //$('#txtFinanceName').addClass("hide");
                //$('#txtPurchasingName').addClass("hide");
                //$('#txtInventoryName').addClass("hide");
                //$('#txtSalesName').addClass("hide");
                $(".name").removeClass("hide");
            }
            else {
                $(".name").addClass("hide");
            }

            $(".calculated").attr("disabled", "disabled");

            $("#purchaseerror").addClass('hideerror');
            $(".numeric").attr("MaxLength", "9");
            $(".integer").attr("MaxLength", "9");

            modalclose();

            //  $("#shade").removeclass("ModalPopupBG");
            // $(".numeric").live("lostfocus", function () {   //**DComment
            $('.integer').focusout(function () {
                //alert('dsfdf');
                if ($(this).val = '')
                    $(this).val("0");
            });

            //$(".integer").live("lostfocus", function () {    //**DComment
            $('.integer').focusout(function () {
                //alert('dsfdf');
                if ($(this).val = '')
                    $(this).val("0");
            });


            $('#txtItemHeaderName').focusout(function () {
                var Name = $(this).val();
                $('#txtEngineeringName').val(Name);
                $('#txtFinanceName').val(Name);
                $('#txtPurchasingName').val(Name);
                $('#txtInventoryName').val(Name);
                $('#txtSalesName').val(Name);

            });

            $('#txtDes').focusout(function () {
                var Name = $(this).val();
                $('#txtSalesDescription').val(Name);
                $('#txtEngineeringDescription').val(Name);
                $('#txtFinanceDescription').val(Name);
                $('#txtPurchasingDescription').val(Name);
                $('#txtInventoryDescription').val(Name);

            });

            //$(".numeric").live("keydown", function (el) {   //**DComment
            $(".numeric").keydown(function (el) {
                //alert(el.keyCode);
                //debugger;
                if (el.shiftKey) {
                    el.preventDefault();
                }
                if (el.keyCode == 46 || el.keyCode == 8 || el.keyCode == 9 || el.key == ".") {



                }
                else {
                    if (el.keyCode < 95) {
                        if (el.keyCode < 48 || el.keyCode > 57) {
                            el.preventDefault();
                        }
                    }
                    else {
                        if (el.keyCode < 96 || el.keyCode > 105) {
                            el.preventDefault();
                        }
                    }
                }
            });

            $('.numeric').on('change', function () {
                this.value = this.value.trim() != "" ? parseFloat(this.value).toFixed(2) : 0.00;
            });


            //$(".integer").live("keydown", function (el) {    //**DComment
            $(".integer").keydown(function (el) {
                //alert(el.keyCode);
                //debugger;
                if (el.shiftKey) {
                    el.preventDefault();
                }
                if (el.keyCode == 46 || el.keyCode == 8 || el.keyCode == 9) {



                }
                else {
                    if (el.keyCode < 95) {
                        if (el.keyCode < 48 || el.keyCode > 57) {
                            el.preventDefault();
                        }
                    }
                    else {
                        if (el.keyCode < 96 || el.keyCode > 105) {
                            el.preventDefault();
                        }
                    }
                }
            });

            $('.integer').on('change', function () {
                this.value = this.value.trim() != "" ? parseInt(this.value, 0) : 0;
            });



            // $("#btnSubmit").live("click", function () {   //**DComment
            $("#btnSubmit").click(function () {

                if (($("#dtlVendors tr")).length <= 0) {

                    $("#purchaseerror").removeClass('hideerror');
                    $("#purchaseerror").addClass('showerror');
                }

                if ($('#<%= ddlglsales.ClientID %>').val() == "0" && $('#<%= ddlglcogs.ClientID %>').val() == "0") {
                    $('html, body').animate({ scrollTop: '+=530px' }, 800);

                    $('#accrdFinance').removeClass('collapsible-header accrd accordian-text-custom').addClass('collapsible-header accrd accordian-text-custom active');
                    $("#divaccrdFinance").attr("style", "display:block");
                    $("#divaccrdFinance").focus();
                    //noty({ text: 'Please Select GL Sales And GL Cogs!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });

                    //var panel = $(this).find('.in');
                    //$('html, body').animate({
                    //    scrollTop: panel.offset().focus() - 130
                    //}, 500);

                    // $('.aHandler').click(function (event) {
                    //     event.preventDefault();

                    // });

                    //$('#accrdFinance').on('shown.bs.collapse', function () {
                    //    alert();
                    //    var panel = $(this).find('.in');
                    //    $('html, body').animate({
                    //        scrollTop: panel.offset().focus() - 130
                    //    }, 500);
                    //});
                    return false;
                }

                if ($('#<%= ddlglsales.ClientID %>').val() == "0") {
                    //noty({ text: 'Please Select GL Sales!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('html, body').animate({ scrollTop: '+=530px' }, 800);
                    $('#accrdFinance').removeClass('collapsible-header accrd accordian-text-custom').addClass('collapsible-header accrd accordian-text-custom active');
                    $("#divaccrdFinance").attr("style", "display:block");
                    $("#divaccrdFinance").focus();

                    return false;
                }
                if ($('#<%= ddlglcogs.ClientID %>').val() == "0") {
                    // noty({ text: 'Please Select GL Cogs!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('html, body').animate({ scrollTop: '+=530px' }, 800);
                    $('#accrdFinance').removeClass('collapsible-header accrd accordian-text-custom').addClass('collapsible-header accrd accordian-text-custom active');
                    $("#divaccrdFinance").attr("style", "display:block");
                    $("#divaccrdFinance").focus();

                    return false;
                }
            });


        });

        $(function () {

            $(".date-picker").datepicker();



            // $("#btndekVendorInfo").live("click", function () {     //**DComment
            $("#btndekVendorInfo").click(function () {

                if (($("#dtlVendors tr").not("#dtlVendors tr:first-child").find("#chkvenitem:checked")).length <= 0) {
                    //alert(($("#dtlVendors tr").find("#chkvenitem:checked")).length);
                    return false;
                }

                //$("#dtlVendors tr").not("#dtlVendors tr:first-child").each(function (i, row) {

                //    var $row = $(row);

                //    var $chk = $row.find("#chkvenitem");
                //    if ($chk.is(":checked") == true) {
                //        // $chk.parent("tr").remove();
                //      //  $(this).closest('tr').addClass("vendordelete");
                //        $(this).closest('tr').attr("visibility", "hidden")


                //    }
                //});


            });


            //$("#btnSubmit").live("click", function () {     //**DComment
            $("#btnSubmit").click(function () {
                $(".numeric").each(function (i, v) {

                    if ($(this).val().trim() == '') {

                        $(this).val('0');
                    }

                });

                $(".integer").each(function (i, v) {
                    if ($(this).val().trim() == '') {

                        $(this).val('0');
                    }

                });

                if (Page_ClientValidate("Inv")) {
                    //alert("valid");
                }
                else {
                    for (var i = 0; i < Page_Validators.length; i++) {
                        var val = Page_Validators[i];
                        var ctrl = document.getElementById(val.controltovalidate);
                        if (ctrl != null && ctrl.style != null) {
                            if (!val.isvalid) {

                                ctrl.style.borderColor = '#FF0000';
                                ctrl.style.backgroundColor = '#fce697';
                            }
                            else {
                                ctrl.style.borderColor = '';
                                ctrl.style.backgroundColor = '';
                            }
                        }
                    }

                }
            });




            //$("#lnkCloseInventoryWarehouse").live("click", function () {   //**DComment
            $("#lnkCloseInventoryWarehouse").click(function () {
                modalclose();

            });





            //$("#lnkCloseInvMergeWarehouse").live("click", function () {           //**DComment
            $("#lnkCloseInvMergeWarehouse").click(function () {
                modalclose();

            });


            // $("#linkquoterequestid").live('click', function () {            //**DComment
            $("#linkquoterequestid").click(function () {
                showquoterequestmodal();
            });


            //$("#ddlApprovedVendorrequestquote").live("change", function () {            //**DComment
            $("#ddlApprovedVendorrequestquote").click(function () {
                // debugger
                $('#ddlMPNMannufacturer option').remove();
                $("#txtvendoremail").val('');

                var vendor = $('#ddlApprovedVendorrequestquote option:selected').val();
                $.ajax({
                    type: "POST",
                    url: "AddInventory.aspx/GetApprovedVendorInfo",
                    data: '{strinvID:"' +<%=invID%> +'",Vendor: "' + vendor + '"}',

                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: OnSuccess,
                    failure: function (response) {
                        //alert("zdfsds");


                    },
                    error: function (response) {

                        //alert("sdfjdzjz");
                    }
                });
            });


            //$("#lnksendRequestQuote").live("click", function () {           //**DComment
            $("#lnksendRequestQuote").click(function () {

                $.ajax({
                    type: "POST",
                    url: "AddInventory.aspx/SendMail",
                    data: '{ToEmail:"' + $("#txtvendoremail").val() + '",Quantity: "' + $("#txtquotequantity").val() + '",body:"' + $("#txtemailcontentent").val() + '"}',

                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: function (response) {


                        if (response.d.Header.HasError == false) {


                            noty({
                                text: response.d.ReponseObject,
                                type: 'success',
                                layout: 'topCenter',
                                closeOnSelfClick: false,
                                timeout: false,
                                theme: 'noty_theme_default',
                                closable: true
                            });
                        }

                        else if (response.d.Header.HasError == true) {


                            noty({
                                text: response.d.ReponseObject,
                                type: 'error',
                                layout: 'topCenter',
                                closeOnSelfClick: false,
                                timeout: false,
                                theme: 'noty_theme_default',
                                closable: true
                            });
                        }

                        modalclose();

                    }

                });


                return false;
            });


            //$("#btnclose").live("click", function () {           //**DComment
            $("#btnclose").click(function () {
                modalclose();

                return false;

            });
        });


        function modalclose() {

            $("#programmaticPopup").attr("style", "display:none");
            $("#programmaticPopup").removeClass("programmaticPopup");
            $("#shade").removeClass("ModalPopupBG");

            $("#programmaticRequestQuotePopup").attr("style", "display:none");
            $("#programmaticRequestQuotePopup").removeClass("programmaticPopup");
            $("#shade").removeClass("ModalPopupBG");

            $("#programmaticPopupforMergeWarehouse").attr("style", "display:none");
            $("#programmaticPopupforMergeWarehouse").removeClass("programmaticPopupforMergeWarehouse");
            $("#shade").removeClass("ModalPopupBG");


            $("#txtInventoryMPN").val('');
            $("#txtVendorPartNumber").val('');
            $("#txtVendorPrice").val('');
            $("#txtManufacturerPrice").val('');
            $("#txtInventoryApprovedManufacturer").val('');
            $("#ddlInventoryApprovedVendor").val('0');
            $("#hdninvvendinfo").val('');
            $('#ddlMPNMannufacturer option').remove();

            //txtInventoryMPN.Text = string.Empty;
            //txtInventoryApprovedManufacturer.Text = string.Empty;
            //ddlInventoryApprovedVendor.ClearSelection();
            //hdninvvendinfo.Value = "";

            $("#ddlApprovedVendorrequestquote").val('0');
            $("#txtvendoremail").val('');
            $("#txtquotequantity").val('');
            $("#txtemailcontentent").val('');

        }


        function showquoterequestmodal() {
            $("#programmaticRequestQuotePopup").attr("style", "display:block");
            $("#programmaticRequestQuotePopup").addClass("programmaticPopup");
            $("#shade").addClass("ModalPopupBG");
        }


        function OnSuccess(response) {
            //debugger;
            // ('#lblMsg').text('An error was encountered.');


            //setTimeout(function () {
            //    $("#dvnotification").show();
            //}, 1);
            var result = response.d.ReponseObject;


            // alert(response.d.ReponseObject.InventoryManufacturerInformationId);
            var sel = $('#ddlMPNMannufacturer');
            $.each(result.ManufacturerInfo, function () {

                var items = this;

                sel.append('<option value="' + items.ID + '">' + items.MPN + "-" + items.Manufacturer + '</option>');
            });

            $("#txtvendoremail").val(result.Email);

            //alert(result.Email);



        }

        function ConfirmUpload(value) {
            var filename;
            var fullPath = value;
            if (fullPath) {
                var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
                filename = fullPath.substring(startIndex);
                if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                    filename = filename.substring(1);
                }
            }

            if (confirm('Upload ' + filename + '?')) { document.getElementById('<%= lnkUploadDoc.ClientID %>').click(); }
            else { document.getElementById('<%= lnkPostback.ClientID %>').click(); }
        }

        function checkdelete() {
            return SelectedRowDelete('<%= RadGrid_Documents.ClientID %>', 'file');
        }


    </script>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Inv" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnAddMergeWarehouse">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowWarehouse" LoadingPanelID="RadAjaxLoadingPanel_Inv" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkAddRevisionDetail">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowRev" LoadingPanelID="RadAjaxLoadingPanel_Inv" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnaddVendorInfo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowVendor" LoadingPanelID="RadAjaxLoadingPanel_Inv" />
                </UpdatedControls>
            </telerik:AjaxSetting>


            <telerik:AjaxSetting AjaxControlID="btneditVendorInfo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowVendor" LoadingPanelID="RadAjaxLoadingPanel_Inv" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridTran" LoadingPanelID="RadAjaxLoadingPanel_Inv" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridTran" LoadingPanelID="RadAjaxLoadingPanel_Inv" />
                </UpdatedControls>
            </telerik:AjaxSetting>


            <telerik:AjaxSetting AjaxControlID="RadGridTran">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridTran" LoadingPanelID="RadAjaxLoadingPanel_Inv" />
                </UpdatedControls>
            </telerik:AjaxSetting>


        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Inv" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div id="new_design_div">
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
                                             <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Item</asp:Label>
                                        </div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"
                                                    TabIndex="1" ValidationGroup="Inv" ClientIDMode="Static">Save</asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click"
                                                TabIndex="2"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>
                                        <div class="rght-content">
                                            <div class="editlabel">
                                                <asp:Label ID="asp" runat="server"></asp:Label>
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
                                    <ul class="anchor-links" id="ulAccrd">
                                        <li><a href="#accrdItemHeader">Item Header</a></li>
                                        <%-- <li id="TabWarehouse" runat="server"><a href="#accrdWarehouse">Warehouse</a></li>--%>
                                        <li><a href="#accrdWarehouse">Warehouse</a></li>
                                        <li><a href="#accrdEngineering">Engineering</a></li>
                                        <li><a href="#accrdFinance">Finance</a></li>
                                        <li><a href="#accrdPurchasing">Purchasing</a></li>
                                        <%--<li><a href="#accrdInventory" style="display:none;">Inventory</a></li>--%>
                                        <li><a href="#accrdSales">Sales</a></li>
                                        <li><a href="#accrdDocument">Documents</a></li>
                                        <li id="tbTransactions" runat="server"><a href="#accrdTransaction">Transactions</a></li>

                                    </ul>
                                </div>
                                <div class="tblnksright">
                                    <div class="nextprev">
                                        <asp:Panel ID="pnlSave" runat="server">
                                            <asp:Panel ID="pnlNext" runat="server">
                                                <span class="angleicons">
                                                    <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False" OnClick="lnkFirst_Click">
                                                        <i class="fa fa-angle-double-left"></i>
                                                    </asp:LinkButton>
                                                </span>
                                                <span class="angleicons">
                                                    <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False" OnClick="lnkPrevious_Click">
                                                        <i class="fa fa-angle-left"></i>
                                                    </asp:LinkButton>
                                                </span>
                                                <span class="angleicons">
                                                    <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False" OnClick="lnkNext_Click">
                                                        <i class="fa fa-angle-right"></i>
                                                    </asp:LinkButton>
                                                </span>
                                                <span class="angleicons">
                                                    <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CssClass="icon-last" CausesValidation="False" OnClick="lnkLast_Click">
                                                        <i class="fa fa-angle-double-right"></i>
                                                    </asp:LinkButton>
                                                </span>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div class="container accordian-wrap">
            <div class="row">


                <div class="srchpane-advanced">
                    <div class="form-section-row">
                        <div class="col s12 m12 l12 p-r-0" >
                            <div class="row">
                                <div class="form-section-row">
                                    <div class="section-ttle">Item Details</div>
                                    <div class="form-input-row">
                                        <div class="form-section3half">
                                            <div class="input-field col s12 pdrightgap">
                                                <div class="row">
                                                    <asp:TextBox ID="txtItemHeaderName" runat="server" ClientIDMode="Static"></asp:TextBox>
                                                    <label for="txtItemHeaderName">Part Number</label>

                                                    <asp:RequiredFieldValidator ID="rfvName" ValidationGroup="Inv"
                                                        runat="server" ControlToValidate="txtItemHeaderName" Display="none" ErrorMessage="P/N name is required In Item Header Tab"
                                                        SetFocusOnError="True" ToolTip="P/N name is required." CssClass="error"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section3half-blank">
                                            &nbsp;
                                        </div>
                                        <div class="form-section3half">
                                            <div class="input-field col s12 pdrightgap">
                                                <div class="row">
                                                    <asp:TextBox ID="txtDes" runat="server" MaxLength="75" ClientIDMode="Static" />

                                                    <label for="txtDes">Description</label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Inv"
                                                        runat="server" ControlToValidate="txtDes" Display="Static" ErrorMessage="Description is required in Item tab"
                                                        SetFocusOnError="True" ToolTip="Description is required." CssClass="error"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-section-row">
                                    <div class="form-input-row">
                                        <div class="form-section3half">
                                            <div class="col s6 m6 l6">
                                                <div class="row">
                                                    <div class="input-field col s12 pdrightgap">
                                                        <div class="row">
                                                            <input id="hdnPatientId" runat="server" type="hidden" />
                                                            <asp:HiddenField ID="hdnMail" runat="server" />
                                                            <asp:HiddenField ID="hdnSageIntegration" runat="server" />
                                                            <asp:TextBox ID="txtOnHnad" CssClass="calculated" runat="server" ClientIDMode="Static"></asp:TextBox>
                                                            <label for="estimateNo">On Hand</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col s6 m6 l6">
                                                <div class="row">
                                                    <div class="input-field col s12 pdrightgap">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtOnOrder" runat="server" CssClass="calculated" ClientIDMode="Static"></asp:TextBox>

                                                            <label for="txtOnOrder">On Order</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section3half-blank">
                                            &nbsp;
                                        </div>
                                        <div class="form-section3half">
                                            <div class="col s6 m6 l6">
                                                <div class="row">
                                                    <div class="input-field col s12 pdrightgap">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtOnComitted" runat="server" CssClass="calculated" ClientIDMode="Static"></asp:TextBox>
                                                            <label for="txtOnComitted">Committed</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col s6 m6 l6">
                                                <div class="row">
                                                    <div class="input-field col s12 pdrightgap">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtOnAvaliable" runat="server" CssClass="calculated" ClientIDMode="Static"></asp:TextBox>
                                                            <label for="txtOnAvaliable">Available</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-section-row" style="display: none;">
                                    <div class="form-input-row">
                                        <div class="form-section3half">
                                            <div class="col s6 m6 l6">
                                                <div class="row">
                                                    <div class="input-field col s12 pdrightgap">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtIssuedtoOpenjobs" runat="server" CssClass="calculated" ClientIDMode="Static"></asp:TextBox>
                                                            <label for="txtIssuedtoOpenjobs">Issued to Open jobs</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col s6 m6 l6">
                                                <div class="row">
                                                    <div class="input-field col s12 pdrightgap">
                                                        <div class="row">
                                                            <div class="btnlinks">
                                                                <asp:LinkButton ID="LinkButton1" runat="server">Details</asp:LinkButton>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="cf"></div>
                    </div>
                </div>

                <div class="col s12 m12 l12">
                    <div class="row">
                        <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                            <li>
                                <div id="accrdItemHeader" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>Item Header</div>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <div class="form-section3half">
                                                            <div class="col s12 m12 l12">
                                                                <div class="row">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label class="drpdwn-label">UOM</label>
                                                                            <asp:DropDownList ID="ddlUOM" runat="server" class="browser-default"></asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvUom" ValidationGroup="Inv"
                                                                                runat="server" ControlToValidate="ddlUOM" Display="Static" ErrorMessage="UOM is required in item tab"
                                                                                SetFocusOnError="True" InitialValue="0" ToolTip="UOM is required." CssClass="error"></asp:RequiredFieldValidator>

                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtDateCreated" disabled="disabled" runat="server" ClientIDMode="Static" ReadOnly="true" />
                                                                            <label for="txtDateCreated">Date Created</label>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Inv"
                                                                                runat="server" ControlToValidate="txtDateCreated" Display="Dynamic" ErrorMessage="*"
                                                                                SetFocusOnError="True" ToolTip="Created date is required." CssClass="error"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label for="txtRemarks">Remarks</label>
                                                                            <asp:TextBox ID="txtRemarks" class="materialize-textarea mtarea" runat="server" TextMode="MultiLine" />

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="form-section3half-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3half">
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Status</label>
                                                                    <asp:DropDownList ID="ddlInvStatus" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Inv"
                                                                        runat="server" ControlToValidate="ddlInvStatus" Display="Dynamic" ErrorMessage="*"
                                                                        SetFocusOnError="True" ToolTip="Satus is required." CssClass="error"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Category</label>
                                                                    <asp:DropDownList ID="ddlCategory" runat="server" class="browser-default"></asp:DropDownList>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                    </div>

                                </div>
                            </li>
                            <li runat="server">
                                <%--  <li runat="server" id="divinvware"> 
                                    <div id="accrdWarehouse" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-cached"></i>Warehouse</div>
                                    <div id="accrdWarehouse" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Warehouse</div>
                                --%>
                                <div id="accrdWarehouse" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-content-content-paste"></i>Warehouse</div>
                                <div class="collapsible-body">
                                    <%--<div class="collapsible-body" id="divInvWarehouse">--%>
                                    <div id="WarehouseHide" runat="server" visible="true">
                                        <div class="form-content-wrap">
                                            <div class="form-content-pd">
                                                <div class="accrd-buttons">
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="btnAddMergeWarehouse" runat="server" OnClick="btnAddMergeWarehouse_Click">Add</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="btndelMergewarehouse" runat="server" OnClick="btndelMergewarehouse_Click"
                                                            ClientIDMode="Static">Delete</asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="grid_container">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock_Warehouse" runat="server">
                                                            <script type="text/javascript">
                                                                function pageLoad() {
                                                                    var grid = $find("<%= RadGrid_Warehouse.ClientID %>");
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

                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Warehouse" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Inv" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Warehouse" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                PagerStyle-AlwaysVisible="true"
                                                                ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                <CommandItemStyle />
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                    <Columns>

                                                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                        </telerik:GridClientSelectColumn>

                                                                        <telerik:GridTemplateColumn HeaderText="Warehouse Name" SortExpression="WarehouseName" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnid" runat="server" Value='<%#Eval("ID")%>' />
                                                                                <asp:Label ID="lblWarehouse" runat="server" Text='<%#Eval("WarehouseName")%>'></asp:Label>
                                                                                <asp:Label ID="lblWarehouseID" runat="server" Text='<%#Eval("WarehouseID")%>' CssClass="hide"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn HeaderText="On Hand" SortExpression="Hand" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblOnHand" runat="server" Text='<%#Eval("Hand")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn HeaderText="Amount" SortExpression="Balance" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Balance")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <%--  <telerik:GridTemplateColumn HeaderText="Committed" SortExpression="Committed" ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCommitted" runat="server" Text='<%#Eval("Committed")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>--%>

                                                                        <telerik:GridTemplateColumn HeaderText="OnOrder" SortExpression="fOrder" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblfOrder" runat="server" Text='<%#Eval("fOrder")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn HeaderText="Available" SortExpression="Available" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAvailable" runat="server" Text='<%#Eval("Available")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn HeaderText="Company" SortExpression="Company" ShowFilterIcon="false" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCompany" runat="server" Text='<%#Eval("Company")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn UniqueName="lblIndexID" Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblIndex" runat="server" Text='<%# Container.ItemIndex %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </telerik:RadAjaxPanel>


                                                    </div>
                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                </div>
                            </li>
                            <li>
                                <div id="accrdEngineering" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Engineering</div>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <div class="form-section3">
                                                            <div class="col s12 m12 l12">
                                                                <div class="row">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtSpecification" runat="server" MaxLength="75" />
                                                                            <label for="txtSpecification">Specifications</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtshelflife" CssClass="numeric integer" runat="server" />
                                                                    <label for="txtshelflife">
                                                                        <asp:Label ID="Label12" runat="server" Text="Shelf Life(days)"></asp:Label></label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="col s4 m4 l4 lblsam">
                                                                Revision Detail
                                                            </div>
                                                            <div class="col s4 m4 l4">
                                                                <div class="btnlinks">
                                                                    <asp:LinkButton
                                                                        ID="lnkAddRevisionDetail" runat="server" OnClick="lnkAddRevisionDetail_Click">Add</asp:LinkButton>

                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>

                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="form-content-pd">
                                            <div class="row">
                                                <div class="grid_container">
                                                    <div class="RadGrid RadGrid_Material FormGrid">


                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Revision" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                            PagerStyle-AlwaysVisible="true" ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                <Columns>
                                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                    </telerik:GridClientSelectColumn>

                                                                    <telerik:GridTemplateColumn Visible="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderText="Revision#" SortExpression="Version" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVersion" runat="server" Text='<%# Eval("Version") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderText="Date" SortExpression="Date" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date","{0:d}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderText="Eco" SortExpression="Eco" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEco" runat="server" Text='<%# Eval("Eco") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderText="Drawing" SortExpression="Drawing" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDrawing" runat="server" Text='<%# Eval("Drawing") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderText="Comment" SortExpression="Comment" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblComment" runat="server" Text='<%# Eval("Comment") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>

                                                    </div>
                                                </div>

                                                <div class="cf"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                </div>
                            </li>
                            <li>
                                <div id="accrdFinance" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Finance</div>
                                <div class="collapsible-body" id="divaccrdFinance">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <div class="form-section3">

                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">

                                                                            <asp:TextBox ID="txtLastPurchaseCost" CssClass="numeric calculated " runat="server" MaxLength="75" />
                                                                            <label for="txtLastPurchaseCost">
                                                                                <%--<asp:Label ID="Label13" runat="server" Text="Last Purchase Price"></asp:Label></label>--%>
                                                                                <asp:Label ID="Label13" runat="server" Text="Last Billing Price"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="col s56 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">

                                                                            <asp:TextBox ID="txtOHVal" CssClass="calculated numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtOHVal">
                                                                                <asp:Label ID="Label14" runat="server" Text="On Hand Value"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtOOVal" CssClass="calculated numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtOOVal">
                                                                                <asp:Label ID="Label17" runat="server" Text="On Order Value"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>

                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>


                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <div class="form-section3">
                                                            <div class="col s12 m12 l12">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtComittedValue" CssClass="calculated numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtComittedValue">
                                                                                <asp:Label ID="Label18" runat="server" Text="Comitted Value"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="input-field col s12 pdrightgap2">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">GL Sales</label>
                                                                    <asp:DropDownList ID="ddlglsales" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="Inv"
                                                                        runat="server" ControlToValidate="ddlglsales" Display="Static" ErrorMessage="GL Sales is required in Finance tab" InitialValue="0"
                                                                        SetFocusOnError="True" ToolTip="GL Sales is required." CssClass="error"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="input-field col s12 pdrightgap2">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">GL COGS</label>
                                                                    <asp:DropDownList ID="ddlglcogs" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="Inv"
                                                                        runat="server" ControlToValidate="ddlglcogs" Display="Static" ErrorMessage="GL COGS is required in Finance tab" InitialValue="0"
                                                                        SetFocusOnError="True" ToolTip=" GL COGS is required." CssClass="error"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>

                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <div class="form-section3">
                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <label class="drpdwn-label">
                                                                                <asp:Label ID="Label23" runat="server" Text="ABC Class"></asp:Label></label>
                                                                            <asp:DropDownList ID="ddlABC" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s6 m6 l6">
                                                                <div class="checkrow">

                                                                    <asp:CheckBox ID="ckhTaxable" CssClass="filled-in" runat="server" />
                                                                    <label for="ckhTaxable" class="title-check-text">
                                                                        <asp:Label ID="Label24" runat="server" Text="Taxable"></asp:Label></label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                </div>
                            </li>
                            <li>
                                <div id="accrdPurchasing" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Purchasing</div>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <div class="form-section3">
                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtNextPoDate" CssClass="calculated date-picker" runat="server" MaxLength="75" />
                                                                            <label for="txtNextPoDate">
                                                                                <asp:Label ID="Label19" runat="server" Text="Next PO due Date"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtLastUnitCost" CssClass="calculated numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtLastUnitCost">
                                                                                <%--<asp:Label ID="Label20" runat="server" Text="Last Unit Cost"></asp:Label></label>--%>
                                                                                <asp:Label ID="Label20" runat="server" Text="Last Purchase Order Price"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="col s56 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtLastVendor" CssClass="calculated" runat="server" MaxLength="75" />
                                                                            <label for="txtLastVendor">
                                                                                <asp:Label ID="Label32" runat="server" Text="Last Purchase From"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtLastPODate" CssClass="calculated date-picker" runat="server" MaxLength="75" />
                                                                            <label for="txtLastPODate">
                                                                                <asp:Label ID="Label21" runat="server" Text="Last Purchase Date"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtLastReceiptDate" CssClass="calculated date-picker" runat="server" MaxLength="75" />
                                                                            <label for="txtLastReceiptDate">
                                                                                <asp:Label ID="Label22" runat="server" Text="Last Receipt Date"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s6 m6 l6" style="display: none;">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtEAU" CssClass="calculated numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtEAU">
                                                                                <asp:Label ID="Label27" runat="server" Text="EAU"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>

                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <div class="form-section3">
                                                            <div class="input-field col s12 pdrightgap2">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">
                                                                        <asp:Label ID="Label26" runat="server" Text="Commodity"></asp:Label></label>
                                                                    <asp:DropDownList ID="ddlCommodity" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <label class="drpdwn-label">
                                                                                <asp:Label ID="Label33" runat="server" Text="End of Life Date"></asp:Label></label>
                                                                            <asp:TextBox ID="txtEOLDate" TabIndex="4" runat="server" MaxLength="75" />
                                                                            <%--<label for="<%=txtEOLDate.ClientID%>">--%>

                                                                            <%-- </label>--%>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="col s6 m6 l6" style="display: none;">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtInventoryTurns" CssClass="calculated numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtInventoryTurns">
                                                                                <asp:Label ID="Label25" runat="server" Text="Inventory Turns"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtLeadTime" CssClass="numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtLeadTime">
                                                                                <asp:Label ID="Label28" runat="server" Text="Lead Time"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>

                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <div class="form-section3">
                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtMOQ" CssClass="numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtMOQ">
                                                                                <asp:Label ID="Label29" runat="server" Text="Minimum Order Qty"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s6 m6 l6">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtEOQ" CssClass="numeric" runat="server" MaxLength="75" />
                                                                            <label for="estimateNo">
                                                                                <asp:Label ID="Label30" runat="server" Text="Economic Order Qty"></asp:Label></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="col s6 m6 l6" style="display: none;">
                                                                <div class="row">
                                                                    <div class="input-field col s12 pdrightgap">
                                                                        <div class="row">
                                                                            <asp:HyperLink Text="Quote Request" runat="server" ID="linkquoterequestid" ClientIDMode="Static"></asp:HyperLink>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                            <div class="form-section-row">
                                                <div class="row">
                                                    <span id="purchaseerror" title="P/N name is required." class="error">*</span>
                                                </div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>

                                        <div class="form-content-pd">
                                            <div class="col s4 m4 l4 lblsam">
                                                Vendors
                                            </div>
                                            <div class="accrd-buttons">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnaddVendorInfo" runat="server" OnClick="btnaddVendorInfo_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btneditVendorInfo" runat="server" OnClick="btneditVendorInfo_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btndekVendorInfo" runat="server"
                                                        OnClick="btndekVendorInfo_Click">Delete</asp:LinkButton>

                                                </div>

                                            </div>
                                            <div class="grid_container">
                                                <div class="RadGrid RadGrid_Material FormGrid">



                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Vendors" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                        PagerStyle-AlwaysVisible="true"
                                                        ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                        <CommandItemStyle />
                                                        <GroupingSettings CaseSensitive="false" />
                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                            <Columns>

                                                                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                </telerik:GridClientSelectColumn>



                                                                <telerik:GridTemplateColumn HeaderText="VPN" SortExpression="Part" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnid" runat="server" Value='<%#Eval("ID")%>' />
                                                                        <asp:Label ID="lblVPN" runat="server" Text='<%#Eval("Part")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn HeaderText="Vendor Name" SortExpression="Supplier" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVendor" runat="server" Text='<%#Eval("Supplier")%>'></asp:Label>
                                                                        <asp:Label ID="lblVendorID" runat="server" Text='<%#Eval("VendorID")%>' CssClass="hide"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn HeaderText="Vendor Price" SortExpression="Price" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVendorPrice" runat="server" Text='<%#Eval("Price")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn HeaderText="MPN" SortExpression="MPN" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblmpn" runat="server" Text='<%#Eval("MPN")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn HeaderText="Mfg Name" SortExpression="Mfg" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblManufacturerName" runat="server" Text='<%#Eval("Mfg")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn HeaderText="Mfg Price" SortExpression="MfgPrice" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMfgPrice" runat="server" Text='<%#Eval("MfgPrice")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn UniqueName="lblIndexID" Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIndex" runat="server" Text='<%# Container.ItemIndex %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                                                </div>
                                            </div>

                                            <div class="cf"></div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                </div>
                            </li>
                            <li style="display: none;">
                                <div id="accrdInventory" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Inventory</div>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <div class="form-section3">
                                                            <div class="col s12 m12 l12">
                                                                <div class="row">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtAisle" CssClass="integer" runat="server" MaxLength="75" />
                                                                            <label for="txtAisle">Aisle</label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtShelf" CssClass="integer" runat="server" MaxLength="75" />
                                                                            <label for="txtShelf">Shelf</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtBin" CssClass="integer" runat="server" MaxLength="75" />
                                                                    <label for="txtBin">Bin</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtDateLastUsed" CssClass="calculated" runat="server" MaxLength="75" />
                                                                    <label for="txtDateLastUsed">Date Last Used</label>


                                                                    <asp:DropDownList ID="ddlWareHouse" runat="server" CssClass="form-control" Visible="false"></asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Inv"
                                                                        runat="server" ControlToValidate="ddlWareHouse" Display="Dynamic" ErrorMessage="*" InitialValue="0"
                                                                        SetFocusOnError="True" ToolTip="WareHouse name is required." CssClass="error"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                </div>
                            </li>
                            <li>
                                <div id="accrdSales" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Sales</div>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <div class="form-section3">
                                                            <div class="col s12 m12 l12">
                                                                <div class="row">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtPrice1" CssClass="numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtPrice1">Price 1</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s12 m12 l12">
                                                                <div class="row">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtPrice2" CssClass="numeric" TabIndex="4" runat="server" MaxLength="75" />
                                                                            <label for="txtPrice2">Price 2</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s12 m12 l12">
                                                                <div class="row">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtPrice3" CssClass="numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtPrice3">Price 3</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s12 m12 l12">
                                                                <div class="row">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtPrice4" CssClass="numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtPrice4">Price 4</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s12 m12 l12">
                                                                <div class="row">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtPrice5" CssClass="numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtPrice5">Price 5</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col s12 m12 l12">
                                                                <div class="row">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtPrice6" CssClass="numeric" runat="server" MaxLength="75" />
                                                                            <label for="txtPrice5">Price 6</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                </div>
                            </li>
                            <li>
                                <div id="accrdDocument" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Documents</div>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <asp:Panel ID="pnlDocPermission" runat="server">
                                            <asp:Panel ID="pnlDoc" runat="server">
                                                <asp:Panel ID="pnlDocumentButtons" runat="server">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div style="display: none">
                                                                <asp:Label ID="Label7" runat="server" Text="Attach Drawing File(s)"></asp:Label>
                                                                <asp:FileUpload runat="server" ID="flDrawing" ClientIDMode="Static" TabIndex="26" />
                                                            </div>
                                                            <div class="col s12 m12 l12">
                                                                <!--<p>Maximum file upload size 2MB.</p>-->
                                                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="dropify"
                                                                    onchange="AddDocumentClick(this);" />
                                                                <!--data-max-file-size="2M"-->
                                                            </div>
                                                            <div class="col s12 m12 l12">
                                                                <div class="btnlinks">
                                                                    <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False"
                                                                        OnClick="lnkDeleteDoc_Click"
                                                                        OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                                                </div>
                                                                <div class="btnlinks">
                                                                    <asp:LinkButton ID="lnkUploadDoc" runat="server"
                                                                        CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                                        Style="display: none">Upload</asp:LinkButton>

                                                                    <asp:LinkButton
                                                                        ID="lnkPostback" runat="server"
                                                                        CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div class="form-content-pd">
                                                    <div class="grid_container mt-10" >
                                                        <div class="RadGrid RadGrid_Material FormGrid">



                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                PagerStyle-AlwaysVisible="true"
                                                                ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                <CommandItemStyle />
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                    <Columns>

                                                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                        </telerik:GridClientSelectColumn>

                                                                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>


                                                                        <telerik:GridTemplateColumn HeaderText="File Name" SortExpression="filename" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="false"
                                                                                    CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>' OnClientClick="return ViewDocumentClick(this);"
                                                                                    OnClick="lblName_Click" Text='<%# Eval("filename") %>'> 
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn HeaderText="File Type" SortExpression="doctype" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("doctype") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn HeaderText="Mobile Service" SortExpression="MSVisible" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn HeaderText="Remarks" SortExpression="remarks" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRemarrkss" Width="500px" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>



                                                        </div>


                                                    </div>

                                                    <div class="cf"></div>
                                                </div>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </div>
                                    <div style="clear: both;"></div>
                                </div>
                            </li>
                            <li>
                                <div id="accrdTransaction" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Transactions</div>
                                <%--<div id="accrdTransaction" class="collapsible-header accrd active accordian-text-custom" ><i class="mdi-content-content-paste"></i>Transactions</div>--%>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="srchpaneinner">
                                                <div class="srchtitle ser-css2">
                                                    Date
                                                </div>
                                                <div class="srchinputwrap">
                                                    <asp:TextBox ID="txtInvDtFrom" runat="server" CssClass="datepicker_mom"
                                                        MaxLength="50"></asp:TextBox>

                                                </div>
                                                <div class="srchinputwrap">

                                                    <asp:TextBox ID="txtInvDtTo" runat="server" CssClass="datepicker_mom"
                                                        MaxLength="50"></asp:TextBox>

                                                </div>
                                                <div class="srchinputwrap srchclr btnlinksicon">
                                                    <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false" ToolTip="Search"
                                                        OnClick="lnkSearch_Click"><i class="mdi-action-search"></i></asp:LinkButton>

                                                </div>
                                                <div class="col lblsz2 lblszfloat">
                                                    <div class="row">
                                                        <span class="tro trost accrd-trost">

                                                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="btnClear_Click"
                                                                CausesValidation="False">Clear</asp:LinkButton>
                                                        </span>
                                                        <span class="tro trost accrd-trost">
                                                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click" Visible="false"
                                                                CausesValidation="False">Show All Adjustments</asp:LinkButton>
                                                        </span>
                                                        <span class="tro trost accrd-trost">
                                                            <asp:Label ID="lblRecordCount" runat="server" Visible="false"></asp:Label>
                                                        </span>


                                                    </div>
                                                </div>

                                                <%--<div class="grid_container" style="margin-top: 10px;">
                                                     <div class="RadGrid RadGrid_Material FormGrid">--%>
                                                <div class="grid_container">
                                                    <div class="form-section-row m-b-0" >
                                                        <div class="RadGrid RadGrid_Material FormGrid">
                                                            <telerik:RadCodeBlock ID="RadCodeBlock_Tran" runat="server">
                                                                <script type="text/javascript">
                                                                    function pageLoad() {
                                                                        var grid = $find("<%= RadGridTran.ClientID %>");
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

                                                            <%--<telerik:RadAjaxPanel  id="transtab1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Inv">--%>
                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel_Tran" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Inv" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGridTran" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                    PagerStyle-AlwaysVisible="true"
                                                                    ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="false" Width="100%" AllowCustomPaging="true" OnPreRender="RadGrid_RadGridTran_PreRender" OnNeedDataSource="RadGrid_RadGridTran_NeedDataSource" OnItemCreated="RadGrid_RadGridTran_ItemCreated">
                                                                    <CommandItemStyle />
                                                                    <GroupingSettings CaseSensitive="false" />
                                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                        <Selecting AllowRowSelect="false"></Selecting>
                                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                    </ClientSettings>

                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                                                        <Columns>


                                                                            <telerik:GridTemplateColumn HeaderText="Ref" DataField="Ref" HeaderStyle-Width="80" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="RefID" ShowFilterIcon="false">

                                                                                <ItemTemplate>
                                                                                    <a id="lblId" style="color: blue!important;" href='<%# Eval("URLref") %>' target="_blank" runat="server"><%# Eval("Ref") %> </a>
                                                                                </ItemTemplate>

                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="80" DataField="fDate" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fDate" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblfDate" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):""%> ' runat="server" />
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="150" DataField="TType" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="TType" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("TType") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>



                                                                            <telerik:GridTemplateColumn HeaderText="Quan" HeaderStyle-Width="80" DataField="Quan" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Quan" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblQuan" runat="server" Text='<%# Eval("Quan") %>'
                                                                                        ForeColor='<%# Convert.ToDouble(Eval("Quan"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="200" DataField="MDesc" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="MDesc" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("MDesc") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Increase" HeaderStyle-Width="80" DataField="Charges" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Charges" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Charges", "{0:c}")%>'
                                                                                        ForeColor='<%# Convert.ToDouble(Eval("Charges"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Decrease" HeaderStyle-Width="80" DataField="Credits" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Credits" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDate1" runat="server"
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Credits", "{0:c}")%>'
                                                                                        ForeColor='<%# Convert.ToDouble(Eval("Charges"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Balance" HeaderStyle-Width="80" DataField="Balance" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Balance" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'></asp:Label>
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
    <telerik:RadWindowManager ID="RadWindowManagerInv" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowWarehouse" Skin="Material" VisibleTitlebar="true" Title="Warehouse" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="300" Height="200">
                <ContentTemplate>
                    <div>
                        <asp:HiddenField ID="hdnInvWarehouseID" runat="server" ClientIDMode="Static" />
                        <%--<asp:Repeater ID="rptWarehouse" runat="server" OnItemDataBound="rpt_ItemDataBound">--%>
                        <%--OnItemDataBound="repeater_ItemDataBound"--%>
                        <asp:Repeater ID="rptWarehouse" runat="server">
                            <HeaderTemplate>
                                <div>
                                    <asp:CheckBox ID="chkwarehouseall" runat="server" Text="select all" onclick="checkall(this)" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>


                                    <asp:CheckBox ID="chkWarehouseName" runat="server" Text='<%# Eval("Name") %>' />
                                    <%--Checked='<%# DataBinder.Eval(Container.DataItem,"Name") %>' --%>

                                    <%--  <asp:HiddenField ID="hdnWarehouseID" runat="server" Value='<%# Eval("ID") %>' />--%>
                                    <asp:Label ID="lblWarehouseID" runat="server" Text='<%# Eval("ID") %>' Style="display: none;"></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                    </div>
                    <div style="clear: both;"></div>

                    <div class="btnlinks">
                        <asp:LinkButton ID="lnkSaveInvMergeWarehouse" runat="server" OnClick="lnkSaveInvMergeWarehouse_Click" CausesValidation="false"
                            ValidationGroup="invware" ClientIDMode="Static">Save Changes</asp:LinkButton>

                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowRev" Skin="Material" VisibleTitlebar="true" Title="Revision Details" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="400" Height="470">
                <ContentTemplate>
                    <div>
                        <asp:HiddenField ID="HiddenRevisionID" runat="server" />
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtRevisionItemRevDate" EnableViewState="true" ViewStateMode="Enabled" runat="server" CssClass="datepicker_mom" MaxLength="25"></asp:TextBox>
                                <asp:Label runat="server" ID="lbltxtRevisionItemRevDate" AssociatedControlID="txtRevisionItemRevDate">Date</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtVersion" EnableViewState="true" ViewStateMode="Enabled" runat="server" MaxLength="25"></asp:TextBox>
                                <asp:Label runat="server" ID="lbltxtVersion" AssociatedControlID="txtVersion">Version</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtEco" EnableViewState="true" ViewStateMode="Enabled" runat="server" MaxLength="25"></asp:TextBox>
                                <asp:Label runat="server" ID="lbltxtEco" AssociatedControlID="txtEco">Eco</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtDrawing" EnableViewState="true" ViewStateMode="Enabled" runat="server" MaxLength="25"></asp:TextBox>
                                <asp:Label runat="server" ID="lbltxtDrawing" AssociatedControlID="txtDrawing">Drawing</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtComment" EnableViewState="true" ViewStateMode="Enabled" runat="server" MaxLength="25"></asp:TextBox>
                                <asp:Label runat="server" ID="lbltxtComment" AssociatedControlID="txtComment">Comment</asp:Label>
                            </div>
                        </div>

                    </div>
                    <div style="clear: both;"></div>

                    <div class="btnlinks">
                        <asp:LinkButton ID="lnkSaveRevisionDetail" runat="server" OnClick="lnkSaveRevisionDetail_Click">Save Changes</asp:LinkButton>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowVendor" Skin="Material" VisibleTitlebar="true" Title="Approved Vendor Info" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="530">
                <ContentTemplate>
                    <div>
                        <asp:HiddenField ID="hdninvvendinfo" runat="server" ClientIDMode="Static" />
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rqInventoryMPN" runat="server" ControlToValidate="txtInventoryMPN"
                                    Display="None" ErrorMessage="MPN Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                    TargetControlID="rqInventoryMPN">
                                </asp:ValidatorCalloutExtender>
                                <asp:TextBox ID="txtInventoryMPN" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                <asp:Label runat="server" ID="Label1" AssociatedControlID="txtInventoryMPN">MPN</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rqVendorPartNumber" runat="server" ControlToValidate="txtVendorPartNumber"
                                    Display="None" ErrorMessage="Manufacturer Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender8" runat="server" Enabled="True"
                                    TargetControlID="rqVendorPartNumber">
                                </asp:ValidatorCalloutExtender>

                                <asp:TextBox ID="txtVendorPartNumber" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                <asp:Label runat="server" ID="Label2" AssociatedControlID="txtVendorPartNumber">VPN</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rqApprovedVendor" runat="server" ControlToValidate="ddlInventoryApprovedVendor"
                                    Display="None" ErrorMessage="Manufacturer Required" SetFocusOnError="True" ValidationGroup="invware" InitialValue="0"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                    TargetControlID="rqApprovedVendor">
                                </asp:ValidatorCalloutExtender>

                                <label class="drpdwn-label">Vendor</label>
                                <asp:DropDownList ID="ddlInventoryApprovedVendor" CssClass="browser-default" runat="server" ClientIDMode="Static"></asp:DropDownList>

                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rqVendorPrice" runat="server" ControlToValidate="txtVendorPrice"
                                    Display="None" ErrorMessage="Price Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender9" runat="server" Enabled="True"
                                    TargetControlID="rqVendorPrice">
                                </asp:ValidatorCalloutExtender>
                                <asp:TextBox ID="txtVendorPrice" runat="server" MaxLength="30" ClientIDMode="Static" onkeypress="return isDecimalKey(this,event);"></asp:TextBox>
                                <asp:Label runat="server" ID="Label4" AssociatedControlID="txtVendorPrice">Vendor Price</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rqApprovedManufacturer" runat="server" ControlToValidate="txtInventoryApprovedManufacturer"
                                    Display="None" ErrorMessage="Approved Manufacturer Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                    TargetControlID="rqApprovedManufacturer">
                                </asp:ValidatorCalloutExtender>
                                <asp:TextBox ID="txtInventoryApprovedManufacturer" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                <asp:Label runat="server" ID="Label5" AssociatedControlID="txtInventoryApprovedManufacturer">Manufacturer</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rqManufacturerPrice" runat="server" ControlToValidate="txtManufacturerPrice"
                                    Display="None" ErrorMessage="Price Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender10" runat="server" Enabled="True"
                                    TargetControlID="rqManufacturerPrice">
                                </asp:ValidatorCalloutExtender>
                                <asp:TextBox ID="txtManufacturerPrice" runat="server" MaxLength="30" ClientIDMode="Static" onkeypress="return isDecimalKey(this,event);"></asp:TextBox>
                                <asp:Label runat="server" ID="Label3" AssociatedControlID="txtManufacturerPrice">Mfg Price</asp:Label>
                            </div>
                        </div>

                    </div>
                    <div style="clear: both;"></div>

                    <div class="btnlinks">
                        <asp:LinkButton ID="lnkSaveInventoryWarehouse" runat="server"
                            ValidationGroup="invware" OnClick="lnkSaveInventoryWarehouse_Click" ClientIDMode="Static">Save Changes</asp:LinkButton>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowRequestQuote" Skin="Material" VisibleTitlebar="true" Title="Approved Vendor Info" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="530">
                <ContentTemplate>
                    <div>
                        <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                        <div class="input-field col s12">
                            <div class="row">
                                <label class="drpdwn-label">Approved Vendor</label>
                                <asp:RequiredFieldValidator ID="rqApprovedVendorrequestquote" runat="server" ControlToValidate="ddlApprovedVendorrequestquote"
                                    Display="None" ErrorMessage="Approved Manufacturer Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote" InitialValue="0"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                    TargetControlID="rqApprovedVendorrequestquote">
                                </asp:ValidatorCalloutExtender>
                                <asp:DropDownList ID="ddlApprovedVendorrequestquote" CssClass="browser-default" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">

                                <label class="drpdwn-label">MPN & Manufacturer </label>
                                <asp:RequiredFieldValidator ID="rqmpnrequestquote" runat="server" ControlToValidate="ddlMPNMannufacturer"
                                    Display="None" ErrorMessage="MPN Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote" InitialValue="0"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True"
                                    TargetControlID="rqmpnrequestquote">
                                </asp:ValidatorCalloutExtender>
                                <asp:DropDownList ID="ddlMPNMannufacturer" CssClass="browser-default" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rqemail" runat="server" ControlToValidate="txtvendoremail"
                                    Display="None" ErrorMessage="Vendor email is required" SetFocusOnError="True" ValidationGroup="invwarerequestquote"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True"
                                    TargetControlID="rqemail">
                                </asp:ValidatorCalloutExtender>
                                <asp:TextBox ID="txtvendoremail" runat="server" ClientIDMode="Static" TextMode="Email" CssClass="form-control"></asp:TextBox>
                                <asp:Label runat="server" ID="Label6" AssociatedControlID="txtvendoremail">Email</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rqquantity" runat="server" ControlToValidate="txtquotequantity"
                                    Display="None" ErrorMessage="Quantity is Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote" InitialValue="0"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" Enabled="True"
                                    TargetControlID="rqquantity">
                                </asp:ValidatorCalloutExtender>

                                <asp:TextBox ID="txtquotequantity" runat="server" ClientIDMode="Static" CssClass="numeric" Width="100px"></asp:TextBox>
                                <asp:Label runat="server" ID="Label9" AssociatedControlID="txtquotequantity">   Quantity </asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtemailcontentent" runat="server" ClientIDMode="Static"></asp:TextBox>
                                <asp:Label runat="server" ID="Label10" AssociatedControlID="txtemailcontentent">Email Content</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">


                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtmpnrequestquote"
                                    Display="None" ErrorMessage="MPN Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender11" runat="server" Enabled="True"
                                    TargetControlID="rqmpnrequestquote">
                                </asp:ValidatorCalloutExtender>
                                <asp:TextBox ID="txtmpnrequestquote" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                <asp:Label runat="server" ID="Label11" AssociatedControlID="txtmpnrequestquote">MPN</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rqApprovedManufacturerrequestquote" runat="server" ControlToValidate="txtApprovedManufacturerrequestquote"
                                    Display="None" ErrorMessage="Approved Manufacturer Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender12" runat="server" Enabled="True"
                                    TargetControlID="rqApprovedManufacturerrequestquote">
                                </asp:ValidatorCalloutExtender>

                                <asp:TextBox ID="txtApprovedManufacturerrequestquote" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                <asp:Label runat="server" ID="Label8" AssociatedControlID="txtApprovedManufacturerrequestquote">  Approved Manufacturer</asp:Label>
                            </div>
                        </div>

                    </div>
                    <div style="clear: both;"></div>

                    <div class="btnlinks">
                        <asp:LinkButton ID="lnksendRequestQuote" runat="server"
                            ValidationGroup="invwarerequestquote" ClientIDMode="Static">Send Quote</asp:LinkButton>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>


    <div class="page-content" style="display: none;">

        <div class="page-cont-top">

            <div class="page-bar-right">
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">


            <div class="col-lg-12 col-md-12">

                <div class="com-cont">
                    <div class="col-md-5 col-lg-5 w-100p">
                        <div class="form-col">
                            <asp:ValidationSummary
                                ID="ValidationSummary1"
                                runat="server"
                                HeaderText="Following error occurs....."
                                ShowMessageBox="false"
                                DisplayMode="BulletList"
                                ShowSummary="true"
                                BackColor="Snow"
                                ForeColor="Red"
                                Font-Size="Small"
                                ValidationGroup="Inv" />
                            <div class="fc-label-main">



                                <div class="fc-label">
                                    Part Number                                                          
                                </div>
                                <div class="fc-input">


                                    <%--  <asp:dropdownlist id="ddlHeaderNameName" runat="server" cssclass="name form-control" onselectedindexchanged="ddlHeaderNameName_SelectedIndexChanged" autopostback="true" tabindex="3"></asp:dropdownlist>--%>
                                </div>
                            </div>
                            <div class="fc-label-main">

                                <%--  <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True"
                                                            TargetControlID="RequiredFieldValidator1">
                                                        </asp:ValidatorCalloutExtender>--%>
                                <div class="fc-label">
                                    Description
                                </div>
                                <div class="fc-input">
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">

                <div class="com-cont">
                    <div class="sc-form">

                        <label>On Hand</label>

                        <label>On Order</label>

                        <label>Committed</label>

                        <label>Avaliable</label>

                        <label>Issued to Open jobs</label>

                        <%--<input type="button" class="btn blue"  title="Details" name="Details" />--%>
                    </div>
                    <div class="clearfix"></div>
                    <div class="p-t-30">

                        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" ClientIDMode="Static">
                            <asp:TabPanel runat="server" ID="tpItemHeader" HeaderText="Item Header" ClientIDMode="Static">


                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Item Header
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <%--<asp:UpdatePanel runat="server" ID="Updateform" UpdateMode="Conditional">

                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlHeaderNameName"  /></Triggers>
                                        <ContentTemplate>--%>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5 w-100p">
                                                <div class="form-col">

                                                    <div class="fc-label-main">

                                                        <%--  <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True" BehaviorID="b2"
                                                            TargetControlID="rfvUom" ClientIDMode="Static">
                                                        </asp:ValidatorCalloutExtender>--%>
                                                        <div class="fc-label">
                                                            UOM
                                                        </div>
                                                        <div class="fc-input">



                                                            <%-- <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                                                PopupPosition="Right" TargetControlID="rfvUom" />--%>
                                                        </div>
                                                    </div>
                                                    <div class="fc-label-main">

                                                        <div class="fc-label">
                                                            Status
                                                        </div>
                                                        <div class="fc-input">
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-col">
                                                    <%--<div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Description 2
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtDes2" CssClass="form-control" TabIndex="5" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>--%>
                                                    <div class="fc-label-main">

                                                        <div class="fc-label">
                                                            Date Created
                                                        </div>
                                                        <div class="fc-input">
                                                        </div>
                                                    </div>
                                                    <div class="fc-label-main">
                                                        <div class="fc-label">
                                                            Category
                                                        </div>
                                                        <div class="fc-input">
                                                        </div>
                                                    </div>
                                                </div>
                                                <%--<div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Description 3
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtDes3" CssClass="form-control" TabIndex="6" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Description 4
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtDes4" CssClass="form-control" TabIndex="7" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>
                                                </div>--%>
                                                <div class="form-col">

                                                    <%--<div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Custom 1
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtCustom1" CssClass="form-control" TabIndex="15" runat="server" MaxLength="75" />
                                                        </div>
                                                    </div>--%>

                                                    <div class="fc-label-main">
                                                        <div class="fc-label">
                                                            Remarks
                                                        </div>
                                                        <div class="fc-input">
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-col">
                                                </div>
                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                    <%-- </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="TblWareHouse" HeaderText="WareHouse" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Warehouse
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5 w-100p">
                                                <div class="form-col">
                                                    <div class="pc-titlesmall-main">


                                                        <div class="pc-titlesmall">

                                                            <ul class="lnklist-header">

                                                                <li></li>
                                                                <%-- <li>


                                                                    </li>--%>
                                                                <li>
                                                                    <%-- <a class="icon-delete" title="Delete" id="btndekVendorInfo"></a>--%>
                                                                  
                                                                </li>

                                                            </ul>
                                                        </div>
                                                        <asp:Panel runat="server" Height="150px" ScrollBars="Vertical">
                                                        </asp:Panel>


                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>
                                    </div>

                                    <!-- edit-tab end -->
                                    <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel ID="tpEngineering" runat="server" HeaderText="Engineering" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Engineering
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5 w-100p">
                                                <%--<div class="form-col">
                                                    <div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            Part Number
                                                        </div>
                                                        <div class="fc-input">

                                                            <asp:TextBox ID="txtEngineeringName" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false" TabIndex="17"></asp:TextBox>
                                                            <asp:DropDownList ID="ddlEngineeringName" runat="server" CssClass="name form-control" Enabled="false" TabIndex="17"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 60%;">

                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label3" runat="server" Text="Revision"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtRevision" CssClass="form-control input-sm input-small" TabIndex="22" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label4" runat="server" Text="Revision Date"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtRevisionDate" CssClass="form-control input-sm input-small date-picker" TabIndex="23" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label5" runat="server" Text="ECO #"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtECO" CssClass="form-control input-sm input-small" TabIndex="24" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label6" runat="server" Text="Drawing #"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtDrawing" CssClass="form-control input-sm input-small" TabIndex="25" runat="server" ClientIDMode="Static" />
                                                                </td>

                                                            </tr>

                                                        </table>


                                                    </div>
                                                </div>--%>
                                                <%--<div class="form-col">
                                                    <div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            Description
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtEngineeringDescription" CssClass="form-control" TabIndex="18" runat="server" MaxLength="75" Enabled="false" ClientIDMode="Static" />

                                                        </div>
                                                    </div>

                                                    <div style="float: left; width: 60%; padding-left: 10px;">

                                                       


                                                    </div>
                                                </div>--%>
                                                <div class="form-col">
                                                    <div class="sp-lable">
                                                        <div class="fc-label">
                                                            Specification
                                                        </div>
                                                        <div class="fc-input">
                                                        </div>
                                                    </div>
                                                    <div class="sp-lable-table">

                                                        <table>
                                                            <tr>
                                                                <td></td>
                                                                <td class="p-r-10"></td>
                                                                <%--<td style="padding-right: 10px;">
                                                                    <asp:CheckBox Text="Insp Required" runat="server" TextAlign="Left" ID="chkInspRequired" TabIndex="33" />
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:CheckBox Text="CoC Required" runat="server" TextAlign="Left" ID="chkCoCpRequired" TabIndex="34" />
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:CheckBox Text="Serialization Required" runat="server" TextAlign="Left" ID="chkSerializationRequired" TabIndex="35" />
                                                                </td>--%>
                                                            </tr>

                                                        </table>


                                                    </div>
                                                    <%--<div style="float: left; width: 60%; padding-left: 10px;">

                                                     
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblReference" runat="server" Text="Reference"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px; width: 100%;">
                                                                    <asp:TextBox ID="txtReference" CssClass="form-control" TabIndex="27" runat="server" MaxLength="75" />
                                                                </td>

                                                            </tr>

                                                        </table>

                                                    </div>--%>
                                                </div>
                                                <div class="form-col">
                                                    <%--<div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            Specification 2
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtSpecification2" CssClass="form-control" TabIndex="19" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>--%>
                                                    <div class="re-lable-main">
                                                        <div class="fc-label">
                                                            Revision Detail
                                                        </div>
                                                        <table class="re-lable-main-table">
                                                            <tr>
                                                                <td>
                                                                    <div class="table-scrollable table-sub-scroll">
                                                                        <asp:Panel ID="Panel20" runat="server">
                                                                            <div style="background-color: #316b9d">
                                                                                <ul class="lnklist-header lnklist-panel">
                                                                                    <li></li>


                                                                                </ul>
                                                                            </div>
                                                                        </asp:Panel>




                                                                    </div>
                                                                </td>
                                                            </tr>

                                                        </table>



                                                        <%--<table style="width: 100%;">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label8" runat="server" Text="Length"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtLength" CssClass="numeric form-control" TabIndex="28" runat="server" Style="width: 100px;" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label9" runat="server" Text="Width"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtWidth" CssClass="numeric form-control" TabIndex="29" runat="server" Style="width: 100px;" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label10" runat="server" Text="Height"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtHeight" CssClass="numeric form-control" TabIndex="30" runat="server" Style="width: 100px;" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label11" runat="server" Text="Weight"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtWeight" CssClass="numeric form-control" TabIndex="31" runat="server" Style="width: 100px;" />
                                                                </td>

                                                            </tr>

                                                        </table>--%>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <%--<div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            Specification 3
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtSpecification3" CssClass="form-control" TabIndex="20" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>--%>
                                                </div>
                                                <div class="form-col">
                                                    <%--<div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            Specification 4
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtSpecification4" CssClass="form-control" TabIndex="21" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>--%>
                                                    <div class="re-lable-main" >
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpFinance" HeaderText="Finance" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Finance
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5 w-100p">
                                                <div class="form-col">
                                                    <%--  <div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Part Number
                                                        </div>
                                                        <div class="fc-input">

                                                            <asp:TextBox ID="txtFinanceName" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                            <asp:DropDownList ID="ddlFinanceName" runat="server" CssClass="name form-control" Enabled="false"></asp:DropDownList>
                                                        </div>
                                                    </div>--%>
                                                    <div class="re-lable-main1" style="float: left; width: 50%; padding-left: 10px;">

                                                        <table>
                                                            <tr>
                                                                <td class="table-p-w" style="padding-right: 10px; width: 25%;"></td>
                                                                <td class="table-p-w" style="padding-right: 10px; width: 25%;"></td>
                                                                <td class="table-p-w" style="padding-right: 10px; width: 25%;"></td>
                                                                <td class="w-25p" style="width: 25%;"></td>


                                                            </tr>

                                                        </table>


                                                    </div>
                                                    <div class="re-lable-main1">

                                                        <table>
                                                           <tr>
                                                                <td class="table-p-w"></td>
                                                                <td class="table-p-w"></td>
                                                                <td class="table-p-w"></td>
                                                                <td class="w-25p" style="width: 25%;"></td>


                                                            </tr>

                                                        </table>


                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <%-- <div style="float: left; width: 50%;">

                                                        <div class="fc-label">
                                                            Description
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtFinanceDescription" CssClass="form-control" TabIndex="18" runat="server" MaxLength="75" Enabled="false" ClientIDMode="Static" />

                                                        </div>
                                                    </div>--%>

                                                    <div class="re-lable-main1">

                                                        <table>
                                                            <tr>
                                                                <td class="table-p-w" ></td>
                                                                <td class="table-p-w" >

                                                                    <%--<asp:DropDownList ID="ddlLastPurchaseFromVendor" runat="server" CssClass="calculated form-control"></asp:DropDownList>--%>
                                                                </td>
                                                                <td class="table-p-w" ></td>
                                                                <td class="w-25p"></td>

                                                            </tr>

                                                        </table>


                                                    </div>
                                                    <div class="re-lable-main1">

                                                        <table>
                                                            <tr>
                                                                <td class="table-p-w15"></td>
                                                                <td class="table-p-w50"></td>


                                                            </tr>

                                                        </table>


                                                    </div>

                                                </div>


                                                <div class="form-col">
                                                    <div class="fc-label-main" >
                                                        <%-- <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True" BehaviorID="b3" ClientIDMode="Static"
                                                            TargetControlID="RequiredFieldValidator4">
                                                        </asp:ValidatorCalloutExtender>--%>

                                                        <div class="fc-label">
                                                            GL Sales
                                                             
                                                        </div>
                                                        <div class="fc-input">
                                                        </div>

                                                    </div>
                                                    <div class="fc-label-main" >

                                                        <%--  <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" Enabled="True" BehaviorID="b4" ClientIDMode="Static"
                                                            TargetControlID="RequiredFieldValidator5">
                                                        </asp:ValidatorCalloutExtender>--%>
                                                        <div class="fc-label">
                                                            GL COGS
                                                        </div>
                                                        <div class="fc-input">
                                                        </div>
                                                    </div>

                                                </div>


                                                <div class="form-col">
                                                    <div class="fc-label-main">
                                                        <table>
                                                            <tr>
                                                                <td class="table-p-w16"></td>
                                                                <td class="table-p-w16"></td>
                                                                <td class="table-p-w10"></td>
                                                                <td class="table-p-w10"></td>


                                                            </tr>

                                                        </table>
                                                    </div>
                                                    <div class="re-lable-main1" >
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpPurchasing" HeaderText="Purchasing" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Purchasing
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5 w-100p" >
                                                <div class="form-col">
                                                    <%--  <div style="float: left; clear: right; width: 50%;">
                                                        <div class="fc-label">
                                                            Part Number
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtPurchasingName" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>

                                                            <asp:DropDownList ID="ddlPurchasingName" runat="server" CssClass="name form-control" Enabled="false"></asp:DropDownList>
                                                        </div>
                                                    </div>--%>
                                                    <div class="table-clear-w50" >
                                                        <table class="w-100p">
                                                            <tr>
                                                                <td class="pr-3></td>
                                                                <td class="pr-3"></td>
                                                                <td class="pr-3"></td>
                                                                <td class="pr-3"></td>
                                                                <td class="pr-3"></td>
                                                                <td style=""></td>


                                                            </tr>

                                                        </table>
                                                    </div>
                                                    <div class="table-clear-w50-p">
                                                        <table class="w-100p">
                                                            <tr>
                                                                <td class="pr-3-w15"></td>
                                                                <td class="pr-3-w15"></td>
                                                                <td class="pr-3-w15"></td>
                                                                <td class="w15p"></td>
                                                            </tr>

                                                        </table>

                                                    </div>


                                                </div>
                                                <%--   <div class="form-col">
                                                    <div style="float: left; width: 50%;">

                                                        <div class="fc-label">
                                                            Description
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtPurchasingDescription" CssClass="form-control" TabIndex="18" runat="server" MaxLength="75" Enabled="false" ClientIDMode="Static" />

                                                        </div>
                                                    </div>

                                                    

                                                </div>--%>
                                                <div class="form-col">
                                                    <div class="table-clear-w50">


                                                        <div class="fc-label">
                                                            Vendor
                                                        </div>
                                                        <div class="fc-input">
                                                            <div class="pc-titlesmall">

                                                                <ul class="lnklist-header">

                                                                    <li></li>
                                                                    <li></li>
                                                                    <li>
                                                                        <%-- <a class="icon-delete" title="Delete" id="btndekVendorInfo"></a>--%>
                                                                      
                                                                    </li>

                                                                </ul>
                                                            </div>
                                                            <asp:Panel ScrollBars="Vertical" runat="server" Height="120px">
                                                            </asp:Panel>


                                                        </div>
                                                    </div>

                                                    <div class="table-clear-w50-p" style="float: right; clear: right; width: 50%; padding-left: 5px;">
                                                        <table class="w-100p" >

                                                            <tr>
                                                                <td class="pr-3-w15" ></td>
                                                                <td class="pr-3-w15" >
                                                                    <%--<asp:TextBox ID="txtCommodity" CssClass="calculated form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />--%>
                                                                    
                                                                </td>
                                                                <td class="pr-3-w15" ></td>
                                                                <td class="pr-3-w15" ></td>
                                                                <td class="pr-3-w15"></td>
                                                                <td class="w15p"></td>

                                                            </tr>
                                                            <tr style="height: 10px;"></tr>
                                                            <tr style="height: 10px;"></tr>
                                                            <tr>
                                                                <td class="pr-3-w15"></td>
                                                                <td class="pr-3-w15"></td>
                                                                <td colspan="2"></td>
                                                                <td class="table-p-w16" ></td>
                                                                <td class="table-p-w16"></td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </div>
                                                <div class="form-col">
                                                    <div class="table-clear-w50" >
                                                        <div class="pc-titlesmall-main" >
                                                            <table>

                                                                <tr>
                                                                    <td class="fc-label"></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>

                                                                </tr>

                                                            </table>
                                                        </div>
                                                    </div>
                                                    <%--<div style="float: right; clear: right; width: 50%; padding-left: 5px;">
                                                        <div style="float: left; width: 100%;">
                                                            <table>
                                                                <tr>
                                                                    <td style="padding-right: 3px; width: 20%">
                                                                        <asp:Label ID="Label31" runat="server" Text="Purchase Transaction History"></asp:Label>

                                                                    </td>
                                                                    <td style="padding-right: 3px; width: 15%">
                                                                        <asp:TextBox ID="TextBox5" CssClass="form-control" TabIndex="4" runat="server" MaxLength="75" />
                                                                    </td>
                                                                    <td style="padding-right: 3px; width: 15%">
                                                                        <asp:TextBox ID="TextBox6" CssClass="form-control" TabIndex="4" runat="server" MaxLength="75" />
                                                                    </td>
                                                                    <td style="padding-right: 3px; width: 15%">
                                                                        <a class="btn submit"><i class="fa fa-search"></i></a>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>--%>
                                                </div>
                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpInventory" HeaderText="Inventory" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Inventory
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5" style="width: 75%">

                                                <%--  <div class="form-col">

                                                    <div class="fc-label">
                                                        Part Number
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtInventoryName" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlInventoryName" runat="server" CssClass="name form-control" Enabled="false"></asp:DropDownList>
                                                    </div>

                                                </div>
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        Description
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtInventoryDescription" CssClass="form-control" TabIndex="18" runat="server" MaxLength="75" Enabled="false" ClientIDMode="Static" />
                                                    </div>
                                                </div>--%>

                                                <div class="form-col">

                                                    <div class="fc-label">
                                                        <%--  WareHouse--%>
                                                    </div>
                                                    <div class="fc-input">
                                                    </div>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Aisle
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">
                                                                </div>

                                                            </td>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Shelf
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">
                                                                </div>

                                                            </td>

                                                            <td>
                                                                <div class="fc-label">
                                                                    Bin
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">
                                                                </div>

                                                            </td>

                                                        </tr>

                                                    </table>



                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Date Last Used
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">
                                                                </div>

                                                            </td>
                                                            <%--<td>
                                                                <div class="fc-label">
                                                                    Shelf Life
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtShelfLife2" CssClass="numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>--%>
                                                        </tr>

                                                    </table>



                                                </div>

                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpSales" HeaderText="Sales" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Sales
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5" style="width: 75%">

                                                <%--<div class="form-col">

                                                    <div class="fc-label">
                                                        Part Number
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtSalesName" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlSalesName" runat="server" CssClass="name form-control" Enabled="false"></asp:DropDownList>
                                                    </div>

                                                </div>

                                                <div class="form-col">

                                                    <div class="fc-label">
                                                        Description
                                                    </div>
                                                    <div class="fc-input">

                                                        <asp:TextBox ID="txtSalesDescription" CssClass="form-control" TabIndex="4" runat="server" ClientIDMode="Static" Enabled="false" />
                                                    </div>

                                                </div>--%>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 1
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">
                                                                </div>

                                                            </td>
                                                            <%-- <td>
                                                                <div class="fc-label">
                                                                    Max Discount %
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtMaxDiscount" CssClass="numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>--%>
                                                        </tr>

                                                    </table>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 2
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">
                                                                </div>

                                                            </td>
                                                            <%--  <td>
                                                                <div class="fc-label">
                                                                    Last Sales Price
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtLastSalesPrice" CssClass="calculated numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>--%>
                                                        </tr>

                                                    </table>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 3
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">
                                                                </div>

                                                            </td>
                                                            <%--<td>
                                                                <div class="fc-label">
                                                                    Annual Sales Quantity
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtAnnualSalesQuantity" CssClass="calculated numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>--%>
                                                        </tr>

                                                    </table>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 4
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">
                                                                </div>

                                                            </td>
                                                            <%--<td>
                                                                <div class="fc-label">
                                                                    Annual Sales $
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtAnnualSales" CssClass="calculated numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>--%>
                                                        </tr>

                                                    </table>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 5
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">
                                                                </div>

                                                            </td>


                                                        </tr>

                                                    </table>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 6
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">
                                                                </div>

                                                            </td>


                                                        </tr>

                                                    </table>

                                                </div>

                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpDocuments" HeaderText="Documents" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Documents
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5" style="width: 75%">
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%; display: none;">
                                                        <table>
                                                            <tr>

                                                                <td style="padding-right: 10px;"></td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <table>
                                                            <tr>
                                                                <td>


                                                                    <ul class="lnklist-header lnklist-panel">
                                                                        <li>

                                                                        <li></li>
                                                                        <li>
                                                                            <table style="float: left; margin-right: 20px; margin-left: 130px; margin-top: 0"
                                                                                cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td></td>
                                                                                </tr>
                                                                            </table>
                                                                        </li>
                                                                    </ul>





                                                                </td>

                                                                <%--<td>
                                                                    <asp:Label ID="lblatt" runat="server" Text="Attach"></asp:Label>

                                                                </td>
                                                                <td>
                                                                    <asp:FileUpload ID="flattachment" runat="server" Style="width: 80px;" TabIndex="8" />
                                                                </td>--%>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </div>


                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tbTransactionsere" HeaderText="Transactions" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Transactions
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="search-customer" style="padding: 10px 0 10px 0">
                                        <div class="sc-form">
                                            Date From
                                                
                                           
                                            Date To
                                               
                                            <%-- <asp:ImageButton ID="lnkSearch" runat="server" ImageUrl="images/search.png"
                                                    OnClick="lnkSearch_Click" ToolTip="Search" CausesValidation="False"></asp:ImageButton>--%>

                                            <ul style="padding: 0 5px 0px 5px">
                                            </ul>
                                        </div>

                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="table-scrollable">
                                        <div class="pc-titlesmall">

                                            <ul class="lnklist-header">

                                                <li>
                                                    <asp:LinkButton CssClass="icon-addnew" ID="LinkButton3" Style="display: none;" ToolTip="Add New" runat="server" OnClick="btnAddMergeWarehouse_Click"
                                                        ClientIDMode="Static"></asp:LinkButton>
                                                </li>


                                            </ul>
                                        </div>

                                    </div>
                                    <%--<div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5" style="width: 75%">
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">

                                                        <table>
                                                            <tr>

                                                                   <td>
                                                                    <asp:Label ID="lbltranshistory" runat="server" Text="Transaction History"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txttranhistorystartdate" CssClass="form-control input-sm input-small date-pickerer" TabIndex="9" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txttranhistoryenddate" CssClass="form-control input-sm input-small date-picker" TabIndex="10" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td>

                                                                    <a class="btn submit"><i class="fa fa-search"></i>
                                                                    </a>


                                                                </td>

                                                                <td></td>


                                                            </tr>

                                                        </table>


                                                    </div>
                                                </div>
                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>--%>
                                </ContentTemplate>
                            </asp:TabPanel>


                        </asp:TabContainer>

                    </div>



                    <div class="clearfix"></div>
                </div>

            </div>
        </div>
        <!-- edit-tab end -->
        <div class="clearfix"></div>











    </div>
    <!-- END DASHBOARD STATS -->
    <div class="clearfix"></div>

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            var picker = new Pikaday(
                {
                    field: document.getElementById('<%=txtEOLDate.ClientID%>'),
                    firstDay: 0,
                    format: 'MM/DD/YYYY',
                    minDate: new Date(2000, 1, 1),
                    maxDate: new Date(2020, 12, 31),
                    yearRange: [2000, 2020]
                });

            $('a[href^="#accrd"]').on('click', function (e) {
                e.preventDefault();

                var target = this.hash;
                var $target = $(target);
                if ($(target).hasClass('active') || target == "") {

                }
                else {
                    $(target).click();
                }

                $('html, body').stop().animate({
                    'scrollTop': $target.offset().top - 125
                }, 900, 'swing');
            });


            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });


            //Dropify Basic
            $('.dropify').dropify();
            // Used events
            var drEvent = $('.dropify-event').dropify();

            drEvent.on('dropify.beforeClear', function (event, element) {
                return confirm("Do you really want to delete \"" + element.filename + "\" ?");
            });

            drEvent.on('dropify.afterClear', function (event, element) {
                alert('File deleted');
            });


            //var tab = GetQueryStringParams('TabWarehouse');

            //if (tab == "TabWarehouse") {
            //    $("#accrdWarehouse").addClass("active");
            //    $("#divInvWarehouse").show();
            //    $('html, body').animate({
            //        scrollTop: $('#divInvWarehouse').offset().top
            //    }, 'slow');

            //}

        });


        function GetQueryStringParams(sParam) {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                if (sParameterName[0] == sParam) {
                    return sParameterName[1];
                }
            }
        }

        function isDecimalKey(el, evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode;

            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            if (number.length > 1 && charCode == 46) {
                return false;
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }
            return true;
        }


    </script>
    <script>

</script>
</asp:Content>


