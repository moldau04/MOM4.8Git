<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="AddTicketFromCustPortal" ValidateRequest="false" Codebehind="AddTicketFromCustPortal.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.maskedinput/1.4.1/jquery.maskedinput.min.js"></script>
    <script type="text/javascript" src="js/jquery.ns-autogrow.js"></script>
    <script src="Appearance/js/bootstrap-filestyle.js"></script>
    <script type="text/javascript" src="Scripts/Timepicker/jquery.timepicker.js"></script>
    <link rel="stylesheet" href="Scripts/Timepicker/jquery.timepicker.css" />
    <style type="text/css">
        .HighliteBlue {
            background-color: #316b9d !important;
        }
    </style>
    <script type="text/javascript">
        ///Multiple ticket        
        function focusParent() {
            window.close();
        }
        function RefressTicketListContact() {
            if (window.opener && !window.opener.closed) {
                if (window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnSearch'))
                    window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnSearch').click();
            }
        }
    </script>

    <script type="text/javascript">
        /////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////        Page load      ///////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////
        // $(document).ready(function () {
        function pageLoad() {
            // multiple ticket             

            $("#<%=txtUnit.ClientID%>").click(function () {
                $("#divEquip").slideToggle();
                return false;
            });
            $("#<%= txtCell.ClientID %>").mask("(999) 999-9999");
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
                this.custID = null;
            }
            ///////////// Ajax call for location auto search ////////////////////
            var queryloc = "";
            $("#<%=txtLocation.ClientID%>").autocomplete(
                {
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        if (document.getElementById('<%=hdnPatientId.ClientID%>').value != '') {
                            dtaaa.custID = document.getElementById('<%=hdnPatientId.ClientID%>').value;
                        }
                        queryloc = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetLocation",
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
                        $("#<%=txtLocation.ClientID%>").val(ui.item.label); 
                        $("#<%=hdnLocId.ClientID%>").val(ui.item.value);
                        $("#<%=hdndesc.ClientID%>").val(ui.item.desc);
                        
                        document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                        return false;
                    },
                    focus: function (event, ui) {
                        //  $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
            .data("autocomplete")._renderItem = function (ul, item) {
                var result_item = item.label;
                var result_desc = item.desc;
                var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...            
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



            ///////////////////////Ajax call for equipment search//////////////////////////////
                function dataEquip() {
                    this.prefixText = null;
                }
                function dataEmpty() {
                    this.d = "";
                }
            ///////////// Validations for auto search ////////////////////
                $("#<%=txtLocation.ClientID%>").keyup(function (event) {
                    var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
                    if (document.getElementById('<%=txtLocation.ClientID%>').value == '') {
                        hdnLocId.value = '';
                         $("#<%=hdndesc.ClientID%>").val('');
                    }
                });
                $('input.timepicker').timepicker({
                    dropdown: false
                });
                $('input.timepicker1').timepicker({
                    dropdown: false
                });
                $('input.timepicker1').on('click', function () {
                    if ($('input.timepicker1').val() == "") {
                        $('input.timepicker1').timepicker('setTime', new Date());
                        $(this).select();
                    }
                    else { $(this).select(); }
                });
                $('input.timepicker1').on('focus', function () {

                    $(this).select();
                });
                $('input.timepicker').on('focus', function () {

                    $(this).select();
                });
            }
            ///////////// Custom validator function for location auto search  ////////////////////
            function ChkLocation(sender, args) {
                var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }
    </script>
    <style type="text/css">
        .ui-autocomplete {
            max-width: 800px;
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */ /*padding-right: 20px;*/
        }
        /* IE 6 doesn't support max-height
	     * we use height instead, but this forces the menu to always be this tall
	     */ * html .ui-autocomplete {
            height: 300px;
        }

        .highlight {
            background-color: Yellow;
        }

        .highlighted {
            background-color: Yellow;
        }

        .menu_popup_chklst {
            position: absolute;
            /*top: 251px;
            right: 57px;*/
            z-index: 1;
            display: none;
            background: transparent;
            overflow: auto; /*border:solid 1px black;  	width:550px; */
            max-height: 260px;
            min-height: 10px;
            overflow-x: hidden;
        }

        .shadow {
            /* rgba(0, 0, 0, 0.3) rgb(90, 168, 208)*/
            -moz-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
        }
    </style>
    <style>
        .ui-timepicker-wrapper {
            display: none;
        }

        .ui-timepicker-wrapperP {
            display: none;
        }

        .ui-timepicker-list {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="page-cont-top">
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <asp:Panel runat="server" ID="pnlGridButtons">
                        <ul class="lnklist-header">
                            <li>
                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Service Request </asp:Label></li>

                            <li>
                                <asp:LinkButton CssClass="icon-save" ID="lnkSave" ToolTip="Save Ticket" runat="server" CausesValidation="true" OnClick="lnkSave_Click"></asp:LinkButton></li>

                            <li>
                                <a class="icon-closed" id="lnkCancelContact" title="Close" onclick="focusParent()"></a>
                            </li>
                        </ul>
                    </asp:Panel>
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <input id="hdnLocId" runat="server" type="hidden" />
                            <input id="hdnPatientId" runat="server" type="hidden" />
                            <input id="hdnUnitID" runat="server" type="hidden" />
                            <input id="hdndesc" runat="server" type="hidden" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <Triggers>
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-9 col-lg-9">
                                        <div class="row">
                                            <div class="col-md-8 col-lg-8">
                                                <div class="row">
                                                    <div class="col-md-12 col-lg-12">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Customer Name
                                                        
                                                            </div>
                                                            <div class="fc-input">
                                                                <div>
                                                                    <asp:TextBox ID="txtCustomer" runat="server" autocomplete="off" CssClass="form-control searchinput"
                                                                        TabIndex="5"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 col-lg-12">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Location Name 
                                                                <img id="imgCreditH" visible="false" runat="server" title="Credit Hold" src="images/credithold.png" style="width: 16px; background-color: rgba(255, 0, 0, 0.34)"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                    ControlToValidate="txtLocation" Display="None" ErrorMessage="Location Name Required"
                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                        ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                        TargetControlID="RequiredFieldValidator1">
                                                                    </asp:ValidatorCalloutExtender>
                                                                <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ChkLocation"
                                                                    ControlToValidate="txtLocation" Display="None" ErrorMessage="Please select the location"
                                                                    SetFocusOnError="True"></asp:CustomValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                    TargetControlID="CustomValidator2">
                                                                </asp:ValidatorCalloutExtender>
                                                            </div>
                                                            <div class="fc-input">
                                                                <asp:TextBox ID="txtLocation" runat="server" autocomplete="off" CssClass="form-control searchinputloc"
                                                                    placeholder="Search by location name, phone#, address etc." TabIndex="6"></asp:TextBox>

                                                                <asp:TextBox Visible="false" TextMode="MultiLine" CssClass="form-control" Height="20px" Font-Size="10px" ID="lblLocation" runat="server" ReadOnly="true"></asp:TextBox>

                                                                <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:Button ID="btnSelectLoc" runat="server" CausesValidation="False" OnClick="btnSelectLoc_Click"
                                                                    Style="display: none;" Text="Button" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 col-lg-12">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Category
                                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server"
                                                             ControlToValidate="ddlCategory" Display="None" ErrorMessage="Category Required"
                                                             SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                 ID="RequiredFieldValidator20_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                 TargetControlID="RequiredFieldValidator20">
                                                             </asp:ValidatorCalloutExtender>
                                                            </div>
                                                            <div class="fc-input">
                                                                <div>
                                                                    <asp:DropDownList Width="100%" ID="ddlCategory" runat="server" CssClass="form-control" AutoPostBack="false"
                                                                        TabIndex="16">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-lg-6">

                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Date 
                                                            </div>
                                                            <div class="fc-input">
                                                                <asp:TextBox ID="txtCallDt" ReadOnly="true" runat="server" CssClass="form-control" MaxLength="28"
                                                                    TabIndex="17"></asp:TextBox>

                                                            </div>
                                                        </div>

                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                <span>Caller<asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server"
                                                                    ControlToValidate="txtNameWho" Display="None" ErrorMessage="Caller Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                        ID="RequiredFieldValidator31_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                        PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator31">
                                                                    </asp:ValidatorCalloutExtender>
                                                                </span>
                                                            </div>
                                                            <div class="fc-input">
                                                                <asp:TextBox ID="txtNameWho" runat="server" CssClass="form-control" MaxLength="30"
                                                                    TabIndex="30"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        

                                                    </div>
                                                    <div class="col-md-6 col-lg-6 col-sm-6">

                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Time
                                                            </div>
                                                            <div class="fc-input">
                                                                <asp:TextBox ID="txtCallTime" ReadOnly="true" CssClass="form-control timepicker" runat="server" TabIndex="18" MaxLength="28"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Phone
                                                            </div>
                                                            <div class="fc-input">
                                                                <asp:TextBox ID="txtCell" runat="server" CssClass="form-control" MaxLength="28"
                                                                    TabIndex="32" placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                       
                                                    </div> 
                                                           <div class="col-md-12 col-lg-12">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                               Equipment
                                                   
                                                            </div>
                                                            <div class="fc-input">
                                                                <div>
                                                                     <asp:TextBox ID="txtUnit" runat="server" Style="position: relative; z-index: 1"
                                                                    autocomplete="off" CssClass="form-control" TextMode="MultiLine"
                                                                    TabIndex="20"></asp:TextBox>
                                                                
                                                                <div id="divEquip" class="menu_popup_chklst shadow" style="width: 500px; z-index: 2">

                                                                    <asp:GridView ID="gvEquip" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                        DataKeyNames="ID" PageSize="20">
                                                                        <RowStyle CssClass="evenrowcolor" />
                                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblID" runat="server" Style="display: none;" Text='<%# Bind("id") %>'></asp:Label>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                </ItemTemplate>
                                                                                <HeaderTemplate>
                                                                                </HeaderTemplate>
                                                                                <ItemStyle Width="0px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Name" SortExpression="unit">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Unique #" SortExpression="state">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUID" runat="server"><%#Eval("state")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Description" SortExpression="fdesc">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Category" SortExpression="category">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblcat" runat="server"><%#Eval("category")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField Visible="false" HeaderText="Service type" SortExpression="cat">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblServiceType" runat="server"><%#Eval("cat")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="% Hours" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtHours" runat="server" Width="50px" MaxLength="20"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                    </asp:GridView>

                                                                </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-lg-12">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Reason for service
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="txtReason"
                                                            Display="None" ErrorMessage="Reason for Service Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator30_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator30">
                                                                </asp:ValidatorCalloutExtender>
                                                            </div>
                                                            <div class="fc-input">
                                                                <div>
                                                                    <asp:TextBox ID="txtReason" runat="server" Style="position: relative;"
                                                                        MaxLength="255" TabIndex="32" TextMode="MultiLine"
                                                                        CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                </span>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <div class="clearfix"></div>
    </div>
    <asp:HiddenField runat="server" ID="MSTimeDataFieldVisibility" />
    <asp:HiddenField runat="server" ID="hdnFocus" />
    <input id="hdnCon" runat="server" type="hidden" />
</asp:Content>
