<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddCompany" Codebehind="~/AddCompany.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <script type="text/javascript">
        function initialize() {
            var address = new google.maps.LatLng(document.getElementById('<%= txtLat.ClientID %>').value, document.getElementById('<%= txtLng.ClientID %>').value);
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
                    state: "#<%= txtState.ClientID %>",
                    zip: "#<%= txtZip.ClientID %>",
                    lat: "#<%= txtLat.ClientID %>",
                    lng: "#<%= txtLng.ClientID %>"
                }).bind("geocode:result", function (event, result) {                    
                    var getCountry = "";
                    for (var i = 0; i < result.address_components.length; i++) {
                        var addr = result.address_components[i];
                        var getCountry;
                        var countryNum = "0";
                        if (addr.types[0] == 'country')
                            getCountry = addr.short_name;

                    }
                    $("#<%=ddlCountry.ClientID%>").val(getCountry);
                        Materialize.updateTextFields();

                    });
                initialize();
                $('#<%=txtTele.ClientID%>').mask("(999) 999-9999");
                $('#<%=txtFax.ClientID%>').mask("(999) 999-9999");
                $('#<%=txtCell.ClientID%>').mask("(999) 999-9999");
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height: 65px !important;">
        <div class="divbutton-container">
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">

                    <header>
                        <div class="container row-color-grey">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">

                                        <div class="page-title"><i class="mdi-action-dns"></i>&nbsp;Add New Database</div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                                        </div>

                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </header>
                </div>
            </div>
        </div>
    </div>
    <div class="container card cardnegate rounded">
        <div class="row">
            <div class="form-content-wrap">
                <div class="form-content-pd">
                    <div class="form-section-row">
                        <div class="section-ttle">Company Info</div>
                        <div class="form-section3">

                            <div class="input-field col s12">
                                <div class="row">
                                    <%--<input id="fName" type="text" class="validate">--%>
                                    <asp:TextBox ID="txtCompany" runat="server" CssClass="validate" Width="100%"
                                        MaxLength="75" TabIndex="1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                        runat="server" ControlToValidate="txtCompany" Display="None"
                                        ErrorMessage="Company Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" PopupPosition="Right"
                                        TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                    <label for="fName">Company Name</label>
                                </div>
                            </div>
                            <div class="input-field col s12">
                                <div class="row">
                                    <%--<textarea id="address" class="materialize-textarea smalltxtarea"></textarea>--%>
                                    <textarea id="txtRemarks" runat="server" class="materialize-textarea"></textarea>
                                    <label for="address">Remarks</label>
                                </div>
                            </div>


                        </div>
                        <div class="form-section3-blank">
                            &nbsp;
                        </div>
                        <div class="form-section3">

                            <div class="input-field col s12">
                                <div class="row">
                                    <%--<textarea id="address" class="materialize-textarea"></textarea>--%>
                                    <textarea id="txtAddress" runat="server" class="materialize-textarea" maxlength="255"></textarea>
                                    <label for="address">Address</label>
                                </div>
                            </div>
                            <div class="input-field col s5">
                                <div class="row">
                                    <%--<input id="city" type="email" class="validate">--%>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="validate"
                                        MaxLength="50" TabIndex="3"></asp:TextBox>
                                    <label for="city">City</label>
                                </div>
                            </div>
                            <div class="input-field col s2">
                                <div class="row">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="input-field col s5">
                                <div class="row">
                                    <asp:TextBox ID="txtState" runat="server" CssClass="validate"
                                        MaxLength="2" TabIndex="4"></asp:TextBox>
                                    <%--<input id="state" type="email" class="validate">--%>
                                    <label for="state">State</label>
                                </div>
                            </div>
                            <div class="input-field col s5">
                                <div class="row">
                                    <%--<input id="zip" type="text" class="validate">--%>
                                    <asp:TextBox ID="txtZip" runat="server" CssClass="validate"
                                        MaxLength="10" TabIndex="5"></asp:TextBox>
                                    <label for="zip">Zip</label>
                                </div>
                            </div>


                            <div class="input-field col s2">
                                <div class="row">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="input-field col s5">
                                <div class="row">
                                    <%--<input id="country" type="text" class="validate">--%>
                                    <label for="country" class="drpdwn-label">Country</label>
                                    <%--<input type="text" id="country" />--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                        ControlToValidate="ddlCountry" ValidationGroup="general, rep" InitialValue="State" Display="None" ErrorMessage="Country Required"
                                        SetFocusOnError="True">
                                    </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                        ID="ValidatorCalloutExtender3"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
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

                        </div>
                        <div class="form-section3-blank">
                            &nbsp;
                        </div>
                        <div class="form-section3">
                            <div class="input-field col s5">
                                <div class="row">
                                    <%--<input id="lat" type="text" class="validate">--%>
                                    <asp:TextBox ID="txtLat" runat="server" CssClass="validate"
                                        MaxLength="10" TabIndex="7"></asp:TextBox>
                                    <label for="lat">Latitude</label>
                                </div>
                            </div>


                            <div class="input-field col s2">
                                <div class="row">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="input-field col s5">
                                <div class="row">
                                    <%--<input id="long" type="text" class="validate">--%>
                                    <asp:TextBox ID="txtLng" runat="server" CssClass="validate"
                                        MaxLength="10" TabIndex="8"></asp:TextBox>
                                    <label for="long">Longitude</label>
                                </div>
                            </div>
                            <div class="input-field col s12">
                                <div class="row">
                                    <%--<iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d13716.88953039753!2d76.77389278096229!3d30.740254306150458!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x390fed0afe5003d3%3A0x8f47abe9f2044934!2sSector+17%2C+Chandigarh!5e0!3m2!1sen!2sin!4v1506502302516" frameborder="0" style="border: 0; width: 100%" allowfullscreen></iframe>--%>
                                    <div id="map" style="overflow: hidden !important; height: 110px!important;"></div>
                                </div>
                            </div>


                        </div>
                    </div>
                    <div class="form-section-row">
                        <div class="section-ttle">Contact Info</div>
                        <div class="form-section3">
                            <div class="input-field col s12">
                                <div class="row">
                                    <%--<input id="conName" type="text" class="validate">--%>
                                    <asp:TextBox ID="txtContName" runat="server" CssClass="validate" Width="100%"
                                        MaxLength="50" TabIndex="6"></asp:TextBox>
                                    <label for="conName">Contact Name</label>
                                </div>
                            </div>
                            <div class="input-field col s12">
                                <div class="row">
                                    <%--<input id="Tel" type="text" class="validate">--%>
                                    <asp:TextBox ID="txtTele" runat="server" CssClass="validate" Width="100%" placeholder="(xxx)xxx-xxxx"
                                        MaxLength="20" TabIndex="6"></asp:TextBox>
                                    <label for="Tel">Phone</label>
                                </div>
                            </div>
                        </div>
                        <div class="form-section3-blank">
                            &nbsp;
                        </div>
                        <div class="form-section3">
                            <div class="input-field col s12">
                                <div class="row">
                                    <%--<input id="txtCell" runat="server" type="text" class="validate" placeholder="(xxx)xxx-xxxx">--%>
                                    <asp:TextBox ID="txtCell" runat="server" CssClass="validate" Width="100%" placeholder="(xxx)xxx-xxxx"
                                        MaxLength="20" TabIndex="7"></asp:TextBox>
                                    <label for="Cell">Cell</label>
                                </div>
                            </div>
                            <div class="input-field col s12">
                                <div class="row">
                                    <%--<input id="Fax" type="text" class="validate">--%>
                                    <asp:TextBox ID="txtFax" runat="server" CssClass="validate" placeholder="(xxx)xxx-xxxx"
                                        MaxLength="20" TabIndex="7"></asp:TextBox>
                                    <label for="Fax">Fax</label>
                                </div>
                            </div>
                        </div>
                        <div class="form-section3-blank">
                            &nbsp;
                        </div>
                        <div class="form-section3">
                            <div class="input-field col s12">
                                <div class="row">
                                    <%--<input id="emai" type="text" class="validate">--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                        runat="server" ControlToValidate="txtEmail" Display="None"
                                        ErrorMessage="Invalid Email" SetFocusOnError="True"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                    <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1" PopupPosition="BottomLeft">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="validate"
                                        MaxLength="50" TabIndex="8"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                        TargetControlID="txtEmail">
                                    </asp:FilteredTextBoxExtender>
                                    <label for="emai">Email</label>
                                </div>
                            </div>
                            <div class="input-field col s12">
                                <div class="row">
                                    <%--<input id="web" type="text" class="validate">--%>                                    
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtWebAdd"
                                        ErrorMessage="Invalid Website URL" ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?" runat="server" Display="None"/>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1"
                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2" PopupPosition="BottomLeft">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:TextBox ID="txtWebAdd" runat="server" CssClass="validate"
                                        MaxLength="50" TabIndex="9"></asp:TextBox>
                                    <label for="txtWebAdd">Web Address</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-section-row">
                        <div class="section-ttle">Database Info</div>
                        <div class="form-section3">

                            <div class="input-field col s12">
                                <div class="row">
                                    <label class="drpdwn-label">Database Type</label>
                                    <%--<select class="browser-default">
                                        <option>Select</option>
                                        <option>MSM</option>
                                    </select>--%>
                                    <asp:DropDownList ID="ddlDBType" runat="server" CssClass="browser-default" Width="100%"
                                        TabIndex="10">
                                        <asp:ListItem Value="MSM">MSM</asp:ListItem>
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
                                    <%--<input id="db" type="text" class="validate">--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="txtDB" Display="None" ErrorMessage="Database Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" PopupPosition="Right"
                                        TargetControlID="RequiredFieldValidator2">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:TextBox ID="txtDB" runat="server" CssClass="validate"
                                        TabIndex="11"></asp:TextBox>
                                    <label for="db">Database Name</label>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnRcvPymtSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
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
    </script>
</asp:Content>
