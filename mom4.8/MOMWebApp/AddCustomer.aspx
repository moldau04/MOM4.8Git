<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" Inherits="addcustomer" MasterPageFile="~/Mom.master" CodeBehind="AddCustomer.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!--Grid Control--> 

    <link href="Design/css/grid.css" rel="stylesheet" />

    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <script type="text/javascript">

        function IsPasswordGreaterThan10(evt, obj) {
            var txt = obj.value.length;
            if (txt >= 10) {
                noty({ text: 'Password length must be less than 11 characters.!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 2000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
        }
        function OpenContactModal() {
            window.radopen(null, "contactWindow");
        }
        function ResetShowAll() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $('#<%=lblQuarter.ClientID%>').removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
            document.getElementById('rdAll2').checked = true;
            document.getElementById('rdAll').checked = true;
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem("hdnInvDate", document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value);

            }
        }
        function ResetShowAllOpen() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $('#<%=lblQuarter.ClientID%>').removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
            document.getElementById('rdAll2').checked = true;
            document.getElementById('rdOpen').checked = true;
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem("hdnInvDate", document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value);

            }
        }
        function showFilterServiceSearch() {
            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");
            var ddlCategory = $("#<%=ddlCategory.ClientID%>");

            txtSearch.css("display", "none");
            ddlCategory.css("display", "none");
            ddlCategory.val("None");
            txtSearch.val("");
            switch (ddlSearch.val()) {
                case 't.cat':
                    ddlCategory.css("display", "block");
                    break;
                default:
                    txtSearch.css("display", "block");
                    break;
            }
            return false;
        }
        function showFilterSearch() {

            var ddlSearchInv = $("#<%=ddlSearchInv.ClientID%>");
            var txtInvDt = $("#<%=txtInvDt.ClientID%>");
            var txtSearchInv = $("#<%=txtSearchInv.ClientID%>");
            var ddllocation = $("#<%=ddllocation.ClientID%>");

            txtSearchInv.css("display", "none");
            txtInvDt.css("display", "none");
            ddllocation.css("display", "none");


            switch (ddlSearchInv.val()) {
                case 'i.ref':
                    txtSearchInv.css("display", "block");
                    break;
                case 'i.fdate':
                    txtInvDt.css("display", "block");
                    break;
                case 'l.loc':
                    ddllocation.css("display", "block");
                    break;
                default:
                    txtSearchInv.css("display", "block");
                    break;
            }
        }

        function AddJobClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeJob.ClientID%>').value;
            if (id == "Y") {
                //alert("hi");
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
    </script>

    <style>
        .dropdown-content {
            margin-top: 2px !important;
        }
        .FormGrid .rgDataDiv {
            max-height: 382px;
        }
        .newClassTooltip {
            background: #000 none repeat scroll 0 0;
            filter: alpha(opacity=80);
            -moz-opacity: 0.80;
            opacity: 0.80;
            border-radius: 0px !important;
            color: #fff;
            display: none;
            left: 0px;
            padding: 10px;
            position: relative;
            top: 0px;
            visibility: hidden;
            width: 400px;
            z-index: 1000;
        }

            .newClassTooltip:after {
                top: 0%;
                left: 0%;
                border: solid transparent;
                content: " ";
                height: 0;
                width: 0;
                position: absolute;
                pointer-events: none;
                border-color: rgba(0, 0, 0, 0);
                border-top-color: #000;
                border-width: 10px;
                margin-left: -10px;
            }

        [id$='RadGrid_Opportunity_GridHeader'] .rgHeader > a,
        [id$='RadGrid_Location_GridHeader'] .rgHeader > a,
        [id$='RadGrid_Project_GridHeader'] .rgHeader > a,
        [id$='RadGrid_Invoices'] .rgHeader > a {
            white-space: nowrap;
            padding-left: 0 !important;
        }

        .RadFilterMenu_CheckList {
            top: 20px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <telerik:RadAjaxManager ID="RadAjaxManager_Customer" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OpenCalls" LoadingPanelID="RadAjaxLoadingPanel_Customer" />

                    <telerik:AjaxUpdatedControl ControlID="hdnSelectedDtRange" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>

                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoices" LoadingPanelID="RadAjaxLoadingPanel_Customer_Trans" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtTo" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtFrom" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_Invoices">
                <UpdatedControls>

                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoices" LoadingPanelID="RadAjaxLoadingPanel_Customer_Trans" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoices" LoadingPanelID="RadAjaxLoadingPanel_Customer_Trans" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="lblLocationBalance" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDt" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtTo" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtFrom" />
                    <telerik:AjaxUpdatedControl ControlID="ddllocation" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>

                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoices" LoadingPanelID="RadAjaxLoadingPanel_Customer_Trans" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtTo" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtFrom" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDt" />
                    <telerik:AjaxUpdatedControl ControlID="ddllocation" />
                    <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAllOpen">
                <UpdatedControls>

                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoices" LoadingPanelID="RadAjaxLoadingPanel_Customer_Trans" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtTo" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtFrom" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDt" />
                    <telerik:AjaxUpdatedControl ControlID="ddllocation" />
                    <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkAddnew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="contactWindow" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="contactWindow" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowCustomerSaved" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>


            <telerik:AjaxSetting AjaxControlID="lnkEditTicket">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowAddTickets" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>


            <telerik:AjaxSetting AjaxControlID="lnkPrint">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowAddTickets" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkContactSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvContacts" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowLog">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContactLog" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Customer" runat="server">
    </telerik:RadAjaxLoadingPanel>

    <div class="topNav top-hei">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-social-people"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Add Customer</asp:Label></div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnSubmit" runat="server" ToolTip="Save" OnClick="btnSubmit_Click"
                                                ValidationGroup="general, rep" OnClientClick="return AlertSageIDUpdate();">Save</asp:LinkButton>
                                        </div>

                                        <ul id="custReport" class="dropdown-content" runat="server" style="margin-top: 2px !important;">
                                            <li>
                                                <a href="aragingreport.aspx?uid=<%=Request.QueryString["uid"]%>&page=addcustomer&lid=<%=Request.QueryString["uid"]%>" class="-text">AR Aging Report</a>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lnkCustomerStatement" OnClick="lnkCustomerStatement_Click" runat="server">Customer Statement</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lnkCustomerTransLedger" runat="server" OnClick="lnkCustomerTransLedger_Click">Customer Transaction Ledger Report</asp:LinkButton>
                                            </li>
                                        </ul>
                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="ctl00_ContentPlaceHolder1_custReport" runat="server" id="lnkReport">Reports
                                            </a>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap" id="divClose" runat="server">
                                        <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close"
                                            OnClick="lnkClose_Click" CausesValidation="false">
                                                    <i class="mdi-content-clear"></i>
                                        </asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
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
                                    <li><a id="lnkCustomeraccrd" href="#accrdcustInfo">Customer Info</a>
                                    </li>
                                    <li runat="server" id="licontact"><a id="lnkContactaccrd" href="#accrdcontacts">Contacts</a></li>
                                    <li runat="server" id="lidocuments"><a id="lnkDocumetnaccrd" href="#accrddocuments">Documents</a></li>
                                    <li runat="server" id="lilocations"><a id="lnkLocationaccrd" href="#accrdlocations">Locations</a></li>
                                    <li runat="server" id="liequipments"><a id="lnkEquipmentaccrd" href="#accrdequipments">Equipment</a></li>
                                    <li runat="server" id="litickets"><a id="lnkSHaccrd" href="#accrdserviceHistory">View Service History</a></li>
                                    <li runat="server" id="litrans"><a id="lnkTransactionaccrd" href="#accrdtransactions">Transactions</a></li>
                                    <li runat="server" id="liOpportunities"><a href="#accrdopportunities">Opportunities</a></li>
                                    <li runat="server" id="liProjects"><a href="#accrdprojects">Projects</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                </ul>
                                <asp:HiddenField ID="AcdFocus" runat="server" />
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev" id="divNavigate" runat="server">
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False"
                                            OnClick="lnkFirst_Click">
                                                <i class="fa fa-angle-double-left"></i>
                                        </asp:LinkButton></span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False"
                                            OnClick="lnkPrevious_Click">
                                                <i class="fa fa-angle-left"></i>
                                        </asp:LinkButton></span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False"
                                            OnClick="lnkNext_Click">
                                                <i class="fa fa-angle-right"></i>
                                        </asp:LinkButton></span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False"
                                            OnClick="lnkLast_Click">
                                                <i class="fa fa-angle-double-right"></i>
                                        </asp:LinkButton></span>
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
            <div class="col s12 m12 l12">
                <div class="row">
                    <%--<ul class="collapsible popout collapsible-accordion form-accordion-head expandable" test>--%>
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li>
                            <div id="accrdcustInfo" class="collapsible-header accrd active accordian-text-custom active"><i class="mdi-communication-contacts"></i>Customer Info</div>
                            <div class="collapsible-body" id="divcustInfo">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="alert alert-danger" runat="server" id="divLabelMessage" style="display: none">
                                                <button type="button" class="close" data-dismiss="alert">×</button>
                                                You cannot select the Combined Billing , as there are no locations added yet.
                                            </div>
                                            <div class="section-ttle">Customer Details</div>



                                            <div class="form-section3">


                                                <div class="input-field col s8">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtAcctno" runat="server" AutoCompleteType="Disabled" MaxLength="15"></asp:TextBox>
                                                        <asp:Label ID="lblSageID" AssociatedControlID="txtAcctno" runat="server">Sage ID</asp:Label>
                                                        <asp:HiddenField ID="hdnAcctID" runat="server" />


                                                    </div>
                                                </div>
                                                <asp:Button ID="btnSageID" runat="server" Text="Check" OnClick="btnSageID_Click"
                                                    CausesValidation="false" />
                                                <div class="srchclr btnlinksicon rowbtn ">
                                                    &nbsp;
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">


                                                        <asp:TextBox ID="txtCName" type="text" runat="server" AutoCompleteType="Disabled" autocomplete="new-customer"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtCName">Customer Name</asp:Label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="general, rep"
                                                            ControlToValidate="txtCName" Display="None" ErrorMessage="Customer name is required" SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server"
                                                            Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                        </asp:ValidatorCalloutExtender>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <div>
                                                            <label id="lblgoogleloc" for="txtGoogleAutoc" class="drpdwn-label">Customer Address</label>
                                                        </div>
                                                        <asp:TextBox AutoCompleteType="Disabled" TextMode="MultiLine" Rows="4" CssClass="materialize-textarea" ID="txtGoogleAutoc" runat="server" placeholder=""></asp:TextBox>

                                                        <%--<textarea  name="txtGoogleAutoc" class="materialize-textarea" placeholder="" />--%>
                                                        <%--<asp:TextBox ID="txtAddress" type="text" runat="server" TextMode="MultiLine" Style="padding-top: 5px; min-height: 20px; height: 20px;" CssClass="materialize-textarea" ONKEYUP="return checkMaxLength(this, event, 35)"></asp:TextBox>--%>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                            ControlToValidate="txtGoogleAutoc" Display="None" ErrorMessage="Address Required"
                                                            SetFocusOnError="True" ValidationGroup="general, rep">
                                                        </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                            ID="RequiredFieldValidator11_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator11">
                                                        </asp:ValidatorCalloutExtender>
                                                    </div>
                                                </div>



                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">

                                                <div class="input-field col s5">
                                                    <div class="row">

                                                        <asp:Label runat="server" AssociatedControlID="txtCity">City</asp:Label>
                                                        <asp:TextBox autocomplete="new-city" ID="txtCity" type="text" runat="server"></asp:TextBox>

                                                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                            ControlToValidate="txtState" ValidationGroup="general, rep" InitialValue="State" Display="None" ErrorMessage="State Required"
                                                            SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                            ID="RequiredFieldValidator7_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator7">
                                                        </asp:ValidatorCalloutExtender>
                                                        <label>State/Province</label>
                                                        <asp:TextBox ID="txtState" type="text" runat="server"></asp:TextBox>

                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">

                                                        <asp:Label runat="server" AssociatedControlID="txtZip">Zip/Postal Code</asp:Label>

                                                        <asp:TextBox autocomplete="new-zip" ID="txtZip" type="text" runat="server" AutoCompleteType="Disabled"></asp:TextBox>

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
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                            ControlToValidate="ddlCountry" ValidationGroup="general, rep" InitialValue="State" Display="None" ErrorMessage="Country Required"
                                                            SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                            ID="ValidatorCalloutExtender1"
                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:DropDownList ID="ddlCountry" runat="server" ToolTip="Country" CssClass="browser-default">
                                                            <asp:ListItem Value="Select">Select</asp:ListItem>
                                                            <asp:ListItem Value="US">United States</asp:ListItem>
                                                            <asp:ListItem Value="CA">Canada</asp:ListItem>


                                                            <asp:ListItem Value="AF">Afghanistan</asp:ListItem>

                                                            <asp:ListItem Value="AL">Albania</asp:ListItem>

                                                            <asp:ListItem Value="DZ">Algeria</asp:ListItem>

                                                            <asp:ListItem Value="AS">American Samoa</asp:ListItem>

                                                            <asp:ListItem Value="AD">Andorra</asp:ListItem>

                                                            <asp:ListItem Value="AO">Angola</asp:ListItem>

                                                            <asp:ListItem Value="AI">Anguilla</asp:ListItem>

                                                            <asp:ListItem Value="AQ">Antarctica</asp:ListItem>

                                                            <asp:ListItem Value="AG">Antigua And Barbuda</asp:ListItem>

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

                                                            <asp:ListItem Value="BA">Bosnia And Herzegowina</asp:ListItem>

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

                                                            <asp:ListItem Value="CK">Cook Islands</asp:ListItem>

                                                            <asp:ListItem Value="CR">Costa Rica</asp:ListItem>

                                                            <asp:ListItem Value="CI">Cote D'Ivoire</asp:ListItem>

                                                            <asp:ListItem Value="HR">Croatia (Local Name: Hrvatska)</asp:ListItem>

                                                            <asp:ListItem Value="CU">Cuba</asp:ListItem>

                                                            <asp:ListItem Value="CY">Cyprus</asp:ListItem>

                                                            <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>

                                                            <asp:ListItem Value="DK">Denmark</asp:ListItem>

                                                            <asp:ListItem Value="DJ">Djibouti</asp:ListItem>

                                                            <asp:ListItem Value="DM">Dominica</asp:ListItem>

                                                            <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>

                                                            <asp:ListItem Value="TP">East Timor</asp:ListItem>

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

                                                            <asp:ListItem Value="GN">Guinea</asp:ListItem>

                                                            <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>

                                                            <asp:ListItem Value="GY">Guyana</asp:ListItem>

                                                            <asp:ListItem Value="HT">Haiti</asp:ListItem>

                                                            <asp:ListItem Value="HM">Heard And Mc Donald Islands</asp:ListItem>

                                                            <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>

                                                            <asp:ListItem Value="HN">Honduras</asp:ListItem>

                                                            <asp:ListItem Value="HK">Hong Kong</asp:ListItem>

                                                            <asp:ListItem Value="HU">Hungary</asp:ListItem>

                                                            <asp:ListItem Value="IS">Icel And</asp:ListItem>

                                                            <asp:ListItem Value="IN">India</asp:ListItem>

                                                            <asp:ListItem Value="ID">Indonesia</asp:ListItem>

                                                            <asp:ListItem Value="IR">Iran (Islamic Republic Of)</asp:ListItem>

                                                            <asp:ListItem Value="IQ">Iraq</asp:ListItem>

                                                            <asp:ListItem Value="IE">Ireland</asp:ListItem>

                                                            <asp:ListItem Value="IL">Israel</asp:ListItem>

                                                            <asp:ListItem Value="IT">Italy</asp:ListItem>

                                                            <asp:ListItem Value="JM">Jamaica</asp:ListItem>

                                                            <asp:ListItem Value="JP">Japan</asp:ListItem>

                                                            <asp:ListItem Value="JO">Jordan</asp:ListItem>

                                                            <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>

                                                            <asp:ListItem Value="KE">Kenya</asp:ListItem>

                                                            <asp:ListItem Value="KI">Kiribati</asp:ListItem>

                                                            <asp:ListItem Value="KP">Korea, Dem People'S Republic</asp:ListItem>

                                                            <asp:ListItem Value="KR">Korea, Republic Of</asp:ListItem>

                                                            <asp:ListItem Value="KW">Kuwait</asp:ListItem>

                                                            <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>

                                                            <asp:ListItem Value="LA">Lao People'S Dem Republic</asp:ListItem>

                                                            <asp:ListItem Value="LV">Latvia</asp:ListItem>

                                                            <asp:ListItem Value="LB">Lebanon</asp:ListItem>

                                                            <asp:ListItem Value="LS">Lesotho</asp:ListItem>

                                                            <asp:ListItem Value="LR">Liberia</asp:ListItem>

                                                            <asp:ListItem Value="LY">Libyan Arab Jamahiriya</asp:ListItem>

                                                            <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>

                                                            <asp:ListItem Value="LT">Lithuania</asp:ListItem>

                                                            <asp:ListItem Value="LU">Luxembourg</asp:ListItem>

                                                            <asp:ListItem Value="MO">Macau</asp:ListItem>

                                                            <asp:ListItem Value="MK">Macedonia</asp:ListItem>

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

                                                            <asp:ListItem Value="FM">Micronesia, Federated States</asp:ListItem>

                                                            <asp:ListItem Value="MD">Moldova, Republic Of</asp:ListItem>

                                                            <asp:ListItem Value="MC">Monaco</asp:ListItem>

                                                            <asp:ListItem Value="MN">Mongolia</asp:ListItem>

                                                            <asp:ListItem Value="MS">Montserrat</asp:ListItem>

                                                            <asp:ListItem Value="MA">Morocco</asp:ListItem>

                                                            <asp:ListItem Value="MZ">Mozambique</asp:ListItem>

                                                            <asp:ListItem Value="MM">Myanmar</asp:ListItem>

                                                            <asp:ListItem Value="NA">Namibia</asp:ListItem>

                                                            <asp:ListItem Value="NR">Nauru</asp:ListItem>

                                                            <asp:ListItem Value="NP">Nepal</asp:ListItem>

                                                            <asp:ListItem Value="NL">Netherlands</asp:ListItem>

                                                            <asp:ListItem Value="AN">Netherlands Ant Illes</asp:ListItem>

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

                                                            <asp:ListItem Value="KN">Saint K Itts And Nevis</asp:ListItem>

                                                            <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>

                                                            <asp:ListItem Value="VC">Saint Vincent, The Grenadines</asp:ListItem>

                                                            <asp:ListItem Value="WS">Samoa</asp:ListItem>

                                                            <asp:ListItem Value="SM">San Marino</asp:ListItem>

                                                            <asp:ListItem Value="ST">Sao Tome And Principe</asp:ListItem>

                                                            <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>

                                                            <asp:ListItem Value="SN">Senegal</asp:ListItem>

                                                            <asp:ListItem Value="SC">Seychelles</asp:ListItem>

                                                            <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>

                                                            <asp:ListItem Value="SG">Singapore</asp:ListItem>

                                                            <asp:ListItem Value="SK">Slovakia (Slovak Republic)</asp:ListItem>

                                                            <asp:ListItem Value="SI">Slovenia</asp:ListItem>

                                                            <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>

                                                            <asp:ListItem Value="SO">Somalia</asp:ListItem>

                                                            <asp:ListItem Value="ZA">South Africa</asp:ListItem>

                                                            <asp:ListItem Value="GS">South Georgia , S Sandwich Is.</asp:ListItem>

                                                            <asp:ListItem Value="ES">Spain</asp:ListItem>

                                                            <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>

                                                            <asp:ListItem Value="SH">St. Helena</asp:ListItem>

                                                            <asp:ListItem Value="PM">St. Pierre And Miquelon</asp:ListItem>

                                                            <asp:ListItem Value="SD">Sudan</asp:ListItem>

                                                            <asp:ListItem Value="SR">Suriname</asp:ListItem>

                                                            <asp:ListItem Value="SJ">Svalbard, Jan Mayen Islands</asp:ListItem>

                                                            <asp:ListItem Value="SZ">Sw Aziland</asp:ListItem>

                                                            <asp:ListItem Value="SE">Sweden</asp:ListItem>

                                                            <asp:ListItem Value="CH">Switzerland</asp:ListItem>

                                                            <asp:ListItem Value="SY">Syrian Arab Republic</asp:ListItem>

                                                            <asp:ListItem Value="TW">Taiwan</asp:ListItem>

                                                            <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>

                                                            <asp:ListItem Value="TZ">Tanzania, United Republic Of</asp:ListItem>

                                                            <asp:ListItem Value="TH">Thailand</asp:ListItem>

                                                            <asp:ListItem Value="TG">Togo</asp:ListItem>

                                                            <asp:ListItem Value="TK">Tokelau</asp:ListItem>

                                                            <asp:ListItem Value="TO">Tonga</asp:ListItem>

                                                            <asp:ListItem Value="TT">Trinidad And Tobago</asp:ListItem>

                                                            <asp:ListItem Value="TN">Tunisia</asp:ListItem>

                                                            <asp:ListItem Value="TR">Turkey</asp:ListItem>

                                                            <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>

                                                            <asp:ListItem Value="TC">Turks And Caicos Islands</asp:ListItem>

                                                            <asp:ListItem Value="TV">Tuvalu</asp:ListItem>

                                                            <asp:ListItem Value="UG">Uganda</asp:ListItem>

                                                            <asp:ListItem Value="UA">Ukraine</asp:ListItem>

                                                            <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>

                                                            <asp:ListItem Value="GB">United Kingdom</asp:ListItem>



                                                            <asp:ListItem Value="UM">United States Minor Is.</asp:ListItem>

                                                            <asp:ListItem Value="UY">Uruguay</asp:ListItem>

                                                            <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>

                                                            <asp:ListItem Value="VU">Vanuatu</asp:ListItem>

                                                            <asp:ListItem Value="VE">Venezuela</asp:ListItem>

                                                            <asp:ListItem Value="VN">Viet Nam</asp:ListItem>

                                                            <asp:ListItem Value="VG">Virgin Islands (British)</asp:ListItem>

                                                            <asp:ListItem Value="VI">Virgin Islands (U.S.)</asp:ListItem>

                                                            <asp:ListItem Value="WF">Wallis And Futuna Islands</asp:ListItem>

                                                            <asp:ListItem Value="EH">Western Sahara</asp:ListItem>

                                                            <asp:ListItem Value="YE">Yemen</asp:ListItem>

                                                            <asp:ListItem Value="YU">Yugoslavia</asp:ListItem>

                                                            <asp:ListItem Value="ZR">Zaire</asp:ListItem>

                                                            <asp:ListItem Value="ZM">Zambia</asp:ListItem>

                                                            <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>

                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <%--USE THIS AS A REFERENCE FOR DROPDOWN--%>
                                                        <label class="drpdwn-label">Type</label>
                                                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Status</label>

                                                        <asp:DropDownList ID="ddlCustStatus" runat="server"
                                                            class="browser-default">
                                                            <asp:ListItem Value="0">Active</asp:ListItem>
                                                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div id="dvCompanyPermission" runat="server">
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Company</label>
                                                            <asp:DropDownList ID="ddlCompany" runat="server"
                                                                class="browser-default">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvCompanyValidate" runat="server"
                                                                ControlToValidate="ddlCompany" ValidationGroup="general, rep" InitialValue="0" Display="None" ErrorMessage="Company Required"
                                                                SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                ID="ValidatorCalloutExtender3"
                                                                runat="server" Enabled="True" TargetControlID="rfvCompanyValidate">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row" style="padding: 0px;">
                                                            <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="srchclr btnlinksicon rowbtn">
                                                        <asp:HyperLink ID="btnCompanyPopUp" runat="server" Style="cursor: pointer;" onclick="OpenCompanyPopUp(this);" ToolTip="Change Company">  <i class="mdi-action-work" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                    </div>
                                                </div>
                                                <%-- <div id="myModals" class="modal fade" role="dialog">
                                                    <div class="modal-dialog">
                                                        <!-- Modal content-->
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                                <h5 class="modal-title">Select Company</h5>
                                                            </div>
                                                            <div class="modal-body">
                                                                <asp:DropDownList ID="ddlCompanyEdit" runat="server"
                                                                    CssClass="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div style="clear: both;"></div>

                                                            <div class="modal-footer">
                                                                <asp:Button ID="btnCompanyEdit" runat="server" class="btn btn-default modal-btn-sbmt" Text="SAVE" OnClick="btnSave_Click" />

                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>--%>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>





                                            <div class="form-section3">
                                                <div class="input-field col s5">
                                                    <div class="row">

                                                        <asp:Label runat="server" AssociatedControlID="lat">Latitude</asp:Label>

                                                        <input id="lat" runat="server" type="text" />

                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">

                                                        <asp:Label runat="server" AssociatedControlID="lng">Longitude</asp:Label>

                                                        <input id="lng" runat="server" type="text" />


                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">


                                                        <div id="map" class="map-cu">
                                                        </div>



                                                    </div>
                                                </div>



                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:Label ID="Label4" Style="display: none" runat="server" Text="ID"></asp:Label>

                                                    </div>
                                                </div>

                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>

                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:Label ID="Label5" Style="display: none" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section-row">

                                            <div class="section-ttle">Main Contact Info.</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtMaincontact" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtMaincontact">Main Contact</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox autocomplete="new-phone" ID="txtPhoneCust" runat="server" AutoCompleteType="Disabled"></asp:TextBox>

                                                        <asp:Label runat="server" AssociatedControlID="txtPhoneCust">Phone</asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCell" runat="server" AutoCompleteType="Disabled"></asp:TextBox>

                                                        <asp:Label runat="server" AssociatedControlID="txtCell">Cellular</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtFax" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtFax">Fax</asp:Label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                            ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email"
                                                            SetFocusOnError="True"
                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="general, rep">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2" PopupPosition="BottomLeft">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox autocomplete="new-email" ID="txtEmail" runat="server" CssClass="form-control" AutoCompleteType="Disabled"
                                                            MaxLength="99">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                            TargetControlID="txtEmail">
                                                        </asp:FilteredTextBoxExtender>

                                                        <asp:Label runat="server" AssociatedControlID="txtEmail">Email</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtWebsite" runat="server"
                                                            ControlToValidate="txtWebsite" Display="None" ErrorMessage="Invalid Website"
                                                            SetFocusOnError="True"
                                                            ValidationExpression="[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)" ValidationGroup="general, rep">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2"
                                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidatortxtWebsite" PopupPosition="BottomLeft">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtWebsite" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                            TargetControlID="txtWebsite">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:Label runat="server" AssociatedControlID="txtWebsite">Website</asp:Label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="form-section-row">

                                            <div class="section-ttle">Billing Details</div>
                                            <div class="form-section3">
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtBillRate" runat="server" AutoCompleteType="Disabled"
                                                            onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)">
                                                        </asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtBillRate">
                                                            Billing rate
                                                        </asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtOt" runat="server" AutoCompleteType="Disabled"
                                                            onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)">
                                                        </asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtOt">OT Rate</asp:Label>
                                                    </div>
                                                </div>

                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtNt" runat="server" AutoCompleteType="Disabled"
                                                            MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)">
                                                        </asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtNt">1.7 Rate</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtDt" runat="server" AutoCompleteType="Disabled" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtDt">DT Rate</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTravel" runat="server" AutoCompleteType="Disabled" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtTravel">Travel Rate</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtMileage" runat="server" AutoCompleteType="Disabled" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtMileage">Mileage</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtCst1" class="">
                                                            <asp:Label ID="lblCustom1" runat="server"></asp:Label></asp:Label>
                                                        <asp:TextBox ID="txtCst1" runat="server" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtCst2" class="">
                                                            <asp:Label ID="lblCustom2" runat="server"></asp:Label></asp:Label>
                                                        <asp:TextBox ID="txtCst2" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s7">
                                                    <div class="checkrow mb-che" >
                                                        <asp:CheckBox ID="CopyToLocAndJob" CssClass="css-checkbox" Text="Copy to Location/Project" runat="server" onclick="checked1();"></asp:CheckBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkShutdownAlert" CssClass="css-checkbox" Text="Shutdown Alert" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label"> Customer Recurring Billing</label>
                                                        <asp:DropDownList ID="ddlBilling" runat="server" onChange="onDdlBillingChange(this.options[this.selectedIndex].value);"
                                                            CssClass="browser-default">
                                                            <asp:ListItem Value="0">Individual</asp:ListItem>
                                                            <asp:ListItem Value="1">Combined</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12" id="divCentral" style="display: none;">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Location for Customer Combined Billing
</label>
                                                        <asp:DropDownList ID="ddlSpecifiedLocation" runat="server"
                                                            CssClass="browser-default">
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
                                                        <asp:Label runat="server" AssociatedControlID="txtAlert">Shutdown Alert Message</asp:Label>
                                                        <asp:TextBox TextMode="MultiLine" ID="txtAlert" AutoCompleteType="Disabled" runat="server" CssClass="materialize-textarea"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtRemarks">Remarks</asp:Label>
                                                        <asp:TextBox ID="txtRemarks" runat="server" AutoCompleteType="Disabled"
                                                            TextMode="MultiLine" CssClass="materialize-textarea">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="form-section-row">

                                            <div class="section-ttle">
                                                Portal
                                            </div>
                                            <div class="form-section3">

                                                <div class="input-field col s12">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkInternet" CssClass="css-checkbox" Text="Internet" runat="server" />
                                                    </div>
                                                </div>

                                            </div>
                                            <div id="divInternet">

                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                ControlToValidate="txtUserName" Display="None" Enabled="False"
                                                                ErrorMessage="Username Required"
                                                                SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="Left"
                                                                TargetControlID="RequiredFieldValidator3">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:TextBox ID="txtUserName" runat="server" AutoCompleteType="Disabled"
                                                                MaxLength="15">
                                                            </asp:TextBox>
                                                            <asp:Label runat="server" AssociatedControlID="txtUserName" class="active">Username</asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                                ControlToValidate="txtPassword" Display="None" Enabled="False" AutoCompleteType="Disabled"
                                                                ErrorMessage="Password Required"
                                                                SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator4_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="Left"
                                                                TargetControlID="RequiredFieldValidator4">
                                                            </asp:ValidatorCalloutExtender>

                                                            <asp:TextBox ID="txtPassword" runat="server" AutoCompleteType="Disabled" onkeypress="javascript:IsPasswordGreaterThan10(event,this);" MaxLength="10"
                                                                CssClass="form-control">
                                                            </asp:TextBox>

                                                            <asp:Label runat="server" AssociatedControlID="txtPassword" class="active">Password</asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section3">

                                                    <div class="input-field col s12 chkmgn">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkMap" CssClass="css-checkbox" Text="View Service History" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkScheduleBrd" CssClass="css-checkbox" Text="View Invoices" runat="server" />
                                                        </div>
                                                    </div>


                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="input-field col s12">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkEquipments" CssClass="css-checkbox" Text="View Equipment" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkGrpWO" CssClass="css-checkbox" Text="Group by Work Order" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">

                                                    <div class="input-field col s12">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkOpenTicket" CssClass="css-checkbox" Text="All Open Tickets" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>

                                </div>
                            </div>
                        </li>
                        <li id="dvContact" runat="server">
                            <div id="accrdcontacts" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-account-circle"></i>Contacts</div>
                            <div class="collapsible-body">
                                <asp:Panel ID="pnlgvConPermission" runat="server">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="btncontainer">

                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAddnew" runat="server" CausesValidation="False" OnClientClick=" AddContactClick(this); return false" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">

                                                    <asp:LinkButton ToolTip="Edit"
                                                        ID="btnEdit" runat="server"
                                                        OnClientClick=" EditContactClick(this);return false" CausesValidation="False" OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnDelete" ToolTip="Delete"
                                                        runat="server" CausesValidation="False"
                                                        OnClientClick="return DeleteContactClick(this);"
                                                        OnClick="btnDelete_Click">Delete</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkMail" runat="server" title="Send Email" ToolTip="Send Email">Email</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton
                                                        ID="lnkShowLog" runat="server" CausesValidation="False" OnClick="lnkShowLog_Click">Contact log</asp:LinkButton>

                                                </div>
                                            </div>

                                            <div class="grid_container">
                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_gvContacts.ClientID %>");
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvContacts" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvContacts" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" PagerStyle-AlwaysVisible="true" Width="100%" FilterType="CheckList" OnPreRender="RadGrid_gvContacts_PreRender"
                                                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvContacts_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                <Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnSelected" runat="server" />
                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                    </telerik:GridClientSelectColumn>

                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="5" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIndex" runat="server" Text='<%# Container.ItemIndex %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn UniqueName="lblIndexID" Display="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex %>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="ContactID" SortExpression="ContactID" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="ContactID" ShowFilterIcon="false" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("ContactID") %>'></asp:Label>

                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderText="Name" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                            <asp:HiddenField ID="hdnID" runat="server" Value='<%# Bind("ContactID") %>' />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderText="Title" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Phone" SortExpression="Phone" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderText="Phone" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPhn" runat="server" Text='<%#Eval("Phone")%>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Fax" SortExpression="Fax" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderText="Fax" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFx" runat="server" Text='<%#Eval("Fax")%>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Cell" SortExpression="Cell" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderText="Cell" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCell" runat="server" Text='<%#Eval("Cell")%>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Email" SortExpression="Email" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderText="Email" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email")%>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn
                                                                        CurrentFilterFunction="Contains" HeaderText="Shutdown Alert" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkShutdown" runat="server" Checked='<%#Convert.ToBoolean( Eval("ShutdownAlert")) %>' />
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

                                </asp:Panel>
                                <div style="clear: both;"></div>
                            </div>
                        </li>

                        <li runat="server" id="dvDocuments">
                            <div id="accrddocuments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-credit-card"></i>Documents</div>
                            <div class="collapsible-body">
                                <asp:Panel ID="pnlDocPermission" runat="server">
                                    <%--<asp:Panel ID="pnlDoc" runat="server" Visible="false">
                                        <asp:Panel ID="pnlDocumentButtons" runat="server">--%>
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="form-section-row">
                                                <div class="col s12 m12 l12">
                                                    <div class="row">
                                                        <!--<p>Maximum file upload size 2MB.</p>-->
                                                       <%-- <input type="file" id="FileUpload1" runat="server" AllowMultiple="true"
                                                            onchange="AddDocumentClick(this);" class="dropify" />--%>
                                                         <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"
                                                            onchange="AddDocumentClick(this);" class="dropify" />
                                                        <!--data-max-file-size="2M"-->
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkUploadDoc" runat="server"
                                                    CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                    Style="display: none">Upload</asp:LinkButton>
                                                <asp:LinkButton
                                                    ID="lnkPostback" runat="server"
                                                    CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False"
                                                    OnClick="lnkDeleteDoc_Click"
                                                    OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                            </div>


                                            <div class="grid_container" style="margin-top: 10px;">
                                                <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock4" runat="server">
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
                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" PostBackControls="lblName" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                            <%---------->--%>
                                                         <%--   <div class="btnlinks" style="margin-left: -10px;">
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click"
                                                                OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                                        </div>
                                                        <span class="tro trost">
                                                            <asp:CheckBox ID="chkShowAllDocs" Text="Show All Documents" OnCheckedChanged="chkShowAllDocs_CheckedChanged" class="css-checkbox" Style="padding-left: 5px; color: black; font-weight: 400" ForeColor="Black" AutoPostBack="true" runat="server" />
                                                        </span>--%>
                                                            <%----------------<--%>
                                  
                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" MasterTableView-RowIndicatorColumn-AutoPostBackOnFilter="true" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                PagerStyle-AlwaysVisible="true" ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" 
                                                                AllowCustomPaging="True" OnPreRender="RadGrid_Documents_PreRender" OnNeedDataSource="RadGrid_Documents_NeedDataSource" >
                                                                <CommandItemStyle />
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                    <Selecting AllowRowSelect="True"></Selecting>

                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true"/>
                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                    <Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>

                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">


                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridClientSelectColumn UniqueName="chkSelect"
                                                                            HeaderStyle-Width="28">
                                                                        </telerik:GridClientSelectColumn>


                                                                        <telerik:GridTemplateColumn DataField="filename" SortExpression="filename" AutoPostBackOnFilter="true" HeaderStyle-Width="30%"
                                                                            CurrentFilterFunction="Contains" HeaderText="File Name" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                                    CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                    OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                                                </asp:LinkButton>

                                                                            </ItemTemplate>

                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn DataField="doctype" SortExpression="doctype" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="File Type" ShowFilterIcon="false" HeaderStyle-Width="15%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("doctype") %>'></asp:Label>

                                                                            </ItemTemplate>

                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="portal" SortExpression="portal" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Portal" ShowFilterIcon="false"  FilterControlWidth="20" HeaderStyle-Width="15%">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                            </ItemTemplate>

                                                                        </telerik:GridTemplateColumn>

                                                                        <%--<telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="20%">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                            </ItemTemplate>

                                                                        </telerik:GridTemplateColumn>--%>

                                                                         <telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                                                            HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="100"
                                                                                                            DataType="System.Int16" UniqueName='MSVisible'>
                                                                                                            <FilterTemplate>
                                                                                                                <telerik:RadComboBox RenderMode="Auto" ID="ImportedFilter" runat="server" OnClientSelectedIndexChanged="ImportedFilterSelectedIndexChanged"
                                                                                                                    SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("MSVisible").CurrentFilterValue %>'
                                                                                                                    Width="100px">
                                                                                                                    <Items>
                                                                                                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                                                                                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                                                                                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                                                                                                    </Items>
                                                                                                                </telerik:RadComboBox>
                                                                                                                <telerik:RadScriptBlock ID="RadScriptBlock12" runat="server">
                                                                                                                    <script type="text/javascript">
                                                                                                                        function ImportedFilterSelectedIndexChanged(sender, args) {
                                                                                                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                                                                                            var filterVal = args.get_item().get_value();
                                                                                                                            if (filterVal == "") {
                                                                                                                                tableView.filter("MSVisible", filterVal, "NoFilter");
                                                                                                                            }
                                                                                                                            else if (filterVal == "1") {
                                                                                                                                tableView.filter("MSVisible", "1", "EqualTo");
                                                                                                                            }
                                                                                                                            else if (filterVal == "0") {
                                                                                                                                tableView.filter("MSVisible", "0", "EqualTo");
                                                                                                                            }
                                                                                                                        }
                                                                                                                    </script>
                                                                                                                </telerik:RadScriptBlock>
                                                                                                            </FilterTemplate>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                                                            </ItemTemplate>
                                                                             </telerik:GridTemplateColumn>


                                                                        <telerik:GridTemplateColumn DataField="remarks" SortExpression="remarks" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Remarks" ShowFilterIcon="false" HeaderStyle-Width="20%">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtremarks" Width="500px" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
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
                                <%--</asp:Panel>
                                  </asp:Panel>--%>
                                </asp:Panel>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="dvLocations">
                            <div id="accrdlocations" class="collapsible-header accrd accordian-text-custom"><i class="mdi-maps-local-laundry-service"></i>Locations</div>
                            <div class="collapsible-body" id="divloc">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddLoc" OnClientClick='return AddLocClick(this)'
                                                    runat="server" OnClick="lnkAddLoc_Click" CausesValidation="False">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkEditLoc" runat="server" OnClientClick='return EditLocClick(this)' OnClick="lnkEditLoc_Click" CausesValidation="False">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkCopyloc" OnClientClick='return CopyLocClick(this)'
                                                    runat="server" OnClick="lnkCopyloc_Click" CausesValidation="False">Copy</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkDeleteLoc" OnClientClick='return DeleteLocClick(this)'
                                                    runat="server"
                                                    OnClick="lnkDeleteLoc_Click" CausesValidation="False">Delete</asp:LinkButton>
                                            </div>

                                        </div>
                                        <div class="grid_container">

                                            <div class="form-section-row" style="margin-bottom: 0 !important;">

                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_Location.ClientID %>");
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Location" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Location" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" OnPreRender="RadGrid_Location_PreRender"
                                                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_Location_NeedDataSource" >
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                <%--<Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>--%>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="loc">
                                                                <Columns>
                                                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="28">
                                                                    </telerik:GridClientSelectColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="30px" Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnSelected" runat="server" />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblloc" runat="server" Text='<%# Bind("loc") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="locid" SortExpression="locid" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Acct #" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("locid") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="name" SortExpression="name" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Location Name" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("tag") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="address" SortExpression="address" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Address" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFx" runat="server"><%#Eval("Address")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="City" SortExpression="City" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="City" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCity" runat="server"><%#Eval("city")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="type" SortExpression="type" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Type" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblType" runat="server"><%#Eval("type")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="status" SortExpression="status" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblstatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="elev" SortExpression="elev" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" DataType="System.String" HeaderText="# of Equip" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblelev" runat="server"><%#Eval("Elevs")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="balance" SortExpression="balance" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="GreaterThan" HeaderText="Balance" ShowFilterIcon="true" FooterAggregateFormatString="{0:c}" Aggregate="Sum">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBalance" runat="server" ForeColor='<%# Convert.ToDouble(Eval("balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'></asp:Label>
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
                            </div>
                        </li>
                        <li runat="server" id="divEquipment">
                            <div id="accrdequipments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-maps-local-laundry-service"></i>Equipment</div>
                            <div class="collapsible-body" id="divEquip">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddEquip" OnClientClick='return AddEquipmentClick(this)'
                                                    runat="server" OnClick="lnkAddEquip_Click" CausesValidation="False">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton
                                                    ID="lnkEditEquip" runat="server" OnClientClick='return EditEquipmentClick(this)' OnClick="lnkEditEquip_Click"
                                                    CausesValidation="False">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkcopyEquip" OnClientClick='return AddEquipmentClick(this)'
                                                    runat="server" OnClick="lnkcopyEquip_Click" CausesValidation="False">Copy</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkDeleteEquip" OnClientClick='return DeleteEquipmentClick(this)'
                                                    runat="server"
                                                    OnClick="lnkDeleteEquip_Click" CausesValidation="False">Delete</asp:LinkButton>
                                            </div>

                                        </div>


                                        <div class="grid_container">
                                            <div class="form-section-row mb" >

                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
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

                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Equip" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Equip" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true"
                                                            Width="100%" FilterType="CheckList" OnPreRender="RadGrid_Equip_PreRender" OnItemEvent="RadGrid_Equip_ItemEvent"
                                                            AllowCustomPaging="True"
                                                            OnFilterCheckListItemsRequested="RadGrid_Equip_FilterCheckListItemsRequested"
                                                            OnNeedDataSource="RadGrid_Equip_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="Unit">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>

                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="28">
                                                                    </telerik:GridClientSelectColumn>

                                                                    <telerik:GridTemplateColumn DataField="unit" SortExpression="unit" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Name" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Manuf" SortExpression="Manuf" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Manuf." ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblmanuf" runat="server" Text='<%# Bind("manuf") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="fdesc" SortExpression="fdesc" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Description" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Type" SortExpression="Type" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Type" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="cat" SortExpression="cat" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Service Type" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblServiceType" runat="server"><%#Eval("cat")%></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="status" SortExpression="status" AutoPostBackOnFilter="false"
                                                                        CurrentFilterFunction="Contains" HeaderText="Status" HeaderStyle-Width="140"
                                                                        FilterCheckListEnableLoadOnDemand="true" UniqueName="status"
                                                                        FilterControlAltText="Filter Status">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalActive" runat="server" />
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="locid" SortExpression="locid" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Location ID" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocid" runat="server"><%#Eval("LocID")%></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="tag" SortExpression="tag" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Location" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocName" runat="server"><%#Eval("tag")%></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Address" SortExpression="Address" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Address" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server"><%#Eval("Address")%></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Price" SortExpression="Price" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="EqualTo" HeaderText="Price" ShowFilterIcon="false"
                                                                        FooterAggregateFormatString="{0:c}" Aggregate="Sum">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPrice" runat="server"><%#Eval("Price")%></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="last" SortExpression="last" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Last Service" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbllast" runat="server"><%# Eval("last")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "last"))):""%></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Install" SortExpression="Install" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Installed" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSince" runat="server"><%# Eval("Install") != DBNull.Value ? String.Format("{0:M/d/yyyy}", Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "Install"))) : ""%></asp:Label>
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
                        <li runat="server" id="dvTickets">
                            <div id="accrdserviceHistory" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-copy"></i>View Service History</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="srchpaneinner">
                                            <div class="srchtitle" style="padding-left: 15px;">
                                                Search
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:DropDownList ID="ddlSearch" runat="server"
                                                    OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged" CssClass="browser-default selectst selectsml"
                                                    onchange="showFilterServiceSearch();">
                                                    <asp:ListItem Value=" ">Select</asp:ListItem>
                                                    <asp:ListItem Value="l.tag">Location</asp:ListItem>
                                                    <asp:ListItem Value="t.ID">Ticket #</asp:ListItem>
                                                    <asp:ListItem Value="t.ldesc4">Address</asp:ListItem>
                                                    <asp:ListItem Value="t.cat">Category</asp:ListItem>
                                                    <asp:ListItem Value="t.WorkOrder">WO #</asp:ListItem>
                                                    <asp:ListItem Value="e.unit">Equipment ID</asp:ListItem>
                                                    <asp:ListItem Value="t.fdesc">Reason for service</asp:ListItem>
                                                    <asp:ListItem Value="t.descres">Work Comp Desc</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search..." CssClass="srchcstm"></asp:TextBox>


                                            </div>
                                            <div class="srchinputwrap" style="margin-top: -9px;">
                                                <asp:DropDownList ID="ddlCategory" runat="server"
                                                    Style="display: none" CssClass="browser-default">
                                                </asp:DropDownList>
                                            </div>


                                        </div>
                                        <div class="srchpaneinner">
                                            <div class="srchtitle" style="padding-left: 15px;">
                                                Status
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:DropDownList ID="ddlStatus" runat="server"
                                                    CssClass="browser-default selectst selectsml">
                                                    <asp:ListItem Value="-1">All</asp:ListItem>
                                                    <asp:ListItem Value="1">Assigned</asp:ListItem>
                                                    <asp:ListItem Value="2">Enroute</asp:ListItem>
                                                    <asp:ListItem Value="3">Onsite</asp:ListItem>
                                                    <asp:ListItem Value="4">Completed</asp:ListItem>
                                                    <asp:ListItem Value="5">Hold</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:TextBox ID="txtfromDate" runat="server" CssClass="srchcstm datepicker_mom"></asp:TextBox>
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="srchcstm datepicker_mom"></asp:TextBox>
                                            </div>
                                            <div class="srchinputwrap tabcontainer">
                                                <ul class="tabselect accrd-tabselect" id="testrdButtons">
                                                    <li>

                                                        <asp:LinkButton AutoPostBack="False" ID="SHdecDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_dateHistory('dec','rdDTRange');return false"></asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <label id="SHlblDay" runat="server">
                                                            <input type="radio" id="SHrdDay" name="rdDTRange" value="rdDay" onclick="SelectDateHistory('Day', 'hdnSHDate'); return false" />
                                                            Day
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="SHlblWeek" runat="server">
                                                            <input type="radio" id="SHrdWeek" name="rdDTRange" value="rdWeek" onclick="SelectDateHistory('Week', 'hdnSHDate'); return false" />
                                                            Week
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="SHlblMonth" runat="server">
                                                            <input type="radio" id="SHrdMonth" name="rdDTRange" value="rdMonth" onclick="SelectDateHistory('Month', 'hdnSHDate'); return false" />
                                                            Month
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="SHlblQuarter" runat="server">
                                                            <input type="radio" id="SHrdQuarter" name="rdDTRange" value="rdQuarter" onclick="SelectDateHistory('Quarter', 'hdnSHDate'); return false" />
                                                            Quarter
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="SHlblYear" runat="server">
                                                            <input type="radio" id="SHrdYear" name="rdDTRange" value="rdYear" onclick="SelectDateHistory('Year', 'hdnSHDate'); return false" />
                                                            Year
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="SHincDate" runat="server" OnClientClick="dec_dateHistory('inc','rdDTRange');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                                                    </li>
                                                </ul>


                                            </div>
                                            <div class="srchinputwrap srchclr btnlinksicon" style="margin-left: -10px; margin-top: -2px;">
                                                <asp:LinkButton ID="btnSearch" runat="server" CausesValidation="False" OnClick="btnSearch_Click" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>

                                                <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click">
                                                    <i class=" mdi-action-print"></i>
                                                </asp:LinkButton>
                                            </div>

                                        </div>
                                        <div>
                                            <%---design issue---%>
                                            <asp:Label ID="lblRecordCount0" runat="server"></asp:Label>
                                        </div>
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkEditTicket" runat="server"
                                                    OnClientClick='return EditTicketClick(this)'
                                                    OnClick="lnkEditTicket_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkServiceExcel" runat="server" OnClick="lnkServiceExcel_Click">Export to Excel</asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="grid_container" style="margin-top: 10px;">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">

                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_OpenCalls.ClientID %>");
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

                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_OpenCalls" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_OpenCalls" PagerStyle-AlwaysVisible="true" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" OnPreRender="RadGrid_OpenCalls_PreRender"
                                                            AllowCustomPaging="True"
                                                            OnNeedDataSource="RadGrid_OpenCalls_NeedDataSource"
                                                            OnItemCreated="RadGrid_OpenCalls_ItemCreated"
                                                            OnFilterCheckListItemsRequested="RadGrid_OpenCalls_FilterCheckListItemsRequested">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="id">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblComp" runat="server" Text='<%# Bind("Comp") %>'></asp:Label>

                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="28">
                                                                    </telerik:GridClientSelectColumn>
                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" DataField="customername" ShowFilterIcon="false" HeaderText="Customer Name" UniqueName="customername">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("customername") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn ShowFilterIcon="false" DataField="id" SortExpression="id" AutoPostBackOnFilter="true" UniqueName="ID"
                                                                        CurrentFilterFunction="Contains" DataType="System.String" HeaderStyle-Width="120" HeaderText="Ticket #">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTicketId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnComp" runat="server" Value='<%# Bind("Comp") %>' />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridBoundColumn DataField="WorkOrder" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        FilterControlAltText="Filter Type" HeaderText="WO #" SortExpression="WorkOrder" HeaderStyle-Width="100"
                                                                        UniqueName="WorkOrder" ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn DataField="locid" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        FilterControlAltText="Filter Type" HeaderText="Acct #" SortExpression="locid" HeaderStyle-Width="100"
                                                                        UniqueName="locid" ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn DataField="locname" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        FilterControlAltText="Filter Type" HeaderText="Location Name" SortExpression="locname" HeaderStyle-Width="160"
                                                                        UniqueName="locname" ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn DataField="fulladdress" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        FilterControlAltText="Filter Type" HeaderText="Address" SortExpression="fulladdress" HeaderStyle-Width="140"
                                                                        UniqueName="fulladdress" ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridTemplateColumn Visible="false" DataField="City" AutoPostBackOnFilter="true" AllowFiltering="true"
                                                                        CurrentFilterFunction="Contains" SortExpression="City" UniqueName="City" HeaderText="City" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCity" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridBoundColumn DataField="Unit" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        FilterControlAltText="Filter Type" HeaderText="Equip" SortExpression="Unit" HeaderStyle-Width="90"
                                                                        UniqueName="Unit" ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridTemplateColumn Visible="false" DataField="Job" DataType="System.String" AutoPostBackOnFilter="true" AllowFiltering="true"
                                                                        CurrentFilterFunction="Contains" SortExpression="Job" UniqueName="Job" HeaderText="Project#" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProjectId" runat="server" Text='<%# Eval("Job").ToString()=="0" ? "": Eval("Job").ToString() %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn Visible="false" DataField="invoiceno" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" SortExpression="invoiceno" UniqueName="invoiceno" HeaderText="Invoice#" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInvoiceId" runat="server" Text='<%# Eval("invoiceno").ToString()=="0" ? "": Eval("invoiceno").ToString() %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn Visible="false" DataField="manualinvoice" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" SortExpression="manualinvoice" UniqueName="manualinvoice" HeaderText="ManualInv.#" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblManualInvoiceId" runat="server" Text='<%# Eval("manualinvoice").ToString()=="0" ? "": Eval("manualinvoice").ToString() %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn Visible="false" DataField="ProjectDescription" DataType="System.String" AutoPostBackOnFilter="true" AllowFiltering="true"
                                                                        CurrentFilterFunction="Contains" SortExpression="ProjectDescription" UniqueName="ProjectDescription" HeaderText="ProjectDesc" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProjectDesc" runat="server" Text='<%# Eval("ProjectDescription").ToString()=="" ? "": Eval("ProjectDescription").ToString() %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="dwork" SortExpression="dwork" UniqueName="dwork" DataType="System.String" AutoPostBackOnFilter="true"
                                                                        HeaderStyle-Width="140" HeaderText="Assigned To" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAssdTo" runat="server" Text='<%# Bind("dwork") %>'></asp:Label>
                                                                            <asp:Label ID="lblRes" runat="server" CssClass="newClassTooltip"
                                                                                Text='<%# ShowHoverText(Eval("description"),Eval("fdescreason")) %>'></asp:Label>
                                                                            <asp:HoverMenuExtender ID="hmeRes" runat="server" OffsetY="0" OffsetX="80" PopupControlID="lblRes"
                                                                                TargetControlID="lblAssdTo" HoverDelay="250">
                                                                            </asp:HoverMenuExtender>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" DataField="defaultworker" ShowFilterIcon="false" HeaderText="Default Worker" UniqueName="defaultworker">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDefaultworker" runat="server" Text='<%# Bind("defaultworker") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <%--     <telerik:GridBoundColumn DataField="cat" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        FilterControlAltText="Filter Type" HeaderText="Category" SortExpression="cat" HeaderStyle-Width="140"
                                                                        UniqueName="cat" ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>--%>

                                                                    <telerik:GridBoundColumn DataField="cat" SortExpression="cat" UniqueName="cat" HeaderText="Category" HeaderStyle-Width="140" FilterCheckListEnableLoadOnDemand="true" CurrentFilterFunction="Contains"
                                                                        FilterControlAltText="Category" AutoPostBackOnFilter="true">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="assignname" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        FilterControlAltText="Filter Type" HeaderText="Status" SortExpression="assignname" HeaderStyle-Width="140"
                                                                        UniqueName="assignname" ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="edate" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AllowFiltering="false"
                                                                        FilterControlAltText="Filter Type" HeaderText="Schedule Date" SortExpression="edate" HeaderStyle-Width="160"
                                                                        UniqueName="edate" ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="cdate" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AllowFiltering="false"
                                                                        FilterControlAltText="Filter Type" HeaderText="Call Date" SortExpression="cdate" HeaderStyle-Width="120"
                                                                        UniqueName="cdate" ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="Tottime" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataType="System.String"
                                                                        FilterControlAltText="Filter Type" HeaderText="Total Time" SortExpression="Tottime" HeaderStyle-Width="140"
                                                                        UniqueName="Tottime" ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn Visible="false" DataField="timediff" AutoPostBackOnFilter="true" AllowFiltering="true" SortExpression="timediff" DataType="System.Decimal"
                                                                        CurrentFilterFunction="EqualTo" UniqueName="timediff" HeaderText="TimeDiff" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotdiff" runat="server" Text='<%# Eval("timediff") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridBoundColumn Visible="false" DataField="department" AutoPostBackOnFilter="true" ShowFilterIcon="false" AllowFiltering="true" CurrentFilterFunction="Contains"
                                                                        HeaderText="Department" SortExpression="Department" HeaderStyle-Width="140"
                                                                        UniqueName="department">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" DataField="fDesc" ShowFilterIcon="false" HeaderText="Reason for service" UniqueName="fDesc">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReason" runat="server" Text='<%# Bind("fDesc") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" DataField="descres" ShowFilterIcon="false" HeaderText="Work Complete Desc" UniqueName="descres">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWorkCompleteDesc" runat="server" Text='<%# Bind("descres") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                            </FilterMenu>
                                                        </telerik:RadGrid>
                                                    </telerik:RadAjaxPanel>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li runat="server" id="divTrans">
                            <div id="accrdtransactions" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-credit-card"></i>Transactions</div>
                            <div class="collapsible-body" id="divInv">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="srchpaneinner">
                                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                                Invoices
                                            </div>
                                            <div class="srchinputwrap " style="margin-right: 24px;">
                                                <asp:DropDownList ID="ddlSearchInv" runat="server" onchange="showFilterSearch();"
                                                    OnSelectedIndexChanged="ddlSearchInv_SelectedIndexChanged" CssClass="browser-default selectsml selectst">
                                                    <asp:ListItem Value=" ">Select</asp:ListItem>
                                                    <asp:ListItem Value="i.ref">Invoice#</asp:ListItem>
                                                    <asp:ListItem Value="i.fdate">Invoice Date</asp:ListItem>
                                                    <%--<asp:ListItem Value="l.ID">Location ID</asp:ListItem>--%>
                                                    <asp:ListItem Value="l.loc">Location</asp:ListItem>

                                                </asp:DropDownList>

                                            </div>


                                            <div class="srchinputwrap">
                                                <asp:DropDownList ID="ddlDepartment" runat="server"
                                                    Style="display: none" CssClass="browser-default selectsml selectst">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlStatusInv" runat="server"
                                                    Style="display: none" CssClass="browser-default">
                                                    <asp:ListItem Value="0">Open</asp:ListItem>
                                                    <asp:ListItem Value="1">Paid</asp:ListItem>
                                                    <asp:ListItem Value="2">Voided</asp:ListItem>
                                                    <asp:ListItem Value="3">Partially Paid</asp:ListItem>
                                                    <asp:ListItem Value="4">Marked as Pending</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddllocation" runat="server"
                                                    Style="display: none" CssClass="srchcstm browser-default">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtInvDt" runat="server" CssClass="srchcstm datepicker_mom"
                                                    Style="display: none">
                                                </asp:TextBox>
                                                <asp:TextBox ID="txtSearchInv" runat="server" CssClass="srchcstm" placeholder="Search..."></asp:TextBox>
                                            </div>
                                            <div class="srchinputwrap rdleftmgn">
                                                <div class="rdpairing">
                                                    <div class="rd-flt">

                                                        <input id="rdAll" class="with-gap" name="radio-group" type="radio" value="rdAll" checked>
                                                        <label for="rdAll">All</label>
                                                    </div>
                                                    <div class="rd-flt">
                                                        <input id="rdOpen" class="with-gap" name="radio-group" type="radio" value="rdOpen">

                                                        <label for="rdOpen">Open</label>

                                                    </div>

                                                    <div class="rd-flt">

                                                        <input id="rdClosed" class="with-gap" name="radio-group" type="radio" value="rdClosed">
                                                        <label for="rdClosed">Closed</label>

                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hdnSearchValue" ClientIDMode="Static" />
                                                </div>

                                                <div class="rdpairing">
                                                    <div class="rd-flt">
                                                        <input id="rdAll2" class="with-gap" name="radio-group1" type="radio" value="rdAll2" checked>
                                                        <label for="rdAll2">All</label>
                                                    </div>

                                                    <div class="rd-flt">
                                                        <input id="rdCharges" class="with-gap" name="radio-group1" type="radio" value="rdCharges">
                                                        <label for="rdCharges">Charges</label>
                                                    </div>

                                                    <div class="rd-flt">
                                                        <input id="rdCredit" class="with-gap" name="radio-group1" type="radio" value="rdCredit">
                                                        <label for="rdCredit">Credits</label>
                                                    </div>


                                                </div>

                                                <asp:HiddenField runat="server" ID="hdnSearchBy" ClientIDMode="Static" />
                                            </div>
                                            <div style="float: right">
                                                <span>Customer Balance:
                                                     <asp:Label runat="server" ID="lblCustomerBalance">$0.00</asp:Label>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="srchpaneinner">
                                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                                Date
                                            </div>
                                            <div class="srchinputwrap" id="divDates" runat="server">
                                                <asp:TextBox ID="txtInvDtFrom" runat="server" CssClass="srchcstm datepicker_mom" AutoCompleteType="Disabled" OnTextChanged="txtInvDtFrom_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="srchinputwrap" id="divDates_1" runat="server">
                                                <asp:TextBox ID="txtInvDtTo" runat="server" CssClass="srchcstm datepicker_mom" AutoCompleteType="Disabled" OnTextChanged="txtInvDtTo_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="srchinputwrap tabcontainer">
                                                <ul class="tabselect accrd-tabselect" id="testradiobutton">
                                                    <li>
                                                        <%-- <asp:LinkButton AutoPostBack="True" ID="decDate" runat="server" OnClick="decDate_Click" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>"></asp:LinkButton></li>--%>
                                                        <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtInvDtTo','ctl00_ContentPlaceHolder1_txtInvDtFrom','rdCal');return false;"></asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <label id="lblDay" runat="server">
                                                            <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtInvDtFrom', 'ctl00_ContentPlaceHolder1_txtInvDtTo', '#lblDay', 'hdnInvDate', 'rdCal')" />

                                                            Day
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblWeek" runat="server">
                                                            <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtInvDtFrom', 'ctl00_ContentPlaceHolder1_txtInvDtTo', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnInvDate', 'rdCal')" />

                                                            Week
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblMonth" runat="server">
                                                            <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtInvDtFrom', 'ctl00_ContentPlaceHolder1_txtInvDtTo', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnInvDate', 'rdCal')" />

                                                            Month
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblQuarter" runat="server">
                                                            <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtInvDtFrom', 'ctl00_ContentPlaceHolder1_txtInvDtTo', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnInvDate', 'rdCal')" />

                                                            Quarter
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblYear" runat="server">
                                                            <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtInvDtFrom', 'ctl00_ContentPlaceHolder1_txtInvDtTo', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnInvDate', 'rdCal')" />

                                                            Year
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtInvDtTo','ctl00_ContentPlaceHolder1_txtInvDtFrom','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                                                    </li>
                                                </ul>


                                            </div>
                                            <div class="srchinputwrap srchclr btnlinksicon" style="margin-left: -10px; margin-top: -2px;">
                                                <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false" AutoPostback="false" ToolTip="Search" OnClick="lnkSearch_Click">
                                <i class="mdi-action-search"></i>
                                                </asp:LinkButton>

                                            </div>
                                        </div>
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddInvoice" CausesValidation="False" runat="server" ToolTip="Add New"
                                                    OnClick="lnkAddInvoice_Click">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkEditInvoice" runat="server" CausesValidation="False" ToolTip="Edit"
                                                    OnClick="lnkEditInvoice_Click">Edit</asp:LinkButton>
                                            </div>
                                            <%--<div class="btnlinks">
                                                <asp:LinkButton ID="lnkDeleteInvoice" ToolTip="Delete"
                                                    runat="server" CausesValidation="False" OnClientClick="return SelectedRowDelete('ctl00_ContentPlaceHolder1_TabContainer1_tpViewInvoicelinks_gvInvoice','Invoice');"
                                                    OnClick="lnkDeleteInvoice_Click" Visible="false">Delete</asp:LinkButton>
                                            </div>--%>

                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkInvoiceExcel" runat="server" OnClick="lnkInvoiceExcel_Click">Export to Excel</asp:LinkButton>

                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkCopyInvoice" ToolTip="Copy"
                                                    runat="server" Visible="False" CausesValidation="False"
                                                    OnClick="lnkCopyInvoice_Click">Copy</asp:LinkButton>
                                            </div>
                                            <div class="col lblsz2 lblszfloat">
                                                <div class="row">
                                                    <span class="tro trost accrd-trost">
                                                        <asp:LinkButton ID="lnkClear" runat="server" AutoPostback="false" OnClientClick="ResetValue();"
                                                            OnClick="lnkClear_Click" CausesValidation="False">Clear</asp:LinkButton>

                                                    </span>
                                                    <span class="tro trost accrd-trost">
                                                        <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click" OnClientClick="ResetShowAll();"
                                                            CausesValidation="False">Show All</asp:LinkButton>

                                                    </span>
                                                    <span class="tro trost accrd-trost">
                                                        <asp:LinkButton ID="lnkShowAllOpen" runat="server" OnClick="lnkShowAllOpen_Click" OnClientClick="ResetShowAllOpen();"
                                                            CausesValidation="False">Show All Open</asp:LinkButton>

                                                    </span>

                                                    <span class="tro trost-label accrd-trost">

                                                        <asp:Label ID="lblRecordCount" runat="server"></asp:Label>

                                                    </span>

                                                    <%--  <span class="tro trost-label accrd-trost">Balance: 
                                                    <asp:Label runat="server" ID="lblTotalRunBalance">$0.00</asp:Label>
                                                    </span>--%>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="grid_container" style="margin-top: 10px;">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">

                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock5" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                try {
                                                                    var grid = $find("<%= RadGrid_Invoices.ClientID %>");
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
                                                                try {
                                                                    requestInitiator = document.activeElement.id;
                                                                    if (document.activeElement.tagName == "INPUT") {
                                                                        selectionStart = document.activeElement.selectionStart;
                                                                    }

                                                                } catch (e) {

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

                                                            }
                                                        </script>
                                                    </telerik:RadCodeBlock>
                                                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Customer_Trans" runat="server">
                                                    </telerik:RadAjaxLoadingPanel>
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Invoices" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer_Trans" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Invoices" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" Width="100%"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" OnPreRender="RadGrid_Invoices_PreRender" PagerStyle-AlwaysVisible="true"
                                                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_Invoices_NeedDataSource" OnItemCreated="RadGrid_Invoices_ItemCreated" OnExcelMLExportRowCreated="RadGrid_Invoices_ExcelMLExportRowCreated"
                                                            OnItemDataBound="RadGrid_Invoices_ItemDataBound">
                                                            <CommandItemStyle />

                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="ref">
                                                                <Columns>

                                                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="40px" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28px" Visible="false">
                                                                    </telerik:GridClientSelectColumn>
                                                                    <%--  <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" DataField="CustName" ShowFilterIcon="false" HeaderText="Customer Name" UniqueName="CustomerName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>--%>
                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="5" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>

                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="160px" DataField="ref" SortExpression="ref" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" DataType="System.String" HeaderText="Ref No." ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <%--  <asp:Label ID="lblInv" runat="server" Text='<%# Bind("ref") %>'></asp:Label>--%>
                                                                            <asp:HyperLink ID="lblInv" runat="server" Text='<%# Bind("ref") %>' NavigateUrl='<%# Bind("Link") %>'></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                        <%--   <FooterTemplate>
                                                                            <asp:Label runat="server" ID="lblCusBal" Text="Balance :  $0.00"></asp:Label>
                                                                        </FooterTemplate>--%>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="fDate" HeaderStyle-Width="100px" SortExpression="fDate" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Date" DataType="System.String" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInvDate" runat="server" Text='<%# String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridBoundColumn DataField="CustName" HeaderText="Customer Name" HeaderStyle-Width="160px"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CustName" UniqueName="CustomerName"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn DataField="locName" SortExpression="locid" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Location Name" ShowFilterIcon="false" HeaderStyle-Width="160px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocID" runat="server" Text='<%#Eval("locName")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <%-- <telerik:GridTemplateColumn DataField="tag" SortExpression="tag" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Location" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("tag")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>--%>

                                                                    <telerik:GridTemplateColumn DataField="fdesc" SortExpression="fdesc" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Description" ShowFilterIcon="false" HeaderStyle-Width="160px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# (Eval("fdesc").ToString().Length > 50) ? (Eval("fdesc").ToString().Substring(0, 50) + "...") : Eval("fdesc")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label runat="server" Text="Total"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="amount" SortExpression="amount" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="EqualTo" HeaderText="Bill Amount" ShowFilterIcon="false" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderStyle-Width="120px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Convert.ToDouble(Eval("Amount"))==0?"":String.Format("{0:C}", Convert.ToDouble(Eval("Amount")) ) %>'
                                                                                ForeColor='<%# Convert.ToDouble(Eval("amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Credits" SortExpression="Credits" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="EqualTo" HeaderText="Credit" ShowFilterIcon="false" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderStyle-Width="120px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmountCredits" runat="server" Text='<%# Convert.ToDouble(Eval("Credits"))==0?"":String.Format("{0:C}", Convert.ToDouble(Eval("Credits")) ) %>'
                                                                                ForeColor='RED'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Balance" SortExpression="Balance" UniqueName="Balance" ShowFilterIcon="false" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="EqualTo" HeaderText="Balance" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmountBalance" runat="server" Text='<%# Convert.ToDouble(Eval("Balance"))==0?"0.00":String.Format("{0:C}", Convert.ToDouble(Eval("Balance")) ) %>'
                                                                                ForeColor='<%# Convert.ToDouble(Eval("Balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridTemplateColumn DataField="RunTotal" SortExpression="RunTotal" AutoPostBackOnFilter="true" HeaderStyle-Width="140px"
                                                                        CurrentFilterFunction="EqualTo" HeaderText="Running Total" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRunTotal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RunTotal", "{0:c}")%>'
                                                                                ForeColor='<%# Convert.ToDouble(Eval("RunTotal"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="status" SortExpression="status" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("status")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Type" SortExpression="Type" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Type" ShowFilterIcon="false" HeaderStyle-Width="100px" FilterControlWidth="50">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type")%>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnLinkTo" runat="server" Value='<%#Eval("linkTo")%>' />
                                                                            <asp:HiddenField ID="hdnTransID" runat="server" Value='<%#Eval("TransID")%>' />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                </Columns>
                                                            </MasterTableView>
                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                            </FilterMenu>
                                                        </telerik:RadGrid>

                                                    </telerik:RadAjaxPanel>

                                                </div>
                                            </div>

                                        </div>

                                        <div class="cf"></div>
                                    </div>
                                </div>
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
                                                        <asp:LinkButton ID="lnkDeleteOpp" runat="server" OnClientClick="return CheckDeleteOpp();" CausesValidation="False" OnClick="lnkDeleteOpp_Click">Delete</asp:LinkButton>
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
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                <div class="RadGrid RadGrid_Material">

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
                                                                    <telerik:GridBoundColumn DataField="name" HeaderText="Location Name"
                                                                        AutoPostBackOnFilter="true" SortExpression="name" CurrentFilterFunction="Contains" HeaderStyle-Width="120"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>
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

                                                                    <telerik:GridBoundColumn DataField="Stage" HeaderText="Stage" HeaderStyle-Width="115"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Stage"
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

                                                                    <%--<telerik:GridBoundColumn DataField="Referral" HeaderText="Referral" HeaderStyle-Width="115"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Referral"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>--%>
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
                        <li runat="server" id="adProjects">
                            <div id="accrdprojects" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Projects</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">

                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="lnkAddProject" onclick='return AddJobClick(this)' runat="server" Target="_self">Add</asp:HyperLink>
                                            </div>
                                        </div>
                                        <div class="grid_container" style="margin-top: 10px;">
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <telerik:RadCodeBlock ID="RadCodeBlock_Project" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoad() {
                                                            var grid = $find("<%= RadGrid_Project.ClientID %>");
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

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Project" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Location" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Project" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                        OnNeedDataSource="RadGrid_Project_NeedDataSource" OnPreRender="RadGrid_Project_PreRender" PagerStyle-AlwaysVisible="true"
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

                                                                <telerik:GridTemplateColumn DataField="id" DataType="System.String" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" SortExpression="id" HeaderText="Project #" HeaderStyle-Width="100" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                                                                        <asp:HyperLink ID="lnkJob" runat="server" NavigateUrl='<%# "addproject.aspx?uid=" + Eval("id") %>'
                                                                            onclick='return EditJobClick(this)' Text='<%# Bind("ID") %>'>
                                                                        </asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="tag" SortExpression="tag" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Location Name" ShowFilterIcon="false" HeaderStyle-Width="140">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("tag") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="fdesc" HeaderText="Desc" SortExpression="fdesc"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="status" HeaderText="Status" HeaderStyle-Width="120"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn DataField="fdate" HeaderText="Date Created" SortExpression="fdate" HeaderStyle-Width="120" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFDate" runat="server" Text='<%# Eval("fdate", "{0:M/d/yyyy}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="NHour" HeaderText="Hours" HeaderStyle-Width="120" FooterAggregateFormatString="{0:c}" Aggregate="Sum"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="NHour"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="NRev" HeaderText="Total Billed" HeaderStyle-Width="120" FooterAggregateFormatString="{0:c}" Aggregate="Sum"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="NRev"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="NCost" HeaderText="Total Expenses" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="NCost"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="NProfit" HeaderText="Net" HeaderStyle-Width="120" FooterAggregateFormatString="{0:c}" Aggregate="Sum"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="NProfit"
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
                    </ul>
                </div>
            </div>
        </div>
    </div>

    <telerik:RadWindowManager ID="RadWindowManagerCustomer" runat="server">
        <Windows>
            <telerik:RadWindow ID="contactWindow" Skin="Material" VisibleTitlebar="true" Title="Add Contact" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="400">
                <ContentTemplate>
                    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelContact">
                        <div style="margin-top: 15px;">
                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                                                Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="cont">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" PopupPosition="left" TargetControlID="RequiredFieldValidator12">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:TextBox ID="txtContcName" runat="server" CssClass="Contact-search" MaxLength="50"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtContcName">Contact Name</asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:TextBox ID="txtTitle" runat="server" MaxLength="50"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtTitle">Title</asp:Label>
                                        </div>
                                    </div>

                                </div>


                            </div>
                            <div class="form-section-row">

                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:TextBox ID="txtContPhone" runat="server" MaxLength="22"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtContPhone">Phone</asp:Label>
                                        </div>
                                    </div>

                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:TextBox ID="txtContFax" runat="server" MaxLength="22"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtContFax">Fax</asp:Label>
                                        </div>
                                    </div>

                                </div>


                            </div>
                            <div class="form-section-row">

                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:TextBox ID="txtContCell" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtContCell">Cell</asp:Label>
                                        </div>
                                    </div>

                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:TextBox ID="txtContEmail" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtContEmail">Email</asp:Label>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContEmail"
                                                Display="None" ErrorMessage="Invalid Email" ValidationGroup="cont" SetFocusOnError="True"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                            </asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <div class="checkrow">
                                                <asp:CheckBox ID="chkShutdownA" runat="server" />
                                                <asp:Label runat="server" AssociatedControlID="chkShutdownA" Style="top: 0 !important;">Shutdown Alert</asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div style="clear: both;"></div>

                            <footer style="float: right;">
                                <div class="btnlinks">
                                    <asp:LinkButton ID="lnkContactSave" runat="server" OnClick="lnkContactSave_Click" ValidationGroup="cont">Save</asp:LinkButton>
                                </div>
                            </footer>
                        </div>
                    </telerik:RadAjaxPanel>
                </ContentTemplate>

            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowCustomerSaved" Skin="Material" VisibleTitlebar="true" Title="Customer saved successfully" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="400">
                <ContentTemplate>
                    <div style="margin-top: 15px;">
                        <div class="form-section-row">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        Do you want to add location for the saved customer?
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>

                        <footer style="float: right;">
                            <div class="btnlinks">
                                <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Ok" OnClick="hideModalPopupViaServerConfirm_Click"
                                    CausesValidation="False" />
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowAddTickets" Skin="Material" VisibleTitlebar="true" Title="Customer saved successfully" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1040" Height="620">
                <ContentTemplate>
                    <div>
                        <iframe id="iframeCustomer" runat="server" width="1040px" height="620px" frameborder="0"></iframe>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowContactLog" Skin="Material" VisibleTitlebar="true" Title="Contact Log" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="450">
                <ContentTemplate>
                    <div class="grid_container">

                        <div class="RadGrid RadGrid_Material FormGrid">
                            <telerik:RadCodeBlock ID="RadCodeBlock7" runat="server">
                                <script type="text/javascript">
                                    function pageLoad() {
                                        try {
                                            var grid = $find("<%= RadGrid_gvContactLogs.ClientID %>");
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
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvLogs" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvContactLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                    ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True">
                                    <CommandItemStyle />
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>

                                    <MasterTableView DataKeyNames="Ref" AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="true" ShowGroupFooter="true" ShowHeader="true">

                                        <Columns>
                                            <telerik:GridTemplateColumn DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true"
                                                CurrentFilterFunction="Contains" HeaderText="Ref" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <Columns>
                                            <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldate" runat="server" Text='<%# Eval("CreatedStamp", "{0:M/d/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltime" runat="server" Text='<%# Eval("CreatedStamp","{0: hh:mm tt}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="fUser" SortExpression="fUser" AutoPostBackOnFilter="true"
                                                CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUpdBy" runat="server" Text='<%# Eval("fUser") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="Field" SortExpression="Field" AutoPostBackOnFilter="true"
                                                CurrentFilterFunction="Contains" HeaderText="Field" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblField" runat="server" Text='<%# Eval("field") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="OldVal" SortExpression="OldVal" AutoPostBackOnFilter="true"
                                                CurrentFilterFunction="Contains" HeaderText="Old Value" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOldVal" runat="server" Text='<%# Eval("OldVal") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="NewVal" SortExpression="NewVal" AutoPostBackOnFilter="true"
                                                CurrentFilterFunction="Contains" HeaderText="New Value" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNewVal" runat="server" Text='<%# Eval("NewVal") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>


                                        <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="Contact" FieldName="fDesc"></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="fDesc" SortOrder="Ascending"></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>

                                        </GroupByExpressions>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadAjaxPanel>
                        </div>


                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowHistoryTransaction" Skin="Material" VisibleTitlebar="true" Title="Payment History" CenterIfModal="true"
                Animation="Fade" AnimationDuration="100" RenderMode="Auto" VisibleStatusbar="false" Width="500px" Height="200px" ReloadOnShow="false"
                runat="server" Modal="true" ShowContentDuringLoad="false" Behaviors="Move, Close" OnClientDragEnd="setCustomPosition">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="CompanyWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="200">
                <ContentTemplate>
                    <div style="margin-top: 15px;">
                        <div class="input-field col s12">
                            <div class="row">
                                <label class="drpdwn-label">Select Company</label>
                                <asp:DropDownList ID="ddlCompanyEdit" runat="server" CssClass="browser-default">
                                </asp:DropDownList>
                            </div>
                        </div>


                        <div style="clear: both;"></div>

                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkCompanyEdit" runat="server" OnClick="btnSave_Click">Save</asp:LinkButton>
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>

            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <asp:HiddenField ID="hdnMail" runat="server" />
    <asp:HiddenField ID="hdnSageIntegration" runat="server" />
    <asp:HiddenField runat="server" ID="hdnAddeLoc" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeLoc" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteLoc" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewLoc" Value="Y" />
    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewEquipment" Value="Y" />

    <asp:HiddenField runat="server" ID="hdnEditeTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewTicket" Value="Y" />
    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnSelectedDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectedDtRangeHistory" Value="" />
    <asp:HiddenField runat="server" ID="ishowAllInvoice" Value="0" />

    <asp:HiddenField runat="server" ID="hdnAddeJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnCon" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="server">
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <%--<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.98.1/js/materialize.min.js"></script>--%>
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>

    <%--SCRIPT 1--%>
    <script type="text/javascript">
        //-----Ticket-------
        function EditTicketClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeTicket.ClientID%>').value;
            if (id == "Y") { return editticket(); } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        //--Location----
        function AddLocClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeLoc.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditLocClick(hyperlink) {
            var id = document.getElementById('<%= hdnEditeLoc.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteLocClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteLoc.ClientID%>').value;
            if (id == "Y") {

                return SelectedRowDelete('<%= RadGrid_Location.ClientID%>', 'location');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function CopyLocClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeLoc.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }


        ///-Contact permission
        function OpenAddContact() {
            $('#<%=txtContcName.ClientID%>').val("");
            $('#<%=txtTitle.ClientID%>').val("");
            $('#<%=txtContFax.ClientID%>').val("");
            $('#<%=txtContPhone.ClientID%>').val("");
            $('#<%=txtContCell.ClientID%>').val("");
            $('#<%=txtContEmail.ClientID%>').val("");
            $('#<%=chkShutdownA.ClientID%>').prop("checked", false);

            var wnd = $find('<%=contactWindow.ClientID %>');

            wnd.set_title("Add Contact");
            wnd.Show();
        }
        function OpenEditContact() {
            var ID = "";
            var txtContcName = "";
            var txtTitle = "";
            var txtContPhone = "";
            var txtContCell = "";
            var txtContFax = "";
            var txtContEmail = "";
            var chkShutdownA = false;

            $("#<%=RadGrid_gvContacts.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                    ID = $tr.find('span[id*=hdnID]').text();
                    txtContcName = $tr.find('input[id*=lblPageId]').text();
                    txtTitle = $tr.find('input[id*=txtTitle]').text();
                    txtContPhone = $tr.find('input[id*=txtContPhone]').text();
                    txtContCell = $tr.find('input[id*=txtContCell]').text();
                    txtContFax = $tr.find('input[id*=txtContFax]').text();
                    txtContEmail = $tr.find('input[id*=txtContEmail]').text();
                    chkShutdownA = $tr.find('input[id*=txtContEmail]').checked();
                });
            });
            if (ID != "") {
                $('#<%=txtContcName.ClientID%>').val(txtContcName);
                $('#<%=txtTitle.ClientID%>').val(txtTitle);
                $('#<%=txtContFax.ClientID%>').val(txtContFax);
                $('#<%=txtContPhone.ClientID%>').val(txtContPhone);
                $('#<%=txtContCell.ClientID%>').val(txtContCell);
                $('#<%=txtContEmail.ClientID%>').val(txtContEmail);
             //   $('#<%=chkShutdownA.ClientID%>').checked = chkShutdownA;
                $('#<%=chkShutdownA.ClientID%>').prop("checked", chkShutdownA);

                var wnd = $find('<%=contactWindow.ClientID %>');
                wnd.set_title("Edit Contact");
                wnd.Show();

                Materialize.updateTextFields();
            }
            else {
                ChkWarning();
            }
        }
        function AddContactClick(hyperlink) {
            //debugger;
            var IsAdd = document.getElementById('<%= hdnAddeContact.ClientID%>').value;
            if (IsAdd == "Y") {
                OpenAddContact()
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });

            }
        }

        function EditContactClick(hyperlink) {

            var IsEdit = document.getElementById('<%= hdnEditeContact.ClientID%>').value;
            if (IsEdit == "Y") {
                OpenEditContact()
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });

            }
        }

        function DeleteContactClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteContact.ClientID%>').value;
            if (IsDelete == "Y") {
                return confirm("Are you sure you want to delete this?");
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }


        ///-Document permission

        function AddDocumentClick(hyperlink) {
            var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd == "Y") {
                ConfirmUpload(ctl00_ContentPlaceHolder1_FileUpload1.value)
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

    </script>

    <%--SCRIPT 2--%>
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
        function CopyEquipmentClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeEquipment.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
    </script>

    <%--SCRIPT 3--%>
    <script type="text/javascript">
       <%-- var pac_input = document.getElementById('<%= txtGoogleAutoc.ClientID %>');
        debugger
        (function pacSelectFirst(input) {
            // store the original event binding function
            var _addEventListener = (input.addEventListener) ? input.addEventListener : input.attachEvent;
            debugger
            function addEventListenerWrapper(type, listener) {
                // Simulate a 'down arrow' keypress on hitting 'return' when no pac suggestion is selected,
                // and then trigger the original listener.
                debugger
                if (type == "keydown") {
                    var orig_listener = listener;
                    listener = function (event) {
                        var suggestion_selected = $(".pac-item-selected").length > 0;
                        if (event.which == 13 && !suggestion_selected) {
                            var simulated_downarrow = $.Event("keydown", { keyCode: 40, which: 40 })
                            orig_listener.apply(input, [simulated_downarrow]);
                        }

                        orig_listener.apply(input, [event]);
                    };
                }

                // add the modified listener
                _addEventListener.apply(input, [type, listener]);
            }

            if (input.addEventListener)
                input.addEventListener = addEventListenerWrapper;
            else if (input.attachEvent)
                input.attachEvent = addEventListenerWrapper;
            //input.getPlace();
            debugger
        })(pac_input);--%>




        $(document).ready(function () {
            $(function () {
                $("#<%= txtGoogleAutoc.ClientID %>").geocomplete({
                    map: false,
                    details: "#divmain",
                    types: ["geocode", "establishment"],
                    address:"#<%= txtGoogleAutoc.ClientID %>",
                    city: "#<%= txtCity.ClientID %>",
                    state: "#<%= txtState.ClientID %>",
                    zip: "#<%= txtZip.ClientID %>",
                    lat: "#<%= lat.ClientID %>",
                    lng: "#<%= lng.ClientID %>"
                }).bind("geocode:result", function (event, result) {
                    //debugger
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


                    $("#<%=ddlCountry.ClientID%>").val(getCountry);
                    $("#<%=txtState.ClientID%>").val(cityAlt);
                    $("#<%=txtZip.ClientID%>").val(countryCode);
                    $("#<%=txtCity.ClientID%>").val(city);

                    Materialize.updateTextFields();

                }).bind("set");

                initialize();
            });
            $(function () {
                var autocomplete = new google.maps.places.Autocomplete(pac_input);
                debugger
            });

            ///////////// Quick Codes //////////////
            $("#<%=txtRemarks.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRemarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

            $("#<%=txtAlert.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtAlert.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

            $("#mapLink").click(function () {
                $("#map").toggle();
                initialize();
            });

            
        });


        function initialize() {
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
    </script>

    <%--SCRIPT 4--%>
    <script type="text/javascript">

        function onDdlBillingChange(val) {
            if (val == 1) {

                $("#divCentral").show();
                var countLocations = document.getElementById("<%=ddlSpecifiedLocation.ClientID%>").length
                if ((countLocations - 1) <= 0) {

                    noty({
                        text: 'You cannot select the Combined Billing, As there are No Locations added yet.',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                }

            }
            else {
                $("#divCentral").hide();
            }
        }
        function ValidateSpecifyLocation() {
            //alert("heloo");
            var countLocations = document.getElementById("<%=ddlSpecifiedLocation.ClientID%>").length
            if ((countLocations - 1) <= 0) {

                noty({
                    text: 'You cannot select the Combined Billing, As there are No Locations added yet.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
            }
        }
        function AddNewCustomer() {
            $('#setuppopup').modal('show');
        }

        function checked1() {
            if (document.getElementById('<%= CopyToLocAndJob.ClientID %>').checked) {
                alert("Copy above Billing Rates to Location/Project ?");
            }
        }

        if ($("#divInv").is(':visible')) {
            Materialize.updateTextFields();
            var SearchValue = $("#<%=hdnSearchValue.ClientID %>").val();

            var SearchBy = $("#<%=hdnSearchBy.ClientID %>").val();

            if (SearchValue == "rdAll") {
                document.getElementById('rdAll').checked = true;

            }
            else if (SearchValue == "rdOpen") {
                document.getElementById('rdOpen').checked = true;

            }
            else if (SearchValue == "rdClosed") {
                document.getElementById('rdClosed').checked = true;
            }
            else {
                document.getElementById('rdAll').checked = true;
            }
            if (SearchBy == "rdAll2") {
                document.getElementById('rdAll2').checked = true;

            }
            else if (SearchBy == "rdCharges") {
                document.getElementById('rdCharges').checked = true;

            }
            else if (SearchBy == "rdCredit") {
                document.getElementById('rdCredit').checked = true;
            }
            else {
                document.getElementById('rdAll2').checked = true;
            }
        }
        function showModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.show();
        }

        function showModalPopupViaClientCust(lblTicketId, lblComp) {
                //            ev.preventDefault();
                //            document.getElementById('<%= iframeCustomer.ClientID %>').width = "1024px";
            document.getElementById('<%= iframeCustomer.ClientID %>').src = "addticket.aspx?id=" + lblTicketId + "&comp=" + lblComp;;
          <%--  document.getElementById('<%= Panel2.ClientID %>').style.display = "none";--%>
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.show();
        }

        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            //debugger;
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }

        function hideModalPopupViaClientCust(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.hide();
        }

        ////////////////// Confirm Document Upload ////////////////////
     <%--   function ConfirmUpload(value) {
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
        }--%>
        function ConfirmUpload(value) {
            if (confirm('Are you sure you want to upload?')) { document.getElementById('<%= lnkUploadDoc.ClientID %>').click(); }
              else { document.getElementById('<%= lnkPostback.ClientID %>').click(); }
          }

        function checkdelete() {
            return SelectedRowDelete('<%= RadGrid_Documents.ClientID %>', 'file');
        }

        function editticket() {
            $("#<%=RadGrid_OpenCalls.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                    var ticket = $tr.find('span[id*=lblTicketId]').text();
                    var comp = $tr.find('input[id*=hdnComp]').val();
                    var url = "addticket.aspx?id=" + ticket + "&comp=" + comp;
                    window.open(url, '_blank');
                    return false;
                });
            });
        }

        function AlertSageIDUpdate() {
            var str = validateBilling()
            if (str != '') {
                noty({
                    text: str,
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 4000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            } else {
                var hdnAcctID = document.getElementById('<%= hdnAcctID.ClientID %>');
                var txtAcctno = document.getElementById('<%= txtAcctno.ClientID %>');
                var hdnSageIntegration = document.getElementById('<%= hdnSageIntegration.ClientID %>');
                var ret = true;
                if (hdnSageIntegration.value == "1") {
                    if ('<%=ViewState["mode"].ToString()  %>' == "1") {
                        if (hdnAcctID.value != txtAcctno.value) {
                            ret = confirm('Sage ID edited, this will create a new Customer in Sage and make existing inactive. Do you want to continue?');
                        }
                    }
                }
                return ret;
            }

        }

        function checkMaxLength(textarea, evt, maxLength) {
            if ($("#<%= hdnSageIntegration.ClientID %>").val() == "1") {
                var lines = textarea.value.split("\n");
                for (var i = 0; i < lines.length; i++) {
                    if (lines[i].length <= 30) continue;
                    var j = 0; space = 30;
                    while (j++ <= 30) {
                        if (lines[i].charAt(j) === " ") space = j;
                    }
                    lines[i + 1] = lines[i].substring(space + 1) + (lines[i + 1] || "");
                    lines[i] = lines[i].substring(0, space);
                }
                textarea.value = lines.slice(0, 4).join("\n");
                $("#spnAddress").fadeIn('slow', function () {
                    $(this).delay(500).fadeOut('slow');
                });
            }
        }

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
        function ConvertDigit(obj) {

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                //document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        $(document).ready(function () {
            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);

            if ($("#ctl00_ContentPlaceHolder1_chkInternet").is(":checked")) {
                $("#divInternet").show();
            } else {
                $("#divInternet").hide();
            }

            $("#ctl00_ContentPlaceHolder1_chkInternet").click(function () {
                if ($(this).is(":checked")) {
                    $("#divInternet").show();
                } else {
                    $("#divInternet").hide();
                }
            });
            Materialize.updateTextFields();
            var billing = $('#<%=ddlBilling.ClientID%>').val();
            if (billing == 1) {

                $("#divCentral").show();
            }

        });
    </script>

    <%--SCRIPT 5--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('a[href^="#accrd"]').on('click', function (e) {
                e.preventDefault();

                var target = this.hash;
                var $target = $(target);
                if ($(target).hasClass('active') || $(target) == "") {

                }
                else {
                    $(target).click();
                }
                $('#<%=btnSearch.ClientID%>').focus();

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

            $(".dropdown-content.select-dropdown li").on("click", function () {
                var that = this;
                setTimeout(function () {
                    if ($(that).parent().hasClass('active')) {
                        $(that).parent().removeClass('active');
                        $(that).parent().hide();
                    }
                }, 100);
            });

            $('#ctl00_ContentPlaceHolder1_txtPhoneCust').mask("(999) 999-9999? Ext 99999");
            $('#ctl00_ContentPlaceHolder1_txtPhoneCust').bind('paste', function () { $(this).val(''); });
            $('#ctl00_ContentPlaceHolder1_txtCell').mask("(999) 999-9999");
            $('#ctl00_ContentPlaceHolder1_txtFax').mask("(999) 999-9999");

            $('#ctl00_ContentPlaceHolder1_contactWindow_C_txtContPhone').mask("(999) 999-9999? Ext 99999");
            $('#ctl00_ContentPlaceHolder1_contactWindow_C_txtContPhone').bind('paste', function () { $(this).val(''); });
            $('#ctl00_ContentPlaceHolder1_contactWindow_C_txtContCell').mask("(999) 999-9999");
            $('#ctl00_ContentPlaceHolder1_contactWindow_C_txtContFax').mask("(999) 999-9999");

        });
        function dec_date(select, txtDateTo, txtDateFrom, rdGroup) {
            var select = select;
            var txtDateTo = txtDateTo;
            var txtDateFrom = txtDateFrom;
            var rdGroup = rdGroup;
            var xday;
            var xWeek;
            var xMonth;
            var xYear;
            var xQuarter;
            if (select == "dec") {
                xday = -1;
                xWeek = -7;
                xMonth = -1;
                xQuarter = -3;
                xYear = -1;
            }
            if (select == "inc") {

                xday = 1;
                xWeek = 7;
                xMonth = 1;
                xQuarter = 3;
                xYear = 1;
            }
            var radio = document.getElementsByName(rdGroup); //Client ID of the RadioButtonList1 
            var selected;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) { // Checked property to check radio Button check or not
                    //alert("Radio button having value " + radio[i].value + " was checked."); // Show the checked value
                    selected = radio[i].value;

                }
                if (selected == "") {
                    selected = 'rdWeek';
                }

            }


            if (selected == 'rdDay') {

                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xday);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xday);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;


            }
            else if (selected == 'rdWeek') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xWeek);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xWeek);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdMonth') {
                //dec the from date

                Date.isLeapYear = function (year) {
                    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
                };

                Date.getDaysInMonth = function (year, month) {
                    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
                };

                Date.prototype.isLeapYear = function () {
                    return Date.isLeapYear(this.getFullYear());
                };

                Date.prototype.getDaysInMonth = function () {
                    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
                };

                Date.prototype.addMonths = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + value);
                    this.setDate(Math.min(n, this.getDaysInMonth()));
                    return this;
                };


                Date.prototype.addMonthsLast = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };

                Date.prototype.addMonthsLastDec = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() - 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };


                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt).toDateString();
                var newdate = new Date(date);

                newdate.addMonths(xMonth);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;


                //dec the to date 
                if (select == 'dec') {
                    var ti = document.getElementById(txtDateTo).value;

                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);

                    newdateti.addMonthsLastDec(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateTo).value = someFormattedDate;
                }

                else {
                    var ti = document.getElementById(txtDateTo).value;


                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);

                    newdateti.addMonthsLast(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateTo).value = someFormattedDate;
                }
            }


            else if (selected == 'rdQuarter') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setMonth(newdate.getMonth() + xQuarter);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                //decrease date range 
                if (select == 'dec') {
                    xQuarter = -3;

                    if (DATE.getMonth() == 11) {
                        NEWDATE.setDate(NEWDATE.getDate() - 1);
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);

                    }
                    else if (DATE.getMonth() == 5) {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                        NEWDATE.setDate(NEWDATE.getDate() + 1);

                    }
                    else {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);

                    }

                }
                else {
                    xQuarter = 3;
                    NEWDATE.setDate(NEWDATE.getDate() - 1);
                    NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    if (NEWDATE.getMonth() == 11 || NEWDATE.getMonth() == 12 || DATE.getMonth() == 11) {
                        NEWDATE.setDate(31);
                    } else {
                        if (DATE.getMonth() == 5) { NEWDATE.setDate(NEWDATE.getDate() + 1); }
                        else { NEWDATE.setDate(NEWDATE.getDate()); }

                    }
                }

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdYear') {

                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setFullYear(newdate.getFullYear() + xYear);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setFullYear(NEWDATE.getFullYear() + xYear);
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }

            return false;
        }


        function SelectDate(type, txtDateFrom, txtdateTo, label, UniqueVal, rdGroup) {
            var type = type;
            var txtDateFrom = txtDateFrom;
            var txtdateTo = txtdateTo;
            var UniqueVal = UniqueVal;
            var label = label;


            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = datestring;
                document.getElementById(txtDateFrom).value = datestring;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value = "Day";
            }
            if (type == 'Week') {

                Date.prototype.GetFirstDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay())));
                }

                Date.prototype.GetLastDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay() + 6)));
                }

                var today = new Date();
                var Firstdate = today.GetFirstDayOfWeek();
                var day = Firstdate.getDate();
                var month = Firstdate.getMonth() + 1;
                var year = Firstdate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;

                $(label).addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value = "Week";


            }
            if (type == 'Month') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfMonth = new Date(y, m, 1);
                var lastDayOfMonth = new Date(y, m + 1, 0);

                var day = FirstDayOfMonth.getDate();
                var month = FirstDayOfMonth.getMonth() + 1;
                var year = FirstDayOfMonth.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;

                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value = "Month";
            }
            if (type == 'Quarter') {
                var d = new Date();
                var quarter = Math.floor((d.getMonth() / 3));

                var firstDate = new Date(d.getFullYear(), quarter * 3, 1);
                var lastDate = new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0);
                var day = firstDate.getDate();
                var month = firstDate.getMonth() + 1;
                var year = firstDate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;

                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value = "Quarter";

            }
            if (type == 'Year') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfYear = new Date(y, 1, 1);
                var lastDayOfYear = new Date(y, 11, 31);

                var day = FirstDayOfYear.getDate();
                var month = FirstDayOfYear.getMonth();
                var year = FirstDayOfYear.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;

                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value);

            }
        }


    </script>

    <%--SCRIPT 6--%>

    <script type="text/javascript">
        $(document).ready(function () {


            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });

            if (typeof (Storage) !== "undefined") {
                // Retrieve
                var val;

                val = localStorage.getItem("hdnInvDate");
                if (val == 'Day') {
                    $("#<%=lblDay.ClientID%>").addClass("labelactive");
                    document.getElementById("rdDay").checked = true;
                }
                else if (val == 'Week') {
                    $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                    document.getElementById("rdWeek").checked = true;
                }
                else if (val == 'Month') {
                    $("#<%=lblMonth.ClientID%>").addClass("labelactive");
                    document.getElementById("rdMonth").checked = true;
                }
                else if (val == 'Quarter') {
                    $("#<%=lblQuarter.ClientID%>").addClass("labelactive");
                    document.getElementById("rdQuarter").checked = true;
                }
                else if (val == 'Year') {
                    $("#<%=lblYear.ClientID%>").addClass("labelactive");
                    document.getElementById("rdYear").checked = true;
                }
                else {
                    $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                    document.getElementById("rdWeek").checked = true;
                }

                var valSH;
                valSH = localStorage.getItem("hdnSHDate");
                valSH = document.getElementById('<%=hdnSelectedDtRangeHistory.ClientID%>').value;
                if (valSH == "") {
                    valSH = "rdWeek";
                }

                if (valSH == 'rdDay') {
                    $("#<%=SHlblDay.ClientID%>").addClass("labelactive");
                    document.getElementById("SHrdDay").checked = true;
                }
                else if (valSH == 'rdWeek') {
                    $("#<%=SHlblWeek.ClientID%>").addClass("labelactive");
                    document.getElementById("SHrdWeek").checked = true;
                }
                else if (valSH == 'rdMonth') {
                    $("#<%=SHlblMonth.ClientID%>").addClass("labelactive");
                    document.getElementById("SHrdMonth").checked = true;
                }
                else if (valSH == 'rdQuarter') {
                    $("#<%=SHlblQuarter.ClientID%>").addClass("labelactive");
                    document.getElementById("SHrdQuarter").checked = true;
                }
                else if (valSH == 'rdYear') {
                    $("#<%=SHlblYear.ClientID%>").addClass("labelactive");
                    document.getElementById("SHrdYear").checked = true;
                }
                else {
                    $("#<%=SHlblWeek.ClientID%>").addClass("labelactive");
                    document.getElementById("SHrdWeek").checked = true;
                }
            }

            function clickLink(linkName) {
                setTimeout(function () {
                    $(linkName).click();
                }, 1000);
            }

            var tab = GetQueryStringParams('tab');

            if (tab == "equip") {
                clickLink("#lnkEquipmentaccrd");
            }
            else if (tab == "inv") {
                clickLink("#lnkTransactionaccrd");
            }
            else if (tab == "loc") {
                clickLink("#lnkLocationaccrd");
            }
            else {
                clickLink("#lnkCustomeraccrd");
            }

            var tabBack = GetQueryStringParams('f');
            if (tabBack == "r") {
                clickLink("#lnkSHaccrd");
                var $target = $("#accrdserviceHistory");
                if ($target.hasClass('active') || $target == "") {
                    $target.click();
                }
                else {
                    $target.click();
                }
                $('html, body').animate({ scrollTop: $target.offset().top }, 'slow');
            }
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

    </script>

    <%--SCRIPT 7--%>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {
            $("[id*=txtContPhone]").mask("(999) 999-9999? Ext 99999");
            $("[id*=txtContPhone]").bind('paste', function () { $(this).val(''); });
            $("[id*=txtContCell]").mask("(999) 999-9999");
            $("[id*=txtContFax]").mask("(999) 999-9999");
        });
        function OpenCompanyPopUp() {
            var wnd = $find('<%=CompanyWindow.ClientID %>');
            wnd.set_title("Select Company");
            wnd.Show();
        }
        function CloseCompanyPopUp() {
            var wnd = $find('<%=CompanyWindow.ClientID %>');
            wnd.Close();
        }
        function OpenContactPopupEdit() {
            var clickEditContact = document.getElementById("<%= btnEdit.ClientID %>");
            clickEditContact.click();
        }
        function ResetValue() {
            document.getElementById('rdAll2').checked = true;
            document.getElementById('rdAll').checked = true;
            if (typeof (Storage) !== "undefined") {

                // Retrieve
                var val;

                val = localStorage.getItem("hdnInvDate");
                if (val == 'Day') {

                    $("#<%=lblDay.ClientID%>").addClass("labelactive");
                    document.getElementById("rdDay").checked = true;

                }
                else if (val == 'Week') {

                    $("#<%=lblWeek.ClientID%>").addClass("labelactive");

                    document.getElementById("rdWeek").checked = true;

                }
                else if (val == 'Month') {
                    $("#<%=lblMonth.ClientID%>").addClass("labelactive");

                    document.getElementById("rdMonth").checked = true;

                }
                else if (val == 'Quarter') {

                    $("#<%=lblQuarter.ClientID%>").addClass("labelactive");
                    document.getElementById("rdQuarter").checked = true;


                }
                else if (val == 'Year') {

                    $("#<%=lblYear.ClientID%>").addClass("labelactive");
                    document.getElementById("rdYear").checked = true;

                }

            }
        }

    </script>
    <script>
        function dec_dateHistory(select, rdGroup) {
            // debugger;
            var select = select;
            var rdGroup = rdGroup;
            var xday;
            var xWeek;
            var xMonth;
            var xYear;
            var xQuarter;
            if (select == "dec") {
                xday = -1;
                xWeek = -7;
                xMonth = -1;
                xQuarter = -3;
                xYear = -1;
            }
            if (select == "inc") {

                xday = 1;
                xWeek = 7;
                xMonth = 1;
                xQuarter = 3;
                xYear = 1;
            }
            var radio = document.getElementsByName(rdGroup); //Client ID of the RadioButtonList1 
            var selected;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) { // Checked property to check radio Button check or not
                    //alert("Radio button having value " + radio[i].value + " was checked."); // Show the checked value
                    selected = radio[i].value;

                }
                if (selected == "") {
                    selected = 'rdWeek';
                }

            }
            selected = document.getElementById('<%=hdnSelectedDtRangeHistory.ClientID%>').value;

            if (selected == 'rdDay') {

                //dec the from date 
                var tt = document.getElementById('<%=txtToDate.ClientID%>').value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xday);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%=txtfromDate.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%=txtToDate.ClientID%>').value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xday);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById('<%=txtToDate.ClientID%>').value = someFormattedDATE;


            }
            else if (selected == 'rdWeek') {
                //dec the from date 
                var tt = document.getElementById('<%=txtfromDate.ClientID%>').value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xWeek);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%=txtfromDate.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%=txtToDate.ClientID%>').value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xWeek);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById('<%=txtToDate.ClientID%>').value = someFormattedDATE;
            }
            else if (selected == 'rdMonth') {
                //dec the from date

                Date.isLeapYear = function (year) {
                    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
                };

                Date.getDaysInMonth = function (year, month) {
                    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
                };

                Date.prototype.isLeapYear = function () {
                    return Date.isLeapYear(this.getFullYear());
                };

                Date.prototype.getDaysInMonth = function () {
                    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
                };

                Date.prototype.addMonths = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + value);
                    this.setDate(Math.min(n, this.getDaysInMonth()));
                    return this;
                };


                Date.prototype.addMonthsLast = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };

                Date.prototype.addMonthsLastDec = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() - 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };


                var tt = document.getElementById('<%=txtfromDate.ClientID%>').value;

                var date = new Date(tt).toDateString();
                var newdate = new Date(date);

                newdate.addMonths(xMonth);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%=txtfromDate.ClientID%>').value = someFormattedDate;


                //dec the to date 
                if (select == 'dec') {
                    var ti = document.getElementById('<%=txtToDate.ClientID%>').value;


                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLastDec(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById('<%=txtToDate.ClientID%>').value = someFormattedDate;
                }

                else {
                    var ti = document.getElementById('<%=txtToDate.ClientID%>').value;
                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLast(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById('<%=txtToDate.ClientID%>').value = someFormattedDate;
                }

            }


            else if (selected == 'rdQuarter') {
                //dec the from date
                var tt = document.getElementById('<%=txtfromDate.ClientID%>').value;
                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setMonth(newdate.getMonth() + xQuarter);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%=txtfromDate.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%=txtToDate.ClientID%>').value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                //decrease date range 
                if (select == 'dec') {
                    xQuarter = -3;

                    if (DATE.getMonth() == 11) {
                        NEWDATE.setDate(NEWDATE.getDate() - 1);
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    }
                    else if (DATE.getMonth() == 5) {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                        NEWDATE.setDate(NEWDATE.getDate() + 1);
                    }
                    else {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    }
                }
                else {
                    xQuarter = 3;
                    NEWDATE.setDate(NEWDATE.getDate() - 1);
                    NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    if (NEWDATE.getMonth() == 11 || NEWDATE.getMonth() == 12 || DATE.getMonth() == 11) {
                        NEWDATE.setDate(31);
                    } else {
                        if (DATE.getMonth() == 5) { NEWDATE.setDate(NEWDATE.getDate() + 1); }
                        else { NEWDATE.setDate(NEWDATE.getDate()); }

                    }
                }

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();
                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById('<%=txtToDate.ClientID%>').value = someFormattedDATE;
            }
            else if (selected == 'rdYear') {

                var tt = document.getElementById('<%=txtfromDate.ClientID%>').value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setFullYear(newdate.getFullYear() + xYear);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%=txtfromDate.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%=txtToDate.ClientID%>').value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);




                NEWDATE.setFullYear(NEWDATE.getFullYear() + xYear);
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById('<%=txtToDate.ClientID%>').value = someFormattedDATE;


            }

            return false;

        }
        function SelectDateHistory(type, UniqueVal) {
            var type = type;
            var UniqueVal = UniqueVal;


            $('#<%=SHlblDay.ClientID%>').removeClass("labelactive");
            $('#<%=SHlblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=SHlblMonth.ClientID%>').removeClass("labelactive");
            $('#<%=SHlblQuarter.ClientID%>').removeClass("labelactive");
            $('#<%=SHlblYear.ClientID%>').removeClass("labelactive");
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById('<%=txtToDate.ClientID%>').value = datestring;
                document.getElementById('<%=txtfromDate.ClientID%>').value = datestring;
                $('#<%=SHlblDay.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRangeHistory.ClientID%>').value = "rdDay";

            }
            if (type == 'Week') {

                Date.prototype.GetFirstDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay())));
                }

                Date.prototype.GetLastDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay() + 6)));
                }

                var today = new Date();
                var Firstdate = today.GetFirstDayOfWeek();
                var day = Firstdate.getDate();
                var month = Firstdate.getMonth() + 1;
                var year = Firstdate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById('<%=txtfromDate.ClientID%>').value = datestring;
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById('<%=txtToDate.ClientID%>').value = dateString;
                $('#<%=SHlblWeek.ClientID%>').addClass("labelactive");

                document.getElementById('<%= hdnSelectedDtRangeHistory.ClientID%>').value = "rdWeek";

            }
            if (type == 'Month') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfMonth = new Date(y, m, 1);
                var lastDayOfMonth = new Date(y, m + 1, 0);

                var day = FirstDayOfMonth.getDate();
                var month = FirstDayOfMonth.getMonth() + 1;
                var year = FirstDayOfMonth.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById('<%=txtfromDate.ClientID%>').value = datestring;

                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById('<%=txtToDate.ClientID%>').value = dateString;

                $('#<%=SHlblMonth.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRangeHistory.ClientID%>').value = "rdMonth";

            }
            if (type == 'Quarter') {
                var d = new Date();
                var quarter = Math.floor((d.getMonth() / 3));

                var firstDate = new Date(d.getFullYear(), quarter * 3, 1);
                var lastDate = new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0);
                var day = firstDate.getDate();
                var month = firstDate.getMonth() + 1;
                var year = firstDate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById('<%=txtfromDate.ClientID%>').value = datestring;

                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById('<%=txtToDate.ClientID%>').value = dateString;
                $('#<%=SHlblQuarter.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRangeHistory.ClientID%>').value = "rdQuarter";

            }
            if (type == 'Year') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfYear = new Date(y, 1, 1);
                var lastDayOfYear = new Date(y, 11, 31);

                var day = FirstDayOfYear.getDate();
                var month = FirstDayOfYear.getMonth();
                var year = FirstDayOfYear.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById('<%=txtfromDate.ClientID%>').value = datestring;

                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById('<%=txtToDate.ClientID%>').value = dateString;
                $('#<%=SHlblYear.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRangeHistory.ClientID%>').value = "rdYear";


            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnSelectedDtRangeHistory.ClientID%>').value);

            }
        }

        function ShowHistoryTransactionPopup(Uid, Type, owner, loc, status, TransID) {

            var oWnd = $find("<%=RadWindowHistoryTransaction.ClientID%>");
            oWnd.setUrl("HistoryTransaction.aspx?uid=" + Uid + "&type=" + Type + "&owner=" + owner + "&loc=" + loc + "&status=" + status + "&tid=" + TransID + "&page=addcustomer");
            oWnd.setSize(800, 400);
            oWnd.show();
        }
        function setCustomPosition(sender, args) {

            var elmnt = document.getElementById("<%=RadGrid_Invoices.ClientID%>");

            sender.moveTo(sender.get_left(), elmnt.offsetTop);
        }

        function validateBilling() {
            var txtBillRate = $('#<%=txtBillRate.ClientID%>').val();
            var txtNt = $('#<%=txtNt.ClientID%>').val();
            var txtOt = $('#<%=txtOt.ClientID%>').val();
            var txtDt = $('#<%=txtDt.ClientID%>').val();
            var txtTravel = $('#<%=txtTravel.ClientID%>').val();
            var txtMileage = $('#<%=txtMileage.ClientID%>').val();


            if (txtBillRate != '' && parseFloat(txtBillRate) < 0) {
                return 'Billing rate amount must be a positive number. Please correct to proceed.'
            }
            if (txtNt != '' && parseFloat(txtNt) < 0) {
                return '1.7 Rate amount must be a positive number. Please correct to proceed.'
            }
            if (txtOt != '' && parseFloat(txtOt) < 0) {
                return 'OT Rate amount must be a positive number. Please correct to proceed.'
            }
            if (txtDt != '' && parseFloat(txtDt) < 0) {
                return 'DT Rate amount must be a positive number. Please correct to proceed.'
            }
            if (txtTravel != '' && parseFloat(txtTravel) < 0) {
                return 'Travel Rate amount must be a positive number. Please correct to proceed.'
            }
            if (txtMileage != '' && parseFloat(txtMileage) < 0) {
                return 'Mileage amount must be a positive number. Please correct to proceed.'
            }
            return '';

        }
    </script>

    <%--SCRIPT 8--%>
    <script type="text/javascript">
        $("#<%=ddlCustStatus.ClientID%>").change(function () {
            debugger;
            if ($('option:selected', $(this)).text() == 'Inactive') {
                var isAllowInactive = "0";
                if (isAllowInactive) {
                    if (!confirm('Please note making the customer inactive will set all location, projects , open tickets and equipment to inactive. Would you like to proceed?')) {
                        $("#<%=ddlCustStatus.ClientID%>").val("0");
                    }
                } else {
                    alert("The Customer cannot be made inactive since there are open tickets for this Customer.");
                    $("#<%=ddlCustStatus.ClientID%>").val("0");
                }
            }
        });
    </script>


</asp:Content>

