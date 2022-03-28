<%@ Page Title="HomeKPI" Language="C#" MasterPageFile="~/MOMNew.master" AutoEventWireup="true" Inherits="HomeKPI" CodeBehind="HomeKPI.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="DashboardNew/js/bootstrap/bootstrap.bundle.js"></script>
    <link href="DashboardNew/css/bootstrap.min.css" rel="stylesheet" />
    <link href="DashboardNew/css/jquery-ui.min.css" rel="stylesheet" />
    <link href="DashboardNew/css/custom.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="DashboardNew/jvectormap/jquery-jvectormap.css" rel="stylesheet" />
    <link href="DashboardNew/assets/owl.carousel.min.css" rel="stylesheet" />
    <link href="DashboardNew/assets/owl.theme.default.min.css" rel="stylesheet" />
    <script src="DashboardNew/js/jquery.min.js"></script>

     <script src="DashboardNew/node_modules/chart.js/dist/Chart.bundle.min.js"></script>

        <!-- Vector Maps Libraries -->
    <script src="Design/js/jvectormap/jquery-jvectormap-1.2.2.min.js"></script>
    <script src="Design/js/jvectormap/jquery-jvectormap-world-mill-en.js"></script>
    <script src="Design/js/jvectormap/vectormap-script.js"></script>
    
	<script type="text/javascript" src="DashboardNew/daterangepicker/daterangepicker.js"></script>
    <link rel="stylesheet" type="text/css" href="DashboardNew/daterangepicker/daterangepicker.css" />
    <link href="DashboardNew/owl/dist/assets/owl.carousel.min.css" rel="stylesheet" />
    <%--<link href="Design/css/custom/custom.css" type="text/css" rel="stylesheet" media="screen,projection">--%>
    <!--            Sparkline Graphs
                <div id="sparklinedash">
                    <span class="bar"></span>
                </div> -->         
    
    <%--<style>
        .select-wrapper{
            display:none !important;
        }
    </style>--%>

    <!-- Universal Date Picker -->
    <div id="reportrange" class="datepick">
        <i class="mdi-action-today"></i>&nbsp;&nbsp;
        <span></span>
    </div>

                <!-- Web View Carousel -->

                <div class="overflow-none">
                <div class="owl-carousel owl-theme" id="carousel1">
                    <div class="item">
                        <div class="card bounce shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 75%"></div>
                                  </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">800 <span class="increase-tag"><i class="Small mdi-navigation-expand-less"></i> 26%</span></h6>
                                <p class="card-text box-content">Open Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #ffff1494;"><i class="Large mdi-editor-attach-money top-card-icon-color"></i>
                                    <!-- <i class="Small mdi-navigation-arrow-back"></i></span> -->
                                </span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 45%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">200</h6>
                                <p class="card-text box-content">Closed Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #0fff7885;"><i class="Large mdi-content-link top-card-icon-color"></i></span>
                                </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 15%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">11 <span class="decrease-tag"><i class="Small mdi-navigation-expand-more"></i> 11%</span></h6>
                                <p class="card-text box-content">Withdrawn Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #00d6e63d;"><i class="Large mdi-content-reply-all top-card-icon-color"></i></span>
                                </span>
                            </div>
                            </div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 35%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">15</h6>
                                <p class="card-text box-content">Cancelled Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #ff9c0942;"><i class="Large mdi-av-equalizer top-card-icon-color"></i></span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 10%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">5</h6>
                                <p class="card-text box-content">Disqualified Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #09a4ff42;"><i class="Large mdi-navigation-close top-card-icon-color"></i></span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 45%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">385</h6>
                                <p class="card-text box-content">Sold Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #09ffc642;"><i class="Large mdi-action-assignment-turned-in top-card-icon-color"></i></span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 65%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">508</h6>
                                <p class="card-text box-content">Quoted Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #fb09ff42;"><i class="Large mdi-action-book top-card-icon-color"></i></span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 85%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">998</h6>
                                <p class="card-text box-content">Total Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #fff70942;"><i class="Large mdi-av-my-library-books top-card-icon-color"></i></span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
                <!-- Mobile View Tiles -->
                <div class="row overflow-auto row1-mobile-view">
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3 col-xl-3 col-xxl-3 card-padding">
                        <!-- <span class="card card-top-border"></span> -->
                        <div class="card bounce shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 75%"></div>
                                  </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">800 <span class="increase-tag"><i class="Small mdi-navigation-expand-less"></i> 26%</span></h6>
                                <p class="card-text box-content">Open Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #ffff1494;"><i class="Large mdi-editor-attach-money top-card-icon-color"></i>
                                    <!-- <i class="Small mdi-navigation-arrow-back"></i></span> -->
                                </span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3 col-xl-3 col-xxl-3 card-padding">
                        <!-- <span class="card card-top-border"></span> -->
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 45%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">200</h6>
                                <p class="card-text box-content">Closed Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #0fff7885;"><i class="Large mdi-content-link top-card-icon-color"></i></span>
                                </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3 col-xl-3 col-xxl-3 card-padding">
                        <!-- <span class="card card-top-border"></span> -->
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 15%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">11 <span class="decrease-tag"><i class="Small mdi-navigation-expand-more"></i> 11%</span></h6>
                                <p class="card-text box-content">Withdrawn Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #00d6e63d;"><i class="Large mdi-content-reply-all top-card-icon-color"></i></span>
                                </span>
                            </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3 col-xl-3 col-xxl-3 card-padding">
                        <!-- <span class="card card-top-border"></span> -->
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 35%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">15</h6>
                                <p class="card-text box-content">Cancelled Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #ff9c0942;"><i class="Large mdi-av-equalizer top-card-icon-color"></i></span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                     <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3 col-xl-3 col-xxl-3 card-padding">
                        <!-- <span class="card card-top-border"></span> -->
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 10%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">5</h6>
                                <p class="card-text box-content">Disqualified Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #09a4ff42;"><i class="Large mdi-navigation-close top-card-icon-color"></i></span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3 col-xl-3 col-xxl-3 card-padding">
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 45%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">385</h6>
                                <p class="card-text box-content">Sold Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #09ffc642;"><i class="Large mdi-action-assignment-turned-in top-card-icon-color"></i></span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3 col-xl-3 col-xxl-3 card-padding">
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 65%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">508</h6>
                                <p class="card-text box-content">Quoted Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #fb09ff42;"><i class="Large mdi-action-book top-card-icon-color"></i></span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3 col-xl-3 col-xxl-3 card-padding">
                        <div class="card shadowz" style="margin-top: 0px;">
                            <div class="card-body crdback">
                                <div class="progress progress-bar-top">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 85%"></div>
                                </div>
                                <div class="row">
                                <span class="col-9">
                                <h6 class="card-title box-title">998</h6>
                                <p class="card-text box-content">Total Opportunities </p>
                                </span>
                                <span class="col-3">
                                <span class="card-icon top-card-icon-size" style="background: #fff70942;"><i class="Large mdi-av-my-library-books top-card-icon-color"></i></span>
                            </span>
                        </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Section Row 2 -->
                <!-- Web View -->
                <div class="overflow-none">
                    <div class="owl-carousel owl-theme" id="carousel2">
                        <div class="item">
                            <div class="card bx-shadow">
                                <div class="card-body card_back">
                                    <h6 class="card-title" style="margin-bottom: 10px;">Actual vs Budgeted Revenue
    
                                        <a href="#" ><img src="DashboardNew/images/download.png" class="download-btn" /></a>
                                        Select Budget:
                                        <select id="sltBudget" name="sltBudget" class="custom-select transparent classic select-box-style">
                                        <option value="1" class="txtclr">MAY</option>
                                        <option value="2" class="txtclr">APRIL</option>
                                        <option value="3" class="txtclr">MARCH</option>
                                        </select>
                                        <%--<select id="sltBudget" name="sltBudget" onChange="checkBudget();" class="custom-select transparent classic select-box-style">
                                        <option value="0" class="txtclr">Select Budget</option>                                        
                                        </select>--%>
                                    </h6>
                                    <%--<canvas id="myChart4"></canvas>--%>
                                    <canvas id="chartActualVsBudgetedRevenue"></canvas>
                                    

                                  
                                    <%--<div class="row card-bottom-text"><p class="card-text card-text2">Closed Won v/s Loss</p></div>--%>
                                </div>
                            </div>
                        </div>
                        <div class="item">
                            <div class="card bx-shadow" style="margin-bottom: 10px;">
                                <div class="card-body card_back">
                                    <h6 class="card-title" style="margin-bottom: 10px;">Equipment by Building
    
                                        <a href="#" ><img src="DashboardNew/images/download.png" class="download-btn"/></a>
                                        <select class="custom-select transparent classic select-box-style">
                                        <option value="1" class="txtclr">MAY</option>
                                        <option value="2" class="txtclr">APRIL</option>
                                        <option value="3" class="txtclr">MARCH</option>
                                    </select>
                                    </h6>
                                    <%--<canvas id="myChart3"></canvas>--%>
                                    <canvas id="chartEquipmentBuilding"></canvas>
                                    <div class="row card-bottom-text"><p class="card-text card-text2"></p></div>
                                </div>
                            </div>
                        </div>
                        <div class="item">
                            <div class="card bx-shadow">
                                <div class="card-body card_back">
                                    <h6 class="card-title" style="margin-bottom: 10px;">Equipment by Type
    
                                        <a href="#" ><img src="DashboardNew/images/download.png" class="download-btn" /></a>
                                        <select class="custom-select transparent classic select-box-style" >
                                        <option value="1" class="txtclr">MAY</option>
                                        <option value="2" class="txtclr">APRIL</option>
                                        <option value="3" class="txtclr">MARCH</option>
                                    </select>
                                    </h6>
                                    <%--<canvas id="myChart5"></canvas>--%>
                                    <canvas id="chartEquipmentbyType"></canvas>
                                    <div class="row card-bottom-text"><p class="card-text card-text2"></p></div>
                                </div>
                            </div>
                        </div>
                        <div class="item">
                            <div class="card bx-shadow" style="margin-bottom: 15px;">
                                <div class="card-body card_back">
                                    <h6 class="card-title" style="margin-bottom: 10px;">Estimates By Salesperson Avg. Days

    
                                    <a href="#" ><img src="DashboardNew/images/download.png" class="download-btn" /></a>
                                    <select class="custom-select transparent classic select-box-style">
                                        <option value="1" class="txtclr">MAY</option>
                                        <option value="2" class="txtclr">APRIL</option>
                                        <option value="3" class="txtclr">MARCH</option>
                                    </select>
                                    </h6>

                                <canvas id="ChartEstimatestBySalesperson"></canvas>
                                    <div class="row card-bottom-text"><p class="card-text card-text2" style="margin: 0px auto; color: gray;"></p></div>
                                </div>
                            </div>
                        </div>
                        <div class="item">
                            <div class="card bx-shadow">
                                <div class="card-body card_back">
                                    <h6 class="card-title" style="margin-bottom: 10px;">Opportunities
    
                                        <a href="#" ><img src="DashboardNew/images/download.png" class="download-btn"/></a>
                                        <select class="custom-select transparent classic select-box-style">
                                        <option value="1" class="txtclr">MAY</option>
                                        <option value="2" class="txtclr">APRIL</option>
                                        <option value="3" class="txtclr">MARCH</option>
                                    </select>
                                    </h6>
                                    <canvas id="myChart2"></canvas>
                                    <div class="row card-bottom-text"><p class="card-text card-text2">Closed Won v/s Loss</p></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Mobile View Charts -->
                <div class="row overflow-auto2 row2-mobile-view">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4 col-xl-4 col-xxl-4 pad-right">
                        <div class="card bx-shadow">
                            <div class="card-body card_back">
                                <h6 class="card-title" style="margin-bottom: 10px;">Opportunities

                                    <a href="#" ><img src="DashboardNew/images/download.png" class="download-btn" /></a>
                                    <select class="custom-select transparent classic select-box-style">
                                    <option value="1" class="txtclr">MAY</option>
                                    <option value="2" class="txtclr">APRIL</option>
                                    <option value="3" class="txtclr">MARCH</option>
                                </select>
                                </h6>
                                <%--<canvas id="myChart41"></canvas>--%>
                                <canvas id="chartMobileActualVsBudgetedRevenue"></canvas>
                                <div class="row card-bottom-text"><p class="card-text card-text2">Closed Won v/s Loss</p></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4 col-xl-4 col-xxl-4 pad-right">
                        <div class="card bx-shadow" style="margin-bottom: 10px;">
                            <div class="card-body card_back">
                                <h6 class="card-title" style="margin-bottom: 10px;">Opportunities

                                    <a href="#" ><img src="DashboardNew/images/download.png" class="download-btn"/></a>
                                    <select class="custom-select transparent classic select-box-style">
                                    <option value="1" class="txtclr">MAY</option>
                                    <option value="2" class="txtclr">APRIL</option>
                                    <option value="3" class="txtclr">MARCH</option>
                                </select>
                                </h6>
                                <canvas id="myChart31"></canvas>
                                <div class="row" style="margin-top: 10px;  margin-bottom: -10px;"><p class="card-text card-text2">Closed Won v/s Loss</p></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4 col-xl-4 col-xxl-4 pad-right">
                        <div class="card bx-shadow">
                                <div class="card-body card_back">
                                    <h6 class="card-title" style="margin-bottom: 10px;">Opportunities
    
                                        <a href="#" ><img src="DashboardNew/images/download.png" class="download-btn" /></a>
                                        <select class="custom-select transparent classic select-box-style">
                                        <option value="1" class="txtclr">MAY</option>
                                        <option value="2" class="txtclr">APRIL</option>
                                        <option value="3" class="txtclr">MARCH</option>
                                    </select>
                                    </h6>
                                    <canvas id="myChart51"></canvas>
                                    <div class="row" style="margin-top: 10px; margin-bottom: -10px;"><p class="card-text card-text2">Closed Won v/s Loss</p></div>
                                </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4 col-xl-4 col-xxl-4 pad-right">
                        <div class="card bx-shadow" style="margin-bottom: 15px;">
                            <div class="card-body card_back">
                                <h6 class="card-title" style="margin-bottom: 10px;">Opportunities

                                <a href="#" ><img src="DashboardNew/images/download.png" class="download-btn" /></a>
                                <select class="custom-select transparent classic select-box-style">
                                    <option value="1" class="txtclr">MAY</option>
                                    <option value="2" class="txtclr">APRIL</option>
                                    <option value="3" class="txtclr">MARCH</option>
                                </select>
                                </h6>

                            <canvas id="ChartMobileEstimatestBySalesperson"></canvas>
                                <div class="row card-bottom-text"><p class="card-text card-text2" style="margin: 0px auto; color: gray;">Current Period</p></div>
                            </div>
                        </div>
                    </div>
                     <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4 col-xl-4 col-xxl-4 pad-right">
                        <div class="card bx-shadow">
                            <div class="card-body card_back">
                                <h6 class="card-title" style="margin-bottom: 10px;">Opportunities

                                    <a href="#" ><img src="DashboardNew/images/download.png" class="download-btn"/></a>
                                    <select class="custom-select transparent classic select-box-style">
                                    <option value="1" class="txtclr">MAY</option>
                                    <option value="2" class="txtclr">APRIL</option>
                                    <option value="3" class="txtclr">MARCH</option>
                                </select>
                                </h6>
                                <canvas id="myChart21"></canvas>
                                <div class="row card-bottom-text"><p class="card-text card-text2">Closed Won v/s Loss</p></div>
                            </div>
                        </div>
                    </div>
                </div>
        
                <!-- Section Row 3 -->
                <div class="row" style="margin-bottom: -30px;">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 col-xl-6 col-xxl-6">
                        <div class="card" style="margin-right: -15px;">
                            <div class="card-header" style="margin-bottom: -6px; background-color: #272c32;">
                                <h6 class="card-title sales-person-heading">Sales Person List

                                <i class="Large mdi-navigation-more-vert dropdown-sales-person" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"></i>


                                <div class="dropdown-menu dropdown-menu-right">
                                    <a class="dropdown-item dropdown-items" href="">Action 1</a>
                                    <a class="dropdown-item dropdown-items" href="">Action 2</a>
                                    <a class="dropdown-item dropdown-items" href="">Action 3</a>
                                </div>

                                <a href="#" >
                                    <img src="DashboardNew/images/download.png" class="download-btn"/></a>
                                    <select class="custom-select transparent classic select-box-style">
                                    <option value="1" class="txtclr">MAY</option>
                                    <option value="2" class="txtclr">APRIL</option>
                                    <option value="3" class="txtclr">MARCH</option>
                                </select>

                                </h6>
                            </div>
                            <div class="table-css-res">
                                <div class="table-responsive">
                                    <table id="example" class="table table-striped table-bordered second" style="width:100%; color: white; line-height: 0.5;">
                                        <thead>
                                            <tr style="background-color: #1c5fb1;">
                                                <th class="d-none d-md-table-cell">Sno.</th>
                                                <th>Name</th>
                                                <th class="d-none d-xl-table-cell">No. of Opportunity</th>
                                                <!-- <th class="d-none d-lg-table-cell">Quantity</th> -->
                                                <th>Price</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td class="d-none d-md-table-cell">1.</td>
                                                <td>Tiger Nixon</td>
                                                <td class="d-none d-xl-table-cell">8</td>
                                                <!-- <td class="d-none d-lg-table-cell">61</td> -->
                                                <td>$320,800</td>
                                            </tr>
                                            <tr>
                                                <td class="d-none d-md-table-cell">2.</td>
                                                <td>Garrett Winters</td>
                                                <td class="d-none d-xl-table-cell">15</td>
                                                <!-- <td class="d-none d-lg-table-cell">63</td> -->
                                                <td>$170,750</td>
                                            </tr>
                                            <tr>
                                                <td class="d-none d-md-table-cell">3.</td>
                                                <td>Garrett</td>
                                                <td class="d-none d-xl-table-cell">12</td>
                                                <!-- <td class="d-none d-lg-table-cell">67</td> -->
                                                <td>$195,750</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>

                      <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 col-xl-6 col-xxl-6">
                        <div class="card" style="margin-right: -15px;">
                            <div class="row" style="margin-bottom: -20px; background-color: #272c32;">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 col-xl-6 col-xxl-6">
                            <div style="padding: 5px;">
                                <div id="locationmap" style="width:100%; height:150px;"></div>
                            </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 col-xl-6 col-xxl-6">
                            <div style="padding: 5px;">
                                <h5 class="card-title" style="margin: 10px 5px;">Revenue by Location</h5>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
                                        <div class="sell-ratio">
                                            <h5 class="map-text">New York</h5>
                                            <div class="progress-w-percent d-flex">
                                                <p class="progress-value map-text-figures">72k </p>
                                                <div class="progress progress-sm">
                                                    <div class="progress-bar" role="progressbar" style="width: 72%;" aria-valuenow="72" aria-valuemin="0" aria-valuemax="100"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
                                        <div class="sell-ratio">
                                            <h5 class="map-text">London</h5>
                                            <div class="progress-w-percent d-flex">
                                                <p class="progress-value map-text-figures">39k</p>
                                                <div class="progress progress-sm">
                                                    <div class="progress-bar" role="progressbar" style="width: 39%;" aria-valuenow="39" aria-valuemin="0" aria-valuemax="100"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
                                        <div class="sell-ratio">
                                            <h5 class="map-text">Sydney</h5>
                                            <div class="progress-w-percent d-flex">
                                                <p class="progress-value map-text-figures">25k </p>
                                                <div class="progress progress-sm">
                                                    <div class="progress-bar" role="progressbar" style="width: 39%;" aria-valuenow="39" aria-valuemin="0" aria-valuemax="100"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
                                        <div class="sell-ratio">
                                            <h5 class="map-text">Singapore</h5>
                                            <div class="progress-w-percent d-flex">
                                                <p class="progress-value map-text-figures" >61k </p>
                                                <div class="progress progress-sm">
                                                    <div class="progress-bar" role="progressbar" style="width: 61%;" aria-valuenow="61" aria-valuemin="0" aria-valuemax="100"></div>
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
                
                <!-- Fab Btn HTML-->
                <!-- <div class="btn-group-fab" role="group" aria-label="FAB Menu">
                    <div>
                      <button type="button" class="btn btn-main btn-primary has-tooltip" data-placement="left" title="Menu"> <i class="mdi-navigation-menu"></i> </button>
                      <button type="button" class="btn btn-sub btn-info has-tooltip" data-placement="left" title="Fullscreen"  data-toggle="modal" data-target="#exampleModalCenter2"> <i class="mdi-social-person" style="margin-left: -2px; margin-top: -2px;"></i> </button>
                      <button type="button" class="btn btn-sub btn-danger has-tooltip" data-placement="left" title="Save" > <i class="mdi-editor-border-color" style="margin-left: -2px; margin-top: -2px;"></i> </button>
                    </div>
                  </div>

                  <div class="modal fade" id="exampleModalCenter2" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                      <div class="modal-content modal-change">
                        <div class="modal-body">
                            <button type="button" class="btn btn-secondary modal-cross-btn-position" data-dismiss="modal" id="rotate-cross">X</button>
                            <div class="row" style="margin-bottom: -10px;">
                                <div class="col-9">
                                    <h3>Name Here</h3>
                                </div>
                                <div class="col-3">
                                    <p style="margin: 0px auto; float: right; font-size: larger;">3%</p>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-9">
                                    <p style="font-size: smaller;">Sales Manager</p>
                                </div>
                                <div class="col-3">
                                    <p style="margin: 0px auto; float: right; font-size: smaller;">Conversion Rate</p>
                                </div>
                            </div>
                                <div id="sparklinedash">
                                    <span class="bar"></span>
                                </div>

                        </div>
                      </div>
                    </div>
                  </div> -->
    <script defer>
       
 

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".icon").click(function () {
                $('.box').toggleClass("open-more");
                $(this).toggleClass("button");
            });

            setTimeout(function () {
                //console.log("shuyam babu");
                $('.select-dropdown').hide();
                $('.select-dropdown').remove();

                $('.caret').hide();
                $('.caret').remove();


                $('.select-wrapper').contents().unwrap();
            }, 100);

            GetRecurringHours();
            GetActualVsBudgetedRevenue();
            GetEquipmentBuilding();
            GetEquipmentbyType();
        });

        function GetRecurringHours() {
            var url = "<%= ConfigurationSettings.AppSettings["BaseApiUrl"] %>" + "DashBoardAPI/DashBoard_GetRecurringHours";
            //var url = "https://localhost:44306/api/DashBoardAPI/DashBoard_GetRecurringHours"; 
            var APIRequest = [
                { Token:  '<%=Session["API_Token"] %>' }
            ];
            APIRequest = JSON.stringify({ 'User': APIRequest });
            var data = { "Token": '<%=Session["API_Token"] %>', "Param": "" };
            data = JSON.stringify(data);
            //console.log(data);
            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                //dataType: "json",
                //crossDomain: true,
                dataType: "json",
                //dataType: 'json',
                //cache: false,
                //crossOrigin: true,
                //traditional: true, 
                data: data,
                success: function (chData) {
                    //console.log(chData);
                    //console.log(chData.responseData);
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "EncrptDecrpt.asmx/Decrypt",
                        data: '{"strEncrypted":"' + chData.responseData + '"}',
                        dataType: "json",
                        async: true,
                    }).done(function (data) {
                        //console.log(data.d);
                        var b = JSON.parse(JSON.stringify(data.d));
                        data.d = data.d.replace(/[\u0000-\u0019]+/g, "");
                        //data.d = JSON.parse(data.d);
                        updateRecurringHoursChart(b);
                    });
                }
            });
        }
        function updateRecurringHoursChart(obj) {
            //console.log(obj);
            obj = JSON.parse(obj);
            //debugger;
            var varLabel = [];
            var varAvg = [];
            for (let prop in obj) {
                //console.log(obj[prop].SalesPerson);
                //console.log(obj[prop].Avg);
                //varLabel += obj[prop].SalesPerson;
                varLabel.push(obj[prop].SalesPerson);
                //varAvg += obj[prop].Avg;
                varAvg.push(obj[prop].Avg);

            }

            //console.log(varLabel);
            //console.log(varAvg);

            //Charts Web View
            var ctx = document.getElementById('ChartEstimatestBySalesperson').getContext('2d');
            var chart = new Chart(ctx, {
                type: 'bar',

                data: {
                    //labels: ['Prospecting', 'Qualification', 'Survey', 'Proposal', 'Negotiation', 'Closed Won', 'Closed Lost'],
                    labels: varLabel,
                    datasets: [{
                        label: 'Hours',
                        barPercentage: 0.7,
                        //backgroundColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2'],
                        backgroundColor: ['#ee4b6a', '#ee4b6a', '#ee4b6a', '#ee4b6a', '#ee4b6a', '#ee4b6a', '#ee4b6a'],
                        //data: [8, 25, 60, 38, 18, 20, 60]
                        data: varAvg
                    }]
                },
                options: {
                    legend: {
                        labels: {
                            fontSize: 10
                        }
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                color: 'white',
                                drawOnChartArea: false
                            },
                            ticks: {
                                beginAtZero: true,
                                fontSize: 10
                            }
                        }],
                        yAxes: [{
                            gridLines: {
                                color: 'white',
                                drawOnChartArea: false
                            },
                            ticks: {
                                beginAtZero: true,
                                fontSize: 10
                            }
                        }]
                    },
                    tooltips: {
                        enabled: true
                    }
                }
            });

            //Charts Mobile View
            var ctx11 = document.getElementById('ChartMobileEstimatestBySalesperson').getContext('2d');
            var chart11 = new Chart(ctx11, {
                type: 'bar',

                data: {
                    //labels: ['Prospecting', 'Qualification', 'Survey', 'Proposal', 'Negotiation', 'Closed Won', 'Closed Lost'],
                    labels: varLabel,
                    datasets: [{
                        label: 'Hour',
                        barPercentage: 0.7,
                        backgroundColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2'],
                        //data: [8, 25, 60, 38, 18, 20, 60]
                        data: varAvg
                    }]
                },
                options: {
                    legend: {
                        labels: {
                            fontSize: 10
                        }
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                color: 'white',
                                drawOnChartArea: false
                            },
                            ticks: {
                                beginAtZero: true,
                                fontSize: 10
                            }
                        }],
                        yAxes: [{
                            gridLines: {
                                color: 'white',
                                drawOnChartArea: false
                            },
                            ticks: {
                                beginAtZero: true,
                                fontSize: 10
                            }
                        }]
                    },
                    tooltips: {
                        enabled: true
                    }
                }
            });

        }
        function checkBudget() {
            GetActualVsBudgetedRevenue();
        }
        function GetActualVsBudgetedRevenue() {
            var url = "<%= ConfigurationSettings.AppSettings["BaseApiUrl"] %>" + "DashBoardAPI/DashBoard_GetActualvsBudgetedRevenue";
            var APIRequest = [
                { Token:  '<%=Session["API_Token"] %>' }
            ];
            APIRequest = JSON.stringify({ 'User': APIRequest });
            var data = { "Token": '<%=Session["API_Token"] %>', "Param": "" };
            data = JSON.stringify(data);
            //console.log(data);

            var planCodes = $('#sltBudget');
            //debugger;
            var drpurl = "<%= ConfigurationSettings.AppSettings["BaseApiUrl"] %>" + "DashBoardAPI/DashBoard_GetListBudgetName";
            
            
            //$.ajax({
            //    type: "POST",
            //    url : drpurl,
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    data: data,
            //    success: function (chData) {
            //        $.ajax({
            //            type: "POST",
            //            contentType: "application/json; charset=utf-8",
            //            url: "EncrptDecrpt.asmx/Decrypt",
            //            data: '{"strEncrypted":"' + chData.responseData + '"}',
            //            dataType: "json",
            //            async: true,
            //        }).done(function (data) {
            //            data.d = JSON.parse(data.d);
            //            $("#sltBudget").html(""); // clear before appending new list

            //            //$.each(data.d, function (i, b) {
            //            //    $("#sltBudget").append(

            //            //        $('<option></option>').val(b.BudgetID).html(b.Budget));
            //            //});
                        
            //            for (var i = 0; i < data.d.length; i++) {
            //               // console.log(data.d[i].BudgetID);  
            //               // console.log(data.d[i].Budget); 

            //                var option = $("<option/>");
            //                if (i == 0) {
            //                    //option.attr("value", '0').text('Select Budget');      
            //                }

            //                    option.attr("value", data.d[i].BudgetID).text(data.d[i].Budget);
            //                    planCodes.append(option);
                            
            //            }
            //        });
            //    }
            //});

            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",                
                dataType: "json",
                data: data,
                success: function (chData) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "EncrptDecrpt.asmx/Decrypt",
                        data: '{"strEncrypted":"' + chData.responseData + '"}',
                        dataType: "json",
                        async: true,
                    }).done(function (data) {
                       // console.log(data.d);
                        var b = JSON.parse(JSON.stringify(data.d));
                        data.d = data.d.replace(/[\u0000-\u0019]+/g, "");
                        //data.d = JSON.parse(data.d);
                        updateActualVsBudgetedRevenue(b);
                    });
                }
            });
        }
        function updateActualVsBudgetedRevenue(obj) {
            obj = JSON.parse(obj);
            
            var varLabel = [];
            var varBudget = [];
            var varTotal = [];            
            for (let prop in obj) {
                varLabel.push(obj[prop].NMonth);
                varBudget.push(obj[prop].NBudget);
                varTotal.push(obj[prop].NTotal);                
            }
            var ctx4 = document.getElementById('chartActualVsBudgetedRevenue').getContext('2d');
            var chart4 = new Chart(ctx4, {
                type: 'bar',

                data: {
                    //labels: ['January', 'February', 'March', 'April'],
                    labels: varLabel,
                    datasets: [{
                        label: 'Actual Revenues',
                        barPercentage: 0.7,
                        backgroundColor: ['#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f'],
                        borderColor: ['#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f'],
                        //data: [45, 10, 30, 45]
                        data: varTotal
                    },
                    {
                        label: 'Budgeted Revenues',
                        barPercentage: 0.7,
                        backgroundColor: ['#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42'],
                        borderColor: ['#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42'],
                        //data: [35, 15, 35, 50]
                        data: varBudget
                    }
                    ]
                },
                options: {
                    legend: {
                        labels: {
                            fontSize: 10
                        }
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                color: 'white',
                                drawOnChartArea: false
                            },
                            ticks: {
                                beginAtZero: true,
                                fontSize: 10
                            }
                        }],
                        yAxes: [{
                            gridLines: {
                                color: 'white',
                                drawOnChartArea: false
                            },
                            ticks: {
                                beginAtZero: true,
                                fontSize: 10
                            }
                        }]
                    },
                    tooltips: {
                        enabled: true
                    }
                }
            });

            //Charts Mobile View
            var ctx41 = document.getElementById('chartMobileActualVsBudgetedRevenue').getContext('2d');
            var chart41 = new Chart(ctx41, {
                type: 'bar',

                data: {
                    //labels: ['January', 'February', 'March', 'April'],
                    labels: varLabel,
                    datasets: [{
                        label: 'Actual Revenues',
                        barPercentage: 0.7,
                        backgroundColor: ['#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f'],
                        borderColor: ['#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f'],
                        //data: [45, 10, 30, 45]
                        data: varTotal
                    },
                    {
                        label: 'Budgeted Revenues',
                        barPercentage: 0.7,
                        backgroundColor: ['#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42'],
                        borderColor: ['#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42'],
                        //data: [35, 15, 35, 50]
                        data: varBudget
                    }
                    ]
                },
                options: {
                    legend: {
                        labels: {
                            fontSize: 10
                        }
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                color: 'white',
                                drawOnChartArea: false
                            },
                            ticks: {
                                beginAtZero: true,
                                fontSize: 10
                            }
                        }],
                        yAxes: [{
                            gridLines: {
                                color: 'white',
                                drawOnChartArea: false
                            },
                            ticks: {
                                beginAtZero: true,
                                fontSize: 10
                            }
                        }]
                    },
                    tooltips: {
                        enabled: true
                    }
                }
            });

        }
        function GetEquipmentBuilding() {
            
            var url = "<%= ConfigurationSettings.AppSettings["BaseApiUrl"] %>" + "DashBoardAPI/DashBoard_GetEquipmentBuildingChart";
            //var url = "https://localhost:44306/api/DashBoardAPI/DashBoard_GetEquipmentBuildingChart";  
            
            var APIRequest = [
                { Token:  '<%=Session["API_Token"] %>' }
            ];
            APIRequest = JSON.stringify({ 'User': APIRequest });
            var data = { "Token": '<%=Session["API_Token"] %>', "Param": "" };
            data = JSON.stringify(data);
            
            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: data,
                success: function (chData) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "EncrptDecrpt.asmx/Decrypt",
                        data: '{"strEncrypted":"' + chData.responseData + '"}',
                        dataType: "json",
                        async: true,
                    }).done(function (data) {
                         console.log(data.d);
                        var b = JSON.parse(JSON.stringify(data.d));
                        data.d = data.d.replace(/[\u0000-\u0019]+/g, "");
                        //data.d = JSON.parse(data.d);
                        updateGetEquipmentBuilding(b);
                    });
                }
            });
        }
        function updateGetEquipmentBuilding(obj) {
            //console.log(obj);
            obj = JSON.parse(obj);
            
            var varLabel = [];
            var varTotal = [];

            for (let prop in obj) {
                varLabel.push(obj[prop].Building);
                varTotal.push(obj[prop].Total);             
            }
            console.log(varLabel);
            console.log(varTotal);
            //Charts Web View
            var ctx3 = document.getElementById('chartEquipmentBuilding').getContext('2d');
           
          var chart3 = new Chart(ctx3, {
            type: 'doughnut',

            data: {
                title: {
                    fontColor: "red",
                },
                labels: varLabel,//['Open', 'Closed', 'Withdrawn', 'Cancelled', 'Disqualified', 'Sold', 'Quoted'],
                datasets: [{
                    label: 'Opportunities',
                    barPercentage: 0.5,
                    barThickness: 2,
                    maxBarThickness: 8,
                    backgroundColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2'],
                    borderColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2'],
                    data: varTotal//[15, 20, 30, 40, 10, 16, 25]
                }]
            },
            options: {
                legend: {
                    labels: {
                        fontSize: 10
                    }
                },
            }
        });
      

        }

        function GetEquipmentbyType() {
            var url = "<%= ConfigurationSettings.AppSettings["BaseApiUrl"] %>" + "DashBoardAPI/DashBoard_GetEquipmentTypeChart";
            //var url = "https://localhost:44306/api/DashBoardAPI/DashBoard_GetEquipmentTypeChart";  
            var APIRequest = [
                { Token:  '<%=Session["API_Token"] %>' }
            ];
            APIRequest = JSON.stringify({ 'User': APIRequest });
            var data = { "Token": '<%=Session["API_Token"] %>', "Param": "" };
            data = JSON.stringify(data);

            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: data,
                success: function (chData) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "EncrptDecrpt.asmx/Decrypt",
                        data: '{"strEncrypted":"' + chData.responseData + '"}',
                        dataType: "json",
                        async: true,
                    }).done(function (data) {
                        // console.log(data.d);
                        var b = JSON.parse(JSON.stringify(data.d));
                        data.d = data.d.replace(/[\u0000-\u0019]+/g, "");
                        //data.d = JSON.parse(data.d);
                        updateEquipmentbyType(b);
                    });
                }
            });
        }
        function updateEquipmentbyType(obj) {
            console.log(obj);
            obj = JSON.parse(obj);
            
            var varLabel = [];
            var varTotal = [];

            for (let prop in obj) {
                varLabel.push(obj[prop].TypeName);
                varTotal.push(obj[prop].Total);
            }
            console.log(varLabel);
            console.log(varTotal);
            //Charts Web View
            var ctx3 = document.getElementById('chartEquipmentbyType').getContext('2d');

            var chart3 = new Chart(ctx3, {
                type: 'doughnut',

                data: {
                    title: {
                        fontColor: "red",
                    },
                    labels: varLabel,//['Open', 'Closed', 'Withdrawn', 'Cancelled', 'Disqualified', 'Sold', 'Quoted'],
                    datasets: [{
                        label: 'Opportunities',
                        barPercentage: 0.5,
                        barThickness: 2,
                        maxBarThickness: 8,
                        backgroundColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2', '#614bee', '#e84bee', '#ee4b74'],
                        borderColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2', '#614bee', '#e84bee', '#ee4b74'],
                        //data: [15, 20, 30, 40, 10, 16, 25]
                        data: varTotal
                    }]
                },
                options: {
                    legend: {
                        labels: {
                            fontSize: 10
                        }
                    },
                }
            });


        }
    </script>

    <!-- Vector Map -->
    <script>
        jQuery('#locationmap').vectorMap({

            map: 'world_mill_en',
            backgroundColor: 'transparent',
            borderColor: '#000',
            borderOpacity: 1,
            borderWidth: 0.6,
            zoomOnScroll: true,
            color: '#25d5f2',
            regionStyle: {
                initial: {
                    fill: "#d1dfe8"
                }
            },
            markerStyle: {
                initial: {
                    r: 9,
                    fill: "#1c5fb1",
                    "fill-opacity": .9,
                    stroke: "#fff",
                    "stroke-width": 7,
                    "stroke-opacity": .4
                },
                hover: {
                    stroke: "#fff",
                    "fill-opacity": 1,
                    "stroke-width": 1.5
                }
            },

            markers: [{
                latLng: [40.71, -74],
                name: "New York"
            }, {
                latLng: [37.77, -122.41],
                name: "San Francisco"
            }, {
                latLng: [-33.86, 151.2],
                name: "Sydney"
            }, {
                latLng: [1.3, 103.8],
                name: "Singapore"
            }],


            hoverOpacity: null,
            normalizeFunction: 'linear',
            scaleColors: ['#25d5f2', '#25d5f2'],
            selectedColor: '#c9dfaf',
            selectedRegions: [],
            showTooltip: true,
            onRegionClick: function (element, code, region) {
                var message = 'You clicked "' + region + '" which has the code: ' + code.toUpperCase();
                alert(message);
            }

        });

    </script>

    <!-- Charts Web View -->
    <script>
        Chart.defaults.global.defaultFontColor = 'white';

        //var ctx = document.getElementById('myChart').getContext('2d');
        //var chart = new Chart(ctx, {
        //    type: 'bar',

        //    data: {
        //        labels: ['Prospecting', 'Qualification', 'Survey', 'Proposal', 'Negotiation', 'Closed Won', 'Closed Lost'],
        //        datasets: [{
        //            label: 'Dataset',
        //            barPercentage: 0.7,
        //            backgroundColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2'],
        //            data: [8, 25, 60, 38, 18, 20, 60]
        //        }]
        //    },
        //    options: {
        //        legend: {
        //            labels: {
        //                fontSize: 10
        //            }
        //        },
        //        scales: {
        //            xAxes: [{
        //                gridLines: {
        //                    color: 'white',
        //                    drawOnChartArea: false
        //                },
        //                ticks: {
        //                    beginAtZero: true,
        //                    fontSize: 10
        //                }
        //            }],
        //            yAxes: [{
        //                gridLines: {
        //                    color: 'white',
        //                    drawOnChartArea: false
        //                },
        //                ticks: {
        //                    beginAtZero: true,
        //                    fontSize: 10
        //                }
        //            }]
        //        },
        //        tooltips: {
        //            enabled: true
        //        }
        //    }
        //});

        var ctx2 = document.getElementById('myChart2').getContext('2d');
        var chart2 = new Chart(ctx2, {
            type: 'line',

            data: {
                labels: ['March', 'April', 'May', 'June', 'July'],
                datasets: [{
                    label: 'Won',
                    backgroundColor: '#4a90e278',
                    borderColor: '#4a90e2',
                    data: [0, 12, 20, 18, 45]
                }
                ]
            },
            options: {
                legend: {
                    labels: {
                        fontSize: 10
                    }
                },
                scales: {
                    xAxes: [{
                        gridLines: {
                            color: 'white',
                            drawOnChartArea: false
                        },
                        ticks: {
                            fontSize: 10
                        }
                    }],
                    yAxes: [{
                        gridLines: {
                            color: 'white',
                            drawOnChartArea: false
                        },
                        ticks: {
                            fontSize: 10
                        }
                    }]
                }
            }
        });

        //var ctx3 = document.getElementById('myChart3').getContext('2d');
        //var chart3 = new Chart(ctx3, {
        //    type: 'doughnut',

        //    data: {
        //        title: {
        //            fontColor: "red",
        //        },
        //        labels: ['Open', 'Closed', 'Withdrawn', 'Cancelled', 'Disqualified', 'Sold', 'Quoted'],
        //        datasets: [{
        //            label: 'Opportunities',
        //            barPercentage: 0.5,
        //            barThickness: 2,
        //            maxBarThickness: 8,
        //            backgroundColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2'],
        //            borderColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2'],
        //            data: [15, 20, 30, 40, 10, 16, 25]
        //        }]
        //    },
        //    options: {
        //        legend: {
        //            labels: {
        //                fontSize: 10
        //            }
        //        },
        //    }
        //});

      
      
  
        //var ctx4 = document.getElementById('myChart4').getContext('2d');
        //var chart4 = new Chart(ctx4, {
        //    type: 'bar',

        //    data: {
        //        labels: ['January', 'February', 'March', 'April'],
        //        datasets: [{
        //            label: 'My First dataset',
        //            barPercentage: 0.7,
        //            backgroundColor: ['#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f'],
        //            borderColor: ['#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f'],
        //            data: [45, 10, 30, 45]
        //        },
        //        {
        //            label: 'My Second dataset',
        //            barPercentage: 0.7,
        //            backgroundColor: ['#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42'],
        //            borderColor: ['#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42'],
        //            data: [35, 15, 35, 50]
        //        }
        //        ]
        //    },
        //    options: {
        //        legend: {
        //            labels: {
        //                fontSize: 10
        //            }
        //        },
        //        scales: {
        //            xAxes: [{
        //                gridLines: {
        //                    color: 'white',
        //                    drawOnChartArea: false
        //                },
        //                ticks: {
        //                    beginAtZero: true,
        //                    fontSize: 10
        //                }
        //            }],
        //            yAxes: [{
        //                gridLines: {
        //                    color: 'white',
        //                    drawOnChartArea: false
        //                },
        //                ticks: {
        //                    beginAtZero: true,
        //                    fontSize: 10
        //                }
        //            }]
        //        },
        //        tooltips: {
        //            enabled: true
        //        }
        //    }
        //});


        //var ctx5 = document.getElementById('myChart5').getContext('2d');
        //var chart5 = new Chart(ctx5, {
        //    type: 'line',

        //    data: {
        //        labels: ['March', 'April', 'May', 'June', 'July'],
        //        datasets: [{
        //            label: 'Opp',
        //            backgroundColor: 'transparent',
        //            borderColor: '#ee4b6a',
        //            data: [0, 50, 20, 18, 45]
        //        }, {
        //            label: 'Estimate',
        //            backgroundColor: 'transparent',
        //            borderColor: '#7ac74f',
        //            data: [0, 35, 25, 40, 50]
        //        }]
        //    },
        //    options: {
        //        legend: {
        //            labels: {
        //                fontSize: 10
        //            }
        //        },
        //        scales: {
        //            xAxes: [{
        //                gridLines: {
        //                    color: 'white',
        //                    drawOnChartArea: false
        //                },
        //                ticks: {
        //                    fontSize: 10
        //                }
        //            }],
        //            yAxes: [{
        //                gridLines: {
        //                    color: 'white',
        //                    drawOnChartArea: false
        //                },
        //                ticks: {
        //                    fontSize: 10
        //                }
        //            }]
        //        }
        //    }
        //});
    </script>


    <!-- Charts Mobile View -->
    <script>
            Chart.defaults.global.defaultFontColor = 'white';
    
        //var ctx11 = document.getElementById('myChart11').getContext('2d');
        //var chart11 = new Chart(ctx11, {
        //    type: 'bar',

        //    data: {
        //        labels: ['Prospecting', 'Qualification', 'Survey', 'Proposal', 'Negotiation', 'Closed Won', 'Closed Lost'],
        //        datasets: [{
        //            label: 'Dataset',
        //            barPercentage: 0.7,
        //            backgroundColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2'],
        //            data: [8, 25, 60, 38, 18, 20, 60]
        //        }]
        //    },
        //    options: {
        //        legend: {
        //            labels: {
        //                fontSize: 10
        //            }
        //        },
        //        scales: {
        //            xAxes: [{
        //                gridLines: {
        //                    color: 'white',
        //                    drawOnChartArea: false
        //                },
        //                ticks: {
        //                    beginAtZero: true,
        //                    fontSize: 10
        //                }
        //            }],
        //            yAxes: [{
        //                gridLines: {
        //                    color: 'white',
        //                    drawOnChartArea: false
        //                },
        //                ticks: {
        //                    beginAtZero: true,
        //                    fontSize: 10
        //                }
        //            }]
        //        },
        //        tooltips: {
        //            enabled: true
        //        }
        //    }
        //});

        var ctx21 = document.getElementById('myChart21').getContext('2d');
        var chart21 = new Chart(ctx21, {
            type: 'line',

            data: {
                labels: ['March', 'April', 'May', 'June', 'July'],
                datasets: [{
                    label: 'Won',
                    backgroundColor: '#4a90e278',
                    borderColor: '#4a90e2',
                    data: [0, 12, 20, 18, 45]
                }
                ]
            },
            options: {
                legend: {
                    labels: {
                        fontSize: 10
                    }
                },
                scales: {
                    xAxes: [{
                        gridLines: {
                            color: 'white',
                            drawOnChartArea: false
                        },
                        ticks: {
                            fontSize: 10
                        }
                    }],
                    yAxes: [{
                        gridLines: {
                            color: 'white',
                            drawOnChartArea: false
                        },
                        ticks: {
                            fontSize: 10
                        }
                    }]
                }
            }
        });

        var ctx31 = document.getElementById('myChart31').getContext('2d');
        var chart31 = new Chart(ctx31, {
            type: 'doughnut',

            data: {
                title: {
                    fontColor: "red",
                },
                labels: ['Open', 'Closed', 'Withdrawn', 'Cancelled', 'Disqualified', 'Sold', 'Quoted'],
                datasets: [{
                    label: 'Opportunities',
                    barPercentage: 0.5,
                    barThickness: 2,
                    maxBarThickness: 8,
                    backgroundColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2'],
                    borderColor: ['#a1cf6b', '#7ac74f', '#ee4b6a', '#ff7f30', '#f8e71c', '#66d7d1', '#4a90e2'],
                    data: [15, 20, 30, 40, 10, 16, 25]
                }]
            },
            options: {
                legend: {
                    labels: {
                        fontSize: 10
                    }
                },
            }
        });


        //var ctx41 = document.getElementById('myChart41').getContext('2d');
        //var chart41 = new Chart(ctx41, {
        //    type: 'bar',

        //    data: {
        //        labels: ['January', 'February', 'March', 'April'],
        //        datasets: [{
        //            label: 'My First dataset',
        //            barPercentage: 0.7,
        //            backgroundColor: ['#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f'],
        //            borderColor: ['#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f', '#eacf6f'],
        //            data: [45, 10, 30, 45]
        //        },
        //        {
        //            label: 'My Second dataset',
        //            barPercentage: 0.7,
        //            backgroundColor: ['#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42'],
        //            borderColor: ['#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42', '#ff9b42'],
        //            data: [35, 15, 35, 50]
        //        }
        //        ]
        //    },
        //    options: {
        //        legend: {
        //            labels: {
        //                fontSize: 10
        //            }
        //        },
        //        scales: {
        //            xAxes: [{
        //                gridLines: {
        //                    color: 'white',
        //                    drawOnChartArea: false
        //                },
        //                ticks: {
        //                    beginAtZero: true,
        //                    fontSize: 10
        //                }
        //            }],
        //            yAxes: [{
        //                gridLines: {
        //                    color: 'white',
        //                    drawOnChartArea: false
        //                },
        //                ticks: {
        //                    beginAtZero: true,
        //                    fontSize: 10
        //                }
        //            }]
        //        },
        //        tooltips: {
        //            enabled: true
        //        }
        //    }
        //});


        var ctx51 = document.getElementById('myChart51').getContext('2d');
        var chart51 = new Chart(ctx51, {
            type: 'line',

            data: {
                labels: ['March', 'April', 'May', 'June', 'July'],
                datasets: [{
                    label: 'Opp',
                    backgroundColor: 'transparent',
                    borderColor: '#ee4b6a',
                    data: [0, 50, 20, 18, 45]
                }, {
                    label: 'Estimate',
                    backgroundColor: 'transparent',
                    borderColor: '#7ac74f',
                    data: [0, 35, 25, 40, 50]
                }]
            },
            options: {
                legend: {
                    labels: {
                        fontSize: 10
                    }
                },
                scales: {
                    xAxes: [{
                        gridLines: {
                            color: 'white',
                            drawOnChartArea: false
                        },
                        ticks: {
                            fontSize: 10
                        }
                    }],
                    yAxes: [{
                        gridLines: {
                            color: 'white',
                            drawOnChartArea: false
                        },
                        ticks: {
                            fontSize: 10
                        }
                    }]
                }
            }
        });
    </script>

    <!-- Fab Button -->
    <!-- <script>
        $(function() {
            $('.btn-group-fab').on('click', '.btn', function() {
                $('.btn-group-fab').toggleClass('active');
            });
            $('has-tooltip').tooltip();
        });
    </script> -->


    

    <!-- <script src="Design/js/plugins/sparkline/jquery.sparkline.min.js"></script> -->
     <!-- <script>

        function openChart(){
            console.log("main chutoiya hu");
        const data = [0, 5, 6, 10, 9, 12, 4, 9]
        const config = {
        type: 'bar',
        height: '50',
        barWidth: '10',
        resize: true,
        barSpacing: '5',
        barColor: '#7ace4c'
        }
        $('#sparklinedash').sparkline(data, config)
        }
    </script> -->

    <!-- Horizontal Carousel JS -->
<link href="DashboardNew/owl/dist/assets/owl.carousel.min.css" rel="stylesheet" />
<script src="DashboardNew/owl/dist/owl.carousel.min.js"></script>
    <script type="text/javascript">
        $('#carousel1').owlCarousel({
            loop: true,
            margin: 10,
            responsiveClass: true,
            responsive: {
                600: {
                    items: 2,
                    nav: true
                },
                1000: {
                    items: 4,
                    nav: true,
                    loop: false
                }
            }
        })
    </script>

    <script>
        $('#carousel2').owlCarousel({
            loop: true,
            margin: 10,
            responsiveClass: true,
            responsive: {
                600: {
                    items: 2,
                    nav: true
                },
                1200: {
                    items: 3,
                    nav: true,
                    loop: false
                }
            }
        })
    </script>
    <!-- Universal Date picker Js -->
    <script type="text/javascript">
        $(function () {

            var start = moment().subtract(29, 'days');
            var end = moment();

            function cb(start, end) {
                $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
            }

            $('#reportrange').daterangepicker({
                startDate: start,
                endDate: end,
                ranges: {
                    'Today': [moment(), moment()],
                    'Current Week': [moment().subtract(6, 'days'), moment()],
                    'Current Month': [moment().startOf('month'), moment().endOf('month')],
                    'Quarter': [moment().subtract(3, 'month').startOf('month'), moment()],
                    'Last Six Month': [moment().subtract(6, 'month').startOf('month'), moment()],
                    'Annual': [moment().subtract(12, 'month').startOf('month'), moment()]
                }
            }, cb);

            cb(start, end);

        });
    </script>



     

</asp:Content>
