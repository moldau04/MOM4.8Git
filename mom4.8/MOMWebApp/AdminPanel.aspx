<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AdminPanel" Codebehind="~/AdminPanel.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <script type="text/javascript">
        //function AlertPing(control) {
           // var ddlGPSPing = document.getElementById(control.id);

         //   if (ddlGPSPing.value == 0) {
           //     alert('Setting the ping interval to zero will cause significant rise in the phone battery consumption. Recommended ping interval is 1 minute.');
         //   }
      //  }    

      //  function DeleteDatabase() {

      //  }
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">    
    <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-action-dns"></i>&nbsp;Database Maintenance</div>
                                    <div class="btnlinks">
                                  
                                        <asp:LinkButton ID="btnChangePassword" runat="server" Text="Change Password" OnClientClick="opencCeateForm();return false" Visible="false"/>
                                    </div>
                                  <%--  <div class="btnlinks">
                                        <asp:LinkButton ID="LinkButton3" runat="server" Text="GPS Settings" TabIndex="38"  OnClientClick="opencGPSForm();return false" Visible="false"></asp:LinkButton>
                                    </div>--%>                                    
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="lnkAddnew" runat="server" CausesValidation="False" OnClick="lnkAddnew_Click" Text="Add New Customer Database">
                                        </asp:LinkButton>
                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="lnkTS" Text="Add Existing Database"  runat="server" CausesValidation="False" OnClick="lnkTS_Click">Add TS Database</asp:LinkButton>
                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="lnkExisting" Text="Add Existing Database"  runat="server" CausesValidation="False" OnClick="lnkExisting_Click">Add Existing MOM Database</asp:LinkButton>
                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ToolTip="Delete" Text="Delete"
                                                ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="False"
                                                OnClientClick="javascript:return confirm('Do you want to delete selected database details? However database will not be deleted, you can again add database details from add existing database.')"></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <div class="btnclosewrap">
                                            <asp:LinkButton id="lnkClose" runat="server" tooltip="Close" causesvalidation="false" onclick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>
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
            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">
                    <telerik:RadAjaxManager ID="RadAjaxManager_SharedDocument" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Ap" LoadingPanelID="RadAjaxLoadingPanel_AP" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_AP" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_AP.ClientID %>");
                                    var columns = grid.get_masterTableView().get_columns();
                                    for (var i = 0; i < columns.length; i++) {
                                        columns[i].resizeToFit(false, true);
                                    }
                                }
                                var requestInitiator = null;
                                var selectionStart = null;

                                function requestStart(sender, args) {
                                    requestInitiator = document.activeElement.id;
                                    if (document.activeElement.tagName == "INPUT") {
                                        selectionStart = document.activeElement.selectionStart;
                                    }
                                }

                                function responseEnd(sender, args) {
                                    var element = document.getElementById(requestInitiator);
                                    if (element && element.tagName == "INPUT") {
                                        element.focus();
                                        element.selectionStart = selectionStart;
                                    }
                                }

                            </script>
                        </telerik:RadCodeBlock>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_AP" runat="server" LoadingPanelID="RadAjaxLoadingPanel_AP" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                           
                            <telerik:RadPersistenceManager ID="RadPersistence_AP" runat="server">
                                <PersistenceSettings>
                                    <telerik:PersistenceSetting ControlID="RadGrid_AP" />
                                </PersistenceSettings>
                            </telerik:RadPersistenceManager>
                             <telerik:RadGrid RenderMode="Auto" ID="RadGrid_AP" runat="server" CssClass="RadGrid_AP" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                OnNeedDataSource="RadGrid_AP_NeedDataSource" OnItemCreated="RadGrid_AP_ItemCreated" OnPreRender="RadGrid_AP_PreRender">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>

                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>
                                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnSelected" runat="server" />  
                                                <%--<asp:CheckBox ID="chkSelect" CssClass="chkSelect" runat="server"/>--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="dbname" HeaderText="Database Name" SortExpression="dbname" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDBName" runat="server" Text='<%# Eval("dbname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                          <telerik:GridTemplateColumn FilterDelay="5" DataField="companyname" HeaderText="Company Name" SortExpression="companyname" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("companyname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="Type" HeaderText="Type" SortExpression="Type" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                </FilterMenu>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>                      
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--<asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
        CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="pnlOverlay"
        PopupDragHandleControlID="programmaticPopupDragHandle"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlOverlay" Visible="false">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="pnlGPSSett" runat="server" Visible="false" Style="border: 1px solid #316b9d">
                    <div class="title_bar_popup">
                        <asp:Label CssClass="title_text" ID="Label2" Style="color: white" runat="server">GPS Settings</asp:Label>
                        <asp:LinkButton ID="lnkCloseGPS" runat="server" CausesValidation="False" OnClick="lnkCloseGPS_Click"
                            Style="float: right; color: #fff; margin-left: 10px; height: 16px;">Close</asp:LinkButton>
                        <asp:LinkButton ID="lnkGPS" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                            OnClick="lnkGPS_Click">Save</asp:LinkButton>
                    </div>

                    <div style="padding: 15px; background-color: white">
                        <table style="width: 350px; background-color: white; height: 70px;">
                            <tr>
                                <td>GPS ping interval
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGPSPing" runat="server" onchange="AlertPing(this)" class="form-control"
                                        ToolTip="This feature is for GPS tracking app. The interval selected will be the interval at which the GPS tracking app pings the GPS satellites to get the location. Maximum the time is minimum will be the phone battery consumption. Setting the interval to zero will cause huge amout of phone battery use. Recommended interval is 1 minute.">
                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="30 Second" Value="30000"></asp:ListItem>
                                        <asp:ListItem Text="1 Minute" Value="60000"></asp:ListItem>
                                        <asp:ListItem Text="2 Minutes" Value="120000"></asp:ListItem>
                                        <asp:ListItem Text="3 Minutes" Value="180000"></asp:ListItem>
                                        <asp:ListItem Text="4 Minutes" Value="240000"></asp:ListItem>
                                        <asp:ListItem Text="5 Minutes" Value="300000"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Label ID="lblMSgGPS" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlContact" runat="server" Visible="false" Style="border: 1px solid #316b9d">
                    <div class="title_bar_popup">
                        <asp:Label CssClass="title_text" ID="Label1" Style="color: white" runat="server">Change Password</asp:Label>
                        <asp:LinkButton ID="lnkCancelContact" runat="server" CausesValidation="False" OnClick="LinkButton2_Click"
                            Style="float: right; color: #fff; margin-left: 10px; height: 16px;">Close</asp:LinkButton>
                        <asp:LinkButton ID="lnkContactSave" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                            OnClick="lnkContactSave_Click"
                            ValidationGroup="cont">Save</asp:LinkButton>
                    </div>
                    <div align="center">
                        <asp:Label ID="Label4" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                    </div>
                    <div style="padding: 15px; background-color: white">
                        <table style="width: 350px; background-color: white; height: 120px;">
                            <tr>
                                <td>User Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdminUser" runat="server" class="form-control" placeholder="Username"
                                        ToolTip="Username"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Password
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdminPass" runat="server" class="form-control" placeholder="Password"
                                        ToolTip="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Label ID="lblMsgAdmin" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Panel>--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadChangePasswordWindow" Skin="Material" VisibleTitlebar="true" Title="Change Password" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="400" Height="270">
                <ContentTemplate>                                                                   
                    <div style="margin-top: 15px; font-size: 1.1em !important;">
                        <table>
                            <tr>
                                <td>User Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdminUser" runat="server" class="form-control" placeholder="Username"
                                        ToolTip="Username"></asp:TextBox>
                                </td>
                            </tr>
                            <tr  style="background:#fff !important;">
                                <td>Password
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdminPass" runat="server" class="form-control" placeholder="Password"
                                        ToolTip="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Label ID="lblMsgAdmin" CssClass="lblMsg" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div style="clear: both;"></div>

                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                <%--<asp:LinkButton ID="lnkSearch" CommandName="GenerateBudget" OnClick="lnkSearch_Click" runat="server">Load</asp:LinkButton>--%>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkContactSave_Click"
                                ValidationGroup="cont">Save</asp:LinkButton>
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

           
        </Windows>
    </telerik:RadWindowManager>
<asp:HiddenField runat="server" ID="hdnRcvPymtSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function opencCeateForm() {
            window.radopen(null, "RadChangePasswordWindow");
        }
      //  function opencGPSForm() {
          //  window.radopen(null, "RadGPSWindow");
       // }
        $(document).ready(function () {


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
    <script>
      
    </script>
</asp:Content>
