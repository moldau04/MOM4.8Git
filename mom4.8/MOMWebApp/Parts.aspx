<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="Parts" Codebehind="Parts.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">

        <div class="page-cont-top">
            <ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="#">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Billing Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <%--<li>
                    <a href="#">Locations</a>
                    <i class="fa fa-angle-right"></i>
                </li>--%>
                <li>
                    <span>Parts</span>
                </li>
            </ul>
            <div class="page-bar-right">
                <asp:LinkButton CssClass="Close_button" ID="lnkClose" ForeColor="Red" runat="server" CausesValidation="false"
                    OnClick="lnkClose_Click"><i class="fa fa-times"></i></asp:LinkButton>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Parts</asp:Label>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div>
                        <div class="table-scrollable">
                            <asp:Panel runat="server" ID="pnlGridButtons" Style="border-style: solid solid none solid; background-position: #B8E5FC; background: #B8E5FC; width: 100%; height: 25px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px; border-width: 1px; border-color: #a9c6c9;">
                                <asp:LinkButton Style="float: left; color: #2382B2; margin-right: 20px; margin-left: 10px;"
                                    ID="LinkButton8" runat="server" OnClick="lnkEditBillingCodes_Click"
                                    CausesValidation="False">Edit</asp:LinkButton>
                                <asp:LinkButton Style="float: left; color: #2382B2; margin-right: 20px;" ID="LinkButton9"
                                    runat="server" OnClientClick="return confirm('Do you really want to delete this Billing Code ?');"
                                    OnClick="lnkDeleteBillingCodes_Click" CausesValidation="False">Delete</asp:LinkButton>
                                <asp:LinkButton Style="float: left; color: #2382B2; margin-right: 20px;" ID="LinkButton10"
                                    runat="server" OnClick="lnkAddBillingCodes_Click" CausesValidation="False">Add New</asp:LinkButton>
                            </asp:Panel>
                            <div>
                                <asp:GridView ID="gvBillCodes" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                    Width="100%" PageSize="20" AllowPaging="True"
                                    AllowSorting="True" OnDataBound="gvBillCodes_DataBound"
                                    OnRowCommand="gvBillCodes_RowCommand" OnSorting="gvBillCodes_Sorting">
                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part" SortExpression="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBillID" Visible="false" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                <asp:Label ID="lblCatID" Visible="false" runat="server" Text='<%# Eval("cat") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" SortExpression="fdesc">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" SortExpression="jobtype">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCat" runat="server" Text='<%# Eval("jobtype") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" SortExpression="balance">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbalance" runat="server" Text='<%# Eval("balance") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Measure" SortExpression="Measure">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMeasure" runat="server" Text='<%# Eval("Measure") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Warehouse" SortExpression="warehousename">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWarehouse" runat="server" Text='<%# Eval("warehousename") %>'></asp:Label>
                                                <asp:Label Visible="false" ID="lblwarehouseid" runat="server" Text='<%# Eval("warehouse") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" SortExpression="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrem" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle CssClass="footer" />
                                    <RowStyle CssClass="evenrowcolor" />
                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                    <PagerTemplate>
                                        <div align="center">
                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" ImageUrl="images/first.png" />
                                            &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev"
                                                ImageUrl="~/images/Backward.png" />
                                            &nbsp &nbsp <span>Page</span>
                                            <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span>of </span>
                                            <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                            &nbsp &nbsp
                        <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument="Next" ImageUrl="images/Forward.png" />
                                            &nbsp &nbsp
                        <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument="Last" ImageUrl="images/last.png" />
                                        </div>
                                    </PagerTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                        CausesValidation="False" />
                    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                        PopupDragHandleControlID="programmaticPopupDragHandle"
                        RepositionMode="RepositionOnWindowResizeAndScroll">
                    </asp:ModalPopupExtender>
                    <asp:Panel runat="server" ID="programmaticPopup" Style="display: block; left:300px !important; background: #fff; border: 1px solid #316b9d;">
                        <asp:Panel runat="Server" ID="programmaticPopupDragHandle">
                        </asp:Panel>
                        <div>
                            <asp:Panel ID="pnlBillCode" runat="server">
                                <div class="model-popup-body">
                                    <asp:Label CssClass="title_text" ID="Label8" runat="server">Bill Code</asp:Label>
                                    <asp:LinkButton ID="LinkButton11" runat="server" CausesValidation="False" Style="float: right; color: #fff; margin-left: 10px; height: 16px;"
                                        OnClick="lnkBillingCodesClose_Click">Close</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton12" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                                        ValidationGroup="serv" OnClick="lnkBillingCodesSave_Click">Save</asp:LinkButton>
                                </div>
                                <asp:HiddenField ID="hdnBillID" runat="server" />
                                <div style="padding:10px">
                                <table style="width: 400px; height:300px">
                                    <tr>
                                        <td class="register_lbl">Part
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server"
                                ControlToValidate="txtBillCode" Display="None"
                                ErrorMessage="Part Required" SetFocusOnError="True"
                                ValidationGroup="serv"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator40_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator40">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillCode" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">Part Description
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillDesc" runat="server" CssClass="form-control" MaxLength="255"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">Status
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">Rate
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server"
                                ControlToValidate="txtBillBal" Display="None" ErrorMessage="Rate Required"
                                SetFocusOnError="True" ValidationGroup="serv"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator41_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator41">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillBal" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtBillBal_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" TargetControlID="txtBillBal"
                                                ValidChars="1234567890.-">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">Measure
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillMeasure" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">Warehouse</td>
                                        <td>
                                            <asp:DropDownList ID="ddlWarehouse" runat="server"
                                                CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">Remarks
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillRemarks" TextMode="MultiLine" CssClass="form-control" runat="server" MaxLength="200"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table></div>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>

