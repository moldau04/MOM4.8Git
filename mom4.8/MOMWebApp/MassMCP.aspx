<%@ Page Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="MassMCP" Title="Mass MCP - Mobile Office Manager 4.0" Codebehind="MassMCP.aspx.cs" %>

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
                    <span>Customer Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="/Equipments.aspx">Equipment</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Mass MCP</span>
                </li>
            </ul>
            <div class="page-bar-right">
                <asp:LinkButton CssClass="Close_button" ID="lnkClose" runat="server" OnClick="lnkClose_Click" ForeColor="Red" CausesValidation="false"><i class="fa fa-times"></i></asp:LinkButton>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Mass MCP</asp:Label>
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div class="col-lg-12">
                        <div class="form-col">
                            <asp:Label ID="lblMsg" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                            <p style="font-size: larger"><b>Equipment</b></p>
                            <div class="search-customer" style="padding-bottom: 20px">
                                <div class="sc-form">
                                    Search
                                    <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" CssClass="form-control input-sm input-small"
                                        OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                        <asp:ListItem Value="e.Unit">Name</asp:ListItem>
                                        <asp:ListItem Value="e.type">Type</asp:ListItem>
                                        <asp:ListItem Value="e.Cat">Service Type</asp:ListItem>
                                        <asp:ListItem Value="e.Status">Status</asp:ListItem>
                                        <asp:ListItem Value="r.name">Customer</asp:ListItem>
                                        <asp:ListItem Value="l.id">Location ID</asp:ListItem>
                                        <asp:ListItem Value="l.tag">Location</asp:ListItem>
                                        <asp:ListItem Value="address">Address</asp:ListItem>
                                        <asp:ListItem Value="e.Category">Category</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="rbStatus" runat="server" CssClass="form-control input-sm input-small"
                                        Visible="False">
                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control input-sm input-small" TabIndex="4"
                                        Visible="False" Width="200px">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control input-sm input-small"
                                        TabIndex="5" Visible="False" Width="200px">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control input-sm input-small"
                                        Width="200px" Visible="false" TabIndex="7">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtSearch" runat="server" Width="242px" CssClass="form-control input-sm input-small"></asp:TextBox>
                                    <%-- <asp:ImageButton ID="lnkSearch" runat="server" ImageUrl="images/search.png" OnClick="lnkSearch_Click"
                                    ToolTip="Search" />--%>
                                    <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server" CausesValidation="false"
                                        OnClick="lnkSearch_Click" ToolTip="Search"><i class="fa fa-search"></i></asp:LinkButton>
                                </div>
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                                    <li>
                                        <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All Equipment</asp:LinkButton>
                                    <li>
                                        <asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic;"></asp:Label>
                                    </li>
                                </ul>
                            </div>
                            <div class="search-customer">
                                <div class="sc-form">
                                    Manufacturer
                                <asp:TextBox ID="txtManufact" runat="server" Width="110px" CssClass="form-control"></asp:TextBox>
                                    Price
                                <asp:DropDownList ID="ddlCompareP" runat="server" CssClass="form-control"
                                    Width="60px">
                                    <asp:ListItem Value="0">=</asp:ListItem>
                                    <asp:ListItem Value="1">&gt;=</asp:ListItem>
                                    <asp:ListItem Value="2">&lt;=</asp:ListItem>
                                    <asp:ListItem Value="3">&gt;</asp:ListItem>
                                    <asp:ListItem Value="4">&lt;</asp:ListItem>
                                </asp:DropDownList>
                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" Width="110px"></asp:TextBox>
                                    Install date
                                <asp:DropDownList ID="ddlComareI" runat="server" CssClass="form-control"
                                    Width="60px">
                                    <asp:ListItem Value="0">=</asp:ListItem>
                                    <asp:ListItem Value="1">&gt;=</asp:ListItem>
                                    <asp:ListItem Value="2">&lt;=</asp:ListItem>
                                    <asp:ListItem Value="3">&gt;</asp:ListItem>
                                    <asp:ListItem Value="4">&lt;</asp:ListItem>
                                </asp:DropDownList>
                                    <asp:TextBox ID="txtInstallDt" runat="server" CssClass="form-control"
                                        Width="110px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtInstallDt_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtInstallDt">
                                    </asp:CalendarExtender>
                                    Last service date
                                <asp:DropDownList ID="ddlcompare" runat="server" CssClass="form-control"
                                    Width="60px">
                                    <asp:ListItem Value="0">=</asp:ListItem>
                                    <asp:ListItem Value="1">&gt;=</asp:ListItem>
                                    <asp:ListItem Value="2">&lt;=</asp:ListItem>
                                    <asp:ListItem Value="3">&gt;</asp:ListItem>
                                    <asp:ListItem Value="4">&lt;</asp:ListItem>
                                </asp:DropDownList>
                                    <asp:TextBox ID="txtLastServiceDt" runat="server" CssClass="form-control"
                                        Width="110px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtLastServiceDt_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtLastServiceDt">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-12">
                        <div style="border:none">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <fieldset>
                                        <div>
                                            <table style="display:none" class="table table-bordered table-striped table-condensed flip-content">
                                                <tr>
                                                    <%--  <th style="width: 10px;" scope="col">
                                                                    </th>--%>
                                                    <th style="width: 130px;" scope="col">Name
                                                    </th>
                                                    <th style="width: 150px;" scope="col">Manuf.
                                                    </th>
                                                    <th style="width: 150px;" scope="col">Description
                                                    </th>
                                                    <th style="width: 80px;" scope="col">Type
                                                    </th>
                                                    <th style="width: 80px;" scope="col">Service type
                                                    </th>
                                                    <th style="width: 80px;" scope="col">Category
                                                    </th>
                                                    <th style="width: 50px;" scope="col">Status
                                                    </th>
                                                    <th style="width: 150px;" scope="col">Location
                                                    </th>
                                                    <th style="width: 60px;" scope="col">Price
                                                    </th>
                                                    <th style="width: 50px;" scope="col">Last Service
                                                    </th>
                                                    <th style="width: 85px;" scope="col">Installed
                                                    </th>
                                                </tr>
                                            </table>
                                            <div id="panelContainer" style="max-height: 400px; min-height: 200px; overflow-y: scroll;">
                                                <asp:GridView ID="gvEquip" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                    DataKeyNames="ID" ShowHeader="true"
                                                    EmptyDataText="No Equipments Found...">
                                                    <RowStyle CssClass="evenrowcolor" />
                                                    <FooterStyle CssClass="footer" />
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                                    <Columns>
                                                        <%-- <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-Width="10px">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="ID" SortExpression="id" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name" SortExpression="unit" HeaderStyle-Width="130px"
                                                            ItemStyle-Width="130px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="130px" />
                                                            <ItemStyle Width="130px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Manuf." SortExpression="manuf" HeaderStyle-Width="150px"
                                                            ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblmanuf" runat="server" Text='<%# Bind("manuf") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="150px" />
                                                            <ItemStyle Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description" SortExpression="fdesc" HeaderStyle-Width="150px"
                                                            ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="150px" />
                                                            <ItemStyle Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Type" SortExpression="Type" HeaderStyle-Width="80px"
                                                            ItemStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                            <ItemStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Service type" SortExpression="cat" HeaderStyle-Width="80px"
                                                            ItemStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblServiceType" runat="server"><%#Eval("cat")%></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                            <ItemStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Category" SortExpression="Category" HeaderStyle-Width="80px"
                                                            ItemStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCategory" runat="server"><%#Eval("Category")%></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                            <ItemStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" SortExpression="status" HeaderStyle-Width="50px"
                                                            ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="50px" />
                                                            <ItemStyle Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Location" SortExpression="tag" HeaderStyle-Width="150px"
                                                            ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLocName" runat="server"><%#Eval("tag")%></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="150px" />
                                                            <ItemStyle Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Price" SortExpression="Price" HeaderStyle-Width="60px"
                                                            ItemStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPrice" runat="server"><%#Eval("Price")%></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="60px" />
                                                            <ItemStyle Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last Service" SortExpression="last" HeaderStyle-Width="50px"
                                                            ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbllast" runat="server"><%# Eval("last")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "last"))):""%></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="50px" />
                                                            <ItemStyle Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Installed" SortExpression="since" HeaderStyle-Width="50px"
                                                            ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSince" runat="server"><%# Eval("since") != DBNull.Value ? String.Format("{0:M/d/yyyy}", Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "since"))) : ""%></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="50px" />
                                                            <ItemStyle Width="50px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div  style="border: none">
                        <fieldset style="padding: 10px 10px 10px 10px">
                            <p style="font-size: larger"><b>MCP</b></p>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div style="float: left; height: 100%; background-color: #ccc;"
                                        class="roundCorner">
                                        <asp:GridView ID="gvSelectTemplate" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content">
                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                            <Columns>
                                                <asp:TemplateField SortExpression="fdesc">
                                                    <HeaderTemplate>
                                                        Template
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblRepTempName" OnClick="cbRepTemplate_SelectedIndexChanged"
                                                            CausesValidation="false" runat="server" Text='<%# Eval("fdesc") %>' OnClientClick="changes = 1;"></asp:LinkButton>
                                                        <asp:Label ID="lblRepTempId" runat="server" Visible="false" Text='<%# Eval("ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Last Date
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control input-sm input-small" Width="70px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtStartDate">
                                                        </asp:CalendarExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="evenrowcolor" />
                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                        </asp:GridView>
                                    </div>
                                    <%--<div class="clearfix"></div>--%>
                                    <div style="padding-left:10px;">
                                        <div style="border-style: solid solid none solid;  float: left; background-position: #B8E5FC; background: #B8E5FC; height: 25px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px; border-width: 1px; border-color: #a9c6c9;"
                                            class="roundCorner">
                                            <asp:Panel runat="server" ID="pnlGridButtons" CssClass="gridButtonspanel" Style="width: 100%; min-width: 909px">
                                                <%--  <asp:LinkButton ID="btnAddNewItem" runat="server" OnClick="btnAddNewItem_Click" Style="margin-right: 20px;
                                    float: right" Text="Add New Item" CausesValidation="False">Add New</asp:LinkButton>--%>
                                                <asp:LinkButton ID="btnProcess" runat="server" Style="margin-left: 20px;"
                                                    Text="Process" ValidationGroup="rep" OnClientClick="javascript:if (Page_IsValid) {return confirm('Are you sure you want to add items to filtered equipments?');}"
                                                    OnClick="btnProcess_Click">Process</asp:LinkButton>
                                                <asp:LinkButton ID="btnDeleteItem" runat="server" CausesValidation="False" Style="margin-right: 20px;"
                                                    OnClick="btnDeleteItem_Click" Text="Delete">Delete</asp:LinkButton>
                                            </asp:Panel>
                                            <div style="overflow:auto;height:500px;">
                                            <asp:GridView ID="gvTemplateItems" runat="server"  AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                Width="909px" EmptyDataText="Please select a template...">
                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                <RowStyle CssClass="evenrowcolor" />
                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Code
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Section">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSection" MaxLength="25" runat="server" Text='<%# Eval("section") %>'
                                                                Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Enabled="false" Text='<%# Eval("Name") %>'
                                                                Width="120px"></asp:Label>
                                                            <asp:Label ID="lblEquipT" Visible="false" runat="server" Text='<%# Eval("EquipT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desc" SortExpression="fdesc">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblDesc" runat="server" Text='<%# Eval("fdesc") %>' Width="200px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ValidationGroup="rep" ControlToValidate="lblDesc"
                                                                Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" Font-Bold="True" Font-Size="Larger"></asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                        <FooterStyle VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Last Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLdate" runat="server" OnTextChanged="txtLdate_TextChanged" AutoPostBack="true"
                                                                Width="70px" Text='<%#Eval("lastdate").ToString().Length>0? Convert.ToDateTime(Eval("lastdate")).ToShortDateString():"" %>'></asp:TextBox>
                                                            <asp:CalendarExtender ID="lblLdate_CalendarExtender" runat="server" Enabled="True"
                                                                TargetControlID="txtLdate">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Freq.">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlFreq" Width="100px" runat="server" SelectedValue='<%# Eval("Frequency") %>'
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlFreq_SelectedIndexChanged">
                                                                <asp:ListItem Value="-1">-Select-</asp:ListItem>
                                                            <asp:ListItem Value="0">Daily</asp:ListItem>
                                                            <asp:ListItem Value="1">Weekly</asp:ListItem>
                                                            <asp:ListItem Value="2">Bi-Weekly</asp:ListItem>
                                                            <asp:ListItem Value="3">Monthly</asp:ListItem>
                                                            <asp:ListItem Value="4">Bi-Monthly</asp:ListItem>
                                                            <asp:ListItem Value="5">Quarterly</asp:ListItem>
                                                            <asp:ListItem Value="6">Semi-Annually </asp:ListItem>
                                                            <asp:ListItem Value="7">Annually</asp:ListItem>
                                                            <asp:ListItem Value="8">One Time</asp:ListItem>
                                                            <asp:ListItem Value="9">3 Times a Year</asp:ListItem>
                                                            <asp:ListItem Value="10">Every 2 Year</asp:ListItem>
                                                            <asp:ListItem Value="11">Every 3 Year</asp:ListItem>
                                                            <asp:ListItem Value="12">Every 5 Year</asp:ListItem>
                                                            <asp:ListItem Value="13">Every 7 Year</asp:ListItem>
                                                            <asp:ListItem Value="14">On-Demand</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Next Due Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDuedate" Width="70px" runat="server" Text='<%#Eval("NextDateDue").ToString().Length>0? Convert.ToDateTime(Eval("NextDateDue")).ToShortDateString():"" %>'></asp:TextBox>
                                                            <asp:CalendarExtender ID="lblDuedate_CalendarExtender" runat="server" Enabled="True"
                                                                TargetControlID="txtDuedate">
                                                            </asp:CalendarExtender>
                                                            <asp:RequiredFieldValidator ID="rfvNextDate" runat="server" ValidationGroup="rep"
                                                                ControlToValidate="txtDuedate" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True"
                                                                Font-Bold="True" Font-Size="Larger"></asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Notes" >
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtNotes" Height="30px" TextMode="MultiLine" runat="server" Text='<%# Eval("Notes") %>' Width="200px"></asp:TextBox>                                                            
                                                        </ItemTemplate>
                                                        <FooterStyle VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView> 
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </fieldset>
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
