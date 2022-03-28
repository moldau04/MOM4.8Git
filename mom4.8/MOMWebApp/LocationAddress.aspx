<%@ Page Title="" Language="C#" MasterPageFile="~/MOMRadWindow.Master" AutoEventWireup="true" Inherits="LocationAddress" Codebehind="LocationAddress.aspx.cs" %>

 <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!--Grid Control-->

    <link href="Design/css/grid.css" rel="stylesheet" />

    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

     <%--SCRIPT 3--%>
    <script type="text/javascript">
      
         

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

        function CheckClose() {
            var IsEdited = $("#<%=hdnEdited.ClientID%>").val();

            if (IsEdited == "1") {
                window.parent.document.getElementById('ctl00_ContentPlaceHolder1_btnClearClick').click();
            }
            else if (IsEdited == "0") {
                window.parent.document.getElementById('btnCloseAdd').click();
            }
        }
    </script>

    <style>
        .dropdown-content {
            margin-top: 2px !important;
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


  

    <div class="topNav top-hei">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-maps-pin-drop"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Edit Address</asp:Label></div>
                                    <div class="btnlinks"> 
                         <asp:LinkButton ID="btnSubmitAddress" runat="server" Text="Update" OnClick="btnSubmitAddress_Click" />
                                        
                                    </div>
                                    <div class="btnclosewrap" id="divClose" runat="server">
                                                           <asp:HiddenField ID="hdnEdited" runat="server" Value="0" />
                                                                  <a href="#" id="lnkCancelContact" " onclick="CheckClose();"><i class="mdi-content-clear"></i></a>
         
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


        
        </div>
    </div>

    <div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <%--<ul class="collapsible popout collapsible-accordion form-accordion-head expandable" >--%>
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li>
                            <div id="accrdcustInfo" style="display:none;" class="collapsible-header accrd active accordian-text-custom active"><i class="mdi-communication-contacts"></i> </div>
                            <div class="collapsible-body" id="divcustInfo">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            


                                            <div class="form-section3">

 
                                               
                                                <div class="srchclr btnlinksicon rowbtn ">
                                                    &nbsp;
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">


                                                        <asp:TextBox ID="txtLocationName" ReadOnly="true" type="text" Text="Location Name" runat="server" AutoCompleteType="Disabled" autocomplete="new-customer"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtLocationName">Location Name</asp:Label>
                                                       
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <div>
                                                            <label id="lblgoogleloc" for="txtGoogleAutoc" class="drpdwn-label">Customer Address</label>
                                                        </div>
                                                        <asp:TextBox AutoCompleteType="Disabled" TextMode="MultiLine" Rows="4" CssClass="materialize-textarea" ID="txtGoogleAutoc" runat="server" placeholder=""></asp:TextBox>

                                                      
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
                                              
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                             

                                                
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


                                                        <div id="map" style="overflow: hidden !important; height: 200px!important;">
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
                                   
                                        <div class="cf"></div>
                                    </div>

                                </div>
                            </div>
                        </li>                  
                    </ul>
                </div>
            </div>
        </div>
    </div>

     
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="server">
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script> 
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script> 

</asp:Content>
