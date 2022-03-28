  <%@ Page Title="GoogleMap" Language="C#" MasterPageFile="Mom.master" AutoEventWireup="true" Inherits="GoogleMap" CodeBehind="GoogleMap.aspx.cs" %>
<%@ Register Assembly="Artem.GoogleMap" Namespace="Artem.Web.UI.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!--Grid Control-->

    <link href="Design/css/grid.css" rel="stylesheet" />

  
    <style>


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
            color: white;
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







        .RadComboBox_Bootstrap .rcbReadOnly {
            margin-top: 9px !important;
        }

        .rcbInner {
            margin-bottom: 10px !important;
        }

        #divMap {
            /*height: 90%;
   width: 90%;
   margin: 0px;
   padding: 0px*/
            position: relative;
            overflow: hidden;
            width: 99% !important;
            height: 99% !important;
            border-color: slateblue;
            border-width: 1px;
            border-style: solid;
            height: 99.5%;
            width: 99.9%;
        }
        /*position: relative;
    overflow: visible;
    width: 72% !important;
    height: 42% !important;*/
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



        function LocateAddress(Tech, Date, Time, AddressField, TicketID, timestamp) {


            debugger;


            $("#" + AddressField).html('Loading...');
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "MapFill.asmx/getlocationAddress",
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


    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
        <script type="text/javascript">

            function openInnerDiv() {
                $("#legend").show('slow');
            }


            (function (global, undefined) {
                var telerikDemo = global.telerikDemo = {};
                Showopencallsonmap = function (sender, args) {
                    var btn = document.getElementById('<%=btnchkOpencalls.ClientID%>');
                    btn.click();
                };
            })(window);

            function chkStateLiveData() {
                //myButton.OnClientClick = "chkStateLiveData();"
                //btnProcess.Attributes.Add("onclick", "javascript:chkStateLiveData();");
                if (gSelectCurrentDate == false) {
                    chkLiveData = false;
                    gSelectCurrentDate = false;
                    $('#<%=chkLiveData.ClientID%>').prop("disabled", true);
                    $('#<%=chkLiveData.ClientID%>').css("opacity", ".5");
                }
                else {
                    gSelectCurrentDate = true;
                    //$('#<%=chkLiveData.ClientID%>').attr("disabled", false);
                    $('#<%=chkLiveData.ClientID%>').prop("disabled", false);
                    $('#<%=chkLiveData.ClientID%>').css("opacity", "");
                }
            }



            //<![CDATA[
            var gSelectCurrentDate = true;
            var gCallGetJson = true;
            function OnDateSelected(sender, eventArgs) {
                //  var date1 = sender.get_selectedDate();
                // date1.setDate(date1.getDate() + 31);

                var datepicker = $find("<%= txtDate.ClientID %>");
                // datepicker.set_maxDate(date1);

                var selectedDate = datepicker.get_dateInput().get_selectedDate().format("yyyy/MM/dd");
                var todayDate = new Date();
                var dd = String(todayDate.getDate()).padStart(2, '0');
                var mm = String(todayDate.getMonth() + 1).padStart(2, '0'); //January is 0!
                var yyyy = todayDate.getFullYear();
                todayDate = yyyy + '/' + mm + '/' + dd;
                if (selectedDate != todayDate) {
                    chkLiveData = false;
                    gSelectCurrentDate = false;

                    var checkBox = $find("<%=chkLiveData.ClientID%>");
                    var isChecked = checkBox.get_checked();
                    if (isChecked == true) {
                        gCallGetJson = false;
                        checkBox.set_checked(!isChecked);
                        clearInterval(varInterval);

                    }


                    //$('#<%=chkLiveData.ClientID%>').attr("disabled", true);
                    $('#<%=chkLiveData.ClientID%>').prop("disabled", true);
                    $('#<%=chkLiveData.ClientID%>').css("opacity", ".5");
                }
                else {
                    gSelectCurrentDate = true;
                    gCallGetJson = true;

                    var checkBox = $find("<%=chkLiveData.ClientID%>");
                    var isChecked = checkBox.get_checked();
                    if (isChecked == false) {
                        gCallGetJson = true;
                    }

                    //$('#<%=chkLiveData.ClientID%>').attr("disabled", false);
                    $('#<%=chkLiveData.ClientID%>').prop("disabled", false);
                    $('#<%=chkLiveData.ClientID%>').css("opacity", "");
                }
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
                                                

                                                 <a href="GoogleMap.aspx">Reset</a>

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
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnNearest">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="GoogleMapDIV" LoadingPanelID="RadAjaxLoadingPaneMAP"></telerik:AjaxUpdatedControl>
                             <telerik:AjaxUpdatedControl ControlID="hdnNearest" ></telerik:AjaxUpdatedControl>                            
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnchkOpencalls">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="GoogleMapDIV" LoadingPanelID="RadAjaxLoadingPaneMAP"></telerik:AjaxUpdatedControl>
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




            <telerik:RadSplitter RenderMode="Auto" ID="RadSplitter1" Skin="Material" runat="server" Width="100%" Height="650px">
                <telerik:RadPane ID="LeftPane" runat="server" Width="22px" Scrolling="none">
                    <telerik:RadSlidingZone ID="SlidingZone1" runat="server" Width="22px" ClickToOpen="true">
                        <telerik:RadSlidingPane ID="RadSlidingPane1" Title="Open Calls" runat="server" Width="500px"    MinWidth="400">

                         

                            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelRADOpneCall"  LoadingPanelID="RadAjaxLoadingPanelRadGridOpenCalls" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                                <div class="find-ne-css" >
                                    <asp:LinkButton
                                        ID="btnNearest" runat="server" ForeColor="SkyBlue" Font-Bold="true" Style="text-decoration: underline;" OnClick="btnNearest_Click">Find Nearest Worker</asp:LinkButton>


                                  

                                    <telerik:RadCheckBox Style="background-color: white !important; font-size: 12px;" class="show-open-css"
                                        OnClientCheckedChanged="chkOpencallsCheckedCngd" Checked="false" runat="server" ID="chkOpencalls" Text="Show open calls on map" AutoPostBack="false">
                                    </telerik:RadCheckBox>

                                    <asp:LinkButton ID="btnchkOpencalls" runat="server" Style="display: none;" OnClick="btnchkOpencalls_Click" Text=""></asp:LinkButton>
                                </div>

                                 <%-- OnDataBound="gvOpenCalls_DataBound"
                                    OnRowCommand="gvOpenCalls_RowCommand"--%>
                                   <telerik:RadGrid ID="RadGridOpenCalls" runat="server" PageSize="100" AllowPaging="true"
                                    EmptyDataText="No Data Found..."
                                    Skin="Material"
                                    OnNeedDataSource="RadGridOpenCalls_NeedDataSource"
                                    ShowHeader="false"
                                    PagerStyle-AlwaysVisible="True"
                                    AllowSorting="false"
                                    ShowFooter="True">
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>

<%--                                <telerik:RadGrid PageSize="3" AllowPaging="true"   
                                    EmptyDataText="No Tickets Found..."
                                    runat="server" ID="" GridLines="None"
                                    OnNeedDataSource=""
                                    AutoGenerateColumns="False"
                                  
                                    Skin="Material"
                                    ShowFooter="true" ShowHeader="false"
                                    PagerStyle-AlwaysVisible="true"
                                    AllowSorting="false">
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>--%>

                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="id" AllowPaging="true" ShowFooter="false">
                                        <Columns>
                                            <telerik:GridTemplateColumn DataField="id">
                                                <ItemTemplate>

                                                    <div >

                                                        <table class="table">
                                                            <tr>

                                                                <td class="w-200" > <telerik:RadCheckBox runat="server" ID="chkSelect" Text='<%# "TicketID#: "+Eval("id") %>' AutoPostBack="false">
                                                        </telerik:RadCheckBox></td>

                                                               <td class="w-200" >  <b>Status:</b>
                                                            <span>
                                                                <%# Eval("assignname") %>
                                                            </span></td>
                                                                   

                                                            </tr>  


                                                             <tr>       <td  class="w-200" >   <b>Location:</b>
                                                        <span><%# Eval("ldesc1") %> </span> </td> 

                                                                  <td class="w-200" >  <b>Category:</b>
                                                            <span><%# Eval("cat") %>
                                                            </span></td>

                                                             </tr>

                                                          

                                                              <tr>
                                                                  <td class="w-200"> <b>Address:</b>
                                                            <span><%# Eval("address") %></span></td>

                                                                          <td class="w-200" > <b>Assigned to:</b>

                                                            <span>
                                                                <%# Eval("dwork") %>
                                                            </span></td>

                                                            </tr>  


                                                              

                                                             <tr>  

                                                                     <td class="w-200" > <b>Schedule Date:</b>

                                                            <span>
                                                                <%# Eval("edate","{0:MM/dd/yyyy}") %>
                                                            </span></td> 

                                             <td class="w-200" >   <b>Call Date:</b>
                                                            <span><%# Eval("cdate","{0:MM/dd/yyyy}") %> </span></td> 
                                                             </tr>  

                                                                      
                                                            
                                                           

                                                              
                                                           

                                                        </table>
                                                         
                                                             
                                                          
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
                                <telerik:RadSlidingPane ID="Radslidingpane4"  IconUrl="~/images/Black_Search.png"
                                    Font-Size="0.9em" Title="Search by" runat="server" Width="100px" Height="120px">
                                    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelSreachBY1" LoadingPanelID="RadAjaxLoadingPanelSearchBy" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                                        <div class="m-10" style="margin: 10px;">


                                            <div class="form-section3 m-w180" >
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Worker</label>
                                                        <asp:DropDownList ID="ddlTech" CssClass="browser-default" runat="server">
                                                        </asp:DropDownList>
                                                 
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>

                                            <div class="form-section3 m-w100" >
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
                                            <div class="form-section3 m-w200" >
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

                                            <div class="form-section3 m-w180" >
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="label">Date</label>

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDate"
                                                            Display="None" ErrorMessage="Date Required" SetFocusOnError="True" ValidationGroup="src"></asp:RequiredFieldValidator>
                                                        <telerik:RadDatePicker ID="txtDate" Height="35px" CssClass="browser-default" ClientEvents-OnDateSelected="OnDateSelected" AutoPostBack ="true" runat="server">
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>



                                            <div class="form-section3 m-w60" >
                                                <div class="input-field col s12">
                                                    <div class="row">

                                                        <telerik:RadCheckBox  Style="background-color: white!important; font-size: 12px;" runat="server" ID="chkAssignedCall" ToolTip="Show calls assigned to Tech" Text="Timestamps" OnClientCheckedChanged="chkAssignedCallCheckedCngd" AutoPostBack="false">
                                                        </telerik:RadCheckBox>

                                                    </div>
                                                </div>

                                            </div>


                                            <div class="form-section3 m-w100" >
                                                <div class="input-field col s12">
                                                    <div class="row">

                                                        <telerik:RadCheckBox  Style="background-color: white!important; font-size: 12px;" runat="server" ID="chkLiveData" ToolTip="" Text="Live Data" OnClientCheckedChanged="chkLivDataCheckedCngd" AutoPostBack="false">
                                                        </telerik:RadCheckBox>

                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                            </div>


                                            <div class="form-section3 m-w50" >
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="label"></label>
                                                        <img src="images/legend.ico"  id="imglegend"  alt="Legend"  onClick="openInnerDiv()" /> 

                                                         <div id="legend" style="background-color: #fff; display: none;" class="shadow">

                <table class="acl-pat-css" >
                    <tr>
                        <td  class="pat-css1"  width="25px">&nbsp;
                        </td>
                        <td>Actual path (GPS)
                        </td>
                    </tr>
                  
                    <tr>
                        <td  class="pat-css2"  width="25px">&nbsp;
                        </td>
                        <td>Effective path (Estimated shortest path)&nbsp;
                        </td>
                    </tr>
                   
                    <tr>
                        <td>
                            <img alt="" src="http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=U|F2AC09|000" />
                        </td>
                        <td>Un-Assigned Call
                        </td>
                    </tr>
                  
                    <tr>
                        <td>
                            <img alt="" src="http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=A|0947F2|000" />
                        </td>
                        <td>Assigned Call
                        </td>
                    </tr>
                   
                    <tr>
                        <td>
                            <img alt="" src="http://www.googlemapsmarkers.com/v1/CT/0099FF/" />
                        </td>
                        <td>Completed task position
                        </td>
                    </tr>
                  
                    <tr>
                        <td>
                            <img alt="" src="http://www.googlemapsmarkers.com/v1/OS/0099FF/" />
                        </td>
                        <td>Onsite position
                        </td>
                    </tr>
                
                    <tr>
                        <td>
                            <img alt="" src="http://www.googlemapsmarkers.com/v1/ER/0099FF/" />
                        </td>
                        <td>Enroute position
                        </td>
                    </tr>
                  
                    <tr>
                        <td>
                            <img alt="" src="http://www.googlemapsmarkers.com/v1/S/0099FF/FFFFFF/FF0000/" />
                        </td>
                        <td>Start position
                        </td>
                    </tr>
                
                    <tr>
                        <td>
                            <img alt="" src="http://www.googlemapsmarkers.com/v1/E/0099FF/FFFFFF/FF0000/" />
                        </td>
                        <td>End position
                        </td>
                    </tr>
                   
                    <tr>
                        <td>
                            <img alt="" src="images/white.png" />
                        </td>
                        <td>Scheduled call &nbsp;
                        </td>
                    </tr>
                    <%-- </table>

                <table>--%>
                    <tr>
                        <td>
                            <img alt="" src="images/yellow.png" />
                        </td>
                        <td>Hold call &nbsp;
                        </td>
                    </tr>
                 
                    <tr>
                        <td>
                            <img alt="" src="images/orange.png" />
                        </td>
                        <td>Onsite call&nbsp;
                        </td>
                    </tr>
               
                    <tr>
                        <td>
                            <img alt="" src="images/green.png" />
                        </td>
                        <td>Enroute call &nbsp;
                        </td>
                    </tr>
                  
                    <tr>
                        <td>
                            <img alt="" src="images/blue.png" />
                        </td>
                        <td>Completed call &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
                                                       
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                            </div>


                                            <div class="form-section3 m-w100" >
                                              
                                                <div class="srchinputwrap srchclr btnlinksicon m-t-n5">
                                                
                                                    
                                                        <asp:LinkButton ID="btnSearch" ValidationGroup="src" Width="50px" CssClass="btnSearchClass"
                                                            ToolTip="Refresh Worker Location" OnClientClick="javascript:return LoadMap();" OnClick="btnSearch_Click" runat="server" Visible="true" CausesValidation="true">
                                                       <%-- <i class="mdi-notification-sync"></i>  --%>
                                                             <i class="mdi-action-search"></i>
                                                        </asp:LinkButton>
                                                    </div>





                                               
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp; 
                                            </div>



                                            <div class="form-section3 m-w200" >
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
                                                         <asp:HiddenField ID="hdnNearest" runat="server" />
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
                            <div id="divMap"></div> 
                        </telerik:RadPane>
                    </telerik:RadSplitter>
                </telerik:RadPane>


                <telerik:RadSplitBar ID="RadSplitBar2" runat="server">
                </telerik:RadSplitBar>

                <telerik:RadPane ID="RightPane" runat="server" Width="22px" Scrolling="None">
                    <telerik:RadSlidingZone ID="Radslidingzone4" runat="server" Width="22px" ClickToOpen="true"
                        SlideDirection="Left">
                        <telerik:RadSlidingPane ID="Radslidingpane7" Title="Timestamp" runat="server" Width="500px"                   MinWidth="400">

                            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelGVTimeStamp" LoadingPanelID="RadAjaxLoadingPaneTimeStamp" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                              
                                <telerik:RadGrid ID="RadgvTimeStmp" runat="server" PageSize="100" AllowPaging="true"
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

                                                    
                                                        <table class="table">
                                                            <tr>

                                                                <td  class="width-r20" style="width:200px!important">    <b>TicketID# :</b>
                                                        <%# Eval("id") %>      </td>

                                                               <td class="width-r20" style="width:200px!important">       <b>Status :</b>
                                                        <%# Eval("assignname") %>      </td> 

                                                                <td class="width-r20" style="width:200px!important">        <b>Assigned to :</b>
                                                        <%# Eval("dwork") %>     </td> 

                                                            </tr>   
                                                            

<%--                                                                <td  style="width:200px!important">     <b>Assigned calls :</b>
                                                        <%# Eval("address") %>      </td>--%>
                                                             
 

                                                              <tr>

                                                                <td class="width-r20" style="width:200px!important">      <b>Address   :</b>
                                                        <%# String.Format("{0}, {1}", Eval("ldesc3"), Eval("city")) %>       </td>

                                                               <td class="width-r20" style="width:200px!important">      <b>Location  :</b>
                                                        <%# Eval("ldesc1") %>        </td> 

                                                                  <td class="width-r20"  style="width:200px!important">      <b>Time   :</b>
                                                        <%# Eval("EDate", "{0:t}") %>      </td> 

                                                            </tr>  


                                                              <tr>

                                                                <td class="width-r20" style="width:200px!important">       <b>On Site Time:</b>
                                                        <span>

                                                            <asp:Label ID="lblTimert" runat="server" Text='<%# Eval("timeroute", "{0:t}") %>'></asp:Label>
                                                            <asp:Label ID="lblAddressOR" runat="server" CssClass="hoverGrid shadow   roundCorner" Text='<%# Eval("AddressER") %>'></asp:Label>
                                                            <asp:HoverMenuExtender ID="hmeAddressor" runat="server" OffsetY="15" OffsetX="80"
                                                                PopupControlID="lblAddressOR" TargetControlID="lblTimert">
                                                            </asp:HoverMenuExtender>
                                                        </span>      </td>

                                                               <td class="width-r20"  style="width:200px!important">     <b>Complete Time:</b>

                                                        <span>
                                                            <asp:Label ID="lblTimesite" runat="server" Text='<%# Eval("timesite", "{0:t}") %>'></asp:Label>
                                                            <asp:Label ID="lblAddressOS" runat="server" CssClass="hoverGrid shadow  roundCorner" Text='<%# Eval("AddressOS") %>'></asp:Label>
                                                            <asp:HoverMenuExtender ID="hmeAddressOS" runat="server" OffsetY="15" OffsetX="80"
                                                                PopupControlID="lblAddressOS" TargetControlID="lblTimesite">
                                                            </asp:HoverMenuExtender>

                                                        </span>      </td> 
  <td  style="width:200px!important">     <b>Enroute Time</b>

                                                        <span>
                                                            <asp:Label ID="lblTimecomp" runat="server" Text='<%# Eval("timecomp", "{0:t}") %>'></asp:Label>
                                                            <asp:Label ID="lblAddressCM" runat="server" CssClass="hoverGrid shadow  roundCorner" Text='<%# Eval("AddressCM") %>'></asp:Label>
                                                            <asp:HoverMenuExtender ID="hmeAddressCM" runat="server" OffsetY="15" OffsetX="80"
                                                                PopupControlID="lblAddressCM" TargetControlID="lblTimecomp">
                                                            </asp:HoverMenuExtender>
                                                        </span>      </td>

                                                            </tr>  
                                                             

                                                              <tr>

                                                                <td class="width-r20" style="width:200px!important">      <b>Dist. ER-OS (Miles):</b>
                                                        <span>
                                                            <asp:Label ID="lbldistENOS" runat="server" Text='<%# Bind("distanceEROS") %>'></asp:Label></span>     </td>

                                                               <td  class="width-r20" style="width:200px!important">     <b>Dist. CO-Next ER (Miles):</b>
                                                        <span>
                                                            <asp:Label ID="lbldistCOER" runat="server" Text='<%# Bind("distanceCOER") %>'></asp:Label></span>      </td> 


                                                                  <td class="width-r20" style="width:200px!important">      <b>Total Dist. (Miles):</b>
                                                        <span>
                                                            <asp:Label ID="lbldisttot" runat="server">0</asp:Label>
                                                        </span>      </td> 

                                                            </tr>  

                                                            </table>

                                                   

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

          

           

        </div>
    </div>
    <%--  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>--%>
    <script src="Scripts/jquery/jquery-1.10.2.min.js"></script>
    <%--Add code for google Map by Shyam Start--%>
    <script src="Design/js/UI/jquery-ui.js"></script>
    <script src="Design/js/materialize-plugins/forms.js"></script>
    <script src="Design/js/Notifyjs/jquery.noty.js"></script>
    <script src="Design/js/Notifyjs/themes/default.js"></script>
    <script src="Design/js/Notifyjs/layouts/topCenter.js"></script>
    <%--<script  src="http://maps.googleapis.com/maps/api/js?sensor=AIzaSyCndNAw_XYuJaz2SLtNd40zaVw8e2S8N2Q"></script>--%>
    <script src="https://maps.googleapis.com/maps/api/js?sensor=false&key=AIzaSyCndNAw_XYuJaz2SLtNd40zaVw8e2S8N2Q"></script>

    <script type="text/javascript">

        var varMapsAPIKey = '<%=ConfigurationManager.AppSettings["MapsAPIKey"].ToString() %>'
        var currentlocarray = [];
        var bikearray = [];
        var from_lat, from_long, to_lat, to_long, from_date, to_date;
        var latlngarr = [];
        var eroroctarr = [];
        var callonarr = [];
        var bikePath, map, lineSymbol;
        var geocoder;
        var varLatitude = parseFloat(39.0917394);
        var varLongitude = parseFloat(-94.5828553);
        var openticketarr = [];
        var opencallmapscreenarr = [];
        var mapVar;
        var varInterval;
        var chkLiveData = false;
        var markersArray = [];
        var line = [];

        function ClearMap() {
            flightPath.setMap(null);

        }

        function chkLivDataCheckedCngd(sender, args) {
            chkLiveData = args.get_checked();
            if (chkLiveData == true) {
                //latlngarr = [];
                //removeLines();
                //clearOverlays();
                varInterval = setInterval(getLatestData, 7000);
            }
            else {
                //alert('call1');
                //latlngarr = [];
                //removeLines();
                //clearOverlays();

                clearInterval(varInterval);
                if (gCallGetJson == true) {
                    //alert('call2');
                    // getLatestData();
                }
            }
        }

        function chkOpencallsCheckedCngd(sender, args) {
            //console.log(args.get_checked());
            var chkOpencalls = args.get_checked();
            if (chkLiveData != true) {
                if (chkOpencalls == true) {
                    //  getLatestData();
                    debugger;
                    drawPathOpencalls();

                }
            }


        }

        function chkAssignedCallCheckedCngd(sender, args) {
            var chkAssignedCall = args.get_checked();
            //var chkLiveData = args.get_checked();
            if (chkLiveData != true) {
                if (chkAssignedCall == true) {
                    getLatestData();
                }
            }
        }



        function LoadMap() {
            eroroctarr = [];
            bikearray = [];
            latlngarr = [];
            removeLines();
            clearOverlays();

            $("#<%= btnSearch.ClientID %>").attr("disabled", true);

            if ($("#<%= ddlTech.ClientID %> option:selected").text() != 'Select') {

                var fUser = $("#<%= ddlTech.ClientID %> option:selected").text().trim();

                var date = $("#<%= txtDate.ClientID %>").val().trim();

                var _webconfig = $("#<%= hdnwebconfig.ClientID %>").val();


                // debugger;

                var cat = '';

                var checked_checkboxes = $("[id*=" + '<%=ddlCategory.ClientID%>' + "] input:checked"); //get all checked checkbox
                checked_checkboxes.each(function () {
                    cat += $(this).closest("li").find("label").text() + ',';

                });


                var checkBox = $find("<%=chkAssignedCall.ClientID%>");

                var isChecked = checkBox.get_checked();

                var iscall = '0'; if (isChecked) { iscall = '1'; }





                //////////////LOAD MAP DATA


                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "MapFill.asmx/GetMapData",

                    data: '{"fUser":"' + fUser + '","date":"' + date + '","cat":"' + cat + '","iscall":"' + iscall + '"}',
                    dataType: "json",
                    async: true,

                }).done(function (data) {
                    data.d = JSON.parse(data.d);
                    if (typeof data.d != 'undefined') {
                        if (data.d.length > 0) {
                            from_long = parseFloat(data.d[0].longitude);
                            from_lat = parseFloat(data.d[0].latitude);

                            localStorage.setItem("lastId", data.d[data.d.length - 1].ID);
                            for (var i = 0; i < data.d.length - 1; i++) {
                                lats = parseFloat(data.d[i].latitude);
                                longs = parseFloat(data.d[i].longitude);
                                MAPID = data.d[i].MAPID;
                                Name = data.d[i].Name;
                                var varDate = ConvertToJavaScriptDateTime(data.d[i].date);
                                latlngarr.push({ lat: parseFloat(lats), lng: parseFloat(longs), MAPID: MAPID, Name: Name, date: varDate, Cat: data.d[i].Cat });
                            }
                            to_lat = parseFloat(latlngarr[latlngarr.length - 1].lat);
                            to_long = parseFloat(latlngarr[latlngarr.length - 1].lng);
                            from_date = latlngarr[0].date;
                            to_date = latlngarr[latlngarr.length - 1].date;
                            eroroctarr = [];
                            for (var i = 0; i < data.d.length; i++) {
                                lats = data.d[i].latitude;
                                longs = data.d[i].longitude;
                                MAPID = data.d[i].MAPID;
                                Name = data.d[i].Name;
                                var varDate = ConvertToJavaScriptDateTime(data.d[i].date);
                                eroroctarr.push({ lat: parseFloat(lats), lng: parseFloat(longs), timestm: data.d[i].timestm, MAPID: MAPID, Name: Name, date: varDate, Cat: data.d[i].Cat, id: data.d[i].id });
                            }
                            var bikearray = [{ from_lat: from_lat, from_long: from_long, to_lat: to_lat, to_long: to_long, colour: "red", time: "12:00", fdate: from_date, tdate: to_date }];
                            var lastlatlong = { "lat": to_lat, "lng": to_long };
                            initMap(bikearray, parseFloat(to_lat), parseFloat(to_long), eroroctarr);
                            $("#<%= btnSearch.ClientID %>").attr("disabled", false);
                        }
                        else {
                            initMapDefault();
                            //$('.ajax-loader').css("visibility", "hidden");
                            $("#<%= btnSearch.ClientID %>").attr("disabled", false);
                        }
                    }
                    else {
                        initMapDefault();
                        // $('.ajax-loader').css("visibility", "hidden");
                        $("#<%= btnSearch.ClientID %>").attr("disabled", false);
                    }
                });
                if (gSelectCurrentDate == false) {
                    chkLiveData = false;
                    $('#<%=chkLiveData.ClientID%>').prop("disabled", true);
                    $('#<%=chkLiveData.ClientID%>').css("opacity", ".5");
                }
                else {
                    $('#<%=chkLiveData.ClientID%>').prop("disabled", false);
                    $('#<%=chkLiveData.ClientID%>').css("opacity", "");
                }
                return true;
            }
            else {
                var fUser = $("#<%= ddlTech.ClientID %> option:selected").text().trim();
                var date = $("#<%= txtDate.ClientID %>").val().trim();
                if (fUser == 'Select') {
                    noty({
                        text: "Please Select Worker.",
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 3000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                }
            }
        }

        function ConvertToJavaScriptDateTime(value) {
            var pattern = /Date\(([^)]+)\)/;
            var results = pattern.exec(value);
            if (results[1] != 'undefined') {
                var dt = new Date(parseFloat(results[1]));
                return ('0' + (dt.getMonth() + 1)).slice(-2) + "." + ('0' + dt.getDate()).slice(-2) + "." + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes();
            }
            else { return ''; }
        }

        function FormatJsonDate(jsonDt) {
            var MIN_DATE = -62135578800000; // const

            var date = new Date(parseInt(jsonDt.substr(6, jsonDt.length - 8)));
            return date.toString() == new Date(MIN_DATE).toString() ? "" : (date.getMonth() + 1) + "\\" + date.getDate() + "\\" + date.getFullYear();
        }

        function initMapDefault() {

            GetCurrentLocation();
            var map = new google.maps.Map(document.getElementById('divMap'), {
                center: { lat: parseFloat(varLatitude), lng: parseFloat(varLongitude) },
                zoom: 4
            });
            //console.log(currentlocarray);
            if (typeof currentlocarray != 'undefined') {
                if (currentlocarray.length > 0) {
                    for (i = 0; i < currentlocarray.length; i++) {
                        var locMarker;
                        var content = '';
                        var latitude = parseFloat(currentlocarray[i].latitude);
                        var longitude = parseFloat(currentlocarray[i].longitude);
                        var date = currentlocarray[i].date;
                        const event = new Date(date);
                        //console.log(event.toLocaleTimeString());

                        //var date = ConvertToJavaScriptDateTime(currentlocarray[i].date);
                        var address = currentlocarray[i].address;
                        var callsign = currentlocarray[i].callsign;
                        var GPS = currentlocarray[i].GPS;
                        content = "Position of " + callsign + " on " + date;
                        var icon;
                        if (GPS == '1') {
                            icon = {
                                url: "http://maps.google.com/mapfiles/ms/icons/green-dot.png", // url
                                scaledSize: new google.maps.Size(30, 30), // scaled size
                                //origin: new google.maps.Point(0, 0), // origin
                                //anchor: new google.maps.Point(0, 0) // anchor
                            };

                        }
                        var point = new google.maps.LatLng(latitude, longitude);
                        locMarker = new google.maps.Marker({
                            map: map,
                            position: point
                            //, icon: icon
                        });
                        var infowindow = new google.maps.InfoWindow()
                        google.maps.event.addListener(locMarker, 'click', (function (locMarker, content, infowindow) {
                            return function () {
                                infowindow.setContent(content);
                                infowindow.open(map, locMarker);
                            };
                        })(locMarker, content, infowindow));

                    }
                }
            }

        }

        function initMap(bikearray, to_lat, to_long, eroroctarr) {
            geocoder = new google.maps.Geocoder();
            map = new google.maps.Map(document.getElementById('divMap'), {
                zoom: 13,
                center: new google.maps.LatLng(to_lat, to_long),
                mapTypeId: google.maps.MapTypeId.ROADMAP,
            });
            lineSymbol = {
                path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
            };
            drawPath(bikearray, latlngarr, map, lineSymbol, eroroctarr);
        }

        // Removes the overlays from the map, but keeps them in the array
        function clearOverlays() {
            if (markersArray) {
                for (i in markersArray) {
                    markersArray[i].setMap(null);
                }
            }
        }

        function hideLines() {
            for (var z = 0; z < line.length; z++) {
                line[z].setVisible(false);
            }
        }

        function removeLines() {
            for (i = 0; i < line.length; i++) {
                line[i].setMap(null); //or 
                //line[i].setVisible(false);
                bikePath.setMap(null);
            }
        }

        function removebikePathLines() {

            for (i = 0; i < line.length; i++) {
                line[i].setMap(null); //or line[i].setVisible(false);
            }
            map.removeOverlay(polyline);
        }

        function codeAddress(address) {

            var testMarker;
            if (address != '') {


                var latAdd;
                var lngAdd;
                //var url = 'http://maps.googleapis.com/maps/api/geocode/json?callback=?&address=' + address + '&sensor=false&key=AIzaSyCndNAw_XYuJaz2SLtNd40zaVw8e2S8N2Q';
                var url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&sensor=false&key=" + varMapsAPIKey;
                $.getJSON(url, null, function (data) {
                    if (data.results.length > 0) {
                        latAdd = parseFloat(data.results[0].geometry.location.lat);
                        lngAdd = parseFloat(data.results[0].geometry.location.lng);
                        //console.log(data.results[0].geometry.location.lat);
                        //console.log(data.results[0].geometry.location.lng);
                    }
                    else
                        console.log('getJSON unsuccess');
                });


                if (latAdd != 'undefined' && lngAdd != 'undefined') {
                    var data = "<strong> Ticket #: " + 786 + "</strong></BR></BR> Category:" + "Repair" + " </BR></BR> <strong>" + "Jennie 2" + "</strong></BR></BR> " + "909 Fulton St SE, Minneapolis, MN, 55455" + " </BR></BR>" + "2020-04-20 04:05";

                    var point = new google.maps.LatLng(parseFloat(44.9707763), parseFloat(-93.2248323));
                    var infowindow = new google.maps.InfoWindow({
                        content: data,
                    });
                    testMarker = new google.maps.Marker({
                        map: map,
                        position: point
                        , icon: "imagehandler.ashx?assign=U&catid=Repair"
                    });
                    google.maps.event.addListener(testMarker, 'click', function () {
                        infowindow.open(map, testMarker);
                    });

                }
            }


        }

        function drawPath(bikearray, latlngarr, map, lineSymbol, eroroctarr) {
            debugger;
            // console.log('drawPath');
            if (typeof bikearray != 'undefined') {
                if (bikearray.length > 0) {
                    clearOverlays();
                    var OpenAddress = '';
                    var chkOpencalls = $find("<%= chkOpencalls.ClientID %>");
                    var ischkOpencalls = chkOpencalls.get_checked();
                    if (ischkOpencalls) {
                        opencallmapscreenarr = [];
                        GetOpenCallsMapScreen();
                        //console.log(opencallmapscreenarr);
                        if (typeof opencallmapscreenarr != 'undefined') {
                            if (opencallmapscreenarr.length > 0) {

                                for (i = 0; i < opencallmapscreenarr.length; i++) {
                                    var callMarker;
                                    var content;
                                    var call_lat = parseFloat(opencallmapscreenarr[i].lat);
                                    var call_long = parseFloat(opencallmapscreenarr[i].lng);
                                    var cat = opencallmapscreenarr[i].cat;
                                    var IconStr = "imagehandler.ashx?catid=" + cat;
                                    var assignname = opencallmapscreenarr[i].assignname;
                                    var strIfno = '';
                                    //if (assignname.trim().toLowerCase() == 'assigned' || assignname.trim().toLowerCase() == 'un-Assigned'.trim().toLowerCase()) {
                                    if (assignname.trim().toLowerCase() == 'un-Assigned'.trim().toLowerCase()) {
                                        IconUrl = "imagehandler.ashx?assign=U&catid=" + cat;
                                        content = "<b>Ticket # " + opencallmapscreenarr[i].id + "</b> <br/> <img src='" + IconStr + "' /> Category:" + cat + " </BR></BR>" + opencallmapscreenarr[i].ldesc1 + "<BR/>" + opencallmapscreenarr[i].address;
                                    }
                                    else {
                                        IconUrl = "imagehandler.ashx?assign=A&catid=" + cat;
                                        content = "<b>Ticket # " + opencallmapscreenarr[i].id + "</b> - " + opencallmapscreenarr[i].dwork + " <br/> <img src='" + IconStr + "' /> Category:" + cat + " </BR></BR>" + opencallmapscreenarr[i].ldesc1 + "<BR/>" + opencallmapscreenarr[i].address;
                                    }
                                    var point = new google.maps.LatLng(call_lat, call_long);
                                    callMarker = new google.maps.Marker({
                                        map: map,
                                        position: point
                                        , icon: IconUrl

                                    });
                                    var infowindow = new google.maps.InfoWindow()
                                    google.maps.event.addListener(callMarker, 'click', (function (callMarker, content, infowindow) {
                                        return function () {
                                            infowindow.setContent(content);
                                            infowindow.open(map, callMarker);
                                        };
                                    })(callMarker, content, infowindow));

                                }
                                if (chkLiveData == false)
                                    map.zoom = 4;
                                else
                                    map.zoom = 15;
                            }
                        }
                    }
                    var openTicketMarker;
                    var chkAssignedCall = $find("<%= chkAssignedCall.ClientID %>");
                    var ischkAssignedCall = chkAssignedCall.get_checked();
                    if (ischkAssignedCall) {
                        openticketarr = [];
                        GetOpenTicket();
                        if (typeof openticketarr != 'undefined') {
                            if (openticketarr.length > 0) {
                                //debugger;
                                for (i = 0; i < openticketarr.length; i++) {
                                    var image = '';
                                    var add = openticketarr[i].address;
                                    var assign = openticketarr[i].assigned;
                                    var ldesc = openticketarr[i].ldesc1;
                                    var edate = ConvertToJavaScriptDateTime(openticketarr[i].edate);
                                    var edateDate = new Date(edate);
                                    var etime = edateDate.toLocaleTimeString();




                                    var TicketID = openticketarr[i].id;
                                    var Cat = openticketarr[i].cat;
                                    //var IconStr = "imagehandler.ashx?catid=" + Cat;
                                    //console.log(Cat);
                                    var IconStr = "imagehandler.ashx?assign=U&catid=" + Cat;
                                    if (assign == "1") {
                                        image = "/images/white.png";
                                    }
                                    if (assign == "2") {
                                        image = "/images/green.png";
                                    }
                                    if (assign == "3") {
                                        image = "/images/orange.png";
                                    }
                                    if (assign == "4") {
                                        image = "/images/blue.png";
                                    }
                                    if (assign == "5") {
                                        image = "/images/yellow.png";
                                    }

                                    if (add != 'undefined' && add != '') {
                                        var latAdd;
                                        var lngAdd;
                                        var url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + add + "&sensor=false&key=" + varMapsAPIKey;

                                        $.ajax({
                                            url: url,
                                            type: 'GET',
                                            async: false,
                                            cache: false,
                                            success: function (data) {
                                                if (data.results.length > 0) {
                                                    latAdd = data.results[0].geometry.location.lat;
                                                    lngAdd = data.results[0].geometry.location.lng;

                                                }
                                            }
                                        });

                                        if (latAdd != 'undefined' && lngAdd != 'undefined') {
                                            var content = "<strong> Ticket #: " + TicketID + "</strong></BR></BR> Category:" + Cat + " </BR></BR> <strong>" + ldesc + "</strong></BR></BR> " + add + " </BR></BR>" + etime;




                                            var point = new google.maps.LatLng(parseFloat(latAdd), parseFloat(lngAdd));
                                            openTicketMarker = new google.maps.Marker({
                                                map: map,
                                                position: point
                                                //, icon: IconStr
                                                , icon: image
                                            });
                                            var infowindow = new google.maps.InfoWindow()
                                            google.maps.event.addListener(openTicketMarker, 'click', (function (openTicketMarker, content, infowindow) {
                                                return function () {
                                                    infowindow.setContent(content);
                                                    infowindow.open(map, openTicketMarker);
                                                };
                                            })(openTicketMarker, content, infowindow));

                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (i = 0; i < bikearray.length; i++) {
                        var from_lat = parseFloat(bikearray[i].from_lat);
                        var from_long = parseFloat(bikearray[i].from_long);
                        var startMarker, endMarker, activityMarker, arrowMarker;


                        //Start Marker Start
                        var datetimeStart = new Date(bikearray[0].fdate);
                        var timeStart = "Starting point - " + datetimeStart.toLocaleTimeString();
                        startMarker = new google.maps.Marker({
                            map: map,
                            position: { lat: from_lat, lng: from_long }
                            , text: timeStart
                            , title: timeStart
                            , icon: 'https://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=S|0099FF|FFFFFF|FF0000'
                        });
                        var infowindowStart = new google.maps.InfoWindow()
                        google.maps.event.addListener(startMarker, 'click', (function (startMarker, timeStart, infowindowStart) {
                            return function () {
                                infowindowStart.setContent(timeStart);
                                infowindowStart.open(map, startMarker);
                            };
                        })(startMarker, timeStart, infowindowStart));
                        //Start Marker End

                        //End Marker Start
                        var datetimeEnd = new Date(bikearray[0].tdate);
                        var timeEnd = "End point - " + datetimeEnd.toLocaleTimeString();
                        var to_lat, to_long;
                        to_lat = parseFloat(bikearray[i].to_lat);
                        to_long = parseFloat(bikearray[i].to_long);


                        endMarker = new google.maps.Marker({
                            map: map,
                            position: { lat: to_lat, lng: to_long }
                            , text: timeEnd
                            , title: timeEnd
                            , icon: 'https://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=E|0099FF|FFFFFF|FF0000'
                        });
                        var infowindowEnd = new google.maps.InfoWindow()
                        google.maps.event.addListener(endMarker, 'click', (function (endMarker, timeEnd, infowindowEnd) {
                            return function () {
                                infowindowEnd.setContent(timeEnd);
                                infowindowEnd.open(map, endMarker);
                            };
                        })(endMarker, timeEnd, infowindowEnd));
                        //End Marker End



                        markersArray.push(startMarker);
                        markersArray.push(endMarker);
                        map.setCenter({ lat: to_lat, lng: to_long });
                        //map.zoom = 15;
                        var distance = 0;
                        var linecolor = bikearray[i].colour;
                        if (typeof eroroctarr != 'undefined') {
                            for (var j = 0; j <= eroroctarr.length - 1; j++) {
                                if (typeof (eroroctarr[j].Name) == 'undefined') {
                                    eroroctarr[j].Name = '';
                                }
                                var varAdd = '';
                                if (eroroctarr[j].timestm == 1) {
                                    //get address by lat long start
                                    var url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + parseFloat(eroroctarr[j].lat) + "," + parseFloat(eroroctarr[j].lng) + "&key=" + varMapsAPIKey;
                                    $.ajax({
                                        url: url,
                                        type: 'GET',
                                        async: false,
                                        cache: false,
                                        success: function (data) {
                                            if (data.results.length > 0) {
                                                varAdd = data.results[0].formatted_address;
                                            }
                                        }
                                    });
                                    var cat = eroroctarr[j].Cat;
                                    image = 'https://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=CT|0947F2|000';
                                    var point = new google.maps.LatLng(parseFloat(eroroctarr[j].lat), parseFloat(eroroctarr[j].lng));
                                    var IconStr = "imagehandler.ashx?catid=" + cat;
                                    var content = "<strong> Ticket #: " + eroroctarr[j].id + "</strong></BR><img src='" + IconStr + "' /></BR><strong> Name: " + eroroctarr[j].Name + "</strong> </BR>" + "Current Address: " + varAdd + " </BR>" + eroroctarr[j].date;
                                    activityMarker = new google.maps.Marker({
                                        map: map,
                                        position: point
                                        , icon: image
                                    });
                                    var infowindow = new google.maps.InfoWindow()
                                    google.maps.event.addListener(activityMarker, 'click', (function (activityMarker, content, infowindow) {
                                        return function () {
                                            infowindow.setContent(content);
                                            infowindow.open(map, activityMarker);
                                        };
                                    })(activityMarker, content, infowindow));
                                }
                                if (eroroctarr[j].timestm == 2) {
                                    //get address by lat long start
                                    var url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + parseFloat(eroroctarr[j].lat) + "," + parseFloat(eroroctarr[j].lng) + "&key=" + varMapsAPIKey;
                                    $.ajax({
                                        url: url,
                                        type: 'GET',
                                        async: false,
                                        cache: false,
                                        success: function (data) {
                                            if (data.results.length > 0) {
                                                varAdd = data.results[0].formatted_address;
                                            }
                                        }
                                    });
                                    var cat = eroroctarr[j].Cat;
                                    //get address by lat long end
                                    image = 'https://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=OS|ff8000|000';
                                    var point = new google.maps.LatLng(parseFloat(eroroctarr[j].lat), parseFloat(eroroctarr[j].lng));
                                    var IconStr = "imagehandler.ashx?catid=" + cat;
                                    var content = "<strong> Ticket #: " + eroroctarr[j].id + "</strong></BR><img src='" + IconStr + "' /></BR></BR><strong> Name: " + eroroctarr[j].Name + "</strong> </BR></BR>" + "Current Address: " + varAdd + " </BR></BR>" + eroroctarr[j].date;
                                    activityMarker = new google.maps.Marker({
                                        map: map,
                                        position: point
                                        , icon: image
                                    });
                                    var infowindow = new google.maps.InfoWindow()
                                    google.maps.event.addListener(activityMarker, 'click', (function (activityMarker, content, infowindow) {
                                        return function () {
                                            infowindow.setContent(content);
                                            infowindow.open(map, activityMarker);
                                        };
                                    })(activityMarker, content, infowindow));
                                }
                                if (eroroctarr[j].timestm == 3) {
                                    //get address by lat long start
                                    var url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + eroroctarr[j].lat + "," + eroroctarr[j].lng + "&key=" + varMapsAPIKey;
                                    $.ajax({
                                        url: url,
                                        type: 'GET',
                                        async: false,
                                        cache: false,
                                        success: function (data) {
                                            if (data.results.length > 0) {
                                                varAdd = data.results[0].formatted_address;
                                            }
                                        }
                                    });
                                    var cat = eroroctarr[j].Cat;
                                    //get address by lat long end
                                    image = 'https://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=ER|339933|000';
                                    var point = new google.maps.LatLng(parseFloat(eroroctarr[j].lat), parseFloat(eroroctarr[j].lng));
                                    var IconStr = "imagehandler.ashx?catid=" + cat;
                                    var content = "<strong> Ticket #: " + eroroctarr[j].id + "</strong></BR><img src='" + IconStr + "' /></BR></BR><strong> Name: " + eroroctarr[j].Name + "</strong> </BR></BR>" + "Current Address: " + varAdd + " </BR></BR>" + eroroctarr[j].date;
                                    activityMarker = new google.maps.Marker({
                                        map: map,
                                        position: point
                                        , icon: image
                                    });
                                    var infowindow = new google.maps.InfoWindow()
                                    google.maps.event.addListener(activityMarker, 'click', (function (activityMarker, content, infowindow) {
                                        return function () {
                                            infowindow.setContent(content);
                                            infowindow.open(map, activityMarker);
                                        };
                                    })(activityMarker, content, infowindow));
                                }


                                if (parseInt(j) != parseInt(eroroctarr.length - 1)) {
                                    var latdistance = parseFloat(Distance(eroroctarr[j].lat, eroroctarr[j].lng, eroroctarr[j + 1].lat, eroroctarr[j + 1].lng));
                                    distance += latdistance;
                                    if (parseInt(eroroctarr[j].MAPID) == 0 || distance >= 2.5)//0.05
                                    {
                                        //debugger;
                                        distance = 0;
                                        var p1 = parseFloat(eroroctarr[j].lat);
                                        var p2 = parseFloat(eroroctarr[j].lng);
                                        var p3 = parseFloat(eroroctarr[j + 1].lat);
                                        var p4 = parseFloat(eroroctarr[j + 1].lng);

                                        var dir = bearing(p1, p2, p3, p4);
                                        // == round it to a multiple of 3 and cast out 120s
                                        dir = parseFloat(Math.round(dir / 3) * 3);
                                        while (dir >= 120) { dir -= 120; }
                                        var info = eroroctarr[j].date;
                                        var IconUrl = "http://www.google.com/intl/en_ALL/mapfiles/dir_" + dir + ".png";
                                        var datetime = new Date(info);
                                        var time = datetime.toLocaleTimeString();
                                        ////arrowMarker
                                        var p5Lat = parseFloat((p1 + p3) / 2);
                                        var p5Lng = parseFloat((p2 + p4) / 2);
                                        //console.log(IconUrl);
                                        //console.log(p5Lat);
                                        //console.log(p5Lng);
                                        var iconImage = {
                                            url: IconUrl,
                                            scaledSize: new google.maps.Size(16, 16),
                                            anchor: new google.maps.Point(8, 8)

                                        };
                                        var point = new google.maps.LatLng(parseFloat(p5Lat), parseFloat(p5Lng));
                                        arrowMarker = new google.maps.Marker({
                                            map: map,
                                            position: point
                                            , icon: iconImage
                                            , text: time
                                            , title: time
                                            // , icon: { url: IconUrl, scaledSize: new google.maps.Size(16, 16), anchor: new google.maps.Point(8, 8) },
                                            //  , Size : new google.maps.Size(16, 16)
                                        });
                                        var infowindow = new google.maps.InfoWindow()
                                        google.maps.event.addListener(arrowMarker, 'click', (function (arrowMarker, time, infowindow) {
                                            return function () {
                                                infowindow.setContent(time);
                                                infowindow.open(map, arrowMarker);
                                            };
                                        })(arrowMarker, time, infowindow));
                                        markersArray.push(arrowMarker);
                                    }
                                }
                            }
                        }
                        //bikePath = '';

                        //bikePath.setMap(null);
                        bikePath = new google.maps.Polyline({
                            path: latlngarr,

                            geodesic: true,
                            strokeColor: linecolor,
                            strokeOpacity: 1.0,
                            strokeWeight: 2,
                            map: map
                        });
                        line.push(bikePath);
                        //bikePath.setMap(null);
                        removeLines();
                        bikePath.setMap(map);
                    }
                }
            }
        }

        function drawPathOpencalls() {


            removeLines();
            clearOverlays();


            var map = new google.maps.Map(document.getElementById('divMap'), {
                center: { lat: parseFloat(varLatitude), lng: parseFloat(varLongitude) },
                zoom: 4
            });

            debugger;
            console.log('drawPathOpencalls');
            {
                {

                    var OpenAddress = '';
                    var chkOpencalls = $find("<%= chkOpencalls.ClientID %>");
                    var ischkOpencalls = chkOpencalls.get_checked();
                    if (ischkOpencalls) {
                        opencallmapscreenarr = [];
                        GetOpenCallsMapScreen();
                        //console.log(opencallmapscreenarr);
                        if (typeof opencallmapscreenarr != 'undefined') {
                            if (opencallmapscreenarr.length > 0) {

                                for (i = 0; i < opencallmapscreenarr.length; i++) {
                                    var callMarker;
                                    var content;
                                    var call_lat = parseFloat(opencallmapscreenarr[i].lat);
                                    var call_long = parseFloat(opencallmapscreenarr[i].lng);
                                    var cat = opencallmapscreenarr[i].cat;
                                    var IconStr = "imagehandler.ashx?catid=" + cat;
                                    var assignname = opencallmapscreenarr[i].assignname;
                                    var strIfno = '';
                                    //if (assignname.trim().toLowerCase() == 'assigned' || assignname.trim().toLowerCase() == 'un-Assigned'.trim().toLowerCase()) {
                                    if (assignname.trim().toLowerCase() == 'un-Assigned'.trim().toLowerCase()) {
                                        IconUrl = "imagehandler.ashx?assign=U&catid=" + cat;
                                        content = "<b>Ticket # " + opencallmapscreenarr[i].id + "</b> <br/> <img src='" + IconStr + "' /> Category:" + cat + " </BR></BR>" + opencallmapscreenarr[i].ldesc1 + "<BR/>" + opencallmapscreenarr[i].address;
                                    }
                                    else {
                                        IconUrl = "imagehandler.ashx?assign=A&catid=" + cat;
                                        content = "<b>Ticket # " + opencallmapscreenarr[i].id + "</b> - " + opencallmapscreenarr[i].dwork + " <br/> <img src='" + IconStr + "' /> Category:" + cat + " </BR></BR>" + opencallmapscreenarr[i].ldesc1 + "<BR/>" + opencallmapscreenarr[i].address;
                                    }
                                    var point = new google.maps.LatLng(call_lat, call_long);
                                    callMarker = new google.maps.Marker({
                                        map: map,
                                        position: point
                                        , icon: IconUrl

                                    });
                                    var infowindow = new google.maps.InfoWindow()
                                    google.maps.event.addListener(callMarker, 'click', (function (callMarker, content, infowindow) {
                                        return function () {
                                            infowindow.setContent(content);
                                            infowindow.open(map, callMarker);
                                        };
                                    })(callMarker, content, infowindow));

                                }
                                if (chkLiveData == false)
                                    map.zoom = 4;
                                else
                                    map.zoom = 15;
                            }
                        }
                    }

                }
            }
        }


        function drawPathNearestWorker() {

            debugger;




            var jsonNearestWorker = $("#<%= hdnNearest.ClientID %>").val();





            removeLines();

            clearOverlays();


            var map = new google.maps.Map(document.getElementById('divMap'), {
                center: { lat: parseFloat(varLatitude), lng: parseFloat(varLongitude) },
                zoom: 4
            });

            debugger;
            console.log('drawPathOpencalls');
            {
                {


                    {
                        NWmapscreenarr = [];
                        NWmapscreenarr = JSON.parse(jsonNearestWorker);


                        if (typeof NWmapscreenarr != 'undefined') {
                            if (NWmapscreenarr.length > 0) {

                                for (i = 0; i < NWmapscreenarr.length; i++) {
                                    var callMarker;
                                    var content;
                                    var call_lat = parseFloat(NWmapscreenarr[i].lat);
                                    var call_long = parseFloat(NWmapscreenarr[i].lng);
                                    var assignname = NWmapscreenarr[i].assignname;
                                    var cat = NWmapscreenarr[i].cat;

                                    var IconStr = "";

                                    var strIfno = '';

                                    if (cat.trim().toLowerCase() == '1') {
                                        var assign = "U";

                                        if (assignname.trim().toLowerCase() == 'Assigned') { assign = "A"; }

                                        IconUrl = "imagehandler.ashx?assign=" + assign + "&catid=" + cat;
                                        content = " <b> Ticket# </b> " + NWmapscreenarr[i].dwork + "    <br/>   <b>  Address: </b> " + NWmapscreenarr[i].address + " </br>";
                                    }
                                    else {
                                        IconUrl = "imagehandler.ashx?assign=A&catid=" + cat;
                                        content = "<b> Work#</b> " + NWmapscreenarr[i].dwork + "    <br/>  <b>Distance:</b>" + NWmapscreenarr[i].dist + "  <br/>  <b>Time:</b>" + NWmapscreenarr[i].Time + "  <br/>  <b>Address:</b>" + NWmapscreenarr[i].address + " </br>";
                                    }
                                    var point = new google.maps.LatLng(call_lat, call_long);
                                    callMarker = new google.maps.Marker({
                                        map: map,
                                        position: point
                                        , icon: IconUrl

                                    });
                                    var infowindow = new google.maps.InfoWindow()
                                    google.maps.event.addListener(callMarker, 'click', (function (callMarker, content, infowindow) {
                                        return function () {
                                            infowindow.setContent(content);
                                            infowindow.open(map, callMarker);
                                        };
                                    })(callMarker, content, infowindow));

                                }
                                if (chkLiveData == false)
                                    map.zoom = 4;
                                else
                                    map.zoom = 15;
                            }
                        }
                    }

                }
            }
        }

        function Distance(Lat1, Lon1, Lat2, Lon2) {
            var Latitude1 = parseFloat(Lat1);
            var Longitude1 = parseFloat(Lon1);
            var Latitude2 = parseFloat(Lat2);
            var Longitude2 = parseFloat(Lon2);

            var R = 6371;

            var dLat = this.toRadian(parseFloat(Latitude2) - parseFloat(Latitude1));
            var dLon = this.toRadian(parseFloat(Longitude2) - parseFloat(Longitude1));

            var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) + Math.cos(this.toRadian(Latitude1)) * Math.cos(this.toRadian(Latitude2)) * Math.sin(dLon / 2) * Math.sin(dLon / 2);

            var c = 2 * Math.asin(Math.min(1, Math.sqrt(a)));
            var d = R * c;
            return d;
        }

        function toRadian(val) {
            return (Math.PI / 180) * val;
        }

        function bearing(Alat1, Alon1, Alat2, Alon2) {
            var lat1 = ConvertToRadians(Alat1);
            var lon1 = ConvertToRadians(Alon1);
            var lat2 = ConvertToRadians(Alat2);
            var lon2 = ConvertToRadians(Alon2);

            var degreesPerRadian = parseFloat(180.0 / Math.PI);

            // Compute the angle.
            var angle = parseFloat(-Math.atan2(Math.sin(lon1 - lon2) * Math.cos(lat2), Math.cos(lat1) * Math.sin(lat2) - Math.sin(lat1) * Math.cos(lat2) * Math.cos(lon1 - lon2)));

            if (angle < 0.0) {
                angle += Math.PI * 2.0;
            }

            // And convert result to degrees.
            angle = parseFloat(angle * degreesPerRadian);
            //angle = angle.toFixed(1);

            return angle;
        }

        function ConvertToRadians(angle) {
            return parseFloat((Math.PI / 180) * angle);
        }

        function GetOpenCallsMapScreen() {


            // console.log('isChecked');
            var selectedValues = [];
            var fUser = '';
            var date = $("#<%= txtDate.ClientID %>").val().trim();
            var checked_checkboxes = $("[id*=" + '<%=ddlCategory.ClientID%>' + "] input:checked"); //get all checked checkbox
            checked_checkboxes.each(function () {
                var text = $(this).closest("li").find("label").text();
                selectedValues.push(text);
            });
            var selectedCate = '';
            if (selectedValues.length > 0)
                selectedCate = selectedValues.join();
            else
                var selectedCate = 'NoCat';
            if ($("#<%= ddlTech.ClientID %> option:selected").text() != 'Select') {
                fUser = $("#<%= ddlTech.ClientID %> option:selected").text().trim();
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "MapFill.asmx/GetOpenCallsMapScreen",
                data: '{"fUser":"' + fUser + '","date":"' + date + '","CategoryName":"' + selectedCate + '"}',
                dataType: "json",
                //async: true,
                async: false,
                cache: false,
            }).done(function (data) {
                opencallmapscreenarr = [];
                data.d = JSON.parse(data.d);
                opencallmapscreenarr = data.d;
            });
        }

        function GetOpenTicket() {
            if ($("#<%= ddlTech.ClientID %> option:selected").text() != 'Select') {
                var fUser = $("#<%= ddlTech.ClientID %> option:selected").text().trim();
                var date = $("#<%= txtDate.ClientID %>").val().trim();

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "MapFill.asmx/GetOpenTicket",
                    data: '{"fUser":"' + fUser + '","date":"' + date + '"}',
                    dataType: "json",
                    //async: true,
                    async: false,
                    cache: false,
                }).done(function (data) {
                    openticketarr = [];
                    data.d = JSON.parse(data.d);
                    openticketarr = data.d;
                });


            }

        }

        function GetCurrentLocation() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "MapFill.asmx/GetTechCurrentLocationNew",
                dataType: "json",
                async: false,
                cache: false,



            }).done(function (data) {
                currentlocarray = [];
                data.d = JSON.parse(data.d);
                currentlocarray = data.d;
                if (typeof currentlocarray != 'undefined') {
                    if (currentlocarray.length > 0) {
                        for (i = 0; i < currentlocarray.length; i++) {
                            varLatitude = parseFloat(currentlocarray[0].latitude);
                            varLongitude = parseFloat(currentlocarray[0].longitude);
                        }
                    }
                }
            });
        }

        function getLatestData() {

            debugger;

            var cat = '';

            var checked_checkboxes = $("[id*=" + '<%=ddlCategory.ClientID%>' + "] input:checked"); //get all checked checkbox
            checked_checkboxes.each(function () {
                cat += $(this).closest("li").find("label").text() + ',';

            });


            var checkBox = $find("<%=chkAssignedCall.ClientID%>");

            var isChecked = checkBox.get_checked();

            var iscall = '0'; if (isChecked) { iscall = '1'; }




         <%--   if ($("#<%= ddlTech.ClientID %> option:selected").text() != 'Select')--%>


            {
                var fUser = $("#<%= ddlTech.ClientID %> option:selected").text().trim();

                var date = $("#<%= txtDate.ClientID %>").val().trim();


                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "MapFill.asmx/GetMapData",
                    data: '{"fUser":"' + fUser + '","date":"' + date + '","cat":"' + cat + '","iscall":"' + iscall + '"}',
                    dataType: "json",
                    async: true,
                }).done(function (data) {

                    debugger;
                    data.d = JSON.parse(data.d);
                    if (typeof data.d != 'undefined') {
                        if (data.d.length > 0) {
                            from_lat = parseFloat(data.d[0].latitude);
                            from_long = parseFloat(data.d[0].longitude);
                            to_lat = parseFloat(data.d[data.d.length - 1].latitude);
                            to_long = parseFloat(data.d[data.d.length - 1].longitude);
                            from_date = ConvertToJavaScriptDateTime(data.d[0].date);
                            to_date = ConvertToJavaScriptDateTime(data.d[data.d.length - 1].date);
                            var llt = to_lat;
                            var llng = to_long;
                            var llng = to_long;

                            var path = bikePath.getPath();
                            path.push(new google.maps.LatLng(llt, llng));
                            bikePath.setPath(null);
                            bikePath.setPath(path);
                            latlngarr.push({ lat: llt, lng: llng });

                            eroroctarr = [];
                            for (var i = 0; i < data.d.length; i++) {
                                var varDate = '';

                                //console.log(data.d);
                                varDate = ConvertToJavaScriptDateTime(data.d[i].date);
                                eroroctarr.push({ lat: data.d[i].latitude, lng: data.d[i].longitude, timestm: data.d[i].timestm, MAPID: data.d[i].MAPID, Name: data.d[i].Name, date: varDate, Cat: data.d[i].Cat, id: data.d[i].id });
                            }
                            bikearray = [{ from_lat: from_lat, from_long: from_long, to_lat: llt, to_long: llng, colour: "red", time: "01:00", fdate: from_date, tdate: to_date },


                            ]
                            debugger;
                            drawPath(bikearray, latlngarr, map, lineSymbol, eroroctarr);
                        }

                    }

                });
            }
        }

        google.maps.event.addDomListener(window, "load", initMapDefault);

</script>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">

   

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

                var randomIdnk = $.now();

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
                    data: mainDataToPass,
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

            var myVar; var TO; var strtext;

            function pingResponse(deviceToken, randomIdnk, workername, _webconfig) {

                var tArr = {};
                tArr.workername = workername.trim();
                tArr.webconfig = _webconfig;
                tArr.Randomid = randomIdnk;
                var _tArr = JSON.stringify(tArr);
                TO = setTimeout(function () {
                    clearInterval(myVar);
                    failuremessage()
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
 
    
    <script type="text/javascript">
        var isHttps = false;
        $(document).ready(function () {
            //show current location on default map start
            //  GetCurrentLocation();
            if (window.location.href.startsWith("https"))
                isHttps = true;
            else
                isHttps = false;


            //show current location on default map end



            var datepicker = $find("<%= txtDate.ClientID %>");
            var selectedDate = datepicker.get_dateInput().get_selectedDate().format("yyyy/MM/dd");
            var todayDate = new Date();
            var dd = String(todayDate.getDate()).padStart(2, '0');
            var mm = String(todayDate.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = todayDate.getFullYear();
            todayDate = yyyy + '/' + mm + '/' + dd;
            if (selectedDate != todayDate) {
                chkLiveData = false;
                //alert('if');
                    //$('#<%=chkLiveData.ClientID%>').attr("disabled", true);
                $('#<%=chkLiveData.ClientID%>').prop("disabled", true);
                $('#<%=chkLiveData.ClientID%>').css("opacity", ".5");
            }
            else {
                //alert('else');
                    //$('#<%=chkLiveData.ClientID%>').attr("disabled", false);
                $('#<%=chkLiveData.ClientID%>').prop("disabled", false);
                $('#<%=chkLiveData.ClientID%>').css("opacity", "");
            }



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
