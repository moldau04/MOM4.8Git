<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="ProjectExpensesDetails" Codebehind="ProjectExpensesDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">

        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server"></asp:Label>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <asp:GridView ID="gvJobCostInvoice" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-bordered table-striped table-condensed flip-content"
                        GridLines="None" Width="100%" ShowHeaderWhenEmpty="false" EmptyDataText="No Invoices Found" ShowFooter="true">
                        <RowStyle CssClass="evenrowcolor" />
                        <FooterStyle CssClass="footer" />
                        <SelectedRowStyle CssClass="selectedrowcolor" />
                        <AlternatingRowStyle CssClass="oddrowcolor" />
                        <Columns>
                            <asp:TemplateField HeaderText="Ref #" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") == DBNull.Value ? " - " : Eval("Ref") %>' Style="display: none;"></asp:Label>
                                    <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                        NavigateUrl='<%# Eval("Url").ToString()+ "&page=addproject&pid="+ Request.QueryString["uid"].ToString() %>' ForeColor="#0066CC"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("fDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="VendorName" HeaderText="Vendor" Visible="true" ItemStyle-Width="15%"
                                ItemStyle-BackColor="White" FooterStyle-BackColor="White" />
                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="20%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("fDesc") == DBNull.Value ? " - " : Eval("fDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Op Sequence" ItemStyle-Width="10%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpSeq" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                    <asp:HiddenField ID="hdnJobType" Value='<%# Bind("type") %>' runat="server" />
                                    <!-- revenue and expense jobtype-->
                                    <asp:HiddenField ID="hdnItem" Value='<%# Eval("Type").Equals(1) ? Eval("MatItem") : Eval("MTypeID") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type" ItemStyle-Width="10%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("TypeDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item" ItemStyle-Width="15%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblItem" runat="server" Text='<%# Eval("ItemDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budgeted Labor" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetLabor" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BLabExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BLabExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budgeted Material" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetMat" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BMatExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BMatExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gvJobCostTicket" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-bordered table-striped table-condensed flip-content"
                        GridLines="None" Width="100%" ShowHeaderWhenEmpty="false" EmptyDataText="No Tickets Found" ShowFooter="true">
                        <RowStyle CssClass="evenrowcolor" />
                        <FooterStyle CssClass="footer" />
                        <SelectedRowStyle CssClass="selectedrowcolor" />
                        <AlternatingRowStyle CssClass="oddrowcolor" />
                        <Columns>
                            <asp:TemplateField HeaderText="Ticket#" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblTicket" runat="server" Text='<%# Bind("TicketID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Text='<%# Bind("TypeValue") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item" ItemStyle-Width="20%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblItem" runat="server" Text='<%# Bind("Item") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Op Sequence" ItemStyle-Width="10%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpSeq" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estimated Hours" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstHr" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Est"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Est", "{0:0.00}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalEstHr" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actual Hours" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblActualHr" runat="server" ForeColor='<%# Convert.ToDouble(Eval("ActualHr"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ActualHr", "{0:0.00}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalActualHr" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budgeted Hours" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetedHr" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BudgetHr"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BudgetHr", "{0:0.00}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Labor Expense" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblLaborExp" runat="server" ForeColor='<%# Convert.ToDouble(Eval("LaborExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "LaborExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalLaborExp" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Expenses" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblExp" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Expenses"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Expenses", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblOtherExp" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Expenses" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTExp" runat="server" ForeColor='<%# Convert.ToDouble(Eval("TotalExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "TotalExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalExp" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budgeted Material" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetMatExp" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BMatExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BMatExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budgeted Labor" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetLabExp" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BLabExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BLabExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Budgeted" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalBudget" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BudgetExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BudgetExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-BackColor="White" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gvJobCostJe" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-bordered table-striped table-condensed flip-content"
                        GridLines="None" Width="100%" ShowHeaderWhenEmpty="false" EmptyDataText="No JE Found" ShowFooter="true">
                        <RowStyle CssClass="evenrowcolor" />
                        <FooterStyle CssClass="footer" />
                        <SelectedRowStyle CssClass="selectedrowcolor" />
                        <AlternatingRowStyle CssClass="oddrowcolor" />
                        <Columns>
                            <asp:TemplateField HeaderText="Ref #" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") == DBNull.Value ? " - " : Eval("Ref") %>' Style="display: none;"></asp:Label>
                                    <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                        NavigateUrl='<%# Eval("Url").ToString()+ "&page=addproject&pid="+ Request.QueryString["uid"].ToString() %>' ForeColor="#0066CC"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblfDate" runat="server" Text='<%# Eval("fDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="35%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblfDesc" runat="server" Text='<%# Eval("fDesc") == DBNull.Value ? " - " : Eval("fDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Op Sequence" ItemStyle-Width="10%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpSeq" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type" ItemStyle-Width="10%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("ExpType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item" ItemStyle-Width="15%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblItem" runat="server" Text='<%# Eval("MatItem") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budgeted Labor" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetLabor" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BLabExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BLabExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budgeted Material" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetMat" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BMatExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BMatExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gvJobCostPO" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-bordered table-striped table-condensed flip-content"
                        GridLines="None" Width="100%" ShowHeaderWhenEmpty="false" EmptyDataText="No POs Found" ShowFooter="true">
                        <RowStyle CssClass="evenrowcolor" />
                        <FooterStyle CssClass="footer" />
                        <SelectedRowStyle CssClass="selectedrowcolor" />
                        <AlternatingRowStyle CssClass="oddrowcolor" />
                        <Columns>
                            <asp:TemplateField HeaderText="PO" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblPO" runat="server" Text='<%# Eval("PO") == DBNull.Value ? " - " : Eval("PO") %>' Style="display: none;"></asp:Label>
                                    <asp:HyperLink ID="hlPO" runat="server" Text='<%# Bind("PO") %>' Target="_blank"
                                        NavigateUrl='<%# Eval("Url").ToString()+ "&page=addproject&pid="+ Request.QueryString["uid"].ToString() %>' ForeColor="#0066CC"></asp:HyperLink>
                                    <asp:HiddenField ID="hdnJobType" Value='<%# Bind("type") %>' runat="server" />
                                    <!-- revenue and expense jobtype-->

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" ItemStyle-Width="8%" ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                    <asp:Label ID="lblPODate" runat="server" Text='<%# Eval("fDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="VendorName" HeaderText="Vendor" ItemStyle-Width="15%"
                                ItemStyle-BackColor="White" FooterStyle-BackColor="White" />
                            <asp:BoundField DataField="fDesc" HeaderText="Description" ItemStyle-Width="20%"
                                ItemStyle-BackColor="White" FooterStyle-BackColor="White" />
                            <asp:BoundField DataField="Code" HeaderText="Op Sequence" ItemStyle-Width="10%"
                                ItemStyle-BackColor="White" FooterStyle-BackColor="White" />
                            <asp:BoundField DataField="TypeDesc" HeaderText="Type" ItemStyle-Width="10%"
                                ItemStyle-BackColor="White" FooterStyle-BackColor="White" />
                            <asp:BoundField DataField="ItemDesc" HeaderText="Item" ItemStyle-Width="15%"
                                ItemStyle-BackColor="White" FooterStyle-BackColor="White" />
                            <asp:TemplateField HeaderText="Balance" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%"
                                ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White"
                                FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBalance" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budgeted Labor" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%"
                                ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White"
                                FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetLabor" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BLabExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BLabExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budgeted Material" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%"
                                ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="White"
                                FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetMat" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BMatExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BMatExp", "{0:c}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </div>
            </div>
        </div>
        <div class="clearfix"></div>
    </div>
</asp:Content>

