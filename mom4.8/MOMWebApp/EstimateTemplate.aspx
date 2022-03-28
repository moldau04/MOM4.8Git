<%@ Page Language="C#" MasterPageFile="~/NewSalesMaster.master" AutoEventWireup="true" Inherits="EstimateTemplate" Codebehind="EstimateTemplate.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function CheckDelete() {
            var result = false;
            $("#<%=gvEstimates.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Do you really want to delete this Template ?');
            }
            else {
                alert('Please select a Template to delete.')
                return false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
            </li>
            <li>
                <a href="<%=ResolveUrl("~/estimatetemplate.aspx") %>">Estimate Template</a>--%>
        <%--<i class="fa fa-angle-right"></i>--%>
        <%-- </li>
        </ul>--%>
        <div class="page-bar-right">
        </div>
    </div>
    <div class="add-estimate">
        <div class="ra-title">
            <asp:Panel runat="server" ID="pnlGridButtons">
                <ul class="lnklist-header">
                    <li>
                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Estimate Template</asp:Label></li>
                    <li>
                        <asp:LinkButton runat="server" CausesValidation="False" ToolTip="Add New" CssClass="icon-addnew" OnClick="lnkAdd_Click"></asp:LinkButton></li>
                    <li>
                        <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" ToolTip="Edit" CssClass="icon-edit" OnClick="lnkEdit_Click"></asp:LinkButton></li>
                    <li>
                        <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return CheckDelete();" CausesValidation="False" ToolTip="Delete" CssClass="icon-delete"
                            OnClick="lnkDelete_Click"></asp:LinkButton></li>
                    <li>
                        <ul class="nav navbar-nav pull-right">
                            <li class="dropdown dropdown-user">
                                <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                    <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear: both;"></div>
                                    </a></li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <asp:LinkButton CssClass="icon-closed" ForeColor="Red" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                            OnClick="lnkClose_Click"></asp:LinkButton></li>
                </ul>
            </asp:Panel>
        </div>
        <div class="ae-content">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--     <table>
                                <tr>
                                    <td>
                                        Search
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged"
                                            CssClass="register_input_bg_ddl_small">
                                            <asp:ListItem Value=" ">--Select--</asp:ListItem>
                                            <asp:ListItem Value="r.name">Name</asp:ListItem>
                                            <asp:ListItem Value="p.terr">Salesperson</asp:ListItem>
                                            <asp:ListItem Value="r.address">Address</asp:ListItem>
                                            <asp:ListItem Value="r.city">City</asp:ListItem>
                                            <asp:ListItem Value="r.state">State</asp:ListItem>
                                            <asp:ListItem Value="p.type">Type</asp:ListItem>
                                            <asp:ListItem Value="p.status">Status</asp:ListItem>
                                            <asp:ListItem Value="days">Days</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        &nbsp;: &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="register_input_bg_small"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:ImageButton CausesValidation="false" ID="lnkSearch" runat="server" ImageUrl="images/search.png"
                                            OnClick="lnkSearch_Click" ToolTip="Search"></asp:ImageButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkShowAll" runat="server" CausesValidation="False" Style="margin-left: 20px;"
                                            OnClick="lnkShowAll_Click">Show All</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic; margin-left: 100px;"></asp:Label>
                                    </td>
                                </tr>
                            </table>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <%--   <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lnkSearch" />
                                <asp:AsyncPostBackTrigger ControlID="lnkShowAll" />
                            </Triggers>--%>
                <ContentTemplate>

                    <asp:GridView ID="gvEstimates" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                        Width="100%" PageSize="20" AllowPaging="false" AllowSorting="false">
                        <AlternatingRowStyle CssClass="oddrowcolor" />
                        <FooterStyle CssClass="footer" />
                        <RowStyle CssClass="evenrowcolor" />
                        <SelectedRowStyle CssClass="selectedrowcolor" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" SortExpression="name">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                                    <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Desc" SortExpression="fdesc">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
