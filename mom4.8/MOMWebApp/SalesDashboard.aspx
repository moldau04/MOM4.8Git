<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="SalesDashboard" Codebehind="SalesDashboard.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="js/raphael.2.1.0.min.js"></script>

    <script type="text/javascript" src="js/justgage.1.0.1.min.js"></script>

    <script type="text/javascript">
        //        $(document).ready(function() {
        //            createGauge(0,0);
        //        });

        function createGauge(max, value) {
            var g1 = new JustGage({
                id: "g1",
                value: value,
                min: 0,
                max: max,
                title: "Closed Sales to Date",
                label: "Sum of Amount($)",
                levelColorsGradient: false,
                labelFontColor: "#626262",
                startAnimationTime: 1000,
                startAnimationType: "bounce",
                levelColors: [
                          "#990000",
                          "#FFCC66",
                          "#009900"
                ]
            });
        }
    </script>

    <style type="text/css">
        #g1 {
            width: 350px;
            height: 280px;
            display: inline-block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="page-cont-top">
            <%--<ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="<%=ResolveUrl("~/Home.aspx") %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Sales Manager</a>
                    <i class="fa fa-angle-right"></i>
                </li>--%>
                <%-- <li>
                    <a href="#">Locations</a>
                    <i class="fa fa-angle-right"></i>
                </li>--%>
                <%--<li>
                    <span>CRM Dashboard</span>
                </li>
            </ul>--%>
           
        </div>
        <div class="clearfix"></div>
        <div class="add-estimate">
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="ra-title">
                        <ul class="lnklist-header">
                            <li><asp:Label CssClass="title_text" ID="lblHeader" runat="server">Dashboard</asp:Label></li>
                            <li>
                                <ul class="nav navbar-nav pull-right">
                                    <li class="dropdown dropdown-user">
                                        <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                        <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                            <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear:both;"></div></a></li>
                                        </ul>
                                    </li>
                                </ul>
                            </li>
                            <li><a href="home.aspx" class="icon-closed" title="Close"></a></li>
                        </ul>
                    </div>
                </div>
                <!-- edit-tab start -->
                <div class="ae-content">
                    <div class="col-lg-12 col-md-12">
                        <div class="com-cont">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Timer ID="Timer1" runat="server" Interval="60000" OnTick="Timer1_Tick">
                                    </asp:Timer>
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="sc-form">
                                                <label>Search</label>
                                                <asp:DropDownList ID="ddlSearch" runat="server" CssClass="form-control"
                                                    Width="200px">
                                                </asp:DropDownList>
                                                <asp:ImageButton ID="ibtnRefresh" runat="server" ImageUrl="images/refresh.png" OnClick="ibtnRefresh_Click"
                                                    Style="margin: 0 20px 0 20px;" ToolTip="Refresh" Width="25px" />
                                                <asp:Label ID="lblLast" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="table-scrollable" style="border: none">
                                                <div class="sc-form" style="border: none">
                                                    <div id="g1"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="roundCorner shadow">
                                                <div class="table-scrollable" style="border: none">
                                                    <label style="font-size: 18px; font-weight: bold;">10 Open Deals</label>
                                                    <asp:GridView ID="gvOpenOpportunity" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        Width="400px" PageSize="20" EmptyDataText="No records to display">
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Opportunity Name" SortExpression="opp">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="lnkname" NavigateUrl='<%# "addopprt.aspx?uid=" + Eval("id") %>' Target="_blank" runat="server" Text='<%# Eval("opp") %>'></asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Owner" SortExpression="fuser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOwner" runat="server" Text='<%# Eval("fuser") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount" SortExpression="revenue">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrevenue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "revenue", "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="roundCorner shadow">
                                                <div class="table-scrollable" style="border: none">
                                                    <label style="font-size: 18px; font-weight: bold;">Sales Leaderboard</label>
                                                    <asp:GridView ID="gvSalesLead" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        Width="400px" PageSize="20" EmptyDataText="No records to display">
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Owner" SortExpression="fuser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOwner" runat="server" Text='<%# Eval("fuser") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount" SortExpression="revenue">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrevenue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "revenue", "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="table-scrollable" style="border: none">
                                                <div class="roundCorner shadow">
                                                    <label style="font-size: 18px; font-weight: bold;">Top 10 Lead/Account</label>
                                                    <asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        Width="400px" PageSize="20" EmptyDataText="No records to display">
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Lead/Account Name" SortExpression="opp">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblname" runat="server" Text='<%# Eval("opp") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount" SortExpression="revenue">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrevenue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "revenue", "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>

</asp:Content>
