<%@ Page Title="" Language="C#" MasterPageFile="~/MOMRadWindow.Master" AutoEventWireup="true" Inherits="ProjectBudgetDetail" Codebehind="ProjectBudgetDetail.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
   
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height: 65px !important;">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                       <div class="page-title"><i class="mdi-editor-attach-money"></i>&nbsp; <span runat="server" id="spanProjectName"></span></div>
                                     
                                    <div class="rght-content">
                                        <div class="editlabel">    <span runat="server" id="spanProjectNo" style="font-size:18px;font-weight:900;"></span>&nbsp;</div>
                                        
                                        
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
          
  
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>

             <%---- Revenue------%>

                <div class="grid_container" id="divrevenue" runat="server">  
                       <div class="container">
        <div class="row">
            <div class="card" style="min-height: 70vh !important; border-radius: 6px; margin-top: -10px;">
                <div class="card-content">
                    <ul class="tabs tab-demo-active white" style="width: 100%;">
                        <li class="tab col s2">
                            <a class="white-text waves-effect waves-light active" href="#activeoneRevenue"><i class="mdi-notification-sync-problem"></i>&nbsp;AR Invoices</a>
                        </li> 

                          <li class="tab col s2">
                            <a class="white-text waves-effect waves-light" id="tabRevenueJE"   href="#RevenueJE"><i class="mdi-notification-sync-problem"></i>&nbsp;Journal Entries</a>
                        </li>

                    </ul>

                    <div id="activeoneRevenue" class="col s12 tab-container-border lighten-4" style="display: block;">

                        <div> 
      <div class="page-title" id="Div2" runat="server" ><i class="mdi-content-inbox"></i>&nbsp; AP Invoices</div>
                     <div class="grid_container" >  
                <div class="RadGrid RadGrid_Material "> 
                   <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">

                    <telerik:RadGrid ID="RadGridARInvoice"  runat="server" AutoGenerateColumns="false"
                        Width="100%" ShowHeaderWhenEmpty="false" 
                        NoMasterRecordsText="No Invoices Found" 
                        EmptyDataText="No Invoices Found" 
                        OnNeedDataSource="RadGridARInvoice_NeedDataSource"
                        ShowFooter="true"                        
                        AllowCustomPaging="false" PageSize="15"
                        AllowPaging="true"
                        AllowFilteringByColumn="True"
                        AllowSorting="true"
                        PagerStyle-AlwaysVisible="true"
                        >
                        <MasterTableView 
                            AutoGenerateColumns="false"
                            AllowSorting="true"   
                            AllowFilteringByColumn="true" 
                            ShowFooter="true" 
                            AllowPaging="true" 
                            ShowHeadersWhenNoRecords="true" 
                            NoMasterRecordsText="No Invoices Found">
                            <Columns>


                                <telerik:GridTemplateColumn HeaderText="Ref #" 
                                    
                                       UniqueName="Ref" 
                                    DataField="Ref"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                    AllowSorting="true"
                                    SortExpression="ref"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") == DBNull.Value ? " - " : Eval("Ref") %>' Style="display: none;"></asp:Label>
                                        <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                            NavigateUrl='<%# Eval("Url").ToString()+ "&page=addproject&pid="+ Request.QueryString["uid"].ToString() %>' ForeColor="#0066CC"></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            
                                 
                                <telerik:GridTemplateColumn HeaderText="Date"    
                                      
                                        UniqueName="fDate" 
                                    DataField="fDate"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                       AllowSorting="true"
                                    SortExpression="fDate"
                                      ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("fDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 

                                <telerik:GridTemplateColumn HeaderText="Desc" 
                                       
                                         UniqueName="fdesc" 
                                    DataField="fdesc"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                        AllowSorting="true"
                                    SortExpression="fdesc"
                                       ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn HeaderText="Amount($)"   
                                     
                                         UniqueName="Amount" 
                                    DataField="Amount"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="EqualTo"
                                    DataType="System.Decimal"
                                     DataFormatString="{0:C}"
                                    Aggregate="Sum" Visible="true" 
                                     FilterControlAltText="Filter Amount column"  
                                 AllowSorting="true"
                                    SortExpression="Amount"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>

                         </telerik:RadAjaxPanel>
                </div>
                         </div>
                        </div>

                      </div>
                        
                    

                     <div id="RevenueJE" class="col s12 tab-container-border lighten-4"> 

                           <div class="page-title" id="Div6" runat="server" ><i class="mdi-content-inbox"></i>&nbsp;Journal Entries</div>
                    <div class="grid_container" >  
                <div class="RadGrid RadGrid_Material ">

                    <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                    <telerik:RadGrid ID="gvJobCostRevenueJE"  
                        runat="server" 
                        AutoGenerateColumns="false"
                        Width="100%" 
                        ShowHeaderWhenEmpty="false"  
                        OnNeedDataSource="gvJobCostRevenueJE_NeedDataSource"
                        ShowFooter="true"                        
                        AllowCustomPaging="false" PageSize="15"
                        AllowPaging="true"
                        AllowFilteringByColumn="True"
                        AllowSorting="true"
                        PagerStyle-AlwaysVisible="true">
                       <MasterTableView 
                            AutoGenerateColumns="false"
                            AllowSorting="true"   
                            AllowFilteringByColumn="true" 
                            ShowFooter="true" 
                            AllowPaging="true" 
                            ShowHeadersWhenNoRecords="true" 
                            NoMasterRecordsText="No JE Found">
                            <Columns>
                                  <telerik:GridTemplateColumn HeaderText="Ref #" 
                                    
                                       UniqueName="Ref" 
                                    DataField="Ref"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                    AllowSorting="true"
                                    SortExpression="ref"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") == DBNull.Value ? " - " : Eval("Ref") %>' Style="display: none;"></asp:Label>
                                        <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                            NavigateUrl='<%# Eval("Url").ToString()+ "&page=addproject&pid="+ Request.QueryString["uid"].ToString() %>' ForeColor="#0066CC"></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            
                                 
                                <telerik:GridTemplateColumn HeaderText="Date"    
                                      
                                        UniqueName="fDate" 
                                    DataField="fDate"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                       AllowSorting="true"
                                    SortExpression="fDate"
                                      ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("fDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 

                                <telerik:GridTemplateColumn HeaderText="Desc" 
                                       
                                         UniqueName="fdesc" 
                                    DataField="fdesc"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                        AllowSorting="true"
                                    SortExpression="fdesc"
                                       ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>


                                <telerik:GridBoundColumn HeaderText="Amount($)"   
                                     
                                         UniqueName="Amount" 
                                    DataField="Amount"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="EqualTo"
                                    DataType="System.Decimal"
                                     DataFormatString="{0:C}"
                                    Aggregate="Sum" Visible="true" 
                                     FilterControlAltText="Filter Amount column"  
                                 AllowSorting="true"
                                    SortExpression="Amount"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                   </telerik:RadAjaxPanel>
                </div>
                        </div>
                             </div>

                    </div>
                </div>
            </div>
                           </div> 
              

        </div> 
               
            </div>  </div>
              <%-----Cost-----%>
                <div class="grid_container" id="divCost" runat="server">  
                       <div class="container">
        <div class="row">
            <div class="card" style="min-height: 70vh !important; border-radius: 6px; margin-top: -10px;">
                <div class="card-content">
                    <ul class="tabs tab-demo-active white" style="width: 100%;">
                        <li class="tab col s2">
                            <a class="white-text waves-effect waves-light active" href="#activeone"><i class="mdi-notification-sync-problem"></i>&nbsp;AP Invoices</a>
                        </li>
                        <li class="tab col s2">
                            <a class="white-text waves-effect waves-light" id="tabTickets"   href="#Ticket"><i class="mdi-notification-sync-problem"></i>&nbsp;Tickets</a>
                        </li>
                         <li class="tab col s2">
                            <a class="white-text waves-effect waves-light" id="tabPO"   href="#PO"><i class="mdi-notification-sync-problem"></i>&nbsp;Total On Order</a>
                        </li>
                         <li class="tab col s2">
                            <a class="white-text waves-effect waves-light" id="tabRPO"   href="#RPO"><i class="mdi-notification-sync-problem"></i>&nbsp;Receive PO</a>
                        </li>

                          <li class="tab col s2">
                            <a class="white-text waves-effect waves-light" id="tabJE"   href="#JE"><i class="mdi-notification-sync-problem"></i>&nbsp;Journal Entries</a>
                        </li>

                    </ul>

                    <div id="activeone" class="col s12 tab-container-border lighten-4" style="display: block;">

                        <div> 
      <div class="page-title" id="DivAPInvoices" runat="server" ><i class="mdi-content-inbox"></i>&nbsp; AP Invoices</div>
                     <div class="grid_container" >  
                <div class="RadGrid RadGrid_Material "> 
                    <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">

                    <telerik:RadGrid ID="gvJobCostInvoice"  runat="server" 
                        AutoGenerateColumns="false"
                        OnNeedDataSource="gvJobCostInvoice_NeedDataSource"
                         ShowFooter="true"                        
                        AllowCustomPaging="false" PageSize="15"
                        AllowPaging="true"
                        AllowFilteringByColumn="True"
                        AllowSorting="true"
                        PagerStyle-AlwaysVisible="true"
                        Width="100%" ShowHeaderWhenEmpty="false" NoMasterRecordsText="No AP Invoices Found" EmptyDataText="No AP Invoices Found"  >
                         <MasterTableView 
                            AutoGenerateColumns="false"
                            AllowSorting="true"   
                            AllowFilteringByColumn="true" 
                            ShowFooter="true" 
                            AllowPaging="true" 
                            ShowHeadersWhenNoRecords="true" 
                            NoMasterRecordsText="No AP Invoices Found">
                            <Columns>
                           
                                 
                                <telerik:GridTemplateColumn HeaderText="Ref #" 
                                    
                                       UniqueName="Ref" 
                                    DataField="Ref"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                    AllowSorting="true"
                                    SortExpression="ref"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") == DBNull.Value ? " - " : Eval("Ref") %>' Style="display: none;"></asp:Label>
                                        <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                            NavigateUrl='<%# Eval("Url").ToString()+ "&page=addproject&pid="+ Request.QueryString["uid"].ToString() %>' ForeColor="#0066CC"></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            
                                 
                                <telerik:GridTemplateColumn HeaderText="Date"    
                                      
                                        UniqueName="fDate" 
                                    DataField="fDate"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                       AllowSorting="true"
                                    SortExpression="fDate"
                                      ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("fDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 

                                <telerik:GridTemplateColumn HeaderText="Desc" 
                                       
                                         UniqueName="fdesc" 
                                    DataField="fdesc"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                        AllowSorting="true"
                                    SortExpression="fdesc"
                                       ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>


                              
                                   <telerik:GridTemplateColumn HeaderText="VendorName"   
                                       
                                            UniqueName="VendorName" 
                                    DataField="VendorName"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                       AllowSorting="true"
                                    SortExpression="VendorName"
                                       
                                       ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVendorName" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridBoundColumn HeaderText="Amount($)"   
                                     
                                         UniqueName="Amount" 
                                    DataField="Amount"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="EqualTo"
                                    DataType="System.Decimal"
                                     DataFormatString="{0:C}"
                                    Aggregate="Sum" Visible="true" 
                                     FilterControlAltText="Filter Amount column"  
                                 AllowSorting="true"
                                    SortExpression="Amount"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                </telerik:GridBoundColumn>
                               
                                
                                 

                              
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid> 

                        </telerik:RadAjaxPanel>
                </div>
                         </div>
                        </div>

                      </div>
                        
                     <div id="Ticket" class="col s12 tab-container-border lighten-4">
                        <div class="page-title" id="DivTickets" runat="server" ><i class="mdi-device-access-time"></i>&nbsp;Tickets</div>
                    <div class="grid_container" >  
                <div class="RadGrid RadGrid_Material ">
                     
                    <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                    <telerik:RadGrid ID="gvJobCostTicket"   runat="server" AutoGenerateColumns="false"
                        Width="100%" ShowHeaderWhenEmpty="false" NoMasterRecordsText="No Ticket Found" EmptyDataText="No Ticket Found"                          
                         OnNeedDataSource="gvJobCostTicket_NeedDataSource"
                        ShowFooter="true"                        
                        AllowCustomPaging="false" PageSize="15"
                        AllowPaging="true"
                        AllowFilteringByColumn="True"
                        AllowSorting="true"
                        PagerStyle-AlwaysVisible="true"  >
                       

                               <MasterTableView 
                            AutoGenerateColumns="false"
                            AllowSorting="true"   
                            AllowFilteringByColumn="true" 
                            ShowFooter="true" 
                            AllowPaging="true" 
                            ShowHeadersWhenNoRecords="true" 
                            NoMasterRecordsText="No Ticket Found">

                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Ref #" 
                                    UniqueName="Ref" 
                                    DataField="Ref"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                    AllowSorting="true"
                                    SortExpression="Ref"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") == DBNull.Value ? " - " : Eval("Ref") %>' Style="display: none;"></asp:Label>
                                        <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                            NavigateUrl='<%# Eval("Url").ToString()+ "&page=addproject&pid="+ Request.QueryString["uid"].ToString() %>' ForeColor="#0066CC"></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                              
                             
                                 <telerik:GridTemplateColumn HeaderText="Date"    
                                      
                                        UniqueName="fDate" 
                                    DataField="fDate"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                      SortExpression="fDate"
                                      ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("fDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 

                                 <telerik:GridTemplateColumn HeaderText="Desc" 
                                       
                                         UniqueName="fdesc" 
                                    DataField="fdesc"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                     SortExpression="fdesc"
                                       ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn HeaderText="AssignedWorker"  
                                     
                                         UniqueName="AssignedWorker" 
                                    DataField="AssignedWorker"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                     SortExpression="AssignedWorker"
                                     ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("AssignedWorker") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 
                                 <telerik:GridBoundColumn HeaderText="Amount($)"   
                                     
                                         UniqueName="Amount" 
                                    DataField="Amount"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="EqualTo"
                                    DataType="System.Decimal"
                                     DataFormatString="{0:C}"
                                    Aggregate="Sum" Visible="true" 
                                     FilterControlAltText="Filter Amount column"  
                                 SortExpression="Amount"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                </telerik:GridBoundColumn>
 
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>   
                        </telerik:RadAjaxPanel>
                         
                </div>   
                        </div>
                        
                    </div> 
                      
                     <div id="PO" class="col s12 tab-container-border lighten-4"> 
                              <div class="page-title" id="DivPO" runat="server" ><i class="mdi-device-access-time"></i>&nbsp;Total On Order</div>
                    <div class="grid_container" >  
                <div class="RadGrid RadGrid_Material ">

                    <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                    <telerik:RadGrid ID="RadGridTotalOnOrder"   runat="server" AutoGenerateColumns="false"
                        Width="100%" ShowHeaderWhenEmpty="false" NoMasterRecordsText="No PO Found" EmptyDataText="No PO Found" 
                         OnNeedDataSource="RadGridTotalOnOrder_NeedDataSource"
                          ShowFooter="true"                        
                        AllowCustomPaging="false" PageSize="15"
                        AllowPaging="true"
                        AllowFilteringByColumn="True"
                        AllowSorting="true"
                        PagerStyle-AlwaysVisible="true"

                          >
                          <MasterTableView 
                            AutoGenerateColumns="false"
                            AllowSorting="true"   
                            AllowFilteringByColumn="true" 
                            ShowFooter="true" 
                            AllowPaging="true" 
                            ShowHeadersWhenNoRecords="true" 
                            NoMasterRecordsText="No PO Found">
                            <Columns>
                               <telerik:GridTemplateColumn HeaderText="Ref #" 
                                    
                                       UniqueName="Ref" 
                                    DataField="Ref"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                    AllowSorting="true"
                                    SortExpression="ref"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") == DBNull.Value ? " - " : Eval("Ref") %>' Style="display: none;"></asp:Label>
                                        <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                            NavigateUrl='<%# Eval("Url").ToString()+ "&page=addproject&pid="+ Request.QueryString["uid"].ToString() %>' ForeColor="#0066CC"></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            
                                 
                                <telerik:GridTemplateColumn HeaderText="Date"    
                                      
                                        UniqueName="fDate" 
                                    DataField="fDate"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                       AllowSorting="true"
                                    SortExpression="fDate"
                                      ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("fDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 

                                <telerik:GridTemplateColumn HeaderText="Desc" 
                                       
                                         UniqueName="fdesc" 
                                    DataField="fdesc"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                        AllowSorting="true"
                                    SortExpression="fdesc"
                                       ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                
                                   <telerik:GridTemplateColumn HeaderText="VendorName"   
                                       
                                            UniqueName="VendorName" 
                                    DataField="VendorName"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                       AllowSorting="true"
                                    SortExpression="VendorName"
                                       
                                       ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVendorName" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn HeaderText="Amount($)"   
                                     
                                         UniqueName="Amount" 
                                    DataField="Amount"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="EqualTo"
                                    DataType="System.Decimal"
                                     DataFormatString="{0:C}"
                                    Aggregate="Sum" Visible="true" 
                                     FilterControlAltText="Filter Amount column"  
                                 AllowSorting="true"
                                    SortExpression="Amount"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                </telerik:GridBoundColumn>
                                 
                               
                               </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>  
                        </telerik:RadAjaxPanel>
                </div>  
                        </div>

                             </div>

                     <div id="RPO" class="col s12 tab-container-border lighten-4"> 

                               <div class="page-title" id="DivReceivePO" runat="server" ><i class="mdi-content-reply-all"></i>&nbsp;Receive PO</div>
                    <div class="grid_container" >  
                <div class="RadGrid RadGrid_Material ">

                    <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                             <telerik:RadGrid ID="RadGrid1ReceivePO"   runat="server" AutoGenerateColumns="false"
                        Width="100%" ShowHeaderWhenEmpty="false" NoMasterRecordsText="No PO Found" EmptyDataText="No PO Found"  
                                  OnNeedDataSource="RadGrid1ReceivePO_NeedDataSource"
                        ShowFooter="true"                        
                        AllowCustomPaging="false" PageSize="15"
                        AllowPaging="true"
                        AllowFilteringByColumn="True"
                        AllowSorting="true"
                        PagerStyle-AlwaysVisible="true"
                                 
                                 >
                         <MasterTableView 
                            AutoGenerateColumns="false"
                            AllowSorting="true"   
                            AllowFilteringByColumn="true" 
                            ShowFooter="true" 
                            AllowPaging="true" 
                            ShowHeadersWhenNoRecords="true" 
                            NoMasterRecordsText="No RPO Found">
                            <Columns>
                             
                                  <telerik:GridTemplateColumn HeaderText="Ref #" 
                                    
                                       UniqueName="Ref" 
                                    DataField="Ref"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                    AllowSorting="true"
                                    SortExpression="ref"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") == DBNull.Value ? " - " : Eval("Ref") %>' Style="display: none;"></asp:Label>
                                        <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                            NavigateUrl='<%# Eval("Url").ToString()+ "&page=addproject&pid="+ Request.QueryString["uid"].ToString() %>' ForeColor="#0066CC"></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            
                                 
                                <telerik:GridTemplateColumn HeaderText="Date"    
                                      
                                        UniqueName="fDate" 
                                    DataField="fDate"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                       AllowSorting="true"
                                    SortExpression="fDate"
                                      ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("fDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 

                                <telerik:GridTemplateColumn HeaderText="Desc" 
                                       
                                         UniqueName="fdesc" 
                                    DataField="fdesc"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                        AllowSorting="true"
                                    SortExpression="fdesc"
                                       ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                   <telerik:GridTemplateColumn HeaderText="VendorName"   
                                       
                                            UniqueName="VendorName" 
                                    DataField="VendorName"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                       AllowSorting="true"
                                    SortExpression="VendorName"
                                       
                                       ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVendorName" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn HeaderText="Amount($)"   
                                     
                                         UniqueName="Amount" 
                                    DataField="Amount"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="EqualTo"
                                    DataType="System.Decimal"
                                     DataFormatString="{0:C}"
                                    Aggregate="Sum" Visible="true" 
                                     FilterControlAltText="Filter Amount column"  
                                 AllowSorting="true"
                                    SortExpression="Amount"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                </telerik:GridBoundColumn>

                             
                                 

                                
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>  
                        </telerik:RadAjaxPanel>
                </div>        
                        </div>
                             </div> 

                     <div id="JE" class="col s12 tab-container-border lighten-4"> 

                           <div class="page-title" id="DivJournalEntries" runat="server" ><i class="mdi-content-inbox"></i>&nbsp;Journal Entries</div>
                    <div class="grid_container" >  
                <div class="RadGrid RadGrid_Material ">

                    <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                    <telerik:RadGrid ID="gvJobCostJe"  
                        runat="server" 
                        AutoGenerateColumns="false"
                        Width="100%" 
                        ShowHeaderWhenEmpty="false"  
                        OnNeedDataSource="gvJobCostJe_NeedDataSource"
                        ShowFooter="true"                        
                        AllowCustomPaging="false" PageSize="15"
                        AllowPaging="true"
                        AllowFilteringByColumn="True"
                        AllowSorting="true"
                        PagerStyle-AlwaysVisible="true">
                       <MasterTableView 
                            AutoGenerateColumns="false"
                            AllowSorting="true"   
                            AllowFilteringByColumn="true" 
                            ShowFooter="true" 
                            AllowPaging="true" 
                            ShowHeadersWhenNoRecords="true" 
                            NoMasterRecordsText="No JE Found">
                            <Columns>
                                  <telerik:GridTemplateColumn HeaderText="Ref #" 
                                    
                                       UniqueName="Ref" 
                                    DataField="Ref"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                    AllowSorting="true"
                                    SortExpression="ref"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") == DBNull.Value ? " - " : Eval("Ref") %>' Style="display: none;"></asp:Label>
                                        <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                            NavigateUrl='<%# Eval("Url").ToString()+ "&page=addproject&pid="+ Request.QueryString["uid"].ToString() %>' ForeColor="#0066CC"></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            
                                 
                                <telerik:GridTemplateColumn HeaderText="Date"    
                                      
                                        UniqueName="fDate" 
                                    DataField="fDate"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                       AllowSorting="true"
                                    SortExpression="fDate"
                                      ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("fDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("fDate")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 

                                <telerik:GridTemplateColumn HeaderText="Desc" 
                                       
                                         UniqueName="fdesc" 
                                    DataField="fdesc"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains"
                                    DataType="System.String"
                                        AllowSorting="true"
                                    SortExpression="fdesc"
                                       ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>


                                <telerik:GridBoundColumn HeaderText="Amount($)"   
                                     
                                         UniqueName="Amount" 
                                    DataField="Amount"
                                     AllowFiltering="true" 
                                    ShowFilterIcon="false"   
                                    FilterControlWidth="50px" 
                                    AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="EqualTo"
                                    DataType="System.Decimal"
                                     DataFormatString="{0:C}"
                                    Aggregate="Sum" Visible="true" 
                                     FilterControlAltText="Filter Amount column"  
                                 AllowSorting="true"
                                    SortExpression="Amount"
                                    ItemStyle-BackColor="White" FooterStyle-BackColor="White">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                   </telerik:RadAjaxPanel>
                </div>
                        </div>
                             </div>

                    </div>
                </div>
            </div>
                           </div>
                    

          
               
               
           
               
              

        </div>
  
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#addinfo').hide();
            $('.add-btn-click').click(function () {
                $('#addinfo').slideToggle('2000', "swing", function () {
                    // Animation complete.
                });
                if ($('.divbutton-container').height() != 65)
                    $('.divbutton-container').animate({ height: 65 }, 500);
                else
                    $('.divbutton-container').animate({ height: 350 }, 500);
            });
            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

        });
    </script>
    <script type="text/javascript" lang="javascript">

</script>
</asp:Content>
