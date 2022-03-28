<%@ Page Title="" Language="C#" MasterPageFile="~/MOMRadWindow.Master" AutoEventWireup="true" Inherits="VendorTransactionHistory" Codebehind="VendorTransactionHistory.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
 <style>
     #ctl00_ContentPlaceHolder1_RadGridARInvoice_GridData{
         height:250px !important;
     }
     .RadGrid {
         height:350px !important;
     }
      .RadGrid_Material {
         height:350px !important;
     }
 </style>
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="container">
        <div class="row2">             
                <div class="grid_container">                   
                        <div class="RadGrid RadGrid_Material ">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGridARInvoice"
                                
                                AllowFilteringByColumn="true"
                                ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" Height="100%"
                                PagerStyle-AlwaysVisible="true"                       
                                EnableLinqExpressions="false"
                                OnNeedDataSource="RadGridARInvoice_NeedDataSource"                              
                                >
								  <GroupingSettings CaseSensitive="false" />
                               <ClientSettings>
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true"/>
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                        
                                    </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="true" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No records to display.">
                                    <Columns>
                                           <telerik:GridTemplateColumn HeaderText="Bill/Check" HeaderStyle-Width="120px" SortExpression="Ref"  Reorderable="true">
                                            <ItemTemplate>         
                                                     <asp:HyperLink ID="lnkRef" runat="server"  Text='<%# Bind("ref") %>' NavigateUrl='<%# Bind("LinkTo") %>' Target="_blank"></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="100px"  SortExpression="fDate"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="100px"  SortExpression="fDate"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="200px" SortExpression="CheckNo"  Reorderable="true">
                                            <ItemTemplate>
                                           
                                                <asp:Label ID="lblCheckNo" runat="server" Text='<%# (Eval("fdesc").ToString().Length > 50) ? (Eval("fdesc").ToString().Substring(0, 50) + "...") : Eval("fdesc")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                     <%--    <telerik:GridTemplateColumn UniqueName="Original" DataField="Original" FooterAggregateFormatString="{0:c}"  SortExpression="Original" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Original" ShowFilterIcon="false" HeaderStyle-Width="120"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOriginal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Original", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                        <telerik:GridTemplateColumn UniqueName="Amount" DataField="Amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false" HeaderStyle-Width="120"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesTax" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>                 

                </div>       
        </div>

    </div>
</asp:Content>


