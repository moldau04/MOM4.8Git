<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MOMRadWindow.Master" CodeBehind="ServiceType.aspx.cs" Inherits="MOMWebApp.ServiceType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
   
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />

    <script type="text/javascript">
        function OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();
        }
</script>
    

     <script type="text/javascript"> 

         function myFunction() {
             var addeditcase = $('#<%=hdnAddEdit.ClientID%>').val(); 
             if (addeditcase != '0') {
                 var choice = window.confirm('Would you like to update all projects with these changes?');
                 if (choice == true) {
                     $('#<%=hdnFlage.ClientID%>').val('1'); 
                     return true;
                 }
                 else {
                     $('#<%=hdnFlage.ClientID%>').val('0'); 
                     return true;
                 }
             }
             else {
                 return true;
             }
             
         }

         function OpenServiceTypeWindowEdit() {
             
             $("#<%=RadGrid_ServiceType.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                    ID = $tr.find('span[id*=lblId]').text();
                   
                   
                });
            });
            if (ID != "") {
                $('#<%=txtServiceType.ClientID%>').val(ID); 
                        $('#<%=hdnAddEdit.ClientID%>').val(ID); 
                 var wnd = $find('<%=ServiceTypeWindow.ClientID %>');
                 wnd.set_title("Edit Service Type"); 
                 Materialize.updateTextFields();
             }
             else {
                 ChkWarning();
             }
         }
         function OpenServiceTypeWindowEditDoubleclick(ID) {

            <%-- $("#<%=RadGrid_ServiceType.ClientID %>").find('tr:not(:first,:last)').each(function () {
                 var $tr = $(this);
                 $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                     ID = $tr.find('span[id*=lblId]').text();


                 });
             });--%>
             if (ID != "") {
                 $('#<%=txtServiceType.ClientID%>').val(ID);
                $('#<%=hdnAddEdit.ClientID%>').val(ID);
                var wnd = $find('<%=ServiceTypeWindow.ClientID %>');
                 wnd.set_title("Edit Service Type");
                 Materialize.updateTextFields();
                 document.getElementById('<%=lnkEditService.ClientID%>').click();
             }
             else {
                 ChkWarning();
             }
         }
      

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <%-------$$$$$$$$$$$$$$$ RAD AJAX MANAGER  $$$$$$$$$$$$$$$-----%>

     <telerik:RadAjaxManager ID="RadAjaxManager_SerType" runat="server"  >
        <AjaxSettings>  
            <telerik:AjaxSetting AjaxControlID="lnkEditService">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ServiceTypeWindow" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            </AjaxSettings>
        
        <AjaxSettings>  
            <telerik:AjaxSetting AjaxControlID="lnkAddService">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ServiceTypeWindow" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager> 

   
      <script type="text/javascript">
    function OnClientItemsRequesting(sender, eventArgs) {
        var context = eventArgs.get_context();
        context["FilterString"] = eventArgs.get_text();
    }
</script>
    <script type="text/javascript">
        function onLoad(sender) {
            var div = sender.get_element();

            //$telerik.$(div).bind('mouseenter', function () {
            //    if (!sender.get_dropDownVisible())
            //        sender.showDropDown();
            //});


            $telerik.$(".RadComboBoxDropDown").mouseleave(function (e) {
                hideDropDown("#" + sender.get_id(), sender, e);
            });


            $telerik.$(div).mouseleave(function (e) {
                hideDropDown(".RadComboBoxDropDown", sender, e);
            });

        }

        function hideDropDown(selector, combo, e) {
            var tgt = e.relatedTarget;
            var parent = $telerik.$(selector)[0];
            var parents = $telerik.$(tgt).parents(selector);

            if (tgt != parent && parents.length == 0) {
                if (combo.get_dropDownVisible())
                    combo.hideDropDown();
            }
        }
</script>
     
       
      <div class="tabs-custom-mgn1 mn-ht">
                                                                        <div class="form-section-row">
                                                                            <div class="btnlinks">
                                                                                <asp:LinkButton ID="lnkAddService" runat="server"  OnClick="lnkAddService_Click"  >Add</asp:LinkButton>
                                                                            </div>
                                                                            <div class="btnlinks">
                                                                                <asp:LinkButton ID="lnkEditService" runat="server" OnClick="lnkEditService_Click"  OnClientClick="OpenServiceTypeWindowEdit(this); ">Edit</asp:LinkButton>
                                                                            </div>
                                                                            <div class="btnlinks">
                                                                                <asp:LinkButton ID="lnkDeleteService" runat="server" CausesValidation="false" OnClientClick="return confirm('Do you really want to delete this Service type ?');" OnClick="lnkDeleteService_Click">Delete</asp:LinkButton>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-section-row mt-10">
                                                                            <div class="grid_container">
                                                                                <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                                                        <telerik:RadCodeBlock ID="RadCodeBlock15" runat="server">
                                                                                            <script type="text/javascript">
                                                                                                function pageLoad() {
                                                                                                    var grid = $find("<%= RadGrid_ServiceType.ClientID %>");
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
                                                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel14" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Setup" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_ServiceType" AllowFilteringByColumn="true" ShowFooter="True" PageSize="25"
                                                                                                OnNeedDataSource="RadGrid_ServiceType_NeedDataSource" 
                                                                                                 OnItemCreated="RadGrid_ServiceType_ItemCreated"
                                                                                                PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_ServiceType_PreRender"
                                                                                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                                                <CommandItemStyle />
                                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                </ClientSettings>
                                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                                    <Columns>
                                                                                                        <telerik:GridClientSelectColumn UniqueName="chkSelect"  HeaderStyle-Width="40">
                                                                                                        </telerik:GridClientSelectColumn>
                                                                                                        <telerik:GridTemplateColumn   Display="false" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblInvID" runat="server" Text='<%# Eval("InvID") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblInvName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblRT" runat="server" Text='<%# Eval("RT") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblOT" runat="server" Text='<%# Eval("OT") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblNT" runat="server" Text='<%# Eval("NT") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblDT" runat="server" Text='<%# Eval("DT") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblStatusVal" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn   DataField="Type" HeaderText="Service Type" SortExpression="Type" UniqueName="Type"
                                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn   DataField="fdesc" HeaderText="Description" UniqueName="fdesc" SortExpression="fdesc"
                                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn   DataField="Count" HeaderText="No. of Equipment" UniqueName="Count" SortExpression="Count"
                                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Count") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn   DataField="StatusLabel" HeaderText="Status" UniqueName="StatusLabel" SortExpression="StatusLabel"
                                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("StatusLabel") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                    </Columns>
                                                                                                </MasterTableView>
                                                                                            </telerik:RadGrid>
                                                                                        </telerik:RadAjaxPanel>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>


      <telerik:RadWindowManager ID="RadWindowManager18" runat="server">
        <Windows>
            <telerik:RadWindow ID="ServiceTypeWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="480"
                runat="server" Modal="true" Width="850" Height="700">
                <ContentTemplate>
                    <%--<div style="margin-top: 5px;">--%>
                        <div class="form-section-row" style="margin-bottom:0px !important;">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtServiceType"
                                            Display="None" ErrorMessage="Type Required" SetFocusOnError="True" ValidationGroup="service"></asp:RequiredFieldValidator>

                                        

                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender15" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                            TargetControlID="RequiredFieldValidator16">
                                        </asp:ValidatorCalloutExtender>

                                        <asp:TextBox ID="txtServiceType" runat="server" MaxLength="15"></asp:TextBox>
                                        <label for="txtServiceType">Type</label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6"   runat="server" ControlToValidate="txtServiceTypeDesc"
                                             ErrorMessage="Description Required" SetFocusOnError="True" ValidationGroup="service"></asp:RequiredFieldValidator>

                                        

                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                            TargetControlID="RequiredFieldValidator16">
                                        </asp:ValidatorCalloutExtender>

                                        <asp:TextBox ID="txtServiceTypeDesc" runat="server" MaxLength="75"></asp:TextBox>
                                        <label for="txtServiceTypeDesc">Description</label>
                                    </div>
                                </div>
                            </div>
                        </div>





                            
                        



                       <div class="form-section-row" style="margin-bottom:0px !important;">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">

                                        
                                        <label for="ddlLocationtype"  class="drpdwn-label"> Location Type</label>
                                   

                                        <telerik:RadComboBox RenderMode="Auto" ID="ddlLocationtype1" runat="server" Width="400" Height="150"
                                        EmptyMessage="Select" EnableLoadOnDemand="true" ShowMoreResultsBox="true"  OnClientLoad="onLoad"
                                        EnableVirtualScrolling="true" style="padding-top:10px;" >
                                        <WebServiceSettings Method="GetAddEditLocationtypeDLLData" Path="MOMCallWebApi.asmx" />
                                        </telerik:RadComboBox>

                                    <%--      <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                            ControlToValidate="ddlLocationtype1"  ErrorMessage="Location Type Required"
                                            SetFocusOnError="True" ValidationGroup="service"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender
                                                ID="ValidatorCalloutExtender4" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                TargetControlID="RequiredFieldValidator4">
                                            </asp:ValidatorCalloutExtender>--%>

                                   
 
                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                    <label class="drpdwn-label">Status</label>
                                        <asp:DropDownList ID="ddlServiceTypeStatus" runat="server" CssClass="browser-default">
                                            <asp:ListItem Value="0">Active</asp:ListItem>
                                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>


                      <div class="form-section-row" style="margin-bottom:0px !important;">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                 


                                            <label class="drpdwn-label">RT</label>
                                        <%--<asp:DropDownList ID="ddlRT" runat="server" CssClass="browser-default" CausesValidation="false" Visible="false"></asp:DropDownList>--%>

                                        <telerik:RadComboBox RenderMode="Auto" ID="ddlRT1" runat="server" Width="400" Height="150"
                                        EmptyMessage="Select" EnableLoadOnDemand="true" ShowMoreResultsBox="true" OnClientLoad="onLoad" 
                                        EnableVirtualScrolling="true" style="padding-top:10px;"                                      
    onclientitemsrequesting="OnClientItemsRequesting" >
                                        <WebServiceSettings Method="GetAddEditRTDLLData" Path="MOMCallWebApi.asmx"  />
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">OT</label>
                                        <%--<asp:DropDownList ID="DDlot" runat="server" CssClass="browser-default"></asp:DropDownList>--%>

                                        <telerik:RadComboBox RenderMode="Auto" ID="DDlot1" runat="server" Width="400" Height="150"
                                        EmptyMessage="Select" EnableLoadOnDemand="true" ShowMoreResultsBox="true"  OnClientLoad="onLoad"
                                        EnableVirtualScrolling="true" style="padding-top:10px;" >
                                        <WebServiceSettings Method="GetAddEditDDlotDLLData" Path="MOMCallWebApi.asmx" />
                                        </telerik:RadComboBox>

                                    </div>
                                </div>
                            </div>
                        </div>


                       <div class="form-section-row" style="margin-bottom:0px !important;">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                             <label class="drpdwn-label">1.7</label>
                                       
                                        <telerik:RadComboBox RenderMode="Auto" ID="ddl1Point71" runat="server" Width="400" Height="150"
                                        EmptyMessage="Select" EnableLoadOnDemand="true" ShowMoreResultsBox="true"  OnClientLoad="onLoad"
                                        EnableVirtualScrolling="true" style="padding-top:10px;" >
                                        <WebServiceSettings Method="GetAddEditddl1Point7DLLData" Path="MOMCallWebApi.asmx" />
                                        </telerik:RadComboBox>
                                        
                                    </div>
                                </div>
                            </div>
                       
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                       
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                               <label class="drpdwn-label">DT</label>
                                     
                                        <telerik:RadComboBox RenderMode="Auto" ID="ddlDT1" runat="server" Width="400" Height="150"
                                        EmptyMessage="Select" EnableLoadOnDemand="true" ShowMoreResultsBox="true"  OnClientLoad="onLoad"
                                        EnableVirtualScrolling="true" style="padding-top:10px;" >
                                        <WebServiceSettings Method="GetAddEditddlDTDLLData" Path="MOMCallWebApi.asmx" />
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>


                       <div class="form-section-row" style="margin-bottom:0px !important;">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">


                                              <label id="lblroutelabel" runat="server" for="ddlRoute"  class="drpdwn-label">  </label>                                          
                                    
                                            <telerik:RadComboBox ID="ddlRoute" BackColor="White"  style="width:100%!important;" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" MaxHeight="150" OnClientLoad="onLoad">
                                            </telerik:RadComboBox> 


      
                                     
                                      
                                       
                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                   
                                          <label for="ddlLocationtype"  class="drpdwn-label"> Department</label>
                                

                                        <telerik:RadComboBox ID="ddldepartment1" BackColor="White"  style="width:100%!important;" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" MaxHeight="150" OnClientLoad="onLoad">
                                            </telerik:RadComboBox>

                                    </div>
                                </div>
                            </div>
                        </div>
                    

                        <div class="form-section-row" style="margin-bottom:0px !important;">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                          <label for="ddlBillingCode"  class="drpdwn-label"> Billing Code</label>
                                    
                                        <telerik:RadComboBox RenderMode="Auto" ID="ddlBillingCode1" runat="server" Width="400" Height="150"
                                        EmptyMessage="Select" EnableLoadOnDemand="true" ShowMoreResultsBox="true" OnClientLoad="onLoad" 
                                        EnableVirtualScrolling="true" style="padding-top:10px;" >
                                        <WebServiceSettings Method="GetAddEditddlBillingCodeDLLData" Path="MOMCallWebApi.asmx" />
                                        </telerik:RadComboBox>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="ddlBillingCode1"   ErrorMessage=" Billing Code"
                                            SetFocusOnError="True" ValidationGroup="service"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender
                                                ID="ValidatorCalloutExtender1" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                TargetControlID="RequiredFieldValidator1">
                                            </asp:ValidatorCalloutExtender>
                                 
                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row"> 
                                         <label for="txtWageCategory"  class="drpdwn-label">  Wage Category  </label>
                                      
                                        <telerik:RadComboBox RenderMode="Auto" ID="ddlWC1" runat="server" Width="400" Height="150"
                                        EmptyMessage="Select" EnableLoadOnDemand="true" ShowMoreResultsBox="true"  OnClientLoad="onLoad"
                                        EnableVirtualScrolling="true" style="padding-top:10px;" >
                                        <WebServiceSettings Method="GetAddEditddlWCDLLData" Path="MOMCallWebApi.asmx" />
                                        </telerik:RadComboBox>

                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                            ControlToValidate="ddlWC1"  ErrorMessage="Wage Category Required"
                                            SetFocusOnError="True" ValidationGroup="service"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender
                                                ID="ValidatorCalloutExtender5" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                TargetControlID="RequiredFieldValidator5">
                                            </asp:ValidatorCalloutExtender>
                                       
                                    </div>
                                </div>
                            </div>
                        </div>

                        
                        <div class="form-section-row" style="margin-bottom:0px !important;">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                          <label for="DDLEGL"  class="drpdwn-label"> Expense GL</label>
                                      
                                         <telerik:RadComboBox RenderMode="Auto" ID="DDLEGL1" runat="server" Width="400" Height="150"
                                        EmptyMessage="Select" EnableLoadOnDemand="true" ShowMoreResultsBox="true"  OnClientLoad="onLoad"
                                        EnableVirtualScrolling="true" style="padding-top:10px;" >
                                        <WebServiceSettings Method="GetAddEditDDLEGLDLLData" Path="MOMCallWebApi.asmx" />
                                        </telerik:RadComboBox>
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="DDLEGL1"  ErrorMessage="Expense GL Required"
                                            SetFocusOnError="True" ValidationGroup="service"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender
                                                ID="ValidatorCalloutExtender3" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                TargetControlID="RequiredFieldValidator3">
                                            </asp:ValidatorCalloutExtender>

                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                         <label for="txtBillingCode"  class="drpdwn-label"> Interest GL</label>
                                        
                                           <%--<asp:DropDownList ID="DDLIGL" runat="server" CssClass="browser-default">
                                        </asp:DropDownList>--%>
                                        <telerik:RadComboBox RenderMode="Auto" ID="DDLIGL1" runat="server" Width="400" Height="150"
                                        EmptyMessage="Select" EnableLoadOnDemand="true" ShowMoreResultsBox="true"  OnClientLoad="onLoad"
                                        EnableVirtualScrolling="true" style="padding-top:10px;" >
                                        <WebServiceSettings Method="GetAddEditDDLIGLDLLData" Path="MOMCallWebApi.asmx" />
                                        </telerik:RadComboBox>

                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="DDLIGL1"   ErrorMessage="Interest GL Required"
                                            SetFocusOnError="True" ValidationGroup="service"></asp:RequiredFieldValidator>
                                        
                                        <asp:ValidatorCalloutExtender
                                                ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                TargetControlID="RequiredFieldValidator2">
                                            </asp:ValidatorCalloutExtender>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>


                       <div class="form-section-row" style="margin-bottom:0px !important;">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                       
                                     <asp:TextBox ID="txtServRemarks" runat="server"></asp:TextBox>
                                        <label for="txtServRemarks">Remarks</label>

                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">

                                       
                                    </div>
                                </div>
                            </div>
                        </div>

                         

                        <div style="clear: both;"></div>
                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkServiceSave" runat="server" OnClientClick="return myFunction();" ValidationGroup="service" OnClick="lnkServiceSave_Click">Save</asp:LinkButton>
                                <asp:HiddenField ID="hdnAddEdit" runat="server" />
                                <asp:HiddenField ID="hdnFlage" runat="server" />
                                
                            </div>
                        </footer>
                    <%--</div>--%>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

     

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">

      
</asp:Content>