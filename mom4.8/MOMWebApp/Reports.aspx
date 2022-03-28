<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="Reports" Codebehind="Reports.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            var _type = "<%= Request.QueryString["Type"] %>";
            if (_type != null) {
                $('#ctl00_ContentPlaceHolder1_liRep' + _type).addClass('active');
            }

            $('.ra-list li a').click(function () {
                $('.ra-list li').each(function (index, element) {
                    if ($(element).hasClass('active')) {
                        $(element).removeClass('active');
                    }
                });
                $(this).closest('li').addClass('active');
            })
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="page-cont-top">
            <%-- <ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="#">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Sales Manager</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Estimate</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add Estimate</span>
                </li>
            </ul>--%>
            <%-- <div class="page-bar-right">
                <a href="#" class="pbr-save tooltips" data-original-title="Save" data-placement="bottom"><i class="fa fa-check"></i></a>
                <a href="#" class="pbr-close tooltips" data-original-title="Close" data-placement="bottom"><i class="fa fa-times"></i></a>
            </div>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-sm-3 col-md-3">
                <div class="recent-activity">
                    <div class="ra-title">Report List</div>
                    <ul class="ra-list cr-list">
                        <li id="liRepCustomer" runat="server">
                            <asp:LinkButton ID="lnkRepCustMgr" OnClick="lnkRepCustMgr_Click" runat="server"><i class="fa fa-arrow-right"></i>Customer Manager</asp:LinkButton>
                        </li>
                        <li id="liRepRecurring" runat="server">
                            <asp:LinkButton ID="lnkRepRecMgt" OnClick="lnkRepRecMgt_Click" runat="server"><i class="fa fa-arrow-right"></i>Recurring Manager</asp:LinkButton>
                        </li>

                        <li id="liRepScheduler"><a href="#">
                            <i class="fa fa-arrow-right"></i>
                            <span>Schedule Manager</span>
                        </a></li>
                        <li><a href="#">
                            <i class="fa fa-arrow-right"></i>
                            <span>Billing Manager</span>
                        </a></li>

                        <li><a href="#">
                            <i class="fa fa-arrow-right"></i>
                            <span>Sales Manager</span>
                        </a></li>
                        <li><a href="#">
                            <i class="fa fa-arrow-right"></i>
                            <span>Project Manager</span>
                        </a></li>

                        <li><a href="#">
                            <i class="fa fa-arrow-right"></i>
                            <span>Financial Manager</span>
                        </a></li>
                        <li><a href="#">
                            <i class="fa fa-arrow-right"></i>
                            <span>Financial Statement</span>
                        </a></li>

                        <li><a href="#">
                            <i class="fa fa-arrow-right"></i>
                            <span>Program Manager</span>
                        </a></li>
                    </ul>
                </div>
            </div>
            <asp:UpdatePanel runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lnkRepCustMgr" />
                    <asp:AsyncPostBackTrigger ControlID="lnkRepRecMgt" />
                </Triggers>
                <ContentTemplate>
                    <!-- Reports Activity start -->

                    <!-- Recent Activity start -->

                    <!-- ADD ESTIMATE start -->
                    <div class="col-sm-9 col-md-9">
                        <div class="add-estimate">
                            <div class="ra-title">
                                <ul class="lnklist-header">
                                    <li>
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Reports</asp:Label></li>
                                    <li>
                                        <a href="#" class="icon-save" title="Save" data-placement="bottom"></a>
                                    </li>
                                    <li>
                                        <a href="#" class="icon-closed" title="Close" data-placement="bottom"></a>
                                    </li>
                                </ul>
                            </div>
                            <div class="ae-content">
                                <div class="row">
                                    <div id="divMain" runat="server" style="overflow: auto; padding-top: 20px;" align="left">
                                        <asp:PlaceHolder ID="PlaceHolder1" runat="server" />
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- ADD ESTIMATE end -->
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>

