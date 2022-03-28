<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddCOA" Codebehind="AddCOA.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    
     <!--File Upload Control-->
   <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
   <script src="js/chosen.jquery.js" type="text/javascript"></script>
   <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>

    <style type="text/css">
        .auto-style1 {
            float: left;
            min-width: 116px;
            text-align: right;
            padding-top: 5px;
            color: #626262;
            display: block;
            font: normal 13px/18px Helvetica;
            margin-right: 10px;
            height: 42px;
        }

        .auto-style2 {
            height: 42px;
        }

        .popupConfirmation {
            width: 500px;
            height: 133px;
        }

        .popup_Container {
            background-color: #ffffff;
            /*border: 2px solid #000000;*/
            padding: 0px 0px 0px 0px;
        }

        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        .divInnerBody {
            padding-top: 10px;
            padding-left: 10px;
            padding-right: 10px;
            padding-bottom: 10px;
        }

        .parent {
            width: 100%;
        }

            .parent .sub1 {
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
                padding-left: 10px;
                /*background-color: red;*/
            }

            .parent .sub2 {
                /*background-color: blue;*/
                float: left;
                width: 200px;
            }

            .parent .sub3 {
                padding-left: 100px;
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
            }

            .parent .sub33 {
                padding-left: 10px;
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
            }

            .parent .sub4 {
                float: left;
                width: 190px;
            }

        /*.pnlStyle {
            width: 400px;
            float: left;
            height: 268px;
        }*/

        .pnlAccount {
            height: 268px;
            float: left;
        }

        .pnlBankAccount {
            width: 920px;
            height: 268px;
            float: right;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#ctl00_ContentPlaceHolder1_ddlType option:selected").text() != 'Bank') {
                //alert("hide bank panel")
                $("#pnlBankAccount").hide();
                <%--$("#<%=pnlBankAccount2.ClientID %>").hide();--%>
                //$("#pnlBankInfo").hide();
                $("#<%=liGeneral.ClientID%>").hide();
                $("#<%=adGeneral.ClientID%>").hide();
            }
            $('#<%=txtDescription.ClientID%>').focus(function () {
                $(this).animate({
                    //right: "+=0",
                    width: '520px',
                    height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtDescription.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%',
                    height: '46px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            if (typeof Materialize !== 'undefined' && typeof Materialize.updateTextFields === 'function') {
                Materialize.updateTextFields();
            }
        });

        function reSizeTextbox() {
            $('#<%=txtAddress.ClientID%>').focus(function () {
                $(this).animate({
                    //right: "+=0",
                    width: '520px',
                    height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtAddress.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%',
                    height: '46px'
                }, 500, function () {
                    // Animation complete.
                });
            });
        }

        function ResizeControls(IsBank) {
            //debugger;
            if (IsBank == 'true') {
                //$("#pnlAccount").addClass("pnlStyle");
               <%-- $("#<%=ddlType.ClientID %>").width("200px");
                $("#<%=ddlSubAcCategory.ClientID %>").width("226px");
                $("#<%=txtAcctNum.ClientID %>").width("200px");
                $("#<%=txtAcName.ClientID %>").width("200px");
                $("#<%=txtDescription.ClientID %>").width("200px");
                $("#<%=txtBal.ClientID %>").width("200px");
                $("#<%=ddlStatus.ClientID %>").width("200px");--%>
                $("#pnlBankAccount").show();
                //$("#pnlBankInfo").show();
                $("#<%=liGeneral.ClientID%>").show();
                $("#<%=adGeneral.ClientID%>").show();
            }
            else {
                //$("#pnlAccount").removeClass("pnlStyle");
                <%--  $("#<%=ddlType.ClientID %>").width("200px");
                $("#<%=ddlSubAcCategory.ClientID %>").width("226px");
                $("#<%=txtAcctNum.ClientID %>").width("200px");
                $("#<%=txtAcName.ClientID %>").width("540px");
                $("#<%=txtDescription.ClientID %>").width("540px");
                $("#<%=txtBal.ClientID %>").width("540px");
                $("#<%=ddlStatus.ClientID %>").width("540px");--%>
                //$("#pnlBankAccount").hide();
                //$("#pnlBankInfo").hide();
            }
        }
    </script>
    <script type="text/javascript">
        function AddedCoa() {
            noty({
                text: 'Chart of Account Added Successfully!',
                type: 'success',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function cancel() {
            window.parent.document.getElementById('ctl00_ContentPlaceHolder1_hideModalPopupViaServer').click();
        }
        function ReloadPage() {
            return false;
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
            $("#<%= txtSubAcct.ClientID %>").val('');
        }

        function AddNewSubAccount(subAcct) {
            //console.log(subAcct);
            //alert(subAcct);
        }
        function SetSubCategoryData() {
            $("#<%= lblAcctName.ClientID %>").text($("#<%= txtAcName.ClientID %>").val());
            $("#<%= lblAcctNum.ClientID %>").text($("#<%= txtAcctNum.ClientID %>").val());
        }
        <%-- function updateName() {
            debugger;
            var getName = document.getElementById("<%= txtAcName.ClientID %>").value;
            document.getElementById("<%= lblAcctName.ClientID %>").Text = getName;
            var abc = document.getElementById("<%= lblAcctName.ClientID %>").Text;
            alert(abc);
        }
        function updateNumber() {
            var getNumber = document.getElementById("<%= txtAcctNum.ClientID %>").value;
            document.getElementById("<%= lblAcctNum.ClientID %>").Text = getNumber;
            var abc = document.getElementById("<%= lblAcctNum.ClientID %>").Text;
            alert(abc);
        }--%>
    </script>
    <script type="text/javascript">

        function isDecimalKey(evt, txt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                return false;
            else {
                var len = document.getElementById(txt.id).value.length;
                var index = document.getElementById(txt.id).value.indexOf('.');

                if (index > 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    var CharAfterdot = (len + 1) - index;
                    if (CharAfterdot > 3) {
                        return false;
                    }
                }

            }
            return true;
        }

        function isNumberKey(evt, txt) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function updateTextField() {
            if (typeof Materialize !== 'undefined' && typeof Materialize.updateTextFields === 'function') {
                Materialize.updateTextFields();
            }
        }
    </script>

    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>

    <script type="text/javascript">

</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_COA" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnAddCentral">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowCentral" LoadingPanelID="RadAjaxLoadingPanel_COA" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlSubAcCategory">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowSubCategory" LoadingPanelID="RadAjaxLoadingPanel_COA" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnCompanyPopUp">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowCompany" LoadingPanelID="RadAjaxLoadingPanel_COA" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_COA" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-action-trending-up"></i>&nbsp;
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Edit New Account</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label runat="server" ID="lblAccountName"></asp:Label>
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
                                    
                                    <li><a href="#accrdAccount" class="link-slide">Account Info</a></li>
                                    <li runat="server" id="liGeneral" style="display: none"><a href="#accrdgeneral" class="link-slide">Bank Info</a></li>
                                    <li id="liDocuments"><a href="#accrdDocuments">Documents</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlNext" runat="server" Visible="False">
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False" OnClick="lnkFirst_Click"><i class="fa fa-angle-double-left"></i></asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False" OnClick="lnkPrevious_Click"><i class="fa fa-angle-left"></i></asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False" OnClick="lnkNext_Click"><i class="fa fa-angle-right"></i></asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False" OnClick="lnkLast_Click"><i class="fa fa-angle-double-right"></i></asp:LinkButton>
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
                        <li class="active">
                            <div id="accrdAccount" class="collapsible-header accrd active accordian-text-custom ">
                                <i class="mdi-social-poll"></i>Account Info</div>
                            

                            <div class="collapsible-body" style="display: block;">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">

                                        <div class="form-section-row">
                                            <div class="section-ttle">Account Details</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Account Type</label>
                                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="rfvAccountNum"
                                                            runat="server" ControlToValidate="txtAcctNum" Display="None" ErrorMessage="Account # is Required"
                                                            SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender
                                                            ID="vceAcctNum" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="rfvAccountNum" />
                                                        <asp:CustomValidator ID="cvAccountNum" runat="server" ErrorMessage="Acct# already exists, please use different Acct# !"
                                                            OnServerValidate="cvAccountNum_ServerValidate" ControlToValidate="txtAcctNum" Display="None" SetFocusOnError="true">
                                                        </asp:CustomValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceAcctNum1" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="cvAccountNum" />
                                                        <asp:TextBox ID="txtAcctNum" runat="server" MaxLength="15" autocomplete="off"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtAcctNum" AssociatedControlID="txtAcctNum" for="txtAcctNum">Account Number</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="rfvAcName"
                                                            runat="server" ControlToValidate="txtAcName" Display="None" ErrorMessage="Account Name is Required"
                                                            SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator>
                                                          <asp:CustomValidator ID="cvAcName" runat="server" ErrorMessage="Account Name already exists, please use different Account Name !"
                                                            OnServerValidate="cvAcName_ServerValidate" ControlToValidate="txtAcName" Display="None" SetFocusOnError="true">
                                                        </asp:CustomValidator>
                                                        <asp:ValidatorCalloutExtender
                                                            ID="vceAcName" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="cvAcName" />
                                                        <asp:TextBox ID="txtAcName" runat="server" MaxLength="75" autocomplete="off"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtAcName" AssociatedControlID="txtAcName" for="txtAcName">Account Name</asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s4">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtBal" runat="server" MaxLength="75"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtBal" AssociatedControlID="txtBal" for="txtBal">Balance</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s1">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s4">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Status</label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s1">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>





                                                <div class="input-field col s2">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkNoJE" runat="server" TabIndex="19" />
                                                        <asp:Label runat="server" ID="lblchkNoJE" AssociatedControlID="chkNoJE">No JE</asp:Label>
                                                    </div>
                                                </div>




                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:Label runat="server" ID="lbltxtDescription" AssociatedControlID="txtDescription">Description</asp:Label>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="materialize-textarea" TextMode="MultiLine" MaxLength="75"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s10">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Center</label>
                                                        <asp:DropDownList ID="ddlCentral" runat="server" CssClass="browser-default" TabIndex="5">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        <div class="btnlinksicon">
                                                            <asp:LinkButton runat="server" ID="btnAddCentral" OnClick="btnAddCentral_Click" CausesValidation="false" Visible="True"><i class="mdi-social-person-add"></i></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Sub Category</label>
                                                        <asp:UpdatePanel ID="updPnlSubAcct" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlSubAcCategory" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlSubAcCategory_SelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Company</label>
                                                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                                        <asp:LinkButton runat="server" ID="btnCompanyPopUp" OnClick="btnCompanyPopUp_Click">Change Company</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>

                        <li runat="server" id="adGeneral" style="display: none">
                            <div id="accrdgeneral" class="collapsible-header accrd active accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Bank Info</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <asp:UpdatePanel ID="uPnlChart" runat="server">
                                            <ContentTemplate>
                                                <div class="form-section-row" id="pnlBankAccount" runat="server">
                                                    <div class="section-ttle">Bank Details</div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox runat="server" ID="txtBankName"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtBankName" AssociatedControlID="txtBankName">Bank Name</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" ID="lbltxtAddress" AssociatedControlID="txtAddress">Address</asp:Label>
                                                                <asp:TextBox ID="txtAddress" placeholder="" CssClass="materialize-textarea" runat="server" MaxLength="15" TextMode="MultiLine"></asp:TextBox>
                                                            </div>
                                                        </div>


                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="250"></asp:TextBox>
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
                                                                <asp:TextBox ID="ddlState" runat="server" MaxLength="250"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtState" AssociatedControlID="ddlState">State</asp:Label>
                                                                <%--<asp:DropDownList ID="ddlState" runat="server" CssClass="browser-default">
                                                                </asp:DropDownList>
                                                                <label class="drpdwn-label">State</label>--%>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtZip" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtZip" AssociatedControlID="txtZip">Zip</asp:Label>
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
                                                                <%--  <asp:TextBox ID="txtCountry" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtCountry" AssociatedControlID="txtCountry">Country</asp:Label>--%>
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
                                                                <asp:TextBox runat="server" ID="lat"></asp:TextBox>
                                                                <%--  <input id="lat" type="text" runat="server" />--%>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            &nbsp;
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="lng">Longitude <span class="reqd">*</span></asp:Label>
                                                                <asp:TextBox runat="server" ID="lng"></asp:TextBox>
                                                                <%--    <input id="lng" type="text" runat="server" />--%>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <div id="map" class="map-css2" style="overflow: hidden !important; height: 170px!important;">
                                                                </div>
                                                                <%--   <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d13716.88953039753!2d76.77389278096229!3d30.740254306150458!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x390fed0afe5003d3%3A0x8f47abe9f2044934!2sSector+17%2C+Chandigarh!5e0!3m2!1sen!2sin!4v1506502302516" frameborder="0" style="border: 0; width: 100%" allowfullscreen></iframe>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section-row" runat="server" id="pnlBankAccount2">
                                                    <div class="section-ttle">Main Contact Details</div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtContact" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtContact" AssociatedControlID="txtContact"> Contact Name</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:MaskedEditExtender ID="txtPhoneCust_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                                    MaskType="Number" TargetControlID="txtPhone">
                                                                </asp:MaskedEditExtender>
                                                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtPhone" AssociatedControlID="txtPhone">Phone</asp:Label>
                                                            </div>
                                                        </div>


                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:MaskedEditExtender ID="txtFax_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                                    MaskType="Number" TargetControlID="txtFax">
                                                                </asp:MaskedEditExtender>
                                                                <asp:TextBox ID="txtFax" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtFax" AssociatedControlID="txtFax">Fax</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:MaskedEditExtender ID="txtCellular_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                                    MaskType="Number" TargetControlID="txtCellular">
                                                                </asp:MaskedEditExtender>
                                                                <asp:TextBox ID="txtCellular" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtCellular" AssociatedControlID="txtCellular">Cellular</asp:Label>
                                                            </div>
                                                        </div>


                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                                    ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                <asp:ValidatorCalloutExtender ID="vceEmail" runat="server" Enabled="True"
                                                                    TargetControlID="revEmail">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="150"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtEmail" AssociatedControlID="txtEmail">Email</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtWebsite" runat="server" MaxLength="1000"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtWebsite" AssociatedControlID="txtWebsite"> Web address</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlType" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <div class="form-section-row">
                                            <div class="section-ttle">Bank Account Details</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtBranch" runat="server" MaxLength="15"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtBranch" AssociatedControlID="txtBranch">Branch Number</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtAcct" runat="server" MaxLength="15" autocomplete="off"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtAcct" AssociatedControlID="txtAcct">Account Number</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtRoute" runat="server" MaxLength="9"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtRoute" AssociatedControlID="txtRoute"> Route Number</asp:Label>
                                                    </div>
                                                </div>
                                                 <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtACHFileHeaderStringA" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label1" AssociatedControlID="txtACHFileHeaderStringA"> ACH FileHeaderString-A</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtACHBatchControlString1" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label6" AssociatedControlID="txtACHBatchControlString1"> ACH BatchControlString-1</asp:Label>
                                                    </div>
                                                </div>
                                                 <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtACHCompanyHeaderString1" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label7" AssociatedControlID="txtACHCompanyHeaderString1"> ACH CompanyHeaderString-1</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtAPACHCompanyID" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label12" AssociatedControlID="txtAPACHCompanyID"> ACH CompanyID</asp:Label>
                                                    </div>
                                                </div>


                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTraceNo1" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label13" AssociatedControlID="txtTraceNo1"> Trace No Emp</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtRecordTypeCode1" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label20" AssociatedControlID="txtRecordTypeCode1"> Record Type Code Emp</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTransactionCode1" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label21" AssociatedControlID="txtTransactionCode1"> Transaction Code Emp</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtEndRecordIndicator1" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label22" AssociatedControlID="txtEndRecordIndicator1"> Add end Record Indicator Emp</asp:Label>
                                                    </div>
                                                </div>
                                                
                                                <%--<div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCreditLimit" runat="server" MaxLength="13" onkeypress="return isDecimalKey(event,this)"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtCreditLimit" AssociatedControlID="txtCreditLimit">Credit Limit</asp:Label>
                                                    </div>
                                                </div>--%>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkWarn" runat="server" TabIndex="18" />
                                                        <asp:Label runat="server" ID="lblchkWarn" AssociatedControlID="chkWarn">Warn on Overdraft</asp:Label>
                                                    </div>
                                                </div>
                                                
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtNCheck" runat="server" MaxLength="9" onkeypress="return isNumberKey(event,this)" autocomplete="off"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtNCheck" AssociatedControlID="txtNCheck">Start Check Number</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtNDeposit" runat="server" MaxLength="9" onkeypress="return isNumberKey(event,this)" autocomplete="off"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtNDeposit" AssociatedControlID="txtNDeposit">Start Deposit Number</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtNEPay" runat="server" MaxLength="9" onkeypress="return isNumberKey(event,this)" autocomplete="off"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtNEPay" AssociatedControlID="txtNEPay">Start EPay Number</asp:Label>
                                                    </div>
                                                </div>
                                                 <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtACHFileHeaderStringB" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label2" AssociatedControlID="txtACHFileHeaderStringB"> ACH FileHeaderString-B</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtACHBatchControlString2" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label5" AssociatedControlID="txtACHBatchControlString2"> ACH BatchControlString-2</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtACHCompanyHeaderString2" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label8" AssociatedControlID="txtACHCompanyHeaderString2"> ACH CompanyHeaderString-2</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtAPImmediateOrigin" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label11" AssociatedControlID="txtAPImmediateOrigin"> ImmediateOrigin</asp:Label>
                                                    </div>
                                                </div>

                                                 <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTraceNo2" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label14" AssociatedControlID="txtTraceNo2"> Trace No Bank</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtRecordTypeCode2" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label18" AssociatedControlID="txtRecordTypeCode2"> Record Type Code Bank</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTransactionCode2" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label19" AssociatedControlID="txtTransactionCode2"> Transaction Code Bank</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtEndRecordIndicator2" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label23" AssociatedControlID="txtEndRecordIndicator2"> Add end Record Indicator Bank</asp:Label>
                                                    </div>
                                                </div>


                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99.99" ClearMaskOnLostFocus="true"
                                                            MaskType="Number" TargetControlID="txtRate" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                            CultureTimePlaceholder="" Enabled="True">
                                                        </asp:MaskedEditExtender>
                                                        <asp:TextBox ID="txtRate" runat="server" MaxLength="5"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtRate" AssociatedControlID="txtRate">Int Rate</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtReconciled" runat="server" MaxLength="15" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtReconciled" AssociatedControlID="txtReconciled">Reconciled</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCreditLimit" runat="server" MaxLength="13" onkeypress="return isDecimalKey(event,this)"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtCreditLimit" AssociatedControlID="txtCreditLimit">Credit Limit</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtACHFileHeaderStringC" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label3" AssociatedControlID="txtACHFileHeaderStringC"> ACH FileHeaderString-C</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtACHBatchControlString3" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label4" AssociatedControlID="txtACHBatchControlString3"> ACH BatchControlString-3</asp:Label>
                                                    </div>
                                                </div>
                                                 <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtACHFileControlString1" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label9" AssociatedControlID="txtACHFileControlString1"> ACH FileControlString</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtNextACH" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label10" AssociatedControlID="txtNextACH"> Next ACH</asp:Label>
                                                    </div>
                                                </div>

                                                <%--<div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCompanyEntryDescription" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label15" AssociatedControlID="txtCompanyEntryDescription"> Company Entry Description</asp:Label>
                                                    </div>
                                                </div>--%>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtOriginatorStatusCode" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label16" AssociatedControlID="txtOriginatorStatusCode"> Originator Status Code</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtRecordTypeCode3" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label17" AssociatedControlID="txtRecordTypeCode3"> Record Type Code</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtBatchNumber" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label24" AssociatedControlID="txtBatchNumber"> Batch Number</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJulianDate" runat="server" MaxLength="225"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label25" AssociatedControlID="txtJulianDate"> Julian Date</asp:Label>
                                                    </div>
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
    <asp:HiddenField ID="hdnRol" runat="server" />
    <asp:HiddenField ID="hdnBank" runat="server" />

    <telerik:RadWindowManager ID="RadWindowManagerCOA" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowCentral" Skin="Material" VisibleTitlebar="true" Title="Add Center" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="280">
                <ContentTemplate>
                    <div>
                        <div class="input-field col s12">
                            <div class="row">
                                <label for="txtCentral">Center</label>
                                <asp:TextBox ID="txtCentral" runat="server" MaxLength="150" ValidationGroup="valCentral"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="valCentral" ID="rfvCentral" runat="server" ControlToValidate="txtCentral"
                                    Display="Dynamic" ErrorMessage="Center is Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vceCentral"
                                    runat="server" Enabled="True" TargetControlID="rfvCentral" PopupPosition="BottomLeft">
                                </asp:ValidatorCalloutExtender>
                            </div>
                        </div>
                        <div class="btnlinks">
                            <asp:LinkButton ID="lbtnCentralSubmit" runat="server" OnClick="lbtnCentralSubmit_Click"
                                CausesValidation="true" ValidationGroup="valCentral">Save</asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowSubCategory" Skin="Material" VisibleTitlebar="true" Title="Add Sub Category" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="280">
                <ContentTemplate>
                    <div>
                        <div class="input-field col s12">
                            <div class="row">
                                <div class="col s6">
                                    Account Type
                                </div>
                                <div class="col s6">

                                    <asp:Label ID="lblAcctType" runat="server"></asp:Label>

                                </div>
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <div class="col s6">
                                    Account Number
                                </div>
                                <div class="col s6">

                                    <asp:Label ID="lblAcctNum" runat="server"></asp:Label>

                                </div>
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <div class="col s6">
                                    Account Name
                                </div>
                                <div class="col s6">

                                    <asp:Label ID="lblAcctName" runat="server"></asp:Label>

                                </div>
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rfvSubAcctName"
                                    runat="server" ControlToValidate="txtSubAcct" Display="None" ErrorMessage="Sub Category Name is required"
                                    SetFocusOnError="True" ValidationGroup="valSubAccount">
                                </asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender
                                    ID="vceSubAcctName" runat="server" Enabled="True"
                                    PopupPosition="BottomLeft" TargetControlID="rfvSubAcctName" />
                                <asp:TextBox ID="txtSubAcct" runat="server" MaxLength="150"></asp:TextBox>
                                <asp:Label runat="server" ID="lbltxtSubAcct" AssociatedControlID="txtSubAcct">Sub Category Name</asp:Label>
                            </div>
                        </div>

                        <div class="btnlinks">
                            <asp:LinkButton ID="lbtnSubAcctSubmit" runat="server" OnClick="lbtnSubAcctSubmit_Click" CausesValidation="true" ValidationGroup="valSubAccount">Save</asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowCompany" Skin="Material" VisibleTitlebar="true" Title="Select Company" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="220">
                <ContentTemplate>
                    <div>
                        <div class="input-field col s12">
                            <div class="row">
                                <label class="drpdwn-label">Select Company</label>
                                <asp:DropDownList ID="ddlCompanyEdit" runat="server" CssClass="browser-default"></asp:DropDownList>
                            </div>
                        </div>

                        <div style="clear: both;"></div>
                        <div class="btnlinks">
                            <asp:Button ID="btnCompanyEdit" runat="server" Text="Save" OnClick="btnCompanyEdit_Click" />

                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
        <div class="container accordian-wrap">
        <div class="col s12 m12 l12">
            <div class="row">
                <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                    <li id="tbDocuments" runat="server">
                        <div id="accrdDocuments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-file-attachment"></i>Documents</div>
                        <div class="collapsible-body">
                            <asp:Panel ID="pnlDocPermission" runat="server">
                                <div class="form-section-row">
                                    <div class="col s12 m12 l12">
                                        <div class="row">
                                            <asp:FileUpload ID="FileUpload1" runat="server" class="dropify" AllowMultiple="true" onchange="AddDocumentClick(this);" />
                                        </div>
                                    </div>
                                </div>
                                <div class="btncontainer">
                                    <asp:Panel ID="pnlDocumentButtons" runat="server">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                            <asp:LinkButton ID="lnkUploadDoc" runat="server" CausesValidation="False" OnClick="lnkUploadDoc_Click" Style="display: none">Upload</asp:LinkButton>
                                            <asp:LinkButton ID="lnkPostback" runat="server" CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                    <div style="clear: both;"></div>
                                </div>
                                <div class="form-section-row">
                                    <div class="grid_container">
                                        <div class="RadGrid RadGrid_Material FormGrid">
                                            <telerik:RadCodeBlock ID="RadCodeBlock_Documents" runat="server">
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

                                                    function requestStart2(sender, args) {
                                                        requestInitiator = document.activeElement.id;
                                                        if (document.activeElement.tagName == "INPUT") {
                                                            selectionStart = document.activeElement.selectionStart;
                                                        }


                                                    }

                                                    function responseEnd2(sender, args) {
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

                                            <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" PostBackControls="lblName" runat="server" ClientEvents-OnRequestStart="requestStart2" ClientEvents-OnResponseEnd="responseEnd2">
                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                    PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_Documents_PreRender" OnNeedDataSource="RadGrid_Documents_NeedDataSource"
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

                                                            <telerik:GridTemplateColumn AllowFiltering="false" Visible="false" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdnTempId" Value='<%# Eval("id").ToString() == "0"? Eval("TempId"): string.Empty %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="filename" SortExpression="filename" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                HeaderText="File Name" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                        CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                        OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                                    </asp:LinkButton>

                                                                </ItemTemplate>

                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridBoundColumn FilterDelay="5" DataField="doctype" HeaderText="File Type" HeaderStyle-Width="140"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="doctype"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>

                                                            <%--<telerik:GridTemplateColumn SortExpression="portal" HeaderText="Portal" DataField="portal" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>--%>

                                                            <%--<telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>--%>

                                                            <telerik:GridTemplateColumn SortExpression="remarks" HeaderText="Remarks" DataField="remarks" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtremarks" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </telerik:RadAjaxPanel>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div style="clear: both;"></div>
                        </div>
                    </li>
                    <li id="tbLogs" runat="server" style="display: none">
                        <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
                        <div class="collapsible-body">
                            <div class="tab-container-content">
                                <div class="form-content-pd">
                                    <div class="grid_container">
                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                            <div class="RadGrid RadGrid_Material">
                                                <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoadLog() {
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
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />
    <asp:HiddenField ID="hdnIsAutoCompleteSelected" ClientIDMode="Static" runat="server" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
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

        });
    </script>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {

            $("#<%= txtAddress.ClientID %>").geocomplete({
                map: false,
                details: "#divmain",
                types: ["geocode", "establishment"],
                address: "#<%= txtAddress.ClientID %>",
                city: "#<%= txtCity.ClientID %>",
                state: "#<%= ddlState.ClientID %>",
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
                    //if (cityAlt.length > 2)
                    //    for (var i = 0; i < result.address_components.length; i++) {
                    //        var addr = result.address_components[i];
                    //        if (addr.types[0] == 'administrative_area_level_2')
                    //            cityAlt = addr.short_name;
                    //    }

                    $("#<%=ddlCountry.ClientID%>").val(getCountry);
                    $("#<%=ddlState.ClientID%>").val(cityAlt);
                $("#<%=txtZip.ClientID%>").val(countryCode);
                $("#<%=txtCity.ClientID%>").val(city);

                Materialize.updateTextFields();
              /*  initialize();*/
            });
        });
  <%--      function initialize() {
            var address = new google.maps.LatLng(document.getElementById('<%= lat.ClientID %>').value, document.getElementById('<%= lng.ClientID %>').value);
            var marker;
            var map;
            var mapOptions = {
                zoom: 13,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                center: address,

            };--%>

            map = new google.maps.Map(document.getElementById('map'),
                mapOptions);

            marker = new google.maps.Marker({
                map: map,
                draggable: false,
                position: address
            });
        }

        $(document).ready(function () {
            initialize();
        });
    </script>

      <script type="text/javascript">
          ///-Document permission

          function AddDocumentClick(hyperlink) {

              var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
              if (IsAdd == "Y") {
                  debugger;
                  ConfirmUpload(ctl00_ContentPlaceHolder1_FileUpload1.value);
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


          function checkdelete() {
              return SelectedRowDelete('<%= RadGrid_Documents.ClientID %>', 'file');
          }

          function SelectedRowDelete(gridview, message) {
              var grid = $find(gridview);
              var MasterTable = grid.get_masterTableView();
              var Rows = null;
              if (MasterTable != null) {
                  Rows = MasterTable.get_dataItems();
              }
              if (Rows != null && Rows.length > 0) {
                  for (i = 0; i < Rows.length; i++) {
                      if (Rows[i].get_selected()) {
                          //return confirm('Are you sure you want to delete ' + message + ' "' + Rows[i].get_columns(1) + '" ?');
                          return true;
                      }
                  }
              }
              alert('Please select ' + message + '.');
              return false;
          }

        ////////////////// Confirm Document Upload ////////////////////
        <%--function ConfirmUpload(value) {
            //debugger
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

              Materialize.updateTextFields();
          });
      </script>
     <script>
         $(document).ready(function () {
             $('form').attr('autocomplete', 'off');
         });
     </script>
</asp:Content>


