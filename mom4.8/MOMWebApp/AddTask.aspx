<%@ Page Title="Add Task || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddTask" ValidateRequest="false" Codebehind="AddTask.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="Scripts/Timepicker/jquery.timepicker.js"></script>

    <link rel="stylesheet" href="Scripts/Timepicker/jquery.timepicker.css" />

    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <style>
        .highlight {
            background-color: Yellow;
        }

        .highlighted {
            background-color: Yellow;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {


            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
                this.isProspect = null;
            }




            //############# Customer Name autopopulate #############
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
                    var currolId = $("#<%=hdnId.ClientID%>").val();
                    if (currolId != ui.item.rolid) {
                        $("#<%=hdnId.ClientID%>").val(ui.item.rolid);
                        $("#<%=txtContact.ClientID%>").val('');
                        $("#<%=txtContactPhone.ClientID%>").val('');
                        $("#<%=txtContactEmail.ClientID%>").val('');
                    }
                    if (ui.item.prospect == 1) {
                        <%--  $("#<%=txtName.ClientID%>").val(ui.item.prospectloclabel);--%>
                        $("#<%=txtName.ClientID%>").val(ui.item.lName);
                        $("#<%=hdnOwnerID.ClientID%>").val(ui.item.value);
                        $("#<%=hdnCustId.ClientID%>").val("0");
                        //$("#<%=lblType.ClientID%>").val("Lead");
                        $("#<%=lblType.ClientID%>").html("Lead");
                        
                    }
                    else {
                        //$("#<%=hdnOwnerID.ClientID%>").val(ui.item.value);
                        $("#<%=hdnOwnerID.ClientID%>").val("0");
                        $("#<%=hdnCustId.ClientID%>").val(ui.item.value);
                        //$("#<%=lblType.ClientID%>").val("Existing");
                        $("#<%=lblType.ClientID%>").html("Existing");
                        $("#<%=txtName.ClientID%>").val(ui.item.lName);
                    }

                    document.getElementById('<%=btnFillTasks.ClientID%>').click();
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

                    $("#<%=txtCustomer.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                //_renderItem = function (ul, item) {

                var result_item = item.label;
                var result_desc = item.desc;
                var result_type = item.type;
                var result_Prospect = item.prospect;
                var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                result_item = result_item.toString().replace(x, function (FullMatch, n) {
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




            //############ Location Name autopopulate

            $("#<%=txtName.ClientID%>").autocomplete({
                
                source: function (request, response) {
                    debugger
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    dtaaa.custID = 0;

                    if ($('#<%=hdnOwnerID.ClientID%>').val() != '' && $('#<%=hdnOwnerID.ClientID%>').val() != '0') {
                        dtaaa.isProspect = 1;
                        dtaaa.custID = $('#<%=hdnOwnerID.ClientID%>').val();
                    } else {
                        dtaaa.isProspect = 0;
                        if (document.getElementById('<%=txtCustomer.ClientID%>').value != '') {
                            if (document.getElementById('<%=hdnCustId.ClientID%>').value != '') {
                                dtaaa.custID = document.getElementById('<%=hdnCustId.ClientID%>').value;
                            }
                        }
                    }

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
                        $("#<%=txtName.ClientID%>").val("");
                    }
                    else {
                        var currolId = $("#<%=hdnId.ClientID%>").val();
                        if (currolId != ui.item.rolid) {
                            $("#<%=hdnId.ClientID%>").val(ui.item.rolid);
                            $("#<%=txtContact.ClientID%>").val('');
                            $("#<%=txtContactPhone.ClientID%>").val('');
                            $("#<%=txtContactEmail.ClientID%>").val('');
                        }
                        if (ui.item.ProspectID == 0) {
                            //$("#<%=hdnId.ClientID%>").val(ui.item.rolid);
                            $("#<%=txtName.ClientID%>").val(ui.item.label);
                            $("#<%=hdnOwnerID.ClientID%>").val("0");
                            //$("#<%=lblType.ClientID%>").val("Existing");
                            $("#<%=lblType.ClientID%>").html("Existing");
                            $("#<%=txtCustomer.ClientID%>").val(ui.item.CompanyName);
                            document.getElementById('<%=btnFillTasks.ClientID%>').click();
                        }
                        else {

                            //$("#<%=hdnId.ClientID%>").val(ui.item.rolid);
                            $("#<%=txtName.ClientID%>").val(ui.item.label);
                            $("#<%=hdnOwnerID.ClientID%>").val(ui.item.ProspectID);
                            //$("#<%=hdnOwnerID.ClientID%>").val(ui.item.value);
                            //$("#<%=lblType.ClientID%>").val("Lead");
                            $("#<%=lblType.ClientID%>").html("Lead");
                            $("#<%=txtCustomer.ClientID%>").val(ui.item.CompanyName);
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
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                //_renderItem = function (ul, item) {

                var result_item = item.label;
                var result_desc = item.desc;
                var result_type = item.type;
                var result_Prospect = item.ProspectID;
                var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                result_item = result_item.toString().replace(x, function (FullMatch, n) {
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





            //############ Location Name autopopulate

            $("#<%=txtContact.ClientID%>").autocomplete({
                //open: function (e, ui) {
                //    /* create the scrollbar each time autocomplete menu opens/updates */
                //    $(".ui-autocomplete").mCustomScrollbar({

                //        setHeight: 182,
                //        theme: "dark-3",
                //        keyboard: {
                //            enable: true,
                //            scrollType: "stepless"
                //        },
                //        autoExpandScrollbar: true
                //    });
                //},
                //response: function (e, ui) {
                //    /* destroy the scrollbar after each search completes, before the menu is shown */
                //    $(".ui-autocomplete").mCustomScrollbar("destroy");
                //},
                source: function (request, response) {

                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    dtaaa.RoleID = 0;
                    if (document.getElementById('<%=txtName.ClientID%>').value != '') {
                        if (document.getElementById('<%=hdnId.ClientID%>').value != '') {
                            dtaaa.RoleID = document.getElementById('<%=hdnId.ClientID%>').value;
                        }
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetContactsSearchbyRolid",
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
                    if (str != "No Record Found!") {
                        $("#<%=txtContact.ClientID%>").val(str);
                        $("#<%=txtContactPhone.ClientID%>").val(ui.item.Phone);
                        $("#<%=txtContactEmail.ClientID%>").val(ui.item.Email);
                    }
                    Materialize.updateTextFields();
                    return false;
                },
                //change: function (event, ui) {
                //    if (!ui.item) {
                //        $(this).val("");
                //        return false;
                //    }
                //},
                focus: function (event, ui) {
                    var str = ui.item.label;
                    if (str != "No Record Found!") {
                        $("#<%=txtContact.ClientID%>").val(ui.item.label);
                    }
                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                //._renderItem = function (ul, item) {

                var result_item = item.label;
                var result_desc = item.desc;
                var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                result_item = result_item.toString().replace(x, function (FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });
                if (result_desc != null) {
                    result_desc = result_desc.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                }
                var color = 'black';

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a style='color:" + color + ";'>" + result_item + " <span style='color:Gray;'> " + result_desc + "</span></a>")
                    .appendTo(ul);
            };



            setInterval(serviceCall, 600000);
            //  AutoCompleteText('CustomerAuto.asmx/getTaskContacts', '<%= txtName.ClientID %>', '<%= hdnId.ClientID %>', '<%= btnFillTasks.ClientID %>', '<%= lblType.ClientID %>', null)

            $("#<%= txtName.ClientID %>").keyup(function (e) {
                var hdnId = document.getElementById('<%= hdnId.ClientID %>');
                if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                    hdnId.value = '';
                }
                if (e.value == '') {
                    hdnId.value = '';
                }
            });

            $(document).ready(function () {

                $('input.timepicker').timepicker({
                    dropdown: false
                });
                $('input.timepicker').on('click', function () {
                    if ($('input.timepicker').val() == "") {
                        $('input.timepicker').timepicker('setTime', new Date());
                        $(this).select();
                    }
                    else { $(this).select(); }
                });
                $('input.timepicker').on('focus', function () {

                    $(this).select();
                });
                if ($(window).width() > 767) {
                 <%--   $('#<%=txtDesc.ClientID%>').focus(function () {
                        $(this).animate({
                            //right: "+=0",
                            width: '520px',
                            height: '75px'
                        }, 500, function () {
                            // Animation complete.
                        });
                    });

                    $('#<%=txtDesc.ClientID%>').blur(function () {
                        $(this).animate({
                            width: '100%',
                            height: '75px'
                        }, 500, function () {
                            // Animation complete.
                        });
                    });--%>
                    $('#<%=txtResol.ClientID%>').focus(function () {
                        $(this).animate({
                            //right: "+=0",
                            width: '520px',
                            height: '75px'
                        }, 500, function () {
                            // Animation complete.
                        });
                    });

                    $('#<%=txtResol.ClientID%>').blur(function () {
                        $(this).animate({
                            width: '100%',
                            height: '75px'
                        }, 500, function () {
                            // Animation complete.
                        });
                    });
                }
            });

        });

        function ChkCustomer(sender, args) {
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
            if (rol.value != '') {
                $.ajax({
                    type: "POST",
                    url: 'AddProspect.aspx/CheckEmail',
                    data: '{"rol":"' + rol.value + '","type":"-1","uid":"0"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        //                    alert(msg.d);
                        MailCount(msg.d)
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

        function DeleteContactClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteContact.ClientID%>').value;
            if (IsDelete == "Y") {
                return SelectedRowDelete('<%= RadGrid_Contacts.ClientID%>', 'contact');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Task" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnFillTasks">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OpenTasks" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_TaskHistory" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                    <telerik:AjaxUpdatedControl ControlID="HyperLink1" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                    <telerik:AjaxUpdatedControl ControlID="HyperLink2" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                    <telerik:AjaxUpdatedControl ControlID="lnkNewEmail" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                    <telerik:AjaxUpdatedControl ControlID="lnkAddnewContact" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                    <telerik:AjaxUpdatedControl ControlID="btnEditContact" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                    <telerik:AjaxUpdatedControl ControlID="btnDeleteContact" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkAddnewContact">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnEditContact">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkContactSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnDeleteContact">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--<telerik:AjaxSetting AjaxControlID="lnkSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdn" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Task" runat="server">
    </telerik:RadAjaxLoadingPanel>


    <div>
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-maps-place"></i>&nbsp;
                                         <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Task</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkSave" runat="server" OnClick="lnkSave_Click">Save</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:CheckBox ID="chkFollowUp" runat="server" CssClass="css-checkbox" Text="Follow-Up" Visible="false" />
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="False" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                                    <li runat="server" id="liTaskInformation"><a href="#accrdtaskInfo">Task Information</a></li>
                                    <li runat="server" id="liContacts"><a href="#accrdcontacts">Contacts</a></li>
                                    <li runat="server" id="liOpenTask"><a href="#accrdopentask">Open Tasks</a></li>
                                    <li runat="server" id="liTaskHistory"><a href="#accrdtaskhistory">Task History</a></li>
                                    <li runat="server" id="liEmails"><a href="#accrdemails">Emails</a></li>
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

        <a id="maillink" href="#Div5" class="maillink-css" style="display: none; position: fixed; top: 0; left: 615px;">
            <div id="dvmailct"  style="width: 100%; height: 100%; vertical-align: middle; text-align: center; padding: 5px; background-color: Black"
                class="transparent roundCorner shadow">
            </div>
        </a>
        <asp:Button ID="btnFillTasks" runat="server" Text="Button" CausesValidation="false"
            Style="display: none" OnClick="btnFillTasks_Click" />


        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li runat="server" id="adTaskInformation">
                            <div id="accrdtaskInfo" class="collapsible-header accrd active accordian-text-custom ">
                                <i class="mdi-av-my-library-books"></i>Task Information
                               <%-- <asp:TextBox ID="lblType" runat="server" CssClass="texttransparent" onfocus="this.blur();"
                                    Style="color: Gray; font-style: italic; width: 60px">
                                </asp:TextBox>--%>
                                <asp:Label ID="lblType" runat="server"></asp:Label>
                            </div>
                            <div class="collapsible-body" id="firstTab">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server"
                                                            ControlToValidate="txtCustomer" Display="None"
                                                            ErrorMessage="Customer Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator47_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator47">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtCustomer" runat="server" MaxLength="75"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblCustomer" AssociatedControlID="txtCustomer">Customer Name</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtName"
                                                            Display="None" ErrorMessage="Location Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidator40_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator40">
                                                            </asp:ValidatorCalloutExtender>
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ChkCustomer"
                                                            ControlToValidate="txtName" Display="None" ErrorMessage="Please select the Location from list"
                                                            SetFocusOnError="True"></asp:CustomValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                            PopupPosition="BottomLeft" TargetControlID="CustomValidator1">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblLocationName" AssociatedControlID="txtName">Location Name</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtContact" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblContact" AssociatedControlID="txtContact">Contact Name</asp:Label>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                            ControlToValidate="txtContactEmail" Display="None" ErrorMessage="Invalid Email"
                                                            SetFocusOnError="True"
                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtContactEmail" runat="server" CssClass="form-control" AutoCompleteType="Disabled"
                                                            MaxLength="99">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                            TargetControlID="txtContactEmail">
                                                        </asp:FilteredTextBoxExtender>
                                                        
                                                        <asp:Label runat="server" ID="lblContactEmail" AssociatedControlID="txtContactEmail">Contact Email Address</asp:Label>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtContactPhone" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblContactPhone" AssociatedControlID="txtContactPhone">Contact Phone No</asp:Label>
                                                    </div>
                                                </div>
                                                


                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Status</label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" CssClass="browser-default" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                            <asp:ListItem Value="0">Open</asp:ListItem>
                                                            <asp:ListItem Value="1">Completed</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server"
                                                            ControlToValidate="txtCallDt" Display="None" ErrorMessage="Date Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidator42_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator42">
                                                            </asp:ValidatorCalloutExtender>
                                                        <%--<asp:CalendarExtender ID="txtCallDt_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtCallDt">
                                                        </asp:CalendarExtender>--%>
                                                        <asp:TextBox ID="txtCallDt" CssClass="datepicker_mom" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblDueDate" AssociatedControlID="txtCallDt">Due Date</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCallTime" CssClass="timepicker" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblTime" AssociatedControlID="txtCallTime">Time</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtSubject"
                                                            Display="None" ErrorMessage="Subject Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidator41_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator41">
                                                            </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblSubject" AssociatedControlID="txtSubject">Subject</asp:Label>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Assigned To</label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server"
                                                            ControlToValidate="ddlAssigned" Display="None" ErrorMessage="User Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidator43_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator43">
                                                            </asp:ValidatorCalloutExtender>
                                                        <asp:DropDownList ID="ddlAssigned" runat="server" CssClass="browser-default" TabIndex="5"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtDuration" runat="server" onkeypress="return isDecimalKey(this,event)" ></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblDuration" AssociatedControlID="txtDuration">Duration(h)</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5"  style="margin-top: 15px; margin-bottom: 9px;">
                                                    <div class="row">
                                                        <asp:CheckBox ID="chkAlert" runat="server" Text="Email Alert" CssClass="css-checkbox"/>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                    <div class="row">
                                                        <asp:Label runat="server" ID="lblCompany" AssociatedControlID="txtCompany">Company</asp:Label>
                                                        <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Task Category</label>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                            ControlToValidate="ddlTaskCategory" Display="None" ErrorMessage="Task Category Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator1">
                                                        </asp:ValidatorCalloutExtender>--%>
                                                        <asp:DropDownList ID="ddlTaskCategory" runat="server" CssClass="browser-default" TabIndex="5"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" ID="lblDescription" AssociatedControlID="txtDesc">Task Description</asp:Label>
                                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="materialize-textarea" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>


                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" ID="lblResolution" AssociatedControlID="txtResol">Task Complete Notes</asp:Label>
                                                        <asp:TextBox ID="txtResol" runat="server" CssClass="materialize-textarea" TextMode="MultiLine" Enabled="False"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>

                                </div>
                            </div>
                        </li>
                        <li runat="server" id="adContacts">
                            <div id="accrdcontacts" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-account-circle"></i>Contacts</div>
                            <div class="collapsible-body" id="divContacts">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <asp:Panel ID="pnlContactButtons" runat="server">
                                                <div class="btnlinks">
                                                    <%--   <asp:LinkButton ID="lnkAddnew" runat="server" CausesValidation="False"
                                                    OnClick="lnkAddnew_Click">Add</asp:LinkButton>--%>
                                                    <asp:LinkButton ID="lnkAddnewContact" Visible="false" runat="server" CausesValidation="False" OnClick="lnkAddnewContact_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <%--   <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
                                                        OnClientClick="return EditContactClick(this);" OnClick="btnEdit_Click">Edit</asp:LinkButton>--%>
                                                    <asp:LinkButton ID="btnEditContact"  Visible="false" runat="server" CausesValidation="False"
                                                        OnClick="btnEditContact_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnDeleteContact" Visible="false" runat="server"
                                                        OnClientClick="return DeleteContactClick(this);"
                                                        CausesValidation="False"
                                                        OnClick="btnDeleteContact_Click">Delete</asp:LinkButton>
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
                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Contacts" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Task" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
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

                                                                <%--<telerik:GridBoundColumn DataField="Name" HeaderText="Name" SortExpression="Name"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
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
                                                                </telerik:GridBoundColumn>--%>
                                                                <telerik:GridTemplateColumn DataField="Name" HeaderText="Name" SortExpression="Name"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactName" Text='<%# Eval("Name")%>' runat="server"></asp:Label>
                                                                        <asp:HiddenField ID="hdnContactID" Value='<%# Bind("ContactId") %>' runat="server"/>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Title" HeaderText="Title" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Title"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactTitle" Text='<%# Eval("Title")%>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Phone" HeaderText="Phone" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Phone"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactPhone" Text='<%#Eval("Phone")%>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="Fax" HeaderText="Fax" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Fax"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactFax" Text='<%#Eval("Fax")%>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="Cell" HeaderText="Cell" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Cell"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactCell" Text='<%#Eval("Cell")%>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderStyle-Width="140" HeaderText="Email" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Email" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <a href='<%# "email.aspx?to=" + Eval("Email") +"&rol="+hdnId.Value %>' target="_self">
                                                                            <asp:Label ID="lblEmail" Text='<%#Eval("Email")%>' runat="server"></asp:Label></a>
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
                            <div class="collapsible-body" id="divOpenTask">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/AddTask.aspx" Target="_self">New</asp:HyperLink>
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
                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_OpenTasks" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Task" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_OpenTasks" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
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

                                                                <telerik:GridBoundColumn DataField="Contact" HeaderText="Contact Name" SortExpression="Contact"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn SortExpression="Subject" HeaderText="Subject" HeaderStyle-Width="200" DataField="Subject" ShowFilterIcon="false">
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

                                                                <telerik:GridTemplateColumn SortExpression="days" HeaderText="# Days" HeaderStyle-Width="80"  DataField="days" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="Remarks" HeaderText="Desc" HeaderStyle-Width="200"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Remarks"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="result" HeaderText="Resolution" HeaderStyle-Width="200"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="result"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="fUser" HeaderText="Assigned to" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fUser"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="status" HeaderText="Status" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="keyword" HeaderText="Keyword" HeaderStyle-Width="115"
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
                            <div class="collapsible-body" id="divTaskHistory">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/AddTask.aspx" Target="_self">New</asp:HyperLink>
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
                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_TaskHistory" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Task" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
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

                                                                <telerik:GridBoundColumn DataField="Contact" HeaderText="Contact Name" SortExpression="Contact"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn SortExpression="Subject" HeaderText="Subject" HeaderStyle-Width="200" DataField="Subject" ShowFilterIcon="false">
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

                                                                <telerik:GridBoundColumn DataField="Remarks" HeaderText="Desc" HeaderStyle-Width="200"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Remarks"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="result" HeaderText="Resolution" HeaderStyle-Width="200"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="result"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="fUser" HeaderText="Assigned to" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fUser"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="status" HeaderText="Status" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="keyword" HeaderText="Keyword" HeaderStyle-Width="115"
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
                        <li runat="server" id="adEmails">
                            <div id="accrdemails" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-inbox"></i>Emails</div>
                            <div class="collapsible-body" id="divEmails">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>--%>
                                                    <div class="btnlinks">
                                                        <asp:HyperLink ID="lnkNewEmail" Target="_self" runat="server">New</asp:HyperLink>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkRefreshMails" runat="server" CausesValidation="False" OnClick="lnkRefreshMails_Click">Refresh</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:HiddenField ID="hdnMailct" runat="server" />
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

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Mail" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Task" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
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
                                                                        <asp:HyperLink ID="lnkSub" NavigateUrl='<%# "email.aspx?aid=" + Eval("guid")  +"&rol="+hdnId.Value  %>'
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
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
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
                            <div class="collapsible-body" id="divSystemInfo">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12 m12 l12">
                                                <div style="width: 100%; visibility: visible; height: auto;">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 70px">&nbsp;
                                                                </td>
                                                                <td style="width: 70px">Created By:
                                                                </td>
                                                                <td style="width: 300px">
                                                                    <span style="font-weight: bold;">
                                                                        <asp:Label ID="lblCreate" runat="server" Font-Bold="True"></asp:Label></span>
                                                                </td>
                                                                <td style="width: 100px">Last Updated By:
                                                                </td>
                                                                <td style="width: 300px">
                                                                    <span style="font-weight: bold;">
                                                                        <asp:Label ID="lblUpdate" runat="server" Font-Bold="True"></asp:Label></span>
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

    <telerik:RadWindow ID="RadWindowContact" Skin="Material" VisibleTitlebar="true" Title="Add Contact" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
        runat="server" Modal="true" Width="800" Height="255">
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

    <asp:HiddenField ID="hdnId" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hdnOwnerID" runat="server" />
    <asp:HiddenField ID="hdnCustId" runat="server" />

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewContact" Value="Y" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <script defer src="https://use.fontawesome.com/releases/v5.0.10/js/all.js"></script>
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

            $('#<%=txtContactPhone.ClientID%>').mask("(999) 999-9999? Ext 99999");
            $('#<%=txtContactPhone.ClientID%>').bind('paste', function () { $(this).val(''); });
        });

        //////////////// To make textbox value decimal ///////////////////////////
        function isDecimalKey(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            if (charCode == 45) {
                return true;
            }

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

        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }
    </script>
</asp:Content>
