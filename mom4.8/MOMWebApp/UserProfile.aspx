<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="UserProfile" Codebehind="~/UserProfile.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <style>
        .eyeicon{
            width:40px;
            height:28px;
            padding:0;
            background-color:transparent;
            border:none;
            position:relative;
            margin-top:-40px;
            float: right;
        }
        button.eyeicon:focus{
            background-color:transparent;
        }

        .txtSignBody {
            min-height: 200px;
        }

        [id$='FontName'], [id$='FontSize'] {
            display: inline-block !important;
            margin: 0 !important;
            font-size: 12px !important;
        }

        .headerCollection .rwTitleWrapper {
            padding: 0 !important;
            font-size: 1.42857em;
            font-family: inherit;
            line-height: 1em;
            color: #fff;
            text-transform: capitalize;
        }

        .headerCollection .rwTitleBar {
            background-color: #1c5fb1 !important;
            padding: 5px;
        }
    </style>
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>

    <script type="text/javascript">
         function openDialog(url, modal, width, height) {
             if (radopen) {
                var wnd = radopen(url, null);
                wnd.set_destroyOnClose(true);
                //add checks here in case parameters have not been passed
                wnd.setSize(width, height);
                wnd.center();
                wnd.set_modal(modal);
            }
        }
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
                    types: ["geocode", "establishment"],
                    address: "#<%= txtAddress.ClientID %>",
                    city: "#<%= txtCity.ClientID %>",
                    state: "#<%= ddlState.ClientID %>",
                    zip: "#<%= txtZip.ClientID %>",
                    lat: "#<%= txtLat.ClientID %>",
                    lng: "#<%= txtLng.ClientID %>"
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
                    //if (cityAlt.length > 2)
                    //    for (var i = 0; i < result.address_components.length; i++) {
                    //        var addr = result.address_components[i];
                    //        if (addr.types[0] == 'administrative_area_level_2')
                    //            cityAlt = addr.short_name;
                    //    }

                    $("#<%=ddlCountry.ClientID%>").val(getCountry);
                    $("#<%=txtZip.ClientID%>").val(countryCode);
                    $("#<%=ddlState.ClientID%>").val(cityAlt);
                      $("#<%=txtCity.ClientID%>").val(city);
                        Materialize.updateTextFields();

                    });
                initialize();
                $('#<%=txtTelePhone.ClientID%>').mask("(999) 999-9999");
                $('#<%=txtCellular.ClientID%>').mask("(999) 999-9999");
            });
            chkEmailChange();

        });

        <%--function SelectSameSameIncoming() {
            document.getElementById('<%= emailTab.ClientID %>').click();
        }--%>
        function ConfirmUpload() {
            document.getElementById('<%=btnUpload.ClientID%>').click();
        }
        function OpenAvatarDialog() {
            document.getElementById('<%=FileUpload1.ClientID%>').click();
        }
        function OpenCoverPicDialog() {
            document.getElementById('<%=FileUpload2.ClientID%>').click();
        }
        function ConfirmUploadCoverPic() {
            document.getElementById('<%=btnUploadCoverPic.ClientID%>').click();
        }
        function chkEmailChange() {
            var isShowEmailTab = $('#<%=chkEmailAccount.ClientID%>').prop("checked");
            var valName = document.getElementById("<%=rfvEmail.ClientID%>");
            if (isShowEmailTab === true) {
                ValidatorEnable(valName, true);
                $('#<%=liEmail.ClientID%>').attr("style", "display:block;");
                $('#<%=liEmail.ClientID%> > a').click();
            } else {
                ValidatorEnable(valName, false);
                //var cssleft = $('.indicator').css("left");
                //var cssright = $('.indicator').css("right");

                //var tabWidth = (parseFloat(cssleft.replace("px", "")) + parseFloat(cssright.replace("px", "")) + 1)/5;


                $('#<%=liEmail.ClientID%>').attr("style", "display:none;");

                $('#<%=liLogin.ClientID%> > a').click();
            }

            //$('#<%=revealcard.ClientID%>').attr("style", "display: block; transform: translateY(-100%);");
        }

        function RefreshScreen() {
            document.getElementById("<%=btnRefreshScreen.ClientID%>").click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Setup" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkDashBoardSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnRefreshScreen">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_EmailSigns"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnDeleteEmailSign">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_EmailSigns"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div style="width: 100%; min-height: 800px !important; background-color: #272C32 !important;">
        <div class="container">

            <div id="profile-page" class="section">
                <!-- profile-page-header -->
                <div id="profile-page-header" class="card" style="height: 450px;">
                    <div class="card-image waves-effect waves-block waves-light add_img" style="background: linear-gradient(to top right, #1565C0, #272C32) !important">
                        <%--<img class="activator" src="Design/images/user-profile-bg.jpg" alt="user background">--%>
                        <asp:Image ID="imgUserBG" runat="server" ImageUrl="~/Design/images/user-bg-simple.jpg" alt="user background" CssClass="z-depth-2 responsive-img adimg" />
                        <div class="mdl">
                            <div class="text">
                                <label for="coverpic" onclick="OpenCoverPicDialog()" style="cursor: pointer;"><i class="mdi-file-cloud-upload" style="font-size: 2em; margin-left: -25px; color: #1865BE;"></i></label>
                            </div>
                        </div>
                        <%--<input id="coverpic" type="file" />--%>
                        <asp:FileUpload ID="FileUpload2" runat="server" onchange="ConfirmUploadCoverPic();" />
                        <asp:Button ID="btnUploadCoverPic" runat="server" CssClass="form-control" OnClick="btnUploadCoverPic_Click" Text="Upload Logo" ValidationGroup="img" TabIndex="23" Style="display: none;" />
                    </div>
                    <figure class="card-profile-image add_img">

                        <%--<img src="Design/images/avatar.jpg" alt="profile image" class="circle z-depth-2 responsive-img adimg" />--%>
                        <asp:Image ID="imgProfile" runat="server" ImageUrl="~/Design/images/avatar.jpg" alt="profile image" CssClass="circle z-depth-2 responsive-img adimg" style="width:180px;height:180px;" />
                        <div class="mdl">
                            <div class="text">
                                <label for="profilepic" onclick="OpenAvatarDialog()" style="cursor: pointer;"><i class="mdi-file-cloud-upload" style="font-size: 2em; margin-left: -25px; color: #222;"></i></label>
                            </div>
                        </div>
                        <%--<input id="profilepic" type="file" />--%>
                        <asp:FileUpload ID="FileUpload1" runat="server" onchange="ConfirmUpload();" />
                        <asp:Button ID="btnUpload" runat="server" CssClass="form-control" OnClick="btnUpload_Click" Text="Upload Logo" ValidationGroup="img" TabIndex="23" Style="display: none;" />
                    </figure>
                    <div class="card-content">
                        <div class="row" style="margin-bottom: 20px;">
                            <div class="col s8 offset-s2">
                                <%--<h4 class="card-title grey-text text-darken-4">Roger Waters</h4>--%>
                                <h4 class="card-title grey-text text-darken-4">
                                    <asp:Label runat="server" ID="lblUserName"></asp:Label></h4>
                                <%--<p class="medium-small grey-text">User Type</p>--%>
                                <p class="medium-small grey-text" runat="server" id="lblUserType"></p>
                            </div>

                            <div class="col s2 center-align">
                                <a class="btn-floating activator waves-effect waves-light darken-2 right" style="margin-right: 30px; background-color: #1565C0 !important">
                                    <i class="mdi-editor-border-color white-text"></i>
                                </a>
                            </div>
                        </div>
                    </div>
                    
                    <div id="revealcard" runat="server" class="card-reveal">

                        <span class="card-title grey-text text-darken-4" style="margin-top:-10px;">User Info<i class="mdi-navigation-close right"></i></span>
                        <div class="form-section-row" style="margin-bottom:0;">
                            <div class="btnlinks" style="height:26px;">
                                <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ToolTip="Save" Text="Save"></asp:LinkButton>
                            </div>
                            <div class="col s12 m12 l12">
                                <ul class="tabs tab-demo-active white" style="width: 100%;">
                                    <li id="liLogin" runat="server" class="tab col s2">
                                        <a class="waves-effect waves-light prodept" id="userDetTab" href="#up1">Login</a>
                                    </li>
                                    <li class="tab col s2" id="liAddress" runat="server" >
                                        <a class="waves-effect waves-light prodept" id="addressTab" href="#up2">Details</a>
                                    </li>
                                     <li class="tab col s2" id="liEmailSignature" runat="server" >
                                        <a class="waves-effect waves-light prodept" href="#up4">Email Signature</a>
                                    </li>
                                    <li class="tab col s2" id="liEmail" runat="server" >
                                        <a class="waves-effect waves-light prodept" href="#up3">Email Setup</a>
                                    </li>
                                    <li class="tab col s2" id="liDashboard" runat="server">
                                        <a class="waves-effect waves-light prodept" id="dashboardTab" href="#up5">Dashboard</a>
                                    </li>

                                </ul>
                            </div>


                            <div class="col s12 m12">

                                <div style="display: block;">
                                    <div class="form-content-wrap">
                                        <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                            <div class="form-section-row"  style="margin-bottom: 0;">
                                                <div class="row">
                                                    <div id="up1" class="col s12 tab-container-border lighten-4" style="display: block;">
                                                        <div class="tabs-custom-mgn1">
                                                            <div class="form-section-row" style="margin-bottom: 0;">
                                                                <div class="section-ttle">User Info</div>
                                                                <div class="form-section3">
                                                                    <div class="col s3">
                                                                        <asp:Image ID="imgProfile2" Style="width:59.3px;height:59.3px;" runat="server" ImageUrl="~/Design/images/avatar.jpg" alt="profile image" CssClass="circle z-depth-2 responsive-img adimg" />

                                                                    </div>
                                                                    <div class="col s9">
                                                                         <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label for="fName" id="lbFName" runat="server">First Name</label>
                                                                            <%--<input type="text" id="fName" />--%>
                                                                            <asp:TextBox runat="server" ID="txtFName"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12" id="divMName" runat="server">
                                                                        <div class="row">
                                                                            <label for="mName">Middle Name</label>
                                                                            <%--<input type="text" id="mName" />--%>
                                                                            <asp:TextBox runat="server" ID="txtMName"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12" id="divLName" runat="server">
                                                                        <div class="row">
                                                                            <label for="lName">Last Name</label>
                                                                            <%--<input type="text" id="lName" />--%>
                                                                            <asp:TextBox runat="server" ID="txtLName"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    
                                                                   
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <%--<input id="uTitle" type="text" class="validate">--%>
                                                                            <label for="uTitle" class="">User Title</label>
                                                                            <asp:TextBox runat="server" ID="txtUserTitle"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                  <%--  <div class="input-field col s2">
                                                                        <div class="row">
                                                                            &nbsp;
                                                                        </div>
                                                                    </div>--%>
                                                                    <div class="input-field col s12" id="divUserType" runat="server">
                                                                        <div class="row">
                                                                            <label class="drpdwn-label">User Type</label>
                                                                            <%--<select class="browser-default" disabled="disabled">
                                                                                <option>Select</option>
                                                                            </select>--%>
                                                                            <asp:DropDownList ID="ddlUserType" runat="server" CssClass="browser-default" Enabled="false"
                                                                                TabIndex="14">
                                                                                <asp:ListItem Value="0">Office</asp:ListItem>
                                                                                <asp:ListItem Value="1">Field</asp:ListItem>
                                                                                <asp:ListItem Value="2">Customer</asp:ListItem>
                                                                            </asp:DropDownList>
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
                                                                            <%--<input id="telephone" type="text" class="validate">--%>
                                                                            <asp:TextBox runat="server" ID="txtTelePhone" placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                                            <label for="telephone" class="">Phone</label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <%--<input id="cellular" type="text" class="">--%>
                                                                            <asp:TextBox runat="server" ID="txtCellular" placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                                            <label for="cellular">Cell</label>
                                                                        </div>
                                                                    </div>

                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <%--<input id="email" type="text" class="validate">--%>
                                                                            <asp:TextBox runat="server" ID="txtEmail"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="revEmail"
                                                                                runat="server" ControlToValidate="txtEmail" Display="None"
                                                                                ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                            </asp:RegularExpressionValidator>
                                                                            <asp:ValidatorCalloutExtender ID="rfvEmail_ValidatorCalloutExtender"
                                                                                runat="server" Enabled="True" TargetControlID="revEmail" PopupPosition="BottomLeft">
                                                                            </asp:ValidatorCalloutExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                                                Display="None" ErrorMessage="Email Required" SetFocusOnError="True" Enabled="False">
                                                                            </asp:RequiredFieldValidator>
                                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3"
                                                                                runat="server" Enabled="True" TargetControlID="rfvEmail">
                                                                            </asp:ValidatorCalloutExtender>
                                                                            <label for="email">Email</label>
                                                                        </div>
                                                                    </div>



                                                                    <div class="input-field col s12">
                                                                        <div class="row" id="divMsg" runat="server">
                                                                            <%--<input id="textmessage" type="text" class="validate">--%>
                                                                            <asp:TextBox runat="server" ID="txtMsg"></asp:TextBox>
                                                                            <label for="textmessage">Text Message</label>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                                <div class="form-section3-blank">
                                                                    &nbsp;
                                                                </div>

                                                                <div class="form-section3">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label for="uname">Username</label>
                                                                            <asp:TextBox runat="server" ID="txtUserName" Enabled="false" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
    // fix for mobile safari to show virtual keyboard
    this.blur();    this.focus();  }"></asp:TextBox>
                                                                            <%--<input type="text" id="uname" autocomplete="off" disabled="disabled" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
    // fix for mobile safari to show virtual keyboard
    this.blur();    this.focus();  }" />--%>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row" >
                                                                            <label for="pswd">Password</label>
                                                                            <asp:TextBox runat="server" ID="txtPassword" Enabled="false" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                // fix for mobile safari to show virtual keyboard
                                                                                this.blur();    this.focus();  }"></asp:TextBox>
                                                                            <button id="show_password" class="eyeicon" type="button">  
                                                                                <%--<span class="fa fa-eye-slash icon"></span>  --%>
                                                                                <img src="images/icon-show-password.svg" style="width: 15px;" />
                                                                            </button>  
                                                                            <%--<input type="password" id="pswd" autocomplete="off" disabled="disabled" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
    // fix for mobile safari to show virtual keyboard
    this.blur();    this.focus();  }" />--%>
                                                                        </div>
                                                                    </div>

                                                                    <div class="input-field col s12 mgntp10">
                                                                        <div class="checkrow" runat="server" id="divEmailAccount">
                                                                            <%--<input id="emailcheck" type="checkbox" class="filled-in">--%>
                                                                            <%--<asp:CheckBox runat="server" ID="chkEmailAccount" class="filled-in" OnCheckedChanged="chkEmailAccount_CheckedChanged" AutoPostBack="true" />
                                                                            <label for="chkEmailAccount" class="">Email Account</label>--%>
                                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:CheckBox ID="chkEmailAccount" runat="server" AutoPostBack="True" onchange="chkEmailChange();"
                                                                                        OnCheckedChanged="chkEmailAccount_CheckedChanged" CssClass="filled-in" />
                                                                                    <asp:Label runat="server" Text="Email Account"></asp:Label>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="up2" class="col s12 tab-container-border lighten-4">
                                                        <div class="tabs-custom-mgn1">
                                                            <div class="form-section-row"  style="margin-bottom: 0;">
                                                                <div class="form-section9">
                                                                    <div class="section-ttle">Address</div>
                                                                    <div class="form-section3half">
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="add">Address</label>
                                                                                <%--<textarea id="add" class="materialize-textarea"></textarea>--%>
                                                                                <textarea id="txtAddress" runat="server" class="materialize-textarea" maxlength="255"></textarea>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label for="city">City</label>
                                                                                <%--<input type="text" id="city" />--%>
                                                                                <asp:TextBox runat="server" ID="txtCity"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <%--<label for="state">State</label>                                                                                
                                                                                <asp:TextBox runat="server" ID="txtState" MaxLength="2"></asp:TextBox>--%>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                                                    ControlToValidate="ddlState" ValidationGroup="general, rep"  Display="None" ErrorMessage="State Required"
                                                                                    SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                    ID="RequiredFieldValidator7_ValidatorCalloutExtender"
                                                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator7">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <label >State/Province</label>
                                                                                  <asp:TextBox runat="server" ID="ddlState"></asp:TextBox>
                                                                               <%-- <asp:DropDownList ID="ddlState" runat="server" ToolTip="State" CssClass="browser-default">
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
                                                                                <label for="zip">Zip</label>
                                                                                <%--<input type="text" id="zip" />--%>
                                                                                <asp:TextBox runat="server" ID="txtZip"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <%--<input type="text" id="country" />--%>
                                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                                    ControlToValidate="ddlCountry" ValidationGroup="general, rep" InitialValue="State" Display="None" ErrorMessage="Country Required"
                                                                                    SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender3"
                                                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                                                                </asp:ValidatorCalloutExtender>--%>

                                                                                <asp:DropDownList ID="ddlCountry" runat="server" ToolTip="Country" CssClass="browser-default">
                                                                                    <asp:ListItem Value="">Select</asp:ListItem>
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
                                                                                <label for="ddlCountry" class="drpdwn-label">Country</label>

                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3half-blank">
                                                                        &nbsp;
                                                                    </div>

                                                                    <div class="form-section3half">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label for="long">Longitude</label>
                                                                                <%--<input type="text" id="long" />--%>
                                                                                <asp:TextBox runat="server" ID="txtLng"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">

                                                                            <div class="row">
                                                                                <label for="lat">Latitude</label>
                                                                                <%--<input type="text" id="lat" />--%>
                                                                                <asp:TextBox runat="server" ID="txtLat"></asp:TextBox>
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
                                                                <div class="form-section3-blank">
                                                                    &nbsp;
                                                                </div>
                                                                <div class="form-section3" id="divEmergency" runat="server">
                                                                    <div class="section-ttle">Emergency Contact</div>

                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label for="EmName">Emergency Contact Name</label>
                                                                            <%--<input type="text" id="EmName" />--%>
                                                                            <asp:TextBox runat="server" ID="txtEmName"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label for="EmNum">Emergency Contact Number</label>
                                                                            <%--<input type="text" id="EmNum" />--%>
                                                                            <asp:TextBox runat="server" ID="txtEmNum"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>

                                                        </div>
                                                    </div>
                                                    <div id="up3" class="col s12 tab-container-border lighten-4">
                                                        <div class="tabs-custom-mgn1">
                                                            <%--<div class="form-section-row" style="margin-bottom: 0;">--%>
                                                                <%--<div class="form-section3half">--%>
                                                                    <asp:UpdatePanel runat="server" ID="updatepnlemail">
                                                                        <ContentTemplate>
                                                                            <asp:Panel ID="pnlEmailAccount" runat="server" Visible="False">

                                                                                <div class="form-section-row" style="margin-bottom: 0;">
                                                                                    <div class="form-section3half">
                                                                                        <div class="section-ttle">
                                                                                            Incoming Mail Settings
                                                                                        </div>
                                                                                        <div class="form-section-row" style="margin-bottom: 0;">
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <label for="txtInServer">Incoming Mail Server (IMAP)</label>
                                                                                                    <%--<input type="text" id="imap" />--%>
                                                                                                    <asp:TextBox ID="txtInServer" runat="server"
                                                                                                        MaxLength="100"></asp:TextBox>

                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19"
                                                                                                        runat="server" ControlToValidate="txtInServer" Display="None" ErrorMessage="Required"
                                                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                    <asp:ValidatorCalloutExtender
                                                                                                        ID="RequiredFieldValidator19_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                        PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator19">
                                                                                                    </asp:ValidatorCalloutExtender>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s5" style="border-bottom: 1px solid #9e9e9e">
                                                                                                <div class="checkrow">
                                                                                                    <%--input id="ssl" type="checkbox" class="filled-in">--%>
                                                                                                    <asp:CheckBox ID="chkSSL" CssClass="filled-in" runat="server" />
                                                                                                    <label for="chkSSL" class="margin-top-3px">SSL</label>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s2">
                                                                                                <div class="row">
                                                                                                    &nbsp;
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s5">
                                                                                                <div class="row">
                                                                                                    <label for="portin">Port</label>
                                                                                                    <%--<input type="text" id="portin" />--%>
                                                                                                    <asp:TextBox ID="txtinPort" runat="server"></asp:TextBox>
                                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                                        FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtinPort">
                                                                                                    </asp:FilteredTextBoxExtender>

                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtinPort"
                                                                                                        Display="None" ErrorMessage="Required" SetFocusOnError="True">
                                                                                                    </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                                        ID="RequiredFieldValidator20_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                        PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator20">
                                                                                                    </asp:ValidatorCalloutExtender>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s5">
                                                                                                <div class="row">
                                                                                                    <label for="unamein">Email Username
                                                                                                    </label>
                                                                                                    <%--            <input type="text" id="unamein" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                        // fix for mobile safari to show virtual keyboard
                                                                                        this.blur();    this.focus();  }" />--%>
                                                                                                    <asp:TextBox ID="txtInUSername" runat="server" CssClass="validate" AutoCompleteType="Disabled" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                                    // fix for mobile safari to show virtual keyboard
                                                                                                    this.blur();    this.focus();  }" MaxLength="100"></asp:TextBox> <%--onfocusout="CopyToBCC()"--%>
                                                                                                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                                                                        ControlToValidate="txtInUSername" Display="None" ErrorMessage="Invalid Username"
                                                                                                        SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                                    </asp:RegularExpressionValidator>
                                                                                                    <asp:ValidatorCalloutExtender
                                                                                                        ID="RegularExpressionValidator2_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                        TargetControlID="RegularExpressionValidator2" PopupPosition="TopLeft">
                                                                                                    </asp:ValidatorCalloutExtender>--%>
                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtInUSername"
                                                                                                        Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator21_ValidatorCalloutExtender"
                                                                                                        runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator21">
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
                                                                                                    <label for="pwdin">Email Password</label>
                                                                                                    <%--   <input type="password" id="pwdin" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                        // fix for mobile safari to show virtual keyboard
                                                                                        this.blur();    this.focus();  }" />--%>
                                                                                                    <asp:TextBox ID="txtInPassword" TextMode="Password" OnPreRender="txtInPassword_PreRender" runat="server" AutoCompleteType="Disabled" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                    // fix for mobile safari to show virtual keyboard
                                                                                    this.blur();    this.focus();  }"
                                                                                                        MaxLength="50"></asp:TextBox>
                                                                                                    <%--<telerik:RadTextBox RenderMode="Auto" runat="server" ID="txtInPassword"  Width="200px" TextMode="Password"></telerik:RadTextBox>--%>
                                                                                                
                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server"
                                                                                                        ControlToValidate="txtInPassword" Display="None" ErrorMessage="Required" SetFocusOnError="True">
                                                                                                    </asp:RequiredFieldValidator>
                                                                                                    <asp:ValidatorCalloutExtender
                                                                                                        ID="RequiredFieldValidator22_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                        PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator22">
                                                                                                    </asp:ValidatorCalloutExtender>

                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s5">
                                                                                                <div class="row">
                                                                                                    <label for="pwdin">Bcc Email</label>
                                                                                                    <asp:TextBox ID="txtBccEmail" runat="server" MaxLength="50" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                                    // fix for mobile safari to show virtual keyboard
                                                                                                    this.blur();    this.focus();  }"></asp:TextBox>
                                                                                                    <asp:RegularExpressionValidator
                                                                                                        ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtBccEmail"
                                                                                                        Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                                    </asp:RegularExpressionValidator>
                                                                                                    <asp:ValidatorCalloutExtender
                                                                                                        ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                                                        TargetControlID="RegularExpressionValidator4" PopupPosition="TopLeft">
                                                                                                    </asp:ValidatorCalloutExtender>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s12 mgntp17">
                                                                                                <div class="row">
                                                                                                    <div class="btnlinks">
                                                                                                        <%--<asp:LinkButton ID="btnTestIncoming" runat="server"  OnClick="btnTestIncoming_Click" Text="Test Settings" />--%>
                                                                                                        <asp:LinkButton ID="btnTestIncoming" runat="server" OnClick="btnTestIncoming_Click" Text="Test Settings"></asp:LinkButton>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>

                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3half-blank">
                                                                                        &nbsp;
                                                                                    </div>
                                                                                    <div class="form-section3half">

                                                                                        <div class="section-ttle">
                                                                                            Outgoing Mail Settings
                                                 
                                                                                        </div>
                                                                                        <div class="form-section-row">
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <label for="txtOutServer">Outgoing Mail Server (SMTP)</label>
                                                                                                    <%--<input type="text" id="smtp" />--%>
                                                                                                    <asp:TextBox ID="txtOutServer" runat="server" MaxLength="100"></asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator23"
                                                                                                        runat="server" ControlToValidate="txtOutServer" Display="None" ErrorMessage="Required"
                                                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                    <asp:ValidatorCalloutExtender
                                                                                                        ID="RequiredFieldValidator23_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                        PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator23">
                                                                                                    </asp:ValidatorCalloutExtender>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s5" style="border-bottom: 1px solid #9e9e9e">
                                                                                                <div class="checkrow">
                                                                                                    <%--<input id="sslo" type="checkbox" class="filled-in">--%>
                                                                                                    <asp:CheckBox ID="chkSame" runat="server" CssClass="filled-in" AutoPostBack="True" OnCheckedChanged="chkSame_CheckedChanged" />
                                                                                                    <label for="chkSame" class="margin-top-3px">Same as Incoming</label>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s2">
                                                                                                <div class="row">
                                                                                                    &nbsp;
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s5">
                                                                                                <div class="row">
                                                                                                    <label for="porto">Port</label>
                                                                                                    <%--<input type="text" id="porto" />--%>
                                                                                                    <asp:TextBox ID="txtOutPort" runat="server"></asp:TextBox>
                                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                                        FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtOutPort">
                                                                                                    </asp:FilteredTextBoxExtender>
                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtOutPort"
                                                                                                        Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                    <asp:ValidatorCalloutExtender
                                                                                                        ID="RequiredFieldValidator24_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                        PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator24">
                                                                                                    </asp:ValidatorCalloutExtender>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s5">
                                                                                                <div class="row">
                                                                                                    <label for="unameo">Email Username</label>
                                                                                                    <%--<input type="text" id="unameo" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                                    // fix for mobile safari to show virtual keyboard
                                                                                                    this.blur();    this.focus();  }" />--%>
                                                                                                    <asp:TextBox ID="txtOutUsername" runat="server" MaxLength="100">
                                                                                                    </asp:TextBox>
                                                                                                    <%--onfocus="if (this.hasAttribute('readonly')) 
                                                                                                                { 
                                                                                                                    this.removeAttribute('readonly');
                                                                                                                    // fix for mobile safari to show virtual keyboard
                                                                                                                    this.blur();
                                                                                                                    this.focus(); 
                                                                                                                }"--%>
                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server"
                                                                                                        ControlToValidate="txtOutUsername" Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                    <%--<asp:RegularExpressionValidator
                                                                                                        ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOutUsername"
                                                                                                        Display="None" ErrorMessage="Invalid Username" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                                    </asp:RegularExpressionValidator>
                                                                                                    <asp:ValidatorCalloutExtender
                                                                                                        ID="RegularExpressionValidator3_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                        TargetControlID="RegularExpressionValidator3" PopupPosition="TopLeft">
                                                                                                    </asp:ValidatorCalloutExtender>--%>
                                                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator25_ValidatorCalloutExtender"
                                                                                                        runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator25">
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
                                                                                                    <label for="pwdin">Email Password</label>
                                                                                                    <%--<input type="password" id="pwdo" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                                    // fix for mobile safari to show virtual keyboard
                                                                                                    this.blur();    this.focus();  }" />--%>
                                                                                                    <asp:TextBox ID="txtOutPassword" TextMode="Password" OnPreRender="txtOutPassword_PreRender" runat="server" MaxLength="50">
                                                                                                    </asp:TextBox>
                                                                                                    <!--onfocus="if (this.hasAttribute('readonly')) 
                                                                                                                    { 
                                                                                                                        this.removeAttribute('readonly');
                                                                                                                        // fix for mobile safari to show virtual keyboard
                                                                                                                        this.blur();
                                                                                                                        this.focus();  
                                                                                                                    }"-->
                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server"
                                                                                                        ControlToValidate="txtOutPassword" Display="None" ErrorMessage="Required" SetFocusOnError="True">
                                                                                                    </asp:RequiredFieldValidator>
                                                                                                    <asp:ValidatorCalloutExtender
                                                                                                        ID="RequiredFieldValidator26_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                        PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator26">
                                                                                                    </asp:ValidatorCalloutExtender>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s5" style="border-bottom: 1px solid #9e9e9e">
                                                                                                <div class="checkrow">
                                                                                                    <%--<input id="sslo" type="checkbox" class="filled-in">--%>
                                                                                                    <asp:CheckBox ID="chkTakeASentEmailCopy" runat="server" CssClass="filled-in"/>
                                                                                                    <label for="chkTakeASentEmailCopy" class="margin-top-3px">Send Copy to "Sent Items"</label>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s12 mgntp17">
                                                                                                <div class="row">
                                                                                                    <div class="btnlinks">
                                                                                                        <asp:LinkButton ID="btnTestOut" runat="server" OnClick="btnTestOut_Click" Text="Test Settings" />
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                
                                                                <%--</div>--%>
                                                            <%--</div>--%>
                                                        </div>
                                                    </div>
                                                    <div id="up4" class="col s12 tab-container-border lighten-4">
                                                        <div class="tabs-custom-mgn1">
                                                            <div class="form-content-wrap">
                                                                <div class="form-content-pd">
                                                                    <div class="btncontainer">
                                                                        <div class="btnlinks">
                                                                            <asp:LinkButton ID="lnkAddEmailSign" runat="server" CausesValidation="False" OnClientClick="OpenAddEmailSignatureWindow();return false;">Add</asp:LinkButton>
                                                                        </div>
                                                                        <div class="btnlinks">
                                                                            <asp:LinkButton ID="btnEditEmailSign" runat="server" CausesValidation="False" OnClientClick="OpenEditEmailSignatureWindow();return false;">Edit</asp:LinkButton>
                                                                        </div>
                                                                        <div class="btnlinks">
                                                                            <asp:LinkButton ID="btnDeleteEmailSign" runat="server" OnClick="btnDeleteEmailSign_Click">Delete</asp:LinkButton>
                                                                        </div>
                                                                        <div style="display: none;">
                                                                            <a id="lnkRefreshScreen" style="display: none;" onclick="RefreshScreen()"></a>
                                                                            <asp:LinkButton ID="btnRefreshScreen" Style="display: none;" runat="server" CausesValidation="false" OnClick="lnkRefreshScreen_Click"></asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                    <div class="grid_container">
                                                                        <div class="RadGrid RadGrid_Material FormGrid">
                                                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                                                <div class="RadGrid RadGrid_Material">
                                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_EmailSigns" runat="server">
                                                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_EmailSigns" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                                            OnNeedDataSource="RadGrid_EmailSigns_NeedDataSource" PagerStyle-AlwaysVisible="true"
                                                                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                                            <CommandItemStyle />
                                                                                            <GroupingSettings CaseSensitive="false" />
                                                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                            </ClientSettings>
                                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true">
                                                                                                <Columns>
                                                                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40">
                                                                                                    </telerik:GridClientSelectColumn>

                                                                                                    <telerik:GridTemplateColumn DataField="SignName" HeaderText="Name" SortExpression="SignName"
                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="200"
                                                                                                        ShowFilterIcon="false">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblSignName" Text='<%# Eval("SignName")%>' runat="server"></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnSignId" Value='<%# Bind("Id") %>' runat="server" />
                                                                                                        </ItemTemplate>
                                                                                                    </telerik:GridTemplateColumn>
                                                                                                    <telerik:GridTemplateColumn DataField="SignContent" HeaderText="Signature" HeaderStyle-Width="400"
                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Title"
                                                                                                        ShowFilterIcon="false">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblSignContent" Text='<%# Eval("SignContent")%>' runat="server"></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </telerik:GridTemplateColumn>

                                                                                                    <telerik:GridTemplateColumn DataField="IsDefault" HeaderText="Is Default" HeaderStyle-Width="100"
                                                                                                        AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" SortExpression="Title"
                                                                                                        ShowFilterIcon="false">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblIsDefault" Text='<%# (Convert.ToString(Eval("IsDefault")) == "True") ? "Yes" : "No" %>' runat="server"></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </telerik:GridTemplateColumn>
                                                                                                </Columns>
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
                                                                                    </telerik:RadAjaxPanel>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="cf"></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="up5" class="col s12 tab-container-border lighten-4">
                                                        <div class="tabs-custom-mgn1">
                                                            <asp:UpdatePanel runat="server" ID="DashboardUpdatePanel">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="lnkAddDashboard" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="lnkEditDashboard" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="lnkDelDashboard" EventName="Click" />
                                                                </Triggers>

                                                                <ContentTemplate>
                                                                    <div class="form-section-row">
                                                                        <div class="section-ttle">
                                                                            Dashboard Settings
                                                                        </div>
                                                                        <div class="form-section3">

                                                                            <div class="tabs-custom-mgn1 mn-ht">
                                                                                <div class="form-section-row">

                                                                                    <div class="btnlinks">
                                                                                        <asp:LinkButton ID="lnkAddDashboard" runat="server" OnClick="lnkAddDashboard_Click" CausesValidation="false">Add</asp:LinkButton>
                                                                                    </div>
                                                                                    <div class="btnlinks">
                                                                                        <asp:LinkButton ID="lnkEditDashboard" runat="server" OnClick="lnkEditDashboard_Click" CausesValidation="false">Edit</asp:LinkButton>
                                                                                    </div>
                                                                                    <div class="btnlinks">
                                                                                        <asp:LinkButton ID="lnkDelDashboard" runat="server" CausesValidation="false" OnClick="lnkDelDashboard_Click" OnClientClick="return confirm('Do you really want to delete this dashboard ?');">Delete</asp:LinkButton>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="form-section-row">
                                                                                    <div class="grid_container">
                                                                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                                                                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Dashboard">
                                                                                                    <telerik:RadGrid ID="gvListDashboard" ShowFooter="True"
                                                                                                        runat="server" Width="100%">
                                                                                                        <CommandItemStyle />
                                                                                                        <GroupingSettings CaseSensitive="false" />
                                                                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                        </ClientSettings>
                                                                                                        <MasterTableView AutoGenerateColumns="false" ShowFooter="True" DataKeyNames="ID">
                                                                                                            <Columns>
                                                                                                                <telerik:GridTemplateColumn HeaderText="ID" DataField="ID" UniqueName="lblDbID">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblDbID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-CssClass="text_center" ItemStyle-CssClass="text_center">
                                                                                                                </telerik:GridClientSelectColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Name" DataField="Name" UniqueName="lblDbName" HeaderStyle-Width="70%" ItemStyle-Width="70%" HeaderStyle-CssClass="text_center" ItemStyle-CssClass="text_center">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblDbName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Default" DataField="IsDefault" UniqueName="lblDbDefault" HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-CssClass="text_center" ItemStyle-CssClass="text_center">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblDbDefault" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("IsDefault").ToString()) && Convert.ToBoolean(Eval("IsDefault")) ? "Default" : "" %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                            </Columns>
                                                                                                        </MasterTableView>
                                                                                                    </telerik:RadGrid>
                                                                                                </telerik:RadAjaxPanel>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                    </div>

                                                                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" VisibleStatusbar="false" CssClass="headerCollection">
                                                                        <Windows>
                                                                            <telerik:RadWindow ID="AddDashboardWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true" ReloadOnShow="true"
                                                                                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                                                                runat="server" Modal="true" Width="800" Height="600">

                                                                                <ContentTemplate>
                                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" UpdateMode="Conditional">
                                                                                        <Contenttemplate>
                                                                                        </Contenttemplate>
                                                                                    </telerik:RadAjaxPanel>
                                                                                    <header style="float: left; padding-left: 0 !important; margin-bottom: 30px;">
                                                                                        <div class="btnlinks">
                                                                                            <asp:LinkButton ID="lnkDashBoardSave" runat="server" OnClick="lnkDashBoardSave_Click" ValidationGroup="cont1">Save</asp:LinkButton>
                                                                                        </div>
                                                                                    </header>
                                                                                    <div style="margin-top: 15px;">
                                                                                        <div class="form-section-row">

                                                                                            <div class="form-section2">
                                                                                                <div class="input-field">
                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDashboardName"
                                                                                                        Display="None" ErrorMessage="Name Required" SetFocusOnError="True" ValidationGroup="cont1"></asp:RequiredFieldValidator>
                                                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2"
                                                                                                        runat="server" Enabled="True" PopupPosition="BottomRight" TargetControlID="RequiredFieldValidator1">
                                                                                                    </asp:ValidatorCalloutExtender>

                                                                                                    <asp:TextBox ID="txtDashboardName" runat="server" MaxLength="100" CssClass="Contact-search"></asp:TextBox>
                                                                                                    <asp:HiddenField ID="txtDashboardID" runat="server"></asp:HiddenField>
                                                                                                    <label for="txtDashboardName">Dashboard Name</label>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="form-section2">
                                                                                                <div class="input-field">
                                                                                                    <asp:CheckBox ID="chkDefault" CssClass="css-checkbox" Text="Default" runat="server" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="form-section-row">
                                                                                            <div class="section-ttle">
                                                                                                List of KPIs
                                                                                            </div>

                                                                                            <div class="form-section-rơw">
                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                        <div class="grid_container">
                                                                                                            <div class="col-lg-7 col-md-7 col-sm-7">
                                                                                                                <div class="col-sm-8 table-scrollable-borderless">
                                                                                                                    <div class="RadGrid RadGrid_Material">
                                                                                                                        <telerik:RadGrid ID="gvKPIs" runat="server" AllowMultiRowSelection="true" OnNeedDataSource="gvKPIs_NeedDataSource"
                                                                                                                            AutoGenerateColumns="False" CssClass="gv-list-kpis table table-bordered table-striped table-condensed flip-content">
                                                                                                                            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                                                                                                            <CommandItemStyle />
                                                                                                                            <GroupingSettings CaseSensitive="false" />
                                                                                                                            <ActiveItemStyle CssClass="evenrowcolor" />
                                                                                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true"></Selecting>
                                                                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                                            </ClientSettings>
                                                                                                                            <MasterTableView AutoGenerateColumns="false" ShowFooter="True" DataKeyNames="ID">
                                                                                                                                <Columns>
                                                                                                                                    <telerik:GridClientSelectColumn HeaderText="Allow" UniqueName="ClientSelectColumn" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                                                                                                    </telerik:GridClientSelectColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="KPI" DataField="Name" UniqueName="Name" HeaderStyle-CssClass="text_center" ItemStyle-CssClass="text_center">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Module" DataField="Module" UniqueName="Module" HeaderStyle-CssClass="text_center" ItemStyle-CssClass="text_center">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:Label ID="lblModule" runat="server" Text='<%# Bind("Module") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Screen" DataField="Name" UniqueName="Screen" HeaderStyle-CssClass="text_center" ItemStyle-CssClass="text_center">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:Label ID="lblScreen" runat="server" Text='<%# Bind("Screen") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                                </Columns>
                                                                                                                            </MasterTableView>
                                                                                                                            <SelectedItemStyle CssClass="selectedrowcolor" />
                                                                                                                            <AlternatingItemStyle CssClass="oddrowcolor" />
                                                                                                                        </telerik:RadGrid>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>

                                                                                        </div>
                                                                                        <div style="clear: both;"></div>

                                                                                    </div>
                                                                                </ContentTemplate>
                                                                            </telerik:RadWindow>

                                                                            <telerik:RadWindow ID="EmailSignatureWindow" Skin="Material" VisibleTitlebar="true" Title="Email Signature" Behaviors="Default" CenterIfModal="true"
                                                                                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="300"
                                                                                runat="server" Modal="true" Width="800" ShowContentDuringLoad="false" Height="530">
                                                                            </telerik:RadWindow>
                                                                        </Windows>
                                                                    </telerik:RadWindowManager>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
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
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdnUserId" runat="server" />
    </div>
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

            $("#<%= txtPassword.ClientID %>").attr('type', 'password');

            $('#show_password').click(function show() {
                var currentType = $("#<%= txtPassword.ClientID %>").attr('type');
                if (currentType == 'password') {
                    //Change the attribute to text 
                    $("#<%= txtPassword.ClientID %>").attr('type', 'text');
                    $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                }
                else {
                    //Change the attribute back to password  
                    $("#<%= txtPassword.ClientID %>").attr('type', 'password');
                    $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
                }
            }); 
        });
        function CopyToBCC() {
            $("#<%=txtBccEmail.ClientID%>").val($("#<%=txtInUSername.ClientID%>").val());
            $("#<%=txtBccEmail.ClientID%>").focus();
        }

        /* Email Signature*/

        function OpenEditEmailSignatureWindow() {
            var RefID = "";
            $("#<%= RadGrid_EmailSigns.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                if ($tr.find('input[type="checkbox"]').prop("checked") == true) {
                    var val = $tr.find('input[id*=hdnSignId]').val();
                    RefID = val;
                }
            });
            var userId = $("#<%= hdnUserId.ClientID %>").val();;
            var strUrl = "EmailSignaturePopup.aspx?id=" + RefID + "&userId=" + userId + "&page=userprofile";
            var oWnd = radopen(strUrl, "<%=EmailSignatureWindow.ClientID%>"); //Pass parameter using URL 

            oWnd.setSize(800, 530);
            oWnd.minimize();
            oWnd.maximize();
            oWnd.restore();
            oWnd.center();
            oWnd.set_modal(true);
            oWnd.show();
        }

        function OpenAddEmailSignatureWindow() {
            var userId = $("#<%= hdnUserId.ClientID %>").val();
            var strUrl = "EmailSignaturePopup.aspx?userId=" + userId + "&page=userprofile";
            var oWnd = radopen(strUrl, "<%=EmailSignatureWindow.ClientID%>"); //Pass parameter using URL 

            oWnd.setSize(800, 530);
            //oWnd.minimize();
            //oWnd.maximize();
            oWnd.restore();
            oWnd.center();
            oWnd.set_modal(true);
            oWnd.show();
        }
    </script>
</asp:Content>

