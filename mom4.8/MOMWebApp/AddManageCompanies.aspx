<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddManageCompanies" Codebehind="AddManageCompanies.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <script type="text/javascript">
        function ShowPopup() {
            $(function () {
                $("#popup2").dialog({
                    height: 450,
                    width: 800
                });
            });
        };
        function test() {
            var value = document.getElementById('<%=txtCompanyID.ClientID%>').value;
            console.log("here", value);
        }
        function ConfirmUpload() {
            document.getElementById('<%=btnUpload.ClientID%>').click();
        }
        function isDecimalKey(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 45) {
                return false;
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
        function ConvertDigit(obj) {

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                //document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        function AlertSageIDUpdate() {

            var ret = true;

            if ('<%=ViewState["mode"].ToString()  %>' == "1") {
                if (hdnAcctID.value != txtAcctno.value) {
                    ret = confirm('Sage ID edited, this will create a new Customer in Sage and make existing inactive. Do you want to continue?');
                }
            }
            return ret;
        }

        function initialize() {
            var address = new google.maps.LatLng(document.getElementById('<%= txtLatitude.ClientID %>').value, document.getElementById('<%= txtLongitude.ClientID %>').value);
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
                    lat: "#<%= txtLatitude.ClientID %>",
                    lng: "#<%= txtLongitude.ClientID %>"
                }).bind("geocode:result", function (event, result) {
                    var getCountry = "";
                    var getCountry = "";
                    for (var i = 0; i < result.address_components.length; i++) {
                        var addr = result.address_components[i];
                        var getCountry;
                        if (addr.types[0] == 'country')
                            getCountry = addr.short_name;
                    }

                    $("#<%=ddlCountry.ClientID%>").val(getCountry);

                        Materialize.updateTextFields();
                    });

                initialize();
            });

            $('#<%=txtPhone.ClientID%>').mask("(999) 999-9999");
            $('#<%=txtFax.ClientID%>').mask("(999) 999-9999");
        });
    </script>
    <style>
        a[disabled=disabled] {
            color: gray;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-communication-business"></i>
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Company</asp:Label>
                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" ToolTip="Save" OnClick="btnSubmit_Click"
                                            ValidationGroup="general, rep">Save</asp:LinkButton>
                                    </div>
                                    <div style="font-size: 15px; font-weight: 800;" id="tdCompanyOffice" runat="server">
                                        <asp:RadioButton ID="rbCompany" runat="server" Text="Company" GroupName="compOffice" AutoPostBack="true" Checked="true"
                                            OnCheckedChanged="rbCompany_CheckedChanged" />
                                        &nbsp;<asp:RadioButton ID="rbOffice" runat="server" Text="Office" GroupName="compOffice" AutoPostBack="true"
                                            OnCheckedChanged="rbOffice_CheckedChanged" />
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label CssClass="title_text_CompanyId" ID="lblCompanyId" runat="server"></asp:Label>
                                        </div>
                                        <%--<div class="editlabel">
                                            <asp:Label CssClass="title_text_Name" ID="lblCompanyName" runat="server"></asp:Label>
                                        </div>--%>
                                        <div class="btnclosewrap">
                                            <%--<asp:LinkButton CssClass="mdi-content-clear" ID="lnkClose" ForeColor="Red" runat="server" ToolTip="Close" CausesValidation="false"
                                                OnClick="lnkClose_Click"></asp:LinkButton>--%>
                                            <asp:LinkButton CssClass="mdi-content-clear" ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click" TabIndex="26"></asp:LinkButton>
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
                                    <li><a href="#accrdCompanyInfo">Company Info</a></li>
                                    <li><a href="#accrdDefault">Default</a></li>
                                    <li><a href="#accrdNotes">Notes</a></li>
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
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="container accordian-wrap">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                                        <li>
                                            <div id="accrdCompanyInfo" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-communication-business"></i>Company Info</div>
                                            <div class="collapsible-body">
                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div class="col s12">
                                                                <div class="form-section-row">
                                                                    <div class="section-ttle">Company Details</div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:Label ID="lblCompany" runat="server" Text="Company"></asp:Label>
                                                                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control input-sm input-small">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtCompanyID" runat="server" CssClass="validate"
                                                                                    MaxLength="15" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                                    ControlToValidate="txtCompanyID" ValidationGroup="general, rep" Display="None" ErrorMessage="ID required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                        ID="ValidatorCalloutExtender1"
                                                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                                                    </asp:ValidatorCalloutExtender>
                                                                                <%--<asp:Label ID="lblCompOfficeID" runat="server" Text="Company ID"></asp:Label>--%>
                                                                                <label id="lblCompOfficeID" runat="server" for="txtName">Company ID</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtName" runat="server" MaxLength="75" CssClass="validate"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                                    ControlToValidate="txtName" ValidationGroup="general, rep" Display="None" ErrorMessage="Name required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                        ID="ValidatorCalloutExtender2"
                                                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                                                                    </asp:ValidatorCalloutExtender>
                                                                                <label for="txtName">Name</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtCostCenter" runat="server" MaxLength="20" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtCostCenter">Center</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Status </label>
                                                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst selectsml">
                                                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Closed</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Hold</asp:ListItem>
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
                                                                                <%--                          <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="materialize-textarea"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                                    ControlToValidate="txtAddress" ValidationGroup="general, rep" Display="None" ErrorMessage="Address required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                        ID="ValidatorCalloutExtender3"
                                                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                                                                    </asp:ValidatorCalloutExtender>--%>
                                                                                <textarea id="txtAddress" runat="server" class="materialize-textarea" maxlength="100"></textarea>
                                                                                <label for="txtAddress">Address</label>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                                                    ControlToValidate="txtAddress" ValidationGroup="general, rep" Display="None" ErrorMessage="Address required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                        ID="ValidatorCalloutExtender5"
                                                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator5">
                                                                                    </asp:ValidatorCalloutExtender>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="50" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtCity">City</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtZip" MaxLength="10" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="email">Zip</label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <%--<asp:TextBox ID="txtState"  MaxLength="2" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtState">State</label>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                                                    ControlToValidate="txtState" ValidationGroup="general, rep" Display="None" ErrorMessage="State required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                        ID="ValidatorCalloutExtender6"
                                                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator6">
                                                                                    </asp:ValidatorCalloutExtender>--%>
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
                                                                                <label class="drpdwn-label">Country</label>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                                    ControlToValidate="ddlCountry" ValidationGroup="general, rep" InitialValue="State" Display="None" ErrorMessage="Country Required"
                                                                                    SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender3"
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

                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtLatitude" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtLatitude">Latitude</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtLongitude" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtLongitude">Longitude</label>
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

                                                                    <div class="form-section3">
                                                                        <div class="section-ttle">
                                                                            Contact Details
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtContactName" MaxLength="50" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtContactName">Contact Name</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtARContact" runat="server" MaxLength="75" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtARContact">AR Contact</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="25" CssClass="validate" placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                                                <label for="txtPhone">Phone</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtFax" runat="server" MaxLength="25" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtFax">Fax</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section9">
                                                                        <div class="section-ttle">
                                                                            Logo
                                                                        </div>
                                                                        <%--         <div class="form-section9">--%>
                                                                        <%--<label class="drpdwn-label">Logo</label>--%>
                                                                        <div class="input-field col s6">
                                                                            <div class="row">
                                                                                <asp:FileUpload ID="FileUpload1" CssClass="dropify" runat="server" onchange="ConfirmUpload();" />
                                                                                <asp:Button ID="btnUpload" runat="server" CssClass="form-control" OnClick="btnUpload_Click" Text="Upload Logo"
                                                                                    ValidationGroup="img" Style="display: none;" />
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
                                                                                    <div class="row" style="text-align: center;">
                                                                                        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Design/images/logonew.png" Style="height: 130px; width: 155px;" />
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>

                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="btnupload" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:Label ID="Label2" runat="server" Text="QB File Path (on server)"
                                                                                    ToolTip="Give the path of Quickbooks data file located on the hosted server."
                                                                                    Visible="False"></asp:Label>
                                                                            </div>
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtLogoPath" runat="server" MaxLength="500" Width="200px"
                                                                                    ToolTip="Give the path of Quickbooks data file located on the hosted server."
                                                                                    Enabled="False" CssClass="form-control" Visible="False"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <%--</div>--%>

                                                                        <div class="form-section3-blank">
                                                                            &nbsp;
                                                                        </div>
                                                                        <div class="form-section3half">
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
                                            <div id="accrdDefault" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-cached"></i>Default</div>
                                            <div class="collapsible-body">
                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div class="col s12">
                                                                <div class="form-section-row">

                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Salesperson</label>
                                                                                <asp:DropDownList ID="ddlSalePerson" runat="server" CssClass="browser-default">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Preferred Worker</label>
                                                                                <asp:DropDownList ID="ddlWorker" runat="server" CssClass="browser-default">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Zone</label>
                                                                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="browser-default">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Sales Tax</label>
                                                                                <asp:DropDownList ID="ddlSalesTax" runat="server" CssClass="browser-default">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Location Type </label>
                                                                                <asp:DropDownList ID="ddlLocationType" runat="server" CssClass="browser-default">
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
                                                                                <label class="drpdwn-label">AR Terms </label>
                                                                                <asp:DropDownList ID="ddlARTerms" runat="server" CssClass="browser-default">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12 mgntp10" style="display: none;">
                                                                            <div class="checkrow">
                                                                                <asp:CheckBox ID="chkUTaxR" CssClass="filled-in" runat="server"></asp:CheckBox>
                                                                                <label for="utax">Use Tax</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtDArea" runat="server" MaxLength="3" CssClass="validate"></asp:TextBox>
                                                                                <label for="defArea">Area Code</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <%--<asp:TextBox ID="txtDefState" runat="server" MaxLength="2" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtDefState">State</label>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                                                    ControlToValidate="txtDefState" ValidationGroup="general, rep" Display="None" ErrorMessage="State required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                        ID="ValidatorCalloutExtender4"
                                                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator4">
                                                                                    </asp:ValidatorCalloutExtender>--%>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                                                    ControlToValidate="ddlDefState" ValidationGroup="general, rep" InitialValue="State" Display="None" ErrorMessage="State Required"
                                                                                    SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender4"
                                                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator4">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <label class="drpdwn-label">State/Province</label>
                                                                                <asp:DropDownList ID="ddlDefState" runat="server" ToolTip="State" CssClass="browser-default">
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
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtMileRate" runat="server" CssClass="validate" MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                                                <label for="milRate">Mileage Rate</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12" style="display: none;">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Use Tax </label>
                                                                                <asp:DropDownList ID="ddlUseTax" runat="server" CssClass="browser-default">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtADP" MaxLength="3" runat="server"></asp:TextBox>
                                                                                <label for="txtADP">ADP</label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtCB" runat="server" MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                                                <label for="txtCB">Cost Burden</label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Inv Account </label>
                                                                                <asp:DropDownList runat="server" CssClass="browser-default"
                                                                                    ID="DrpAllChartAcct">
                                                                                </asp:DropDownList>
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
                                            <div id="accrdNotes" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-speaker-notes"></i>Notes</div>
                                            <div class="collapsible-body">
                                                <div class="form-content-wrap form-content-wrapwd">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div class="form-section3">
                                                                <div class="input-field col s12">
                                                                    <div class="row">
                                                                        <asp:TextBox ID="txtInvRemarks" runat="server" TextMode="MultiLine" CssClass="materialize-textarea textarea-border bigtxtarea"></asp:TextBox>
                                                                        <label for="txtInvRemarks" class="txtbrdlbl">Inv Remarks</label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-section3-blank">
                                                                &nbsp;
                                                            </div>
                                                            <div class="form-section3">
                                                                <div class="input-field col s12">
                                                                    <div class="row">
                                                                        <asp:TextBox ID="txtBillRemit" runat="server" TextMode="MultiLine" CssClass="materialize-textarea textarea-border bigtxtarea"></asp:TextBox>
                                                                        <label for="txtBillRemit" class="txtbrdlbl">Bill Remit</label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-section3-blank">
                                                                &nbsp;
                                                            </div>
                                                            <div class="form-section3">
                                                                <div class="input-field col s12">
                                                                    <div class="row">
                                                                        <asp:TextBox ID="txtPORemit" runat="server" TextMode="MultiLine" CssClass="materialize-textarea textarea-border bigtxtarea"></asp:TextBox>
                                                                        <label for="txtPORemit" class="txtbrdlbl">PO Remit</label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="cf"></div>
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
            </div>
        </div>
    </div>



    <div class="container accordian-wrap" style="display: none;">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="card" style="min-height: 700px !important; border-radius: 6px;">
                        <div class="card-content">
                            <div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <div class="form-section-row">
                                        <div class="row">
                                            <div class="col s12 m12 l12">
                                                <div class="row">
                                                    <div class="col s12 m12 l12">
                                                        <ul class="tabs tab-demo-active white" style="width: 100%;">
                                                            <li class="tab col s2">
                                                                <a class="waves-effect waves-light active" href="#activeone"><i class="mdi-maps-local-library"></i>&nbsp;Vendor Info</a>
                                                            </li>
                                                            <li class="tab col s2">
                                                                <a class="waves-effect waves-light" href="#two"><i class="mdi-editor-attach-money"></i>&nbsp;Transactions</a>
                                                            </li>

                                                        </ul>
                                                    </div>
                                                    <div class="col s12">
                                                        <div id="activeone" class="col s12 tab-container-border lighten-4" style="display: block;">
                                                            <div class="form-content-wrap" style="overflow: auto; margin-top: 20px; padding-top: 5px;">
                                                                <div class="tabs-custom-mgn1 mn-ht">
                                                                    <div class="form-content-wrap">
                                                                    </div>

                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div id="two" class="col s12 tab-container-border lighten-4" style="display: none;">
                                                            <div class="form-content-wrap" style="overflow: auto; margin-top: 20px; padding-top: 5px;">
                                                                <div class="tabs-custom-mgn1 mn-ht">
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
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
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
    <script>

</script>
</asp:Content>
