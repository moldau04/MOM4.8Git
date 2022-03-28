<%@ Page Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddProspect" ValidateRequest="false" Debug="true" CodeBehind="AddProspect.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="uc_CustomerSearch.ascx" TagName="uc_customersearch" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--        <link rel="stylesheet" href="css/bootstrap-select.css">
      <link rel="stylesheet" href="css/ForBootStrapDropdown.css">--%>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.3/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.3/js/select2.min.js"></script>



    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>

    <%--  <script src="js/bootstrap-select.js"></script>--%>
    <script>
        $('select').select2();
    </script>
    <style>
        .nomgn {
            margin-bottom: 0 !important;
        }

        #task_wrap_container .padding-left-right-0, #task_history_container .padding-left-right-0 {
            padding-left: 0 !important;
            padding-right: 0 !important;
        }

        [id$='RadGrid_Opportunity_GridHeader'] .rgHeader > a {
            white-space: nowrap;
            padding-left: 0 !important;
        }

        [id$='lnkEstimate'] a {
            padding: 0;
        }
    </style>

    <script type="text/javascript">
        function CheckDelete() {
            var result = false;
            $("#<%=RadGrid_Opportunity.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Do you really want to delete this Opportunity ?');
            }
            else {
                alert('Please select an Opportunity to delete.');
                return false;
            }
        }
        ///-Contact permission
    <%-- function AddContactClick(hyperlink) {
            var IsAdd = document.getElementById('<%= hdnAddeContact.ClientID%>').value;
            if (IsAdd == "Y") {              
                return true; 
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditContactClick(hyperlink) {
            var IsEdit = document.getElementById('<%= hdnEditeContact.ClientID%>').value;
            if (IsEdit == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }--%>
        function DeleteContactClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteContact.ClientID%>').value;
            if (IsDelete == "Y") {
                return SelectedRowDelete('<%= RadGrid_Contacts.ClientID%>', 'contact');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        ///-Document permission
        function AddDocumentClick(hyperlink) {

            var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd == "Y") {
                //ConfirmUpload(this.value);

            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
        }
        function DeleteDocumentClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteDocument.ClientID%>').value;
            if (IsDelete == "Y") {
                return SelectedRowDelete('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvDocuments', 'note');;
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


        var newEMailCount = 0;
        //debugger;
        $(function () {
            //debugger;
            $("#<%= txtAddress.ClientID %>").geocomplete({
                map: false,
                details: "#divmain",
                types: ["geocode", "establishment"],
                address: "#<%= txtAddress.ClientID %>",
                city: "#<%= txtCity.ClientID %>",
                state: "#<%= ddlState.ClientID %>",
                zip: "#<%= txtZip.ClientID %>",
                lat: "#<%= lat.ClientID %>",
                lng: "#<%= lng.ClientID %>"

            }).bind("geocode:result", function (event, result) {

                var countryCode = "", city = "", cityAlt = "", getCountry = "";
                for (var i = 0; i < result.address_components.length; i++) {
                    debugger
                    var addr = result.address_components[i];

                    if (addr.types[0] == 'country')
                        getCountry = addr.short_name;
                    if (addr.types[0] == 'locality')
                        city = addr.long_name;
                    if (addr.types[0] == 'administrative_area_level_1')
                        cityAlt = addr.short_name;
                    if (addr.types[0] == 'postal_code')
                        countryCode = addr.long_name;
                }
                //if (cityAlt.length > 2)
                //        for (var i = 0; i < result.address_components.length; i++) {                       
                //       var addr = result.address_components[i];                      
                //            if (addr.types[0] == 'administrative_area_level_2')
                //                cityAlt = addr.short_name;                                            
                //   }
                $("#<%=ddlCountry.ClientID%>").val(getCountry);
                $("#<%=txtZip.ClientID%>").val(countryCode);
                $("#<%=ddlState.ClientID%>").val(cityAlt);
                $("#<%=txtCity.ClientID%>").val(city);
                Materialize.updateTextFields();
            });
            initialize();
        });

        $(function () {
            // debugger;
            $("#<%= txtBillAddress.ClientID %>").geocomplete({
                map: false,
                details: "#divmain",
                types: ["geocode", "establishment"],
                address: "#<%= txtBillAddress.ClientID %>",
                city: "#<%= txtBillCity.ClientID %>",
                state: "#<%= ddlBillState.ClientID %>",
                zip: "#<%= txtBillZip.ClientID %>"

            }).bind("geocode:result", function (event, result) {

                var countryCode = "", city = "", cityAlt = "", getCountry = "";
                for (var i = 0; i < result.address_components.length; i++) {

                    var addr = result.address_components[i];

                    if (addr.types[0] == 'country')
                        getCountry = addr.short_name;
                    if (addr.types[0] == 'locality')
                        city = addr.long_name;
                    if (addr.types[0] == 'administrative_area_level_1')
                        cityAlt = addr.short_name;
                    if (addr.types[0] == 'postal_code')
                        countryCode = addr.long_name;
                }
                //if (cityAlt.length > 2)
                //        for (var i = 0; i < result.address_components.length; i++) {                       
                //       var addr = result.address_components[i];                      
                //            if (addr.types[0] == 'administrative_area_level_2')
                //                cityAlt = addr.short_name;                                            
                //   }
                $("#<%=ddlBillCountry.ClientID%>").val(getCountry);
                $("#<%=txtBillZip.ClientID%>").val(countryCode);
                $("#<%=ddlBillState.ClientID%>").val(cityAlt);
                $("#<%=txtBillCity.ClientID%>").val(city);


                Materialize.updateTextFields();
            });
            //initialize();
        });

        $(document).ready(function () {
            setInterval(serviceCall, 600000);
            $('#<%= txtAddress.ClientID %>').keyup(function (event) {
                //                if (event.which != 27 && event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40 && event.which != 13) {
                if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                    var txtLat = document.getElementById('<%= lat.ClientID %>');
                    var txtLng = document.getElementById('<%= lng.ClientID %>');
                    txtLat.value = '';
                    txtLng.value = '';
                }
            });

            $("#mapLink").click(function () {
                $("#map").toggle();
                $("#Coord").toggle();
                initialize();
            });

        });


        function initialize() {
            //debugger;
            var address = new google.maps.LatLng(document.getElementById('<%= lat.ClientID %>').value, document.getElementById('<%= lng.ClientID %>').value);
            var marker;
            var map;
            var mapOptions = {
                zoom: 13,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                center: address
            };

            map = new google.maps.Map(document.getElementById('map'),
                mapOptions);

            marker = new google.maps.Marker({
                map: map,
                draggable: false,
                position: address
            });
        }

        ////////////////// Confirm Document Upload ////////////////////
        function ConfirmUpload(value) {
            var filename;
            var fullPath = value;
            alert(fullPath);
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


        function ChkAddress() {
            var txtBillAdd = document.getElementById('<%= txtBillAddress.ClientID %>');
            var txtBillCity = document.getElementById('<%= txtBillCity.ClientID %>');
            var ddlBillState = document.getElementById('<%= ddlBillState.ClientID %>');
            var ddlBillCountry = document.getElementById('<%= ddlBillCountry.ClientID %>');
            var txtBillZip = document.getElementById('<%= txtBillZip.ClientID %>');
           <%-- var txtBillPhone = document.getElementById('<%= txtBillPhone.ClientID %>');--%>

            var txtAddress = document.getElementById('<%= txtAddress.ClientID %>');
            var txtCity = document.getElementById('<%= txtCity.ClientID %>');
            var ddlState = document.getElementById('<%= ddlState.ClientID %>');
            var ddlCountry = document.getElementById('<%= ddlCountry.ClientID %>');
            var txtZip = document.getElementById('<%= txtZip.ClientID %>');
            var txtPhone = document.getElementById('<%= txtPhone.ClientID %>');

            var chkAdd = document.getElementById('<%= chkAddress.ClientID %>');

            if (chkAdd.checked == true) {
                txtBillAdd.value = txtAddress.value;
                txtBillCity.value = txtCity.value;
                ddlBillState.value = ddlState.value;

                ddlBillCountry.value = ddlCountry.value;
                txtBillZip.value = txtZip.value;
                //txtBillPhone.value = txtPhone.value;
            }
            Materialize.updateTextFields();

        }

        function serviceCall() {
            var rol = document.getElementById('<%= hdnRol.ClientID %>');
            if (rol.value != '') {
                $.ajax({
                    type: "POST",
                    url: 'CustomerAuto.asmx/CheckEmail',
                    data: '{"rol":"' + rol.value + '","type":"-1","uid":"0"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        //                    alert(msg.d);
                        MailCount(msg.d);
                    },
                    error: function (e) {
                        //                        alert(jQuery.parseJSON(e));
                    }
                });
            }
        }

        function MailCount(d) {

            var newmail = 0;
            var hdnct = document.getElementById('<%= hdnMailct.ClientID %>').value;

            if (hdnct != '') {
                newmail = hdnct;
            }

            //            alert(newmail + ' -- ' + d);
            if (newmail != d) {
                document.getElementById('dvmailct').innerHTML = d - newmail + ' New Email(s)';
                $("#maillink").show();
            }
            else {
                $("#maillink").hide();
            }

        }

        function AddSource() {

            //document.getElementById('AddNewSourceDiv').style.display = "block";

        }
        function HideSource() {

            //document.getElementById('AddNewSourceDiv').style.display = "none";

        }


        $(document).ready(function () {

            //document.getElementById('AddNewSourceDiv').style.display = "none";


            var chkRef = document.getElementById('<%= chkReferral.ClientID %>');

            if (chkRef.checked == true) {
                $('#<%= ddlReferral.ClientID %>').show();
            } else {
                $('#<%= ddlReferral.ClientID %>').hide();
            }

            if ($(window).width() > 767) {
                $('#<%=txtRemarks.ClientID%>').focus(function () {
                    $(this).animate({
                        //   right: "+=0",
                        //left: '300px',
                        width: '100%',
                        height: '200px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtRemarks.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });
            }

        });

        function chkReferral() {

            var chkRef = document.getElementById('<%= chkReferral.ClientID %>');

            if (chkRef.checked == true) {
                $('#<%= ddlReferral.ClientID %>').show();
            } else {
                $('#<%= ddlReferral.ClientID %>').hide();
                $('[id*=divDllCustomer]').css("display", "none");
                $('[id*=divDlltxt]').css("display", "none");
                $('[id*=divDllVendor]').css("display", "none");
            }
        }

        function SelectReferral() {

            var referral = $('[id*=ddlReferral] option:selected').text();

            if (referral == 'Vendor') {
                $('[id*=divDllCustomer]').css("display", "none");
                $('[id*=divDlltxt]').css("display", "none");
                $('[id*=divDllVendor]').css("display", "block");
            }
            else if (referral == 'Customer') {
                $('[id*=divDlltxt]').css("display", "none");
                $('[id*=divDllVendor]').css("display", "none");
                $('[id*=divDllCustomer]').css("display", "block");
            }
            else if (referral == 'Others') {
                $('[id*=divDllVendor]').css("display", "none");
                $('[id*=divDllCustomer]').css("display", "none");
                $('[id*=divDlltxt]').css("display", "block");
            }
            else {
                $('[id*=divDllVendor]').css("display", "none");
                $('[id*=divDllCustomer]').css("display", "none");
                $('[id*=divDlltxt]').css("display", "none");
            }
        }


        function notyConfirmForAddEquipment() {
            noty({
                dismissQueue: true,
                layout: 'topCenter',
                theme: 'noty_theme_default',
                animateOpen: { height: 'toggle' },
                animateClose: { height: 'toggle' },
                easing: 'swing',
                text: 'Do you want to add equipment?',
                type: 'alert',
                speed: 500,
                timeout: false,
                closeButton: false,
                closeOnSelfClick: true,
                closeOnSelfOver: false,
                force: false,
                onShow: false,
                onShown: false,
                onClose: false,
                onClosed: false,
                buttons:
                    [
                        {
                            type: 'btn btn-primary', text: 'Ok', click: function ($noty) {
                                $noty.close();
                                var btn = document.getElementById('<%=lnkAddEQ.ClientID%>');
                                btn.click();
                            }
                        },
                        {
                            type: 'btn btn-danger', text: 'Cancel', click: function ($noty) {
                                $noty.close();
                            }
                        }
                    ],
                modal: true,
                template: '<div class="noty_message"><span class="noty_text"></span><div class="noty_close"></div></div>',
                cssPrefix: 'noty_',
                custom:
                {
                    container: null
                }
            });
        }

        // For open active tab after returning
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
        function clickLink(linkName) {
            setTimeout(function () {
                $(linkName).click();
            }, 1000);
        }

        var tab = GetQueryStringParams('tab');

        if (tab == "opentask") {
            clickLink("#lnkaccrdopentask");
        }
        else if (tab == "taskhistory") {
            clickLink("#lnkaccrdtaskhistory");
        }

        function BodyLimitedValidation(obj) {
            if (obj.value.length <= 500) {
                return true;
            } else {
                obj.value = "Thomas";
                return true;
            }
        }

        function CharLimit(input, maxChar) {
            debugger
            var len = $(input).val().length;
            if (len > maxChar) {
                $(input).val($(input).val().substring(0, maxChar));
            }
        }

    </script>
    <script type="text/javascript">
        //--Equipment---
        function AddEquipmentClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeEquipment.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditEquipmentClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeEquipment.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteEquipmentClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteEquipment.ClientID%>').value;
            if (id == "Y") {

                return SelectedRowDelete('<%= RadGrid_Equip.ClientID%>', 'Equipment');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
    </script>
    <%--<style>
        .chkmargin {
            margin-bottom: 4px !important;
        }

        .overflow {
            overflow-x: visible !important;
            overflow-y: visible !important;
            overflow: visible !important;
        }

        .dis {
            display: inline-block !important;
            line-height: 30px !important;
        }

        .hide {
            display: none !important;
        }

        .select2-container {
            border-color: #cecece !important;
        }

            .select2-container .select2-selection--single {
                height: 30px !important;
                border-color: #cecece !important;
            }

        .select2-container--default .select2-selection--single .select2-selection__rendered {
            line-height: 30px !important;
            border-color: #cecece !important;
        }
    </style>--%>

    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Prospect" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="AddSource">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowSource" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkAddnew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkAddNote">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowNotes" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkEditNote">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowNotes" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnCompanyPopUp">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowCompany" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnAddSource">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcSource" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkSave" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <%--<telerik:AjaxSetting AjaxControlID="lnkConvert">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkConvert" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                    <telerik:AjaxUpdatedControl ControlID="pnlCustomer" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                    <telerik:AjaxUpdatedControl ControlID="uc_CustomerSearch1" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                    <telerik:AjaxUpdatedControl ControlID="lnkSave" LoadingPanelID="RadAjaxLoadingPanel_Prospect" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Prospect" runat="server">
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
                                        <i class="mdi-maps-place"></i>&nbsp;  
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Lead</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkSave" runat="server" Text="Save" OnClick="lnkSave_Click"></asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkConvert" Visible="false" runat="server"
                                                ToolTip="Convert to Customer/Location"
                                                OnClick="lnkConvert_Click">Convert</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="False" ToolTip="Close" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="btnclosewrap-one">
                                        <a class="collapse-expand opened" data-position="bottom" data-tooltip="Expand/Collapse Accordion">
                                            <i class="mdi-action-open-in-browser"></i>
                                        </a>
                                    </div>

                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label runat="server" ID="lblHeaderLabel"></asp:Label>
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
                                    <li><a href="#accrdleadInfo">Lead Information</a></li>
                                    <li runat="server" id="liAddressInformation"><a href="#accrdaddinfo">Address Information</a></li>
                                    <li runat="server" id="liContacts"><a href="#accrdcontacts">Contacts</a></li>
                                    <li runat="server" id="liEquipments" style="display: none"><a href="#accrdequipments">View Equipment</a></li>
                                    <li runat="server" id="liOpenTask"><a id="lnkaccrdopentask" href="#accrdopentask">Open Task</a></li>
                                    <li runat="server" id="liTaskHistory"><a id="lnkaccrdtaskhistory" href="#accrdtaskhistory">Task History</a></li>
                                    <li runat="server" id="liOpportunities"><a href="#accrdopportunities">Opportunities</a></li>
                                    <li runat="server" id="liEmails"><a href="#accrdemails">Emails</a></li>
                                    <li runat="server" id="liNotes"><a href="#accrdnotes">Notes & Attachments</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                    <li runat="server" id="liSystemInfo"><a href="#accrdsystem">System Info</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlNext" runat="server">
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" OnClick="lnkFirst_Click" CausesValidation="False">
                                                        <i class="fa fa-angle-double-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" OnClick="lnkPrevious_Click" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" OnClick="lnkNext_Click" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-right"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" OnClick="lnkLast_Click" runat="server" CssClass="icon-last" CausesValidation="False">
                                                        <i class="fa fa-angle-double-right"></i>
                                            </asp:LinkButton>
                                        </span>
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
        <asp:Panel ID="pnlCustomer" CssClass="srchpane" Style="margin-top: 20px;" runat="server" Visible="false">
            <div class="row">
                <div>
                    Please select an existing customer or leave the field blank and this will create a new customer. 
                </div>
                <div style="clear: both;">
                    <uc1:uc_customersearch ID="uc_CustomerSearch1" runat="server" />
                </div>
            </div>
        </asp:Panel>

        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li>
                            <div id="accrdleadInfo" class="collapsible-header accrd active accordian-text-custom"><i class="mdi-maps-map"></i>Lead Information</div>
                            <div class="collapsible-body" id="firstTab">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="txtCustomer">Customer Lead Name</label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server"
                                                            ControlToValidate="txtCustomer" Display="None"
                                                            ErrorMessage="Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator47_ValidatorCalloutExtender"
                                                            runat="server" PopupPosition="BottomLeft" Enabled="True" TargetControlID="RequiredFieldValidator47">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtCustomer" runat="server" MaxLength="75"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="txtProspectName">Location Lead Name</label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtProspectName"
                                                            Display="None" ErrorMessage="Lead Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator40_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator40">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtProspectName" runat="server" MaxLength="75"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="txtMaincontact">Contact Name</label>
                                                        <asp:TextBox ID="txtMaincontact" runat="server" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="txtEmail">Email</label>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail"
                                                            Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                                            runat="server" PopupPosition="BottomLeft" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                        </asp:ValidatorCalloutExtender>

                                                        <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtEmail">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="txtWebsite">Website</label>
                                                        <asp:TextBox ID="txtWebsite" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="txtPhone">Phone</label>
                                                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="28"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="phone_fax">Fax</label>
                                                        <asp:TextBox ID="txtFax" runat="server" MaxLength="28"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="txtCell">Cellular</label>
                                                        <asp:TextBox ID="txtCell" runat="server" MaxLength="28"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Status</label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Value="0">Active</asp:ListItem>
                                                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:UpdatePanel ID="UpnSalespersonList" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <label class="drpdwn-label">Assigned To</label>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator44" Enabled="true" runat="server" ControlToValidate="ddlSalesperson"
                                                                    Display="None" ErrorMessage="Salesperson Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator44_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator44">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:DropDownList ID="ddlSalesperson" runat="server" CssClass="browser-default">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Type</label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator45" runat="server" ControlToValidate="ddlType"
                                                            Display="None" ErrorMessage="Type Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator45_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator45">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                    <div class="row">
                                                        <label id="ddlCompanyLabel" runat="server" class="drpdwn-label">Company</label>
                                                        <asp:DropDownList ID="ddlCompany" CssClass="browser-default" runat="server"></asp:DropDownList>
                                                        <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                                        <asp:LinkButton runat="server" OnClick="btnCompanyPopUp_Click" ID="btnCompanyPopUp">Change Company</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                            <div class="form-section3">

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">
                                                            <asp:Label runat="server" ID="lblBusinessType"></asp:Label></label>
                                                        <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="browser-default" Placeholder="Select">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="input-field col s11">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Source</label>
                                                        <%--   <asp:DropDownList runat="server" ID="ddlSource" CssClass="browser-default">
                                                        </asp:DropDownList>--%>


                                                        <telerik:RadComboBox RenderMode="Auto" Skin="Metro" ID="rcSource" runat="server" Filter="StartsWith"
                                                            EmptyMessage=":: Select ::">
                                                        </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s1 social-person-add" >
                                                    <div class="row">
                                                        <div class="btnlinksicon">
                                                            <%--<asp:LinkButton runat="server" ID="AddSource" href="#" OnClientClick="AddSource();"  Visible="True">Add Source</asp:LinkButton>--%>
                                                            <asp:LinkButton runat="server" ID="AddSource" OnClick="AddSource_Click" CausesValidation="false" Visible="True"><i class="mdi-social-person-add"></i></asp:LinkButton>
                                                        </div>



                                                    </div>


                                                </div>
                                                <div class="input-field col s3 referral-css">
                                                    <div class="row">
                                                        <asp:CheckBox ID="chkReferral" runat="server" Text="Referral" CssClass="css-checkbox" onclick="javascript:chkReferral();" />
                                                    </div>

                                                </div>
                                                <div class="input-field col s1">
                                                    &nbsp;
                                                </div>
                                                <div class="input-field col s8 m-t-14">
                                                    <div class="row">
                                                        <asp:DropDownList runat="server" ID="ddlReferral" AutoPostBack="false" CssClass="browser-default nomgn" onchange="SelectReferral();" EnableTheming="False" Style="margin-bottom: 0 !important">
                                                            <asp:ListItem Selected="True">Select</asp:ListItem>
                                                            <asp:ListItem>Customer</asp:ListItem>
                                                            <asp:ListItem>Vendor</asp:ListItem>
                                                            <asp:ListItem>Others</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdnReferral" runat="server" Visible="False" />
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row m-t-7" >

                                                        <div id="divDllCustomer" runat="server" style="display: none">
                                                            <%-- <asp:DropDownList runat="server" ID="ddlCustomer" CssClass="browser-default">
                                                            </asp:DropDownList>--%>
                                                            <telerik:RadComboBox RenderMode="Auto" Skin="Metro" CssClass="browser-default" ID="rcCustomer" runat="server" Filter="StartsWith">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                        <div id="divDllVendor" runat="server" style="display: none">
                                                            <%--       <asp:DropDownList runat="server" ID="ddlVendor" CssClass="browser-default">
                                                            </asp:DropDownList>--%>
                                                            <telerik:RadComboBox RenderMode="Auto" Skin="Metro" CssClass="browser-default" ID="rcVendor" runat="server" Filter="StartsWith">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                        <div id="divDlltxt" runat="server" style="display: none">
                                                            <asp:TextBox runat="server" ID="txtReferral" CssClass="form-control">
                                                            </asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12  m-t-7" >
                                                    <div class="row">
                                                        <label for="Remarks">Remarks</label>
                                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="materialize-textarea" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>

                                </div>
                            </div>
                        </li>
                        <li runat="server" id="adAddressInformation">
                            <div id="accrdaddinfo" class="collapsible-header accrd accordian-text-custom"><i class="mdi-maps-pin-drop"></i>Address Information</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">


                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="txtAddress">Shipping Address</label>
                                                        <asp:TextBox ID="txtAddress" runat="server" placeholder="" CssClass="materialize-textarea" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label for="txtCity">City</label>
                                                        <asp:TextBox ID="txtCity" runat="server" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label>State/Province</label>
                                                        <asp:TextBox ID="ddlState" runat="server" MaxLength="50"></asp:TextBox>
                                                        <%--<asp:DropDownList ID="" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Value="State">State</asp:ListItem>
                                                            <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                            <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                            <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                            <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                            <asp:ListItem Value="CA">California</asp:ListItem>
                                                            <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                            <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                            <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                            <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                            <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                            <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                            <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                            <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                            <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                            <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                            <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                            <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                            <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                            <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                            <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                            <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                            <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                            <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                            <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                            <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                            <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                            <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                            <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                            <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                            <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                            <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                            <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                            <asp:ListItem Value="NY">New York</asp:ListItem>
                                                            <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                            <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                            <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                            <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                            <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                            <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                            <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                            <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                            <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                            <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                            <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                            <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                            <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                            <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                            <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                            <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                            <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                            <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                            <asp:ListItem Value="AB">Alberta</asp:ListItem>
                                                            <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                                                            <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                                                            <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                                                            <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                                                            <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                                                            <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                                                            <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                                                            <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                                                            <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                                                            <asp:ListItem Value="ON">Ontario</asp:ListItem>
                                                            <asp:ListItem Value="QC">Quebec</asp:ListItem>
                                                            <asp:ListItem Value="YT">Yukon</asp:ListItem>
                                                        </asp:DropDownList>--%>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label for="txtZip">Zip/Postal Code</label>
                                                        <asp:TextBox ID="txtZip" runat="server" MaxLength="10"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Country</label>
                                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Value="country">Country</asp:ListItem>
                                                            <asp:ListItem Value="AF">Afghanistan</asp:ListItem>
                                                            <asp:ListItem Value="AX">Åland Islands</asp:ListItem>
                                                            <asp:ListItem Value="AL">Albania</asp:ListItem>
                                                            <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                                                            <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                                                            <asp:ListItem Value="AD">Andorra</asp:ListItem>
                                                            <asp:ListItem Value="AO">Angola</asp:ListItem>
                                                            <asp:ListItem Value="AI">Anguilla</asp:ListItem>
                                                            <asp:ListItem Value="AQ">Antarctica</asp:ListItem>
                                                            <asp:ListItem Value="AG">Antigua and Barbuda</asp:ListItem>
                                                            <asp:ListItem Value="AR">Argentina</asp:ListItem>
                                                            <asp:ListItem Value="AM">Armenia</asp:ListItem>
                                                            <asp:ListItem Value="AW">Aruba</asp:ListItem>
                                                            <asp:ListItem Value="AU">Australia</asp:ListItem>
                                                            <asp:ListItem Value="AT">Austria</asp:ListItem>
                                                            <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                                                            <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                                                            <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                                                            <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                                                            <asp:ListItem Value="BB">Barbados</asp:ListItem>
                                                            <asp:ListItem Value="BY">Belarus</asp:ListItem>
                                                            <asp:ListItem Value="BE">Belgium</asp:ListItem>
                                                            <asp:ListItem Value="BZ">Belize</asp:ListItem>
                                                            <asp:ListItem Value="BJ">Benin</asp:ListItem>
                                                            <asp:ListItem Value="BM">Bermuda</asp:ListItem>
                                                            <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                                                            <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                                                            <asp:ListItem Value="BA">Bosnia and Herzegovina</asp:ListItem>
                                                            <asp:ListItem Value="BW">Botswana</asp:ListItem>
                                                            <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>
                                                            <asp:ListItem Value="BR">Brazil</asp:ListItem>
                                                            <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>
                                                            <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>
                                                            <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                                                            <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                                                            <asp:ListItem Value="BI">Burundi</asp:ListItem>
                                                            <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                                                            <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                                                            <asp:ListItem Value="CA">Canada</asp:ListItem>
                                                            <asp:ListItem Value="CV">Cape Verde</asp:ListItem>
                                                            <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>
                                                            <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                                                            <asp:ListItem Value="TD">Chad</asp:ListItem>
                                                            <asp:ListItem Value="CL">Chile</asp:ListItem>
                                                            <asp:ListItem Value="CN">China</asp:ListItem>
                                                            <asp:ListItem Value="CX">Christmas Island</asp:ListItem>
                                                            <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>
                                                            <asp:ListItem Value="CO">Colombia</asp:ListItem>
                                                            <asp:ListItem Value="KM">Comoros</asp:ListItem>
                                                            <asp:ListItem Value="CG">Congo</asp:ListItem>
                                                            <asp:ListItem Value="CD">Congo, The Democratic Republic of The</asp:ListItem>
                                                            <asp:ListItem Value="CK">Cook Islands</asp:ListItem>
                                                            <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                                                            <asp:ListItem Value="CI">Cote D'ivoire</asp:ListItem>
                                                            <asp:ListItem Value="HR">Croatia</asp:ListItem>
                                                            <asp:ListItem Value="CU">Cuba</asp:ListItem>
                                                            <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                                                            <asp:ListItem Value="CZ">Czechia</asp:ListItem>
                                                            <asp:ListItem Value="DK">Denmark</asp:ListItem>
                                                            <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                                                            <asp:ListItem Value="DM">Dominica</asp:ListItem>
                                                            <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                                                            <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                                                            <asp:ListItem Value="EG">Egypt</asp:ListItem>
                                                            <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                                                            <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                                                            <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                                                            <asp:ListItem Value="EE">Estonia</asp:ListItem>
                                                            <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                                                            <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>
                                                            <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>
                                                            <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                                                            <asp:ListItem Value="FI">Finland</asp:ListItem>
                                                            <asp:ListItem Value="FR">France</asp:ListItem>
                                                            <asp:ListItem Value="GF">French Guiana</asp:ListItem>
                                                            <asp:ListItem Value="PF">French Polynesia</asp:ListItem>
                                                            <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>
                                                            <asp:ListItem Value="GA">Gabon</asp:ListItem>
                                                            <asp:ListItem Value="GM">Gambia</asp:ListItem>
                                                            <asp:ListItem Value="GE">Georgia</asp:ListItem>
                                                            <asp:ListItem Value="DE">Germany</asp:ListItem>
                                                            <asp:ListItem Value="GH">Ghana</asp:ListItem>
                                                            <asp:ListItem Value="GI">Gibraltar</asp:ListItem>
                                                            <asp:ListItem Value="GR">Greece</asp:ListItem>
                                                            <asp:ListItem Value="GL">Greenland</asp:ListItem>
                                                            <asp:ListItem Value="GD">Grenada</asp:ListItem>
                                                            <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>
                                                            <asp:ListItem Value="GU">Guam</asp:ListItem>
                                                            <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                                                            <asp:ListItem Value="GG">Guernsey</asp:ListItem>
                                                            <asp:ListItem Value="GN">Guinea</asp:ListItem>
                                                            <asp:ListItem Value="GW">Guinea-bissau</asp:ListItem>
                                                            <asp:ListItem Value="GY">Guyana</asp:ListItem>
                                                            <asp:ListItem Value="HT">Haiti</asp:ListItem>
                                                            <asp:ListItem Value="HM">Heard Island and Mcdonald Islands</asp:ListItem>
                                                            <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>
                                                            <asp:ListItem Value="HN">Honduras</asp:ListItem>
                                                            <asp:ListItem Value="HK">Hong Kong</asp:ListItem>
                                                            <asp:ListItem Value="HU">Hungary</asp:ListItem>
                                                            <asp:ListItem Value="IS">Iceland</asp:ListItem>
                                                            <asp:ListItem Value="IN">India</asp:ListItem>
                                                            <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                                                            <asp:ListItem Value="IR">Iran, Islamic Republic of</asp:ListItem>
                                                            <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                                                            <asp:ListItem Value="IE">Ireland</asp:ListItem>
                                                            <asp:ListItem Value="IM">Isle of Man</asp:ListItem>
                                                            <asp:ListItem Value="IL">Israel</asp:ListItem>
                                                            <asp:ListItem Value="IT">Italy</asp:ListItem>
                                                            <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                                                            <asp:ListItem Value="JP">Japan</asp:ListItem>
                                                            <asp:ListItem Value="JE">Jersey</asp:ListItem>
                                                            <asp:ListItem Value="JO">Jordan</asp:ListItem>
                                                            <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                                                            <asp:ListItem Value="KE">Kenya</asp:ListItem>
                                                            <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                                                            <asp:ListItem Value="KP">Korea, Democratic People's Republic of</asp:ListItem>
                                                            <asp:ListItem Value="KR">Korea, Republic of</asp:ListItem>
                                                            <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                                                            <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                                                            <asp:ListItem Value="LA">Lao People's Democratic Republic</asp:ListItem>
                                                            <asp:ListItem Value="LV">Latvia</asp:ListItem>
                                                            <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                                                            <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                                                            <asp:ListItem Value="LR">Liberia</asp:ListItem>
                                                            <asp:ListItem Value="LY">Libyan Arab Jamahiriya</asp:ListItem>
                                                            <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                                                            <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                                                            <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                                                            <asp:ListItem Value="MO">Macao</asp:ListItem>
                                                            <asp:ListItem Value="MK">Macedonia, The Former Yugoslav Republic of</asp:ListItem>
                                                            <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                                                            <asp:ListItem Value="MW">Malawi</asp:ListItem>
                                                            <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                                                            <asp:ListItem Value="MV">Maldives</asp:ListItem>
                                                            <asp:ListItem Value="ML">Mali</asp:ListItem>
                                                            <asp:ListItem Value="MT">Malta</asp:ListItem>
                                                            <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                                                            <asp:ListItem Value="MQ">Martinique</asp:ListItem>
                                                            <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                                                            <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                                                            <asp:ListItem Value="YT">Mayotte</asp:ListItem>
                                                            <asp:ListItem Value="MX">Mexico</asp:ListItem>
                                                            <asp:ListItem Value="FM">Micronesia, Federated States of</asp:ListItem>
                                                            <asp:ListItem Value="MD">Moldova, Republic of</asp:ListItem>
                                                            <asp:ListItem Value="MC">Monaco</asp:ListItem>
                                                            <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                                                            <asp:ListItem Value="ME">Montenegro</asp:ListItem>
                                                            <asp:ListItem Value="MS">Montserrat</asp:ListItem>
                                                            <asp:ListItem Value="MA">Morocco</asp:ListItem>
                                                            <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                                                            <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                                                            <asp:ListItem Value="NA">Namibia</asp:ListItem>
                                                            <asp:ListItem Value="NR">Nauru</asp:ListItem>
                                                            <asp:ListItem Value="NP">Nepal</asp:ListItem>
                                                            <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                                                            <asp:ListItem Value="AN">Netherlands Antilles</asp:ListItem>
                                                            <asp:ListItem Value="NC">New Caledonia</asp:ListItem>
                                                            <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                                                            <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                                                            <asp:ListItem Value="NE">Niger</asp:ListItem>
                                                            <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                                                            <asp:ListItem Value="NU">Niue</asp:ListItem>
                                                            <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>
                                                            <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                                                            <asp:ListItem Value="NO">Norway</asp:ListItem>
                                                            <asp:ListItem Value="OM">Oman</asp:ListItem>
                                                            <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                                                            <asp:ListItem Value="PW">Palau</asp:ListItem>
                                                            <asp:ListItem Value="PS">Palestinian Territory, Occupied</asp:ListItem>
                                                            <asp:ListItem Value="PA">Panama</asp:ListItem>
                                                            <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                                                            <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                                                            <asp:ListItem Value="PE">Peru</asp:ListItem>
                                                            <asp:ListItem Value="PH">Philippines</asp:ListItem>
                                                            <asp:ListItem Value="PN">Pitcairn</asp:ListItem>
                                                            <asp:ListItem Value="PL">Poland</asp:ListItem>
                                                            <asp:ListItem Value="PT">Portugal</asp:ListItem>
                                                            <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                                                            <asp:ListItem Value="QA">Qatar</asp:ListItem>
                                                            <asp:ListItem Value="RE">Reunion</asp:ListItem>
                                                            <asp:ListItem Value="RO">Romania</asp:ListItem>
                                                            <asp:ListItem Value="RU">Russian Federation</asp:ListItem>
                                                            <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                                                            <asp:ListItem Value="SH">Saint Helena</asp:ListItem>
                                                            <asp:ListItem Value="KN">Saint Kitts and Nevis</asp:ListItem>
                                                            <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                                                            <asp:ListItem Value="PM">Saint Pierre and Miquelon</asp:ListItem>
                                                            <asp:ListItem Value="VC">Saint Vincent and The Grenadines</asp:ListItem>
                                                            <asp:ListItem Value="WS">Samoa</asp:ListItem>
                                                            <asp:ListItem Value="SM">San Marino</asp:ListItem>
                                                            <asp:ListItem Value="ST">Sao Tome and Principe</asp:ListItem>
                                                            <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                                                            <asp:ListItem Value="SN">Senegal</asp:ListItem>
                                                            <asp:ListItem Value="RS">Serbia</asp:ListItem>
                                                            <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                                                            <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                                                            <asp:ListItem Value="SG">Singapore</asp:ListItem>
                                                            <asp:ListItem Value="SK">Slovakia</asp:ListItem>
                                                            <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                                                            <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                                                            <asp:ListItem Value="SO">Somalia</asp:ListItem>
                                                            <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                                                            <asp:ListItem Value="GS">South Georgia and The South Sandwich Islands</asp:ListItem>
                                                            <asp:ListItem Value="ES">Spain</asp:ListItem>
                                                            <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                                                            <asp:ListItem Value="SD">Sudan</asp:ListItem>
                                                            <asp:ListItem Value="SR">Suriname</asp:ListItem>
                                                            <asp:ListItem Value="SJ">Svalbard and Jan Mayen</asp:ListItem>
                                                            <asp:ListItem Value="SZ">Swaziland</asp:ListItem>
                                                            <asp:ListItem Value="SE">Sweden</asp:ListItem>
                                                            <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                                                            <asp:ListItem Value="SY">Syrian Arab Republic</asp:ListItem>
                                                            <asp:ListItem Value="TW">Taiwan, Province of China</asp:ListItem>
                                                            <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                                                            <asp:ListItem Value="TZ">Tanzania, United Republic of</asp:ListItem>
                                                            <asp:ListItem Value="TH">Thailand</asp:ListItem>
                                                            <asp:ListItem Value="TL">Timor-leste</asp:ListItem>
                                                            <asp:ListItem Value="TG">Togo</asp:ListItem>
                                                            <asp:ListItem Value="TK">Tokelau</asp:ListItem>
                                                            <asp:ListItem Value="TO">Tonga</asp:ListItem>
                                                            <asp:ListItem Value="TT">Trinidad and Tobago</asp:ListItem>
                                                            <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                                                            <asp:ListItem Value="TR">Turkey</asp:ListItem>
                                                            <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                                                            <asp:ListItem Value="TC">Turks and Caicos Islands</asp:ListItem>
                                                            <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                                                            <asp:ListItem Value="UG">Uganda</asp:ListItem>
                                                            <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                                                            <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                                                            <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                                                            <asp:ListItem Value="US">United States</asp:ListItem>
                                                            <asp:ListItem Value="UM">United States Minor Outlying Islands</asp:ListItem>
                                                            <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                                                            <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                                                            <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                                                            <asp:ListItem Value="VE">Venezuela</asp:ListItem>
                                                            <asp:ListItem Value="VN">Viet Nam</asp:ListItem>
                                                            <asp:ListItem Value="VG">Virgin Islands, British</asp:ListItem>
                                                            <asp:ListItem Value="VI">Virgin Islands, U.S.</asp:ListItem>
                                                            <asp:ListItem Value="WF">Wallis and Futuna</asp:ListItem>
                                                            <asp:ListItem Value="EH">Western Sahara</asp:ListItem>
                                                            <asp:ListItem Value="YE">Yemen</asp:ListItem>
                                                            <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                                                            <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>



                                            </div>
                                            <div class="form-section3-blank">&nbsp;</div>
                                            <div class="form-section3">
                                                
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label for="lat">Latitude <span class="reqd">*</span></label>
                                                        <input id="lat" runat="server" type="text" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    &nbsp;
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label for="lat">Longitude <span class="reqd">*</span></label>
                                                        <input id="lng" runat="server" type="text" />
                                                        <input id="locality" disabled="disabled" style="display: none;" />
                                                        <input id="country" disabled="disabled" style="display: none;" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <div id="map" class="map_csson_lead" >
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">&nbsp;</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12 shipping-css">
                                                    <div class="row">
                                                        <asp:CheckBox ID="chkAddress" runat="server" Text="Same as Shipping Address" CssClass="css-checkbox" onclick="javascript:ChkAddress();" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="billAddress">Billing Address</label>
                                                        <asp:TextBox ID="txtBillAddress" runat="server" placeholder="" CssClass="materialize-textarea" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label for="txtBillCity">City</label>
                                                        <asp:TextBox ID="txtBillCity" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label>State/Province</label>
                                                        <asp:TextBox ID="ddlBillState" runat="server"></asp:TextBox>
                                                        <%--<asp:DropDownList ID="ddlBillState" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Value="State">State</asp:ListItem>
                                                            <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                            <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                            <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                            <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                            <asp:ListItem Value="CA">California</asp:ListItem>
                                                            <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                            <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                            <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                            <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                            <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                            <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                            <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                            <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                            <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                            <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                            <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                            <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                            <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                            <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                            <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                            <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                            <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                            <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                            <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                            <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                            <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                            <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                            <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                            <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                            <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                            <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                            <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                            <asp:ListItem Value="NY">New York</asp:ListItem>
                                                            <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                            <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                            <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                            <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                            <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                            <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                            <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                            <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                            <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                            <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                            <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                            <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                            <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                            <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                            <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                            <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                            <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                            <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                            <asp:ListItem Value="AB">Alberta</asp:ListItem>
                                                            <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                                                            <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                                                            <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                                                            <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                                                            <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                                                            <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                                                            <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                                                            <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                                                            <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                                                            <asp:ListItem Value="ON">Ontario</asp:ListItem>
                                                            <asp:ListItem Value="QC">Quebec</asp:ListItem>
                                                            <asp:ListItem Value="YT">Yukon</asp:ListItem>
                                                        </asp:DropDownList>--%>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label for="txtBillZip">Zip/Postal Code</label>
                                                        <asp:TextBox ID="txtBillZip" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Country</label>
                                                        <asp:DropDownList ID="ddlBillCountry" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Value="country">Country</asp:ListItem>
                                                            <asp:ListItem Value="AF">Afghanistan</asp:ListItem>
                                                            <asp:ListItem Value="AX">Åland Islands</asp:ListItem>
                                                            <asp:ListItem Value="AL">Albania</asp:ListItem>
                                                            <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                                                            <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                                                            <asp:ListItem Value="AD">Andorra</asp:ListItem>
                                                            <asp:ListItem Value="AO">Angola</asp:ListItem>
                                                            <asp:ListItem Value="AI">Anguilla</asp:ListItem>
                                                            <asp:ListItem Value="AQ">Antarctica</asp:ListItem>
                                                            <asp:ListItem Value="AG">Antigua and Barbuda</asp:ListItem>
                                                            <asp:ListItem Value="AR">Argentina</asp:ListItem>
                                                            <asp:ListItem Value="AM">Armenia</asp:ListItem>
                                                            <asp:ListItem Value="AW">Aruba</asp:ListItem>
                                                            <asp:ListItem Value="AU">Australia</asp:ListItem>
                                                            <asp:ListItem Value="AT">Austria</asp:ListItem>
                                                            <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                                                            <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                                                            <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                                                            <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                                                            <asp:ListItem Value="BB">Barbados</asp:ListItem>
                                                            <asp:ListItem Value="BY">Belarus</asp:ListItem>
                                                            <asp:ListItem Value="BE">Belgium</asp:ListItem>
                                                            <asp:ListItem Value="BZ">Belize</asp:ListItem>
                                                            <asp:ListItem Value="BJ">Benin</asp:ListItem>
                                                            <asp:ListItem Value="BM">Bermuda</asp:ListItem>
                                                            <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                                                            <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                                                            <asp:ListItem Value="BA">Bosnia and Herzegovina</asp:ListItem>
                                                            <asp:ListItem Value="BW">Botswana</asp:ListItem>
                                                            <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>
                                                            <asp:ListItem Value="BR">Brazil</asp:ListItem>
                                                            <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>
                                                            <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>
                                                            <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                                                            <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                                                            <asp:ListItem Value="BI">Burundi</asp:ListItem>
                                                            <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                                                            <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                                                            <asp:ListItem Value="CA">Canada</asp:ListItem>
                                                            <asp:ListItem Value="CV">Cape Verde</asp:ListItem>
                                                            <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>
                                                            <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                                                            <asp:ListItem Value="TD">Chad</asp:ListItem>
                                                            <asp:ListItem Value="CL">Chile</asp:ListItem>
                                                            <asp:ListItem Value="CN">China</asp:ListItem>
                                                            <asp:ListItem Value="CX">Christmas Island</asp:ListItem>
                                                            <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>
                                                            <asp:ListItem Value="CO">Colombia</asp:ListItem>
                                                            <asp:ListItem Value="KM">Comoros</asp:ListItem>
                                                            <asp:ListItem Value="CG">Congo</asp:ListItem>
                                                            <asp:ListItem Value="CD">Congo, The Democratic Republic of The</asp:ListItem>
                                                            <asp:ListItem Value="CK">Cook Islands</asp:ListItem>
                                                            <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                                                            <asp:ListItem Value="CI">Cote D'ivoire</asp:ListItem>
                                                            <asp:ListItem Value="HR">Croatia</asp:ListItem>
                                                            <asp:ListItem Value="CU">Cuba</asp:ListItem>
                                                            <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                                                            <asp:ListItem Value="CZ">Czechia</asp:ListItem>
                                                            <asp:ListItem Value="DK">Denmark</asp:ListItem>
                                                            <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                                                            <asp:ListItem Value="DM">Dominica</asp:ListItem>
                                                            <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                                                            <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                                                            <asp:ListItem Value="EG">Egypt</asp:ListItem>
                                                            <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                                                            <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                                                            <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                                                            <asp:ListItem Value="EE">Estonia</asp:ListItem>
                                                            <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                                                            <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>
                                                            <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>
                                                            <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                                                            <asp:ListItem Value="FI">Finland</asp:ListItem>
                                                            <asp:ListItem Value="FR">France</asp:ListItem>
                                                            <asp:ListItem Value="GF">French Guiana</asp:ListItem>
                                                            <asp:ListItem Value="PF">French Polynesia</asp:ListItem>
                                                            <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>
                                                            <asp:ListItem Value="GA">Gabon</asp:ListItem>
                                                            <asp:ListItem Value="GM">Gambia</asp:ListItem>
                                                            <asp:ListItem Value="GE">Georgia</asp:ListItem>
                                                            <asp:ListItem Value="DE">Germany</asp:ListItem>
                                                            <asp:ListItem Value="GH">Ghana</asp:ListItem>
                                                            <asp:ListItem Value="GI">Gibraltar</asp:ListItem>
                                                            <asp:ListItem Value="GR">Greece</asp:ListItem>
                                                            <asp:ListItem Value="GL">Greenland</asp:ListItem>
                                                            <asp:ListItem Value="GD">Grenada</asp:ListItem>
                                                            <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>
                                                            <asp:ListItem Value="GU">Guam</asp:ListItem>
                                                            <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                                                            <asp:ListItem Value="GG">Guernsey</asp:ListItem>
                                                            <asp:ListItem Value="GN">Guinea</asp:ListItem>
                                                            <asp:ListItem Value="GW">Guinea-bissau</asp:ListItem>
                                                            <asp:ListItem Value="GY">Guyana</asp:ListItem>
                                                            <asp:ListItem Value="HT">Haiti</asp:ListItem>
                                                            <asp:ListItem Value="HM">Heard Island and Mcdonald Islands</asp:ListItem>
                                                            <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>
                                                            <asp:ListItem Value="HN">Honduras</asp:ListItem>
                                                            <asp:ListItem Value="HK">Hong Kong</asp:ListItem>
                                                            <asp:ListItem Value="HU">Hungary</asp:ListItem>
                                                            <asp:ListItem Value="IS">Iceland</asp:ListItem>
                                                            <asp:ListItem Value="IN">India</asp:ListItem>
                                                            <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                                                            <asp:ListItem Value="IR">Iran, Islamic Republic of</asp:ListItem>
                                                            <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                                                            <asp:ListItem Value="IE">Ireland</asp:ListItem>
                                                            <asp:ListItem Value="IM">Isle of Man</asp:ListItem>
                                                            <asp:ListItem Value="IL">Israel</asp:ListItem>
                                                            <asp:ListItem Value="IT">Italy</asp:ListItem>
                                                            <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                                                            <asp:ListItem Value="JP">Japan</asp:ListItem>
                                                            <asp:ListItem Value="JE">Jersey</asp:ListItem>
                                                            <asp:ListItem Value="JO">Jordan</asp:ListItem>
                                                            <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                                                            <asp:ListItem Value="KE">Kenya</asp:ListItem>
                                                            <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                                                            <asp:ListItem Value="KP">Korea, Democratic People's Republic of</asp:ListItem>
                                                            <asp:ListItem Value="KR">Korea, Republic of</asp:ListItem>
                                                            <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                                                            <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                                                            <asp:ListItem Value="LA">Lao People's Democratic Republic</asp:ListItem>
                                                            <asp:ListItem Value="LV">Latvia</asp:ListItem>
                                                            <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                                                            <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                                                            <asp:ListItem Value="LR">Liberia</asp:ListItem>
                                                            <asp:ListItem Value="LY">Libyan Arab Jamahiriya</asp:ListItem>
                                                            <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                                                            <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                                                            <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                                                            <asp:ListItem Value="MO">Macao</asp:ListItem>
                                                            <asp:ListItem Value="MK">Macedonia, The Former Yugoslav Republic of</asp:ListItem>
                                                            <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                                                            <asp:ListItem Value="MW">Malawi</asp:ListItem>
                                                            <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                                                            <asp:ListItem Value="MV">Maldives</asp:ListItem>
                                                            <asp:ListItem Value="ML">Mali</asp:ListItem>
                                                            <asp:ListItem Value="MT">Malta</asp:ListItem>
                                                            <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                                                            <asp:ListItem Value="MQ">Martinique</asp:ListItem>
                                                            <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                                                            <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                                                            <asp:ListItem Value="YT">Mayotte</asp:ListItem>
                                                            <asp:ListItem Value="MX">Mexico</asp:ListItem>
                                                            <asp:ListItem Value="FM">Micronesia, Federated States of</asp:ListItem>
                                                            <asp:ListItem Value="MD">Moldova, Republic of</asp:ListItem>
                                                            <asp:ListItem Value="MC">Monaco</asp:ListItem>
                                                            <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                                                            <asp:ListItem Value="ME">Montenegro</asp:ListItem>
                                                            <asp:ListItem Value="MS">Montserrat</asp:ListItem>
                                                            <asp:ListItem Value="MA">Morocco</asp:ListItem>
                                                            <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                                                            <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                                                            <asp:ListItem Value="NA">Namibia</asp:ListItem>
                                                            <asp:ListItem Value="NR">Nauru</asp:ListItem>
                                                            <asp:ListItem Value="NP">Nepal</asp:ListItem>
                                                            <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                                                            <asp:ListItem Value="AN">Netherlands Antilles</asp:ListItem>
                                                            <asp:ListItem Value="NC">New Caledonia</asp:ListItem>
                                                            <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                                                            <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                                                            <asp:ListItem Value="NE">Niger</asp:ListItem>
                                                            <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                                                            <asp:ListItem Value="NU">Niue</asp:ListItem>
                                                            <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>
                                                            <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                                                            <asp:ListItem Value="NO">Norway</asp:ListItem>
                                                            <asp:ListItem Value="OM">Oman</asp:ListItem>
                                                            <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                                                            <asp:ListItem Value="PW">Palau</asp:ListItem>
                                                            <asp:ListItem Value="PS">Palestinian Territory, Occupied</asp:ListItem>
                                                            <asp:ListItem Value="PA">Panama</asp:ListItem>
                                                            <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                                                            <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                                                            <asp:ListItem Value="PE">Peru</asp:ListItem>
                                                            <asp:ListItem Value="PH">Philippines</asp:ListItem>
                                                            <asp:ListItem Value="PN">Pitcairn</asp:ListItem>
                                                            <asp:ListItem Value="PL">Poland</asp:ListItem>
                                                            <asp:ListItem Value="PT">Portugal</asp:ListItem>
                                                            <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                                                            <asp:ListItem Value="QA">Qatar</asp:ListItem>
                                                            <asp:ListItem Value="RE">Reunion</asp:ListItem>
                                                            <asp:ListItem Value="RO">Romania</asp:ListItem>
                                                            <asp:ListItem Value="RU">Russian Federation</asp:ListItem>
                                                            <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                                                            <asp:ListItem Value="SH">Saint Helena</asp:ListItem>
                                                            <asp:ListItem Value="KN">Saint Kitts and Nevis</asp:ListItem>
                                                            <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                                                            <asp:ListItem Value="PM">Saint Pierre and Miquelon</asp:ListItem>
                                                            <asp:ListItem Value="VC">Saint Vincent and The Grenadines</asp:ListItem>
                                                            <asp:ListItem Value="WS">Samoa</asp:ListItem>
                                                            <asp:ListItem Value="SM">San Marino</asp:ListItem>
                                                            <asp:ListItem Value="ST">Sao Tome and Principe</asp:ListItem>
                                                            <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                                                            <asp:ListItem Value="SN">Senegal</asp:ListItem>
                                                            <asp:ListItem Value="RS">Serbia</asp:ListItem>
                                                            <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                                                            <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                                                            <asp:ListItem Value="SG">Singapore</asp:ListItem>
                                                            <asp:ListItem Value="SK">Slovakia</asp:ListItem>
                                                            <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                                                            <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                                                            <asp:ListItem Value="SO">Somalia</asp:ListItem>
                                                            <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                                                            <asp:ListItem Value="GS">South Georgia and The South Sandwich Islands</asp:ListItem>
                                                            <asp:ListItem Value="ES">Spain</asp:ListItem>
                                                            <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                                                            <asp:ListItem Value="SD">Sudan</asp:ListItem>
                                                            <asp:ListItem Value="SR">Suriname</asp:ListItem>
                                                            <asp:ListItem Value="SJ">Svalbard and Jan Mayen</asp:ListItem>
                                                            <asp:ListItem Value="SZ">Swaziland</asp:ListItem>
                                                            <asp:ListItem Value="SE">Sweden</asp:ListItem>
                                                            <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                                                            <asp:ListItem Value="SY">Syrian Arab Republic</asp:ListItem>
                                                            <asp:ListItem Value="TW">Taiwan, Province of China</asp:ListItem>
                                                            <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                                                            <asp:ListItem Value="TZ">Tanzania, United Republic of</asp:ListItem>
                                                            <asp:ListItem Value="TH">Thailand</asp:ListItem>
                                                            <asp:ListItem Value="TL">Timor-leste</asp:ListItem>
                                                            <asp:ListItem Value="TG">Togo</asp:ListItem>
                                                            <asp:ListItem Value="TK">Tokelau</asp:ListItem>
                                                            <asp:ListItem Value="TO">Tonga</asp:ListItem>
                                                            <asp:ListItem Value="TT">Trinidad and Tobago</asp:ListItem>
                                                            <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                                                            <asp:ListItem Value="TR">Turkey</asp:ListItem>
                                                            <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                                                            <asp:ListItem Value="TC">Turks and Caicos Islands</asp:ListItem>
                                                            <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                                                            <asp:ListItem Value="UG">Uganda</asp:ListItem>
                                                            <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                                                            <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                                                            <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                                                            <asp:ListItem Value="US">United States</asp:ListItem>
                                                            <asp:ListItem Value="UM">United States Minor Outlying Islands</asp:ListItem>
                                                            <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                                                            <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                                                            <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                                                            <asp:ListItem Value="VE">Venezuela</asp:ListItem>
                                                            <asp:ListItem Value="VN">Viet Nam</asp:ListItem>
                                                            <asp:ListItem Value="VG">Virgin Islands, British</asp:ListItem>
                                                            <asp:ListItem Value="VI">Virgin Islands, U.S.</asp:ListItem>
                                                            <asp:ListItem Value="WF">Wallis and Futuna</asp:ListItem>
                                                            <asp:ListItem Value="EH">Western Sahara</asp:ListItem>
                                                            <asp:ListItem Value="YE">Yemen</asp:ListItem>
                                                            <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                                                            <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>

                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="adContacts">
                            <div id="accrdcontacts" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-account-circle"></i>Contacts</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">

                                            <ContentTemplate>
                                                <div class="btncontainer">
                                                    <asp:Panel ID="pnlContactButtons" runat="server">
                                                        <div class="btnlinks">
                                                            <%--   <asp:LinkButton ID="lnkAddnew" runat="server" CausesValidation="False"
                                                            OnClick="lnkAddnew_Click">Add</asp:LinkButton>--%>
                                                            <asp:LinkButton ID="lnkAddnew" runat="server" CausesValidation="False" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                                        </div>
                                                        <div class="btnlinks">
                                                            <%--   <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
                                                                OnClientClick="return EditContactClick(this);" OnClick="btnEdit_Click">Edit</asp:LinkButton>--%>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
                                                                OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                                        </div>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="btnDelete" runat="server"
                                                                OnClientClick="return DeleteContactClick(this);"
                                                                CausesValidation="False"
                                                                OnClick="btnDelete_Click">Delete</asp:LinkButton>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                                <div class="grid_container">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock_Prospect" runat="server">
                                                            <script type="text/javascript">
                                                                function pageLoad() {
                                                                    var grid = $find("<%= RadGrid_Contacts.ClientID %>");
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
                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Prospect" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Prospect" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Contacts" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                OnNeedDataSource="RadGrid_Contacts_NeedDataSource" PagerStyle-AlwaysVisible="true"
                                                                ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                <CommandItemStyle />
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" EnableNoRecordsTemplate="true" NoDetailRecordsText="No records to display." ShowFooter="True">
                                                                    <Columns>

                                                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                        </telerik:GridClientSelectColumn>

                                                                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblIndex" runat="server" Text='<%# Container.ItemIndex %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn UniqueName="lblIndexID" Display="false" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <%# Container.ItemIndex %>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblId0" runat="server" Text='<%# Bind("ContactID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridBoundColumn DataField="Name" HeaderText="Name" SortExpression="Name"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>


                                                                        <telerik:GridBoundColumn DataField="Title" HeaderText="Title" HeaderStyle-Width="140"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Title"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>

                                                                        <telerik:GridBoundColumn DataField="Phone" HeaderText="Phone" HeaderStyle-Width="140"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Phone"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>

                                                                        <telerik:GridBoundColumn DataField="Fax" HeaderText="Fax" HeaderStyle-Width="140"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Fax"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>

                                                                        <telerik:GridBoundColumn DataField="Cell" HeaderText="Cell" HeaderStyle-Width="140"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Cell"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>

                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="140" HeaderText="Email" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Email" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEmail" runat="server"><%#Eval("Email")%></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </telerik:RadAjaxPanel>
                                                    </div>
                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="cf"></div>

                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="adViewEquipments" style="display: none">
                            <div id="accrdequipments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-maps-local-laundry-service"></i>View Equipment</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <asp:Panel runat="server" ID="pnlEqGridButtons">
                                            <div class="btncontainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAddEQ" runat="server" OnClientClick='return AddEquipmentClick(this)' OnClick="lnkAddEQ_Click" CausesValidation="False">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkEditEq" runat="server" OnClientClick='return EditEquipmentClick(this)' OnClick="lnkEditEq_Click" CausesValidation="False">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnCopyEQ" runat="server" OnClientClick='return AddEquipmentClick(this)'
                                                        OnClick="btnCopyEQ_Click" CausesValidation="False">Copy</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkDeleteEQ" runat="server" OnClientClick='return DeleteEquipmentClick(this)'
                                                        OnClick="lnkDeleteEQ_Click" CausesValidation="False">Delete</asp:LinkButton>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <div class="grid_container">
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <telerik:RadCodeBlock ID="RadCodeBlock_Equip" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoad() {
                                                            var grid = $find("<%= RadGrid_Equip.ClientID %>");
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

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Equip" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Location" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Equip" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                        OnNeedDataSource="RadGrid_Equip_NeedDataSource" OnPreRender="RadGrid_Equip_PreRender" PagerStyle-AlwaysVisible="true"
                                                        ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True">
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
                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="unit" HeaderText="Name" SortExpression="unit"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140" FooterText="Total:-"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>


                                                                <telerik:GridBoundColumn DataField="manuf" HeaderText="Manuf." HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="manuf"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="fdesc" HeaderText="Description" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fdesc"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="Type" HeaderText="Type" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Type"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status" HeaderText="Status" HeaderStyle-Width="140" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="Price" HeaderText="Price" HeaderStyle-Width="140" FooterAggregateFormatString="{0:n}" Aggregate="Sum"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Price"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn AutoPostBackOnFilter="true" DataField="last" CurrentFilterFunction="Contains" SortExpression="last" HeaderText="Last Service" HeaderStyle-Width="140" ShowFilterIcon="false" DataType="System.String">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllast" runat="server"><%# Eval("last")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "last"))):""%></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn AutoPostBackOnFilter="true" DataField="since" CurrentFilterFunction="Contains" SortExpression="since" HeaderText="Installed" HeaderStyle-Width="140" ShowFilterIcon="false" DataType="System.String">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSince" runat="server"><%# Eval("since") != DBNull.Value ? String.Format("{0:M/d/yyyy}", Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "since"))) : ""%></asp:Label>
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

                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="adOpenTask">
                            <div id="accrdopentask" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-assignment"></i>Open Tasks</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="HyperLink2" runat="server" Target="_self">Add</asp:HyperLink>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkDeleteTask" OnClick="lnkDeleteTask_Click" runat="server">Delete</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkCloseTask" OnClick="lnkCloseTask_Click" runat="server">Close</asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="grid_container">
                                            <div id="task_wrap_container" class="RadGrid RadGrid_Material FormGrid">
                                                <telerik:RadCodeBlock ID="RadCodeBlock_Tasks" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoad() {
                                                            var grid = $find("<%= RadGrid_Tasks.ClientID %>");
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
                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Tasks" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Prospect" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Tasks" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                        OnNeedDataSource="RadGrid_Tasks_NeedDataSource" PagerStyle-AlwaysVisible="true"
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
                                                                        <asp:HiddenField runat="server" ID="hdID" Value='<%# Eval("ID") %>' />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn SortExpression="Contact" HeaderText="Contact Name" DataField="Contact" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140" ShowFilterIcon="false" HeaderStyle-CssClass="padding-left-right-0">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContact" runat="server" Text='<%#   Eval("Contact").ToString() %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="Subject" HeaderText="Subject" DataField="Subject" ShowFilterIcon="false" HeaderStyle-Width="200" HeaderStyle-CssClass="padding-left-right-0">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="lnkSubject" NavigateUrl='<%# "addtask.aspx?uid=" + Eval("id") + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl + "&tab=opentask")%>'
                                                                            Target="_self" runat="server" Text='<%# Eval("Subject") %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="duedate" HeaderText="Due Date/Date Done" DataField="duedate" ShowFilterIcon="false" HeaderStyle-Width="120" HeaderStyle-CssClass="padding-left-right-0">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDuedate" runat="server" Style='<%#  Eval("statusid").ToString()!="1" ?( string.Format("color:{0}",Convert.ToDateTime( Eval("duedate") )<= System.DateTime.Now ? "RED": "BLACK")) : "Black" %>'
                                                                            Text='<%# (String.IsNullOrEmpty(Eval("duedate").ToString())) ? "No Date Available" : Eval("duedate", "{0:MM/dd/yyyy h:mm tt}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="days" HeaderText="# Days" DataField="days" ShowFilterIcon="false" HeaderStyle-Width="80" HeaderStyle-CssClass="padding-left-right-0">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="Remarks" HeaderText="Desc" HeaderStyle-Width="200" HeaderStyle-CssClass="padding-left-right-0"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Remarks"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="result" HeaderText="Resolution" HeaderStyle-Width="200" HeaderStyle-CssClass="padding-left-right-0"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="result"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="fUser" HeaderText="Assigned to" HeaderStyle-Width="140" HeaderStyle-CssClass="padding-left-right-0"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fUser"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="status" HeaderText="Status" HeaderStyle-Width="120" HeaderStyle-CssClass="padding-left-right-0"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="keyword" HeaderText="Category" HeaderStyle-Width="115"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="CreatedBy" HeaderText="Created By" HeaderStyle-Width="115"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn SortExpression="CreatedDate" HeaderText="Created Date" HeaderStyle-Width="150" DataField="CreatedDate" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCreatedDate" runat="server"
                                                                            Text='<%# (String.IsNullOrEmpty(Eval("CreatedDate").ToString())) ? "" : Eval("CreatedDate", "{0:MM/dd/yyyy h:mm tt}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="screen" HeaderText="Screen" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="screen"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ref" HeaderText="Ref ID" HeaderStyle-Width="100"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="ref"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </telerik:RadAjaxPanel>
                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="adTaskHistory">
                            <div id="accrdtaskhistory" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-assignment-return"></i>Task History</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="HyperLink1" runat="server">Add</asp:HyperLink>
                                            </div>
                                        </div>
                                        <div id="task_history_container" class="grid_container">
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <telerik:RadCodeBlock ID="RadCodeBlock_TasksCompleted" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoad() {
                                                            var grid = $find("<%= RadGrid_TasksCompleted.ClientID %>");
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

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_TasksCompleted" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Prospect" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_TasksCompleted" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                        OnNeedDataSource="RadGrid_TasksCompleted_NeedDataSource" PagerStyle-AlwaysVisible="true"
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

                                                                <telerik:GridBoundColumn DataField="Contact" HeaderText="Contact Name" SortExpression="Contact" HeaderStyle-CssClass="padding-left-right-0"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn SortExpression="Subject" HeaderText="Subject" DataField="Subject" ShowFilterIcon="false" HeaderStyle-CssClass="padding-left-right-0" HeaderStyle-Width="200">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="lnkSubject" NavigateUrl='<%# "addtask.aspx?uid=" + Eval("id") + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl + "&tab=taskhistory") %>'
                                                                            Target="_self" runat="server" Text='<%# Eval("Subject") %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="duedate" HeaderText="Due Date/Date Done" DataField="duedate" ShowFilterIcon="false" HeaderStyle-CssClass="padding-left-right-0" HeaderStyle-Width="150">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDuedate" runat="server" Style='<%#  Eval("statusid").ToString()!="1" ?( string.Format("color:{0}",Convert.ToDateTime( Eval("duedate") )<= System.DateTime.Now ? "RED": "BLACK")) : "Black" %>'
                                                                            Text='<%# (String.IsNullOrEmpty(Eval("duedate").ToString())) ? "No Date Available" : Eval("duedate", "{0:MM/dd/yyyy h:mm tt}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="days" HeaderText="# Days" DataField="days" ShowFilterIcon="false" HeaderStyle-CssClass="padding-left-right-0" HeaderStyle-Width="80">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="Remarks" HeaderText="Desc" HeaderStyle-Width="200" HeaderStyle-CssClass="padding-left-right-0"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Remarks"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="result" HeaderText="Resolution" HeaderStyle-Width="200" HeaderStyle-CssClass="padding-left-right-0"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="result"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="fUser" HeaderText="Assigned to" HeaderStyle-Width="140" HeaderStyle-CssClass="padding-left-right-0"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fUser"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="status" HeaderText="Status" HeaderStyle-Width="120" HeaderStyle-CssClass="padding-left-right-0"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="keyword" HeaderText="Category" HeaderStyle-Width="115"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="keyword"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="CreatedBy" HeaderText="Created By" HeaderStyle-Width="115"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CreatedBy"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn SortExpression="CreatedDate" HeaderText="Created Date" HeaderStyle-Width="150" DataField="CreatedDate" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCreatedDate" runat="server"
                                                                            Text='<%# (String.IsNullOrEmpty(Eval("CreatedDate").ToString())) ? "" : Eval("CreatedDate", "{0:MM/dd/yyyy h:mm tt}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="screen" HeaderText="Screen" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="screen"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ref" HeaderText="Ref ID" HeaderStyle-Width="100"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="ref"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </telerik:RadAjaxPanel>
                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="adOpportunities">
                            <div id="accrdopportunities" class="collapsible-header accrd accordian-text-custom"><i class="mdi-image-style"></i>Opportunities</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <%--<div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="lnkAddopp" runat="server">Add</asp:HyperLink>
                                            </div>
                                        </div>--%>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="lnkAddopp" runat="server">Add</asp:HyperLink>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkEditOpp" runat="server" CausesValidation="False" OnClick="lnkEditOpp_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkCopyOpp" runat="server" CausesValidation="False" OnClick="lnkCopyOpp_Click">Copy</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>
                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkDeleteOpp" runat="server" OnClientClick="return CheckDelete();" CausesValidation="False" OnClick="lnkDeleteOpp_Click">Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkExcelOpp" runat="server" OnClick="lnkExcelOpp_Click">Export to Excel</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <%--<li>
                                                    <ul id="dropdown1" class="dropdown-content">
                                                        <li>
                                                            <asp:LinkButton ID="lnkOpportunityReport" runat="server" CausesValidation="False" OnClick="lnkOpportunityReport_Click">Opportunity Report</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <div class="btnlinks">
                                                        <a class="dropdown-button" id="lnkReport" runat="server" data-beloworigin="true" href="#!" data-activates="dropdown1">Reports
                                                        </a>
                                                    </div>
                                                </li>--%>
                                            </ul>
                                        </div>
                                        <div class="grid_container">
                                            <div class="RadGrid RadGrid_Material FormGrid">

                                                <telerik:RadCodeBlock ID="RadCodeBlock_Opportunity" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoad() {
                                                            var grid = $find("<%= RadGrid_Opportunity.ClientID %>");
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

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Opportunity" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Prospect" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Opportunity" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                        OnNeedDataSource="RadGrid_Opportunity_NeedDataSource" PagerStyle-AlwaysVisible="true"
                                                        OnExcelMLExportRowCreated="RadGrid_Opportunity_ExcelMLExportRowCreated"
                                                        OnItemCreated="RadGrid_Opportunity_ItemCreated"
                                                        OnItemDataBound="RadGrid_Opportunity_ItemDataBound"
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

                                                                <telerik:GridTemplateColumn DataField="id" AutoPostBackOnFilter="true" SortExpression="id" HeaderText="Opportunity #" ShowFilterIcon="false" HeaderStyle-Width="120">
                                                                    <ItemTemplate>
                                                                        <a href="addopprt.aspx?uid=<%# Eval("id") %>&redirect=<%# HttpUtility.UrlEncode(Request.RawUrl)%>"><%# Eval("id") %></a>
                                                                        <asp:Label ID="lblID" Visible="false" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                                    </FooterTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="fdesc" HeaderText="Opportunity Name" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fdesc"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <%--<telerik:GridTemplateColumn SortExpression="fdesc" HeaderText="Opportunity Name" DataField="fdesc" ShowFilterIcon="false"  HeaderStyle-Width="140">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="lnkname" NavigateUrl='<%# "addopprt.aspx?uid=" + Eval("id") %>'
                                                                            Target="_self" runat="server" Text='<%# Eval("fdesc") %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>--%>

                                                                <%--<telerik:GridBoundColumn DataField="CompanyName" HeaderText="Customer" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CompanyName"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>--%>

                                                                <telerik:GridBoundColumn DataField="fuser" HeaderText="Assigned To" HeaderStyle-Width="115"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fuser"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn DataField="CreateDate" SortExpression="CreateDate" HeaderText="Date Created" ShowFilterIcon="false" CurrentFilterFunction="Contains" HeaderStyle-Width="130" DataType="System.String">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("CreateDate","{0:d}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <%--<telerik:GridTemplateColumn SortExpression="duedate" HeaderText="Close Date" DataField="duedate" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("closedate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>--%>

                                                                <telerik:GridBoundColumn DataField="Probability" HeaderText="Probability" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Probability"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="status" HeaderText="Status" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="Product" HeaderText="Product" HeaderStyle-Width="115"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Product"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Company" HeaderText="Company" HeaderStyle-Width="115"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Company"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <%--<telerik:GridTemplateColumn DataField="fFor" HeaderText="Type" SortExpression="fFor"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="90"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblType" runat="server" Text='<%# Convert.ToString(Eval("fFor"))=="ACCOUNT"?"Existing":"Lead" %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>--%>

                                                                <telerik:GridBoundColumn DataField="StageWithProbability" HeaderText="Stage" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="StageWithProbability"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="Dept" HeaderText="Department" HeaderStyle-Width="115"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Dept"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn DataField="revenue" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="revenue" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Budgeted Amt" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "revenue", "{0:c}")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="BidPrice" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="BidPrice" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Bid Price" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBidPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BidPrice", "{0:c}")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="FinalBid" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="FinalBid" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Final Bid" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFinalBid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FinalBid", "{0:c}")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="estimate" SortExpression="estimate" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Estimate #" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnGridEstimate" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "estimate")%>'></asp:HiddenField>
                                                                        <asp:Repeater ID="rptEstimates" runat="server">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton Style="padding: 0" ID="lnkEstimate" runat="server" CommandName="Estimate #" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EstimateID")%>' OnCommand="LinkButton_Click"><%#DataBinder.Eval(Container.DataItem, "EstimateID")%></asp:LinkButton>
                                                                                <asp:Label ID="lblComma" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Last") == "false" ? ", " : ""%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="EstimateDiscounted" HeaderText="Discounted" HeaderStyle-Width="100"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="EstimateDiscounted"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn DataField="job" SortExpression="job" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Project #" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnGridProject" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "job")%>'></asp:HiddenField>
                                                                        <asp:Repeater ID="rptProjects" runat="server">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton Style="padding: 0" ID="lnkjob" runat="server" CommandName="Project #" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ProjectID")%>' OnCommand="LinkButton_Click"><%#DataBinder.Eval(Container.DataItem, "ProjectID")%></asp:LinkButton>
                                                                                <asp:Label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Last") == "false" ? ", " : ""%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="closedate" SortExpression="closedate" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Bid Date" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldate" runat="server" Text='<%# Convert.ToString(Eval("status"))=="Open"?"":Eval("closedate","{0:d}" )%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="Referral" HeaderText="Referral" HeaderStyle-Width="115"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Referral"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>


                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </telerik:RadAjaxPanel>

                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="adEmails">
                            <div id="accrdemails" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-inbox"></i>Emails</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <div class="btncontainer">
                                                    <div class="btnlinks">
                                                        <asp:HyperLink ID="lnkNewEmail" Target="_self" runat="server">Email</asp:HyperLink>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkRefreshMails" runat="server" CausesValidation="False" OnClick="lnkRefreshMails_Click">Refresh</asp:LinkButton>
                                                        <asp:HiddenField ID="hdnMailct" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="grid_container">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock_Mail" runat="server">
                                                            <script type="text/javascript">
                                                                function pageLoad() {
                                                                    var grid = $find("<%= RadGrid_Mail.ClientID %>");
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

                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Mail" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Prospect" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Mail" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                OnNeedDataSource="RadGrid_Mail_NeedDataSource" PagerStyle-AlwaysVisible="true"
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

                                                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Image runat="server" ID="imgType" Width="11px" ImageUrl='<%# Eval("type").ToString() != "0" ? "images/uparr.png" : "images/downarr.png"%>' />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn SortExpression="subject" HeaderText="Subject" DataField="subject" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lnkMsgID" Visible="false" runat="server" Text='<%# Eval("guid") %>'></asp:Label>
                                                                                <asp:HyperLink ID="lnkSub" NavigateUrl='<%# "email.aspx?aid=" + Eval("guid") +"&rol="+hdnRol.Value %>'
                                                                                    Target="_self" runat="server" Text='<%# Eval("subject") %>'></asp:HyperLink>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>



                                                                        <telerik:GridBoundColumn DataField="from" HeaderText="From" HeaderStyle-Width="140"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="from"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>

                                                                        <telerik:GridBoundColumn DataField="to" HeaderText="To" HeaderStyle-Width="140"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="to"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>

                                                                        <telerik:GridBoundColumn DataField="SentDate" HeaderText="Date Sent" HeaderStyle-Width="140"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="SentDate"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>

                                                                        <telerik:GridBoundColumn DataField="recDate" HeaderText="Rec. Date" HeaderStyle-Width="140"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="recDate"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </telerik:RadAjaxPanel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="adNotes">
                            <div id="accrdnotes" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Notes & Attachments</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <asp:UpdatePanel ID="updatepnl" runat="server">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="RadGrid_Documents" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <div class="btncontainer">
                                                    <asp:Panel ID="pnlDocumentButtons" runat="server">
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkAddNote" runat="server" CausesValidation="False"
                                                                OnClientClick="return AddDocumentClick(this);"
                                                                OnClick="lnkAddNote_Click">Add</asp:LinkButton>
                                                        </div>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkEditNote" runat="server" CausesValidation="False"
                                                                OnClick="lnkEditNote_Click">Edit</asp:LinkButton>
                                                        </div>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click"
                                                                OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                                        </div>

                                                    </asp:Panel>
                                                </div>
                                                <div class="grid_container">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock_Documents" runat="server">
                                                            <script type="text/javascript">
                                                                function pageLoad() {
                                                                    var grid = $find("<%= RadGrid_Documents.ClientID %>");
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

                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Prospect" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                OnNeedDataSource="RadGrid_Documents_NeedDataSource" PagerStyle-AlwaysVisible="true"
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

                                                                        <telerik:GridTemplateColumn SortExpression="subject" HeaderText="Subject" DataField="subject"
                                                                            HeaderStyle-Width="200" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSub" runat="server" Text='<%# Eval("subject") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn SortExpression="body" HeaderText="Note (body)" DataField="body" ShowFilterIcon="false" HeaderStyle-Width="300">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblBody" runat="server" Text='<%# Eval("body") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn SortExpression="filename" HeaderText="File Name" DataField="filename"
                                                                            HeaderStyle-Width="220" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lblName" runat="server" CausesValidation="false" CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                    OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'> </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridBoundColumn DataField="doctype" HeaderText="File Type" HeaderStyle-Width="140"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="doctype"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>


                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </telerik:RadAjaxPanel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li id="tbLogs" runat="server" style="display: none">
                            <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row m-b-0">
                                                <div class="RadGrid RadGrid_Material">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                try {
                                                                    var grid = $find("<%= RadGrid_gvLogs.ClientID %>");
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvLogs" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvLogs" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true" OnItemCreated="RadGrid_gvLogs_ItemCreated"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvLogs_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false" DataKeyNames="fUser">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbldate" runat="server" Text='<%# Eval("CreatedStamp", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbltime" runat="server" Text='<%# Eval("CreatedStamp","{0: hh:mm tt}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="fUser" SortExpression="fUser" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUpdBy" runat="server" Text='<%# Eval("fUser") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Field" SortExpression="Field" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Field" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblField" runat="server" Text='<%# Eval("field") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="OldVal" SortExpression="OldVal" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Old Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOldVal" runat="server" Text='<%# Eval("OldVal") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="NewVal" SortExpression="NewVal" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="New Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNewVal" runat="server" Text='<%# Eval("NewVal") %>'></asp:Label>
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
                        <li runat="server" id="adSystemInfo">
                            <div id="accrdsystem" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-info"></i>System Info</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12 m12 l12">
                                                <div class="systemInfo-css" >
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td class="w-70" >&nbsp;
                                                                </td>
                                                                <td class="w-70">Created By:
                                                                </td>
                                                                <td class="width-r30">
                                                                    <span style="font-weight: bold;">
                                                                        <asp:Label ID="lblCreate" runat="server" Style="font-weight: bolder"></asp:Label></span>
                                                                </td>
                                                                <td class="w-100">Last Updated By:
                                                                </td>
                                                                <td class="width-r30">
                                                                    <span style="font-weight: bold;">
                                                                        <asp:Label ID="lblUpdate" runat="server" Style="font-weight: bolder"></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
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

    <asp:HiddenField ID="hdnRol" runat="server" />
    <!-- ===================================================== START POPUP CODE ================================================-->
    <telerik:RadWindowManager ID="RadWindowManagerAddOpp" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowSource" Skin="Material" VisibleTitlebar="true" Title="Add Source" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="220">
                <ContentTemplate>
                    <div>
                        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelSource">
                            <div class="input-field col s12">
                                <div class="row">
                                    <label for="txtnewSource">Source</label>
                                    <asp:TextBox ID="txtnewSource" ValidationGroup="Source" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="Source" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtnewSource"
                                        Display="Dynamic" ErrorMessage="Source Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" PopupPosition="BottomLeft"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                            </div>

                            <div class="btnlinks">
                                <asp:LinkButton ID="btnAddSource" runat="server" ValidationGroup="Source" OnClick="btnAddSource_Click">Save</asp:LinkButton>
                            </div>

                        </telerik:RadAjaxPanel>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowContact" Skin="Material" VisibleTitlebar="true" Title="Add Contact" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="270">
                <ContentTemplate>
                    <div>
                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                                            Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator12">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtContcName" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblContcName" AssociatedControlID="txtContcName">Contact Name</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatortxtTitle" runat="server" ControlToValidate="txtTitle"
                                            Display="None" ErrorMessage="Title  Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtendertxtTitle"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidatortxtTitle">
                                        </asp:ValidatorCalloutExtender>--%>
                                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblTitle" AssociatedControlID="txtTitle">Title</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtContPhone"
                                            Display="None" ErrorMessage="Phone Required" SetFocusOnError="True" ValidationGroup="cont"
                                            Enabled="False"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator13_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator13">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtContPhone" runat="server" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblPhone" AssociatedControlID="txtContPhone">Phone</asp:Label>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContFax" runat="server" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblFax" AssociatedControlID="txtContFax">Fax</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContCell" runat="server" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblCell" AssociatedControlID="txtContCell">Cell</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtContEmail"
                                            Display="None" ErrorMessage="Email Required" SetFocusOnError="True" ValidationGroup="cont"
                                            Enabled="False"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator16_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator16">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContEmail"
                                            Display="None" ErrorMessage="Invalid Email" ValidationGroup="cont" SetFocusOnError="True"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtContEmail" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblEmail" AssociatedControlID="txtContEmail">Email</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                        <div class="btnlinks">
                            <asp:LinkButton ID="lnkContactSave" runat="server" OnClick="lnkContactSave_Click" ValidationGroup="cont">Save</asp:LinkButton>
                        </div>

                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowNotes" Skin="Material" VisibleTitlebar="true" Title="Note" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="320">
                <ContentTemplate>
                    <div>
                        <div class="input-field col s12">
                            <div class="row">

                                <asp:HiddenField ID="hdnNoteID" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator46" runat="server" ControlToValidate="txtNoteSub"
                                    Display="None" ErrorMessage="Subject Required" SetFocusOnError="True" ValidationGroup="note"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator46_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator46">
                                </asp:ValidatorCalloutExtender>
                                <asp:TextBox ID="txtNoteSub" runat="server" MaxLength="70"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSubject" AssociatedControlID="txtNoteSub">Subject</asp:Label>
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:Label runat="server" ID="lblBody" AssociatedControlID="txtNoteBody">Body</asp:Label>
                                <asp:TextBox ID="txtNoteBody" runat="server" CssClass="materialize-textarea" TextMode="MultiLine"></asp:TextBox>

                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                <asp:LinkButton ID="lnkPostback" runat="server" CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                            </div>
                        </div>
                        <div style="clear: both;"></div>

                        <div class="btnlinks">
                            <asp:LinkButton ID="lnkUploadDoc" runat="server" OnClick="lnkUploadDoc_Click" ValidationGroup="note">Save</asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowCompany" Skin="Material" VisibleTitlebar="true" Title="Select Company" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="220">
                <ContentTemplate>
                    <div>
                        <div class="input-field col s12">
                            <div class="row">
                                <label class="drpdwn-label">Company</label>
                                <asp:DropDownList ID="ddlCompanyEdit" CssClass="browser-default" runat="server"></asp:DropDownList>
                            </div>
                        </div>

                        <div style="clear: both;"></div>
                        <div class="btnlinks">
                            <asp:LinkButton ID="btnCompanyEdit" runat="server" Text="Save" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <!-- ===================================================== END POPUP CODE ================================================-->

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewContact" Value="Y" />


    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewEquipment" Value="Y" />

    <asp:HiddenField runat="server" ID="hdnCon" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.maskedinput/1.4.1/jquery.maskedinput.min.js"></script>
    <script defer src="https://use.fontawesome.com/releases/v5.0.10/js/all.js"></script>
    <%--<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.98.1/js/materialize.min.js"></script>--%>
    <script>
        $(document).ready(function () {

            //$("#accrdleadInfo").addClass("active");
            //$("#firstTab").attr("style", "display:block");


            $("[id*=txtCustomer]").change(function () {
                var customer = $("[id*=txtCustomer]").val();
                var prospectName = $("[id*=txtProspectName]").val();
                if (prospectName == "") {
                    $("[id*=txtProspectName]").val(customer);
                    Materialize.updateTextFields();
                }
            });

            $("[id*=txtPhone]").mask("(999) 999-9999? Ext 99999");
            $("[id*=txtPhone]").bind('paste', function () { $(this).val(''); });
            $("[id*=txtCell]").mask("(999) 999-9999");
            $("[id*=txtFax]").mask("(999) 999-9999");


            //Contact Popup
            $("[id*=txtContPhone]").mask("(999) 999-9999? Ext 99999");
            $("[id*=txtContPhone]").bind('paste', function () { $(this).val(''); });
            $("[id*=txtContCell]").mask("(999) 999-9999");
            $("[id*=txtContFax]").mask("(999) 999-9999");


        });

        function pageLoad() {
            $("#<%=txtNoteBody.ClientID%>").on("input propertychange", function () {
                CharLimit(this, 492);
            });
        }

    </script>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {
            $("[id*=txtContPhone]").mask("(999) 999-9999? Ext 99999");
            $("[id*=txtContPhone]").bind('paste', function () { $(this).val(''); });
            $("[id*=txtContCell]").mask("(999) 999-9999");
            $("[id*=txtContFax]").mask("(999) 999-9999");
        });
    </script>



    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('a[href^="#accrd"]').on('click', function (e) {
                e.preventDefault();
                $("a.anchorActive").removeClass("anchorActive");
                $(this).addClass("anchorActive");
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



            function collapseAll() {
                $(".accrd").removeClass(function () {
                    return "active";
                });
                $('html, body').stop().animate({
                    'scrollTop': 0
                }, 300, 'swing');
                $(".collapsible-accordion").collapsible({ accordion: true });
                $(".collapsible-accordion").collapsible({ accordion: false });

            }

            function expandAll() {
                $('html, body').stop().animate({
                    'scrollTop': 0
                }, 300, 'swing');
                $(".accrd").addClass("active");
                $(".collapsible-accordion").collapsible({ accordion: false });
            }

            $('.collapse-expand').on('click', function (e) {
                if (this.classList.contains("opened") === true) {
                    this.classList.remove("opened");
                    collapseAll();
                    $("#expcolp").attr("title", "Expand All");
                    $("#expcolp").addClass('mdi-navigation-unfold-more');
                    $("#expcolp").removeClass('mdi-navigation-unfold-less');
                }
                else {
                    this.classList.add("is-active");
                    this.classList.add("opened");
                    expandAll();
                    $("#expcolp").attr("title", "Collapse All");
                    $("#expcolp").addClass('mdi-navigation-unfold-less');
                    $("#expcolp").removeClass('mdi-navigation-unfold-more');
                }
            });

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

            $(".dropdown-content.select-dropdown li").on("click", function () {
                var that = this;
                setTimeout(function () {
                    if ($(that).parent().hasClass('active')) {
                        $(that).parent().removeClass('active');
                        $(that).parent().hide();
                    }
                }, 100);
            });


            $('#phone-demo').mask("(999) 999-9999? Ext 99999");
            $('#phone-demo').bind('paste', function () { $(this).val(''); });
            $('#phone_cell').mask("(999) 999-9999");
            $('#phone_fax').mask("(999) 999-9999");

            $('#modal-phone').mask("(999) 999-9999? Ext 99999");
            $('#modal-phone').bind('paste', function () { $(this).val(''); });
            $('#modal-cell').mask("(999) 999-9999");
            $('#modal-fax').mask("(999) 999-9999");

            setTimeout(function () {
                $("#<%=uc_CustomerSearch1._txtCustomer.ClientID%>").focus();
            }, 500);

            ///////////// Quick Codes //////////////
            $("#<%=txtRemarks.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRemarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
        });



        function dtaa() {
            this.prefixText = null;
            this.con = null;
            this.custID = null;
        }

        //Company Name autopopulate
        $("#<%=txtCustomer.ClientID%>").autocomplete({

            source: function (request, response) {

                var dtaaa = new dtaa();
                dtaaa.prefixText = request.term;
                query = request.term;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "CustomerAuto.asmx/GetCustomerProspect",
                    data: JSON.stringify(dtaaa),
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        response($.parseJSON(data.d));
                    },
                    error: function (result) {
                        alert("Due to unexpected errors we were unable to load contact name");
                    }
                });
            },
            select: function (event, ui) {
                $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                return false;
            },
             <%-- change: function (event, ui) {
                  if (ui.item) {
                      noty({
                          text: 'Company name already exist!',
                          type: 'warning',
                          layout: 'topCenter',
                          closeOnSelfClick: false,
                          timeout: false,
                          theme: 'noty_theme_default',
                          closable: true
                      });
                      $(this).val("");
                      $("#<%=txtProspectName.ClientID%>").val("");
                      return false;
                    }
                },--%>
            focus: function (event, ui) {
                $("#<%=txtCustomer.ClientID%>").val(ui.item.label);

                return false;
            },
            minLength: 0,
            delay: 250
        }).data("ui-autocomplete")._renderItem = function (ul, item) {

            var result_item = item.label;
            var result_desc = item.desc;
            var result_type = item.type;
            var result_Prospect = item.prospect;
            var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
            result_item = result_item.replace(x, function (FullMatch, n) {
                return '<span class="highlight">' + FullMatch + '</span>'
            });
            if (result_desc != null) {
                result_desc = result_desc.replace(x, function (FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });
            }
            var color = 'Black';
            if (result_Prospect != 0) {
                color = 'brown';
            }


            var color = '#222';
            if (result_Prospect != 0) {
                display = "inline-block";
            }
            else {
                display = "none";
            }
            return $("<li></li>")
                .data("item.autocomplete", item)
                .append("<span class='auto_item'><i style='display:" + display + ";margin-right:8px;width:auto;color:#1565C0 !important;' class='fas fa-thumbs-up' title='Prospect'></i>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
                .appendTo(ul);
        };




        $("#<%=txtProspectName.ClientID%>").autocomplete({

            source: function (request, response) {
                //debugger
                var dtaaa = new dtaa();
                dtaaa.prefixText = request.term;
                query = request.term;
                dtaaa.isProspect = 1;
                dtaaa.custID = 0;

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "CustomerAuto.asmx/GetLocationProspect",
                    data: JSON.stringify(dtaaa),
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        response($.parseJSON(data.d));
                    },
                    error: function (result) {
                        alert("Due to unexpected errors we were unable to load contact name");
                    }
                });
            },
            select: function (event, ui) {
                var str = ui.item.label;
                if (str == "No Record Found!") {
                    $("#<%=txtProspectName.ClientID%>").val("");
                }
                else {
                    $("#<%=txtProspectName.ClientID%>").val(ui.item.label);
                }
                return false;
            },
            change: function (event, ui) {
                if (ui.item) {
                    noty({
                        text: 'Location lead name already exist!',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: false,
                        theme: 'noty_theme_default',
                        closable: true
                    });

                    $(this).val("");
                    return false;
                }
            },
            focus: function (event, ui) {

                $("#<%=txtProspectName.ClientID%>").val(ui.item.label);

                return false;
            },
            minLength: 0,
            delay: 250
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            var result_item = item.label;
            var result_desc = item.desc;
            var result_Prospect = item.ProspectID;
            var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
            result_item = result_item.replace(x, function (FullMatch, n) {
                return '<span class="highlight">' + FullMatch + '</span>'
            });
            if (result_desc != null) {
                result_desc = result_desc.replace(x, function (FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });
            }
            var color = 'gray';
            if (result_Prospect == 0) {
                color = 'Black';
            }
            else {
                color = 'brown';
            }

            return $("<li></li>")
                .data("item.autocomplete", item)
                .append("<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
                .appendTo(ul);
        };


    </script>
</asp:Content>

