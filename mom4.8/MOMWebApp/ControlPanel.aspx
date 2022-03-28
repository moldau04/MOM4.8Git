<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="ControlPanel" CodeBehind="~/ControlPanel.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <style>
        ul.a {
            list-style-type: circle !important;
            display: block;
            list-style-type: disc;
            margin-block-start: 1em;
            margin-block-end: 1em;
            margin-inline-start: 0px;
            margin-inline-end: 0px;
            padding-inline-start: 40px;
        }

            ul.a > li {
                list-style-type: circle !important;
            }

        ul.b {
            list-style-type: disc !important;
            padding-left: 20px;
        }

            ul.b > li {
                list-style-type: disc !important;
            }
    </style>
    <script type="text/javascript">
        function ConfirmUpload() {
            document.getElementById('<%=btnUpload.ClientID%>').click();
        }
        function GetSelectedValue(ddlCountry) {
            var selectedValue = ddlCountry.value;
            if (selectedValue == "1") {
                $(".GST").show();
            }
            else {
                $(".GST").hide();
            }
        }
        function onYearChanged(ddlYear) {
            var selectedValue = ddlYear.value;
            if (selectedValue === ":: Select ::") {
                ddlYear.value = "0";
            }
        }
        function ChkInventoryTrackingChanged(chkInvTracking) {
            if (!chkInvTracking.checked) {
                $('.divDrpAllChartAcct').hide();
            } else {
                $('.divDrpAllChartAcct').show();
            }
        }

        function ChkApplyPasswordRulesChanged(chkApplyPasswordRules) {
            //var valName = document.getElementById("<=rfvAdminEmail.ClientID%>");
            if (!chkApplyPasswordRules.checked) {
                $('.divApplyPasswordRules').hide();
                //ValidatorEnable(valName, false);
            } else {
                $('.divApplyPasswordRules').show();
                //ValidatorEnable(valName, true);
            }
        }

        function ChkPwResetDaysChanged(chkPwResetDays) {
            var valName = document.getElementById("<%=rfvPwResetDays.ClientID%>");
            var valMessBox = $("#<%=ValidatorCalloutExtender5.ClientID%>" + "_popupTable");
            if (!chkPwResetDays.checked) {
                $('#<%=txtPwResetDays.ClientID%>').hide();
                ValidatorEnable(valName, false);
                if (valMessBox) valMessBox.hide();
            } else {
                $('#<%=txtPwResetDays.ClientID%>').show();
                ValidatorEnable(valName, true);
                if (valMessBox && $('#<%=txtPwResetDays.ClientID%>').val() == "") valMessBox.show();
            }
        }
    </script>

    <script type="text/javascript">

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
        $(document).ready(function () {
            $(function () {
                $("#<%= txtAddress.ClientID %>").geocomplete({
                    map: false,
                    details: "#divmain",
                    types: ["address"],
                    address: "#<%= txtAddress.ClientID %>",
                    city: "#<%= txtCity.ClientID %>",
                    state: "#<%= ddlState.ClientID %>",
                    zip: "#<%= txtZip.ClientID %>",
                    lat: "#<%= lat.ClientID %>",
                    lng: "#<%= lng.ClientID %>"
                }).bind("geocode:result", function (event, result) {
                    var getCountry = "";
                    for (var i = 0; i < result.address_components.length; i++) {
                        var addr = result.address_components[i];
                        var getCountry;
                        var countryNum = "0";
                        if (addr.types[0] == 'country')
                            getCountry = addr.short_name;
                        switch (getCountry) {
                            case 'US':
                                getCountry = "0";
                                $(".GST").hide();
                                break;
                            case 'CA':
                                getCountry = "1";
                                $(".GST").show();
                                break;
                            case 'GB':
                                getCountry = "2";
                                $(".GST").hide();
                                break;
                            default:
                        }
                    }

                    $("#<%=ddlCountry.ClientID%>").val(getCountry);

                    Materialize.updateTextFields();

                });
                ChkInventoryTrackingChanged(document.getElementById('<%=ChkInventoryTracking.ClientID%>'));
                ChkApplyPasswordRulesChanged(document.getElementById('<%=chkApplyPasswordRules.ClientID%>'));
                ChkPwResetDaysChanged(document.getElementById('<%=chkPwResetDays.ClientID%>'));
                initialize();
            });

            var ddlCountry = document.getElementById('<%=ddlCountry.ClientID%>');
            if (ddlCountry.value == "1") {
                $(".GST").show();
            } else {
                $(".GST").hide();
            }

            if ((!$('#ctl00_ContentPlaceHolder1_ChkMultiCompany').is(":checked")) == true) {
                $('#ctl00_ContentPlaceHolder1_dvMultiOffice').hide();
            }
            else {
                $('#ctl00_ContentPlaceHolder1_dvMultiOffice').show();
            }
            $('#ctl00_ContentPlaceHolder1_ChkMultiCompany').click(function () {
                var $this = $(this);
                if ($this.is(':checked')) {
                    $('#ctl00_ContentPlaceHolder1_dvMultiOffice').show();
                } else {
                    $('#ctl00_ContentPlaceHolder1_dvMultiOffice').hide();
                }
            });
            ///////////// Ajax call for GL acct auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            }
            $("#<%=txtGSTGL.ClientID%>").autocomplete({

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

                    $("#<%=txtGSTGL.ClientID%>").val(ui.item.label);
                    $("#<%=hdnGSTGL.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtGSTGL.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).
                data("ui-autocomplete")._renderItem = function (ul, item) {
                    //debugger;
                    var ula = ul;
                    var itema = item;
                    var result_value = item.value;
                    var result_item = item.label;
                    var result_desc = item.acct;
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
                            .data("ui-autocomplete-item", item)
                            .append("<a>" + result_item + ", <span>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };

            if ($("[id*=ChkInventoryTracking]").is(":checked")) {
                $("[id*=defaultInvAcct]").css({ 'display': 'block' });
            }
            else {
                $("[id*=defaultInvAcct]").css({ 'display': 'none' });
            }

            $("[id*=ChkInventoryTracking]").change(function () {
                if (this.checked) {
                    $("[id*=defaultInvAcct]").css({ 'display': 'block' });
                }
                else {
                    $("[id*=defaultInvAcct]").css({ 'display': 'none' });
                }
            });

            $('#<%=txtTele.ClientID%>').mask("(999) 999-9999");
            $('#<%=txtFax.ClientID%>').mask("(999) 999-9999");
            UpdateTextbox();
        });
        function UpdateTextbox() {
            if (typeof (Materialize) != 'undefined' && typeof (Materialize.updateTextFields) == 'function') {
                Materialize.updateTextFields();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-action-trending-up"></i>&nbsp;Control Panel</div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <%--<a href="#">Save </a>--%>
                                            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <%--<asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false"><i class="mdi-content-clear"></i></asp:LinkButton>--%>
                                        <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" Text="Close" ToolTip="Close" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
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
                                    <li><a href="#accrdContact" class="link-slide">Contact</a></li>
                                    <li><a href="#accrdOther" class="link-slide">Settings</a></li>
                                    <li><a href="#accrdPasswordSettings" class="link-slide">Password Settings</a></li>
                                    <li><a href="#accrdOnlinePaymentSetting" class="link-slide">Online Payment Settings</a></li>
                                    <li><a href="#accrdQB" class="link-slide">Quickbooks</a></li>
                                    <li><a id="API" href="#accrdAPI" class="link-slide" runat="server">Web API</a></li>
                                </ul>
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
                        <li class="active">
                            <div id="accrdContact" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>Contact</div>
                            <div class="collapsible-body" style="display: block;">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">Company Info.</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <%--<input id="company" type="text" class="validate">
                                                        <label for="company">Company Name</label>--%>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                            ControlToValidate="txtCompany" Display="None" ErrorMessage="Company Required"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1"
                                                            runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator1">
                                                        </asp:ValidatorCalloutExtender>

                                                        <asp:TextBox ID="txtCompany" runat="server" CssClass="validate" MaxLength="75" TabIndex="1"></asp:TextBox>
                                                        <label for="txtCompany">Company Name <span style="color: red;">*</span></label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <%--<label for="address">Address </label>
                                                        <textarea id="address" class="materialize-textarea"></textarea>--%>
                                                        <label for="txtAddress">Address </label>
                                                        <textarea id="txtAddress" runat="server" class="materialize-textarea" maxlength="255"></textarea>
                                                        <asp:HiddenField ID="lng" runat="server" />
                                                        <asp:HiddenField ID="lat" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <%--<input id="city" type="text" class="validate">
                                                        <label for="city">City </label>--%>
                                                        <asp:TextBox ID="txtCity" runat="server" CssClass="validate" MaxLength="50"></asp:TextBox>
                                                        <label for="txtCity">City </label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <%--<input id="zip" type="text" class="validate">
                                                        <label for="zip">Zip </label>--%>
                                                        <asp:TextBox ID="txtZip" runat="server" CssClass="validate" MaxLength="10"></asp:TextBox>
                                                        <label for="txtZip">Zip </label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <%--<input id="state" type="text" class="validate">
                                                        <label for="state">State </label>--%>
                                                        <%--<asp:TextBox ID="txtState" runat="server" CssClass="validate"></asp:TextBox>--%>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                            ControlToValidate="ddlState" ValidationGroup="general, rep" InitialValue="State" Display="None" ErrorMessage="State Required"
                                                            SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                            ID="RequiredFieldValidator7_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator7">
                                                        </asp:ValidatorCalloutExtender>
                                                        <label class="drpdwn-label">State/Province</label>
                                                        <asp:DropDownList ID="ddlState" runat="server" ToolTip="State" CssClass="browser-default">
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
                                                        <%--<label class="drpdwn-label">Country </label>
                                                        <select class="browser-default">
                                                            <option>Select</option>
                                                            <option>USA</option>
                                                            <option>Canada</option>
                                                        </select>--%>
                                                        <label class="drpdwn-label">Country </label>
                                                        <asp:DropDownList ID="ddlCountry" runat="server" ToolTip="Country"
                                                            CssClass="browser-default" TabIndex="10" onchange="GetSelectedValue(this)" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                                                            <asp:ListItem Value="0">United States</asp:ListItem>
                                                            <asp:ListItem Value="1">Canada</asp:ListItem>
                                                            <asp:ListItem Value="2">United Kingdom</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row GST">
                                                        <label id="lblGSTReg">GST Reg#</label>
                                                        <asp:TextBox ID="txtGSTReg" runat="server" CssClass="form-control"
                                                            TabIndex="11"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row GST">
                                                        <label id="lblGSTRate">GST Rate %</label>
                                                        <asp:TextBox ID="txtGSTRate" runat="server" CssClass="form-control"
                                                            TabIndex="12" MaxLength="10"></asp:TextBox>
                                                        <asp:MaskedEditExtender ID="txtGSTRate_MaskedEditExtender" runat="server" Enabled="False"
                                                            Mask="9,999,999.99" TargetControlID="txtGSTRate" MaskType="Number" DisplayMoney="Left"
                                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                            CultureTimePlaceholder="">
                                                        </asp:MaskedEditExtender>
                                                        <asp:FilteredTextBoxExtender ID="txtGSTRate_FilteredTextBoxExtender" runat="server"
                                                            TargetControlID="txtGSTRate" ValidChars="0123456789." Enabled="True">
                                                        </asp:FilteredTextBoxExtender>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row GST">
                                                        <label id="lblGSTGL">GST GL</label>
                                                        <asp:TextBox ID="txtGSTGL" runat="server" CssClass="form-control"
                                                            TabIndex="13"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnGSTGL" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <%--<iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d13716.88953039753!2d76.77389278096229!3d30.740254306150458!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x390fed0afe5003d3%3A0x8f47abe9f2044934!2sSector+17%2C+Chandigarh!5e0!3m2!1sen!2sin!4v1506502302516" frameborder="0" style="border: 0; width: 100%" allowfullscreen></iframe>--%>
                                                        <div id="map" style="overflow: hidden !important; height: 110px!important;"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section-row">
                                            <div class="form-section3">
                                                <div class="section-ttle">Contact Info.</div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <%--<input id="contName" type="text" class="validate">
                                                    <label for="contName">Contact Name</label>--%>
                                                        <asp:TextBox ID="txtContName" runat="server" CssClass="validate" MaxLength="50" TabIndex="2"></asp:TextBox>
                                                        <label for="txtContName">Contact Name</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <%--<input id="telephone" type="text" class="validate">
                                                    <label for="telephone">Telephone</label>--%>
                                                        <asp:TextBox ID="txtTele" runat="server" CssClass="validate" MaxLength="20" TabIndex="4" placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                        <label for="txtTele">Telephone</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <%--<input id="fax" type="text" class="validate">
                                                    <label for="fax">Fax</label>--%>
                                                        <asp:TextBox ID="txtFax" runat="server" CssClass="validate" MaxLength="20"></asp:TextBox>
                                                        <label for="txtFax">Fax</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <%--<input id="email" type="text" class="validate">
                                                    <label for="email">Email</label>--%>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                            ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="validate" MaxLength="50"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                            TargetControlID="txtEmail">
                                                        </asp:FilteredTextBoxExtender>
                                                        <label for="txtEmail">Email</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <%--<input id="waddress" type="text" class="validate">
                                                    <label for="waddress">Web Address</label>--%>
                                                        <asp:TextBox ID="txtWebAdd" runat="server" CssClass="validate" MaxLength="50"></asp:TextBox>
                                                        <label for="txtWebAdd">Web Address</label>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section9">
                                                <div class="section-ttle">Company Logo</div>
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <%--<asp:FileUpload ID="FileUpload1" runat="server" class="dropify" />--%>
                                                        <asp:FileUpload ID="FileUpload1" CssClass="dropify" runat="server" onchange="ConfirmUpload();" />
                                                        <asp:Button ID="btnUpload" runat="server" CssClass="form-control" OnClick="btnUpload_Click" Text="Upload Logo"
                                                            ValidationGroup="img" TabIndex="23" Style="display: none;" />
                                                        <asp:RegularExpressionValidator ID="revImage" runat="server" ControlToValidate="FileUpload1"
                                                            ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G)|(b|B)(m|M)(p|P))$"
                                                            ValidationGroup="img" Display="None" ErrorMessage=" ! Invalid image type. Only JPEG, GIF, PNG and BMP allowed."
                                                            SetFocusOnError="True" />
                                                        <asp:ValidatorCalloutExtender ID="revImage_ValidatorCalloutExtender" runat="server"
                                                            Enabled="True" TargetControlID="revImage" PopupPosition="Left">
                                                        </asp:ValidatorCalloutExtender>
                                                    </div>
                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <div class="input-field col s6">
                                                            <div class="row " style="text-align: center;">
                                                                <%--<img src="Design/images/logonew.png" style="height: 130px; width: 155px;">--%>
                                                                <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Design/images/logonew.png" Style="height: 130px; width: 155px;" />
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>

                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnupload" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-section3-blank">
                                            &nbsp;
                                        </div>

                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>

                        <li>
                            <div id="accrdOther" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Settings</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">

                                        <div class="form-section3">
                                            <div class="section-ttle">Database Info.</div>
                                            <div class="input-field col s12">
                                                <div class="row">

                                                    <label class="drpdwn-label">Database Type </label>
                                                    <asp:DropDownList ID="ddlDBType" runat="server" CssClass="browser-default"
                                                        TabIndex="13">
                                                        <asp:ListItem Value="MSM">MSM</asp:ListItem>
                                                        <asp:ListItem Value="TS">TS</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="input-field col s12">
                                                <div class="row">

                                                    <label>Database </label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDB"
                                                        Display="None" ErrorMessage="Database Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator2">
                                                    </asp:ValidatorCalloutExtender>
                                                    <%--<asp:DropDownList ID="txtDB" runat="server" CssClass="browser-default"></asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtDB" runat="server" CssClass="validate"></asp:TextBox>
                                                </div>
                                            </div>





                                            <div class="input-field col s6">
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="chkMultilang" runat="server" class="css-checkbox" Text="Multi Language" />
                                                    <label for="chkMultilang"></label>
                                                </div>
                                            </div>

                                            <div class="input-field col s6">
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="chkMSEmail" runat="server" CssClass="css-checkbox" Text="Mobile Service Email" />
                                                    <label for="chkMSEmail"></label>
                                                </div>
                                            </div>

                                            <div class="input-field col s6">
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="ChkMultiCompany" runat="server" CssClass="css-checkbox" Text="Multi Company"></asp:CheckBox>
                                                    <label for="ChkMultiCompany">
                                                    </label>
                                                </div>
                                            </div>

                                            <div class="input-field col s6">
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="chkCustRegistrn" runat="server" class="css-checkbox" Text="Customer Registration" />
                                                    <label id="lblCustReg" runat="server" for="chkCustRegistrn" style="display: none"></label>
                                                </div>
                                            </div>



                                            <div class="input-field col s6">
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="ChkConsultantAPI" runat="server" CssClass="css-checkbox" Text="Consultant API"></asp:CheckBox>
                                                    <label id="lblConAPI" runat="server" for="ChkConsultantAPI" style="display: none;">
                                                    </label>
                                                </div>
                                            </div>

                                            <div class="input-field col s6" id="dvMultiOffice" runat="server" visible="false">
                                                <div class="checkrow">
                                                    <asp:CheckBox ID="chkMultiOffice" runat="server" CssClass="css-checkbox" Text="Multi Office"></asp:CheckBox>
                                                    <label id="lblMultiOffice" style="display: none;">Multi Office</label>

                                                </div>
                                            </div>

                                            <div class="input-field col s6">
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="ChkInventoryTracking" Text="Inventory Tracking" runat="server" CssClass="css-checkbox" onclick="ChkInventoryTrackingChanged(this)" />
                                                    <label for="ChkInventoryTracking" style="display: none;"></label>
                                                </div>
                                            </div>

                                            <div class="input-field col s6 divDrpAllChartAcct ">
                                                <div class="checkrow">
                                                    <asp:DropDownList runat="server" CssClass="browser-default" ID="DrpAllChartAcct"></asp:DropDownList>
                                                    <label class="drpdwn-label">Default Inventory Account </label>
                                                    <%--<asp:RequiredFieldValidator ID="rfvDrpAllChartAcct" runat="server"
                                                            ControlToValidate="ddlState" ValidationGroup="general, rep" InitialValue=":: Select ::" Display="None" ErrorMessage="Inventory Tracking Account Required"
                                                            SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                            ID="vcrfvDrpAllChartAcct"
                                                            runat="server" Enabled="True" TargetControlID="rfvDrpAllChartAcct">
                                                        </asp:ValidatorCalloutExtender>--%>
                                                    <%-- <asp:CheckBox ID="ChkInventoryTracking" Text="Inventory Tracking" runat="server" CssClass="css-checkbox" onclick="ChkInventoryTrackingChanged(this)" />
                                                        <label for="ChkInventoryTracking" style="display:none;"></label> mgnbtm8--%>
                                                </div>
                                            </div>




                                            <div class="input-field col s6 ">
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="chkpayroll" runat="server" CssClass="css-checkbox" Text="Payroll"></asp:CheckBox>
                                                    <label id="lblpayroll" style="display: none;">Payroll</label>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-section3-blank">
                                            &nbsp;
                                        </div>

                                        <div class="form-section3">
                                            <div class="section-ttle">More Details</div>
                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <%--<label for="rmks" class="txtbrdlbl">Notes </label>
                                                    <textarea id="rmks" class="textarea-border materialize-textarea" style="height: 125px !important; max-height: 125px !important;"></textarea>--%>
                                                    <label for="txtRemarks" class="txtbrdlbl">Notes </label>
                                                    <textarea id="txtRemarks" runat="server" class="textarea-border materialize-textarea materialize-2" style="height: 170px !important; max-height: 170px !important; border: 2px solid #ddd; border-radius: 5px;"></textarea>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-section3-blank">
                                            &nbsp;
                                        </div>

                                        <div class="form-section3">

                                            <div class="section-ttle">General Info.</div>
                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <label class="drpdwn-label">Year End Month <span style="color: red;">*</span></label>
                                                    <asp:DropDownList ID="ddlYearEnd" runat="server" CssClass="browser-default" onchange="onYearChanged(this)"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="input-field col s3">
                                                <div class="checkrow">


                                                    <asp:CheckBox ID="chkZone" runat="server" Text="Zone" CssClass="css-checkbox" />
                                                    <label for="chkZone" style="display: none;">&nbsp; </label>
                                                </div>
                                            </div>
                                            <div class="input-field col s1">
                                                <div class="row">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <div class="input-field col s4">
                                                <div class="checkrow">
                                                    <asp:CheckBox ID="chkUseTax" runat="server" Text="Use Tax for Location" CssClass="css-checkbox"></asp:CheckBox>
                                                    <label for="chkUseTax" style="display: none;">&nbsp;</label>
                                                </div>
                                            </div>
                                            <div class="input-field col s1">
                                                <div class="row">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <div class="input-field col s3">
                                                <div class="checkrow">
                                                    <asp:CheckBox ID="chkSalesTax2" runat="server" CssClass="css-checkbox" Text="Sales Tax2" />
                                                    <label for="chkSalesTax2" style="display: none;">&nbsp;Sales Tax2 </label>
                                                </div>
                                            </div>


                                            <div class="input-field col s12" id="divFederalID" runat="server">
                                                <div class="row">
                                                    <asp:TextBox ID="txtFederalID" runat="server" MaxLength="50" />
                                                    <asp:Label runat="server" ID="lblFedralID" AssociatedControlID="txtFederalID">Federal ID Number</asp:Label>
                                                    <%--                                                    <asp:CompareValidator runat="server" Display="None" ID="valtxtFedralId" Operator="DataTypeCheck" Type="Integer"
                                                        ControlToValidate="txtFederalID" ErrorMessage="Value must be a whole number" />
                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6"
                                                        runat="server" Enabled="True" PopupPosition="BottomRight" TargetControlID="valtxtFedralId">
                                                    </asp:ValidatorCalloutExtender>--%>
                                                </div>
                                            </div>
                                            <div class="input-field col s12" id="divStateID" runat="server">
                                                <div class="row">
                                                    <asp:TextBox ID="txtStateID" runat="server" MaxLength="50" />
                                                    <asp:Label runat="server" ID="lblStateID" AssociatedControlID="txtStateID">State ID Number</asp:Label>
                                                    <%--                                                    <asp:CompareValidator runat="server" Display="None" ID="valtxtStateID" Operator="DataTypeCheck" Type="Integer"
                                                        ControlToValidate="txtStateID" ErrorMessage="Value must be a whole number" />
                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7"
                                                        runat="server" Enabled="True" PopupPosition="BottomRight" TargetControlID="valtxtStateID">
                                                    </asp:ValidatorCalloutExtender>--%>
                                                </div>
                                            </div>
                                            <div class="input-field col s12" id="divProvincialID" runat="server">
                                                <div class="row">
                                                    <asp:TextBox ID="txtProvincialID" runat="server" MaxLength="50" />
                                                    <asp:Label runat="server" ID="lblProvincialID" AssociatedControlID="txtProvincialID">Provincial ID Number</asp:Label>
                                                    <%--                                                    <asp:CompareValidator runat="server" Display="None" ID="ValtxtProvincialID" Operator="DataTypeCheck" Type="Integer"
                                                        ControlToValidate="txtProvincialID" ErrorMessage="Value must be a whole number" />
                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender8"
                                                        runat="server" Enabled="True" PopupPosition="BottomRight" TargetControlID="ValtxtProvincialID">
                                                    </asp:ValidatorCalloutExtender>--%>
                                                </div>
                                            </div>

                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <asp:TextBox ID="txtNextInvoiceNumber" runat="server" MaxLength="50" />
                                                    <asp:Label runat="server" ID="lbltxtNextInvoiceNumber" AssociatedControlID="txtNextInvoiceNumber">Next Invoice Number</asp:Label>
                                                    <asp:CompareValidator runat="server" Display="None" ID="valtxtNextInvoiceNumber" Operator="DataTypeCheck" Type="Integer"
                                                        ControlToValidate="txtNextInvoiceNumber" ErrorMessage="Value must be a whole number" />
                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2"
                                                        runat="server" Enabled="True" PopupPosition="BottomRight" TargetControlID="valtxtNextInvoiceNumber">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>
                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <asp:TextBox ID="txtNextPONumber" runat="server" MaxLength="50" onblur="validatePONumber();" />
                                                    <asp:HiddenField ID="hdnNextPONumber" runat="server" />
                                                    <asp:Label runat="server" ID="lblNextPONumber" AssociatedControlID="txtNextPONumber">Next PO Number</asp:Label>
                                                    <asp:CompareValidator runat="server" Display="None" ID="valtxtNextPONumber" Operator="DataTypeCheck" Type="Integer"
                                                        ControlToValidate="txtNextPONumber" ErrorMessage="Value must be a whole number" />
                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3"
                                                        runat="server" Enabled="True" PopupPosition="BottomRight" TargetControlID="valtxtNextPONumber">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>
                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <asp:TextBox ID="txtNextEstimateNumber" runat="server" MaxLength="50" />
                                                    <asp:Label runat="server" ID="lblNextEstimateNumber" AssociatedControlID="txtNextEstimateNumber">Next Estimate Number</asp:Label>
                                                    <asp:CompareValidator runat="server" Display="None" ID="valtxtNextEstimateNumber" Operator="DataTypeCheck" Type="Integer"
                                                        ControlToValidate="txtNextEstimateNumber" ErrorMessage="Value must be a whole number" />
                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4"
                                                        runat="server" Enabled="True" PopupPosition="BottomRight" TargetControlID="valtxtNextEstimateNumber">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdPasswordSettings" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Password Settings</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section">
                                            <div class="section-ttle">
                                                <asp:CheckBox ID="chkApplyPasswordRules" runat="server" Text="Apply Password Policy"
                                                    CssClass="css-checkbox" onclick="ChkApplyPasswordRulesChanged(this)" /><%--  --%>
                                                <label for="chkApplyPasswordRules">&nbsp; </label>
                                            </div>
                                        </div>
                                        <div class="form-section4 divApplyPasswordRules">
                                            <div class="section-ttle">Apply to</div>
                                            <div class="row">
                                                <div class="input-field col s12">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkApplyOfficeUsers" runat="server" Text="Apply to Office Users" CssClass="css-checkbox" />
                                                        <label for="chkApplyOfficeUsers">&nbsp; </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row ">
                                                <div class="input-field col s12">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkApplyFieldUser" runat="server" Text="Apply to Field Users" CssClass="css-checkbox" />
                                                        <label for="chkApplyFieldUser">&nbsp; </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col s12">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkApplyCustomerUsers" runat="server" Text="Apply to Customer Portal Users" CssClass="css-checkbox" />
                                                        <label for="chkApplyCustomerUsers">&nbsp; </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section4-blank">
                                            &nbsp;
                                        </div>
                                        <div class="form-section8 divApplyPasswordRules">
                                            <div class="section-ttle">Password Reset</div>
                                            <div class="form-section2">
                                                <div class="row divApplyPasswordRules">
                                                    <div class="input-field col s6 p-b-10" >
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkPwResetDays" runat="server" Text="Password reset in #days"
                                                                CssClass="css-checkbox" onclick="ChkPwResetDaysChanged(this)" />
                                                            <label for="chkPwResetDays">&nbsp; </label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s6">
                                                        <asp:TextBox ID="txtPwResetDays" runat="server" />
                                                        <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Enabled="False"
                                                            Mask="9,999,999" TargetControlID="txtPwResetDays" MaskType="Number" DisplayMoney="Left">
                                                        </asp:MaskedEditExtender>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                            TargetControlID="txtPwResetDays" ValidChars="0123456789" Enabled="True">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvPwResetDays" runat="server" ControlToValidate="txtPwResetDays"
                                                            Display="None" ErrorMessage="Password reset in #days is required" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5"
                                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvPwResetDays">
                                                        </asp:ValidatorCalloutExtender>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Password Resetting option</label>
                                                        <asp:DropDownList ID="ddlPasswordResetOption" runat="server" ToolTip="Password Resetting"
                                                            CssClass="browser-default" TabIndex="10">
                                                            <asp:ListItem Value="0">Reset by Email</asp:ListItem>
                                                            <asp:ListItem Value="1">Reset by Admin</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section2-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section2">
                                                <%--<div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="rfvAdminEmail" runat="server" ControlToValidate="txtAdminEmail"
                                                            Display="None" ErrorMessage="Email Administrator is required" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="rfvAdminEmail_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvAdminEmail">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:RegularExpressionValidator ID="revAdminEmail" runat="server"
                                                            ControlToValidate="txtAdminEmail" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="revAdminEmail_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="revAdminEmail" PopupPosition="BottomLeft">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                            TargetControlID="txtEmail">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="txtAdminEmail" runat="server" CssClass="validate" MaxLength="50"></asp:TextBox>
                                                        <label for="txtAdminEmail">Email Administrator</label>
                                                    </div>
                                                </div>--%>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Administrator User</label>
                                                        <asp:DropDownList ID="ddlAdminUser" runat="server" ToolTip="Administrator User"
                                                            CssClass="browser-default" TabIndex="10">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="form-section4-blank">
                                            &nbsp;
                                        </div>
                                        <div class="form-section2 divApplyPasswordRules">
                                            <div class="section-ttle">Password Policy Info.</div>
                                            <ul class="b">
                                                <li>At least six characters in length</li>
                                                <li>Should not contain first 3 characters of their user's Account Name or user's First Name and/or Last Name.</li>
                                                <li>Should contain characters from all of the four categories:
                                                    <ul class="a">
                                                        <li>Must contain English uppercase characters (A through Z)</li>
                                                        <li>Must contain English lowercase characters (a through z)</li>
                                                        <li>Must contain Numerical digits (0 through 9)</li>
                                                        <li>Must contain a Non-alphabetic/Special Characters symbols (for example, !, $, #, %)</li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </div>--%>
                                        <div style="clear: both;"></div>
                                    </div>
                                </div>
                            </div>
                            <div style="clear: both;"></div>
                        </li>



                        <li>
                            <div id="accrdOnlinePaymentSetting" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Online Payment Settings</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section">
                                            <div class="section-ttle">
                                                <asp:CheckBox ID="chkOnllinePaymentSetting" runat="server" Text="Online Payment Setting"
                                                    CssClass="css-checkbox" onclick="return false" onkeydown="return false"/>
                                               
                                                <%--<asp:CheckBox ID="chkOnllinePaymentSetting" runat="server" Text="Online Payment Setting"
                                                    CssClass="css-checkbox" onclick="ChkApplyPasswordRulesChanged(this)" disabled="true" />--%>
                                                <label for="chkApplyPasswordRules">&nbsp; </label>
                                            </div>
                                        </div>
                                        <div class="form-section4 divApplyPasswordRules">
                                            <div class="section-ttle">Apply to</div>
                                            <div class="row">
                                                <div class="input-field col s12">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="CheckBox2" runat="server" Text="Apply to Office Users" CssClass="css-checkbox" />
                                                        <label for="chkApplyOfficeUsers">&nbsp; </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row ">
                                                <div class="input-field col s12">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="CheckBox3" runat="server" Text="Apply to Field Users" CssClass="css-checkbox" />
                                                        <label for="chkApplyFieldUser">&nbsp; </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col s12">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="CheckBox4" runat="server" Text="Apply to Customer Portal Users" CssClass="css-checkbox" />
                                                        <label for="chkApplyCustomerUsers">&nbsp; </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section4-blank">
                                            &nbsp;
                                        </div>
                                        <div class="form-section8 divApplyPasswordRules">
                                            <div class="section-ttle">Password Reset</div>
                                            <div class="form-section2">
                                                <div class="row divApplyPasswordRules">
                                                    <div class="input-field col s6 p-b-10" >
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="CheckBox5" runat="server" Text="Password reset in #days"
                                                                CssClass="css-checkbox" onclick="ChkPwResetDaysChanged(this)" />
                                                            <label for="chkPwResetDays">&nbsp; </label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s6">
                                                        <asp:TextBox ID="TextBox1" runat="server" />
                                                        <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Enabled="False"
                                                            Mask="9,999,999" TargetControlID="txtPwResetDays" MaskType="Number" DisplayMoney="Left">
                                                        </asp:MaskedEditExtender>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                            TargetControlID="txtPwResetDays" ValidChars="0123456789" Enabled="True">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPwResetDays"
                                                            Display="None" ErrorMessage="Password reset in #days is required" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6"
                                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvPwResetDays">
                                                        </asp:ValidatorCalloutExtender>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Password Resetting option</label>
                                                        <asp:DropDownList ID="DropDownList1" runat="server" ToolTip="Password Resetting"
                                                            CssClass="browser-default" TabIndex="10">
                                                            <asp:ListItem Value="0">Reset by Email</asp:ListItem>
                                                            <asp:ListItem Value="1">Reset by Admin</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section2-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section2">                                                
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Administrator User</label>
                                                        <asp:DropDownList ID="DropDownList2" runat="server" ToolTip="Administrator User"
                                                            CssClass="browser-default" TabIndex="10">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>                                       
                                        <div style="clear: both;"></div>
                                    </div>
                                </div>
                            </div>
                            <div style="clear: both;"></div>
                        </li>




                        <li>
                            <div id="accrdQB" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Quickbooks</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">Quickbooks Info.</div>
                                            <div class="form-section3">
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label id="Label1" for="chkAcctIntegration" runat="server" title="Sync data between MSM and Quickbooks"></label>
                                                        <asp:CheckBox ID="chkAcctIntegration" runat="server" CssClass="css-checkbox" Text="QB Integration"
                                                            ToolTip="Sync data between MSM and Quickbooks" OnCheckedChanged="chkAcctIntegration_CheckedChanged"
                                                            TabIndex="15" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s6  ">
                                                    <div class="row">
                                                        <label for="chkSyncEmp" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkSyncEmp" Text="Sync Employee Only" CssClass="css-checkbox" runat="server" />


                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                            <div class="form-section3">
                                                <div class="input-field col s5">
                                                    <div class="row">

                                                        <label class="drpdwn-label">Expense Service Item </label>
                                                        <asp:DropDownList runat="server" CssClass="browser-default" ID="ddlServiceExpense"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <div class="row">

                                                            <label class="drpdwn-label">Labor Service Item </label>
                                                            <asp:DropDownList runat="server" CssClass="browser-default" ID="ddlServicelabor"></asp:DropDownList>
                                                        </div>
                                                        <label id="Label2" runat="server" title="Give the path of Quickbooks data file located on the hosted server." visible="false">QB File Path (on server)</label>
                                                        <asp:TextBox ID="txtFilePath" runat="server" MaxLength="500" TabIndex="17" Width="200px"
                                                            ToolTip="Give the path of Quickbooks data file located on the hosted server."
                                                            Enabled="False" CssClass="form-control" Visible="False"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                              <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">


                                                <div class="input-field col s5">
                                                    <div class="row">

                                                        <label class="drpdwn-label">Mileage Service Item </label>
                                                        <asp:DropDownList runat="server" CssClass="browser-default " ID="ddlService"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>

                        <!--Start: API Changes - Juily:04/06/2020 -->

                        <li runat="server" id="liAPI">
                            <div id="accrdAPI" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Web API</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">Web API Settings</div>
                                            <div class="form-section3">
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label for="chkDashBoard" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkDashBoard" runat="server" CssClass="css-checkbox" Text="DashBoard" ToolTip="Web API Setting For DashBoard Module" OnCheckedChanged="chkDashBoard_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label for="chkCustomers" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkCustomers" runat="server" CssClass="css-checkbox" Text="Customers" ToolTip="Web API Setting For Customers Module" OnCheckedChanged="chkCustomers_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label for="chkRecurring" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkRecurring" runat="server" CssClass="css-checkbox" Text="Recurring" ToolTip="Web API Setting For Recurring Module" OnCheckedChanged="chkRecurring_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s6  ">
                                                    <div class="row">
                                                        <label for="chkSchedule" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkSchedule" runat="server" CssClass="css-checkbox" Text="Schedule" ToolTip="Web API Setting For Schedule Module" OnCheckedChanged="chkSchedule_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label for="chkBilling" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkBilling" runat="server" CssClass="css-checkbox" Text="Billing" ToolTip="Web API Setting For Billing Module" OnCheckedChanged="chkBilling_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label for="chkAP" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkAP" runat="server" CssClass="css-checkbox" Text="AP" ToolTip="Web API Setting For AP Module" OnCheckedChanged="chkAP_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-section3">
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label for="chkPurchasing" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkPurchasing" runat="server" CssClass="css-checkbox" Text="Purchasing" ToolTip="Web API Setting For Purchasing Module" OnCheckedChanged="chkPurchasing_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s6  ">
                                                    <div class="row">
                                                        <label for="chkSales" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkSales" runat="server" CssClass="css-checkbox" Text="Sales" ToolTip="Web API Setting For Sales Module" OnCheckedChanged="chkSales_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label for="chkProjects" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkProjects" runat="server" CssClass="css-checkbox" Text="Projects" ToolTip="Web API Setting For Projects Module" OnCheckedChanged="chkProjects_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s6  ">
                                                    <div class="row">
                                                        <label for="chkInventory" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkInventory" runat="server" CssClass="css-checkbox" Text="Inventory" ToolTip="Web API Setting For Inventory Module" OnCheckedChanged="chkInventory_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label for="chkFinancials" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkFinancials" runat="server" CssClass="css-checkbox" Text="Financials" ToolTip="Web API Setting For Financials Module" OnCheckedChanged="chkFinancials_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label for="chkStatements" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkStatements" runat="server" CssClass="css-checkbox" Text="Statements" ToolTip="Web API Setting For Statements Module" OnCheckedChanged="chkStatements_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <label for="chkPrograms" style="display: none;"></label>
                                                        <asp:CheckBox ID="chkPrograms" runat="server" CssClass="css-checkbox" Text="Programs" ToolTip="Web API Setting For Programs Module" OnCheckedChanged="chkPrograms_CheckedChanged" TabIndex="15" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>

                        <!--End: API Changes - Juily:04/06/2020 -->

                    </ul>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
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

        });
        function validatePONumber() {
            <%--var valueNext = document.getElementById('<%= txtNextPONumber.ClientID%>').value;
            var valueLast = document.getElementById('<%= hdnNextPONumber.ClientID%>').value;

            var str = "NextPO number can not be less then " + valueLast;
            if (parseInt(valueLast) > parseInt(valueNext)) {
                noty({ text: str, type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 5000, theme: 'noty_theme_default', closable: true });
                document.getElementById('<%= txtNextPONumber.ClientID%>').value = valueLast;
                var element = document.getElementById('<%= lblNextPONumber.ClientID%>');
                element.classList.add("active");
                               
            }--%>

        }
    </script>
    <script>

</script>
    <script type="text/javascript">
        function ChkApplyPasswordRulesChanged(e) {
            chkOnllinePaymentSetting.Enabled = false;
        }
    </script>
</asp:Content>

