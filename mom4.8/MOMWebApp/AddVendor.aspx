<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddVendor" CodeBehind="AddVendor.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <style>
        .spanStyle {
            color: grey;
            font-weight: normal;
            font-size: 14px;
            font-family: sans-serif;
            text-align: center;
            align-self: center;
        }

        .form-control1, .form-control_2.input-sm {
            border: 1px solid #e0e0e0;
            padding: 5px;
            color: #808080;
            background: #fff;
            width: 100%;
            font-size: small;
            font-weight: 300;
            font-family: sans-serif;
            border-radius: 2px !important;
            -webkit-appearance: none;
            outline: none;
            height: 28px;
            display: initial !important;
        }

        .form-control {
            display: initial !important;
        }

        .btn {
            border: 1px solid #0086b3;
            font-weight: 700;
            letter-spacing: 1px;
            outline: ridge;
            height: 28px;
            margin-top: -4px !important;
        }

            .btn:focus, .btn:active:focus, .btn.active:focus {
                outline: ridge;
            }

        .btn-primary {
            background: #0099cc !important;
            border-color: #208eb3 !important;
            /*color: #ffffff !important;*/
            /*border-right-color:#007399!important;
            border-right:thin!important;*/
            padding: 3px 10px !important;
            /*box-shadow: 0 3px 0 0 #4380b5;*/
        }

            .btn-primary:hover, .btn-primary:focus, .btn-primary:active, .btn-primary.active, .open > .dropdown-toggle.btn-primary {
                background: #33a6cc !important;
                height: 28px;
            }

            .btn-primary:active, .btn-primary.active {
                background: #007299 !important;
                box-shadow: none;
                height: 28px;
            }

        .istyle {
            padding-right: 0px;
            margin-right: -1px;
            margin-top: -4px !important;
            padding: 2px !important;
            margin-left: -1px !important;
        }

        .checkbox-custom, .radio-custom {
            opacity: 0;
            position: absolute;
        }

        .checkbox-custom, .checkbox-custom-label, .radio-custom, .radio-custom-label {
            display: inline-block;
            vertical-align: middle;
            margin: 5px;
            cursor: pointer;
        }

        .checkbox-custom-label, .radio-custom-label {
            position: relative;
        }

        .checkbox-custom + .checkbox-custom-label:before, .radio-custom + .radio-custom-label:before {
            content: '';
            background: #fff;
            border: 2px solid #3EAFE6;
            display: inline-block;
            vertical-align: middle;
            width: 20px;
            height: 20px;
            padding: 2px;
            margin-right: 10px;
            text-align: center;
        }

        .checkbox-custom:checked + .checkbox-custom-label:before {
            background: #3EAFE6;
        }

        .radio-custom + .radio-custom-label:before {
            border-radius: 50%;
        }

        .radio-custom:checked + .radio-custom-label:before {
            background: #3EAFE6;
        }


        .checkbox-custom:focus + .checkbox-custom-label, .radio-custom:focus + .radio-custom-label {
            outline: 1px solid #ddd;
            / focus style /;
        }

        .rd-flt {
            float: left;
            margin-right: 20px;
            margin-bottom: 5px;
        }

        .trost-label2 a {
            color: #1565C0 !important;
        }
        [id$='RadGrid_VendorTran'] .rgHeader > a {
            white-space: nowrap;
            padding-left: 0 !important;
        }
        
        
    </style>
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
   
    <script>
        $(document).ready(function () {
            $(function () {
                $("#<%=txtAddress.ClientID %>").geocomplete({
                    map: false,
                    details: "#divmain",
                    types: ["geocode", "establishment"],
                    address_components: "#<%= txtAddress.ClientID %>",
                    city: "#<%= txtCity.ClientID %>",
                    state: "#<%= txtState.ClientID %>",
                    zip: "#<%= txtZip.ClientID %>"
                    

                }).bind("geocode:result", function (event, result) {

                    var countryCode = "", city = "", cityAlt = "",getCountry = "";                   
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
                     if (cityAlt.length > 2)
                         for (var i = 0; i < result.address_components.length; i++) {                       
                        var addr = result.address_components[i];                      
                             if (addr.types[0] == 'administrative_area_level_2')
                                 cityAlt = addr.short_name;                                            
                    }
                    $("#<%=txtCountry.ClientID%>").val(getCountry);                  
                    $("#<%=txtZip.ClientID%>").val(countryCode);
                    $("#<%=txtState.ClientID%>").val(cityAlt);
                      $("#<%=txtCity.ClientID%>").val(city);
                    Materialize.updateTextFields();
                   // document.getElementById('<%=btnSelectAdd.ClientID%>').click();
                    });
                initialize();
            });
        });
        function initialize() {
            try {
                var address = new google.maps.LatLng(document.getElementById('<%= lat.ClientID %>').value, document.getElementById('<%= lng.ClientID %>').value);
                var marker;
                var map;
                var mapOptions = {
                    zoom: 13,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    center: address,

                };

                map = new google.maps.Map(document.getElementById('map'),
                    mapOptions);

                marker = new google.maps.Marker({
                    map: map,
                    draggable: false,
                    position: address
                });
            } catch (e) {

            }

        }

        function isNumberKey(evt, txt) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function pageLoad(sender, args) {




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

        $(document).ready(function () {
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }
            $("#<%=txtDefaultAcct.ClientID%>").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetAccountName",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load accounts");
                            }
                        });
                    },
                    select: function (event, ui) {
                        $("#<%=txtDefaultAcct.ClientID%>").val(ui.item.label);
                        $("#<%=hdnAcctID.ClientID%>").val(ui.item.value);
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtDefaultAcct.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    //debugger;
                    var ula = ul;
                    var itema = item;
                    var result_value = item.value;
                    var result_item = item.acct;
                    var result_desc = item.label;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }

                    if (result_value == 0) {
                        //return $("<li></li>")
                        //.data("item.autocomplete", item)
                        //.append("<a>" + result_item + "</a>")
                        //.appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };




        });

        

    </script>
    <style>
        .ui-autocomplete {
            max-height: 200px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }

        .pnlUpdate {
            left: 33vw;
            width: 500px !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Vendor" runat="server">
        <AjaxSettings>

            <telerik:AjaxSetting AjaxControlID="lnkAddnew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkAddCustomer">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowAddCustomer" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_VendorTran" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />                    
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />                    
                    
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_VendorTran" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" /> 
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_VendorTran" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" /> 
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAllOpen">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_VendorTran" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" /> 
                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_VendorTran">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_VendorTran" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />               
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" /> 
                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Vendor" runat="server">
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
                                        <i class="mdi-social-person-outline"></i>&nbsp;
                                         <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add New Vendor</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" OnClientClick="javascript:return ValidateEmail();" ValidationGroup="general, rep" runat="server" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                            OnClick="lnkClose_Click" PostBackUrl="~/Vendors.aspx?AddVendor=Y"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label ID="lblVendorName" runat="server"></asp:Label>
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
                                    <li><a href="#accrdVendorInfo">Vendor Info</a></li>
                                    <li><a href="#accrdContacts">Contacts</a></li>
                                    <li runat="server" id="liTransactions"><a href="#accrdTransactions">Transactions</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
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
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li>
                            <div id="accrdVendorInfo" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>Vendor Info.</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    <div class="section-ttle">Vendor Details</div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">


                                                                <asp:HiddenField ID="hdnRolID" runat="server" />
                                                                <asp:TextBox ID="txtAccountid" runat="server" MaxLength="31" />
                                                                <asp:RequiredFieldValidator ID="rfvAccountNum"
                                                                    runat="server" ControlToValidate="txtaccountid" ValidationGroup="general, rep" Display="None" ErrorMessage="Vendor ID is Required"
                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender
                                                                    ID="vceAcctNum" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="rfvAccountNum" />
                                                                <asp:Label runat="server" ID="lbltxtAccountid" AssociatedControlID="txtAccountid">Vendor ID</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">


                                                                <asp:TextBox ID="txtName" runat="server" MaxLength="75" />
                                                                <asp:RequiredFieldValidator ID="rfvAcName"
                                                                    runat="server" ControlToValidate="txtName" ValidationGroup="general, rep" Display="None" ErrorMessage="Name is Required"
                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender
                                                                    ID="vceAcctName" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="rfvAcName" />
                                                                <asp:Label runat="server" ID="lbltxtName" AssociatedControlID="txtName">Vendor Name</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">

                                                                <asp:TextBox ID="txtContact" runat="server" MaxLength="50" />
                                                                <asp:Label runat="server" ID="lbltxtContact" AssociatedControlID="txtContact">Contact Name</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">

                                                                <asp:TextBox ID="txtPhone" CssClass="form-control phone" TabIndex="12" runat="server" MaxLength="28" />
                                                                <asp:Label runat="server" ID="lbltxtPhone" AssociatedControlID="txtPhone">Phone</asp:Label>
                                                                <%--<asp:MaskedEditExtender ID="txtPhone_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" InputDirection="LeftToRight"
                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                    MaskType="Number" TargetControlID="txtPhone" ValidateRequestMode="Enabled">
                                                </asp:MaskedEditExtender>--%>
                                                            </div>
                                                        </div>


                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">

                                                                <asp:Label runat="server" ID="lbltxtAddress" AssociatedControlID="txtAddress">Address</asp:Label>
                                                                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="materialize-textarea" placeholder="" MaxLength="255" />
                                                                <asp:Button ID="btnSelectAdd" runat="server" CausesValidation="False" OnClick="btnSelectAdd_Click" Style="display: none;" Text="Button" />
                                                                <input id="lat" runat="server" type="text" style="display: none;" />
                                                                <input id="lng" runat="server" type="text" style="display: none;" />
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="50" />
                                                                <asp:Label runat="server" ID="lbltxtCity" AssociatedControlID="txtCity">City</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtZip" runat="server" MaxLength="10" />
                                                                <asp:Label runat="server" ID="lbltxtZip" AssociatedControlID="txtZip">Zip</asp:Label>

                                                            </div>
                                                        </div>

                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                  <asp:TextBox ID="txtState" runat="server" MaxLength="10" />
                                                                <asp:Label runat="server" ID="Label5" AssociatedControlID="txtState">State</asp:Label>
                                                                <%--<label class="drpdwn-label">State</label>
                                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="browser-default" AutoPostBack="true" OnSelectedIndexChanged="ddlState_OnSelectedIndexChanged">
                                                                </asp:DropDownList>--%>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">

                                                                <asp:TextBox ID="txtCountry" Text="United States" runat="server" />
                                                                <asp:Label runat="server" ID="Label2" AssociatedControlID="txtCountry">Country</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                            <div class="row">
                                                                <asp:Label runat="server" ID="lbltxtCompany" AssociatedControlID="txtCompany">Company</asp:Label>
                                                                <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>

                                                                <label runat="server" id="lblddlCompany" class="drpdwn-label">Company</label>
                                                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="browser-default">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvCompanyValidate" runat="server"
                                                                    ControlToValidate="ddlCompany" ValidationGroup="general, rep" InitialValue="0" Display="None" ErrorMessage="Company Required"
                                                                    SetFocusOnError="True">
                                                                </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                    ID="ValidatorCalloutExtender3"
                                                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="rfvCompanyValidate">
                                                                </asp:ValidatorCalloutExtender>
                                                                <div class="srchclr btnlinksicon rowbtn">
                                                                    <asp:HyperLink ID="btnCompanyPopUp" runat="server" Style="cursor: pointer;" onclick="OpenCompanyPopUp(this);" ToolTip="Change Company">  <i class="mdi-action-work ml" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                                    <%--<asp:LinkButton runat="server" OnClick="btnCompanyPopUp_Click" ID="btnCompanyPopUp" ToolTip="Change Company"><i class="mdi-action-work" style="margin-left:0px !important;"></i></asp:LinkButton>--%>
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

                                                                <asp:TextBox ID="txtFax" runat="server" MaxLength="28" />
                                                                <asp:Label runat="server" ID="lbltxtFax" AssociatedControlID="txtFax">Fax</asp:Label>
                                                                <%-- <asp:MaskedEditExtender ID="txtFax_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" InputDirection="LeftToRight"
                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                    MaskType="Number" TargetControlID="txtFax" ValidateRequestMode="Enabled">
                                                </asp:MaskedEditExtender>--%>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">


                                                                <asp:TextBox ID="txtEmailid" TextMode="Email" runat="server" MaxLength="50" />
                                                                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                                    ControlToValidate="txtEmailid" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                <asp:ValidatorCalloutExtender ID="vceEmail" PopupPosition="BottomLeft" runat="server" Enabled="True"
                                                                    TargetControlID="revEmail">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:Label runat="server" ID="lbltxtEmailid" AssociatedControlID="txtEmailid">Email</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">

                                                                <asp:TextBox ID="txtCellular" runat="server" MaxLength="28" placeholder="(xxx)xxx-xxxx" />
                                                                <asp:Label runat="server" ID="lbltxtCellular" AssociatedControlID="txtCellular">Cellular</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">

                                                                <asp:TextBox ID="txtWebsite" runat="server" MaxLength="50" />
                                                                <asp:RegularExpressionValidator 
            ID="revWebsite"
            runat="server" 
            ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
            ControlToValidate="txtWebsite"
            ErrorMessage="Invalid Webaddress URL!"
            ></asp:RegularExpressionValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" PopupPosition="BottomLeft" runat="server" Enabled="True"
                                                                    TargetControlID="revWebsite">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:Label runat="server" ID="lbltxtWebsite" AssociatedControlID="txtWebsite">Web Address</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4">
                                                            <div class="checkrow">

                                                                <%--<label for="test12">1099</label>--%>
                                                                <asp:CheckBox ID="chkmainEmailPO" runat="server" CssClass="css-checkbox" Text="Email PO" />
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>
                                                <div class="form-section-row">
                                                    <div class="section-ttle">General</div>
                                                    <div class="form-section3">

                                                        <div class="input-field col s12">
                                                            <div class="row">

                                                                <asp:TextBox ID="txtAcct" Text="" runat="server" />
                                                                <asp:Label runat="server" ID="lbltxtAcct" AssociatedControlID="txtAcct">Acct #</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">

                                                                <label class="drpdwn-label">Type</label>
                                                                <asp:DropDownList ID="ddlType" CssClass="browser-default" runat="server">
                                                                    <%--    <asp:ListItem Text="Select Type" Value="0" />
                                                                    <asp:ListItem Text="Cost Of Sales" Value="1" />
                                                                    <asp:ListItem Text="Overhead" Value="2" />--%>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlType"
                                                                    runat="server" ControlToValidate="ddlType" ValidationGroup="general, rep" Display="None" InitialValue="0"  ErrorMessage="Type is Required"
                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender
                                                                    ID="rfvddlTypeExtender" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="rfvddlType" />
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">

                                                                <asp:TextBox ID="txtCreditlimit" Text="0.0" runat="server" />
                                                                <asp:Label runat="server" ID="lbltxtCreditlimit" AssociatedControlID="txtCreditlimit">Credit Limit</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field2 col s10">
                                                            <div class="row">

                                                                <asp:TextBox ID="txtDays" Text="10" runat="server" />

                                                                <asp:Label runat="server" ID="lbltxtDays" AssociatedControlID="txtDays">If Paid In</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field2 col s2 days-css" >
                                                            <div class="row">
                                                                Days
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">


                                                                <asp:Label runat="server" ID="Label3" AssociatedControlID="txtDays">Remit To</asp:Label>
                                                                <asp:TextBox ID="txtRemitTo" runat="server" CssClass="materialize-textarea remit-ht" TextMode="MultiLine" MaxLength="255" placeholder ="Remit to/Address" />
                                                            </div>
                                                        </div>
                                                        

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Status</label>
                                                                <asp:DropDownList ID="ddlStatus" CssClass="browser-default" runat="server">
                                                                    <asp:ListItem Text="Select Status" Value="-1" />
                                                                    <asp:ListItem Text="Active" Value="0" />
                                                                    <asp:ListItem Text="Inactive" Value="1" />
                                                                    <asp:ListItem Text="Hold" Value="2" />
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlStatus"
                                                                    runat="server" ControlToValidate="ddlStatus" ValidationGroup="general, rep" Display="None" InitialValue="-1"  ErrorMessage="Status is Required"
                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender
                                                                    ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="rfvddlStatus" />
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Terms</label>

                                                                <asp:DropDownList ID="ddlTerms" runat="server" CssClass="browser-default">
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


                                                                <asp:Label runat="server" ID="Label1" AssociatedControlID="txtDays">Remark</asp:Label>
                                                                <asp:TextBox ID="txtvenremark" runat="server" CssClass="materialize-textarea remit-ht" TextMode="MultiLine" MaxLength="255" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section-row">
                                                    <div class="section-ttle">Control</div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">


                                                                <asp:TextBox ID="txtShipvia" runat="server" MaxLength="50" />
                                                                <asp:Label runat="server" ID="Label4" AssociatedControlID="txtDays">Ship Via</asp:Label>

                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCourieracct" runat="server" MaxLength="500" />
                                                                <asp:Label runat="server" ID="lbltxtCourieracct" AssociatedControlID="txtCourieracct">Courier account #</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">


                                                                <asp:TextBox ID="txtBalance" placeholder="0.0" runat="server" ReadOnly="True" disabled="disabled"  />
                                                                <asp:Label runat="server" ID="lbltxtBalance" AssociatedControlID="txtBalance">Balance</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12" runat="server" id="div1099" visible="false">
                                                            <div class="row">
                                                                <asp:TextBox ID="txt1099" Visible="false" Text="1" runat="server" />
                                                                <asp:Label runat="server" ID="Label13" AssociatedControlID="txt1099">1099 Box</asp:Label>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    

                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>

                                                    <div class="form-section3">
                                                        <div class="input-field col s4">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chk1099" runat="server" CssClass="css-checkbox" Text="1099" />
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s1">
                                                            &nbsp;
                                                        </div>

                                                        <div class="input-field col s7">
                                                            <div class="row">

                                                                <asp:TextBox ID="txt1099Box" runat="server" MaxLength="50"
                                                                    onkeypress="return isNumberKey(event,this)" autocomplete="off" Text="7" />
                                                                <asp:Label runat="server" ID="lbltxt1099Box" AssociatedControlID="txt1099Box">1099 Box</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtFedID" runat="server" MaxLength="15" />
                                                                <asp:Label runat="server" ID="lbltxtFedID" AssociatedControlID="txtFedID">Fed Id #</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                
                                                                <asp:TextBox ID="txtDefaultAcct" runat="server" MaxLength="75"
                                                                    Placeholder="Search by acct# and name" />
                                                                <asp:HiddenField ID="hdnAcctID" runat="server" />
                                                                <asp:Label runat="server" ID="lbltxtDefaultAcct" AssociatedControlID="txtDefaultAcct">Default Acct</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12" runat="server" id="divLast">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtLast" runat="server" />
                                                                <asp:Label runat="server" ID="Label12" AssociatedControlID="txtLast">Last</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12" runat="server" id="divInuse">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtInuse" Text="1" runat="server" />
                                                                <asp:Label runat="server" ID="Label15" AssociatedControlID="txtInuse">InUse</asp:Label>
                                                            </div>
                                                        </div>

                                                    </div>

                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">

                                                        

                                                        <div class="input-field col s12" style="display:block;" id="ddlSTaxgv">
                                                            <div class="row">

                                                                <label class="drpdwn-label" id="spansalestax" runat="server">Sales Tax</label>
                                                                <asp:DropDownList ID="ddlSTax" CssClass="browser-default" runat="server">
                                                                   
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="revddlsalesTax"
                                                                    runat="server" ControlToValidate="ddlSTax" ValidationGroup="general, rep" Display="None" InitialValue="0"  ErrorMessage="Sales Tax is Required"
                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender
                                                                    ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="revddlsalesTax" />
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12" style="display:none;" id="ddlUseTaxgv">
                                                            <div class="row">

                                                                <label class="drpdwn-label">Use Tax</label>
                                                                <asp:DropDownList ID="ddlUseTax" CssClass="browser-default" runat="server">
                                                                    
                                                                </asp:DropDownList>
                                                               <%-- <asp:RequiredFieldValidator ID="rfvddlUseTax"
                                                                    runat="server" ControlToValidate="ddlUseTax" ValidationGroup="general, rep" Display="None" InitialValue="0"  ErrorMessage="Use Tax is Required"
                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender
                                                                    ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="rfvddlUseTax" />--%>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12" runat="server" id="divSince">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtSince" runat="server" />
                                                                <asp:Label runat="server" ID="Label11" AssociatedControlID="txtSince">Since</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12" runat="server" id="divGeolock">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtGeolock" Text="0" runat="server" />
                                                                <asp:Label runat="server" ID="Label14" AssociatedControlID="txtGeolock">GeoLock</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>


                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdContacts" class="collapsible-header accrd accordian-text-custom"><i class="mdi-social-people"></i>Contacts</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap form-content-wrapwd">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="srchpaneinner">
                                                <div class="btnlinks">
                                                    <asp:LinkButton CssClass="icon-addnew" ID="lnkAddnew" runat="server" CausesValidation="False" OnClientClick="return AddContactClick(this);" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton CssClass="icon-edit" OnClick="btnEdit_Click"
                                                        ID="btnEdit" runat="server"
                                                        OnClientClick="return EditContactClick(this);" CausesValidation="False">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton CssClass="icon-delete" ID="btnDelete" OnClick="btnDelete_Click"
                                                        runat="server" CausesValidation="False" OnClientClick="return DeleteContactClick(this);">Delete</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <a id="lnkMail" class="icon-mail" runat="server">Email</a>
                                                </div>
                                            </div>
                                            <div class="grid_container">
                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock_Prospect" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_VendorContact.ClientID %>");
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Vendor" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Vendor" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_VendorContact" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                            OnNeedDataSource="RadGrid_VendorContact_NeedDataSource" PagerStyle-AlwaysVisible="true"
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
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("ContactID") %>'></asp:Label>
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

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="140" AutoPostBackOnFilter="true" HeaderText="Email Address" DataField="Email" CurrentFilterFunction="Contains" SortExpression="Email" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmail" runat="server"><%#Eval("Email")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="140" AutoPostBackOnFilter="true" HeaderText="Email PO" CurrentFilterFunction="Contains" SortExpression="Email PO" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkEmailPo" CssClass="css-checkbox" Checked='<%# Convert.ToBoolean(Eval("EmailRecPO")) %>' runat="server" />
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
                        <li runat="server" id="adTransactions">
                            <div id="accrdTransactions" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-cached"></i>Transactions</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap form-content-wrapwd">
                                    <div class="form-content-pd">
                                        <div class="srchpaneinner">
                                            <div class="srchtitle date-csss" >
                                                Date
                                            </div>
                                            <div class="srchinputwrap">
                                                <%-- <asp:TextBox ID="txtfromDate" runat="server" MaxLength="50" OnTextChanged="txtfromDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                <asp:CalendarExtender ID="txtfromDate_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtfromDate">
                                                                </asp:CalendarExtender>--%>
                                                <asp:TextBox ID="txtFromDate" runat="server" MaxLength="50" CssClass="datepicker_mom" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="srchinputwrap">
                                                <%-- <asp:TextBox ID="txtToDate" runat="server" MaxLength="50" OnTextChanged="txtToDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtToDate">
                                                                </asp:CalendarExtender>--%>
                                                <asp:TextBox ID="txtToDate" runat="server" MaxLength="50" CssClass="datepicker_mom" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="srchinputwrap tabcontainer">
                                                <ul class="tabselect accrd-tabselect" id="testradiobutton">
                                                    <li>
                                                        <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <label id="lblDay" runat="server">
                                                            <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#lblDay', 'hdnInvoiceDate', 'rdCal')" />
                                                            Day
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblWeek" runat="server">
                                                            <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnInvoiceDate', 'rdCal')" />
                                                            Week
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblMonth" runat="server">
                                                            <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnInvoiceDate', 'rdCal')" />
                                                            Month
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblQuarter" runat="server">
                                                            <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnInvoiceDate', 'rdCal')" />
                                                            Quarter
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblYear" runat="server">
                                                            <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnInvoiceDate', 'rdCal')" />
                                                            Year
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                                                    </li>
                                                </ul>

                                            </div>


                                            <div class="btnlinksicon">
                                                <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false" ToolTip="Search"
                                                    OnClick="lnkSearch_Click"><i class="mdi-action-search"></i></asp:LinkButton>
                                            </div>
                                            <div class="btnlinks" style="float: right;">
                                                <asp:LinkButton ID="lnkTransactionExcel" runat="server" OnClick="lnkTransactionExcel_Click" CausesValidation="false">Export to Excel</asp:LinkButton>

                                            </div>
                                            <div class="Vendor-css">
                                                        <span>Vendor Balance:
                                                     <asp:Label runat="server" ID="lblVendorBalance">$0.00</asp:Label>
                                                    </span>
                                                    </div>
                                        </div>
                                        <div class="srchpaneinner">
                                            <asp:UpdatePanel ID="updtall" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                            <div class="rd-flt">
                                                <input id="rdAll" runat="server" class="radio-custom" name="radio-group" type="radio" value="rdAll">
                                                <asp:Label runat="server" ID="lblrdAll" CssClass="radio-custom-label" AssociatedControlID="rdAll">All</asp:Label>
                                            </div>
                                            <div class="rd-flt">
                                                <input id="rdOpen" runat="server" class="radio-custom" name="radio-group" type="radio" value="rdOpen">
                                                <asp:Label runat="server" ID="Label6" CssClass="radio-custom-label" AssociatedControlID="rdOpen">Open</asp:Label>
                                            </div>

                                            <div class="rd-flt">
                                                <input id="rdClosed" runat="server" class="radio-custom" name="radio-group" type="radio" value="rdClosed">
                                                <asp:Label runat="server" ID="Label7" CssClass="radio-custom-label" AssociatedControlID="rdClosed">Closed</asp:Label>
                                            </div>
                                            <asp:HiddenField runat="server" ID="hdnSearchValue" ClientIDMode="Static" />


                                            <div class="rd-flt rd-fltdifference">
                                                <input id="rdAll2" runat="server" class="radio-custom" name="radio-group1" type="radio" value="rdAll2">
                                                <asp:Label runat="server" ID="Label8" CssClass="radio-custom-label" AssociatedControlID="rdAll2">All</asp:Label>
                                            </div>
                                            <div class="rd-flt">
                                                <input id="rdCharges" runat="server" class="radio-custom" name="radio-group1" type="radio" value="rdCharges">
                                                <asp:Label runat="server" ID="Label9" CssClass="radio-custom-label" AssociatedControlID="rdCharges">Charges</asp:Label>
                                            </div>

                                            <div class="rd-flt">
                                                <input id="rdCredit" runat="server" class="radio-custom" name="radio-group1" type="radio" value="rdCredit">
                                                <asp:Label runat="server" ID="Label10" CssClass="radio-custom-label" AssociatedControlID="rdCredit">Credits</asp:Label>
                                            </div>
                                            <asp:HiddenField runat="server" ID="hdnSearchBy" ClientIDMode="Static" />
                                            </ContentTemplate>
                                            </asp:UpdatePanel>        
                                            
                                            <div class="col lblsz2 lblszfloat">
                                                <div class="row">
                                                    <span class="tro trost-label2 accrd-trost">
                                                        <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                                                    </span>
                                                </div>
                                            </div>
                                                    
                                            <div class="col lblsz2 lblszfloat">
                                                <div class="row">
                                                    <span class="tro trost-label2 accrd-trost">
                                                        <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click" CausesValidation="False">Show All </asp:LinkButton>
                                                    </span>
                                                    <span class="tro trost-label2 accrd-trost">
                                                        <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click" CausesValidation="False">Clear</asp:LinkButton>
                                                    </span>
                                                    <span class="tro trost-label2 accrd-trost">
                                                        <asp:LinkButton ID="lnkShowAllOpen" runat="server" OnClick="lnkShowAllOpen_Click" CausesValidation="False">Show All Open</asp:LinkButton>
                                                    </span>
                                                    
                                                </div>
                                            </div>
                                        </div>

                                        <div class="grid_container">
                                            <div class="form-section-row m-b-0" >
                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock_VendorTran" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_VendorTran.ClientID %>");
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

                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_VendorTran" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Vendor" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_VendorTran" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                            OnNeedDataSource="RadGrid_VendorTran_NeedDataSource" OnPreRender="RadGrid_VendorTran_PreRender" OnItemDataBound="RadGrid_VendorTran_ItemDataBound" PagerStyle-AlwaysVisible="true"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True" OnItemCreated="RadGrid_VendorTran_ItemCreated" OnExcelMLExportRowCreated="RadGrid_VendorTran_ExcelMLExportRowCreated">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                <Columns>

                                                                    <%--    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                                    </telerik:GridClientSelectColumn>--%>

                                                                    <telerik:GridTemplateColumn HeaderText="Ref No." DataField="Ref" SortExpression="Ref" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"  HeaderStyle-Width="140px">
                                                                        <ItemTemplate>
                                                                            <%--<asp:LinkButton ID="lblId" runat="server" Text='<%#Eval("ID")%>'></asp:LinkButton>--%>
                                                                            <asp:LinkButton ID="lblId" runat="server" Text='<%#Eval("Ref")%>' ></asp:LinkButton>
                                                                            <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID") %>'></asp:HiddenField>
                                                                            <asp:HiddenField ID="hdnTRID" runat="server" Value='<%# Eval("TRID") %>'></asp:HiddenField>
                                                                        </ItemTemplate>
                                                                        <%--<FooterTemplate>
                                                                            <asp:Label runat="server" ID="lblVenBalance"></asp:Label>
                                                                        </FooterTemplate>--%>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderText="Date" DataField="fDate" SortExpression="fDate" ShowFilterIcon="false"  HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):""%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <%--<telerik:GridBoundColumn DataField="ref" HeaderText="Ref" SortExpression="ref"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>--%>

                                                                    <%--<telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        ShowFilterIcon="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" >
                                                                    </telerik:GridBoundColumn>
                                                                        
                                                                        <telerik:GridBoundColumn DataField="fdesc" HeaderText="Description" SortExpression="fdesc"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        ShowFilterIcon="false" HeaderStyle-HorizontalAlign="Left">
                                                                    </telerik:GridBoundColumn>   --%>

                                                                    <telerik:GridTemplateColumn HeaderText="Vendor" DataField="VendorName" SortExpression="VendorName" ShowFilterIcon="false"  CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVendorName" runat="server" Text='<%#Eval("VendorName")%>'  ></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Description" DataField="fdesc" SortExpression="fdesc" ShowFilterIcon="false"  CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblfdesc" runat="server" Text='<%#Eval("fdesc")%>'  ></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                      

                                                                    <telerik:GridTemplateColumn DataField="Debit" HeaderText="Amount" SortExpression="Debit" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderStyle-Width="140" ItemStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Debit", "{0:c}")%>'
                                                                                 ></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Credit" HeaderText="Credit" SortExpression="Credit" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderStyle-Width="140" ItemStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" >
                                                                        <ItemTemplate>
                                                                            <%--<asp:Label ID="lblCredit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Credit", "{0:c}")%>'
                                                                                ForeColor='<%# Convert.ToDouble(Eval("Credit"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' style="padding-left:15px;"></asp:Label>--%>
                                                                            <asp:Label ID="lblCredit" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem, "Credit", "{0:c}")%>'
                                                                                ForeColor="Red" ></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Balance" HeaderText="Balance" UniqueName="Balance" SortExpression="Balance" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderStyle-Width="140" ItemStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'
                                                                                 ></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="RunTotal" HeaderText="Running Total" SortExpression="RunTotal" HeaderStyle-Width="160" ItemStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRun" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RunTotal", "{0:c}")%>'
                                                                                ForeColor='<%# Convert.ToDouble(Eval("RunTotal"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' ></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <%--<telerik:GridBoundColumn DataField="StatusName" HeaderText="Status" SortExpression="StatusName"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                        ShowFilterIcon="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                                                    </telerik:GridBoundColumn>--%>

                                                                    <telerik:GridTemplateColumn HeaderText="Status" DataField="StatusName" SortExpression="StatusName" HeaderStyle-Width="100"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"  >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatusName" runat="server" Text='<%#Eval("StatusName")%>'  ></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderText="Type" DataField="Type" SortExpression="Type" HeaderStyle-Width="100"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type")%>'  ></asp:Label>
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
                    </ul>
                </div>
            </div>
        </div>
    </div>



    <telerik:RadWindowManager ID="RadWindowManagerVendor" runat="server">
        <Windows>
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
                            <asp:Button ID="btnCompanyEdit" runat="server" Text="Save" OnClick="btnCompanyEdit_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowContact" Skin="Material" VisibleTitlebar="true" Title="Add Contact" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="320">
                <ContentTemplate>
                    <div>
                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                                            Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="left" TargetControlID="RequiredFieldValidator12">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtContcName" runat="server" MaxLength="50">    </asp:TextBox>
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
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ControlToValidate="txtContEmail"
                                            Display="None" ErrorMessage="Invalid Email" ValidationGroup="cont" SetFocusOnError="True"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                        </asp:ValidatorCalloutExtender>

                                        <asp:FilteredTextBoxExtender ID="txtContEmail_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                            TargetControlID="txtContEmail">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="txtContEmail" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblEmail" AssociatedControlID="txtContEmail">Email</asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:CheckBox ID="chkEmailPo" runat="server" Text="Email PO" CssClass="css-checkbox" />
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
            <telerik:RadWindow ID="RadWindowHistoryTransaction" Skin="Material" VisibleTitlebar="true" Title="Payment History"  CenterIfModal="true"
                Animation="Fade" AnimationDuration="100" RenderMode="Auto" VisibleStatusbar="false" Width="500px" Height="200px" ReloadOnShow="false"
                runat="server" Modal="true" ShowContentDuringLoad="false" Behaviors="Move, Close" OnClientDragEnd="setCustomPosition" >
           </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewContact" Value="Y" />


    <asp:HiddenField runat="server" ID="hdnVendorTranSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
    <asp:HiddenField runat="server" ID="hdnCon"/>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.maskedinput/1.4.1/jquery.maskedinput.min.js"></script>
    <script>
        $(document).ready(function () {

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
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


            $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetIsSalesTaxAPBill",
                        //data: '{"prefixText": "' + dtaaa.prefixText + '", "IsJob": "' + false + '", "con": "' + dtaaa.con + '"}',
                        dataType: "json",
                        async: true,
                success: function (data) {
                            debugger
                            //response($.parseJSON(data.d));
                            var ui = JSON.parse(data.d);

                                if (ui.length > 0) {
                                        var IsSalesTaxAPBill = ui[0].IsSalesTaxAPBill ;
                                        var IsUseTaxAPBill = ui[0].IsUseTaxAPBill;
                                        if (IsSalesTaxAPBill == "1") 
                                        {
                                            
                                            document.getElementById('ddlSTaxgv').style.display = 'block';
                                            //document.getElementById('ddlUseTaxgv').style.display = 'block';
                                        } else {
                                        
                                            
                                            document.getElementById('ddlSTaxgv').style.display = 'none';
                                            //document.getElementById('ddlUseTaxgv').style.display = 'none';
                                    }
                                    if (IsUseTaxAPBill == "1") 
                                        {
                                            document.getElementById('ddlUseTaxgv').style.display = 'none';
                                            
                                        } else {
                                        
                                            document.getElementById('ddlUseTaxgv').style.display = 'none';
                                            
                                        }
                                    //$(txtGvAcctName).val(ui[0].DefaultAcct);
                                }
                            

                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load phase details");
                        }
                    });

            ///////////// Quick Codes //////////////
            $("#<%=txtRemitTo.ClientID%>").keyup(function (event) {
                debugger
                replaceQuickCodes(event, '<%=txtRemitTo.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

            $("#<%=txtvenremark.ClientID%>").keyup(function (event) {
                debugger
                replaceQuickCodes(event, '<%=txtvenremark.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
        });
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
        });
    </script>
    <script type="text/javascript">
        function CssClearLabel() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
            debugger;
            if (typeof (Storage) !== "undefined") {

                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblVendorTranActive"])%>';
                var val;
                val = localStorage.getItem("hdnVendorTranDate");
                if (SesVar == '2') {
                    $("#<%=lblDay.ClientID%>").addClass("");
                    $("#<%=lblWeek.ClientID%>").addClass("");
                    $("#<%=lblMonth.ClientID%>").addClass("");
                    $("#<%=lblQuarter.ClientID%>").addClass("");
                    $("#<%=lblYear.ClientID%>").addClass("");
                }
                else {
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
                }
            }
        });
    </script>
    <script type="text/javascript">
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
                document.getElementById('<%= hdnVendorTranSelectDtRange.ClientID%>').value = "Day";
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
                document.getElementById('<%= hdnVendorTranSelectDtRange.ClientID%>').value = "Week";
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
                document.getElementById('<%= hdnVendorTranSelectDtRange.ClientID%>').value = "Month";
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
                document.getElementById('<%= hdnVendorTranSelectDtRange.ClientID%>').value = "Quarter";
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
                document.getElementById('<%= hdnVendorTranSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnVendorTranSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
    <script type="text/javascript">

        function OpenCompanyPopUp() {
            var wnd = $find('<%=RadWindowCompany.ClientID %>');
            wnd.set_title("Select Company");
            wnd.Show();
        }
        function CloseCompanyPopUp() {
            var wnd = $find('<%=RadWindowCompany.ClientID %>');
            wnd.Close();
        }
        function ShowHistoryTransactionPopup(Uid, Type, owner, loc, status, TransID) {

            var oWnd = $find("<%=RadWindowHistoryTransaction.ClientID%>");
            oWnd.setUrl("VendorTransactionHistory.aspx?uid=" + Uid + "&type=" + Type + "&vendor=" + owner + "&loc=" + loc + "&status=" + status + "&tid=" + TransID + "&page=addcustomer");
            oWnd.setSize(800, 400);
            oWnd.show();
        }
        function setCustomPosition(sender, args) {

            var elmnt = document.getElementById("<%=RadGrid_VendorTran.ClientID%>");

            sender.moveTo(sender.get_left(), elmnt.offsetTop);
        }
        function ValidateEmail() {
            retValemail = true;
            retValweb = true;
            var email = $("#<%=txtEmailid.ClientID%>").val();
            var webaddress = $("#<%=txtWebsite.ClientID%>").val();
            if (email == "" && webaddress == "") {
                return true;
                //retVal = true;
            }
            else {
                if (email != "") {
                    var mailformat = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
                    if (email.match(mailformat)) {
                        //alert("Valid email address!");
                        //document.form1.text1.focus();
                        //return true;
                        retValemail = true;
                    }
                    else {
                        alert("Invalid email address!");
                        //document.form1.text1.focus();
                        //return false;
                        retValemail = false;
                    }
                }
                if (webaddress != "") {
                    var webaddressformat =
                        /(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})/gi;
                    var regex = new RegExp(webaddressformat);

                    if (webaddress.match(regex)) {
                        //alert("Valid email address!");
                        //document.form1.text1.focus();
                        retValweb = true;
                    }
                    else {
                        alert("Invalid web address!");
                        //document.form1.text1.focus();
                        retValweb = false;
                    }
                }

                if (retValemail == true && retValweb == true) {
                    return true;
                }
                else {
                    return false;
                }

            }
        }
    </script>
</asp:Content>


