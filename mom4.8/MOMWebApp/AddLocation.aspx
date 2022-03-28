<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" EnableEventValidation="false" AutoEventWireup="true" Inherits="AddLocation" ValidateRequest="false" CodeBehind="AddLocation.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection" />
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <script type="text/javascript">
        var countryList = [{ "name": "US", "value": "United States" }, { "name": "CA", "value": "Canada" }, { "name": "AF", "value": "Afghanistan" },
        { "name": "AL", "value": "Albania" }, { "name": "DZ", "value": "Algeria" },
        { "name": "AS", "value": "American Samoa" },
        { "name": "AD", "value": "Andorra" },
        { "name": "AO", "value": "Angola" },
        { "name": "AI", "value": "Anguilla" },
        { "name": "AQ", "value": "Antarctica" },
        { "name": "AG", "value": "Antigua And Barbuda" },
        { "name": "AR", "value": "Argentina" },
        { "name": "AM", "value": "Armenia" },
        { "name": "AW", "value": "Aruba" },
        { "name": "AU", "value": "Australia" },
        { "name": "AT", "value": "Austria" },
        { "name": "AZ", "value": "Azerbaijan" },
        { "name": "BS", "value": "Bahamas" },
        { "name": "BH", "value": "Bahrain" },
        { "name": "BD", "value": "Bangladesh" },
        { "name": "BB", "value": "Barbados" },
        { "name": "BY", "value": "Belarus" },
        { "name": "BE", "value": "Belgium" },
        { "name": "BZ", "value": "Belize" },
        { "name": "BJ", "value": "Benin" },
        { "name": "BM", "value": "Bermuda" },
        { "name": "BT", "value": "Bhutan" },
        { "name": "BO", "value": "Bolivia" },
        { "name": "BA", "value": "Bosnia And Herzegowina" },
        { "name": "BW", "value": "Botswana" },
        { "name": "BV", "value": "Bouvet Island" },
        { "name": "BR", "value": "Brazil" },
        { "name": "IO", "value": "British Indian Ocean Territory" },
        { "name": "BN", "value": "Brunei Darussalam" },
        { "name": "BG", "value": "Bulgaria" },
        { "name": "BF", "value": "Burkina Faso" },
        { "name": "BI", "value": "Burundi" },
        { "name": "KH", "value": "Cambodia" },
        { "name": "CM", "value": "Cameroon" },
        { "name": "CV", "value": "Cape Verde" },
        { "name": "KY", "value": "Cayman Islands" },
        { "name": "CF", "value": "Central African Republic" },
        { "name": "TD", "value": "Chad" },
        { "name": "CL", "value": "Chile" },
        { "name": "CN", "value": "China" },
        { "name": "CX", "value": "Christmas Island" },
        { "name": "CC", "value": "Cocos (Keeling) Islands" },
        { "name": "CO", "value": "Colombia" },
        { "name": "KM", "value": "Comoros" },
        { "name": "CG", "value": "Congo" },
        { "name": "CK", "value": "Cook Islands" },
        { "name": "CR", "value": "Costa Rica" },
        { "name": "CI", "value": "Cote D'Ivoire" },
        { "name": "CU", "value": "Cuba" },
        { "name": "CY", "value": "Cyprus" },
        { "name": "CZ", "value": "Czech Republic" },
        { "name": "DK", "value": "Denmark" },
        { "name": "DJ", "value": "Djibouti" },
        { "name": "DM", "value": "Dominica" },
        { "name": "DO", "value": "Dominican Republic" },
        { "name": "TP", "value": "East Timor" },
        { "name": "EC", "value": "Ecuador" },
        { "name": "EG", "value": "Egypt" },
        { "name": "SV", "value": "El Salvador" },
        { "name": "GQ", "value": "Equatorial Guinea" },
        { "name": "ER", "value": "Eritrea" },
        { "name": "EE", "value": "Estonia" },
        { "name": "ET", "value": "Ethiopia" },
        { "name": "FK", "value": "Falkland Islands (Malvinas)" },
        { "name": "FO", "value": "Faroe Islands" },
        { "name": "FJ", "value": "Fiji" },
        { "name": "FI", "value": "Finland" },
        { "name": "FR", "value": "France" },
        { "name": "GF", "value": "French Guiana" },
        { "name": "PF", "value": "French Polynesia" },
        { "name": "TF", "value": "French Southern Territories" },
        { "name": "GA", "value": "Gabon" },
        { "name": "GM", "value": "Gambia" },
        { "name": "GE", "value": "Georgia" },
        { "name": "DE", "value": "Germany" },
        { "name": "GH", "value": "Ghana" },
        { "name": "GI", "value": "Gibraltar" },
        { "name": "GR", "value": "Greece" },
        { "name": "GL", "value": "Greenland" },
        { "name": "GD", "value": "Grenada" },
        { "name": "GP", "value": "Guadeloupe" },
        { "name": "GU", "value": "Guam" },
        { "name": "GT", "value": "Guatemala" },
        { "name": "GN", "value": "Guinea" },
        { "name": "GW", "value": "Guinea-Bissau" },
        { "name": "GY", "value": "Guyana" },
        { "name": "HT", "value": "Haiti" },
        { "name": "HM", "value": "Heard And Mc Donald Islands" },
        { "name": "VA", "value": "Holy See (Vatican City State)" },
        { "name": "HN", "value": "Honduras" },
        { "name": "HK", "value": "Hong Kong" },
        { "name": "HU", "value": "Hungary" },
        { "name": "IS", "value": "Icel And" },
        { "name": "IN", "value": "India" },
        { "name": "ID", "value": "Indonesia" },
        { "name": "IR", "value": "Iran (Islamic Republic Of)" },
        { "name": "IQ", "value": "Iraq" },
        { "name": "IE", "value": "Ireland" },
        { "name": "IL", "value": "Israel" },
        { "name": "IT", "value": "Italy" },
        { "name": "JM", "value": "Jamaica" },
        { "name": "JP", "value": "Japan" },
        { "name": "JO", "value": "Jordan" },
        { "name": "KZ", "value": "Kazakhstan" },
        { "name": "KE", "value": "Kenya" },
        { "name": "KI", "value": "Kiribati" },
        { "name": "KP", "value": "Korea, Dem People'S Republic" },
        { "name": "KR", "value": "Korea, Republic Of" },
        { "name": "KW", "value": "Kuwait" },
        { "name": "KG", "value": "Kyrgyzstan" },
        { "name": "LA", "value": "Lao People'S Dem Republic" },
        { "name": "LV", "value": "Latvia" },
        { "name": "LB", "value": "Lebanon" },
        { "name": "LS", "value": "Lesotho" },
        { "name": "LR", "value": "Liberia" },
        { "name": "LY", "value": "Libyan Arab Jamahiriya" },
        { "name": "LI", "value": "Liechtenstein" },
        { "name": "LT", "value": "Lithuania" },
        { "name": "LU", "value": "Luxembourg" },
        { "name": "MO", "value": "Macau" },
        { "name": "MK", "value": "Macedonia" },
        { "name": "MG", "value": "Madagascar" },
        { "name": "MW", "value": "Malawi" },
        { "name": "MY", "value": "Malaysia" },
        { "name": "MV", "value": "Maldives" },
        { "name": "ML", "value": "Mali" },
        { "name": "MT", "value": "Malta" },
        { "name": "MH", "value": "Marshall Islands" },
        { "name": "MQ", "value": "Martinique" },
        { "name": "MR", "value": "Mauritania" },
        { "name": "MU", "value": "Mauritius" },
        { "name": "YT", "value": "Mayotte" },
        { "name": "MX", "value": "Mexico" },
        { "name": "FM", "value": "Micronesia, Federated States" },
        { "name": "MD", "value": "Moldova, Republic Of" },
        { "name": "MC", "value": "Monaco" },
        { "name": "MN", "value": "Mongolia" },
        { "name": "MS", "value": "Montserrat" },
        { "name": "MA", "value": "Morocco" },
        { "name": "MZ", "value": "Mozambique" },
        { "name": "MM", "value": "Myanmar" },
        { "name": "NA", "value": "Namibia" },
        { "name": "NR", "value": "Nauru" },
        { "name": "NP", "value": "Nepal" },
        { "name": "NL", "value": "Netherlands" },
        { "name": "AN", "value": "Netherlands Ant Illes" },
        { "name": "NC", "value": "New Caledonia" },
        { "name": "NZ", "value": "New Zealand" },
        { "name": "NI", "value": "Nicaragua" },
        { "name": "NE", "value": "Niger" },
        { "name": "NG", "value": "Nigeria" },
        { "name": "NU", "value": "Niue" },
        { "name": "NF", "value": "Norfolk Island" },
        { "name": "MP", "value": "Northern Mariana Islands" },
        { "name": "NO", "value": "Norway" },
        { "name": "OM", "value": "Oman" },
        { "name": "PK", "value": "Pakistan" },
        { "name": "PW", "value": "Palau" },
        { "name": "PA", "value": "Panama" },
        { "name": "PG", "value": "Papua New Guinea" },
        { "name": "PY", "value": "Paraguay" },
        { "name": "PE", "value": "Peru" },
        { "name": "PH", "value": "Philippines" },
        { "name": "PN", "value": "Pitcairn" },
        { "name": "PL", "value": "Poland" },
        { "name": "PT", "value": "Portugal" },
        { "name": "PR", "value": "Puerto Rico" },
        { "name": "QA", "value": "Qatar" },
        { "name": "RE", "value": "Reunion" },
        { "name": "RO", "value": "Romania" },
        { "name": "RU", "value": "Russian Federation" },
        { "name": "RW", "value": "Rwanda" },
        { "name": "KN", "value": "Saint K Itts And Nevis" },
        { "name": "LC", "value": "Saint Lucia" },
        { "name": "VC", "value": "Saint Vincent, The Grenadines" },
        { "name": "WS", "value": "Samoa" },
        { "name": "SM", "value": "San Marino" },
        { "name": "ST", "value": "Sao Tome And Principe" },
        { "name": "SA", "value": "Saudi Arabia" },
        { "name": "SN", "value": "Senegal" },
        { "name": "SC", "value": "Seychelles" },
        { "name": "SL", "value": "Sierra Leone" },
        { "name": "SG", "value": "Singapore" },
        { "name": "SK", "value": "Slovakia (Slovak Republic)" },
        { "name": "SI", "value": "Slovenia" },
        { "name": "SB", "value": "Solomon Islands" },
        { "name": "SO", "value": "Somalia" },
        { "name": "ZA", "value": "South Africa" },
        { "name": "GS", "value": "South Georgia , S Sandwich Is." },
        { "name": "ES", "value": "Spain" },
        { "name": "LK", "value": "Sri Lanka" },
        { "name": "SH", "value": "St. Helena" },
        { "name": "PM", "value": "St. Pierre And Miquelon" },
        { "name": "SD", "value": "Sudan" },
        { "name": "SR", "value": "Suri" },
        { "name": "SJ", "value": "Svalbard, Jan Mayen Islands" },
        { "name": "SZ", "value": "Sw Aziland" },
        { "name": "SE", "value": "Sweden" },
        { "name": "CH", "value": "Switzerland" },
        { "name": "SY", "value": "Syrian Arab Republic" },
        { "name": "TW", "value": "Taiwan" },
        { "name": "TJ", "value": "Tajikistan" },
        { "name": "TZ", "value": "Tanzania, United Republic Of" },
        { "name": "TH", "value": "Thailand" },
        { "name": "TG", "value": "Togo" },
        { "name": "TK", "value": "Tokelau" },
        { "name": "TO", "value": "Tonga" },
        { "name": "TT", "value": "Trinidad And Tobago" },
        { "name": "TN", "value": "Tunisia" },
        { "name": "TR", "value": "Turkey" },
        { "name": "TM", "value": "Turkmenistan" },
        { "name": "TC", "value": "Turks And Caicos Islands" },
        { "name": "TV", "value": "Tuvalu" },
        { "name": "UG", "value": "Uganda" },
        { "name": "UA", "value": "Ukraine" },
        { "name": "AE", "value": "United Arab Emirates" },
        { "name": "GB", "value": "United Kingdom" },
        { "name": "UM", "value": "United States Minor Is." },
        { "name": "UY", "value": "Uruguay" },
        { "name": "UZ", "value": "Uzbekistan" },
        { "name": "VU", "value": "Vanuatu" },
        { "name": "VE", "value": "Venezuela" },
        { "name": "VN", "value": "Viet Nam" },
        { "name": "VG", "value": "Virgin Islands (British)" },
        { "name": "VI", "value": "Virgin Islands (U.S.)" },
        { "name": "WF", "value": "Wallis And Futuna Islands" },
        { "name": "EH", "value": "Western Sahara" },
        { "name": "YE", "value": "Yemen" },
        { "name": "YU", "value": "Yugoslavia" },
        { "name": "ZR", "value": "Zaire" },
        { "name": "ZM", "value": "Zambia" }
        ];
        function CheckDeleteOpp() {
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
                alert('Please select an Opportunity to delete.')
                return false;
            }
        }

        $(function () {
            $("#<%= txtAddress.ClientID %>").geocomplete({
                map: false,
                details: "#divmain",
                types: ["geocode", "establishment"],
                address: "#<%= txtAddress.ClientID %>",
                city: "#<%= txtCity.ClientID %>",
                state: "#<%= txtState.ClientID %>",
                zip: "#<%= txtZip.ClientID %>",
                lat: "#<%= lat.ClientID %>",
                lng: "#<%= lng.ClientID %>"
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

                $("#<%=txtCountry.ClientID%>").val(getCountry);
                $("#<%=txtState.ClientID%>").val(cityAlt);
                $("#<%=txtZip.ClientID%>").val(countryCode);
                $("#<%=txtCity.ClientID%>").val(city);

                Materialize.updateTextFields();


                ChkAddress();
            });

            initialize();

        });

        $(function () {
            $("#<%= GCtxtAddress.ClientID %>").geocomplete({
                map: false,
                details: "#divmain",
                types: ["geocode", "establishment"],
                address: "#<%= GCtxtAddress.ClientID %>",
                city: "#<%= GCtxtcity.ClientID %>",
                state: "#<%= GCtxtState.ClientID %>",
                zip: "#<%= GCtxtPostalCode.ClientID %>"
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
                $("#<%=GCtxtCountry.ClientID%>").val(getCountry);
                $("#<%=GCtxtPostalCode.ClientID%>").val(countryCode);
                $("#<%=GCtxtState.ClientID%>").val(cityAlt);
                $("#<%=GCtxtcity.ClientID%>").val(city);
                Materialize.updateTextFields();
            });

        });

        $(function () {
            $("#<%= HotxtAddress.ClientID %>").geocomplete({
                map: false,
                details: "#divmain",
                types: ["geocode", "establishment"],
                address: "#<%= HotxtAddress.ClientID %>",
                city: "#<%= hotxtcity.ClientID %>",
                state: "#<%= hottxtstate.ClientID %>",
                zip: "#<%= hotxtZIP.ClientID %>"
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
                $("#<%=hotxtCountry.ClientID%>").val(getCountry);
                $("#<%=hotxtZIP.ClientID%>").val(countryCode);
                $("#<%=hottxtstate.ClientID%>").val(cityAlt);
                $("#<%=hotxtcity.ClientID%>").val(city);
                Materialize.updateTextFields();
            });
        });

        $(function () {
            $("#<%= txtBillAdd.ClientID %>").geocomplete({
                map: false,
                details: "#divmain",
                types: ["geocode", "establishment"],
                address: "#<%= txtBillAdd.ClientID %>",
                city: "#<%= txtBillCity.ClientID %>",
                state: "#<%= txtBillState.ClientID %>",
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
                //    for (var i = 0; i < result.address_components.length; i++) {
                //        var addr = result.address_components[i];
                //        if (addr.types[0] == 'administrative_area_level_2')
                //            cityAlt = addr.short_name;
                //    }
                $("#<%=drpBillCountry.ClientID%>").val(getCountry);
                $("#<%=txtBillZip.ClientID%>").val(countryCode);
                $("#<%=txtBillState.ClientID%>").val(cityAlt);
                $("#<%=txtBillCity.ClientID%>").val(city);
                Materialize.updateTextFields();
            });
        });



        function pageLoad() {

            ///////////// Ajax call for txtContcName auto search ////////////////////  

            var struid = $("#<%=hdnPatientId.ClientID%>").val();
            function dtaaon() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
                this.LocID = null;
                this.JobId = null;
            }
            $("[id*=txtContcName]").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaaon();
                    dtaaa.prefixText = request.term;
                    dtaaa.custID = 0;
                    dtaaa.LocID = 0;
                    dtaaa.JobId = 0;
                    if (struid != '') {
                        dtaaa.custID = struid;
                        dtaaa.LocID = 0;
                        dtaaa.JobId = 0;
                    }
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetContactAutojquery",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load customers");
                        }

                    });
                },
                select: function (event, ui) {
                    $("#<%=txtContcName.ClientID%>").val(ui.item.fDesc);
                    $("#<%=txtTitle.ClientID%>").val(ui.item.Title);
                    $("#<%=txtContPhone.ClientID%>").val(ui.item.Phone);
                    $("#<%=txtContFax.ClientID%>").val(ui.item.Fax);
                    $("#<%=txtContCell.ClientID%>").val(ui.item.Cell);
                    $("#<%=txtContEmail.ClientID%>").val(ui.item.Email);
                    return false;
                },
                focus: function (event, ui) {
                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
            $.each($(".Contact-search"), function (index, item) {
                $(item).data("autocomplete")._renderItem = function (ul, item) {

                    var result_item = item.fDesc;
                    var result_desc = item.Title + ',' + item.Phone;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here... 

                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });

                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }

                    return $("<li></li>")

                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);

                };
            });



            //////////////////////////////////

            ////////End         

            $addHandler($get("showModalPopupClientButton"), 'click', showModalPopupViaClient);
            $addHandler($get("A1"), 'click', hideModalPopupViaClientCust);

            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }
            $("[id*=txtContact]").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/getAlertContact",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },

                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message);
                        }
                    });
                },
                select: function (event, ui) {
                    var txtContact = this.id;
                    var hdnsid = document.getElementById(txtContact.replace('txtContact', 'hdnsid'));
                    var hdnsname = document.getElementById(txtContact.replace('txtContact', 'hdnsname'));
                    $(hdnsid).val(ui.item.screenid);
                    $(hdnsname).val(ui.item.screenname);
                    $(this).val(ui.item.name);
                    return false;
                },
                focus: function (event, ui) {
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .bind('click', function () { $(this).autocomplete("search"); })
            $.each($(".contactsearch"), function (index, item) {
                $(item).data("autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.name;
                    var result_desc = "Email: " + item.email + " Text: " + item.pager;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + ", <span style='color:gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);
                };
            });

        }

        function hideModalPopupViaClientCust(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.hide();
        }

        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }
        function ace_itemSelected(sender, e) {

            var hdnPatientId = document.getElementById('<%= hdnPatientId.ClientID %>');
            hdnPatientId.value = e.get_value();
            document.getElementById('<%= ddlCustomer.ClientID %>').value = hdnPatientId.value;
            document.getElementById('<%= btnSelectCustomer.ClientID %>').click();
        }
        function dispWarningContract() {

            noty({
                text: 'You cannot select the \'Combined on One Invoice\', As there are No Contracts added yet.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        $(document).ready(function () {
            $('#<%= txtCustomer.ClientID %>').keyup(function (event) {
                var hdnPatientId = document.getElementById('<%= hdnPatientId.ClientID %>');
                hdnPatientId.value = '';
            });

            $('#<%= txtAddress.ClientID %>').keyup(function (ev) {
                //                if (event.which != 27 && event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40 && event.which != 13) {
                if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                    var txtLat = document.getElementById('<%= lat.ClientID %>');
                    var txtLng = document.getElementById('<%= lng.ClientID %>');
                    txtLat.value = '';
                    txtLng.value = '';
                }
            });

            ///////////// Quick Codes //////////////
            $("#<%=txtRemarks.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRemarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

            $("#mapLink").click(function () {
                $("#map").toggle();
                initialize();
            });

        });

        function ChkCustomer(sender, args) {
            var hdnPatientId = document.getElementById('<%= hdnPatientId.ClientID %>');
            if (hdnPatientId.value == '') {
                args.IsValid = false;
            }
        }



        function getCountrybyValue(selectedVal) {
            for (var i = 0; i < countryList.length; i++) {
                var obj = countryList[i];
                if (obj.value == selectedVal)
                    return obj.name;
            }
        }

        function getCountrybyName(selectedVal) {
            for (var i = 0; i < countryList.length; i++) {
                var obj = countryList[i];
                if (obj.name == selectedVal)
                    return obj.name;
            }
        }

        function ChkAddress() {

            var txtBillAdd = document.getElementById('<%= txtBillAdd.ClientID %>');
            var txtBillCity = document.getElementById('<%= txtBillCity.ClientID %>');
            <%-- var ddlBillState = document.getElementById('<%= ddlBillState.ClientID %>');--%>
            var drpBillCountry = document.getElementById('<%= drpBillCountry.ClientID %>');
            var ddlBillState = document.getElementById('<%= txtBillState.ClientID %>');
           <%-- var drpBillCountry = document.getElementById('<%= txtBillCountry.ClientID %>'); --%>
            var txtBillZip = document.getElementById('<%= txtBillZip.ClientID %>');

            var txtAddress = document.getElementById('<%= txtAddress.ClientID %>');
            var txtCity = document.getElementById('<%= txtCity.ClientID %>');
            <%-- var ddlState = document.getElementById('<%= ddlState.ClientID %>');
            var ddlCountry = document.getElementById('<%= ddlCountry.ClientID %>');--%>
            var ddlState = document.getElementById('<%= txtState.ClientID %>');
            var ddlCountry = document.getElementById('<%= txtCountry.ClientID %>');
            var txtZip = document.getElementById('<%= txtZip.ClientID %>');

            var chkAdd = document.getElementById('<%= chkAddress.ClientID %>');

            if (chkAdd.checked == true) {
                txtBillAdd.value = txtAddress.value;
                txtBillCity.value = txtCity.value;
                ddlBillState.value = ddlState.value;
                var countryName = getCountrybyValue(ddlCountry.value);
                if (countryName === undefined || countryName === "") {
                    drpBillCountry.value = getCountrybyName(ddlCountry.value);

                } else {
                    drpBillCountry.value = countryName;
                }

                txtBillZip.value = txtZip.value;
                var chkCust = document.getElementById('<%= chkCustomerAddress.ClientID %>');
                chkCust.checked = false;

            }
            Materialize.updateTextFields();
        }

        function ChkCustomerAddress() {
            var txtBillAdd = document.getElementById('<%= txtBillAdd.ClientID %>');
            var txtBillCity = document.getElementById('<%= txtBillCity.ClientID %>');
            <%-- var ddlBillState = document.getElementById('<%= ddlBillState.ClientID %>');--%>
            var ddlBillState = document.getElementById('<%= txtBillState.ClientID %>');
            var txtBillZip = document.getElementById('<%= txtBillZip.ClientID %>');

            var txtAddress = document.getElementById('<%= txtAddress.ClientID %>');
            var txtCity = document.getElementById('<%= hdnCustomerCity.ClientID %>');
            var ddlState = document.getElementById('<%= hdnCustomerState.ClientID %>');
            var txtZip = document.getElementById('<%= hdnCustomerZipCode.ClientID %>');

            var chkAdd = document.getElementById('<%= chkCustomerAddress.ClientID %>');
            var CustomerAddress = $('#<%= hdnCustomerAddress.ClientID %>').val();
            var CustomerCountry = $('#<%= hdnCustomerCountry.ClientID %>').val();
            var drpBillCountry = document.getElementById('<%= drpBillCountry.ClientID %>');
            if (chkAdd.checked == true) {

                txtBillAdd.value = CustomerAddress;
                txtBillCity.value = txtCity.value;
                ddlBillState.value = ddlState.value;
                txtBillZip.value = txtZip.value;
                drpBillCountry.value = getCountrybyName(CustomerCountry);
                var chklocAdd = document.getElementById('<%= chkAddress.ClientID %>');
                chklocAdd.checked = false;

            }
            Materialize.updateTextFields();
        }


        function initialize() {
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
            //return SelectedRowDelete('<%= RadGrid_Documents.ClientID %>', 'file');
        }

        function editticket() {
            $("#<%=RadGrid_OpenCalls.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                    var ticket = $tr.find('span[id*=lblTicketId]').text();
                    var comp = $tr.find('span[id*=lblComp]').text();
                    var url = "addticket.aspx?id=" + ticket + "&comp=" + comp;
                    window.open(url, '_blank');
                });
            });
        }

        function NotApplyServiceTypeRule() {
            noty({ text: 'This location has multiple service contracts and cannot be updated automatically. Please update the service type manually in the recurring contract screen!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 10000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
        }

        function ApplyServiceTypeRule() {

            var hdnservice = document.getElementById('<%= hdnApplyServiceTypeRule.ClientID %>');

            if (hdnservice.value != '0') {

                var NK = false;

                NK = confirm(' This change will update projects/recurring contracts #' + document.getElementById('<%= hdnProjectaregoingtoUpdate.ClientID %>').value + '. Would you like the system to update?');

                if (!NK) { hdnservice.val("0"); }
            }
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
                            ret = confirm('Account # edited, this will create a new Job in Sage and make existing inactive. Do you want to continue?');
                        }
                    }
                }
                // return ret;
                //confirmation for GC and Homeowner 
                if (ret) { ret = ConfirmGCAdd(); }
                return ret
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
                textarea.value = lines.slice(0, 2).join("\n");
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
        function AddNewCOntact(textbox) {
            ////alert(textbox);
            //var add = $(textbox).closest('table').closest('tr').prev().prev().children("td.addbutton1").attr('id');
            //alert(add);
            //$(add).click();
        }

        //////////////// Confirm Edit Location ///////////////////
        function notyConfirm() {
            noty({
                dismissQueue: true,
                layout: 'topCenter',
                theme: 'noty_theme_default',
                animateOpen: { height: 'toggle' },
                animateClose: { height: 'toggle' },
                easing: 'swing',
                text: 'Do you want to further edit the location?',
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
                buttons: [
                    {
                        type: 'btn btn-primary', text: 'Ok', click: function ($noty) {
                            $noty.close();
                            window.location.href = "addlocation.aspx?uid=" + $("#<%=hdnAddedLoc.ClientID%>").val();

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
                buttons: [
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
        function RedirectPage() {
            var PageQueryString = $("#<%=hdnPageQueryString.ClientID%>");
            if (PageQueryString.val().trim() != '') {
                window.location.href = "addlocation.aspx?uid=" + $("#<%=hdnAddedLoc.ClientID%>").val() + "&AddEquip=true&" + PageQueryString.val().trim();

            } else {
                window.location.href = "addlocation.aspx?uid=" + $("#<%=hdnAddedLoc.ClientID%>").val() + "&AddEquip=true";
            }
        }

        $(document).ready(function () {

            /////Homeowner////
            $("#<%=hotxtname.ClientID%>").keyup(function (event) {

                var hdnHOId = $("#<%=hdnHomeOwnerID.ClientID%>");
                var hdnHOName = $("#<%=hdnHOName.ClientID%>");
                var hdnHONameupdate = $("#<%=hdnHONameupdate.ClientID%>");

                if (hdnHOId.val() > 0) {
                    if (this.value.trim() != hdnHOName.val().trim() && this.value.trim() != '')
                        hdnHONameupdate.val('1');
                    hdnHOId.val('0');
                }

            });


            /////GC info 

            $("#<%=GCtxtName.ClientID%>").keyup(function (event) {

                var hdnGCId = $("#<%=hdnGContractorID.ClientID%>");
                var hdnGCName = $("#<%=hdnGCName.ClientID%>");
                var hdnGCNameupdate = $("#<%=hdnGCNameupdate.ClientID%>");

                if (hdnGCId.val() > 0) {
                    if (this.value.trim() != hdnGCName.val().trim() && this.value.trim() != '')
                        hdnGCNameupdate.val('1');
                    hdnGCId.val(0);
                }

            });


            ///////////// Ajax Call for Homeowner Auto Search ////////////////////                
            var query = "";
            function dta() {
                this.prefixText = null;
                this.con = "";
                this.type = "";
            }
            $("#<%= hotxtname.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dta();
                    dtaaa.prefixText = request.term;
                    dtaaa.type = "2";
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetGCorHomeOwner",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },

                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message);
                        }
                    });
                },

                select: function (event, ui) {
                    $("#<%= hotxtname.ClientID%>").val('');
                    $("#<%= hotxtcity.ClientID%>").val('');
                    $("#<%= HotxtAddress.ClientID%>").val('');
                            <%--$("#<%= hotddlstate.ClientID%>").val('');--%>
                    $("#<%= hottxtstate.ClientID%>").val('');
                    $("#<%= hotxtZIP.ClientID%>").val('');
                    $("#<%= hotxtCountry.ClientID%>").val('');
                    $("#<%= hotxtCName.ClientID%>").val('');
                    $("#<%= hotxtPhone.ClientID%>").val('');
                    $("#<%= HotxtFax.ClientID%>").val('');
                    $("#<%= HotxtEmailWeb.ClientID%>").val('');
                    $("#<%= hotxtMobile.ClientID%>").val('');
                    $("#<%= hotxtRemarks.ClientID%>").val('');
                    $("#<%= hdnHomeOwnerID.ClientID%>").val('0');
                    $("#<%= hdnHOName.ClientID%>").val('');
                    $("#<%= hdnHONameupdate.ClientID%>").val(0);

                    $("#<%= hotxtname.ClientID%>").val(ui.item.label);
                    $("#<%= hotxtcity.ClientID%>").val(ui.item.city);
                    $("#<%= HotxtAddress.ClientID%>").val(ui.item.Address);
                            <%--$("#<%= hotddlstate.ClientID%>").val(ui.item.state);--%>
                    $("#<%= hottxtstate.ClientID%>").val(ui.item.state);
                    $("#<%= hotxtZIP.ClientID%>").val(ui.item.zip);
                    $("#<%= hotxtCountry.ClientID%>").val(ui.item.country);
                    $("#<%= hotxtCName.ClientID%>").val(ui.item.contact);
                    $("#<%= hotxtPhone.ClientID%>").val(ui.item.phone);
                    $("#<%= HotxtFax.ClientID%>").val(ui.item.fax);
                    $("#<%= HotxtEmailWeb.ClientID%>").val(ui.item.email);
                    $("#<%= hotxtMobile.ClientID%>").val(ui.item.cellular);
                    $("#<%= hotxtRemarks.ClientID%>").val(ui.item.remarks);
                    $("#<%= hdnHomeOwnerID.ClientID%>").val(ui.item.GC_HOid);
                    $("#<%= hdnHOName.ClientID%>").val(ui.item.label);


                    return false;
                },
                focus: function (event, ui) {
                    return false;
                },
                minLength: 0,
                delay: 250
            })._renderItem = function (ul, item) {
                var result_item = item.label;
                var result_desc = item.desc;
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
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a style='color:" + color + ";'>" + result_item + ", <span style='color:gray;'>" + result_desc + "</span></a>")
                    .appendTo(ul);
            };





            ///////////// Ajax call for GC auto search ////////////////////                
            var query = "";
            function dta() {
                this.prefixText = null;
                this.con = "";
                this.type = "";
            }
            $("#<%= GCtxtName.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dta();
                    dtaaa.prefixText = request.term;
                    dtaaa.type = "1";
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetGCorHomeOwner",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        //error: function(result) {
                        //    alert("Due to unexpected errors we were unable to load customers");
                        //}
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message);
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%= GCtxtName.ClientID%>").val('');
                    $("#<%= GCtxtcity.ClientID%>").val('');
                    $("#<%= GCtxtAddress.ClientID%>").val('');
                            <%--  $("#<%= GCddlState.ClientID%>").val('');--%>
                    $("#<%= GCtxtState.ClientID%>").val('');
                    $("#<%= GCtxtPostalCode.ClientID%>").val('');
                    $("#<%= GCtxtCountry.ClientID%>").val('');
                    $("#<%= GCtxtCName.ClientID%>").val('');
                    $("#<%= GCtxtPhone.ClientID%>").val('');
                    $("#<%= GCtxtFAX.ClientID%>").val('');
                    $("#<%= GCtxtEmailWeb.ClientID%>").val('');
                    $("#<%= GCtxtMobile.ClientID%>").val('');
                    $("#<%= GCtxtRemarks.ClientID%>").val('');
                    $("#<%= hdnGCNameupdate.ClientID%>").val(0);
                    $("#<%= hdnGCName.ClientID%>").val('');
                    $("#<%= hdnGContractorID.ClientID%>").val(0);
                    var gci = $("#<%=hdnGContractorID.ClientID%>");
                    console.log(gci.val());

                    $("#<%= GCtxtName.ClientID%>").val(ui.item.label);
                    $("#<%= GCtxtcity.ClientID%>").val(ui.item.city);
                    $("#<%= GCtxtAddress.ClientID%>").val(ui.item.Address);
                            <%-- $("#<%= GCddlState.ClientID%>").val(ui.item.state);--%>
                    $("#<%= GCtxtState.ClientID%>").val(ui.item.state);
                    $("#<%= GCtxtPostalCode.ClientID%>").val(ui.item.zip);
                    $("#<%= GCtxtCountry.ClientID%>").val(ui.item.country);
                    $("#<%= GCtxtCName.ClientID%>").val(ui.item.contact);
                    $("#<%= GCtxtPhone.ClientID%>").val(ui.item.phone);
                    $("#<%= GCtxtFAX.ClientID%>").val(ui.item.fax);
                    $("#<%= GCtxtEmailWeb.ClientID%>").val(ui.item.email);
                    $("#<%= GCtxtMobile.ClientID%>").val(ui.item.cellular);
                    $("#<%= GCtxtRemarks.ClientID%>").val(ui.item.remarks);
                    $("#<%= hdnGContractorID.ClientID%>").val(ui.item.GC_HOid);
                    $("#<%= hdnGCNameupdate.ClientID%>").val(0);
                    $("#<%= hdnGCName.ClientID%>").val(ui.item.label);
                    var gci1 = $("#<%=hdnGContractorID.ClientID%>");
                    console.log(gci1.val());
                    return false;
                },
                focus: function (event, ui) {
                    return false;
                },
                minLength: 0,
                delay: 250
            })._renderItem = function (ul, item) {
                var result_item = item.label;
                var result_desc = item.desc;
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
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a style='color:" + color + ";'>" + result_item + ", <span style='color:gray;'>" + result_desc + "</span></a>")
                    .appendTo(ul);
            };

        });

        function ConfirmGCAdd() {
            var hdnGCNameupdate = $('#<%= hdnGCNameupdate.ClientID%>');
            var gcName = $('#<%= GCtxtName.ClientID%>');
            var ret = true;

            if (hdnGCNameupdate.val().trim() == '1') {
                //Confirm for GC
                ret = confirm(gcName.val() + ' will be created as a new General Contractor. Do you want to continue?');
            }

            if (ret) {
                //Confirm for Home owner
                var hdnHONameupdate = $('#<%= hdnHONameupdate.ClientID%>');
                var hotxtname = $('#<%= hotxtname.ClientID%>');
                if (hdnHONameupdate.val().trim() == '1') {
                    ret = confirm(hotxtname.val() + ' will be created as a new Homeowner. Do you want to continue?');
                }
            }
            return ret;
        }
                ///////////////////////////////////////
    </script>

    <script type="text/javascript">
        function ResetShowAllHistory() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $('#<%=lblQuarter.ClientID%>').removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
            var ddlCategory = $("#<%=ddlCategory.ClientID%>");
            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");
            ddlCategory.css("display", "none");
            txtSearch.css("display", "block");


        }
        function showFilterSearchHistory() {

            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");
            var ddlCategory = $("#<%=ddlCategory.ClientID%>");
            txtSearch.css("display", "none");
            ddlCategory.css("display", "none");
            ddlCategory.val("None");
            txtSearch.val("");
            var ddlCategory = $("#<%=ddlCategory.ClientID%>");
            if (ddlSearch.val() === 't.cat') {
                ddlCategory.css("display", "block");
            } else {
                txtSearch.css("display", "block");
            }

        }
        function dec_dateHistory(select, rdGroup) {
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
                var tt = document.getElementById('<%=txtfromDate.ClientID%>').value;

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

            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $('#<%=lblQuarter.ClientID%>').removeClass("labelactive");
            $('#<%=lblYearInv.ClientID%>').removeClass("labelactive");
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById('<%=txtToDate.ClientID%>').value = datestring;
                document.getElementById('<%=txtfromDate.ClientID%>').value = datestring;
                $('#<%=lblDay.ClientID%>').addClass("labelactive");
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
                $('#<%=lblWeek.ClientID%>').addClass("labelactive");

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

                $('#<%=lblMonth.ClientID%>').addClass("labelactive");
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
                $('#<%=lblQuarter.ClientID%>').addClass("labelactive");
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
                $('#<%=lblYear.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRangeHistory.ClientID%>').value = "rdYear";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnSelectedDtRangeHistory.ClientID%>').value);

            }
        }

        $(document).ready(function () {
            valHistory = document.getElementById('<%=hdnSelectedDtRangeHistory.ClientID%>').value;
            if (valHistory == "") {
                valHistory = "rdMonth";
            }
            if (valHistory == 'rdDay') {

                $("#<%=lblDay.ClientID%>").addClass("labelactive");
                document.getElementById("rdDay").checked = true;

            }
            if (valHistory == 'rdWeek') {

                $("#<%=lblWeek.ClientID%>").addClass("labelactive");

                document.getElementById("rdWeek").checked = true;

            }
            if (valHistory == 'rdMonth') {
                $("#<%=lblMonth.ClientID%>").addClass("labelactive");
                document.getElementById("rdMonth").checked = true;

            }
            if (valHistory == 'rdQuarter') {

                $("#<%=lblQuarter.ClientID%>").addClass("labelactive");
                document.getElementById("rdQuarter").checked = true;

            }
            if (valHistory == 'rdYear') {

                $("#<%=lblYear.ClientID%>").addClass("labelactive");
                document.getElementById("rdYear").checked = true;

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
        function clickLink(linkName) {
            debugger
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
        } else {
            clickLink("#lnkLocInfo");
        }
    </script>

    <script type="text/javascript">


        function ResetShowAll() {
            $('#<%=lblDayInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeekInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonthInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblQuarterInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblYearInv.ClientID%>').removeClass("labelactive");
            document.getElementById('rdAll2').checked = true;
            document.getElementById('rdAll').checked = true;
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem("hdnInvDate", document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value);

            }
        }
        function ResetShowAllOpen() {
            $('#<%=lblDayInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeekInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonthInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblQuarterInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblYearInv.ClientID%>').removeClass("labelactive");
            document.getElementById('rdAll2').checked = true;
            document.getElementById('rdOpen').checked = true;
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem("hdnInvDate", document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value);

            }
        }
        function dec_date(select, rdGroup) {
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


            if (selected == 'rdDay') {

                //dec the from date 
                var tt = document.getElementById('<%=txtInvDtFrom.ClientID%>').value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xday);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%=txtInvDtFrom.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%=txtInvDtTo.ClientID%>').value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xday);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById('<%=txtInvDtTo.ClientID%>').value = someFormattedDATE;


            }
            else if (selected == 'rdWeek') {
                //dec the from date 
                var tt = document.getElementById('<%=txtInvDtFrom.ClientID%>').value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xWeek);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%=txtInvDtFrom.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%=txtInvDtTo.ClientID%>').value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xWeek);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById('<%=txtInvDtTo.ClientID%>').value = someFormattedDATE;
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


                var tt = document.getElementById('<%=txtInvDtFrom.ClientID%>').value;

                var date = new Date(tt).toDateString();
                var newdate = new Date(date);

                newdate.addMonths(xMonth);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%=txtInvDtFrom.ClientID%>').value = someFormattedDate;


                //dec the to date 
                if (select == 'dec') {
                    var ti = document.getElementById('<%=txtInvDtTo.ClientID%>').value;


                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLastDec(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById('<%=txtInvDtTo.ClientID%>').value = someFormattedDate;
                }

                else {
                    var ti = document.getElementById('<%=txtInvDtTo.ClientID%>').value;
                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLast(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById('<%=txtInvDtTo.ClientID%>').value = someFormattedDate;
                }

            }


            else if (selected == 'rdQuarter') {
                //dec the from date
                var tt = document.getElementById('<%=txtInvDtFrom.ClientID%>').value;
                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setMonth(newdate.getMonth() + xQuarter);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%=txtInvDtFrom.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%=txtInvDtTo.ClientID%>').value;

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
                document.getElementById('<%=txtInvDtTo.ClientID%>').value = someFormattedDATE;
            }
            else if (selected == 'rdYear') {

                var tt = document.getElementById('<%=txtInvDtFrom.ClientID%>').value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setFullYear(newdate.getFullYear() + xYear);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%=txtInvDtFrom.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%=txtInvDtTo.ClientID%>').value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);




                NEWDATE.setFullYear(NEWDATE.getFullYear() + xYear);
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById('<%=txtInvDtTo.ClientID%>').value = someFormattedDATE;


            }

            return false;

        }

        function SelectDate(type, UniqueVal) {
            var type = type;
            var UniqueVal = UniqueVal;

            $('#<%=lblDayInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeekInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonthInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblQuarterInv.ClientID%>').removeClass("labelactive");
            $('#<%=lblYearInv.ClientID%>').removeClass("labelactive");
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById('<%=txtInvDtTo.ClientID%>').value = datestring;
                document.getElementById('<%=txtInvDtFrom.ClientID%>').value = datestring;
                $('#<%=lblDayInv.ClientID%>').addClass("labelactive");
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
                document.getElementById('<%=txtInvDtFrom.ClientID%>').value = datestring;
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById('<%=txtInvDtTo.ClientID%>').value = dateString;
                $('#<%=lblWeekInv.ClientID%>').addClass("labelactive");

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
                document.getElementById('<%=txtInvDtFrom.ClientID%>').value = datestring;

                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById('<%=txtInvDtTo.ClientID%>').value = dateString;

                $('#<%=lblMonthInv.ClientID%>').addClass("labelactive");
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
                document.getElementById('<%=txtInvDtFrom.ClientID%>').value = datestring;

                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById('<%=txtInvDtTo.ClientID%>').value = dateString;
                $('#<%=lblQuarterInv.ClientID%>').addClass("labelactive");
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
                document.getElementById('<%=txtInvDtFrom.ClientID%>').value = datestring;

                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById('<%=txtInvDtTo.ClientID%>').value = dateString;
                $('#<%=lblYearInv.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnSelectedDtRange.ClientID%>').value);

            }
        }

        function showFilterSearch() {

            var ddlSearchInv = $("#<%=ddlSearchInv.ClientID%>");
            var txtInvDt = $("#<%=txtInvDt.ClientID%>");
            var txtSearchInv = $("#<%=txtSearchInv.ClientID%>");

            if (ddlSearchInv.val() === 'i.ref') {
                txtInvDt.css("display", "none");
                txtSearchInv.css("display", "block");
            } else {
                txtSearchInv.css("display", "none");
                txtInvDt.css("display", "block");
            }

        }





        ///-ticket permission

        function AddTicketClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeTicket.ClientID%>').value;
            if (id == "Y") {
                if (document.getElementById('<%= chkCreditHold.ClientID%>').checked) {
                    noty({ text: 'Location on credit hold can not create ticket!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
                else {
                    return true;
                }
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditTicketClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeTicket.ClientID%>').value;
            if (id == "Y") { return editticket(); } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }



        ///- job permission
        function AddJobClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeJob.ClientID%>').value;
            if (id == "Y") {
                //alert(hi);
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditJobClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeJob.ClientID%>').value;
            var view = document.getElementById('<%= hdnViewJob.ClientID%>').value;
            if (id == "Y" || view == "Y") {
                return CheckEdit();
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteJobClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteJob.ClientID%>').value;
            if (id == "Y") {
                return CheckDelete();
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }



        ///-Contact permission

        function AddContactClick(hyperlink) {

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
                ConfirmUpload(hyperlink.value);

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

        function SelectRowmailPage(mail, lnkMail) {


            var maillink = document.getElementById(lnkMail);
            var mailid = document.getElementById(mail);


            maillink.href = 'mailto:' + mailid.innerHTML;

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
        function CopyEquipmentClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeEquipment.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function checked1() {
            if (document.getElementById('<%= CopyToLocAndJob.ClientID %>').checked) {
                alert("Copy above Billing Rates to Project ?");
            }
        }
    </script>

    <style>
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
        [id$='RadGrid_Invoice'] .rgHeader > a {
            white-space: nowrap;
            padding-left: 0 !important;
        }

        .RadFilterMenu_CheckList {
            top: 20px;
        }

        .disableButton {
            background-image: none !important;
            background-color: #ddd;
            color: #999;
            border: 0.5px solid #adadad;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <telerik:RadAjaxManager ID="RadAjaxManager_Location" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkDeleteTicket">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OpenCalls" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OpenCalls" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                    <telerik:AjaxUpdatedControl ControlID="hdnSelectedDtRange" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAllTicket">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OpenCalls" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                    <telerik:AjaxUpdatedControl ControlID="txtfromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearch" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkSearchInv">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtTo" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtFrom" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_Invoice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />

                </UpdatedControls>
            </telerik:AjaxSetting>


            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />

                    <telerik:AjaxUpdatedControl ControlID="ddlSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDt" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtTo" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtFrom" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtTo" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtFrom" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDt" />
                    <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAllOpen">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtTo" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDtFrom" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearchInv" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDt" />
                    <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkDeleteInvoice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" LoadingPanelID="RadAjaxLoadingPanel_Location" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkAddnew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkAddCustomer">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowAddCustomer" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkShowLog">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContactLog" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <%--  <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlContractGridButtons" />
                    <telerik:AjaxUpdatedControl ControlID="lnkDeleteDoc"/>
                    <telerik:AjaxUpdatedControl ControlID="pnlEqGridButtons"/>
                    <telerik:AjaxUpdatedControl ControlID="pnlOppGridButtons"/>
                      <telerik:AjaxUpdatedControl ControlID="pnlProjectGridButtons"/>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Location" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="divbutton-container">
        <div id="divButtons">

            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-maps-place"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Add Location</asp:Label></div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" OnClientClick="return AlertSageIDUpdate();" ValidationGroup="ValidateSave">Save</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="LocationReport.aspx" data-activates="dropdown1">Reports
                                            </a>
                                            <ul id="dropdown1" class="dropdown-content">
                                                <li>
                                                    <a href="LocationReport.aspx?type=Location" class="-text">Add New Report</a>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lnkLocationHistory" runat="server" OnClick="lnkLocationHistory_Click">Location History Report</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lnkLocationTransLedger" runat="server" OnClick="lnkLocationTransLedger_Click">Location Transaction Ledger Report</asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>

                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label ID="lblLocationName" runat="server"></asp:Label>
                                            <asp:Label ID="lblLocationNameLabel" runat="server"></asp:Label>
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
                                    <li><a id="lnkLocInfo" href="#accrdlocInfo">Location Info</a></li>
                                    <li runat="server" id="liContacts"><a href="#accrdcontacts">Contacts</a></li>
                                    <li runat="server" id="liDocuments"><a href="#accrddocuments">Documents</a></li>
                                    <li runat="server" id="liViewEquipments"><a id="lnkEquipmentaccrd" href="#accrdequipments">Equipment</a></li>
                                    <li runat="server" id="liLocationHistory"><a href="#accrdlocationHistory">Location History</a></li>
                                    <li runat="server" id="liTransactions"><a id="lnkTransactionaccrd" href="#accrdtransactions">Transactions</a></li>
                                    <li runat="server" id="liOpportunities"><a href="#accrdopportunities">Opportunities</a></li>
                                    <li runat="server" id="liProjects"><a href="#accrdprojects">Projects</a></li>
                                    <li runat="server" id="liAlerts"><a href="#accrdalerts">Alerts</a></li>
                                    <li runat="server" id="liGCInformation"><a href="#accrdgcInformation">GC Information</a></li>
                                    <li runat="server" id="liHomeowner"><a href="#accrdhomeowner">Homeowner</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlSave" runat="server">
                                        <asp:Panel ID="pnlNext" runat="server" Visible="false">
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" OnClick="lnkFirst_Click"
                                                    CausesValidation="False">
                                                        <i class="fa fa-angle-double-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False"
                                                    OnClick="lnkPrevious_Click">
                                                        <i class="fa fa-angle-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False"
                                                    OnClick="lnkNext_Click">
                                                        <i class="fa fa-angle-right"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CssClass="icon-last" CausesValidation="False"
                                                    OnClick="lnkLast_Click">
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
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head expandable">
                        <li>
                            <div id="accrdlocInfo" class="collapsible-header accrd accordian-text-custom active"><i class="mdi-maps-map"></i>Location Info</div>
                            <div class="collapsible-body" id="firstTab">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">

                                            <input id="hdnPatientId" runat="server" type="hidden" />
                                            <asp:HiddenField ID="hdnMail" runat="server" />
                                            <asp:HiddenField ID="hdnSageIntegration" runat="server" />
                                            <asp:HiddenField ID="hdnAddedLoc" runat="server" />

                                            <div class="form-input-row">
                                                <div class="form-section3">
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:Label ID="Label4" Visible="False" runat="server" Text="ID"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        &nbsp;
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:Label ID="Label5" Visible="False" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="section-ttle">Location Details</div>
                                            <div class="form-input-row">
                                                <div class="form-section3">
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:Label runat="server" ID="lblCustomer" AssociatedControlID="txtCustomer">Customer Name <span class="reqd">*</span></asp:Label>

                                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtCustomer"
                                                                ErrorMessage="Please select the existing customer" ClientValidationFunction="ChkCustomer"
                                                                Display="None">
                                                            </asp:CustomValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2"
                                                                runat="server" Enabled="True" TargetControlID="CustomValidator1">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" ValidationGroup="ValidateSave" runat="server" ControlToValidate="txtCustomer"
                                                                Display="None" ErrorMessage="Customer Required" SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator19_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator19">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlCustomer"
                                                                Display="None" ErrorMessage="Customer Required" SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender
                                                                ID="ValidatorCalloutExtender1" runat="server" Enabled="True" TargetControlID="RequiredFieldValidator5">
                                                            </asp:ValidatorCalloutExtender>


                                                            <asp:TextBox ID="txtCustomer" runat="server" AutoCompleteType="Disabled" autocomplete="*"></asp:TextBox>
                                                            <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtCustomer"
                                                                EnableCaching="False" ServiceMethod="GetCustomers11" UseContextKey="True" MinimumPrefixLength="0"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" OnClientItemSelected="ace_itemSelected"
                                                                ID="AutoCompleteExtender" DelimiterCharacters="" CompletionInterval="250">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:DropDownList ID="ddlCustomer" Style="display: none;" runat="server" CssClass="browser-default" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:Button CausesValidation="False" ID="btnSelectCustomer" runat="server" Text="Button"
                                                                Style="display: none;" OnClick="ddlCustomer_SelectedIndexChanged" />
                                                        </div>
                                                    </div>
                                                    <div class="srchclr btnlinksicon rowbtn">
                                                        <asp:LinkButton runat="server" CausesValidation="false" OnClick="lnkAddCustomer_Click" ID="lnkAddCustomer"><i class="mdi-social-person-add"></i></asp:LinkButton>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:Label ID="locLabel" AssociatedControlID="txtLocName" runat="server">Location Name <span class="reqd">*</span></asp:Label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                ControlToValidate="txtLocName" ValidationGroup="ValidateSave" Display="None" ErrorMessage="Location Name Required"
                                                                SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator1">
                                                            </asp:ValidatorCalloutExtender>

                                                            <asp:TextBox ID="txtLocName" runat="server" AutoCompleteType="Disabled" MaxLength="100"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="srchclr multirowbtn multirowbtnone">
                                                        <img id="imgCreditH" visible="false" runat="server" title="Credit Hold" src="images/MSCreditHold.png" style="width: 20px;">
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblAccountNo" AssociatedControlID="txtAcctno" runat="server">Account #</asp:Label>
                                                                    <asp:TextBox ID="txtAcctno" runat="server" MaxLength="50" AutoCompleteType="Disabled"></asp:TextBox>


                                                                    <asp:HiddenField ID="hdnAcctID" runat="server" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <%--                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtAcctno"    
                                                                Display="None" ErrorMessage="Account # Required" SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender 
                                                                ID="RequiredFieldValidator20_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator20">
                                                            </asp:ValidatorCalloutExtender>--%>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row btnlinks">
                                                            <asp:Button ID="btnSageID" runat="server" Width="55px" Text="Check" OnClick="btnSageID_Click" CausesValidation="false" />
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Status</label>
                                                            <asp:DropDownList ID="ddlLocStatus" runat="server" CssClass="browser-default">
                                                                <asp:ListItem Value="0">Active</asp:ListItem>
                                                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Type</label>

                                                            <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" CssClass="browser-default">
                                                            </asp:DropDownList>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator822" runat="server" ValidationGroup="ValidateSave" ControlToValidate="ddlType" Display="None" ErrorMessage="Location Type Required" SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator822">
                                                            </asp:ValidatorCalloutExtender>

                                                        </div>

                                                    </div>


                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">

                                                            <div class="input-field col s10">
                                                                <label runat="server" associatedcontrolid="txtContractStatus">Contract status</label>
                                                                <asp:TextBox ID="txtContractStatus" runat="server" MaxLength="50" Enabled="false" style="margin-left: -10px!important;"></asp:TextBox>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <img id="imgContractStatus" visible="false" runat="server" title="Contract status" src="images/maintenance.png" style='cursor: pointer; margin-top: 10px; margin-left: -6px; width: 25px;'>
                                                                <telerik:RadToolTip RenderMode="Auto" ID="ContractStatusTooltip" runat="server" TargetControlID="imgContractStatus" Width="500px"
                                                                    RelativeTo="Element" Position="BottomRight">
                                                                </telerik:RadToolTip>
                                                            </div>

                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label" id="lblDefaultWorker" runat="server"></label>
                                                            <asp:DropDownList ID="ddlRoute" AutoPostBack="true" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged" runat="server" CssClass="browser-default">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" ValidationGroup="ValidateSave" runat="server"
                                                                ControlToValidate="ddlRoute" Display="None"
                                                                ErrorMessage="Default Worker Required" SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator17_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                TargetControlID="RequiredFieldValidator17">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:UpdatePanel ID="UpnSalespersonList" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <label class="drpdwn-label">Default Salesperson</label>
                                                                    <asp:DropDownList ID="ddlTerr" runat="server" CssClass="browser-default">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" ValidationGroup="ValidateSave"
                                                                        runat="server" ControlToValidate="ddlTerr" Display="None"
                                                                        ErrorMessage="Default Salesperson Required" SetFocusOnError="True">
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator18_ValidatorCalloutExtender"
                                                                        runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                        TargetControlID="RequiredFieldValidator18">
                                                                    </asp:ValidatorCalloutExtender>
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
                                                            <asp:UpdatePanel ID="UpnSalespersonList2" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <label class="drpdwn-label">Salesperson 2</label>
                                                                    <asp:DropDownList ID="ddlTerr2" runat="server" CssClass="browser-default">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>


                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">

                                                    <div class="input-field col s12">

                                                        <div class="row">

                                                            <asp:Label runat="server" ID="lblAddress" AssociatedControlID="txtAddress">Address</asp:Label>

                                                            <asp:TextBox ID="txtAddress" CssClass="materialize-textarea" AutoCompleteType="Disabled" runat="server" placeholder="" autocomplete="*" ONKEYUP="return checkMaxLength(this, event, 35)" MaxLength="255" TextMode="MultiLine"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ValidationGroup="ValidateSave" runat="server" ControlToValidate="txtAddress"
                                                                Display="None" ErrorMessage="Address Required" SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator11_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator11">
                                                            </asp:ValidatorCalloutExtender>

                                                            <span id="spnAddress" class="worning-add">Max 2 lines 30 characters each</span>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:Label runat="server" AssociatedControlID="txtCity">City</asp:Label>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="ValidateSave" ControlToValidate="txtCity"
                                                                Display="None" ErrorMessage="City Required" SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator6_ValidatorCalloutExtender" PopupPosition="BottomLeft" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator6">
                                                            </asp:ValidatorCalloutExtender>

                                                            <asp:TextBox ID="txtCity" runat="server" MaxLength="50" AutoCompleteType="Disabled" name="locality"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">

                                                            <asp:Label runat="server" AssociatedControlID="txtState">State/Province</asp:Label>
                                                            <asp:TextBox ID="txtState" runat="server" MaxLength="50" AutoCompleteType="Disabled"></asp:TextBox>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="ValidateSave" runat="server"
                                                                ControlToValidate="txtState" Display="None" ErrorMessage="State Required" SetFocusOnError="True">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator7_ValidatorCalloutExtender" PopupPosition="BottomLeft" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator7">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:Label runat="server" AssociatedControlID="txtZip">Zip/Postal Code</asp:Label>
                                                            <asp:TextBox ID="txtZip" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:Label runat="server" AssociatedControlID="txtCountry">Country</asp:Label>
                                                            <asp:TextBox ID="txtCountry" runat="server" MaxLength="50" AutoCompleteType="Disabled"></asp:TextBox>

                                                        </div>
                                                    </div>

                                                    <div id="dvZone" runat="server">

                                                        <div class="input-field col s5" runat="server">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Zone</label>
                                                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="browser-default">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>

                                                    </div>

                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:Label runat="server" ID="lbBusinessType" AssociatedControlID="ddlBusinessType" CssClass="drpdwn-label">Business Type</asp:Label>

                                                            <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="browser-default">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">

                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:Label runat="server" AssociatedControlID="lat">Latitude <span class="reqd">*</span></asp:Label>
                                                            <input id="lat" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        &nbsp;
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:Label runat="server" AssociatedControlID="lng">Longitude <span class="reqd">*</span></asp:Label>
                                                            <input id="lng" runat="server" />
                                                            <input id="locality" disabled="disabled" style="display: none;">
                                                            <input id="country" disabled="disabled" style="display: none;">
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <div id="map" class="map-c">
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>
                                        <div class="form-section-row">

                                            <div class="section-ttle">Main Contact Details</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtMaincontact">Main Contact</asp:Label>
                                                        <asp:TextBox ID="txtMaincontact" runat="server" MaxLength="50" AutoCompleteType="Disabled"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtPhoneCust">Phone</asp:Label>
                                                        <asp:TextBox ID="txtPhoneCust" runat="server" MaxLength="28" AutoCompleteType="Disabled"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtCell">Cellular</asp:Label>
                                                        <asp:TextBox ID="txtCell" runat="server" MaxLength="28" AutoCompleteType="Disabled"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtFax">Fax</asp:Label>
                                                        <asp:TextBox ID="txtFax" runat="server" MaxLength="28" AutoCompleteType="Disabled"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                                            ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email"
                                                            SetFocusOnError="True"
                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="ValidateSave">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4"
                                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator6" PopupPosition="BottomLeft">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" AutoCompleteType="Disabled"
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
                                                            ValidationExpression="[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)" ValidationGroup="ValidateSave">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5"
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

                                            <div class="section-ttle">Billing Address</div>
                                            <div class="form-section3">

                                                <div style="margin-bottom: 50px;">
                                                    <div class="input-field col s6">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkAddress" class="filled-in" runat="server" onclick="javascript:ChkAddress();" />
                                                            <asp:Label runat="server" AssociatedControlID="chkAddress">Same as Location Address</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s6">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkCustomerAddress" class="filled-in" runat="server" onclick="javascript:ChkCustomerAddress();" />
                                                            <asp:Label runat="server" AssociatedControlID="chkCustomerAddress">Same as Customer Address</asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div style="margin-bottom: 50px;">
                                                    <div class="input-field col s4">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkCreditHold" class="filled-in" runat="server" />
                                                            <asp:Label runat="server" AssociatedControlID="chkCreditHold">Credit Hold</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s4">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkCreditFlag" class="filled-in" runat="server" />
                                                            <asp:Label runat="server" AssociatedControlID="chkCreditFlag">Credit Flag</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s4">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkDispAlert" runat="server" class="filled-in" />
                                                            <asp:Label runat="server" AssociatedControlID="chkDispAlert">Dispatch Alert</asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12" style="margin-top: 20px;">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtCreditReason">Reason</asp:Label>
                                                        <asp:TextBox ID="txtCreditReason" CssClass="materialize-textarea" runat="server" TextMode="MultiLine" AutoCompleteType="Disabled"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">&nbsp;</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtBillAdd">Address</asp:Label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBillAdd"
                                                            Display="None" ErrorMessage="Address Required" ValidationGroup="ValidateSave" SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender
                                                            ID="RequiredFieldValidator2_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                            PopupPosition="Left" TargetControlID="RequiredFieldValidator2">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtBillAdd" runat="server" AutoCompleteType="Disabled"
                                                            ONKEYUP="return checkMaxLength(this, event, 35)"
                                                            CssClass="materialize-textarea" MaxLength="255" TextMode="MultiLine">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtBillCity">City</asp:Label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtBillCity"
                                                            Display="None" ErrorMessage="City Required" SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender
                                                            ID="RequiredFieldValidator3_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                            PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator3">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtBillCity" runat="server" MaxLength="50" AutoCompleteType="Disabled"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">

                                                        <asp:Label runat="server" AssociatedControlID="txtBillState">State/Province</asp:Label>
                                                        <asp:TextBox ID="txtBillState" runat="server" MaxLength="10" AutoCompleteType="Disabled"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                            ControlToValidate="txtBillState" Display="None" ErrorMessage="State Required"
                                                            SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender
                                                            ID="RequiredFieldValidator4_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                            PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator4">
                                                        </asp:ValidatorCalloutExtender>
                                                    </div>
                                                </div>

                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtBillZip">Zip/Postal Code</asp:Label>
                                                        <asp:TextBox ID="txtBillZip" runat="server" MaxLength="10" AutoCompleteType="Disabled"></asp:TextBox>
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
                                                        <asp:DropDownList ID="drpBillCountry" runat="server" ToolTip="Country" CssClass="browser-default">
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

                                                <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                    <div class="row">
                                                        <asp:DropDownList ID="ddlCompany" CssClass="browser-default" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:Label runat="server" AssociatedControlID="txtCompany" class="">Company</asp:Label>
                                                        <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>


                                            </div>
                                            <div class="form-section3-blank">&nbsp;</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtRemarks">Remarks</asp:Label>
                                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="8000" AutoCompleteType="Disabled" CssClass="materialize-textarea"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">&nbsp;</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12" style="margin-top: 25px">
                                                    <label class="drpdwn-label">Consultant</label>
                                                    <asp:DropDownList ID="ddlConsultant" runat="server" CssClass="browser-default">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>


                                        <div class="form-section-row">

                                            <div class="section-ttle">Billing Details</div>

                                            <div class="form-section3">
                                                <div class="input-field col s6 margin-c">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkPrintOnly" runat="server" CssClass="filled-in" />
                                                        <asp:Label runat="server" AssociatedControlID="chkPrintOnly">Print Invoice/Statements</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s6 margin-c">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkEmail" runat="server" CssClass="filled-in" AutoPostBack="true" OnCheckedChanged="chkEMail_CheckedChanged" />
                                                        <asp:Label runat="server" AssociatedControlID="chkEmail">Email Invoice/Statements</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s6 margin-c">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkNoCustStatement" runat="server" CssClass="filled-in" />
                                                        <asp:Label runat="server" AssociatedControlID="chkEmail">No Customer Statement</asp:Label>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtEmailTo">Service Email To:</asp:Label>

                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                            ControlToValidate="txtEmailTo" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                            ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                            SetFocusOnError="True">
                                                        </asp:RegularExpressionValidator>

                                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtEmailTo" runat="server" MaxLength="500" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtEmailTo_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                            TargetControlID="txtEmailTo">
                                                        </asp:FilteredTextBoxExtender>

                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtEmailCC" class="active">Service Email CC:</asp:Label>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                            ControlToValidate="txtEmailCC" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                            ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                            SetFocusOnError="True">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator3_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator3">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtEmailCC" runat="server" MaxLength="500" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtEmailCC_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                            TargetControlID="txtEmailCC">
                                                        </asp:FilteredTextBoxExtender>

                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtEmailToInv">Invoice/Statements Email To:</asp:Label>

                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                                            ControlToValidate="txtEmailToInv" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                            ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                            SetFocusOnError="True">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator4_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator4">
                                                        </asp:ValidatorCalloutExtender>

                                                        <asp:TextBox ID="txtEmailToInv" runat="server" AutoCompleteType="Disabled" MaxLength="500"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtEmailToInv_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                            TargetControlID="txtEmailToInv">
                                                        </asp:FilteredTextBoxExtender>

                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtEmailCCInv">Invoice/Statements Email CC:</asp:Label>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                                            ControlToValidate="txtEmailCCInv" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                            ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                            SetFocusOnError="True">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator5_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator5">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtEmailCCInv" runat="server" AutoCompleteType="Disabled" MaxLength="500"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtEmailCCInv_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                            TargetControlID="txtEmailCCInv">
                                                        </asp:FilteredTextBoxExtender>

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
                                                            <asp:Label ID="lblContractBill" runat="server" Text="Location Recurring Billing"></asp:Label></label>
                                                        <asp:DropDownList ID="ddlContractBill" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlContractBill_SelectedIndexChanged" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">
                                                            <asp:Label ID="Label6" runat="server" Text="Terms"></asp:Label></label>
                                                        <asp:DropDownList ID="ddlTerms" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
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
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Sales Tax</label>
                                                        <asp:DropDownList ID="ddlSTax" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;    
                                                    </div>
                                                </div>
                                                <div class="input-field col s5" id="dvSalesTax2" runat="server">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Sales Tax2</label>
                                                        <asp:DropDownList ID="ddlSalesTax2" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12" id="dvUseTax" runat="server">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Use Tax</label>
                                                        <asp:DropDownList ID="ddlUseTax" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12" style="margin-top: 4px; margin-bottom: 4px;">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="CopyToLocAndJob" runat="server" onclick="checked1();"></asp:CheckBox>
                                                        <asp:Label runat="server" AssociatedControlID="CopyToLocAndJob">Copy To Project</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtBillRate">
                                                            Billing Rate
                                                        </asp:Label>
                                                        <asp:TextBox ID="txtBillRate" runat="server" MaxLength="15" AutoCompleteType="Disabled" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;    
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtOt">OT Rate</asp:Label>
                                                        <asp:TextBox ID="txtOt" runat="server" MaxLength="15" AutoCompleteType="Disabled" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtNt">1.7 Rate</asp:Label>
                                                        <asp:TextBox ID="txtNt" runat="server" MaxLength="15" AutoCompleteType="Disabled" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;    
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtDt">DT Rate</asp:Label>
                                                        <asp:TextBox ID="txtDt" runat="server" MaxLength="15" AutoCompleteType="Disabled" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtTravel">Travel Rate</asp:Label>
                                                        <asp:TextBox ID="txtTravel" runat="server" MaxLength="15" AutoCompleteType="Disabled" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;    
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:Label runat="server" AssociatedControlID="txtMileage">Mileage</asp:Label>
                                                        <asp:TextBox ID="txtMileage" runat="server" MaxLength="15" AutoCompleteType="Disabled" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="cf"></div>
                                    </div>

                                </div>
                                <div>
                                    <asp:HiddenField ID="hdnCustomerAddress" runat="server" />
                                    <asp:HiddenField ID="hdnCustomerCity" runat="server" />
                                    <asp:HiddenField ID="hdnCustomerState" runat="server" />
                                    <asp:HiddenField ID="hdnCustomerZipCode" runat="server" />
                                    <asp:HiddenField ID="hdnCustomerCountry" runat="server" />
                                </div>
                            </div>
                        </li>
                        <li runat="server" id="adContacts">
                            <div id="accrdcontacts" class="collapsible-header accrd accordian-text-custom">
                                <i class="mdi-action-account-circle"></i>
                                <asp:Label ID="Label2" runat="server">Contacts</asp:Label>
                            </div>
                            <div class="collapsible-body">
                                <asp:Panel ID="pnlgvConPermission" runat="server">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="btncontainer">
                                                <div id="pnlContractGridButtons" runat="server">
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkAddnew" runat="server" CausesValidation="False" OnClientClick="return AddContactClick(this);" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton OnClientClick="return EditContactClick(this);"
                                                            ID="btnEdit" runat="server" OnClick="btnEdit_Click" CausesValidation="False">Edit</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton OnClientClick="return DeleteContactClick(this);"
                                                            ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click">Delete</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:HyperLink ID="lnkMail" Style="cursor: pointer;" runat="server">Email</asp:HyperLink>
                                                    </div>
                                                </div>

                                                <div class="btnlinks">
                                                    <asp:LinkButton
                                                        ID="lnkShowLog" runat="server" CausesValidation="False" OnClick="lnkShowLog_Click">Contact log</asp:LinkButton>

                                                </div>
                                            </div>
                                            <div class="grid_container">
                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock_Contacts" runat="server">
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

                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Contacts" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Location" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Contacts" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                            OnNeedDataSource="RadGrid_Contacts_NeedDataSource" OnPreRender="RadGrid_Contacts_PreRender" PagerStyle-AlwaysVisible="true"
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

                                                                    <telerik:GridTemplateColumn UniqueName="lblIndexID" Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIndex" runat="server" Text='<%# Container.ItemIndex %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("ContactID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn UniqueName="lblIndexID" Display="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex %>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Name" HeaderText="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Title" HeaderText="Title" HeaderStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Title" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Phone" HeaderText="Phone" HeaderStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Phone" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPhone" runat="server" Text='<%# Eval("Phone") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Fax" HeaderText="Fax" HeaderStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Fax" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFax" runat="server" Text='<%# Eval("Fax") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Cell" HeaderText="Cell" HeaderStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Cell" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCell" runat="server" Text='<%# Eval("Cell") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="140" DataField="Email" HeaderText="Email" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Email" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="80" HeaderText="Tickets" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" SortExpression="Tickets">
                                                                        <ItemTemplate>
                                                                            <div style="text-align: center">
                                                                                <asp:CheckBox ID="chkTicket" runat="server" Enabled="false" Checked='<%# ((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("EmailTicket")== true?Convert.ToBoolean((Eval("EmailTicket")==DBNull.Value ? false:Eval("EmailTicket"))):false%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Invoice/Statements" ShowFilterIcon="false" HeaderStyle-Width="150" SortExpression="Invoice/Statements">
                                                                        <ItemTemplate>
                                                                            <div style="text-align: center">
                                                                                <asp:CheckBox ID="chkInvoice" runat="server" Enabled="false" Checked='<%#  ((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("EmailRecInvoice")== true?Convert.ToBoolean((Eval("EmailRecInvoice")==DBNull.Value ? false:Eval("EmailRecInvoice"))):false%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Shutdown" ShowFilterIcon="false" HeaderStyle-Width="100" SortExpression="Shutdown">
                                                                        <ItemTemplate>
                                                                            <div style="text-align: center">
                                                                                <asp:CheckBox ID="chkShutdown" runat="server" Enabled="false" Checked='<%#   ((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("ShutdownAlert")== true?Convert.ToBoolean((Eval("ShutdownAlert")==DBNull.Value ? false:Eval("ShutdownAlert"))):false%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Tests" ShowFilterIcon="false" HeaderStyle-Width="80" SortExpression="Tests">
                                                                        <ItemTemplate>
                                                                            <div style="text-align: center">
                                                                                <asp:CheckBox ID="chkTests" runat="server" Enabled="false" Checked='<%#   ((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("EmailRecTestProp")== true?Convert.ToBoolean((Eval("EmailRecTestProp")==DBNull.Value ? false:Eval("EmailRecTestProp"))):false%>' />
                                                                            </div>
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
                        <li runat="server" id="adDocuments">
                            <div id="accrddocuments" class="collapsible-header accrd accordian-text-custom">
                                <i class="mdi-action-credit-card"></i>
                                <asp:Label ID="Label3" runat="server">Documents</asp:Label>
                            </div>
                            <div class="collapsible-body">
                                <asp:Panel ID="pnlDocPermission" runat="server">
                                    <asp:Panel ID="pnlDoc" runat="server" Visible="false">
                                        <asp:Panel ID="pnlDocumentButtons" runat="server">
                                            <div class="form-content-wrap">
                                                <div class="form-content-pd">
                                                    <div class="form-section-row">
                                                        <div class="col s12 m12 l12">
                                                            <div class="row">
                                                                <asp:FileUpload ID="FileUpload1" runat="server" class="dropify" AllowMultiple="true" onchange="AddDocumentClick(this);" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkUploadDoc" runat="server" CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                            Style="display: none">Upload</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkPostback" runat="server"
                                                            CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                                    </div>

                                                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">

                                                        <script type="text/javascript">   
                                                            var currentLoadingPanel = null;
                                                            var currentUpdatedControl = null;

                                                            function RequestStartDocuments(sender, args) {
                                                                if (document.activeElement.id == "<%= chkShowAllDocs.ClientID %>" ||
                                                                    document.activeElement.id == "<%= lnkDeleteDoc.ClientID %>") {
                                                                    currentLoadingPanel = $find("<%= RadAjaxLoadingPanel_Location.ClientID%>");

                                                                    currentUpdatedControl = "<%= RadGrid_Documents.ClientID %>";
                                                                    //show the loading panel over the updated control   
                                                                    currentLoadingPanel.show(currentUpdatedControl);
                                                                }
                                                            }
                                                            function ResponseEndDocuments() {
                                                                //hide the loading panel and clean up the global variables   
                                                                if (currentLoadingPanel != null)
                                                                    currentLoadingPanel.hide(currentUpdatedControl);
                                                                currentUpdatedControl = null;
                                                                currentLoadingPanel = null;
                                                            }
                                                        </script>

                                                    </telerik:RadCodeBlock>
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" PostBackControls="lblName" runat="server" ClientEvents-OnRequestStart="RequestStartDocuments" ClientEvents-OnResponseEnd="ResponseEndDocuments">
                                                        <div class="btnlinks" style="margin-left: -10px;">
                                                            <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click"
                                                                OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                                        </div>
                                                        <span class="tro trost">
                                                            <asp:CheckBox ID="chkShowAllDocs" Text="Show All Documents" OnCheckedChanged="chkShowAllDocs_CheckedChanged" class="css-checkbox" Style="padding-left: 5px; color: black; font-weight: 400" ForeColor="Black" AutoPostBack="true" runat="server" />
                                                        </span>
                                                        <div class="grid_container" style="margin-top: 10px;">
                                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" MasterTableView-RowIndicatorColumn-AutoPostBackOnFilter="true" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                    PagerStyle-AlwaysVisible="true"
                                                                    OnNeedDataSource="RadGrid_Documents_NeedDataSource"
                                                                    ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                    <CommandItemStyle />
                                                                    <GroupingSettings CaseSensitive="false" />
                                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" />
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

                                                                            <%--<telerik:GridTemplateColumn HeaderText="File Name" SortExpression="filename" ShowFilterIcon="false" HeaderStyle-Width="120">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lblName" OnClientClick="return ViewDocumentClick(this);" runat="server" CausesValidation="false" CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                        OnClick="lblName_Click" Text='<%# Eval("filename") %>'> </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>--%>
                                                                            <telerik:GridTemplateColumn DataField="filename" AutoPostBackOnFilter="true" SortExpression="filename" HeaderStyle-Width="250"
                                                                                CurrentFilterFunction="Contains" HeaderText="File Name" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                                        CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                        OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                                                    </asp:LinkButton>

                                                                                </ItemTemplate>

                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="File Type" DataField="doctype" AutoPostBackOnFilter="true" SortExpression="doctype" ShowFilterIcon="false" HeaderStyle-Width="120">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("doctype") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Project #" DataField="project" AutoPostBackOnFilter="true" SortExpression="project" ShowFilterIcon="false" HeaderStyle-Width="120">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblProject" runat="server" Text='<%# Eval("project") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Project Name" DataField="ProjectName" AutoPostBackOnFilter="true" SortExpression="ProjectName" ShowFilterIcon="false" HeaderStyle-Width="140">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("ProjectName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Ticket #" DataField="Ticket" AutoPostBackOnFilter="true" SortExpression="Ticket" ShowFilterIcon="false" HeaderStyle-Width="120">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTicket" runat="server" Text='<%# Eval("Ticket") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Assigned to" DataField="AssignedTo" AutoPostBackOnFilter="true" SortExpression="AssignedTo" ShowFilterIcon="false" HeaderStyle-Width="140">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblAssignedTo" runat="server" Text='<%# Eval("AssignedTo") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Date of the ticket" DataField="Date" AutoPostBackOnFilter="true" SortExpression="Date" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridBoundColumn FilterDelay="5" DataField="Elev" HeaderText="Equipment #" HeaderStyle-Width="140"
                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Elev" DataType="System.String"
                                                                                ShowFilterIcon="false">
                                                                            </telerik:GridBoundColumn>

                                                                            <telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                                CurrentFilterFunction="Contains" HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                                </ItemTemplate>

                                                                            </telerik:GridTemplateColumn>


                                                                            <telerik:GridTemplateColumn HeaderText="File path" DataField="path" AutoPostBackOnFilter="true" SortExpression="path" ShowFilterIcon="false" HeaderStyle-Width="200">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPath" runat="server" Text='<%# Eval("path") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Screen" DataField="Screen" AutoPostBackOnFilter="true" SortExpression="Screen" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblScreen" runat="server" Text='<%# Eval("Screen") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <%--<telerik:GridTemplateColumn HeaderText="Portal" AllowFiltering="false" SortExpression="portal" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderText="Remarks" SortExpression="remarks" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtremarks" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>--%>
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                </telerik:RadGrid>
                                                            </div>
                                                        </div>
                                                    </telerik:RadAjaxPanel>
                                                    <div class="form-section-row">
                                                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </asp:Panel>
                                </asp:Panel>
                                <div style="clear: both;"></div>
                            </div>
                        </li>

                        <li runat="server" id="adViewEquipments">
                            <div id="accrdequipments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-maps-local-laundry-service"></i>Equipment</div>
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

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Equip" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Location"
                                                    ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Equip" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                        OnNeedDataSource="RadGrid_Equip_NeedDataSource" OnPreRender="RadGrid_Equip_PreRender" OnItemEvent="RadGrid_Equip_ItemEvent"
                                                        PagerStyle-AlwaysVisible="true"
                                                        ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%"
                                                        OnFilterCheckListItemsRequested="RadGrid_Equip_FilterCheckListItemsRequested"
                                                        FilterType="CheckList"
                                                        AllowCustomPaging="True">
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

                                                                <telerik:GridTemplateColumn AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                                                    SortExpression="status" HeaderText="Status" HeaderStyle-Width="140"
                                                                    FilterCheckListEnableLoadOnDemand="true" UniqueName="status" DataField="status"
                                                                    FilterControlAltText="Filter Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblTotalActive" runat="server" />
                                                                    </FooterTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="Price" HeaderText="Price" HeaderStyle-Width="140"
                                                                    FooterAggregateFormatString="{0:n}" Aggregate="Sum"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal"
                                                                    SortExpression="Price"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn AutoPostBackOnFilter="true" DataField="last" CurrentFilterFunction="Contains" SortExpression="last" HeaderText="Last Service" HeaderStyle-Width="140" ShowFilterIcon="false" DataType="System.String">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllast" runat="server"><%# Eval("last")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "last"))):""%></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn AutoPostBackOnFilter="true" DataField="Install" CurrentFilterFunction="Contains" SortExpression="Install" HeaderText="Installed" HeaderStyle-Width="140" ShowFilterIcon="false" DataType="System.String">
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
                                        <div class="cf"></div>
                                    </div>
                                </div>

                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="adLocationHistory">
                            <div id="accrdlocationHistory" class="collapsible-header accrd accordian-text-custom"><i class="mdi-social-location-city"></i>Location History</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="srchpaneinner">
                                            <div class="srchtitle srchtitlecustomwidth">
                                                Search
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:DropDownList ID="ddlSearch" runat="server" onchange="showFilterSearchHistory();"
                                                    CssClass="browser-default selectst" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                                    <asp:ListItem Value=" ">Select</asp:ListItem>
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
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search"></asp:TextBox>
                                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="browser-default selectst" Style="display: none">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblRecordCount0" runat="server" Style="font-style: italic;"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="srchpaneinner">
                                            <div class="srchtitle srchtitlecustomwidth">
                                                Status
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst">
                                                    <asp:ListItem Value="-1">All</asp:ListItem>
                                                    <asp:ListItem Value="1">Assigned</asp:ListItem>
                                                    <asp:ListItem Value="2">Enroute</asp:ListItem>
                                                    <asp:ListItem Value="3">Onsite</asp:ListItem>
                                                    <asp:ListItem Value="4">Completed</asp:ListItem>
                                                    <asp:ListItem Value="5">Hold</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:TextBox ID="txtfromDate" runat="server" CssClass="srchcstm datepicker_mom" AutoCompleteType="Disabled"
                                                    MaxLength="50">
                                                </asp:TextBox>

                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="srchcstm datepicker_mom" AutoCompleteType="Disabled"
                                                    MaxLength="50">
                                                </asp:TextBox>
                                            </div>
                                            <div class="srchinputwrap tabcontainer">
                                                <ul class="tabselect accrd-tabselect" id="testSHradiobutton">
                                                    <li>

                                                        <asp:LinkButton AutoPostBack="False" ID="lnkHistoryDec" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_dateHistory('dec','rdCalHistory');return false;"></asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <label id="lblDay" runat="server">
                                                            <input type="radio" id="rdDay" name="rdCalHistory" value="rdDay" onclick="SelectDateHistory('Day', 'hdnHistoryDate')" />

                                                            Day
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblWeek" runat="server">
                                                            <input type="radio" id="rdWeek" name="rdCalHistory" value="rdWeek" onclick="SelectDateHistory('Week', 'hdnHistoryDate')" />

                                                            Week
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblMonth" runat="server">
                                                            <input type="radio" id="rdMonth" name="rdCalHistory" value="rdMonth" onclick="SelectDateHistory('Month', 'hdnHistoryDate')" />

                                                            Month
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblQuarter" runat="server">
                                                            <input type="radio" id="rdQuarter" name="rdCalHistory" value="rdQuarter" onclick="SelectDateHistory('Quarter', 'hdnHistoryDate')" />

                                                            Quarter
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblYear" runat="server">
                                                            <input type="radio" id="rdYear" name="rdCalHistory" value="rdYear" onclick="SelectDateHistory('Year', 'hdnHistoryDate')" />

                                                            Year
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkHistoryInc" runat="server" OnClientClick="dec_dateHistory('inc','rdCalHistory');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                                                    </li>

                                                </ul>
                                            </div>


                                            <div class="btnlinksicon">
                                                <asp:LinkButton ID="btnSearch" runat="server" CausesValidation="false"
                                                    OnClick="btnSearch_Click">
                                                    <i class="mdi-action-search"></i>
                                                </asp:LinkButton>

                                            </div>
                                            <div class="btnlinksicon">
                                                <asp:LinkButton ID="lnkPrint" runat="server" OnClientClick="return checkDateRange();" OnClick="lnkPrint_Click"><i class="fa fa-print"></i></asp:LinkButton>
                                            </div>

                                            <div style="padding-top: 14px">
                                                <asp:LinkButton ID="lnkShowAllTicket" runat="server" OnClick="lnkShowAllTicket_Click" OnClientClick="ResetShowAllHistory();"
                                                    CausesValidation="False">Show All Tickets</asp:LinkButton>
                                            </div>
                                        </div>



                                        <asp:Panel runat="server" ID="pnlGridButtons">
                                            <div class="btncontainer">
                                                <div class="btnlinks">
                                                    <asp:HyperLink ID="lnkAddTicket" OnClick='return AddTicketClick(this)' runat="server" Target="_self">Add</asp:HyperLink>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkEditTicket" OnClientClick='return EditTicketClick(this)' runat="server" OnClick="lnkEditTicket_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkDeleteTicket" runat="server" CausesValidation="False" OnClick="lnkDeleteTicket_Click" OnClientClick="return CheckDeleteLocationHistory();">Delete</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkServiceExcel" runat="server" CausesValidation="False" OnClick="lnkServiceExcel_Click">Export to Excel</asp:LinkButton>
                                                </div>
                                            </div>
                                        </asp:Panel>


                                        <div class="grid_container" style="margin-top: 10px;">
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <telerik:RadCodeBlock ID="RadCodeBlock_OpenCalls" runat="server">
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

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_History" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Location" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_OpenCalls" AllowFilteringByColumn="true" ShowFooter="True" PageSize="25"
                                                        OnNeedDataSource="RadGrid_OpenCalls_NeedDataSource" OnPreRender="RadGrid_OpenCalls_PreRender"
                                                        OnFilterCheckListItemsRequested="RadGrid_OpenCalls_FilterCheckListItemsRequested" FilterType="CheckList"
                                                        PagerStyle-AlwaysVisible="true"
                                                        ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True" OnItemCreated="RadGrid_OpenCalls_ItemCreated">
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
                                                                <telerik:GridBoundColumn DataField="locname" HeaderText="Location Name" HeaderStyle-Width="100" UniqueName="locname"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="locname"
                                                                    ShowFilterIcon="false" Visible="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn AutoPostBackOnFilter="true" DataField="ID" CurrentFilterFunction="Contains"
                                                                    SortExpression="id" UniqueName="ID" HeaderText="Ticket #" HeaderStyle-Width="100" ShowFilterIcon="false" DataType="System.String">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTicketId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                                        <asp:Label ID="lblComp" Visible="true" Style="display: none"
                                                                            runat="server" Text='<%# Bind("Comp") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="WorkOrder" HeaderText="WO #" SortExpression="WorkOrder" UniqueName="WorkOrder"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="Unit" HeaderText="Equip" HeaderStyle-Width="100" UniqueName="unit"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Unit"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn AutoPostBackOnFilter="true" DataField="dwork" SortExpression="dwork" UniqueName="dwork" HeaderText="Assigned to" HeaderStyle-Width="100" ShowFilterIcon="false"
                                                                    CurrentFilterFunction="Contains" DataType="System.String">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAssdTo" runat="server" Text='<%# Bind("dwork") %>'></asp:Label>
                                                                        <asp:Label ID="lblRes" runat="server" CssClass="newClassTooltip"
                                                                            Text='<%# ShowHoverText(Eval("description"),Eval("fdescreason")) %>'></asp:Label>
                                                                        <asp:HoverMenuExtender ID="hmeRes" runat="server" OffsetY="0" OffsetX="95" PopupControlID="lblRes"
                                                                            TargetControlID="lblAssdTo" HoverDelay="250">
                                                                        </asp:HoverMenuExtender>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="cat" SortExpression="cat" UniqueName="cat" HeaderText="Category" HeaderStyle-Width="100" FilterCheckListEnableLoadOnDemand="true" CurrentFilterFunction="Contains"
                                                                    FilterControlAltText="Category" AutoPostBackOnFilter="true">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="assignname" HeaderText="Status" HeaderStyle-Width="100"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="assignname"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="edate" HeaderText="Schedule Date" HeaderStyle-Width="100" DataType="System.String"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="edate" AllowFiltering="false"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="cdate" HeaderText="Call Date" HeaderStyle-Width="100" DataType="System.String"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="cdate" AllowFiltering="false"
                                                                    ShowFilterIcon="false">
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="Tottime" HeaderText="Total Time" HeaderStyle-Width="100" UniqueName="Tottime"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Tottime"
                                                                    ShowFilterIcon="false" FooterAggregateFormatString="{0}" Aggregate="Sum">
                                                                </telerik:GridBoundColumn>



                                                                <telerik:GridTemplateColumn AutoPostBackOnFilter="true" DataField="DescRes" SortExpression="DescRes" UniqueName="DescRes" HeaderText="Completed Description" HeaderStyle-Width="300" ShowFilterIcon="false"
                                                                    CurrentFilterFunction="Contains" DataType="System.String">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtfDesc44" ReadOnly="true" runat="server" TextMode="MultiLine" Text='<%# Bind("DescRes") %>'></asp:TextBox>
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
                        </li>
                        <li runat="server" id="adTransactions">
                            <div id="accrdtransactions" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-credit-card"></i>Transactions</div>
                            <div class="collapsible-body" id="transactions">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="srchpaneinner">
                                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                                Invoices
                                            </div>
                                            <div class="srchinputwrap" style="margin-right: 24px;">
                                                <asp:DropDownList ID="ddlSearchInv" runat="server" onchange="showFilterSearch();"
                                                    OnSelectedIndexChanged="ddlSearchInv_SelectedIndexChanged" CssClass="browser-default selectsml selectst">
                                                    <asp:ListItem Value=" ">Select</asp:ListItem>
                                                    <asp:ListItem Value="i.ref">Invoice#</asp:ListItem>
                                                    <asp:ListItem Value="i.fdate">Invoice Date</asp:ListItem>
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
                                                    Style="display: none" CssClass="browser-default">
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
                                                <span>Location Balance:
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

                                                        <asp:LinkButton AutoPostBack="False" ID="LinkButton1" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','rdCal');return false;"></asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <label id="lblDayInv" runat="server">
                                                            <input type="radio" id="rdDayInv" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'hdnInvDate')" />

                                                            Day
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblWeekInv" runat="server">
                                                            <input type="radio" id="rdWeekInv" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'hdnInvDate')" />

                                                            Week
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblMonthInv" runat="server">
                                                            <input type="radio" id="rdMonthInv" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'hdnInvDate')" />

                                                            Month
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblQuarterInv" runat="server">
                                                            <input type="radio" id="rdQuarterInv" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'hdnInvDate')" />

                                                            Quarter
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <label id="lblYearInv" runat="server">
                                                            <input type="radio" id="rdYearInv" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'hdnInvDate')" />

                                                            Year
                                                        </label>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="dec_date('inc','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                            <div class="srchinputwrap srchclr btnlinksicon" style="margin-left: -10px; margin-top: -2px;">
                                                <asp:LinkButton ID="lnkSearchInv" runat="server" CausesValidation="false" AutoPostback="false" ToolTip="Search" OnClick="lnkSearchInv_Click">
                                <i class="mdi-action-search"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="btncontainer">
                                            <div id="pnlTransGridButtons" runat="server">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAddInvoice" CausesValidation="False" runat="server" ToolTip="Add New"
                                                        OnClick="lnkAddInvoice_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkEditInvoice" runat="server" CausesValidation="False" ToolTip="Edit"
                                                        OnClick="lnkEditInvoice_Click" CssClass="DisableControls">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkCopyInvoice" ToolTip="Copy"
                                                        runat="server" Visible="False" CausesValidation="False"
                                                        OnClick="lnkCopyInvoice_Click">Copy</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkDeleteInvoice" ToolTip="Delete"
                                                        runat="server" CausesValidation="False"
                                                        OnClick="lnkDeleteInvoice_Click" OnClientClick="return CheckDeleteTrans();">Delete</asp:LinkButton>
                                                </div>
                                            </div>

                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkInvoiceExcel" CausesValidation="false" runat="server" OnClick="lnkInvoiceExcel_Click">Export to Excel</asp:LinkButton>

                                            </div>


                                            <div class="col lblsz2 lblszfloat">
                                                <div class="row">
                                                    <span class="tro trost accrd-trost">
                                                        <asp:LinkButton ID="lnkClear" runat="server" AutoPostback="false" OnClientClick="ResetValue();"
                                                            OnClick="lnkClear_Click" CausesValidation="false">Clear</asp:LinkButton>
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
                                                                    var grid = $find("<%= RadGrid_Invoice.ClientID %>");
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

                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Invoice" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Location" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Invoice" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" OnPreRender="RadGrid_Invoice_PreRender" PagerStyle-AlwaysVisible="true"
                                                            AllowCustomPaging="True"
                                                            OnNeedDataSource="RadGrid_Invoice_NeedDataSource"
                                                            OnItemCreated="RadGrid_Invoice_ItemCreated"
                                                            OnItemDataBound="RadGrid_Invoice_ItemDataBound"
                                                            OnExcelMLExportRowCreated="RadGrid_Invoice_ExcelMLExportRowCreated">
                                                            <CommandItemStyle />

                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                <Columns>

                                                                    <telerik:GridClientSelectColumn UniqueName="chkRadGridInvoice" HeaderStyle-Width="28">
                                                                    </telerik:GridClientSelectColumn>

                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>



                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="120px" DataField="ref" SortExpression="ref" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" DataType="System.String" HeaderText="Ref No." ShowFilterIcon="false">
                                                                        <ItemTemplate>

                                                                            <asp:HyperLink ID="lblInv" runat="server" Text='<%# Bind("ref") %>' NavigateUrl='<%# Bind("Link") %>'></asp:HyperLink>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="fDate" SortExpression="fdate" HeaderText="Date" HeaderStyle-Width="140" ShowFilterIcon="false" DataType="System.String">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInvDate" runat="server" Text='<%# String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridBoundColumn DataField="CustName" HeaderText="Customer Name" HeaderStyle-Width="160"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CustName"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="LocName" HeaderText="Location Name" HeaderStyle-Width="160"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="LocName"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridTemplateColumn DataField="fdesc" SortExpression="fdesc" AutoPostBackOnFilter="true" HeaderStyle-Width="160"
                                                                        CurrentFilterFunction="Contains" HeaderText="Description" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# (Eval("fdesc").ToString().Length > 50) ? (Eval("fdesc").ToString().Substring(0, 50) + "...") : Eval("fdesc")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotal" runat="server">Total</asp:Label>
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="amount" HeaderText="Bill Amount" SortExpression="amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Convert.ToDouble(Eval("Amount"))==0?"":String.Format("{0:C}", Convert.ToDouble(Eval("Amount")) ) %>'
                                                                                ForeColor='<%# Convert.ToDouble(Eval("amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>

                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Credits" SortExpression="Credits" AutoPostBackOnFilter="true" HeaderStyle-Width="140"
                                                                        CurrentFilterFunction="EqualTo" HeaderText="Credit" ShowFilterIcon="false" FooterAggregateFormatString="{0:c}" Aggregate="Sum">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmountCredits" runat="server" Text='<%# Convert.ToDouble(Eval("Credits"))==0?"":String.Format("{0:C}", Convert.ToDouble(Eval("Credits")) ) %>'
                                                                                ForeColor='Red'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Balance" SortExpression="Balance" AutoPostBackOnFilter="true" AllowFiltering="false" UniqueName="Balance"
                                                                        CurrentFilterFunction="EqualTo" HeaderText="Balance" ShowFilterIcon="false" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderStyle-Width="140">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmountBalance" runat="server" Text='<%# Convert.ToDouble(Eval("Balance"))==0?"0.00":String.Format("{0:C}", Convert.ToDouble(Eval("Balance")) ) %>'
                                                                                ForeColor='<%# Convert.ToDouble(Eval("Balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="RunTotal" HeaderText="Running Total" SortExpression="RunTotal" HeaderStyle-Width="160" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRunTotal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RunTotal", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("RunTotal"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridTemplateColumn DataField="status" SortExpression="status" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("status")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridTemplateColumn DataField="Type" HeaderText="Type" SortExpression="Type" HeaderStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type")%>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnLinkTo" runat="server" Value='<%#Eval("linkTo")%>' />
                                                                            <asp:HiddenField ID="hdnTransID" runat="server" Value='<%#Eval("TransID")%>' />
                                                                            <asp:HiddenField ID="hdnType" runat="server" Value='<%#Eval("Type")%>' />
                                                                            <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#Eval("status")%>' />
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
                                            <div id="pnlOppGridButtons" runat="server">
                                                <div class="btnlinks">
                                                    <asp:HyperLink ID="lnkAddopp" runat="server">Add</asp:HyperLink>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkEditOpp" runat="server" CausesValidation="False" OnClick="lnkEditOpp_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkCopyOpp" runat="server" CausesValidation="False" OnClick="lnkCopyOpp_Click">Copy</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkDeleteOpp" runat="server" OnClientClick="return CheckDeleteOpp();" CausesValidation="False" OnClick="lnkDeleteOpp_Click">Delete</asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkExcelOpp" runat="server" OnClick="lnkExcelOpp_Click">Export to Excel</asp:LinkButton>
                                            </div>

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
                        <li runat="server" id="adProjects">
                            <div id="accrdprojects" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Projects</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">

                                    <div class="form-content-pd">
                                        <div class="btncontainer" id="pnlProjectGridButtons" runat="server">
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

                                                                <telerik:GridTemplateColumn DataField="id" DataType="System.String" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="id" HeaderText="Project #" HeaderStyle-Width="140" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                                                                        <asp:HyperLink ID="lnkJob" runat="server" NavigateUrl='<%# "addproject.aspx?uid=" + Eval("id") %>'
                                                                            onclick='return EditJobClick(this)' Text='<%# Bind("ID") %>'>
                                                                        </asp:HyperLink>
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

                        <li runat="server" id="adAlerts">
                            <div id="accrdalerts" class="collapsible-header accrd accordian-text-custom"><i class="mdi-alert-warning"></i>Alerts</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:Repeater ID="rtAlerts" runat="server" OnItemDataBound="OnItemDataBound" OnItemCommand="rtAlerts_ItemCommand">
                                                    <HeaderTemplate>
                                                        <table class="table table-bordered table-striped table-condensed flip-content" style="width: 600px; margin-top: 20px;">
                                                            <tr>
                                                                <th scope="col">Code
                                                                </th>
                                                                <th scope="col">Subject
                                                                </th>
                                                                <th scope="col">Message
                                                                </th>
                                                                <th></th>
                                                            </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("alertid") %>'></asp:Label>
                                                                <asp:Label ID="lblAlertCode" runat="server" Visible="false" Text='<%# Eval("alertCode") %>'></asp:Label>
                                                                <asp:Label ID="lblAlertType" runat="server" Text='<%# Eval("alertname") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="lblDesc" runat="server" Text='<%# Eval("alertsubject") %>'></asp:TextBox>

                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="lblMessage" runat="server" Text='<%# Eval("alertmessage") %>'></asp:TextBox>

                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="lnkAddnewRow" runat="server" CssClass="addbutton1" CausesValidation="False" CommandName="AddNew"
                                                                    ImageUrl="images/add.png" Width="20px" ToolTip="Add Contact for Alert" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Repeater ID="rtAlertContacts" runat="server" OnItemCommand="rtAlertContacts_ItemCommand">
                                                                    <HeaderTemplate>
                                                                        <table style="width: 500px; margin-left: 180px;">
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                                                                                <asp:HiddenField ID="hdnsid" runat="server" Value='<%# Eval("screenid") %>' />
                                                                                <asp:HiddenField ID="hdnsname" runat="server" Value='<%# Eval("screenname") %>' />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtContact" ToolTip="search contacts/users"
                                                                                    onblur="AddNewCOntact(this);"
                                                                                    placeholder="search contacts/users"
                                                                                    Text='<%# Eval("name") %>' CssClass="contactsearch" runat="server"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkMail" runat="server" Text="Email" Checked='<%# Eval("email") %>' />
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkText" runat="server" Text="Text" Checked='<%# Eval("text") %>' />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td></td>
                                                                            <td></td>
                                                                            <td></td>
                                                                        </tr>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li runat="server" id="adGCInformation">
                            <div id="accrdgcInformation" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-speaker-notes"></i>GC Information</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <div class="form-section-row">

                                                    <div class="section-ttle">GC Details</div>
                                                    <div class="form-input-row">
                                                        <div class="form-section3">

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="GCtxtName">Name</asp:Label>
                                                                    <asp:TextBox ID="GCtxtName" runat="server" AutoCompleteType="Disabled"></asp:TextBox>

                                                                    <asp:HiddenField ID="hdnGContractorID" Value="0" runat="server" />
                                                                    <asp:HiddenField ID="hdnGCName" runat="server" />
                                                                    <asp:HiddenField ID="hdnGCNameupdate" Value="0" runat="server" />
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="GCtxtAddress">Address</asp:Label>
                                                                    <asp:TextBox ID="GCtxtAddress" TextMode="MultiLine" placeholder="" autocomplete="off" CssClass="materialize-textarea" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">


                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="GCtxtcity">City</asp:Label>
                                                                    <asp:TextBox ID="GCtxtcity" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">

                                                                    <asp:Label runat="server" AssociatedControlID="GCtxtState">State/Province</asp:Label>
                                                                    <asp:TextBox ID="GCtxtState" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="GCtxtPostalCode">Zip/Postal Code</asp:Label>
                                                                    <asp:TextBox ID="GCtxtPostalCode" MaxLength="5" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="GCtxtCountry">Country</asp:Label>
                                                                    <asp:TextBox ID="GCtxtCountry" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>


                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="GCtxtRemarks">Remarks</asp:Label>
                                                                    <asp:TextBox ID="GCtxtRemarks" AutoCompleteType="Disabled" TextMode="MultiLine" runat="server" CssClass="materialize-textarea"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="form-section-row">

                                                    <div class="section-ttle">GC Contact Info.</div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="GCtxtCName">Main Contact</asp:Label>
                                                                <asp:TextBox ID="GCtxtCName" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="GCtxtPhone">Phone</asp:Label>
                                                                <asp:TextBox ID="GCtxtPhone" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="GCtxtMobile">Cellular</asp:Label>
                                                                <asp:TextBox ID="GCtxtMobile" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="GCtxtFAX">Fax</asp:Label>
                                                                <asp:TextBox ID="GCtxtFAX" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>

                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="GCtxtEmailWeb">Email</asp:Label>
                                                                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                                    ControlToValidate="GCtxtEmailWeb" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                <asp:ValidatorCalloutExtender ID="vceEmail" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                    TargetControlID="revEmail">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="GCtxtEmailWeb" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <%-- <input id="email" type="email" class="validate">
                                                        <label for="email">Website</label>--%>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:PlaceHolder ID="PlaceHolderAttriGC" runat="server"></asp:PlaceHolder>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>

                        <li runat="server" id="adHomeowner">
                            <div id="accrdhomeowner" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-home"></i>Homeowner</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <asp:UpdatePanel ID="UpdatePanel28" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <div class="form-section-row">

                                                    <div class="section-ttle">Homeowner Details</div>
                                                    <div class="form-input-row">
                                                        <div class="form-section3">

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="hotxtname">Name</asp:Label>
                                                                    <asp:TextBox ID="hotxtname" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnHomeOwnerID" Value="0" runat="server" />
                                                                    <asp:HiddenField ID="hdnHOName" runat="server" />
                                                                    <asp:HiddenField ID="hdnHONameupdate" Value="0" runat="server" />


                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="HotxtAddress">Address</asp:Label>
                                                                    <asp:TextBox ID="HotxtAddress" AutoCompleteType="Disabled" TextMode="MultiLine" placeholder="" autocomplete="off" CssClass="materialize-textarea" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="hotxtcity">City</asp:Label>
                                                                    <asp:TextBox ID="hotxtcity" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">


                                                                    <asp:Label runat="server" AssociatedControlID="hottxtstate">State/Province</asp:Label>
                                                                    <asp:TextBox ID="hottxtstate" MaxLength="5" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="hotxtZIP">Zip/Postal Code</asp:Label>
                                                                    <asp:TextBox ID="hotxtZIP" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="hotxtCountry">Country</asp:Label>
                                                                    <asp:TextBox ID="hotxtCountry" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:Label runat="server" AssociatedControlID="hotxtRemarks">Remarks</asp:Label>
                                                                    <asp:TextBox ID="hotxtRemarks" AutoCompleteType="Disabled" TextMode="MultiLine" runat="server" CssClass="materialize-textarea"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="form-section-row">

                                                    <div class="section-ttle">Homeowner Contact Info.</div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="hotxtCName">Main Contact</asp:Label>
                                                                <asp:TextBox ID="hotxtCName" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="hotxtPhone">Phone</asp:Label>
                                                                <asp:TextBox ID="hotxtPhone" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="hotxtMobile">Cellular</asp:Label>
                                                                <asp:TextBox ID="hotxtMobile" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="HotxtFax">Fax</asp:Label>
                                                                <asp:TextBox ID="HotxtFax" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>

                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="HotxtEmailWeb">Email</asp:Label>
                                                                <asp:RegularExpressionValidator ID="ghg" runat="server"
                                                                    ControlToValidate="HotxtEmailWeb" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                <asp:ValidatorCalloutExtender ID="hoValidatorC" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                    TargetControlID="ghg">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="HotxtEmailWeb" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <%--   <input id="email" type="email" class="validate">
                                                        <label for="email">Website</label>--%>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="UpdatePanel29" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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


    <!-- ===================================================== START POPUP CODE ================================================-->
    <telerik:RadWindowManager ID="RadWindowManagerAddOpp" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowContact" Skin="Material" VisibleTitlebar="true" Title="Add Contact" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="450">
                <ContentTemplate>
                    <div>
                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                                            Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="cont">
                                        </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="left" TargetControlID="RequiredFieldValidator12">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtContcName" runat="server" CssClass="Contact-search" AutoCompleteType="Disabled" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtContcName">Contact Name</asp:Label>
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
                                        <asp:Label runat="server" AssociatedControlID="txtTitle">Title</asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContPhone" runat="server" AutoCompleteType="Disabled" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtContPhone">Phone</asp:Label>
                                    </div>
                                </div>

                            </div>

                        </div>
                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContFax" runat="server" AutoCompleteType="Disabled" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtContFax">Fax</asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContCell" runat="server" AutoCompleteType="Disabled" CssClass="form-control" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtContCell">Cell</asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContEmail" runat="server" CssClass="form-control" AutoCompleteType="Disabled" MaxLength="50"></asp:TextBox>
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
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>

                            <div class="row">
                                <div class="section-ttle">Email</div>
                            </div>

                        </div>
                        <div class="form-section-row">
                            <div class="form-section4">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <div class="checkrow">
                                            <asp:CheckBox ID="chkEmailTicket" runat="server" class="filled-in" />
                                            <label for="chkEmailTicket" style="top: -1px !important">Ticket</label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section4">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <div class="checkrow">
                                            <asp:CheckBox ID="chkEmailInvoice" runat="server" class="filled-in" />
                                            <label for="chkEmailInvoice" style="top: -1px !important">Invoice/Statement</label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section4">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <div class="checkrow">
                                            <asp:CheckBox ID="chkShutdownAlert" runat="server" class="filled-in" />
                                            <label for="chkShutdownAlert" style="top: -1px !important">Shutdown Alerts</label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section4">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <div class="checkrow">
                                            <asp:CheckBox ID="chkTestProposals" runat="server" class="filled-in" />
                                            <label for="chkTestProposals" style="top: -1px !important">Test Proposals</label>
                                        </div>
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


            <telerik:RadWindow ID="RadWindowAddCustomer" Skin="Material" VisibleTitlebar="true" Title="Add Customer" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="950" NavigateUrl="AddCustomer.aspx?o=1" Height="650">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowPrintTickets" Skin="Material" VisibleTitlebar="true" Title="Add Contact" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1060" Height="620">
                <ContentTemplate>
                    <div>
                        <iframe id="iframeCustomer" runat="server" style="width: 1060px; height: 620px;" frameborder="0"></iframe>
                        <div style="clear: both;"></div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowContactLog" Skin="Material" VisibleTitlebar="true" Title="Contact Log" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="450">
                <ContentTemplate>
                    <div class="grid_container">

                        <div class="RadGrid RadGrid_Material FormGrid">
                            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                                        if (element && element.tagName == "INPUT") {// && element.type != "checkbox") {
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




    <!-- ===================================================== END POPUP CODE ================================================-->
    <!-- Invoice -->
    <asp:HiddenField runat="server" ID="hdnAddInvoice" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditInvoice" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteInvoice" Value="Y" />
    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewJob" Value="Y" />
    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewEquipment" Value="Y" />
    <!-- Ticket -->
    <asp:HiddenField runat="server" ID="hdnAddeTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteTicket" Value="Y" />
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
    <asp:HiddenField runat="server" ID="hdnPageQueryString" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectedDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectedDtRangeHistory" Value="" />
    <asp:HiddenField runat="server" ID="ishowAllInvoice" Value="0" />
    <asp:HiddenField runat="server" ID="hdnOpenTicketsCount" Value="0" />
    <asp:HiddenField runat="server" ID="hdnApplyServiceTypeRule" Value="0" />
    <asp:HiddenField runat="server" ID="hdnServiceTypeName" Value="" />
    <asp:HiddenField runat="server" ID="hdnProjectPerDepartmentCount" Value="0" />
    <asp:HiddenField runat="server" ID="hdnProjectaregoingtoUpdate" Value="0" />

    <asp:HiddenField runat="server" ID="hdnOpenTicketIDs" Value="" />
    <asp:HiddenField runat="server" ID="hdnCon" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">

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


            //MASKING TEXTBOXES
            $('#<%=txtPhoneCust.ClientID%>').mask("(999) 999-9999? Ext 99999");
            $('#<%=txtPhoneCust.ClientID%>').bind('paste', function () { $(this).val(''); });
            $('#<%=txtCell.ClientID%>').mask("(999) 999-9999");
            $('#<%=txtFax.ClientID%>').mask("(999) 999-9999");

            $('#<%=GCtxtPhone.ClientID%>').mask("(999) 999-9999? Ext 99999");
            $('#<%=GCtxtPhone.ClientID%>').bind('paste', function () { $(this).val(''); });
            $('#<%=GCtxtMobile.ClientID%>').mask("(999) 999-9999");
            $('#<%=GCtxtFAX.ClientID%>').mask("(999) 999-9999");

            $('#<%=hotxtPhone.ClientID%>').mask("(999) 999-9999? Ext 99999");
            $('#<%=hotxtPhone.ClientID%>').bind('paste', function () { $(this).val(''); });
            $('#<%=hotxtMobile.ClientID%>').mask("(999) 999-9999");
            $('#<%=HotxtFax.ClientID%>').mask("(999) 999-9999");

            //END MASKING TEXTBOXES
            $('#<%=txtPhoneCust.ClientID%>').click(function () { $(this).select(); });
        });

    </script>
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

                    $("#<%=lblDayInv.ClientID%>").addClass("labelactive");
                    document.getElementById("rdDayInv").checked = true;

                }
                else if (val == 'Week') {

                    $("#<%=lblWeekInv.ClientID%>").addClass("labelactive");

                    document.getElementById("rdWeekInv").checked = true;

                }
                else if (val == 'Month') {
                    $("#<%=lblMonthInv.ClientID%>").addClass("labelactive");

                    document.getElementById("rdMonthInv").checked = true;

                }
                else if (val == 'Quarter') {

                    $("#<%=lblQuarterInv.ClientID%>").addClass("labelactive");
                    document.getElementById("rdQuarterInv").checked = true;


                }
                else if (val == 'Year') {

                    $("#<%=lblYearInv.ClientID%>").addClass("labelactive");
                    document.getElementById("rdYearInv").checked = true;

                }


            }
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

            $(".dropdown-content.select-dropdown li").on("click", function () {
                var that = this;
                setTimeout(function () {
                    if ($(that).parent().hasClass('active')) {
                        $(that).parent().removeClass('active');
                        $(that).parent().hide();
                    }
                }, 100);
            });

            var tabBack = GetQueryStringParams('f');
            if (tabBack == "r") {
                clickLink("#<%=liLocationHistory.ClientID%>");

                var $target = $("#accrdlocationHistory");
                if ($target.hasClass('active') || $target == "") {
                    $target.click();
                }
                else {
                    $target.click();
                }

                $('html, body').animate({ scrollTop: $target.offset().top }, 'slow');
            }
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

    <script type="text/javascript">

        $("#<%=ddlLocStatus.ClientID%>").change(function () {
            if ($('option:selected', $(this)).text() == 'Inactive') {
                //var isAllowInactive = $("#<%=hdnOpenTicketsCount.ClientID%>").val() == "0";
                var isAllowInactive = "0";
                var OpenTicketIDs = $("#<%=hdnOpenTicketIDs.ClientID%>").val();
                if (isAllowInactive) {
                    if (!confirm('Please note making the location inactive will set all projects , open tickets and equipment to inactive. Would you like to proceed?')) {
                        $("#<%=ddlLocStatus.ClientID%>").val("0");
                    }
                } else {
                    alert("The Location cannot be made inactive since there are open tickets for this location.");
                    $("#<%=ddlLocStatus.ClientID%>").val("0");
                }
            }
        });



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

                    $("#<%=lblDayInv.ClientID%>").addClass("labelactive");
                    document.getElementById("rdDayInv").checked = true;

                }
                else if (val == 'Week') {

                    $("#<%=lblWeekInv.ClientID%>").addClass("labelactive");

                    document.getElementById("rdWeekInv").checked = true;

                }
                else if (val == 'Month') {
                    $("#<%=lblMonthInv.ClientID%>").addClass("labelactive");

                    document.getElementById("rdMonthInv").checked = true;

                }
                else if (val == 'Quarter') {

                    $("#<%=lblQuarterInv.ClientID%>").addClass("labelactive");
                    document.getElementById("rdQuarterInv").checked = true;


                }
                else if (val == 'Year') {

                    $("#<%=lblYearInv.ClientID%>").addClass("labelactive");
                    document.getElementById("rdYearInv").checked = true;

                }

            }
        }
        function checkDateRange() {
            if ($("#<%=txtfromDate.ClientID%>").val() == "" || $("#<%=txtToDate.ClientID%>").val() == "") {
                noty({
                    text: 'Set your date range before selecting this report',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
            return true;
        }
        function ShowHistoryTransactionPopup(Uid, Type, owner, loc, status, TransID) {

            var oWnd = $find("<%=RadWindowHistoryTransaction.ClientID%>");
            oWnd.setUrl("HistoryTransaction.aspx?uid=" + Uid + "&type=" + Type + "&owner=" + owner + "&loc=" + loc + "&status=" + status + "&tid=" + TransID + "&page=addlocation");
            oWnd.setSize(800, 400);
            oWnd.show();
        }

        function setCustomPosition(sender, args) {

            var elmnt = document.getElementById("<%=RadGrid_Invoice.ClientID%>");

            sender.moveTo(sender.get_left(), elmnt.offsetTop);
        }
        function CheckDeleteLocationHistory() {

            var result = false;
            if ($("#<%=hdnDeleteTicket.ClientID%>").val() == 'N') {
                return false;
            }

            $("#<%=RadGrid_OpenCalls.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Do you really want to delete this item ?');
            }
            else {
                alert('Please select an item to delete.')
                return false;
            }
        }
        function CheckDeleteTrans() {
            var result = false;
            if ($("#<%=hdnDeleteInvoice.ClientID%>").val() == 'N') {
                return false;
            }



            var message = "Please select an item to delete.";

            $("#<%=RadGrid_Invoice.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var chkSelect = $(this).find("input[type='Checkbox']");
                var hdnType = $(this).find("input[id*='hdnType']");
                var hdnStatus = $(this).find("input[id*='hdnStatus']");
                if (chkSelect.is(":checked")) {
                    if ((hdnType.val() == "AR Invoice" && hdnStatus.val() == "Open") || (hdnType.val() == "Received Payment" && hdnStatus.val() == "Open")) {
                        result = true;
                    } else {
                        if (hdnType.val() == "AR Invoice") {
                            message = "Invoice is not open and can therefore not be deleted.";
                            // break();

                        }
                        if (hdnType.val() == "Received Payment") {
                            message = "Payment is not open and can therefore not be deleted.";
                            //   break();
                        }
                    }

                }
            });

            if (result == true) {
                return confirm('Do you really want to delete this item ?');
            }
            else {
                alert(message);
                return false;
            }
        }


        function validateBilling() {

            var chkPrintOnly = $('#<%=chkPrintOnly.ClientID%>').is(":checked");
            var chkEmail = $('#<%=chkEmail.ClientID%>').is(":checked");
            if (chkEmail == false && chkPrintOnly == false) {
                return 'Please select at least one option "Print Invoice/Statements" or "Email Invoice/Statements".'
            } else {
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
            }

            return '';

        }
    </script>



</asp:Content>
