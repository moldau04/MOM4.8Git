<%@ Page Title="Collections || MOM" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="Collections" Codebehind="Collections.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
         .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
         }
         .collection-tbl
         {
             font-size: 8pt!important;
             background-color: white!important;
         }
         .customer-row {
             background-color: #e9f0f6;
             font-weight: bold;
         }
         .loading
         {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
         }

    </style>
    <script type="text/javascript">
        function ChkWarning() {
            var result = false;
            //debugger;
            $("#<%=gvOverDueInvoice.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });
            <%--$("#<%=gvOverDueInvoice.ClientID%> input[id*='chkSelect']").each(function () {
                debugger;
                var checkBox = $(this).find("input[id*='chkSelect']");
                var chkSelect = checkBox.attr('id');

                //var transid = $(document.getElementById(chkSelect.replace('chkSelect', 'lblTransID'))).text().toString()
                //console.log(transid);
                //if ($(document.getElementById(checkBox.replace('chkSelect', 'lblTransID'))).text().toString() == "0") {
                //    alert($(document.getElementById(checkBox.replace('chkSelect', 'lblTransID'))).text().toString())
                if (checkBox.is(":checked")) {
                    result = true;
                    console(result)
                }
                //}
            });--%>
            if (result == false) {

                noty({
                    text: 'Please select customer invoices to Email.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
        }
        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        <%-- $('#<%=lnkMailCustomerStatement%>').on("click", function () {
            ShowProgress();
        });--%>
        $('#<%=lnkMailInvoice.ClientID%>').on("click", function () {
            ShowProgress();
        });
        $('#<%=lnkMailInvoiceMain.ClientID%>').on("click", function () {
            ShowProgress();
        });
        $('#<%=lnkMailInvoiceException.ClientID%>').on("click", function () {
            ShowProgress();
        });
        function unsuccessMesg(strLoc) {
            noty({ text: 'Email sent unsuccessfully to ' + strLoc + '!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 50000, theme: 'noty_theme_default', closable: false });
        }
        function dispWarningMesg() {
            $('#myModalCS').modal('show');
        }
        function displayWarnInvoice()
        {
            $('#myModalInvoice').modal('show');
        }
        function hideModelCS() {
            $('#myModalCS').modal({
                show: 'false'
            });
            $('#myModalCS').fadeOut('slow');
        }
        function hideModelInvoice()
        {
            $('#myModalInvoice').modal({
                show: 'false'
            });
            $('#myModalInvoice').fadeOut('slow');
        }
        function ShowContact(owner, cust, cont, phone, fax, email, cell)
        {
            var modalContactPopupBehavior = $find('programmaticContactModalPopupBehavior');
            $("#<%=hdnOwner1.ClientID %>").val(owner);
            $("#<%=lblCustomer1.ClientID %>").text(cust);
            $("#<%=txtContact.ClientID %>").val(cont);
            $("#<%=txtEmail.ClientID %>").val(email);
            $("#<%=txtFax.ClientID %>").val(fax);
            $("#<%=txtCell.ClientID%>").val(cell);
            $("#<%=txtPhone.ClientID%>").val(phone);

            modalContactPopupBehavior.show();
        }
        function ShowNote(OwnerId, Cust, CustNotes)
        {
            var modalNotePopupBehavior = $find('programmaticNoteModalPopupBehavior');
            $("#<%= lblCustomer.ClientID %>").text(Cust);
            $("#<%= hdnOwner.ClientID %>").val(OwnerId);
            $("#<%= txtCollectionNote.ClientID %>").val(CustNotes);
            modalNotePopupBehavior.show();
        }
        function ReloadPage() {
            return false;
        }
        function cancel() {

            window.parent.document.getElementById('btnCancel').click();
        }
        
        function pageLoad(sender, args) {
            $(function () {

                $("#<%=gvOverDueInvoice.ClientID%> input[id*='chkSelect']").change(function () {
                    
                    var chkSelect = $(this).attr('id');
                    var chkClass = $(document.getElementById(chkSelect.replace('chkSelect', 'lblOwner'))).text().toString();

                    if ($(document.getElementById(chkSelect.replace('chkSelect', 'lblTransID'))).text().toString() == "0")
                    {
                        $("span.chk" + chkClass + " input[id*='chkSelect']:checkbox").prop('checked', $(this).prop("checked"));

                        $("span.chk" + chkClass + " input[id*='chkSelect']:checkbox").each(function (index) {

                            if ($(this).is(':checked')) {
                                SelectedRowStyle('<%=gvOverDueInvoice.ClientID %>')
                            }
                            else {
                                $(this).closest('tr').removeAttr("style");
                            }

                        });
                    }
                });
                
                $("#ctl00_ContentPlaceHolder1_TabContainerHeader_tbpnlOverdue_gvOverDueInvoice_ctl01_chkSelectHeader").change(function () {
                    
                    $("#<%=gvOverDueInvoice.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', $(this).prop("checked"));

                    $("#<%=gvOverDueInvoice.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {

                        if ($("#ctl00_ContentPlaceHolder1_TabContainerHeader_tbpnlOverdue_gvOverDueInvoice_ctl01_chkSelectHeader").is(':checked')) {
                            SelectedRowStyle('<%=gvOverDueInvoice.ClientID %>')
                        }
                        else {
                            $(this).closest('tr').removeAttr("style");
                        }

                    });
                });
            });
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="page-content">
        <div class="page-cont-top">
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Collections </asp:Label>
                        </li>
                        <li>
                            <ul class="nav navbar-nav pull-right">
                                <li class="dropdown dropdown-user">
                                    <a href="Collections.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-mail" data-hover="dropdown" 
                                        data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                    <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                        <li style="margin-left: 0px;">
                                            <asp:LinkButton ID="lnkMailInvoice" runat="server" CausesValidation="false" 
                                                Text="Email Invoices" ToolTip="Email Invoices"
                                                OnClick="lnkMailInvoice_Click" OnClientClick="return ChkWarning();"></asp:LinkButton>
                                        </li>
                                        <li style="margin-left: 0px;">
                                            <asp:LinkButton ID="lnkMailInvoiceMain" runat="server" CausesValidation="false" 
                                                Text="Email Invoice Maintenance" ToolTip="Email Invoice Maintenance"
                                                OnClick="lnkMailInvoiceMain_Click" OnClientClick="return ChkWarning();"></asp:LinkButton>
                                        </li>
                                        <li style="margin-left: 0px;">
                                            <asp:LinkButton ID="lnkMailInvoiceException" runat="server" CausesValidation="false" 
                                                Text="Email Invoice Exception" ToolTip="Email Invoice Exception"
                                                OnClick="lnkMailInvoiceException_Click" OnClientClick="return ChkWarning();"></asp:LinkButton>
                                        </li>
                                        <li style="margin-left: 0px;">
                                            <asp:LinkButton ID="lnkMailCustomerStatement" Text="Email Customer Statement"
                                                runat="server" CausesValidation="false" ToolTip="Email Customer Statement"
                                                OnClick="lnkMailCustomerStatement_Click" OnClientClick="return ChkWarning();"></asp:LinkButton>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="close"
                                OnClick="lnkClose_Click"></asp:LinkButton>
                        </li>
                        
                    </ul>
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <asp:TabContainer ID="TabContainerHeader" runat="server" ActiveTabIndex="0">
                        <asp:TabPanel ID="tbpnlOverdue" runat="server" HeaderText="Overdue">
                            <HeaderTemplate>
                                Overdue
                            </HeaderTemplate>
                            <ContentTemplate>
                             
<%--                                <asp:GridView ID="gvCustomerInvoices" runat="server" AutoGenerateColumns="False" 
                                    CssClass="table table-bordered table-striped table-condensed flip-content" OnRowCommand="gvCustomerInvoices_RowCommand"
                                    AllowPaging="true" PageSize="10" OnSorting="gvCustomerInvoices_Sorting" OnPageIndexChanging="gvCustomerInvoices_PageIndexChanging"
                                    Width="100%" OnRowDataBound="gvCustomerInvoices_RowDataBound" ShowFooter="true" 
                                    EmptyDataText="No Customer Found" ShowHeaderWhenEmpty="True" Style="font: 10px;">
                                        <RowStyle CssClass="evenrowcolor" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Smaller" />
                                        <HeaderStyle Font-Size="Smaller" />
                                        <FooterStyle CssClass="footer" Font-Size="Smaller" />
                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-BackColor="White" ItemStyle-Width="2%">
                                                 <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAllCheck" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                    <asp:Label ID="lblCID" runat="server" Text='<%# Eval("cid") %>' style="display:none"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CustomerName" HeaderText="Customer" Visible="true" ItemStyle-BackColor="White" FooterStyle-BackColor="White" ItemStyle-Width="42%" />
                                            <asp:BoundField DataField="Balance" DataFormatString="{0:c}" HeaderText="Balance" Visible="true" ItemStyle-BackColor="White" FooterStyle-BackColor="White" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                                            <asp:BoundField DataField="ZeroThirty" DataFormatString="{0:c}" HeaderText="0 - 30" Visible="true" ItemStyle-BackColor="White" FooterStyle-BackColor="White" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                                            <asp:BoundField DataField="ThirtySixty" DataFormatString="{0:c}" HeaderText="31 - 60" Visible="true" ItemStyle-BackColor="White" FooterStyle-BackColor="White" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                                            <asp:BoundField DataField="SixtyNinty" DataFormatString="{0:c}" HeaderText="61 - 90" Visible="true" ItemStyle-BackColor="White" FooterStyle-BackColor="White" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                                            <asp:BoundField DataField="NintyOneTwenty" DataFormatString="{0:c}" HeaderText="91 - 120" Visible="true" ItemStyle-BackColor="White" FooterStyle-BackColor="White" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                                            <asp:BoundField DataField="OneTwentyOnePlus" DataFormatString="{0:c}" HeaderText="121+" Visible="true" ItemStyle-BackColor="White" FooterStyle-BackColor="White" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                                            <asp:TemplateField ItemStyle-BackColor="#316b9d" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <ul class="lnklist-header lnklist-panel">
                                                        <li><asp:ImageButton ID="imgNote" runat="server" ImageUrl="~/images/icons/note.png" 
                                                                Width="17px"/></li>
                                                        <li><asp:ImageButton ID="imgContact" runat="server" ImageUrl="~/images/icons/Contact.png" 
                                                                Width="17px"/></li>
                                                    </ul>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                <tr style="background-color: white">
                                                    <td colspan="100%" style="padding-bottom:0px;padding-left:0px;padding-right:0px;padding-top:0px;">
                                                        <div id="divInvoices" style="position: relative; left: 0px; overflow: auto">--%>
                                                            <asp:GridView ID="gvOverDueInvoice" runat="server" AutoGenerateColumns="false"
                                                                OnRowDataBound="gvOverDueInvoice_RowDataBound" CssClass="table table-bordered collection-tbl"
                                                                GridLines="None" Width="100%" ShowHeaderWhenEmpty="false" EmptyDataText="No Invoices Found" 
                                                                ShowFooter="true">
                                                                <HeaderStyle />
                                                                <RowStyle CssClass="evenrowcolor" VerticalAlign="Middle"  />
                                                                <FooterStyle CssClass="footer" Font-Size="8pt" Font-Bold="true"/>
                                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-Width="1%">
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkSelectHeader" runat="server" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" class='<%# "chk"+Eval("Owner").ToString() %>' />
                                                                            <asp:Label ID="lblUrl" runat="server" Text='<%# Eval("Url") %>' style="display:none"></asp:Label>
                                                                            <asp:Label ID="lblTransID" runat="server" Text='<%# Eval("idTrans") %>' style="display:none"></asp:Label>    
                                                                            <asp:Label ID="lblOwner" runat="server" Text='<%# Eval("Owner") %>' style="display:none"></asp:Label>    
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="20%" HeaderText="Customer">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("Tag") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                 
                                                                    <asp:TemplateField ItemStyle-Width="5%" HeaderText="Invoice#">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref").ToString() == "0" ? "" : Eval("Ref") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="7.1%" HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:MM/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):"" %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="2%" HeaderText="Due Days">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDueDay" runat="server" Text='<%# Eval("DaysPastDue").ToString() == "0" ? "" : Eval("DaysPastDue") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotal" runat="server" Text="Total:"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Balance" Visible="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAgingAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                         <FooterTemplate>
                                                                            <asp:Label ID="lblTotalAgingAmt" runat="server" ></asp:Label>
                                                                         </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="0 - 30" Visible="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount0" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount0", "{0:c}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                         <FooterTemplate>
                                                                            <asp:Label ID="lblTotal0" runat="server" ></asp:Label>
                                                                         </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="31 - 60" Visible="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount30", "{0:c}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                         <FooterTemplate>
                                                                            <asp:Label ID="lblTotal30" runat="server" ></asp:Label>
                                                                         </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="61 - 90" Visible="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount60" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount60", "{0:c}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                         <FooterTemplate>
                                                                            <asp:Label ID="lblTotal60" runat="server" ></asp:Label>
                                                                         </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="91 - 120" Visible="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount90" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount90", "{0:c}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                         <FooterTemplate>
                                                                            <asp:Label ID="lblTotal90" runat="server" ></asp:Label>
                                                                         </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="121+" Visible="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount121" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount121", "{0:c}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                         <FooterTemplate>
                                                                            <asp:Label ID="lblTotal121" runat="server" ></asp:Label>
                                                                         </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                      <asp:TemplateField ItemStyle-Width="10%" HeaderText="Company">
                                                                     <ItemTemplate>
                                                                             <asp:Label ID="lblCompany" runat="server" Text='<%# Eval("Company") %>'></asp:Label>
                                                                     </ItemTemplate>
                                                                 </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-BackColor="#316b9d" ItemStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                            <ul class="lnklist-header lnklist-panel">
                                                                                <li>
                                                                                    <a id="lnkNote" onclick="ShowNote('<%# Eval("Owner") %>','<%# Eval("Tag") %>','<%# Eval("CNotes") %>');" tooltip="Collection Note">
                                                                                        <Image src="images/icons/note.png" Width="17px"/></a>
                                                                                </li>
                                                                                <li>
                                                                                    <a id="lnkContact" onclick="ShowContact('<%# Eval("Owner") %>', '<%# Eval("Tag") %>', '<%# Eval("Contact") %>',
                                                                                         '<%# Eval("Phone") %>', '<%# Eval("Fax") %>', '<%# Eval("Email") %>',
                                                                                         '<%# Eval("Cellular") %>');" tooltip="Contact">
                                                                                        <Image src="images/icons/Contact.png" Width="17px" />
                                                                                    </a>
                                                                                </li>
                                                                            </ul>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                   
                                                                </Columns>
                                                                   
                                                            </asp:GridView>                  
                                                        <%--</div>
                                                    </td>
                                                </tr>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
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
                                </asp:GridView>--%>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tbpnlUpcoming" runat="server" HeaderText="Overdue" Visible="false">
                            <HeaderTemplate>
                                Upcoming
                            </HeaderTemplate>
                            <ContentTemplate>
                                 <asp:GridView ID="gvUpcomingInv" runat="server" AutoGenerateColumns="false"
                                                                CssClass="collection-tbl" 
                                                                GridLines="None" Width="100%" ShowHeaderWhenEmpty="false" EmptyDataText="No Invoices Found" 
                                                                ShowFooter="true">
                                                                <HeaderStyle Font-Size="Smaller" />
                                                                <RowStyle CssClass="evenrowcolor" VerticalAlign="Middle" Font-Size="8pt" />
                                                                <FooterStyle CssClass="footer" Font-Size="8pt" />
                                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-Width="1%">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                            <asp:Label ID="lblUrl" runat="server" Text='<%# Eval("Url") %>' style="display:none"></asp:Label>
                                                                            <asp:Label ID="lblTransID" runat="server" Text='<%# Eval("idTrans") %>' style="display:none"></asp:Label>
                                                                            
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="30%" HeaderText="Customer">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("Tag") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="5%" HeaderText="Invoice#">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref").ToString() == "0" ? "" : Eval("Ref") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="7.1%" HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:MM/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):"" %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField> <%--DaysPastDue--%>
                                                                    <asp:TemplateField ItemStyle-Width="2%" HeaderText="Due Days">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDueDay" runat="server" Text='<%# Eval("DaysPastDue").ToString() == "0" ? "" : Eval("DaysPastDue") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Amount" DataFormatString="{0:c}" HeaderText="Balance" Visible="true" ItemStyle-Width="8.1%" ItemStyle-HorizontalAlign="Right"/>
                                                                    <asp:BoundField DataField="Amount0" DataFormatString="{0:c}" HeaderText="0 - 30" Visible="true" ItemStyle-Width="8.2%" ItemStyle-HorizontalAlign="Right"/>
                                                                    <asp:BoundField DataField="Amount30" DataFormatString="{0:c}" HeaderText="31 - 60" Visible="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                                                                    <asp:BoundField DataField="Amount60" DataFormatString="{0:c}" HeaderText="61 - 90" Visible="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                                                                    <asp:BoundField DataField="Amount120" DataFormatString="{0:c}" HeaderText="91 - 120" Visible="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                                                                    <asp:BoundField DataField="Amount121" DataFormatString="{0:c}" HeaderText="121+" Visible="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                                                                    <asp:TemplateField ItemStyle-BackColor="#316b9d" ItemStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                            <ul class="lnklist-header lnklist-panel">
                                                                                <li>
                                                                                    <a id="lnkNote" onclick="ShowNote();" tooltip="Collection Note"> Note </a>
                                                                                </li>
                                                                                <li><%--<asp:ImageButton ID="imgContact" runat="server" ImageUrl="~/images/icons/Contact.png" Tooltip="Contact"
                                                                                        Width="17px"/> <Image  Url="~/images/icons/note.png" Width="17px"/>--%></li>
                                                                            </ul>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                   
                                                                </Columns>
                                                                   
                                                            </asp:GridView>        
                            </ContentTemplate>
                         </asp:TabPanel>
                    </asp:TabContainer>
                 
                    <div class="clearfix"></div>
                    <a href="#" runat="server" value="Add New" id="btnAddNewNote" ></a><%--onclick="getAccount();"--%>

                    <asp:ModalPopupExtender ID="mpeAddNote" BackgroundCssClass="ModalPopupBG"
                        runat="server" CancelControlID="btnCancel" OkControlID="btnOkay"
                        TargetControlID="btnAddNewNote" BehaviorID="programmaticNoteModalPopupBehavior"
                        PopupControlID="pnlAddNote" Drag="true" PopupDragHandleControlID="PopupHeader" OnOkScript="ReloadPage();">
                    </asp:ModalPopupExtender>

                    <div class="popup_Buttons" style="display: none">
                        <input id="btnOkay" value="Done" type="button" />
                        <input id="btnCancel" value="Cancel" type="button" />
                    </div>

                    <div id="pnlAddNote" class="table-subcategory" style="display: none;height:210px;width:400px;">
                        <div class="popup_Container">
                            <div class="popup_Body">
                                <div class="model-popup-body" style="padding-bottom: 24px;">
                                    <asp:Label CssClass="title_text" Style="float: left; font-size:13px;font-weight:bold;" ID="lblCollectionNote" runat="server">Collection Note</asp:Label>

                                    <div style="float: right;">
                                        <asp:LinkButton CssClass="save_button" ID="lnkSaveNote" Style="color: white;padding-right: 10px;" runat="server" 
                                            OnClick="lnkSaveNote_Click" TabIndex="38" CausesValidation="true" ValidationGroup="note">
                                             Save </asp:LinkButton>
                                        <a class="close_button_Form" id="lbtnClose" style="color: white;" onclick="cancel();">Close </a>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12 col-md-12" style="padding-left: 0px;padding-right: 0px;">
                                <div class="com-cont">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    Customer Name
                                                </div>
                                                <div class="fc-input" style="margin-top:5px;">
                                                    <asp:Label ID="lblCustomer" runat="server" ></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdnOwner"/>
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    Collection Note
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtCollectionNote" runat="server" CssClass="form-control" 
                                                        Rows="5" Columns="14" MaxLength="8000" TextMode="MultiLine" ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvCollectionNote"
                                                        runat="server" ControlToValidate="txtCollectionNote" Display="None" ErrorMessage="Collection Note is required"
                                                        SetFocusOnError="True" ValidationGroup="note"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender
                                                        ID="vceCollectionNote" runat="server" Enabled="True"
                                                        PopupPosition="BottomLeft" TargetControlID="rfvCollectionNote" />
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <a href="#" runat="server" value="Add New" id="btnUpdateContact" ></a><%--onclick="getAccount();"--%>

                    <asp:ModalPopupExtender ID="mpeAddContact" BackgroundCssClass="ModalPopupBG"
                        runat="server" CancelControlID="btnCancel" OkControlID="btnOkay"
                        TargetControlID="btnUpdateContact" BehaviorID="programmaticContactModalPopupBehavior"
                        PopupControlID="pnlUpdateContact" Drag="true" PopupDragHandleControlID="PopupHeader" OnOkScript="ReloadPage();">
                    </asp:ModalPopupExtender>

                    <%--                    
                        <div class="popup_Buttons" style="display: none">
                        <input id="btnOkay" value="Done" type="button" />
                        <input id="btnCancel" value="Cancel" type="button" />
                        </div>--%>

                    <div id="pnlUpdateContact" class="table-subcategory" style="display: none;height:440px;width:410px;">
                        <div class="popup_Container">
                            <div class="popup_Body">
                                <div class="model-popup-body" style="padding-bottom: 24px;">
                                    <asp:Label CssClass="title_text" Style="float: left; font-size:13px;font-weight:bold;" ID="Label1" runat="server">Customer Contact</asp:Label>

                                    <div style="float: right;">
                                        <asp:LinkButton CssClass="save_button" ID="lnkSaveContact" Style="color: white;padding-right: 10px;" runat="server" 
                                            OnClick="lnkSaveContact_Click" TabIndex="38" CausesValidation="true" ValidationGroup="contact">
                                             Save </asp:LinkButton>
                                        <a class="close_button_Form" id="lnkContactClose" style="color: white;" onclick="cancel();">Close </a>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12 col-md-12" style="padding-left: 0px;padding-right: 0px;">
                                <div class="com-cont">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    Customer Name
                                                </div>
                                                <div class="fc-input" style="margin-top:5px;">
                                                    <asp:Label ID="lblCustomer1" runat="server" ></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdnOwner1"/>
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    Contact
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" ></asp:TextBox>
                                                   
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    Phone
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"
                                                        MaxLength="28" Placeholder="(xxx)xxx-xxxx  Ext: xx"></asp:TextBox>
                                                    
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    Fax
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                   
                                                </div>
                                            </div>
                                             <div class="form-col">
                                                <div class="fc-label">
                                                    Cellular
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtCell" runat="server" CssClass="form-control" placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                </div>
                                            </div>
                                             <div class="form-col">
                                                <div class="fc-label">
                                                    E-mail
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationGroup="contact"
                                                        ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email"
                                                        SetFocusOnError="True"
                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceEmail"
                                                        runat="server" Enabled="True" TargetControlID="revEmail">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                        TargetControlID="txtEmail">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div class="container">
                                    <div class="modal fade" id="myModalCS" role="dialog">
                                        <div class="modal-dialog">
                                            <!-- Modal content-->
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: #316b9d">
                                                    <label class="modal-title" style="color: white;font-size:large">Email Status</label>
                                                    <button class="close" data-dismiss="modal">&times;</button>

                                                </div>
                                                <div class="modal-body">
                                                    <label class="modal-title" style="font-size:medium">
                                                        Would you like to print customer statement for accounts with no EmailId ? 
                                                    </label>
                                                    <br />
                                                    <br />
                                                    <asp:Button ID="btnYes" Text="Yes" runat="server" OnClick="btnYes_Click" CssClass="btn btn-primary" />
                                                    <button type="button" class="btn btn-primary" data-dismiss="modal">No</button>
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                               <%--<asp:AsyncPostBackTrigger ControlID="lnkMailCustomerStatement" />--%>
                                <asp:AsyncPostBackTrigger ControlID="lnkMailCustomerStatement" />
                           <%--     <asp:AsyncPostBackTrigger ControlID="btnYes" />--%>
                                <%--<asp:AsyncPostBackTrigger ControlID="btnYes" />--%>
                           </Triggers>
                    </asp:UpdatePanel>
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="container">
                                    <div class="modal fade" id="myModalInvoice" role="dialog">
                                        <div class="modal-dialog">
                                            <!-- Modal content-->
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: #316b9d">
                                                    <label class="modal-title" style="color: white;font-size:large">Email Status</label>
                                                    <button class="close" data-dismiss="modal">&times;</button>

                                                </div>
                                                <div class="modal-body">
                                                    <label class="modal-title" style="font-size:medium">
                                                        Would you like to print invoices for accounts with no EmailId ? 
                                                    </label>
                                                    <br />
                                                    <br />
                                                    <asp:Button ID="btnYesInv" Text="Yes" runat="server" OnClick="btnYesInv_Click" CssClass="btn btn-primary" />
                                                    <button type="button" class="btn btn-primary" data-dismiss="modal">No</button>
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="lnkMailInvoice" />
                               <asp:AsyncPostBackTrigger ControlID="lnkMailInvoiceMain" />
                               <asp:AsyncPostBackTrigger ControlID="lnkMailInvoiceException" />
                                <asp:AsyncPostBackTrigger ControlID="btnYesInv" />
                           </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

