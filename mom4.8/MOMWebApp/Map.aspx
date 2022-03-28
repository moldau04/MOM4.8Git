<%@ Page Title="Map || MOM" Language="C#" MasterPageFile="Mom.master" AutoEventWireup="true" Inherits="Map" Codebehind="Map.aspx.cs" %>

<%@ Register Assembly="Artem.GoogleMap" Namespace="Artem.Web.UI.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        .RadComboBox_Bootstrap .rcbReadOnly {
            margin-top: 9px !important;
        }

        .rcbInner {
            margin-bottom: 10px !important;
        }
    </style>
   
    <script type="text/javascript">
        //Script remove googleapis lib to resolved issue included the Google Maps multiple times.
        if (window.google !== undefined && google.maps !== undefined) {
            delete google.maps;
            $('script').each(function () {
                if (this.src.indexOf('googleapis.com/maps') >= 0) {
                    $(this).remove();
                }
            });
        }

        var myVar;
        var TO;
        var strtext;
 


        function LocateAddress(Tech, Date, Time, AddressField, TicketID, timestamp) {
            $("#" + AddressField).html('Loading...');
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "map.aspx/getlocationAddress",
                data: '{"Tech":"' + Tech + '","Date":"' + Date + '","Time":"' + Time + '","TicketID":"' + TicketID + '","timestamp":"' + timestamp + '"}',
                dataType: "json",
                async: true,
                success: function (data) {
                    $("#" + AddressField).html(data.d);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    var err = eval("(" + XMLHttpRequest.responseText + ")");
                    alert(err.Message);
                }
            });
        }

    </script>

<%--    //////////////NK CODE--%>

    <script type="text/javascript" >
        function ping() {
            Pleasepingtoworker();
        }
        function Pleasepingtoworker() {
            if ($("#<%= ddlTech.ClientID %> option:selected").text() == 'Select') { 

                var err = 'Please select Worker';

                noty({ text: err, dismissQueue: true, type: 'information', layout: 'topCenter', closeOnSelfClick: false, timeout: false, theme: 'noty_theme_default', closable: true });

                 return;
            }
             

            pingbutton('hide'); 

            var workername = $("#<%= ddlTech.ClientID %> option:selected").text();

            var randomIdnk =  $.now();

            $("#<%= hdnRandomid.ClientID %>").val(randomIdnk);

            _webconfig = $("#<%= hdnwebconfig.ClientID %>").val();

            /////////////////
            debugger;
            var tArr = {};
            tArr.workername = workername.trim();
            tArr.webconfig = _webconfig; 
            tArr.Randomid = randomIdnk; 
            var _tArr = JSON.stringify(tArr);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Map/GetTokenbyuser",
                data: _tArr,
                async: true,
                success: function (data) {
                    console.log(data);
                    var dataResult = JSON.parse(data);
                    debugger;
                    if (dataResult.Success === '200') {
                        var deviceToken = dataResult.token;
                        callGoogleAPI(deviceToken, randomIdnk, workername, _webconfig);
                    } else {
                        notregistered(); 
                        pingbutton('show');
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    notregistered();
                    pingbutton('show');
                }
            }); 

            ///////////////////

           
         }

       
        function callGoogleAPI(deviceToken, randomIdnk, workername, _webconfig) {
            debugger;
            console.log(deviceToken);
            console.log(randomIdnk);

            var pushMainArr = {};
            pushMainArr.to = deviceToken.trim();
            pushMainArr.collapse_key = "type_a";
            var pushSecondArr = {};
            var notifArr = {};
            pushSecondArr.body = "Ping Notification";
            pushSecondArr.title = "ESS Mobile Tracker Ping";
            pushSecondArr.RandomId = randomIdnk;
            pushMainArr.data = pushSecondArr;
            notifArr.body = "Ping Notification";
            notifArr.title = "ESS Mobile Tracker Ping";
            notifArr.content_available = "true";
            notifArr.priority = "high";
            pushMainArr.notification = notifArr;


            var mainDataToPass = JSON.stringify(pushMainArr);
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "https://fcm.googleapis.com/fcm/send",
                data:  mainDataToPass ,
                headers: {
                    'Authorization': 'key=AAAAwaKv4PY:APA91bG_hJByzW_TRoSKtmLuh_z8mrD5FwQOIZhyjTTKXZu40lN54qNIOdQch70qCwD5o_WQz4yiuxRtKafU-Vb2BWD6R9zXpoaKSdMNhaG3qTH1GQavd7HWkmNw-F6nBcPayYQ7ysAL'
                },
                
               // async: true,
                success: function (data) { 
                    pingResponse(deviceToken, randomIdnk, workername, _webconfig); 
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    failuremessage();
                     
                }

            });
        }
        
        var myVar;     var TO;       var strtext;

        function pingResponse(deviceToken, randomIdnk, workername, _webconfig) { 

            var tArr = {};
            tArr.workername = workername.trim();
            tArr.webconfig = _webconfig; 
            tArr.Randomid = randomIdnk;
            var _tArr = JSON.stringify(tArr);
            TO = setTimeout(function () {
                clearInterval(myVar); 
                failuremessage ()
            }, 20000);
            myVar = setInterval(function () {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "map/GetpingResponse",
                    data: _tArr, 
                    async: true,
                    success: function (data) {
                        if (data != '0') {
                            noty({ text: data, dismissQueue: true, type: 'information', layout: 'topCenter', closeOnSelfClick: false, timeout: false, theme: 'noty_theme_default', closable: true });
                            clearInterval(myVar);
                            clearTimeout(TO);
                            pingbutton('show');
                        } else {
                            clearInterval(myVar);
                            clearTimeout(TO);
                            failuremessage();
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        failuremessage();
                    }
                });
            }
                , 2000
            );

        }


        function failuremessage() {
            strtext = '<strong>Request Timeout!</strong></br></br> Please check if the device </br>1) Is not switched off </br>2) Mobile Service Tracker is not installed </br>3) Has no internet connectivity'; 
            

                noty({ text: strtext, dismissQueue: true, type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: false, theme: 'noty_theme_default', closable: true });

            pingbutton('show');
        }

        function notregistered() {

            var err = 'Device not registered for ping';

            noty({ text: err, dismissQueue: true, type: 'information', layout: 'topCenter', closeOnSelfClick: false, timeout: false, theme: 'noty_theme_default', closable: true });

        }

        function pingbutton(show) {
            if (show == 'hide') {
                $("#btnPingClient").hide();
                $("#btnPingClientnk").show();
            } else {
                $("#btnPingClient").show();
                $("#btnPingClientnk").hide();
            }
        }

    </script>

    <%--    END//////////////NK CODE--%>


    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
        <script type="text/javascript">



            (function (global, undefined) {
                var telerikDemo = global.telerikDemo = {};
                Showopencallsonmap = function (sender, args) {
                    var btn = document.getElementById('<%=btnchkOpencalls.ClientID%>');
                    btn.click();
                };
            })(window);




            //<![CDATA[

            function OnDateSelected(sender, eventArgs) {
                //var date1 = sender.get_selectedDate();
                //date1.setDate(date1.getDate() + 31);
                var datepicker = $find("<%= txtDate.ClientID %>");
                //datepicker.set_maxDate(date1);
            }


            //]]>


            function pageLoad() {

                var grid = $find("<%= RadGridOpenCalls.ClientID %>");
                var columns = grid.get_masterTableView().get_columns();
                for (var i = 0; i < columns.length; i++) {
                    columns[i].resizeToFit(false, true);
                }
                var grid = $find("<%= RadgvTimeStmp.ClientID %>");
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div>
        <div class="divbutton-container">
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">
                    <header>
                        <div class="container row-color-grey">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <div class="page-title"><i class="mdi-maps-pin-drop"></i>&nbsp;<asp:Label CssClass="title_text" ID="lblHeader" runat="server">Map</asp:Label></div>

                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkReset" runat="server" CssClass="icon-addnew" Text="Reset" CausesValidation="False" OnClick="lnkReset_Click"
                                                    ToolTip="Reset"></asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="btnclosewrap">
                                            <a href="Home.aspx"><i class="mdi-content-clear"></i></a>
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



    <div class="container">
        <div class="row">
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="btnSearch">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGridOpenCalls" LoadingPanelID="RadAjaxLoadingPanelRadGridOpenCalls"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadgvTimeStmp" LoadingPanelID="RadAjaxLoadingPaneTimeStamp"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="GoogleMapDIV" LoadingPanelID="RadAjaxLoadingPaneMAP"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="GoogleMap1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnNearest">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="GoogleMapDIV" LoadingPanelID="RadAjaxLoadingPaneMAP"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnchkOpencalls">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="GoogleMapDIV" LoadingPanelID="RadAjaxLoadingPaneMAP"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>

                    <telerik:AjaxSetting AjaxControlID="lnkReset">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGridOpenCalls" LoadingPanelID="RadAjaxLoadingPanelRadGridOpenCalls"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadgvTimeStmp" LoadingPanelID="RadAjaxLoadingPaneTimeStamp"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="ddlTech" LoadingPanelID="RadAjaxLoadingPanelSearchBy"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="ddlCategory" LoadingPanelID="RadAjaxLoadingPanelSearchBy"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="txtDate" LoadingPanelID="RadAjaxLoadingPanelSearchBy"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="chkAssignedCall" LoadingPanelID="RadAjaxLoadingPanelSearchBy"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="chkOpencalls" LoadingPanelID="RadAjaxLoadingPanelRadGridOpenCalls"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>

            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelRadGridOpenCalls" runat="server">
            </telerik:RadAjaxLoadingPanel>

            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPaneMAP" runat="server">
            </telerik:RadAjaxLoadingPanel>

            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPaneTimeStamp" runat="server">
            </telerik:RadAjaxLoadingPanel>

            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelSearchBy" runat="server">
            </telerik:RadAjaxLoadingPanel>
             <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
            </telerik:RadAjaxLoadingPanel>



            <telerik:RadSplitter RenderMode="Auto" ID="RadSplitter1" Skin="Material" runat="server" Width="100%" Height="750px">
                <telerik:RadPane ID="LeftPane" runat="server" Width="22px" Scrolling="none">
                    <telerik:RadSlidingZone ID="SlidingZone1" runat="server" Width="22px" ClickToOpen="true">
                        <telerik:RadSlidingPane ID="RadSlidingPane1" Title="Open Calls" runat="server" Width="300px"
                            MinWidth="200">

                            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelRADOpneCall" LoadingPanelID="RadAjaxLoadingPanelRadGridOpenCalls" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                                <div style="margin: 2px; padding: 2px;">
                                    <asp:LinkButton
                                        ID="btnNearest" runat="server" ForeColor="SkyBlue" Font-Bold="true" Style="text-decoration: underline;" OnClick="btnNearest_Click">Find Nearest Worker</asp:LinkButton>


                                    <telerik:RadCheckBox Style="background-color: white!important; font-size: 12px;" OnClientCheckedChanged="Showopencallsonmap" Checked="false" runat="server" ID="chkOpencalls" Text="Show open calls on map" AutoPostBack="false">
                                    </telerik:RadCheckBox>

                                    <asp:LinkButton ID="btnchkOpencalls" runat="server" Style="display: none;" OnClick="btnchkOpencalls_Click" Text=""></asp:LinkButton>
                                </div>




                                <telerik:RadGrid PageSize="10" AllowPaging="true"
                                    EmptyDataText="No Tickets Found..."
                                    runat="server" ID="RadGridOpenCalls" GridLines="None"
                                    OnNeedDataSource="RadGridOpenCalls_NeedDataSource"
                                    AutoGenerateColumns="False"
                                    OnDataBound="gvOpenCalls_DataBound"
                                    OnRowCommand="gvOpenCalls_RowCommand"
                                    Skin="Material"
                                    ShowFooter="false" ShowHeader="false"
                                    PagerStyle-AlwaysVisible="true"
                                    AllowSorting="false">
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>

                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="id" AllowPaging="true" ShowFooter="false">
                                        <Columns>
                                            <telerik:GridTemplateColumn DataField="id">
                                                <ItemTemplate>

                                                    <div class="table table-advance">

                                                        <telerik:RadCheckBox runat="server" ID="chkSelect" Text='<%# "TicketID#: "+Eval("id") %>' AutoPostBack="false">
                                                        </telerik:RadCheckBox>
                                                        </br> 
                                                     <b>Location:</b>
                                                        <span><%# Eval("ldesc1") %> 
                                                    </br> 
                                                    <b>Address:</b>
                                                            <span><%# Eval("address") %></span>
                                                            </br> 
                                                         <b>Category:</b>
                                                            <span><%# Eval("cat") %>
                                                            </span>
                                                            </br> 
                                                        <b>Schedule Date:</b>

                                                            <span>
                                                                <%# Eval("edate","{0:MM/dd/yyyy}") %>
                                                            </span>
                                                            </br>  
                                                            <b>Call Date:</b>
                                                            <span><%# Eval("cdate","{0:MM/dd/yyyy}") %> </span>
                                                            </br> 
                                                      
                                                            <b>Assigned to:</b>

                                                            <span>
                                                                <%# Eval("dwork") %>
                                                            </span>
                                                            </br> 
                                                            <b>Status:</b>
                                                            <span>
                                                                <%# Eval("assignname") %>
                                                            </span>
                                                    </div>
                                                    <asp:Label ID="lbladdress" Visible="false" runat="server" Text='<%# Bind("address") %>'></asp:Label>
                                                    <asp:Label ID="lblLat" Visible="false" runat="server" Text='<%# Bind("lat") %>'></asp:Label>
                                                    <asp:Label ID="lblLng" Visible="false" runat="server" Text='<%# Bind("lng") %>'></asp:Label>
                                                    <asp:Label ID="lblStatus" Visible="false" runat="server" Text='<%# Bind("assignname") %>'></asp:Label>
                                                    <asp:Label ID="lblTicketId" Visible="false" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>



                            </telerik:RadAjaxPanel>

                        </telerik:RadSlidingPane>

                    </telerik:RadSlidingZone>
                </telerik:RadPane>

                <telerik:RadSplitBar ID="RadSplitBar1" runat="server">
                </telerik:RadSplitBar>

                <telerik:RadPane ID="MiddlePane1" runat="server" Scrolling="None" Skin="Material" Height="100%">
                    <telerik:RadSplitter RenderMode="Auto" ID="Radsplitter2" runat="server" Orientation="Horizontal" VisibleDuringInit="true" Skin="Material">
                        <telerik:RadPane ID="Radpane1" runat="server" Width="40px" Height="20px" Scrolling="none">
                            <telerik:RadSlidingZone ID="Radslidingzone2" runat="server" Width="100" Height="20px" SlideDirection="Bottom" ClickToOpen="true">
                                <telerik:RadSlidingPane ID="Radslidingpane4" IconUrl="~/images/Black_Search.png"
                                    Font-Size="0.9em" Title="Search by" runat="server" Width="100px" Height="120px">
                                    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelSreachBY1" LoadingPanelID="RadAjaxLoadingPanelSearchBy" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                                        <div style="margin: 10px;">


                                            <div class="form-section3" style="max-width: 200px !important;">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Worker</label>
                                                        <asp:DropDownList ID="ddlTech" CssClass="browser-default" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTech"
                                                            Display="None" ErrorMessage="Worker Required" SetFocusOnError="True" ValidationGroup="src"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator1">
                                                        </asp:ValidatorCalloutExtender>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                            <div class="form-section3" style="max-width: 100px !important;">
                                                <div class="input-field col s12">
                                                    <div class="row">

                                                        <div class="btncontainer">
                                                            <div class="btnlinks">
                                                                <input id="btnPingClient" type="button"   value="Ping" onclick="ping();" /> 

                                                                 <span id="btnPingClientnk" style="display:none!important;"> <img height="30px" src="images/wheel.gif"/></></span>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp; 
 
                                            </div>
                                            <div class="form-section3" style="max-width: 200px !important;">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Category</label>
                                                      
                                                        <telerik:RadComboBox ID="ddlCategory" EmptyMessage="Select" BackColor="White" CssClass="browser-default" runat="server" CheckBoxes="true" Width="102%" EnableCheckAllItemsCheckBox="true">
                                                        </telerik:RadComboBox>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                            <div class="form-section3" style="max-width: 180px !important;">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="label">Date</label>

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDate"
                                                            Display="None" ErrorMessage="Date Required" SetFocusOnError="True" ValidationGroup="src"></asp:RequiredFieldValidator>
                                                        <telerik:RadDatePicker ID="txtDate" Height="35px" CssClass="browser-default" ClientEvents-OnDateSelected="OnDateSelected" runat="server">
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                            <div class="form-section3" style="max-width: 60px !important;">
                                                <div class="input-field col s12">
                                                    <div class="row">

                                                        <telerik:RadCheckBox Style="background-color: white!important; font-size: 12px;" runat="server" ID="chkAssignedCall" ToolTip="Show calls assigned to Tech" Text="Calls" AutoPostBack="false">
                                                        </telerik:RadCheckBox>

                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                            </div>


                                            <div class="form-section3" style="max-width: 50px !important;">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="label"></label>
                                                        <asp:HyperLink Visible="true" ID="lnkLegend" runat="server" Style="margin-top: 5px;" ToolTip="Legend"><img src="images/legend.ico"  alt="Legend"   /></asp:HyperLink>
                                                        <%-- <asp:ImageButton ID="btnSearch" Width="20px" runat="server" ImageUrl="images/refresh.png" OnClick="btnSearch_Click"
                                                ToolTip="Refresh Tech's Location" ValidationGroup="src"></asp:ImageButton>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                            </div>


                                            <div class="form-section3" style="max-width: 100px !important;">
                                                <%-- <div class="input-field col s12">
                                                    <div class="row">
                                                        <div class="btncontainer">
                                                            <div class="btnlinks">--%>
                                                <div class="srchinputwrap srchclr btnlinksicon" style="margin-top: -5px!important;">
                                                    <asp:LinkButton ID="btnSearch" ValidationGroup="src" Width="50px"
                                                        ToolTip="Refresh Worker Location" OnClick="btnSearch_Click" runat="server" CausesValidation="true"><i class="mdi-notification-sync"></i></asp:LinkButton>
                                                    
                                                </div>
                                                <%--</div>
                                                        </div>

                                                    </div>
                                                </div>--%>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp; 
                                            </div>



                                            <div class="form-section3" style="max-width: 200px !important;">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="label"></label>

                                                        <asp:HyperLink ID="HyperLink1" runat="server" ToolTip="Directions" Style="float: right; margin-right: 5px; display: none"><img src="images/directions_sign.ico"  alt="Directions" height="20px" width="20px" /></asp:HyperLink>
                                                        <%--Currently Not Used OnClick="btnPing_Click"--%>
                                                        <asp:Button ID="btnPing" runat="server" Text="Ping Device"
                                                            ValidationGroup="src" Width="100px" Visible="False" />
                                                        <asp:HiddenField ID="hdnRandomid" runat="server" />
                                                        <asp:HiddenField ID="hdnwebconfig" runat="server" />
                                                        <asp:HiddenField ID="hdnDeviceID" runat="server" />
                                                        <asp:CheckBox ID="chkEffective" runat="server" Text="Show effective route"
                                                            Visible="False" />


                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                        </div>

                                    </telerik:RadAjaxPanel>

                                </telerik:RadSlidingPane>
                            </telerik:RadSlidingZone>
                        </telerik:RadPane>
                        <telerik:RadSplitBar ID="RadSplitBar3" runat="server">
                        </telerik:RadSplitBar>

                        <telerik:RadPane ID="Radpane2" runat="server">
                            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelMAP" LoadingPanelID="RadAjaxLoadingPaneMAP" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                <div id="GoogleMapDIV"></div>
                            </telerik:RadAjaxPanel>
                             <%--style="position:absolute !important;overflow:visible !important"--%>
                             <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <cc1:GoogleMap ID="GoogleMap1" runat="server" style="position:absolute !important;overflow:visible !important;width:82.9% !important;height:92.5% !important" BorderColor="SlateBlue" BorderStyle="Solid" BorderWidth="1px" Width="99.9%" Height="99.5%" Zoom="15"
                                DefaultMapView="Normal" EnableScrollWheelZoom="True" EnableReverseGeocoding="true"
                                ZoomPanType="Large3D" InsideUpdatePanel="false" EnableViewState="False">
                            </cc1:GoogleMap>
                                 </telerik:RadAjaxPanel>
                        </telerik:RadPane>
                    </telerik:RadSplitter>
                </telerik:RadPane>


                <telerik:RadSplitBar ID="RadSplitBar2" runat="server">
                </telerik:RadSplitBar>

                <telerik:RadPane ID="RightPane" runat="server" Width="22px" Scrolling="None">
                    <telerik:RadSlidingZone ID="Radslidingzone4" runat="server" Width="22px" ClickToOpen="true"
                        SlideDirection="Left">
                        <telerik:RadSlidingPane ID="Radslidingpane7" Title="Timestamp" runat="server" Width="300px"
                            MinWidth="200">

                            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelGVTimeStamp" LoadingPanelID="RadAjaxLoadingPaneTimeStamp" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                <telerik:RadGrid ID="RadgvTimeStmp" runat="server" PageSize="10" AllowPaging="true"
                                    EmptyDataText="No Data Found..."
                                    Skin="Material"
                                    OnNeedDataSource="RadgvTimeStmp_NeedDataSource"
                                    ShowHeader="false"
                                    PagerStyle-AlwaysVisible="True"
                                    AllowSorting="false"
                                    ShowFooter="True">
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>

                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="id" AllowPaging="true">
                                        <Columns>
                                            <telerik:GridTemplateColumn DataField="id">
                                                <ItemTemplate>
                                                    <div class="table table-advance">
                                                        <b>TicketID# :</b>
                                                        <%# Eval("id") %>
                                                        <br />
                                                        <b>Status :</b>
                                                        <%# Eval("assignname") %>
                                                        <br />
                                                        <b>Assigned calls :</b>
                                                        <%# Eval("address") %>
                                                        <br />
                                                        <b>Assigned to :</b>
                                                        <%# Eval("dwork") %>
                                                        <br />
                                                        <b>Address   :</b>
                                                        <%# String.Format("{0}, {1}", Eval("ldesc3"), Eval("city")) %>
                                                        <br />
                                                        <b>Location  :</b>
                                                        <%# Eval("ldesc1") %>
                                                        <br />
                                                        <b>Time   :</b>
                                                        <%# Eval("EDate", "{0:t}") %>
                                                        <br />


                                                        <b>On Site Time:</b>
                                                        <span>

                                                            <asp:Label ID="lblTimert" runat="server" Text='<%# Eval("timeroute", "{0:t}") %>'></asp:Label>
                                                            <asp:Label ID="lblAddressOR" runat="server" CssClass="hoverGrid shadow transparent roundCorner" Text='<%# Eval("AddressER") %>'></asp:Label>
                                                            <asp:HoverMenuExtender ID="hmeAddressor" runat="server" OffsetY="15" OffsetX="80"
                                                                PopupControlID="lblAddressOR" TargetControlID="lblTimert">
                                                            </asp:HoverMenuExtender>
                                                        </span>
                                                        <br />
                                                        <b>Complete Time:</b>

                                                        <span>
                                                            <asp:Label ID="lblTimesite" runat="server" Text='<%# Eval("timesite", "{0:t}") %>'></asp:Label>
                                                            <asp:Label ID="lblAddressOS" runat="server" CssClass="hoverGrid shadow transparent roundCorner" Text='<%# Eval("AddressOS") %>'></asp:Label>
                                                            <asp:HoverMenuExtender ID="hmeAddressOS" runat="server" OffsetY="15" OffsetX="80"
                                                                PopupControlID="lblAddressOS" TargetControlID="lblTimesite">
                                                            </asp:HoverMenuExtender>

                                                        </span>
                                                        <br />
                                                        <b>Enroute Time</b>

                                                        <span>
                                                            <asp:Label ID="lblTimecomp" runat="server" Text='<%# Eval("timecomp", "{0:t}") %>'></asp:Label>
                                                            <asp:Label ID="lblAddressCM" runat="server" CssClass="hoverGrid shadow transparent roundCorner" Text='<%# Eval("AddressCM") %>'></asp:Label>
                                                            <asp:HoverMenuExtender ID="hmeAddressCM" runat="server" OffsetY="15" OffsetX="80"
                                                                PopupControlID="lblAddressCM" TargetControlID="lblTimecomp">
                                                            </asp:HoverMenuExtender>
                                                        </span>
                                                        <br />

                                                        <b>Dist. ER-OS (Miles):</b>
                                                        <span>
                                                            <asp:Label ID="lbldistENOS" runat="server" Text='<%# Bind("distanceEROS") %>'></asp:Label></span>
                                                        <br />
                                                        <b>Dist. CO-Next ER (Miles):</b>
                                                        <span>
                                                            <asp:Label ID="lbldistCOER" runat="server" Text='<%# Bind("distanceCOER") %>'></asp:Label></span>
                                                        <br />
                                                        <b>Total Dist. (Miles):</b>
                                                        <span>
                                                            <asp:Label ID="lbldisttot" runat="server">0</asp:Label>
                                                        </span>
                                                    </div>

                                                    <asp:Label ID="lblId" Visible="false" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                    <asp:Label ID="lblAssign" Visible="false" runat="server" Text='<%# Bind("assignname") %>'></asp:Label>
                                                    <asp:Label ID="Label2" Visible="false" runat="server" Text='<%# Bind("LDesc1") %>'></asp:Label>
                                                    <asp:Label ID="lblAssdTo" Visible="false" runat="server" Text='<%# Bind("dwork") %>'></asp:Label>
                                                    <asp:Label ID="lblAddressCol" Visible="false" runat="server" Text='<%# String.Format("{0}, {1}", Eval("ldesc3"), Eval("city")) %>'></asp:Label>
                                                    <asp:Label ID="lblEdate" Visible="false" runat="server" Text='<%# Eval("EDate", "{0:t}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <FooterTemplate>
                                                    <table>
                                                        <tr>
                                                            <td><b>Total Dist. ER-OS (Miles):</b>  </td>
                                                            <td><b>Total Dist. CO-Next ER (Miles):</b>  </td>
                                                            <td><b>Total Dist. (Miles):</b>  </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbldistENOSfooter" runat="server">0</asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lbldistCOERfooter" runat="server">0</asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lbldisttotfooter" runat="server">0</asp:Label></td>
                                                        </tr>

                                                    </table>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadAjaxPanel>

                        </telerik:RadSlidingPane>
                    </telerik:RadSlidingZone>
                </telerik:RadPane>

            </telerik:RadSplitter>

            <asp:HoverMenuExtender ID="hmelegend" runat="server" OffsetY="15" PopupControlID="route"
                TargetControlID="HyperLink1">
            </asp:HoverMenuExtender>

            <asp:HoverMenuExtender ID="HoverMenuExtender1" runat="server" OffsetY="15" PopupControlID="legend"
                TargetControlID="lnklegend">
            </asp:HoverMenuExtender>

        
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
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
