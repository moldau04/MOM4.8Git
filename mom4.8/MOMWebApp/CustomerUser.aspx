<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="CustomerUser" CodeBehind="CustomerUser.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <script src="js/jquery.sumoselect.min.js"></script>
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <link href="Styles/sumoselect.css" rel="stylesheet" type="text/css" />

    <style>
        .btn-addgroup a {
            border: 0.5px solid #1C5FB1;
            color: #1C5FB1;
            padding: 5px 20px 5px 20px !important;
            border-radius: 3px;
            font-size: 0.9em;
            background-repeat: repeat-x;
            background-color: #ffffff;
        }

        .add-edit-group {
            border: 0.5px solid #1C5FB1;
            color: #1C5FB1;
            height: 36px !important;
            padding: 4px 12px !important;
            margin-top: 4px;
            margin-left: 15px;
            border-radius: 3px;
            font-size: 0.9em;
            background-repeat: repeat-x;
            background-color: #ffffff;
        }

        .collapsible-body a.add-edit-group i {
            margin-right: 0 !important;
        }

        .custuser-popup {
            display: block;
            background: #fff;
            border: 1px solid #316b9d;
            border-radius: 6px 6px 0px 0px;
        }

            .custuser-popup .custuser-popup-header {
                background: #1c5fb1;
                color: #fff;
                padding: 13px 15px;
                border-radius: 6px 6px 0px 0px;
                display: flex;
                align-items: center;
                justify-content: space-between;
            }

                .custuser-popup .custuser-popup-header .pc-title {
                    width: 100%;
                }

                .custuser-popup .custuser-popup-header .title_text {
                    font-size: 16px;
                }

                .custuser-popup .custuser-popup-header .pt-right {
                    float: right;
                }

        .custuser-popup-body table th {
            background-color: #e9f0f6;
            border: 1px solid #ddd;
        }

        .custuser-popup-body table td.row-content {
            background: #f7f7f7;
            border: 1px solid #ddd;
        }

        /*.custuser-popup-body table .footer td {
            background: #ffffff;
            border: none;
        }*/

        .custuser-popup-body .btn, 
        .custuser-popup-body .btn-large {
            background-color: #1c5fb1;
            padding: 0 12px;
        }

            .custuser-popup-body .btn i, 
            .custuser-popup-body .btn-large i {
                font-size: 1rem;
            }

            .custuser-popup-body .btn:not(:last-child), 
            .custuser-popup-body .btn-large:not(:last-child) {
                margin-right: 8px;
            }

            .custuser-popup-body .btn:hover, .custuser-popup-body .btn-large:hover {
                background-color: #4b80bf;
            }

        .collapsible-body .RadGrid_Material a {
            padding: 0;
        }

        .custuser-popup-body .ajax__validatorcallout_popup_table{
            width: 200px;
        }
    </style>

    <script>
        $(document).ready(function () {
            $('#<%=txtPhoneCust.ClientID%>').mask("(999) 999-9999");
            $('#<%=txtCell.ClientID%>').mask("(999) 999-9999");
        });
    </script>
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
                                    <div class="page-title"><i class="mdi-social-person-outline"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Edit Customer User</asp:Label></div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ToolTip="Save" Text="Save"
                                            OnClientClick="itemJSON();"></asp:LinkButton>
                                        <asp:HiddenField ID="hdnLocCount" runat="server" />
                                    </div>
                                    <div class="rght-content">
                                        <div class="btnclosewrap">
                                            <%--<a href="#"><i class="mdi-content-clear"></i></a>--%>
                                            <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false"
                                                OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="rght-content" style="margin-top: 5px; margin-right: 6px;">
                                        <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
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
                                    <li class="accrdCustomerInfo"><a href="#accrdCustomerInfo">Customer Info</a></li>
                                    <li class="accrdLocation"><a href="#accrdLocation">Locations</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="Panel4" runat="server" Visible="false">
                                        <asp:Panel ID="Panel5" runat="server">
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" OnClick="lnkFirst_Click" CausesValidation="False">
                                                        <i class="fa fa-angle-double-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkPrevious" CssClass="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False" OnClick="lnkPrevious_Click">
                                                        <i class="fa fa-angle-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkNext" CssClass="lnkNext" ToolTip="Next" runat="server" CausesValidation="False" OnClick="lnkNext_Click">
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
                                            <div id="accrdCustomerInfo" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>Customer Info.</div>
                                            <div class="collapsible-body">
                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div class="col s12">
                                                                <div class="form-section-row">
                                                                    <div class="section-ttle">Customer Details</div>
                                                                    <div class="form-section3">
                                                                        <div class="col s12">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <label for="txtCName">Customer Name</label>
                                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCName"
                                                                                        ErrorMessage="First Name Required" Display="None" SetFocusOnError="True" ID="RequiredFieldValidator1"></asp:RequiredFieldValidator>
                                                                                    <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1"
                                                                                        ID="RequiredFieldValidator1_ValidatorCalloutExtender">
                                                                                    </asp:ValidatorCalloutExtender>
                                                                                    <asp:TextBox runat="server" MaxLength="75" CssClass="form-control" TabIndex="1" ID="txtCName"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <label for="txtAddress">Customer Address</label>
                                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress"
                                                                                        ErrorMessage="Address Required" Display="None" SetFocusOnError="True" ID="RequiredFieldValidator11"></asp:RequiredFieldValidator>
                                                                                    <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="RequiredFieldValidator11"
                                                                                        ID="RequiredFieldValidator11_ValidatorCalloutExtender">
                                                                                    </asp:ValidatorCalloutExtender>
                                                                                    <asp:TextBox runat="server" MaxLength="8000" TextMode="MultiLine" CssClass="txtAddress materialize-textarea validate" TabIndex="2" ID="txtAddress"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <label for="txtAddress2">Customer Address 2</label>
                                                                                    <asp:TextBox runat="server" CssClass="form-control" TabIndex="3" ID="txtAddress2"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label for="txtCity">City</label>
                                                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCity" ErrorMessage="City Required"
                                                                                    Display="None" SetFocusOnError="True" ID="RequiredFieldValidator6"></asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="RequiredFieldValidator6"
                                                                                    ID="RequiredFieldValidator6_ValidatorCalloutExtender">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <asp:TextBox runat="server" MaxLength="50" CssClass="form-control" TabIndex="4" ID="txtCity"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">State/Province</label>
                                                                                <asp:RequiredFieldValidator runat="server" InitialValue="State" ControlToValidate="ddlState"
                                                                                    ErrorMessage="State Required" Display="None" SetFocusOnError="True" ID="RequiredFieldValidator7"></asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="RequiredFieldValidator7" ID="RequiredFieldValidator7_ValidatorCalloutExtender">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <asp:DropDownList runat="server" CssClass="browser-default validate" TabIndex="5" ToolTip="State"
                                                                                    ID="ddlState">
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
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label for="txtZip">Zip</label>
                                                                                <asp:TextBox runat="server" MaxLength="10" CssClass="form-control" TabIndex="6" ID="txtZip"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s7">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtRemarks">Remarks</label>
                                                                                <asp:TextBox runat="server" MaxLength="8000" TextMode="MultiLine" CssClass="materialize-textarea validate"
                                                                                    TabIndex="19" ID="txtRemarks"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtMaincontact">Main Contact</label>
                                                                                <asp:TextBox runat="server" MaxLength="50" CssClass="form-control" TabIndex="7"
                                                                                    ID="txtMaincontact"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label for="txtPhoneCust">Phone</label>
                                                                                <asp:TextBox runat="server" MaxLength="28" CssClass="form-control" TabIndex="8"
                                                                                    ID="txtPhoneCust"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label for="txtCell">Cellular</label>
                                                                                <asp:TextBox runat="server" MaxLength="28" CssClass="form-control" TabIndex="11" ID="txtCell"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtEmail">Email</label>
                                                                                <asp:RegularExpressionValidator runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                                    ControlToValidate="txtEmail" ErrorMessage="Invalid Email" Display="None" SetFocusOnError="True"
                                                                                    ID="RegularExpressionValidator2"></asp:RegularExpressionValidator>
                                                                                <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2"
                                                                                    ID="RegularExpressionValidator2_ValidatorCalloutExtender">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <asp:FilteredTextBoxExtender runat="server" FilterMode="InvalidChars" InvalidChars=" "
                                                                                    Enabled="True" TargetControlID="txtEmail" ID="txtEmail_FilteredTextBoxExtender">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:TextBox runat="server" MaxLength="50" CssClass="form-control" TabIndex="10"
                                                                                    ID="txtEmail"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtWebsite">Website</label>
                                                                                <asp:TextBox runat="server" MaxLength="50" CssClass="form-control" TabIndex="9"
                                                                                    ID="txtWebsite"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="form-section-row">
                                                                    <div class="section-ttle">Settings</div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtUserName">Username</label>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUserName" Display="None" ErrorMessage="Username Required" SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender" runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator3">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" MaxLength="15" TabIndex="15"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtPassword">Password</label>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPassword" Display="None" ErrorMessage="Password Required" SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator4_ValidatorCalloutExtender" runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator4">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" MaxLength="10" TabIndex="16"></asp:TextBox>
                                                                            </div>
                                                                        </div>


                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label for="ddlCustStatus" class="drpdwn-label">Status</label>
                                                                                <asp:DropDownList runat="server" CssClass="browser-default validate" TabIndex="13"
                                                                                    ID="ddlCustStatus">
                                                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
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
                                                                                <label for="ddlUserType" class="drpdwn-label">Type</label>
                                                                                <asp:DropDownList runat="server" CssClass="browser-default validate" TabIndex="12"
                                                                                    ID="ddlUserType">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5 mgntp10">
                                                                            <div class="row checkrow">
                                                                                <asp:CheckBox ID="chkGrpWO" CssClass="filled-in" runat="server" TabIndex="18" />
                                                                                <asp:Label ID="Label1" runat="server" Text="Group By Work Order"></asp:Label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s7 mgntp10">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s6 mgntp10">
                                                                            <div class="row checkrow">
                                                                                <asp:CheckBox ID="chkMap" CssClass="filled-in" runat="server" TabIndex="17" />
                                                                                <asp:Label for="chkMap" runat="server" Text="View Service History"></asp:Label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s6 mgntp10">
                                                                            <div class="row checkrow">
                                                                                <asp:CheckBox ID="chkOpenTicket" CssClass="filled-in" runat="server" TabIndex="18" />
                                                                                <asp:Label for="chkOpenTicket" runat="server" Text="View All Open Tickets"></asp:Label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s6 mgntp10">
                                                                            <div class="row checkrow">
                                                                                <asp:CheckBox ID="chkScheduleBrd" CssClass="filled-in" runat="server" TabIndex="18" />
                                                                                <asp:Label for="chkScheduleBrd" runat="server" Text="View Invoices"></asp:Label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s6 mgntp10">
                                                                            <div class="row checkrow">
                                                                                <asp:CheckBox ID="chkEquipments" CssClass="filled-in" runat="server" TabIndex="18" />
                                                                                <asp:Label for="chkEquipments" runat="server" Text="View Equipment"></asp:Label>
                                                                            </div>
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
                                        </li>

                                        <li>
                                            <div id="accrdLocation" class="collapsible-header accrd accordian-text-custom"><i class="mdi-social-people"></i>Locations</div>
                                            <div class="collapsible-body">
                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">
                                                        <div class="btncontainer">
                                                            <div class="btnlinks">
                                                                <asp:LinkButton ID="lnkAddEditGroup" CausesValidation="False" runat="server" ToolTip="Add/Edit Group">Add/Edit Group</asp:LinkButton>
                                                            </div>
                                                        </div>

                                                        <div class="grid_container">
                                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                                <div class="RadGrid RadGrid_Material">
                                                                    <telerik:RadCodeBlock ID="RadCodeBlock_Location" runat="server">
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

                                                                                } catch (e) {
                                                                                    var element = document.getElementById(requestInitiator);
                                                                                    if (element && element.tagName == "INPUT") {
                                                                                        element.focus();
                                                                                        element.selectionStart = selectionStart;
                                                                                    }
                                                                                }
                                                                            }
                                                                        </script>
                                                                    </telerik:RadCodeBlock>
                                                                    <asp:UpdatePanel runat="server" ID="uplLocGroups" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Location" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True" OnPreRender="RadGrid_Location_PreRender" OnNeedDataSource="RadGrid_Location_NeedDataSource">
                                                                                <CommandItemStyle />
                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                </ClientSettings>
                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="False" ShowFooter="True" AllowNaturalSort="false" DataKeyNames="loc">
                                                                                    <Columns>
                                                                                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblloc" runat="server" Text='<%# Eval("loc") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridBoundColumn DataField="locid" SortExpression="locid" HeaderText="Acct #" UniqueName="AcctNo"
                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                                                                            ShowFilterIcon="false">
                                                                                        </telerik:GridBoundColumn>

                                                                                        <telerik:GridTemplateColumn DataField="tag" SortExpression="tag" AutoPostBackOnFilter="true" HeaderStyle-Width="140"
                                                                                            CurrentFilterFunction="Contains" UniqueName="Location" HeaderText="Location Name" ShowFilterIcon="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:HyperLink ID="lnkName" runat="server" Text='<%#Eval("tag")%>'></asp:HyperLink>
                                                                                                <asp:HiddenField ID="hdnLoc" runat="server" Value='<%#Eval("loc")%>' />
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridBoundColumn DataField="Address" SortExpression="Address" UniqueName="Address" HeaderText="Address" HeaderStyle-Width="190"
                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                                            ShowFilterIcon="false">
                                                                                        </telerik:GridBoundColumn>

                                                                                        <telerik:GridBoundColumn DataField="City" SortExpression="City" UniqueName="City" HeaderText="City" HeaderStyle-Width="80"
                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                                            ShowFilterIcon="false">
                                                                                        </telerik:GridBoundColumn>

                                                                                        <telerik:GridBoundColumn DataField="Type" SortExpression="Type" UniqueName="Type" HeaderText="Type" HeaderStyle-Width="80" CurrentFilterFunction="Contains"
                                                                                            ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                                                                        </telerik:GridBoundColumn>

                                                                                        <telerik:GridTemplateColumn DataField="Status" UniqueName="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                                                                            CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="70">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblstatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn DataField="RoleID" UniqueName="RoleID" SortExpression="RoleID" AutoPostBackOnFilter="true"
                                                                                            CurrentFilterFunction="Contains" HeaderText="Group" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddlGroup" CssClass="browser-default validate" runat="server" DataTextField="Role" DataValueField="ID"
                                                                                                    DataSource='<%#dtGroupData%>' SelectedValue='<%# Eval("RoleID") %>'
                                                                                                    Font-Size="1em" Style="float: left">
                                                                                                </asp:DropDownList>
                                                                                                <%--<asp:Panel ID="Panel3" runat="server" CssClass="btn-addgroup">
                                                                                                    <asp:LinkButton ID="" runat="server" OnClientClick="showAddGroup();return false;" CausesValidation="false">Add/Edit Group</asp:LinkButton>
                                                                                                </asp:Panel>
                                                                                                <asp:HoverMenuExtender ID="hmeRes" runat="server" OffsetX="0" OffsetY="30" PopupControlID="Panel3"
                                                                                                    TargetControlID="ddlGroup">
                                                                                                </asp:HoverMenuExtender>--%>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                    </Columns>
                                                                                </MasterTableView>
                                                                            </telerik:RadGrid>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="cf"></div>
                                                    </div>
                                                    <div style="clear: both;"></div>
                                                </div>
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

    <telerik:RadWindow ID="RadAddGroupWindow" Skin="Material" runat="server" Modal="true" Width="800" Height="600" Title="Location Groups">
        <ContentTemplate>
            <div class="com-cont">
                <div class="clearfix"></div>
                <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                    CausesValidation="False" />
                <div class="custuser-popup-body" style="padding: 8px; padding-right: 10px;">
                    <div class="table-scrollable" style="border: none;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                    Width="100%" ID="gvGroup" OnRowEditing="gvGroup_RowEditing" OnRowCancelingEdit="gvGroup_RowCancelingEdit"
                                    OnRowDeleting="gvGroup_RowDeleting" OnRowUpdating="gvGroup_RowUpdating" ShowFooter="true">
                                    <AlternatingRowStyle CssClass="oddrowcolor"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" ItemStyle-CssClass="row-content">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Role") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtName" Text='<%# Eval("Role") %>' runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtName"
                                                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNameF" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNameF"
                                                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="add"></asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Username" ItemStyle-CssClass="row-content">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUser" runat="server"><%#Eval("Username")%></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtUser" CssClass="form-control" Text='<%# Eval("Username") %>' runat="server"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtUser_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                    TargetControlID="txtUser">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtUser"
                                                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="txtUser_Regexval" runat="server" ValidationGroup="edit" SetFocusOnError="True"
                                                    ControlToValidate="txtUser"
                                                    ErrorMessage="Minimum 6 characters"
                                                    ValidationExpression=".{6}.*" Display="None" />
                                                <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="txtUser_Regexval"
                                                    ID="txtUser_Regexval_ValidatorCalloutExtender" PopupPosition="BottomLeft">
                                                </asp:ValidatorCalloutExtender>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtUserF" CssClass="form-control" runat="server" placeholder="Username"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtUserF_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                    TargetControlID="txtUserF">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUserF"
                                                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="txtUserF_Regexval" runat="server" ValidationGroup="add" SetFocusOnError="True"
                                                    ControlToValidate="txtUserF"
                                                    ErrorMessage="Minimum 6 characters"
                                                    ValidationExpression=".{6}.*" Display="None" />
                                                <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="txtUserF_Regexval"
                                                    ID="txtUserF_Regexval_ValidatorCalloutExtender" PopupPosition="BottomLeft">
                                                </asp:ValidatorCalloutExtender>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Password" ItemStyle-CssClass="row-content">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPassword" runat="server"><%# new string('*', Eval("password").ToString().Length)%></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPassword" CssClass="form-control" Text='<%# Eval("password") %>' runat="server"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtPassword_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                    TargetControlID="txtPassword">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPassword"
                                                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="txtPassword_Regexval" runat="server" ValidationGroup="edit" SetFocusOnError="True"
                                                    ControlToValidate="txtPassword"
                                                    ErrorMessage="Minimum 6 characters"
                                                    ValidationExpression=".{6}.*" Display="None" />
                                                <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="txtPassword_Regexval"
                                                    ID="txtPassword_Regexval_ValidatorCalloutExtender" PopupPosition="BottomLeft">
                                                </asp:ValidatorCalloutExtender>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtPasswordF" CssClass="form-control" runat="server" placeholder="Password"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtPasswordF_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                    TargetControlID="txtPasswordF">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPasswordF"
                                                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="txtPasswordF_Regexval" runat="server" SetFocusOnError="True"
                                                    ControlToValidate="txtPasswordF"
                                                    ErrorMessage="Minimum 6 characters"
                                                    ValidationExpression=".{6}.*" Display="None" ValidationGroup="add" />
                                                <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="txtPasswordF_Regexval"
                                                    ID="txtPasswordF_Regexval_ValidatorCalloutExtender" PopupPosition="BottomLeft">
                                                </asp:ValidatorCalloutExtender>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="row-content" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" CssClass="btn location-search" runat="server" ToolTip="Edit group"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn location-search" OnClientClick="return confirm('Are you sure you want to delete this group');" ToolTip="Delete group"><i class="fa fa-trash-o"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" CssClass="btn location-search" ValidationGroup="edit" ToolTip="Save"><i class="fa fa-save"></i></asp:LinkButton>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn location-search" CommandName="Cancel" ToolTip="Cancel"><i class="fa fa-times"></i></asp:LinkButton>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:LinkButton ID="lnkAdd" runat="server" OnClick="lnkAdd_Click" CssClass="btn location-search" ValidationGroup="add" ToolTip="Add new group"><i class="fa fa-plus"></i></asp:LinkButton>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="evenrowcolor"></RowStyle>
                                    <FooterStyle CssClass="footer" />
                                    <SelectedRowStyle CssClass="selectedrowcolor"></SelectedRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>

    <asp:HiddenField runat="server" ID="hdnSelectLoc" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
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

            $("[id*=lnkAddEditGroup]").click(function () {
                var lnkAddGrp = $(this).attr('id');

                var oWnd = $find("<%=RadAddGroupWindow.ClientID%>");
                oWnd.show();
                return false;
            });
        });

        function pageLoad(sender, args) {
            Materialize.updateTextFields();
        }
    </script>

</asp:Content>
