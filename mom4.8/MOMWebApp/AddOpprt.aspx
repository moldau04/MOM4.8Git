
<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddOpprt" ValidateRequest="false" Codebehind="AddOpprt.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="uc_CustomerSearch.ascx" TagName="uc_CustomerSearch" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .highlight {
            background-color: Yellow;
        }

        .highlighted {
            background-color: Yellow;
        }

        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
            max-width: 600px;
        }

        .btnlinks input[type=checkbox] {
            margin-top: 5px;
            margin-left: 10px;
        }

        .RadGrid_EstimatesLinked > .rgDataDiv{
            height: auto !important;
            max-height: 175px;
        }
    </style>
    <script type="text/javascript" src="js/jquery.formatCurrency.js"></script>

    <script>
        $(document).ready(function () {
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

            //Company Name autopopulate
            $("#<%=txtCompanyName.ClientID%>").autocomplete({

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
                    $("#<%=txtCompanyName.ClientID%>").val(ui.item.label);
                    $("#<%=hdnId.ClientID%>").val(ui.item.rolid);
                    if (ui.item.prospect == 1) {
                        <%--  $("#<%=txtName.ClientID%>").val(ui.item.prospectloclabel);--%>
                        $("#<%=txtName.ClientID%>").val(ui.item.lName);
                        $("#<%=hdnOwnerID.ClientID%>").val(ui.item.value);
                        $("#<%=hdnProsID.ClientID%>").val(ui.item.value);
                        $("#<%=hdnCustId.ClientID%>").val("0");
                        $("#<%=lblType.ClientID%>").html("Lead");
                        $("#<%=ddlBusinessType.ClientID%>").val(ui.item.BusinessType);
                        document.getElementById('<%=btnFillTasks.ClientID%>').click();
                    }
                    else {
                        $("#<%=hdnOwnerID.ClientID%>").val(ui.item.value);
                        $("#<%=hdnCustId.ClientID%>").val(ui.item.value);
                        $("#<%=hdnProsID.ClientID%>").val("0");
                        $("#<%=lblType.ClientID%>").html("Existing");
                        $("#<%=txtName.ClientID%>").val(ui.item.lName);
                        $("#<%=ddlBusinessType.ClientID%>").val(ui.item.BusinessType);
                    }
                    Materialize.updateTextFields();
                    return false;
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        $(this).val("");
                        return false;
                    }
                },
                focus: function (event, ui) {
                    $("#<%=txtCompanyName.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                //_renderItem = function (ul, item) {
                //debugger
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

                //return $("<li></li>")
                //    .data("item.autocomplete", item)
                //    .append("<a style='color:" + color + ";'>" + result_item + " <span style='color:Gray;'>" + result_desc + "</span></a>")
                //    .appendTo(ul);

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



            $("#<%=txtName.ClientID%>").autocomplete({

                source: function (request, response) {

                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    dtaaa.custID = 0;
                    

                    if ($('#<%=hdnProsID.ClientID%>').val() != '' && $('#<%=hdnProsID.ClientID%>').val() != '0') {
                        dtaaa.isProspect = 1;
                        dtaaa.custID = $('#<%=hdnProsID.ClientID%>').val();
                    } else {
                        dtaaa.isProspect = 0;
                        if (document.getElementById('<%=txtCompanyName.ClientID%>').value != '') {
                            if (document.getElementById('<%=hdnCustId.ClientID%>').value != '') {
                                dtaaa.custID = document.getElementById('<%=hdnCustId.ClientID%>').value;
                            }
                        }
                        
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        //url: "CustomerAuto.asmx/getTaskContacts",
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
                    //debugger
                    var str = ui.item.label;
                    if (str == "No Record Found!") {
                        $("#<%=txtName.ClientID%>").val("");
                    }
                    else {
<%--                        alert($('#<%= lnkLocationID.ClientID %>').href);
                        $('#<%= lnkLocationID.ClientID %>').href = "www.google.com";--%>
                        if (ui.item.ProspectID == 0) {
                            $("#<%=hdnId.ClientID%>").val(ui.item.rolid);
                            $("#<%=txtName.ClientID%>").val(ui.item.label);
                            $("#<%=hdnLocId.ClientID%>").val(ui.item.value);
                            $("#<%=hdnCustId.ClientID%>").val(ui.item.value);
                            $("#<%=hdnOwnerID.ClientID%>").val(ui.item.value);
                            $("#<%=lblType.ClientID%>").html("Existing");
                            $("#<%=txtCompanyName.ClientID%>").val(ui.item.CompanyName);
                            $("#<%=ddlBusinessType.ClientID%>").val(ui.item.BusinessType);
                            document.getElementById('<%=btnFillTasks.ClientID%>').click();
                        }
                        else {
                            $("#<%=hdnId.ClientID%>").val(ui.item.rolid);
                            $("#<%=txtName.ClientID%>").val(ui.item.label);
                            $("#<%=hdnLocId.ClientID%>").val("0");
                            $("#<%=hdnCustId.ClientID%>").val("0");
                            $("#<%=hdnOwnerID.ClientID%>").val(ui.item.value);
                            $("#<%=hdnProsID.ClientID%>").val(ui.item.ProspectID);
                            $("#<%=lblType.ClientID%>").html("Lead");
                            $("#<%=txtCompanyName.ClientID%>").val(ui.item.CompanyName);
                            $("#<%=ddlBusinessType.ClientID%>").val(ui.item.BusinessType);
                            document.getElementById('<%=btnFillTasks.ClientID%>').click();
                        }
                    }
                    Materialize.updateTextFields();
                    return false;
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        $(this).val("");
                        return false;
                    }
                },
                focus: function (event, ui) {

                    $("#<%=txtName.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                //._renderItem = function (ul, item) {

                var result_item = item.label;
                var result_desc = item.desc;
                //var result_type = item.type;
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

                //return $("<li></li>")
                //    .data("item.autocomplete", item)
                //    .append("<a style='color:" + color + ";'>" + result_item + " <span style='color:Gray;'> " + result_desc + "</span></a>")
                //    .appendTo(ul);
                return $("<li></li>")
               .data("item.autocomplete", item)
               .append("<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
               .appendTo(ul);
            };




        });

        $("#ctl00_ContentPlaceHolder1_txtCompanyName").keyup(function (event) {
            var hdnCustId = document.getElementById('ctl00_ContentPlaceHolder1_hdnCustId');
            if (document.getElementById('ctl00_ContentPlaceHolder1_txtCompanyName').value == '') {
                hdnCustId.value = '';
                $('#<%= lnkCustomerID.ClientID %>').removeAttr("href");
             }
         });

        $("#ctl00_ContentPlaceHolder1_txtName").keyup(function (event) {
            var hdnLocId = document.getElementById('ctl00_ContentPlaceHolder1_hdnLocID');
            if (document.getElementById('ctl00_ContentPlaceHolder1_txtName').value == '') {
                hdnLocId.value = '';
                $('#<%= lnkLocationID.ClientID %>').removeAttr("href");
                    <%--    $('#<%=ddlTerr.ClientID %>').val('');
                      $('#<%=ddlTerr2.ClientID %>').val('');--%>
            }
        });
    </script>

    <script type="text/javascript">

        function EditTicketClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeTicket.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        $(document).ready(function () {

            setInterval(serviceCall, 600000);
            //AutoCompleteText('CustomerAuto.asmx/getTaskContacts', '<%= txtName.ClientID %>', '<%= hdnId.ClientID %>', '<%= btnFillTasks.ClientID %>', '<%= lblType.ClientID %>', '<%= hdnOwnerID.ClientID %>')

            $("#<%= txtName.ClientID %>").keyup(function (e) {

                var hdnId = document.getElementById('<%= hdnId.ClientID %>');
                if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                    hdnId.value = '';
                }
                if (e.value == '') {
                    hdnId.value = '';
                }
            });

            $("#<%=txtAmount.ClientID%>").blur(function () {
                $(this).formatCurrency();
            });

            if ($(window).width() > 767) {
                $('#<%=txtRemarks.ClientID%>').focus(function () {
                    $(this).animate({
                        //right: "+=0",
                        width: '520px',
                        height: '75px'
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

                <%--  $('#<%=txtDesc.ClientID%>').focus(function () {
                    $(this).animate({
                        //right: "+=0",
                        width: '520px',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });--%>

              <%--  $('#<%=txtDesc.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });--%>
            }

        });

        function ChkLoc(sender, args) {
            var hdnId = document.getElementById('<%=hdnId.ClientID%>');
            if (hdnId.value == '') {
                args.IsValid = false;
            }
        }

        function CheckFollowup(id, checked) {

            var r = confirm('Would you like to create a follow-up task at this time?');
            if (r == true) {
                window.setTimeout(function () { window.location.href = "addtask.aspx?fl=2&uid=" + id }, 2000);
            }

        }

        function serviceCall() {
            var rol = document.getElementById('<%= hdnId.ClientID %>');
            var type = -1;
            var UID = 0;
            if (document.getElementById('<%= hdnMailType.ClientID %>').value != '')
                type = document.getElementById('<%= hdnMailType.ClientID %>').value;
            if (document.getElementById('<%= hdnUID.ClientID %>').value != '')
                UID = document.getElementById('<%= hdnUID.ClientID %>').value;

            if (rol.value != '') {
                $.ajax({
                    type: "POST",
                    url: 'CustomerAuto.asmx/CheckEmail',
                    data: '{"rol":"' + rol.value + '","type":"' + type + '","uid":"' + UID + '"}',
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

        function SelectStatus() {

            var Status = $('[id*=ddlStatus] option:selected').text();

            if (Status == 'Withdrawn' || Status == 'Disqualified' || Status == 'Sold') {

                $('[id*=CloseDateDiv]').css("display", "block");
            }

            else {
                $('[id*=CloseDateDiv]').css("display", "none");

            }
        }

        //--Estimate---
        <%--function AddEstimateClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeEquipment.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditEstimateClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeEquipment.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteEstimateClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteEquipment.ClientID%>').value;
            if (id == "Y") {

                return SelectedRowDelete('<%= RadGrid_EstimatesLinked.ClientID%>', 'Estimate');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }--%>

        function ConvertEstConfirm() {
            if ($("#<%= RadGrid_EstimatesLinked.ClientID%>").find('input[type="checkbox"]:checked').length == 0) {
                //alert('Please select items to delete.');
                noty({ text: 'Please select at least an estimate to convert.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
            return confirm('Do you really want to convert selected estimates to project?');
        }

        //function DeleteEstConfirm() {
        //    return confirm('Do you really want to delete selected estimates?');
        //}

        function DeleteEstConfirm() {
            if ($("#<%= RadGrid_EstimatesLinked.ClientID%>").find('input[type="checkbox"]:checked').length == 0) {
                //alert('Please select items to delete.');
                noty({ text: 'Please select at least an estimate to delete.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
            return confirm('Do you really want to delete selected estimates?');
        }

        function SingleSelectValidation() {
            var selectedCount = $("[id$='RadGrid_EstimatesLinked_GridData'] tbody").find('input[type="checkbox"]:checked').length;
            if (selectedCount != 1) {
                //alert('Please select items to delete.');
                noty({ text: 'Should select an estimate to continue!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
            return true;
        }
    </script>

    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Opp" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Logs"/>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkAddnew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnFillTasks">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OpenTasks" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="pnlContactButtons" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Documents" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="dvCompanyPermission" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="HyperLink1" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="HyperLink2" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                           <telerik:AjaxUpdatedControl ControlID="lnkCustomerID" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="lnkLocationID" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
            
                    <telerik:AjaxUpdatedControl ControlID="lnkNewEmail" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            
             <telerik:AjaxSetting AjaxControlID="lnkDeleteDoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Documents" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkDeleteEstimate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_EstimatesLinked" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Logs" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkProjectEstimate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_EstimatesLinked" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Logs" LoadingPanelID="RadAjaxLoadingPanel_Opp" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Opp" runat="server">
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
                                        <i class="mdi-image-style"></i>&nbsp;
                                           <asp:Label ID="lblHeader" runat="server">Opportunity</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkSave" runat="server" OnClick="lnkSave_Click">Save</asp:LinkButton>
                                        </div>
                                        <%--<div class="btnlinks">
                                            <asp:LinkButton ID="lnkEstimate" runat="server" CausesValidation="False" ToolTip="New Estimate" OnClick="lnkEstimate_Click">Add New</asp:LinkButton>
                                        </div>--%>
                                        <div class="btnlinks">
                                            <asp:CheckBox ID="chkClosed" runat="server" CssClass="css-checkbox" Text="Closed" />
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
                                            <asp:Label ID="lblOpportunityText" Text="Opportunity# " runat="server"></asp:Label>
                                            <asp:Label ID="lblOpportunityID" runat="server"></asp:Label>
                                            <asp:Label ID="lblOppNameLabel" runat="server"></asp:Label>
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
                                    <li runat="server" id="liOpportunityInformation"><a href="#accrdoppInfo">Opportunity Information</a></li>
                                    <li runat="server" id="liAdditionalInformation"><a href="#accrdadditionalInfo">Additional Information</a></li>
                                    <li runat="server" id="liOpenTasks"><a href="#accrdopentask">Open Tasks</a></li>
                                    <li runat="server" id="liEstimatesLinked"><a href="#accrdestimateslinked">Estimates</a></li>
                                    <li runat="server" id="liTaskHistory"><a href="#accrdtaskhistory">Task History</a></li>
                                    <li runat="server" id="liContacts"><a href="#accrdcontacts">Contacts</a></li>
                                    <li runat="server" id="liEmails"><a href="#accrdemail">Emails</a></li>
                                    <li runat="server" id="liNotes"><a href="#accrdnotes">Notes & Attachments</a></li>
                                     <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                    <li runat="server" id="liSystemInfo"><a href="#accrdsystem">System Info</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlNext" runat="server">
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" OnClick="lnkFirst_Click" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-double-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" OnClick="lnkPrevious_Click" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" OnClick="lnkNext_Click" CausesValidation="False">
                                                        <i class="fa fa-angle-right"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" OnClick="lnkLast_Click" CssClass="icon-last" CausesValidation="False">
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

        <a id="maillink" href="#Div6" class="maillink-css">
            <div id="dvmailct" class=" dvmail-css transparent roundCorner shadow">
            </div>
        </a>
          
        <asp:Button ID="btnFillTasks" runat="server" Text="Button" CausesValidation="false"
            Style="display: none " OnClick="btnFillTasks_Click"  />
          


        <asp:Panel ID="pnlCustomer" runat="server" Visible="false" Width="800px">
            <div class="row">
                <div>
                    You are about to close this Opportunity and create a new Location. Please select
                                                an existing customer or leave the field blank and this will create a new Customer.
                </div>
                <div style="clear: both;">
                    <uc1:uc_CustomerSearch ID="uc_CustomerSearch1" runat="server" />
                </div>
            </div>
        </asp:Panel>
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li runat="server" id="adOpportunityInformation">
                            <div id="accrdoppInfo" class="collapsible-header accrd active accordian-text-custom ">
                                <i class="mdi-image-style"></i>Opportunity Information
                                          <asp:Label ID="lblType" runat="server" onfocus="this.blur();"></asp:Label>
                            </div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompanyName" runat="server" ControlToValidate="txtCompanyName"
                                                            Display="None" ErrorMessage="Company Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="ValidatorCalloutExtenderCompanyName" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidatorCompanyName">
                                                            </asp:ValidatorCalloutExtender>
                                                         
                                                         <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ChkLoc"
                                                            ControlToValidate="txtCompanyName" Display="None" ErrorMessage="Please select the location"
                                                            SetFocusOnError="True"></asp:CustomValidator>
                                                        <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblCompanyName" AssociatedControlID="txtCompanyName">Customer Name</asp:Label>
                                                    </div>
                                                </div>
                                                    <div class="srchclr btnlinksicon rowbtn">
                                                            <asp:HyperLink for="txtCompanyName" ID="lnkCustomerID" Visible="true" Target="_blank" runat="server"><i class="mdi-social-people" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                        </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtName"
                                                            Display="None" ErrorMessage="Location Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidator40_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator40">
                                                            </asp:ValidatorCalloutExtender>
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ChkLoc"
                                                            ControlToValidate="txtName" Display="None" ErrorMessage="Please select the location"
                                                            SetFocusOnError="True"></asp:CustomValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                            PopupPosition="BottomLeft" TargetControlID="CustomValidator1">
                                                        </asp:ValidatorCalloutExtender>
                                                         <asp:HiddenField ID="hdnLocId" runat="server" />
                                                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblName" AssociatedControlID="txtName">Location Name</asp:Label>
                                                    </div>
                                                </div>
                                                      <div class="srchclr btnlinksicon rowbtn">
                                                            <asp:HyperLink for="txtName" ID="lnkLocationID" Visible="true" Target="_blank" runat="server"><i class="mdi-communication-location-on" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                        </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server"
                                                            ControlToValidate="txtOppName" Display="None" ErrorMessage="Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidator41_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator41">
                                                            </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtOppName" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblOppName" AssociatedControlID="txtOppName">Opportunity Name</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Status</label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlStatus" runat="server"
                                                            ControlToValidate="ddlStatus" Display="None" ErrorMessage="Status Required"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidatorddlStatus_ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidatorddlStatus">
                                                            </asp:ValidatorCalloutExtender>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default" onchange="SelectStatus();">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12" id="CloseDateDiv" runat="server" style="display: none">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCloseDate" CssClass="datepicker_mom" runat="server"></asp:TextBox>
                                                        <%-- <asp:CalendarExtender ID="txtCloseDate_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtCloseDate">
                                                                </asp:CalendarExtender>--%>
                                                        <asp:Label runat="server" ID="lblCloseDate" AssociatedControlID="txtCloseDate">Close Date</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label" id="lblStage" runat="server">Stage</label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorddltage" runat="server"
                                                            ControlToValidate="ddlStage" Display="None" ErrorMessage="Stage Required"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidatorddltage_ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidatorddltage">
                                                            </asp:ValidatorCalloutExtender>
                                                        <asp:DropDownList ID="ddlStage" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlStage_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Opp Source</label>
                                                        <asp:DropDownList ID="ddlSource" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Probability</label>
                                                        <asp:DropDownList ID="ddlProbab" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Value="-1">Select</asp:ListItem>
                                                            <asp:ListItem Value="0">Excellent</asp:ListItem>
                                                            <asp:ListItem Value="1">Very Good</asp:ListItem>
                                                            <asp:ListItem Value="2">Good</asp:ListItem>
                                                            <asp:ListItem Value="3">Average</asp:ListItem>
                                                            <asp:ListItem Value="4">Poor</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">
                                                            <asp:Label ID="lblProduct" runat="server" Text="Product"></asp:Label></label>
                                                        <asp:DropDownList ID="ddlProduct" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">
                                                            <asp:Label ID="lblBusinessType" runat="server" Text="Business Type"></asp:Label></label>
                                                        <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ControlToValidate="txtAmount"
                                                            Display="None" ErrorMessage="Amount Required" SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidator43_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator43">
                                                            </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblAmount" AssociatedControlID="txtAmount">Amount</asp:Label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Assigned To</label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server"
                                                            ControlToValidate="ddlAssigned" Display="None" ErrorMessage="Owner Required"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidator44_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator44">
                                                            </asp:ValidatorCalloutExtender>
                                                        <asp:DropDownList ID="ddlAssigned" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">
                                                            <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label></label>
                                                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" ID="lblRemarks" AssociatedControlID="txtRemarks">Remarks</asp:Label>
                                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="materialize-textarea" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                    <div class="row">
                                                        <asp:Label runat="server" ID="lblCompany" AssociatedControlID="txtCompany">Company</asp:Label>
                                                        <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <asp:Panel ID="pnlTicket" runat="server" Visible="false">
                                                        <div class="row">
                                                            <asp:Image ID="imgDoc" runat="server" Width="16px" ToolTip="Documents" ImageUrl="images/Document.png" /><span runat="server" id="ticketspan">Ticket #</span>
                                                            <a runat="server" id="ticketurl" onclick='return EditTicketClick(this)' target="_self"></a>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>

                                </div>
                            </div>
                        </li>
                        <li runat="server" id="adAdditionalInformation">
                            <div id="accrdadditionalInfo" class="collapsible-header accrd accordian-text-custom "><i class="mdi-action-info"></i>Additional Information</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtNextStep" runat="server" Style="height: 86px;" CssClass="textarea-border"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblNextStep" AssociatedControlID="txtNextStep" CssClass="txtbrdlbl">Next Step</asp:Label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section9">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" ID="lblDesc" AssociatedControlID="txtDesc" CssClass="txtbrdlbl">Desc</asp:Label>
                                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="materialize-textarea textarea-border" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="cf"></div>
                                    </div>

                                </div>
                            </div>
                        </li>
                        <li runat="server" id="adOpenTasks">
                            <div id="accrdopentask" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-assignment"></i>Open Tasks</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="HyperLink2" runat="server" ToolTip="Add New" NavigateUrl="~/AddTask.aspx" Target="_self">Add</asp:HyperLink>
                                            </div>
                                        </div>
                                        <div class="grid_container">
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <telerik:RadCodeBlock ID="RadCodeBlock_OpenTasks" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoad() {
                                                            var grid = $find("<%= RadGrid_OpenTasks.ClientID %>");
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
                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_OpenTasks" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Opp" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_OpenTasks" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                        OnNeedDataSource="RadGrid_OpenTasks_NeedDataSource" PagerStyle-AlwaysVisible="true"
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

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="Contact" HeaderText="Contact Name" SortExpression="Contact"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn SortExpression="Subject" HeaderText="Subject" DataField="Subject" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="lnkSubject" NavigateUrl='<%# "addtask.aspx?uid=" + Eval("id") %>'
                                                                            Target="_self" runat="server" Text='<%# Eval("Subject") %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="duedate" HeaderText="Due Date/Date Done" HeaderStyle-Width="150" DataField="duedate" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDuedate" runat="server" Style='<%#  Eval("statusid").ToString()!="1" ?( string.Format("color:{0}",Convert.ToDateTime( Eval("duedate") )<= System.DateTime.Now ? "RED": "BLACK")) : "Black" %>'
                                                                            Text='<%# (String.IsNullOrEmpty(Eval("duedate").ToString())) ? "No Date Available" : Eval("duedate", "{0:MM/dd/yyyy h:mm tt}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="days" HeaderText="# Days" HeaderStyle-Width="80" DataField="days" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="Remarks" HeaderText="Desc" HeaderStyle-Width="200"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Remarks"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="result" HeaderText="Resolution" HeaderStyle-Width="200"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="result"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="fUser" HeaderText="Assigned to" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fUser"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="status" HeaderText="Status" HeaderStyle-Width="120"
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
                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="screen" HeaderText="Screen" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="screen"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn SortExpression="CreatedDate" HeaderText="Created Date" HeaderStyle-Width="150" DataField="CreatedDate" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCreatedDate" runat="server" 
                                                                            Text='<%# (String.IsNullOrEmpty(Eval("CreatedDate").ToString())) ? "" : Eval("CreatedDate", "{0:MM/dd/yyyy h:mm tt}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="ref" HeaderText="Ref ID" HeaderStyle-Width="100"
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
                        <li runat="server" id="adEstimatesLinked">
                            <div id="accrdestimateslinked" class="collapsible-header accrd accordian-text-custom"><i class="mdi-maps-map"></i>Estimates</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="lnkAddNewEstimate" runat="server" ToolTip="Add New Estimate" NavigateUrl="~/addestimate.aspx" Target="_self">Add</asp:HyperLink>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkEditEstimate" runat="server" OnClick="lnkEditEstimate_Click" CausesValidation="False" OnClientClick="return SingleSelectValidation();">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkCopyEstimate" runat="server" OnClick="lnkCopyEstimate_Click" CausesValidation="False" OnClientClick="return SingleSelectValidation();">Copy</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkDeleteEstimate" runat="server" OnClick="lnkDeleteEstimate_Click" OnClientClick="return DeleteEstConfirm();" CausesValidation="False">Delete</asp:LinkButton>
                                            </div>

                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkExcelEstimate" runat="server" OnClick="lnkExcelEstimate_Click">Export to Excel</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkProjectEstimate" runat="server" OnClick="lnkProjectEstimate_Click" CausesValidation="False" OnClientClick="return ConvertEstConfirm();">Convert to Project</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <a class="dropdown-button" id="lnkReportEstimate" runat="server" data-beloworigin="true" href="#!" data-activates="dropdown1">Reports
                                                </a>
                                            </div>
                                            <ul id="dropdown1" class="dropdown-content">
                                                <li>
                                                    <asp:LinkButton ID="lnkExportEstimateProfile" runat="server" OnClick="lnkExportEstimateProfile_Click"
                                                        CausesValidation="true" ValidationGroup="search">Estimate Profile</asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="grid_container">
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <telerik:RadCodeBlock ID="RadCodeBlock_EstimatesLinked" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoad() {
                                                            var grid = $find("<%= RadAjaxPanel_EstimatesLinked.ClientID %>");
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
                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_EstimatesLinked" runat="server" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_EstimatesLinked" CssClass="RadGrid_EstimatesLinked" AllowFilteringByColumn="true" ShowFooter="True"
                                                        ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%"
                                                        OnNeedDataSource="RadGrid_EstimatesLinked_NeedDataSource" OnItemCreated="RadGrid_EstimatesLinked_ItemCreated" 
                                                        OnPreRender="RadGrid_EstimatesLinked_PreRender" OnExcelMLExportRowCreated="RadGrid_EstimatesLinked_ExcelMLExportRowCreated"
                                                        AllowMultiRowSelection="true"
                                                        emptydatatext="No estimate to display" howFooter="True" showheaderwhenempty="true"
                                                        ClientSettings-Scrolling-UseStaticHeaders="true"
                                                        ClientSettings-Scrolling-ScrollHeight="300px"
                                                        ClientSettings-Scrolling-AllowScroll="true"
                                                        >
                                                        <CommandItemStyle />
                                                        <GroupingSettings CaseSensitive="false" />
                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="EstimateNo">
                                                            <Columns>
                                                                <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                                </telerik:GridClientSelectColumn>
                                                                <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("EstimateNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="EstimateNo" SortExpression="EstimateNo" AutoPostBackOnFilter="true" DataType="System.String"
                                                                    CurrentFilterFunction="Contains" HeaderText="Estimate#" HeaderStyle-Width="90" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="lnkEstimateNo" runat="server" Text='<%# Bind("EstimateNo") %>' 
                                                                            NavigateUrl='<%# "addEstimate.aspx?uid=" + Eval("EstimateNo") + "&opp=" + Request.QueryString["uid"].ToString() + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl)%>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                                    </FooterTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="Category" HeaderText="Category" SortExpression="Category"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="120">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Contact" HeaderText="Contact" SortExpression="Contact" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                    ShowFilterIcon="false" HeaderStyle-Width="180">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Description" HeaderText="Description" SortExpression="Description" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                    ShowFilterIcon="false" HeaderStyle-Width="200">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn DataField="fDate" SortExpression="fDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                    CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("fDate").ToString())) ? "" : Eval("fDate", "{0:M/d/yyyy}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="ProjectNo" HeaderText="Project#" SortExpression="ProjectNo" DataType="System.String"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                    <ItemTemplate>
                                                                        <a href="addproject.aspx?uid=<%# Eval("ProjectNo") %>" style="color: #3175af; text-decoration: none;"><%# Eval("ProjectNo") %></a>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="status" HeaderText="Status" SortExpression="status" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                    ShowFilterIcon="false" HeaderStyle-Width="80">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn HeaderStyle-Width="120" DataField="EstimatePrice" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="EstimatePrice" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Bid Price" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEstimatePrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EstimatePrice", "{0:c}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderStyle-Width="100" DataField="QuotedPrice" FooterAggregateFormatString="{0:c}"  Aggregate="Sum" SortExpression="QuotedPrice" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Final Bid Price" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "QuotedPrice", "{0:c}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="Discounted" HeaderText="Discounted" SortExpression="Discounted" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                    ShowFilterIcon="false" HeaderStyle-Width="130">
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
                                                <asp:HyperLink ID="HyperLink1" runat="server" ToolTip="Add New" NavigateUrl="~/AddTask.aspx"
                                                    Target="_self">Add</asp:HyperLink>
                                            </div>
                                        </div>
                                        <div class="grid_container">
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <telerik:RadCodeBlock ID="RadCodeBlock_TaskHistory" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoad() {
                                                            var grid = $find("<%= RadGrid_TaskHistory.ClientID %>");
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
                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_TaskHistory" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Opp" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_TaskHistory" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                        OnNeedDataSource="RadGrid_TaskHistory_NeedDataSource" PagerStyle-AlwaysVisible="true"
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

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="Contact" HeaderText="Contact Name" SortExpression="Contact"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn SortExpression="Subject" HeaderText="Subject" DataField="Subject" HeaderStyle-Width="200" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="lnkSubject" NavigateUrl='<%# "addtask.aspx?uid=" + Eval("id") + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) %>'
                                                                            Target="_self" runat="server" Text='<%# Eval("Subject") %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="duedate" HeaderText="Due Date/Date Done" HeaderStyle-Width="150" DataField="duedate" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDuedate" runat="server" Style='<%#  Eval("statusid").ToString()!="1" ?( string.Format("color:{0}",Convert.ToDateTime( Eval("duedate") )<= System.DateTime.Now ? "RED": "BLACK")) : "Black" %>'
                                                                            Text='<%# (String.IsNullOrEmpty(Eval("duedate").ToString())) ? "No Date Available" : Eval("duedate", "{0:MM/dd/yyyy h:mm tt}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="days" HeaderText="# Days" HeaderStyle-Width="80" DataField="days" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="Remarks" HeaderText="Desc" HeaderStyle-Width="200"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Remarks"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="result" HeaderText="Resolution" HeaderStyle-Width="200"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="result"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="fUser" HeaderText="Assigned to" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fUser"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="status" HeaderText="Status" HeaderStyle-Width="120"
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
                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="screen" HeaderText="Screen" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="screen"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="ref" HeaderText="Ref ID" HeaderStyle-Width="100"
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
                        <li runat="server" id="adContacts">
                            <div id="accrdcontacts" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-account-circle"></i>Contacts</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <asp:Panel ID="pnlContactButtons" Visible="false" runat="server">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAddnew" runat="server" OnClick="lnkAddnew_Click" CausesValidation="False">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click">Delete</asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="grid_container">
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <div class="table-scrollable">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock_Opp" runat="server">
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Contacts" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Opp" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
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
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
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

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="Name" HeaderText="Name" SortExpression="Name"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>


                                                                    <%-- <telerik:GridBoundColumn FilterDelay="5" DataField="Title" HeaderText="Title" HeaderStyle-Width="140"
                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Title"
                                                                            ShowFilterIcon="false">
                                                                        </telerik:GridBoundColumn>--%>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="Phone" HeaderText="Phone" HeaderStyle-Width="140"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Phone"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="Fax" HeaderText="Fax" HeaderStyle-Width="140"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Fax"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="Cell" HeaderText="Cell" HeaderStyle-Width="140"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Cell"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderText="Email" SortExpression="Email" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <a href='<%# Request.QueryString["uid"] != null ? "email.aspx?to=" + Eval("Email") + "&rol="+ hdnId.Value + "&op=" + Request.QueryString["uid"].ToString() : "email.aspx?to=" + Eval("Email")+ "&rol="+ hdnId.Value %>'
                                                                                target="_self">
                                                                                <asp:Label ID="lblEmail" runat="server"><%#Eval("Email")%></asp:Label></a>
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
                        <li runat="server" id="adEmails">
                            <div id="accrdemail" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-inbox"></i>Emails</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>--%>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkAllMail" runat="server" CausesValidation="False" OnClick="lnkAllMail_Click">Show All</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkSpecific" runat="server" CausesValidation="False" OnClick="lnkSpecific_Click">Specific</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:HyperLink ID="lnkNewEmail" Target="_self" runat="server" NavigateUrl="email.aspx">New</asp:HyperLink>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkRefreshMails" runat="server" CausesValidation="False" OnClick="lnkRefreshMails_Click">Refresh</asp:LinkButton>
                                                        <asp:HiddenField ID="hdnMailct" runat="server" />
                                                        <asp:HiddenField ID="hdnMailType" runat="server" />
                                                    </div>
                                                <%--</ContentTemplate>
                                            </asp:UpdatePanel>--%>
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

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Mail" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Opp" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
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
                                                                        <asp:HyperLink ID="lnkSub" NavigateUrl='<%# Request.QueryString["uid"] != null ? "email.aspx?aid=" + Eval("guid") + "&op=" + Request.QueryString["uid"].ToString() +"&rol="+ hdnId.Value :  "email.aspx?aid=" + Eval("guid") +"&rol="+ hdnId.Value %>'
                                                                            Target="_self" runat="server" Text='<%# Eval("subject") %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="from" HeaderText="From" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="from"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="to" HeaderText="To" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="to"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="SentDate" HeaderText="Date Sent" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="SentDate"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="recDate" HeaderText="Rec. Date" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="recDate"
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
                        <li runat="server" id="adNotes">
                            <div id="accrdnotes" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Notes & Attachments</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12 m12 l12">
                                                <div class="row">
                                                    <asp:FileUpload ID="FileUpload1" runat="server" class="dropify" onchange="AddDocumentClick(this);" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkUploadDoc" runat="server" CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                    Style="display: none">Upload</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks m-b-5">
                                                <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click">Delete</asp:LinkButton>
                                            </div>
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

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" PostBackControls="lblName" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Opp" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
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

                                                                <telerik:GridTemplateColumn SortExpression="subject" Visible="false" HeaderText="Subject" DataField="subject" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSub" runat="server" Text='<%# Eval("subject") %>'></asp:Label>
                                                                        <asp:Label ID="lblBody" runat="server" Text='<%# Eval("body") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="filename" HeaderText="File Name" DataField="filename" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lblName" runat="server" CausesValidation="false" CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                            OnClick="lblName_Click" Text='<%# Eval("filename") %>'> </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="doctype" HeaderText="File Type" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="doctype"
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
                         <li id="tbLogs" runat="server" style="display: none">
                            <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row m-b-0" >
                                                <div class="RadGrid RadGrid_Material">
                                                    
                                                    
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
                                                <div class="table-top">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td class="w-70" >&nbsp;
                                                                </td>
                                                                <td class="w-70" >Created By:
                                                                </td>
                                                                <td class="width-r30" >
                                                                    <span style="font-weight: bold;">
                                                                        <asp:Label ID="lblCreate" runat="server" Font-Bold="True"></asp:Label>
                                                                    </span>
                                                                </td>
                                                                <td class="w-100" >Last Updated By:
                                                                </td>
                                                                <td class="width-r30" >
                                                                    <span style="font-weight: bold;">
                                                                        <asp:Label ID="lblUpdate" runat="server" Font-Bold="True"></asp:Label>
                                                                    </span>
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



    <asp:HiddenField ID="hdnUID" runat="server" />
    <asp:HiddenField ID="hdnId" runat="server" />
    <asp:HiddenField ID="hdnOwnerID" runat="server" />
    <asp:HiddenField ID="hdnProsID" runat="server" />
 
    <input id="hdnCustId" runat="server" type="hidden" />
    <asp:HiddenField runat="server" ID="hdnEditeTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnCon" />

    <telerik:RadWindowManager ID="RadWindowManagerAddOpp" runat="server">
        <Windows>
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtTitle" runat="server" ControlToValidate="txtTitle"
                                            Display="None" ErrorMessage="Title  Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtendertxtTitle"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidatortxtTitle">
                                        </asp:ValidatorCalloutExtender>
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
                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RegularExpressionValidator1">
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
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <script defer src="https://use.fontawesome.com/releases/v5.0.10/js/all.js"></script>
    <script type="text/javascript">


        $(document).ready(function () {

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

        });

        function AddDocumentClick(hyperlink) {
            document.getElementById('<%= lnkUploadDoc.ClientID %>').click();
        }


    </script>

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

            $(".dropdown-content.select-dropdown li").on("click", function () {
                var that = this;
                setTimeout(function () {
                    if ($(that).parent().hasClass('active')) {
                        $(that).parent().removeClass('active');
                        $(that).parent().hide();
                    }
                }, 100);
            });

            ///////////// Quick Codes //////////////
            $("#<%=txtRemarks.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRemarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
        });

        function pageLoad(sender, args) {
            Materialize.updateTextFields();
        }
    </script>

    <script type="text/javascript">
        //Contact Popup
        $("[id*=txtContPhone]").mask("(999) 999-9999? Ext 99999");
        $("[id*=txtContPhone]").bind('paste', function () { $(this).val(''); });
        $("[id*=txtContCell]").mask("(999) 999-9999");
        $("[id*=txtContFax]").mask("(999) 999-9999");


        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {
            $("[id*=txtContPhone]").mask("(999) 999-9999? Ext 99999");
            $("[id*=txtContPhone]").bind('paste', function () { $(this).val(''); });
            $("[id*=txtContCell]").mask("(999) 999-9999");
            $("[id*=txtContFax]").mask("(999) 999-9999");
        });


    
    </script>



</asp:Content>
