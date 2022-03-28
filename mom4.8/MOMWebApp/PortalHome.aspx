<%@ Page Title="" Language="C#" MasterPageFile="~/MOM.master" AutoEventWireup="true" Inherits="PortalHome" Codebehind="PortalHome.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.12/css/all.css" >

    <style>
        body {
            background-color: #272C32 !important;
        }

        .card {
            border-radius: 8px !important;
            box-shadow: 0 9px 18px -3px #000 !important;
        }

        .portalCard {
            padding: 10px !important;
        }

        .largeicon {
            font-size: 2.5em;
        }

        .white-text {
            transition: color 1s;
        }

        .blue {
            transition: background-color 1s;
        }

        .card:hover .white-text {
            color: #1565c0 !important;
        }

        .card:hover .blue {
            background-color: #fff !important;
        }
    </style>
    <script src="Appearance/js/main.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section id="content">
        <!--start container-->
        <div class="container">
            <div class="form-section-row" style="margin-top: 10px;" runat="server" id="tblCustomer">
                <div class="section-ttlewhite">Customer Manager</div>
                <div class="row">
                    <div runat="server" id="divCustomers" class="col s6 m4 l2">
                        <a id="hlnkCustomers" runat="server" href="~/Customers.aspx">
                            <div class="card " style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblCustomers" runat="server" Text="Customers"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-user"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <div runat="server" id="divLocations" class="col s6 m4 l2">
                        <a id="hlnkLocation" runat="server" href="~/Locations.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblLocation" runat="server" Text="Locations"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-location-arrow"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <div runat="server" id="divEquip" class="col s6 m4 l2">
                        <a id="hlnkEquip" runat="server" href="~/Equipments.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblEquip" runat="server" Text="Equipment"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-wrench"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <div runat="server" id="div6" class="col s6 m4 l2">
                        <a id="HyperLink6" runat="server" href="~/Library.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="Label6" runat="server" Text="Shared Documents"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-folder-open"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                </div>

            </div>

            <div class="form-section-row" runat="server" id="tblRecurring">
                <div class="section-ttlewhite">Recurring Manager</div>
                <div class="row">
                    <div runat="server" id="divRecCont" class="col s6 m4 l2">
                        <a id="hlnkReccContracts" runat="server" href="~/RecContracts.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblRecContracts" runat="server" Text="Recurring Contracts"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-sync-alt"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <div runat="server" id="divRecInv" class="col s6 m4 l2">
                        <a id="hlnkReccInvoice" runat="server" href="~/recurringinvoices.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblReccInvoice" runat="server" Text="Recurring Invoices"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-file-alt"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <div runat="server" id="divrecTick" class="col s6 m4 l2">
                        <a id="hlnkReccTickets" runat="server" href="~/RecurringTickets.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblReccTickets" runat="server" Text="Recurring Tickets"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-ticket-alt"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>

            <div class="form-section-row" runat="server" id="tblSchedule">
                <div class="section-ttlewhite">Schedule Manager</div>
                <div class="row">
                    <div runat="server" id="divScheduler" class="col s6 m4 l2">
                        <a id="hlnkSchedule" runat="server" href="~/Scheduler.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblSchedule" runat="server" Text="Schedule"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-calendar-alt"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divTickList" class="col s6 m4 l2">
                        <a id="hlnkTicketList" runat="server" href="~/TicketListView.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblTicketList" runat="server" Text="Ticket List"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-list-alt"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divTimesheet" class="col s6 m4 l2">
                        <a id="hlnkTimesheet" runat="server" href="~/etimesheet.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblTimesheet" runat="server" Text="e-Timesheet"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-clock"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divMapLink"  class="col s6 m4 l2">
                        <a id="hlnkMap" runat="server" href="~/map.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblMap" runat="server" Text="Map"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-map"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divRouteBuilder" class="col s6 m4 l2">
                        <a id="hlnkRouteBuilder" runat="server" href="~/RouteBuilder.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblRouteBuilder" runat="server" Text="Route Builder"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-map-signs"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>

            <div class="form-section-row" runat="server" id="tblBilling">
                <div class="section-ttlewhite">Billing Manager</div>
                <div class="row">
                    <div runat="server" id="divInvoices" class="col s6 m4 l2">
                        <a id="hlnkInvoices" runat="server" href="~/invoices.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblInvoices" runat="server" Text="Invoices"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-file-excel"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divPaymentH" class="col s6 m4 l2">
                        <a id="hlnkPayhist" runat="server" href="~/paymenthistory.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblPayHist" runat="server" Text="Payment History"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-money-bill-alt"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divBillCodes" class="col s6 m4 l2">
                        <a id="hlnkBilCodes" runat="server" href="~/billingcodes.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblBillCodes" runat="server" Text="Billing Codes"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-hand-holding-usd"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                   <%-- <div runat="server" id="div10" class="col s6 m4 l2">
                        <a id="A4" runat="server" href="~/map.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="Label9" runat="server" Text="Map"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-map"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>--%>

                </div>
            </div>

            <div class="form-section-row" runat="server" id="tblSales">
                <div class="section-ttlewhite">Sales Manager</div>
                <div class="row">
                    <div runat="server" id="divProspects" class="col s6 m4 l2">
                        <a id="hlnkProspect" runat="server" navigateurl="~/Prospects.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblProspects" runat="server" Text="Leads"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-handshake"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divOpportunities" class="col s6 m4 l2">
                        <a id="hlnkOpportunities" runat="server" href="~/opportunity.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblOpportunities" runat="server" Text="Opportunities"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-certificate"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divTasks" class="col s6 m4 l2">
                        <a id="hlnkTasks" runat="server" navigateurl="~/tasks.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblTasks" runat="server" Text="Tasks"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-tasks"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divProposals" class="col s6 m4 l2">
                        <a id="hlnkProposals" runat="server" href="#">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblProposals" runat="server" Text="Proposals"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-angle-double-right"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divCRM" class="col s6 m4 l2">
                        <a id="hlnkCRM" runat="server" href="~/salesdashboard.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblCRM" runat="server" Text="CRM Dashboard"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fab fa-cuttlefish"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="div1" class="col s6 m4 l2">
                        <a id="HyperLink1" runat="server" href="salessetup.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="Label1" runat="server" Text="Sales Setup"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fab fa-wpforms"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="div7" class="col s6 m4 l2">
                        <a id="A1" runat="server" href="salessetup.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="Label5" runat="server" Text="Sales Setup"></asp:Label></span>
                                        <div class="white-text largeicon">
                                            <i class="fab fa-wpforms"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>

            <div class="form-section-row" runat="server" id="tblProject">
                <div class="section-ttlewhite">Project Manager</div>
                <div class="row">
                    <div runat="server" id="div2" class="col s6 m4 l2">
                        <a id="HyperLink2" runat="server" href="~/Project.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="Label2" runat="server" Text="Projects"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fab fa-product-hunt"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="div3" class="col s6 m4 l2">
                        <a id="HyperLink3" runat="server" href="~/setup.aspx?tab=1.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="Label3" runat="server" Text="Departments"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-industry"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="div4" class="col s6 m4 l2">
                        <a id="HyperLink4" runat="server" href="~/projecttemplate.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="Label4" runat="server" Text="Project Templates"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-font"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                </div>
            </div>

            <div class="form-section-row" runat="server" id="tblProgram">
                <div class="section-ttlewhite">Program Manager</div>
                <div class="row">
                    <div id="divUsers" runat="server" class="col s6 m4 l2">
                        <a id="lnkUsers" runat="server" navigateurl="~/Users.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblUsers" runat="server" Text="Users"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-user"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divSetup" class="col s6 m4 l2">
                        <a id="lnkSetup" runat="server" navigateurl="~/setup.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblSetup" runat="server" Text="Setup"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-cogs"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divControl" class="col s6 m4 l2">
                        <a id="lnkControl" runat="server" href="~/ControlPanel.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblControl" runat="server" Text="Control Panel"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-address-card"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>

                    <div runat="server" id="divCustom" class="col s6 m4 l2">
                        <a id="lnkCstmLbl" runat="server" href="~/customfields.aspx">
                            <div class="card" style="overflow: hidden; max-height: 100px; height: 100px;">
                                <div class="card-action blue darken-2 portalCard">
                                    <div class="center-align">
                                        <span class="white-text" style="font-size: 1.1em;">
                                            <asp:Label ID="lblCstmLbl" runat="server" Text="Custom Labels"></asp:Label>
                                        </span>
                                        <div class="white-text largeicon">
                                            <i class="fas fa-indent"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>

            <div class="form-section-row" runat="server" id="Table1" style="display: none;">
                <div class="section-ttlewhite">Program Manager</div>
                <div class="row">
                </div>
            </div>
        </div>
    </section>
</asp:Content>

