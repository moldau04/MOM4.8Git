<%@ Page Language="C#" MasterPageFile="~/MOMRadWindow.Master" AutoEventWireup="true" Inherits="addProjectDocument" Codebehind="addProjectDocument.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script src="js/lity/lity.js"></script>
    <link href="js/lity/lity.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/Timepicker/jquery.timepicker.js"></script>
    <link rel="stylesheet" href="Scripts/Timepicker/jquery.timepicker.css" />
    <script type="text/javascript" src="appearance/js/bootstrap-datepicker.min.js"></script>
    <link rel="stylesheet" href="appearance/css/bootstrap-datepicker3.min.css" />
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <!-- dropify -->
    <script defer src="https://use.fontawesome.com/releases/v5.0.10/js/all.js"></script>
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
        });
        ////////////////// Confirm Document Upload ////////////////////
        <%--function ConfirmProjectUpload(value) {
            var filename;
            var fullPath = value;
            if (fullPath) {
                var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
                filename = fullPath.substring(startIndex);
                if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                    filename = filename.substring(1);
                }
            }
            if (confirm('Upload ' + filename + '?')) {
                document.getElementById('<%=lnkUploadProjectDoc.ClientID %>').click();
            }
            else {
                document.getElementById('<%=lnkProjectPostback.ClientID %>').click();
            }
        }--%>

        function ConfirmProjectUpload(value) {
            if (confirm('Are you sure you want to upload?')) { document.getElementById('<%= lnkUploadProjectDoc.ClientID %>').click(); }
            else { document.getElementById('<%= lnkProjectPostback.ClientID %>').click(); }
        }
        ///-Document permission
        function AddDocumentClick(hyperlink) {
            var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd == "Y") {
                ConfirmProjectUpload(hyperlink.value);
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
        }
        function DeleteDocumentClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteDocument.ClientID%>').value;
            if (IsDelete == "Y") {
                return confirm('Are you sure you want to delete?');;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function ViewDocumentClick(hyperlink) {
            var IsView = document.getElementById('<%= hdnViewDocument.ClientID%>').value;
            if (IsView == "Y") {
                window.postback = false; setTimeout(function () { window.postback = true; }, 100);
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function ViewIMGClick(hyperlink) {
            var IsView = document.getElementById('<%= hdnViewDocument.ClientID%>').value;
            if (IsView == "Y") {
                window.postback = false; setTimeout(function () { window.postback = true; }, 100);
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
    </script>

    <style>
        #overlay {
            position: fixed; /* Sit on top of the page content */
            display: none; /* Hidden by default */
            width: 100%; /* Full width (cover the whole page) */
            height: 100%; /* Full height (cover the whole page) */
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5); /* Black background with opacity */
            z-index: 1000000; /* Specify a stack order in case you're using a different order for other elements */
            cursor: pointer; /* Add a pointer on hover */
        }

        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }

        .chklist label {
            margin-left: 10px !important;
        }

        .chklist input {
            height: 12px !important;
        }

        .HighliteBlue {
            background-color: #316b9d !important;
        }

        .shadow {
            /* rgba(0, 0, 0, 0.3) rgb(90, 168, 208)*/
            -moz-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
        }

        .shadowHover:hover {
            -moz-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
        }

        .hoverGrid {
            display: none;
            position: absolute;
            min-width: 300px;
            max-width: 800px;
            min-height: 20px;
            /*font-weight: bold;*/
            font-size: 14px;
            padding: 5px 5px 5px 5px;
            background: black;
            color: #FFF;
        }

        .transparent {
            zoom: 1;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .roundCorner {
            border: 1px solid #ccc;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
        }

        #scrollbox3 {
            overflow: auto;
            width: 400px;
            height: 360px;
            padding: 0 5px;
            border: 1px solid #b7b7b7;
        }

        .track3 {
            width: 10px;
            background: rgba(0, 0, 0, 0);
            margin-right: 2px;
            border-radius: 10px;
            -webkit-transition: background 250ms linear;
            transition: background 250ms linear;
        }

            .track3:hover,
            .track3.dragging {
                background: #d9d9d9; /* Browsers without rgba support */
                background: rgba(0, 0, 0, 0.15);
            }

        .handle3 {
            width: 7px;
            right: 0;
            background: #999;
            background: rgba(0, 0, 0, 0.4);
            border-radius: 7px;
            -webkit-transition: width 250ms;
            transition: width 250ms;
        }

        .track3:hover .handle3,
        .track3.dragging .handle3 {
            width: 10px;
        }

        .thumbnail {
            margin-bottom: 0;
            padding: 0;
        }

        .thumb {
            margin: 0;
            padding: 5px 5px 0;
        }

        .navbar {
            margin-bottom: 5px;
            border-radius: 0;
        }

        .navbar-inverse {
            /*background-color: #414040;*/
            background-color: #316b9d;
            border: none !important;
        }

        .pager {
            margin: 5px 0;
        }

            .pager a {
                background-color: #fff;
                border: 1px solid #ddd;
                border-radius: 15px;
                display: inline-block;
                padding: 5px 14px;
            }

            .pager span {
                padding: 5px;
            }

            .pager .title {
                text-align: center;
                font-weight: bold;
                color: #fff;
                line-height: 32px;
            }

        .gallery span {
            display: inline-block;
            text-align: center;
            width: 100%;
            word-wrap: break-word;
        }

        .img-responsive {
            max-height: 100px;
            /*width: 50px;*/
            border-width: 0px;
            margin: 0 auto;
            text-align: left;
        }

        .pull-leftslider {
            float: left !important;
            /*width: 50%;*/
            cursor: pointer;
        }

        .dropdown-menu table {
            height: 50px;
            width: 50px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="padding: 10px;">
        <div id="overlay">
            <img src="images/loader_wheel.GIF" alt="Be patient..." style="position: fixed; margin-top: 20%; margin-left: 45%;" />
        </div>
        <asp:Panel ID="pnlDocPermission" runat="server">
            <div class="form-section-row">
                <div id="dvAttachment" style="font-weight: bold; font-size: 15px">
                    <%--<div class="form-section3">
                        <div class="input-field col s12">
                            <div class="row">
                                <label class="drpdwn-label">Document Type</label>
                                <asp:DropDownList ID="ddlAttachment" CssClass="browser-default" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlAttachment_SelectedIndexChanged" CausesValidation="false">
                                    <asp:ListItem Value="All">All</asp:ListItem>
                                    <asp:ListItem Value="Project">Project</asp:ListItem>
                                    <asp:ListItem Value="Tickets">Tickets</asp:ListItem>
                                    <asp:ListItem Value="Location">Location</asp:ListItem>
                                    <asp:ListItem Value="Customer">Customer</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <label class="drpdwn-label">Sort By</label>
                                <asp:DropDownList ID="ddlSortAttachment" CssClass="browser-default" CausesValidation="false" AutoPostBack="true" OnSelectedIndexChanged="ddlSortAttachment_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Value="2"> Type </asp:ListItem>
                                    <asp:ListItem Value="1"> Filename </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-section3-blank">
                        <div class="row">
                            &nbsp;
                        </div>
                    </div>--%>
                    <div class="form-section-row">
                        <label class="drpdwn-label" runat="server" id="fuspan">Upload Document</label>
                        <div class="input-field col s12">
                            <div class="row">
                                <div class="fc-input" style="padding-top: 5px;">
                                    <asp:FileUpload ID="FU_Project" runat="server"
                                        onchange="AddDocumentClick(this);" class="dropify" AllowMultiple="true" />
                                    <asp:LinkButton ID="lnkUploadProjectDoc" runat="server"
                                        CausesValidation="False" OnClick="lnkUploadProjectDoc_Click"
                                        Style="display: none">Upload</asp:LinkButton>
                                    <asp:LinkButton ID="lnkProjectPostback" runat="server"
                                        CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                </div>
                            </div>
                            <div class="btncontainer">
                                <asp:Panel ID="pnlDocumentButtons" runat="server">
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                    </div>
                                    <span class="tro trost">
                                        <asp:CheckBox ID="chkShowAllDocs" Text="Show All Documents" OnCheckedChanged="chkShowAllDocs_CheckedChanged" class="css-checkbox" Style="padding-left: 5px; color: black; font-weight: 400" ForeColor="Black" AutoPostBack="true" runat="server" />
                                    </span>
                                </asp:Panel>
                                <div style="clear: both;"></div>
                            </div>
                        </div>
                    </div>
                </div>
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
                            <%--<div class="btnlinks" style="margin-left: -10px;">
                                <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                            </div>--%>
                            
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                PagerStyle-AlwaysVisible="true" OnNeedDataSource="RadGrid_Documents_NeedDataSource" 
                                OnItemCommand="RadGrid_Documents_ItemCommand" OnPreRender="RadGrid_Documents_PreRender"
                                ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" AllowCustomPaging="True"
                                AllowMultiRowSelection="true">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="28">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn AllowFiltering="false" Visible="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdnTempId" Value='<%# Eval("id").ToString() == "0"? Eval("TempId"): string.Empty %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="filename" SortExpression="filename" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            HeaderText="File Name" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                    CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                    OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn FilterDelay="5" DataField="doctype" HeaderText="File Type" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="doctype"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn FilterDelay="5" DataField="Ticket" HeaderText="Ticket #" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Ticket" DataType="System.String"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

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

                                        <telerik:GridBoundColumn FilterDelay="5" DataField="Elev" HeaderText="Equipment #" HeaderStyle-Width="110"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Elev" DataType="System.String"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                            HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="100"
                                            DataType="System.Int16" UniqueName='MSVisible'
                                            >
                                            <FilterTemplate>
                                                <telerik:RadComboBox RenderMode="Auto" ID="ImportedFilter" runat="server" OnClientSelectedIndexChanged="ImportedFilterSelectedIndexChanged"
                                                    SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("MSVisible").CurrentFilterValue %>'
                                                    Width="100px" >
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                                <telerik:RadScriptBlock ID="RadScriptBlock12" runat="server">
                                                    <script type="text/javascript">
                                                        function ImportedFilterSelectedIndexChanged(sender, args) {
                                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                            var filterVal = args.get_item().get_value();
                                                            if (filterVal == "") {
                                                                tableView.filter("MSVisible", filterVal, "NoFilter");
                                                            }
                                                            else if (filterVal == "1") {
                                                                tableView.filter("MSVisible", "1", "EqualTo");
                                                            }
                                                            else if (filterVal == "0") {
                                                                tableView.filter("MSVisible", "0", "EqualTo");
                                                            }
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' 
                                                    Enabled='<%# Eval("Screen").ToString().ToUpper() == "PROJECT" %>' onclick="MSVisible_Click(this);"/>
                                                <asp:Button ID="btnMS" runat="server" CssClass="dummybutton" Style="display: none" CausesValidation="false" CommandName="UpdateMS" CommandArgument='<%# Eval("ID") %>' />
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn FilterDelay="5" UniqueName="Screen" DataField="screen" HeaderText="Screen" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Screen"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblScreen" runat="server" Text='<%# Eval("Screen") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <%--<telerik:GridTemplateColumn SortExpression="portal" HeaderText="Portal" DataField="portal" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>

                                        

                                        <%--<telerik:GridTemplateColumn SortExpression="remarks" HeaderText="Remarks" DataField="remarks" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtremarks" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>

                                        <%--<telerik:GridTemplateColumn FilterDelay="5" SortExpression="Ticket" HeaderText="Ticket #" DataField="Ticket" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblTicket" runat="server" Text='<%# Eval("Ticket") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                        
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>


<%--                <asp:Panel ID="Panel8" runat="server" Style="padding: 10px 10px 10px 10px; min-height: 100px;">
                    <asp:Label ID="Label3" runat="server" Style="font-style: italic; float: right"></asp:Label>
                    <br />
                    <asp:Repeater ID="rptattachmenttype" runat="server" OnItemDataBound="rptattachmenttype_ItemDataBound">
                        <ItemTemplate>
                            <!-- Navigation -->
                            <asp:HiddenField ID="hdntype" runat="server" Value='<%# Eval("Value") %>' />
                            <div>
                                <nav class="navbar" >
                                    <div class="pager">
                                        <span class="col-xs-4">
                                            <asp:LinkButton ID="lnkprevious" Visible="false" runat="server" Text="Previous" CssClass="pull-leftslider" CommandArgument='<%# Eval("Value") %>' CommandName="Previous"></asp:LinkButton>
                                        </span>
                                        <span class="col-xs-5  ">  <%# Eval("Value") %></span><span class="col-xs-3">
                                            <asp:LinkButton ID="lnknext" Visible="false" runat="server" Text="Next" CssClass="pull-right" CommandArgument='<%# Eval("Value") %>' CommandName="Next"></asp:LinkButton>
                                        </span>
                                        <asp:HiddenField ID="hdnpages" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hdnpagescount" runat="server"></asp:HiddenField>
                                    </div>
                                </nav>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:Repeater ID="rptattachment" runat="server" OnItemCommand="rptattachment_ItemCommand" OnItemDataBound="rptattachment_ItemDataBound">
                                            <ItemTemplate>
                                                <!-- Page Content -->
                                                <div class="gallery">
                                                    <div class="raDiv">
                                                        <a class="thumbnail" style="height: 110px; padding: 5px; display: flex; justify-content: center; align-items: center;"
                                                            <%# (Eval("content").ToString() == "image")? "href='"+ Eval("Value")+"'":"" %>
                                                            <%# (Eval("content").ToString() == "image")?" data-lity ":"" %>>
                                                            <asp:ImageButton ImageUrl='<%#  (Eval("content").ToString() == "image")? Eval("Value").ToString() : Eval("Value").ToString() %>'
                                                                ID="imgattachment" runat="server" CssClass="img-responsive"
                                                                OnClientClick="return ViewIMGClick(this);"
                                                                CommandName="OpenAttachment" CommandArgument='<%# Eval("path") %>'></asp:ImageButton>
                                                        </a>
                                                        <span>
                                                            <div style="float: left">
                                                                <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                                    <ContentTemplate>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkMS" runat="server" onclick="$(this).closest('div').find('.dummybutton').click();"
                                                                                        Checked='<%# Convert.ToBoolean( Eval("msvisible")) %>'
                                                                                        ToolTip="Show on Mobile Service" Text="Mobile Service" />
                                                                                </td>
                                                                                <td class="btnlinks">
                                                                                    <asp:LinkButton OnClientClick="return DeleteDocumentClick(this);"
                                                                                        Visible='<%# Eval("screen").ToString() =="Project"?true:false %>'
                                                                                        CommandName="DeleteAttachment" CausesValidation="false" ID="btnDelete" runat="server"
                                                                                        ToolTip="Delete" CommandArgument='<%# Eval("ID") %>'><i class="mdi-action-delete"></i></asp:LinkButton></td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:Button ID="btnMS" runat="server" CssClass="dummybutton" Style="display: none" CausesValidation="false" CommandName="UpdateMS" CommandArgument='<%# Eval("ID") %>' />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                            <div style="float: right">
                                                                
                                                                        <div class="btnlinks">
                                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="RotatedImgleft"
                                                                                Visible='<%# (Eval("content").ToString() == "image")?true:false %>' ToolTip="Rotate Left"
                                                                                CausesValidation="false"
                                                                                CommandArgument='<%# Eval("path") %>'>
                                                                                  <i class="mdi-image-rotate-left"   ></i>
                                                                            </asp:LinkButton>
                                                                        </div>
                                                                        <div class="btnlinks">
                                                                            <asp:LinkButton ID="LinkButton1" ToolTip="Rotate Right" runat="server" CommandName="RotatedImgright"
                                                                                Visible='<%# (Eval("content").ToString() == "image")?true:false %>'
                                                                                CausesValidation="false"
                                                                                CommandArgument='<%# Eval("path") %>'>
                                                                                  <i class="mdi-image-rotate-right"   ></i>
                                                                            </asp:LinkButton>

                                                                        </div>
                                                                    
                                                                
                                                            </div>
                                                          <asp:LinkButton ID="lnkDownload" runat="server" CommandName="OpenAttachment"
                                                                                            OnClientClick="return ViewDocumentClick(this);"
                                                                                            CommandArgument='<%# Eval("path") %>'>
                                                                                    <%# Eval("Text") %>
                                                                                        </asp:LinkButton>  
                                                        </span>
                                                    </div>
                                                </div>
                                                <!-- /.container -->
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="clearfix"></div>
                            <hr>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>--%>
            </div>
        </asp:Panel>
    </div>
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script>
        function pageLoad(sender, args) {
            if (typeof (Materialize) != 'undefined' && typeof (Materialize.updateTextFields) == 'function') {
                Materialize.updateTextFields();
            }
            $("img[id*='imgPlus']").click(function () {
                if ($(this).attr('src') == "images/plus.png") {
                    //$(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $("<tr style='display:none'><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>").insertAfter($(this).closest("tr")).show('slow');
                    $(this).attr("src", "images/minus.png");
                }
                else {
                    $(this).attr("src", "images/plus.png");
                    $(this).closest("tr").next().remove();
                }
            });

            var PageRequestManager = Sys.WebForms.PageRequestManager.getInstance();

            function BlockUI(sender, args) {
                document.getElementById("overlay").style.display = "block";
            }
            function UnblockUI(sender, args) {
                document.getElementById("overlay").style.display = "none";
            }
            PageRequestManager.add_beginRequest(BlockUI);
            PageRequestManager.add_endRequest(UnblockUI);
        }

        function MSVisible_Click(obj) {
            debugger
            $(obj).closest('td').find('.dummybutton').click();
        }
    </script>
</asp:Content>
