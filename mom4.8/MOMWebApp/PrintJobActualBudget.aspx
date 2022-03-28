<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="JobActualBudget" CodeBehind="PrintJobActualBudget.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="width=device-width, initial-scale=1" name="viewport" />
    <meta content="" name="description" />
    <meta content="" name="author" />
    <link rel="shortcut icon" href="Appearance/images/fevicon.png" type="image/x-icon">
    <link rel="icon" href="Appearance/images/fevicon.png" type="image/x-icon">
    <title>Mobile Office Manager 4.0</title>
    <%-- <link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&subset=all" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?libraries=places&key=AIzaSyCndNAw_XYuJaz2SLtNd40zaVw8e2S8N2Q"></script>
    <link href="Appearance/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/simple-line-icons.min.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/uniform.default.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/bootstrap-switch.min.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/daterangepicker-bs3.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/fullcalendar.min.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/tasks.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/components.css" id="style_components" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/plugins.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/layout.css?v=1.0" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/darkblue.css" rel="stylesheet" type="text/css" id="style_color" />
    <link href="Appearance/css/custom.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/main.css" rel="stylesheet" />
    <link href="Appearance/css/style.css?v=1.2" rel="stylesheet" type="text/css" />
    <link type="text/css" href="css/smoothness/jquery-ui-1.8.17.custom.css" rel="stylesheet" />
    <link href="Appearance/css/animsition.min.css" rel="stylesheet" />

    <script src="Appearance/js/jquery.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/ui/jquery-ui-1.8.17.custom.min.js"></script>
    <script type="text/javascript" src="js/ui/jquery.ui.autocomplete.min.js"></script>
    <script type="text/javascript" src="js/RowClick.js"></script>
    <script type="text/javascript" src="js/Notifyjs/jquery.noty.js"></script>
    <script type="text/javascript" src="js/Notifyjs/themes/default.js"></script>
    <script type="text/javascript" src="js/Notifyjs/layouts/topCenter.js"></script>
    <script type="text/javascript" src="js/jquery.simpleWeather.js"></script>
    <script type="text/javascript" src="js/jquery.simpleWeather.min.js"></script>
    <link rel="shortcut icon" href="favicon.ico" />
    <style type="text/css">
        .modal-backdrop {
            z-index: 1 !important;
        }

        .modal-dialog {
            margin-top: 150px !important;
        }

        .modal-title {
            color: #00476D !important;
        }

        .modal-header {
            padding: 12px 12px 6px 12px !important;
        }

        .modal-content {
            border-radius: 10px !important;
        }

        .modal-header {
            border-bottom: 1px solid #ccc !important;
        }

        .modal-btn-sbmt {
            background-color: #117DB4 !important;
            border: 1px solid #117DB4 !important;
            color: #fff !important;
            transition: all 0.3s ease !important;
        }

            .modal-btn-sbmt:hover {
                background-color: #117DB4 !important;
                border: 1px solid #117DB4 !important;
                color: #fff !important;
                background-color: #054E71 !important;
            }

        .btn-info {
            background-color: #0C6A99 !important;
            padding: 4px 4px 4px 4px !important;
            font-size: 0.9em !important;
            border: 1px solid #0C6A99 !important;
            width: 100% !important;
            text-align: center !important;
        }

            .btn-info:hover {
                background-color: #054E71 !important;
                padding: 4px 4px 4px 4px !important;
                font-size: 0.9em !important;
                border: 1px solid #0C6A99 !important;
                width: 100% !important;
                text-align: center !important;
            }

        .sl-cmp {
            float: left;
            clear: left;
            width: 100%;
            padding-bottom: 8px !important;
            margin-bottom: 7px !important;
            border-bottom: 1px solid #ddd;
        }

        .modal-footer {
            border-top: none;
        }


        .sl-cmp:last-child {
            border-bottom: none;
        }

        .sl-cmp-spn1 {
            float: left;
            margin-right: 5px;
        }

        .sl-cmp-spn2 {
            float: left;
            padding-top: 2px;
        }

        .modal-body {
            padding: 12px !important;
        }

        .ui-autocomplete-loading {
            background: white url('images/autocomp.gif') right center no-repeat;
        }

        #headerfix {
            /*position: fixed;
            top: 0;
            left: 0;
            z-index:9999;  
            opacity:0.95;
            filter:alpha(opacity=95);   
            */
            width: 100%;
            background: url('images/header_bg.png') repeat-x scroll 0 0 #fff;
        }

        .grayscales {
            filter: gray; /* IE6-9 */
            filter: grayscale(1); /* Firefox 35+ */
            -webkit-filter: grayscale(1); /* Google Chrome, Safari 6+ & Opera 15+ */
        }
        /*#hover-content {
            display:none; 
        }
    
        #hover-me:hover #hover-content {
            display:block;
        }*/
        #hover-me {
            float: right;
        }

        #hover-content {
            opacity: 0;
            -webkit-transition: .5s;
            -moz-transition: .5s;
            -o-transition: .5s;
            -ms-transition: .5s;
            transition: .5s;
        }

        #hover-me:hover #hover-content {
            opacity: 1;
        }

        .pnlUpdateoverlay {
            /*background-color: #000000;*/
            background-color: transparent;
            height: 100%;
            width: 100%;
            position: fixed;
            top: 0px;
            z-index: 10010;
            filter: alpha(opacity=50);
            -moz-opacity: 0.50;
            opacity: 0.50;
        }


        .pnlUpdate {
            top: 28%;
            position: fixed; /*width: 500px;*/
            z-index: 10011;
            background: #fff;
            border: solid;
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

        .menu_popup_grid {
            background: none repeat scroll 0 0 transparent;
            display: none;
            max-height: 260px;
            min-height: 10px;
            overflow: auto;
            position: absolute; /* right: 603px;     top: 313px;     width: 633px;     border: 1px solid black;*/
            z-index: 1;
            overflow-x: hidden;
        }

        .roundCorner {
            border: 1px solid #ccc;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
        }

        /* Company Dropdown CSS */

        .desc {
            color: #6b6b6b;
        }

            .desc a {
                color: #0092dd;
            }

        .dropdown dd, .dropdown dt, .dropdown ul {
            margin: 0px;
            padding: 0px;
        }

        .dropdown dd {
            position: relative;
        }

        .dropdown a, .dropdown a:visited {
            color: #816c5b;
            text-decoration: none;
            outline: none;
        }

            .dropdown a:hover {
                color: #5d4617;
            }

        .dropdown dt a:hover {
            color: #5d4617;
            border: 1px solid #d0c9af;
        }

        .dropdown dt a {
            background: #0C6A99 url(arrow.png) no-repeat scroll right center;
            display: block;
            border: 1px solid #0C6A99;
            width: 115px;
            color: #fff !important;
            font-size: 0.9em !important;
        }

            .dropdown dt a:hover {
                background: #00476D url(arrow.png) no-repeat scroll right center;
                display: block;
                border: 1px solid #0C6A99;
                width: 115px;
                color: #fff !important;
                font-size: 0.9em !important;
            }

            .dropdown dt a span {
                cursor: pointer;
                display: block;
                padding: 8px;
                text-align: center !important;
            }

        .dropdown dd ul {
            background: #0C6A99 none repeat scroll 0 0;
            border: 1px solid #0C6A99;
            color: #fff;
            display: none;
            left: 118px;
            padding: 0 0px;
            position: absolute;
            top: -32px;
            width: auto;
            min-width: 170px;
            list-style: none;
        }

        .dropdown span.value {
            display: none;
        }

        .dropdown dd ul li a {
            padding: 5px;
            display: block;
            color: #fff !important;
        }

            .dropdown dd ul li a:hover {
                background-color: #00476D;
                color: #fff !important;
                padding-left: 10px !important;
                transition: all 0.3s ease;
            }

        .dropdown img.flag {
            border: none;
            vertical-align: middle;
            margin-left: 10px;
        }

        .flagvisibility {
            display: none;
        }
    </style>

    <script type="text/javascript">      
        function SelectAllCheckboxes(chkboxBranchSelectAll) {
            //debugger;;
            var gvDisplayCart = document.getElementById("ctl00_gvCompany");
            var inputList = gvDisplayCart.getElementsByTagName("input");
            $('td input:checkbox', gvDisplayCart).prop('checked', chkboxBranchSelectAll.checked);
            var numChecked = 0;
            for (var i = 0; i < inputList.length; i++) {
                if (inputList[i].type == "checkbox") {
                    if (chkboxBranchSelectAll.checked == true) {
                        numChecked = numChecked + 1;
                        inputList[i].parentNode.parentNode.style.backgroundColor = "#01A1DC";
                    }
                    else {
                        inputList[i].parentNode.parentNode.style.backgroundColor = "";
                    }
                }
            }
            document.getElementById("ctl00_lblRecordCount").innerHTML = "(" + numChecked + ")" + "Company(s) Selected";
        }

        function CheckBoxCount(chkSelect) {
            var gv = document.getElementById("ctl00_gvCompany");
            var inputList = gv.getElementsByTagName("input");
            var numChecked = 0;
            if (inputList.length > 0) {
                for (var i = 0; i < inputList.length; i++) {
                    if (inputList[i].type == "checkbox" && inputList[i].checked) {
                        numChecked = numChecked + 1;
                    }
                    if (chkSelect.checked)
                        chkSelect.parentNode.parentNode.style.backgroundColor = "#01A1DC";
                    else
                        chkSelect.parentNode.parentNode.style.backgroundColor = "";
                }
            }
            else {
                numChecked = 0;
            }
            document.getElementById("ctl00_lblRecordCount").innerHTML = "(" + numChecked + ")" + "Company(s) Selected";
        }
        function RadioCheck(rb) {
            var gv = document.getElementById("ctl00_gvCompany");
            var rbs = gv.getElementsByTagName("input");
            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">      
        $(document).ready(function () {
            // Docs at http://simpleweatherjs.com                  
            $('[id*=chkboxSelectAll]').click(function () {
                $("[id*='chkSelect']").attr('checked', this.checked);
            });
            /* Does your browser support geolocation? */
            if ("geolocation" in navigator) {
                $('.js-geolocation').show();
            } else {
                $('.js-geolocation').hide();
            }

            /* Where in the world are you? */

            navigator.geolocation.getCurrentPosition(function (position) {
                loadWeather(position.coords.latitude + ',' + position.coords.longitude); //load weather using your lat/lng coordinates

                function loadWeather(location, woeid) {
                    $.simpleWeather({
                        location: location,
                        woeid: woeid,
                        unit: 'c',
                        success: function (weather) {
                            html = '<i class="icon-' + weather.code + '"></i> ' + weather.temp + '&deg;' + weather.units.temp;

                            $("#temperature").html(html);
                        },
                        error: function (error) {
                            $("#temperature").text("20° C")
                            //$("#temperature").html('<p>' + error + '</p>');
                        }
                    });
                }
            });

            if ($(window).width() <= 767) {

                $("#id-1").click(function () {
                    $("#id-2").toggle('slow');
                });

                $("#id-3").click(function () {
                    $("#id-4").toggle('slow');
                });

                $("#id-1").show(); $("#id-2").hide();

            }
            else {
                $("#id-1").hide(); $("#id-2").show();

            }
        });


    </script>
    <script type="text/javascript"> 
        $(document).ready(function () {
            function executeQuery() {
                $.ajax({
                    url: 'CustomerAuto.asmx/GetRequestForServiceCall',
                    type: 'post',
                    cache: false,
                    dataType: 'text',
                    contentType: "application/json",
                    data: "",
                    success: function (data) {
                        var obj = jQuery.parseJSON(data);
                        //console.log(obj.d); 
                        if (parseInt(obj.d) == 0) {
                            $('#aresults').hide();
                        }
                        else {
                            $('#aresults').show();
                            $('#results').text(obj.d);
                        }
                    },
                    error: function (xhr, status) {

                    }
                });
                setTimeout(executeQuery, 5000); // you could choose not to continue on failure...
            }

            $(document).ready(function () {
                // run the first time; all subsequent calls will take care of themselves
                executeQuery();
            });
        });
    </script>

    <script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>

</head>

<body>
    <form id="form1" runat="server">

        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release"
            AsyncPostBackTimeout="360000">
        </asp:ToolkitScriptManager>
        <asp:Button ID="Button1" runat="server" Style="display: none;" Enabled="false" />
        <div class="main-content">

            <div id="page-wrapper">
                <div class="side-body padding-top">
                    <div class="page-content">
                        <div class="clearfix"></div>
                        <div class="row">

                            <!-- edit-tab start -->
                            <div class="col-lg-12 col-md-12">
                                <div class="com-cont">
                                    <div class="search-customer">
                                        <div class="sc-form">
                                            <label for="">
                                                Select for Projects where
                                            </label>

                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="form-control input-sm input-small" ClientIDMode="Static" Visible="False">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                <asp:ListItem Value="j.id">Project #</asp:ListItem>
                                                <asp:ListItem Value="j.fdate">Date</asp:ListItem>
                                                <asp:ListItem Value="l.tag">Location</asp:ListItem>
                                                <asp:ListItem Value="j.Status">Status</asp:ListItem>
                                                <asp:ListItem Value="j.fdesc">Description</asp:ListItem>
                                                <asp:ListItem Value="j.Owner">Customer</asp:ListItem>
                                            </asp:DropDownList>


                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-sm input-small hide"
                                                Width="200px" TabIndex="13" ClientIDMode="Static" Visible="False">
                                                <asp:ListItem Value="0">Active</asp:ListItem>
                                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:TextBox ID="txtInvDt" runat="server" CssClass="form-control input-sm input-small hide" ClientIDMode="Static" Visible="False"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtInvDt_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtInvDt">
                                            </asp:CalendarExtender>
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control input-sm input-small" ClientIDMode="Static" Visible="False"></asp:TextBox>

                                            <asp:DropDownList ID="ddlDateRange" runat="server" CssClass="form-control input-sm input-small "
                                                Style="width: 200px!important;" TabIndex="14" ClientIDMode="Static" Visible="False">
                                                <asp:ListItem Value="1">Cumulative</asp:ListItem>
                                                <asp:ListItem Value="2">Date Range - Activity</asp:ListItem>
                                                <asp:ListItem Value="3">Date Range - Closed</asp:ListItem>
                                                <asp:ListItem Value="4">Date Range - Created</asp:ListItem>
                                            </asp:DropDownList>


                                            <asp:Label ID="lblSdate" runat="server" Text="Date From" CssClass="hide" ClientIDMode="Static" />
                                            <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control input-sm input-small hide" MaxLength="50"
                                                Width="80px" ClientIDMode="Static" Visible="False">
                                            </asp:TextBox>
                                            <asp:CalendarExtender ID="txtfromDate_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtfromDate">
                                            </asp:CalendarExtender>

                                            <asp:Label ID="lblEdate" runat="server" Text="Date To" CssClass="hide" />
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control input-sm input-small hide" MaxLength="50"
                                                Width="80px" ClientIDMode="Static" Visible="False">
                                            </asp:TextBox>
                                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtToDate">
                                            </asp:CalendarExtender>
                                            <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server" OnClick="lnkSearch_Click" Visible="False"><i class="fa fa-search"></i></asp:LinkButton>

                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div>
                                        <asp:UpdatePanel ID="upJob" runat="server">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lnkSearch" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <div style="margin-left: 5px; margin-top: 5px;">
                                                    <rsweb:ReportViewer ID="rvJob" runat="server" Width="850px" Height="900px"
                                                        BorderColor="Gray" BorderStyle="None" BorderWidth="1px"
                                                        ShowZoomControl="False">
                                                    </rsweb:ReportViewer>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <!-- edit-tab end -->
                            <div class="clearfix"></div>
                        </div>
                        <!-- END DASHBOARD STATS -->
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>
        </div>

        <div class="clearfix"></div>


    </form>

    <script src="Appearance/js/Custom-validation.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery-migrate.min.js" type="text/javascript"></script>
    <script src="Appearance/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="Appearance/js/bootstrap-hover-dropdown.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script src="Appearance/js/moment.min.js" type="text/javascript"></script>
    <script src="Appearance/js/daterangepicker.js" type="text/javascript"></script>
    <script src="Appearance/js/fullcalendar.min.js" type="text/javascript"></script>
    <script src="Appearance/js/metronic.js" type="text/javascript"></script>
    <script src="Appearance/js/layout.js" type="text/javascript"></script>
    <script src="Appearance/js/quick-sidebar.js" type="text/javascript"></script>
    <script src="Appearance/js/demo.js" type="text/javascript"></script>
    <script src="Appearance/js/index.js" type="text/javascript"></script>
    <script src="Appearance/js/tasks.js" type="text/javascript"></script>
    <script src="Appearance/js/animsition.min.js"></script>


    <!-- END PAGE LEVEL SCRIPTS -->
    <script>

        //$(document).ready(function () {
        //    $('.animsition-overlay').animsition({
        //        inClass: 'overlay-slide-in-left',
        //        outClass: 'overlay-slide-out-left',
        //        overlay: true,
        //        overlayClass: 'animsition-overlay-slide',
        //        overlayParentElement: 'body'
        //    })

        <%-- if ($(window).width() > 780) {
                document.body.classList.add('page-sidebar-closed');
                document.getElementById('<%= toggleMenu.ClientID %>').classList.add('page-sidebar-menu-closed');
            }--%>
        //});

        //jQuery(document).ready(function () {
        //    Metronic.init(); // init metronic core componets
        //    Layout.init(); // init layout
        //    QuickSidebar.init(); // init quick sidebar
        //    ajax_validation.setImage();
        //    Demo.init(); // init demo features
        //    Index.init();
        //    Index.initDashboardDaterange();
        //    Index.initJQVMAP(); // init index page's custom scripts
        //    Index.initCalendar(); // init index page's custom scripts
        //    Index.initCharts(); // init index page's custom scripts
        //    Index.initChat();
        //    Index.initMiniCharts();
        //    Tasks.initDashboardWidget();
        //});
    </script>
    <!-- END JAVASCRIPTS -->
    <%-- <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
        ga('create', 'UA-37564768-1', 'keenthemes.com');
        ga('send', 'pageview');
    </script>--%>
    <script type="text/javascript" src="js/Signature/jquery.signaturepad.js"></script>

    <%-- <script type="text/javascript">
        ///////////// Signature box handling  ////////////////////
        // $(document).ready(function () {
        //    $('.sigPad').signaturePad();
        //});
    </script>--%>
    <script>
        jQuery.fn.extend({
            propAttr: $.fn.prop || $.fn.attr
        });
    </script>

    <%-- <script type="text/javascript">
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(success);
        }
        function success(position) {
            var city=position.coords.locality;
            };
    </script>--%>


    <%--<script type="text/javascript">
            // Docs at http://simpleweatherjs.com
        $(document).ready(function () {
            $.simpleWeather({
                woeid: '2357536', //2357536
                location: 'Montreal',
                unit: 'c',
                success: function (weather) {
                    if (weather.temp=""){
                        document.getElementById("temeperature").value=20 +'&deg;' + weather.units.temp
                    }
                   
                    else { document.getElementById("temeperature").value = weather.temp + '&deg;' + weather.units.temp }
                    }
                });
            });

        </script>--%>
    <%--<script src="dashboard/js/Chart.js"></script>--%>
    <%--<link href="dashboard/css/animate.css" rel="stylesheet" type="text/css" media="all">--%>

    <%--<script src="dashboard/js/wow.min.js"></script>
<script>
    new WOW().init();
	</script>--%>

    <%--<script src="dashboard/js/dcalendar.picker.js"></script>
<script>
    $('#demo').dcalendarpicker();
    $('#calendar-demo').dcalendar(); //creates the calendar
</script>--%>

    <%--<script type="text/javascript" src="dashboard/js/app.js"></script>
<script src="dashboard/js/jquery.nicescroll.js"></script>--%>
    <script src="dashboard/js/scripts.js"></script>
</body>
</html>
