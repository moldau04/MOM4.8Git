<%@ Page Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Etimesheet_v2" Codebehind="Etimesheet_v2.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<!--Grid Control-->
	<link href="Design/css/grid.css" rel="stylesheet" />
	<link href="Design/css/pikaday.css" rel="stylesheet" />
	<style>
		.btn {
			border: 1px solid #0086b3;
			font-weight: 500;
			letter-spacing: 0px;
			outline: ridge;
			height: 28px;
			margin-top: -4px !important;
		}

			.btn:focus, .btn:active:focus, .btn.active:focus {
				outline: ridge;
			}

		.btn-primary {
			background: #0099cc !important;
			border-color: #208eb3 !important;
			padding: 3px 10px !important;
		}

			.btn-primary:hover, .btn-primary:focus, .btn-primary:active, .btn-primary.active, .open > .dropdown-toggle.btn-primary {
				background: #33a6cc !important;
				height: 28px;
			}

			.btn-primary:active, .btn-primary.active {
				background: #007299 !important;
				box-shadow: none;
				height: 28px;
			}

		.istyle {
			padding-right: 0px;
			margin-right: -1px;
			margin-top: -4px !important;
			padding: 2px !important;
			margin-left: -1px !important;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div>
		<div class="divbutton-container">
			<div id="divButtons">
				<div id="breadcrumbs-wrapper">
					<header>
						<div class="container row-color-grey">
							<div class="row">
								<div class="col s12 m12 l12">
									<div class="row">
										<div class="page-title">
											<i class="mdi-action-payment"></i>&nbsp;
                                        <telerik:RadLabel CssClass="title_text" runat="server" ID="lblHeader" AssociatedControlID="RadTextBox1" Text="e-Timesheet"></telerik:RadLabel>
										</div>
										<asp:DropDownList ID="ddlExport" class="dropdown-content" runat="server">
                                    <asp:ListItem>CSV</asp:ListItem>
                                    <asp:ListItem>Excel</asp:ListItem>
                                    <asp:ListItem>Text</asp:ListItem>
                                </asp:DropDownList>
										<div class="btnlinks">
											<a class="dropdown-button" id="lnkExport" data-beloworigin="true" runat="server" onclick="lnkExport_Click" data-activates="ddlExport" onclientclick="return confirm('Please make sure you have saved the changes before exporting.');">Export
											</a>
										</div>
										<div class="btnlinks">
											<asp:LinkButton CssClass="icon-save" ID="lnkProcess" runat="server" CausesValidation="False" Style="display: none" OnClientClick="removeDummyRows();">Save</asp:LinkButton>
										</div>
										<div class="btnlinks">
											<asp:LinkButton ID="lnkSave" runat="server" CausesValidation="False" Style="display: none">Save</asp:LinkButton>
										</div>
										<div class="btnclosewrap">
											<asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false">  <i class="mdi-content-clear"></i></asp:LinkButton>
										</div>
										<div class="btnlinksicon">
											<ul class="dropdown-content">
												<li class="dropdown dropdown-user">
													<a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print"
														data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
													<ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
														<li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear: both;"></div>
														</a>
														</li>
														<li style="margin-left: 0px;"><a href="TicketByType.aspx"><span>Ticket Listing Report</span><div style="clear: both;"></div>
														</a>
														</li>
													</ul>
												</li>
											</ul>
										</div>
									</div>
								</div>
							</div>
						</div>
					</header>
				</div>
			</div>
		</div>
	</div>
	<div class="container">
		<asp:HiddenField ID="hdnTickets" runat="server" />
		<asp:HiddenField ID="hdnWeeks" runat="server" />
		<div class="row">
			<div class="srchpane">
				<div class="srchpaneinner col s12">
					<div class="srchtitle" style="padding-left: 15px;">
						Payroll Period From
					</div>
					<div class="srchinputwrap pd-negatenw input-field">
						<asp:TextBox ID="txtfromDate" CssClass="datepicker_mom" runat="server" MaxLength="28"></asp:TextBox>
					</div>
					<div class="srchinputwrap pd-negatenw input-field">
						<asp:TextBox ID="txtToDate" CssClass="datepicker_mom" runat="server" MaxLength="28"></asp:TextBox>
					</div>
					<div class="srchinputwrap pd-negatenw">
						<ul class="tabselect accrd-tabselect" id="testradiobutton">
							<li>
								<asp:LinkButton AutoPostBack="True" ID="LinkButton1" Style="margin-right: 3px;" runat="server" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtTo','ctl00_ContentPlaceHolder1_txtFrom','rdCal');return false;" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>"></asp:LinkButton>
							<li>
								<label id="lblDay" runat="server">
									<input type="radio" id="rdDay" runat="server" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#lblDay', 'hdnTaskDate', 'rdCal')" />
									Day
								</label>
							</li>
							<li>
								<label id="lblWeek" runat="server">
									<input type="radio" id="rdWeek" runat="server" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnTaskDate', 'rdCal')" />
									Week
								</label>
							</li>
							<li>
								<label id="lblMonth" runat="server">
									<input type="radio" id="rdMonth" runat="server" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnTaskDate', 'rdCal')" />
									Month
								</label>
							</li>
							<li>
								<label id="lblQuarter" runat="server">
									<input type="radio" id="rdQuarter" runat="server" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnTaskDate', 'rdCal')" />
									Quarter
								</label>
							</li>
							<li>
								<label id="lblYear" runat="server">
									<input type="radio" id="rdYear" runat="server" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnTaskDate', 'rdCal')" />
									Year
								</label>
							</li>
							<li>
								<asp:LinkButton AutoPostBack="True" ID="incDate" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtTo','ctl00_ContentPlaceHolder1_txtFrom','rdCal');return false" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
							</li>
						</ul>
					</div>
					<div class="col lblsz2 lblszfloat">
						<div class="row">
							<span class="tro trost">
								<asp:LinkButton ID="lnkClear" runat="server">Clear</asp:LinkButton>
							</span>
							<span class="tro trost">
								<asp:LinkButton ID="lnkShowAll" runat="server">Show All</asp:LinkButton>
							</span>
							<span class="tro trost">
								<asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic;"></asp:Label>
							</span>
							<span class="tro trost">
								<asp:Label ID="lblProcessed" runat="server" CssClass="shadow" Style="color: Red; font-weight: bold; font-size: medium;"
									Text="PROCESSED"></asp:Label>
							</span>
							<span class="tro trost">
								<asp:Label ID="lblSaved" runat="server" CssClass="shadow " Style="color: Blue; font-weight: bold; font-size: medium;"
									Text="SAVED"></asp:Label>
							</span>
						</div>
					</div>
				</div>
				<div class="srchinputwrap" style="margin-right: 23px;">
					<div class="fc-label1">
						Supervisor
					</div>
					<div class="fc-input">
						<asp:DropDownList ID="ddlSuper" runat="server" CssClass="form-control" Width="100px" Visible="false"
							AutoPostBack="True" OnSelectedIndexChanged="ddlSuper_SelectedIndexChanged">
						</asp:DropDownList>
					</div>
				</div>
				<div class="srchinputwrap" style="margin-right: 23px;">
					<div class="fc-label1">
						Department
					</div>
					<div class="fc-input">
						<asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control" Width="100px"
							AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
						</asp:DropDownList>
					</div>
				</div>
			</div>
			<div class="grid_container">
				<div class="form-section-row" style="margin-bottom: 0 !important;">
					<telerik:RadAjaxManager ID="RadAjaxManager_Task" runat="server">
						<AjaxSettings>
							<telerik:AjaxSetting AjaxControlID="lnkShowAll">
								<UpdatedControls>
									<telerik:AjaxUpdatedControl ControlID="gvTimesheet" LoadingPanelID="RadAjaxLoadingPanel_Task" />
								</UpdatedControls>
							</telerik:AjaxSetting>
						</AjaxSettings>
					</telerik:RadAjaxManager>
					<div class="RadGrid RadGrid_Material">
						<telerik:RadCodeBlock ID="RadCodeBlock_Task" runat="server">
							<script type="text/javascript">
								function pageLoad() {
									var grid = $find("<%= gvTimesheet.ClientID %>");
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
						<telerik:RadAjaxPanel ID="RadAjaxPanel_Task" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Task" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
							<telerik:RadPersistenceManager ID="RadPersistenceManager_Task" runat="server">
								<PersistenceSettings>
									<telerik:PersistenceSetting ControlID="gvTimesheet" />
								</PersistenceSettings>
							</telerik:RadPersistenceManager>
							<telerik:RadGrid ID="gvTimesheet" RenderMode="Auto" runat="server" CssClass="table table-bordered table-striped table-condensed flip-content"
								Width="100%" ShowStatusBar="true" PageSize="20" AllowPaging="true" PagerStyle-AlwaysVisible="true" AllowSorting="false" DataKeyNames="ID,Reg1,OT1,DT1,TT1,NT1,Zone1,Mileage1,Extra1,Misc1,Toll1,HourRate1"
								ShowFooter="true" OnItemDataBound="gvTimesheet_ItemDataBound">
								<CommandItemStyle />
								<GroupingSettings CaseSensitive="false" />
								<ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
									<Selecting AllowRowSelect="True"></Selecting>
									<Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
									<Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
								</ClientSettings>
								<MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="False" ShowFooter="True" DataKeyNames="ID">
									<Columns>
										<telerik:GridClientSelectColumn UniqueName="ClientSelectColumn">
										</telerik:GridClientSelectColumn>
										<telerik:GridTemplateColumn>
											<ItemTemplate>
												<a id="lnkExpand" runat="server" href='<%# string.Format("javascript:divexpandcollapse({0});", Eval("id")) %>'>
													<img alt="Details" id="imgdiv<%# Eval("id") %>" src="images/plusgray.png" />
												</a>
												<div id="div<%# Eval("id") %>" style="display: none;">
													<telerik:RadGrid ID="gvTickets" runat="server" AutoGenerateColumns="false" ShowFooter="true"
														ShowHeader="true" BorderColor="#818D99" DataKeyNames="ticketID" CssClass="float_right">
														<MasterTableView AutoGenerateColumns="false" TableLayout="Fixed">
															<Columns>
																<telerik:GridTemplateColumn HeaderText="Ticket#">
																	<ItemTemplate>
																		<asp:HyperLink ID="lnkTick" NavigateUrl='<%# "addticket.aspx?comp=1&id=" + Eval("ticketid") %>'
																			Target="_blank" runat="server" Text='<%# Eval("ticketid") %>' Style="margin-left: 50px"></asp:HyperLink>
																		<asp:UpdatePanel ID="UpdatePanel1" runat="server">
																			<ContentTemplate>
																				<asp:Button ID="lnkUpdateTick" CssClass="updatet" CausesValidation="false" CommandName="UpdateTicket" Style="display: none;" CommandArgument='<%# Eval("ticketid") %>' runat="server" Text="Update" />
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</ItemTemplate>
																	<ItemStyle Width="430px" />
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Custom">
																	<ItemTemplate>
																		<asp:TextBox ID="txtTCustom" ForeColor="DarkSlateGray" Font-Size="8pt" Style="display: none"
																			Width="50px" Text='<%# BindList( Eval("custom") )%>' Onkeyup="calculateExpand($(this).closest('table'));"
																			runat="server">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="txtCustom" runat="server" TargetControlID="ftb115"
																			ValidChars="1234567890.-">
																		</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Rate">
																	<ItemTemplate>
																		<asp:TextBox ID="txtTRate" ForeColor="DarkSlateGray" Font-Size="8pt" ToolTip="Hourly Rate"
																			Width="50px" Text='<%# BindList( Eval("hourlyRate") )%>' Onkeyup="calculateExpand($(this).closest('table'));"
																			onblur="$(this).closest('tr').find('.updatet').click();" runat="server">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb15" runat="server" TargetControlID="txtTRate"
																			ValidChars="1234567890.-">
																		</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Rate Job">
																	<ItemTemplate>
																		<asp:CheckBox ID="chkTJobRate" runat="server"
																			onchange="calculateExpand($(this).closest('table')); $(this).closest('tr').find('.updatet').click();"
																			Checked='<%# Convert.ToBoolean( Eval("CustomTick3") !=DBNull.Value ? Eval("CustomTick3") : 0 ) %>'></asp:CheckBox>
																	</ItemTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Rate Job Hr.">
																	<ItemTemplate>
																		<asp:TextBox ID="txtTJobRate" ForeColor="DarkSlateGray" Font-Size="8pt"
																			Width="50px" Text='<%#  Eval("CustomTick1") %>' Onkeyup="calculateExpand($(this).closest('table'));"
																			onblur="$(this).closest('tr').find('.updatet').click();" runat="server">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftbn15" runat="server" TargetControlID="txtTJobRate"
																			ValidChars="1234567890.-">
																		</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Reg" ItemStyle-Width="50px" FooterStyle-Width="50px"
																	ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
																	FooterStyle-BorderColor="#D6D6D6">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTReg" Width="50px"
																			runat="server" Text='<%# BindList( Eval("Reg") )%>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			Onkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb16" runat="server" TargetControlID="txtTReg"
																			ValidChars="1234567890.-">
																		</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="OT" ItemStyle-Width="50px" FooterStyle-Width="50px"
																	ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
																	FooterStyle-BorderColor="#D6D6D6">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTOT" Width="50px" runat="server"
																			Text='<%# BindList (Eval("OT") )%>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			Onkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb17" runat="server" TargetControlID="txtTOT"
																			ValidChars="1234567890.-">
																		</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderText="1.7"
																	ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
																	FooterStyle-BorderColor="#D6D6D6">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTNT" Width="50px" runat="server"
																			Text='<%# BindList( Eval("NT") )%>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			Onkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb18" runat="server" TargetControlID="txtTNT"
																			ValidChars="1234567890.-">
																		</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="DT" ItemStyle-Width="50px" FooterStyle-Width="50px"
																	ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
																	FooterStyle-BorderColor="#D6D6D6">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTDT" Width="50px" runat="server"
																			Text='<%# BindList( Eval("DT") )%>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			Onkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb19" runat="server" TargetControlID="txtTDT"
																			ValidChars="1234567890.-">
																		</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="TT" ItemStyle-Width="50px" FooterStyle-Width="50px"
																	ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
																	FooterStyle-BorderColor="#D6D6D6">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTravel" Width="50px"
																			runat="server" Text='<%# BindList( Eval("TT") )%>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			Onkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb20" runat="server" TargetControlID="txtTravel"
																			ValidChars="1234567890.-">
																		</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Total Time" ItemStyle-Width="50px" FooterStyle-Width="50px"
																	ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
																	FooterStyle-BorderColor="#D6D6D6">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtPTimeTotal" Width="50px"
																			runat="server" onfocus="this.blur();" CssClass="texttransparent">
																		</asp:TextBox>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblPTimeTotal" runat="server"
																			onfocus="this.blur();" CssClass="texttransparent" Width="50px">
																		</asp:TextBox>
																	</FooterTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Amount" ItemStyle-Width="50px" FooterStyle-Width="50px">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" onfocus="this.blur();" CssClass="texttransparent" ForeColor="DarkSlateGray"
																			ID="txtTAmount" Width="50px" runat="server">
																		</asp:TextBox>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTAmount" runat="server"
																			onfocus="this.blur();" CssClass="texttransparent" Width="50px">
																		</asp:TextBox>
																	</FooterTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Zone" ItemStyle-Width="50px" FooterStyle-Width="50px">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTZone" Width="50px"
																			runat="server" Text='<%# BindList( Eval("Zone") )%>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			Onkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb21" runat="server" TargetControlID="txtTZone"
																	ValidChars="1234567890.-">
																</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTZone" runat="server"
																			onfocus="this.blur();" CssClass="texttransparent" Width="50px">
																		</asp:TextBox>
																	</FooterTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Mileage" ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="80">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTMileage" Width="50px"
																			runat="server" Text='<%# BindList( Eval("Mileage") )%>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			Onkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb22" runat="server" TargetControlID="txtTMileage"
																			ValidChars="1234567890.-">
																		</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTMileage" runat="server"
																			onfocus="this.blur();" CssClass="texttransparent" Width="50px">
																		</asp:TextBox>
																	</FooterTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Misc" ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="80">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTMisc" Width="50px"
																			runat="server" Text='<%# BindList( Eval("othere") )%>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			Onkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb23" runat="server" TargetControlID="txtTMisc"
																	ValidChars="1234567890.-">
																</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTMisc" runat="server"
																			onfocus="this.blur();" CssClass="texttransparent" Width="50px">
																		</asp:TextBox>
																	</FooterTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Toll" ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="80">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtPToll" Width="50px"
																			runat="server" Text='<%# BindList( Eval("toll") )%>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			nkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb24" runat="server" TargetControlID="txtPToll"
																	ValidChars="1234567890.-">
																</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblPToll" runat="server"
																			onfocus="this.blur();" CssClass="texttransparent" Width="50px">
																		</asp:TextBox>
																	</FooterTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Extra Exp." ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="80">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTExtra" Width="50px"
																			runat="server" Text='<%# BindList(Eval("Extra")) %>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			Onkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftb25" runat="server" TargetControlID="txtTExtra"
																	ValidChars="1234567890.-">
																</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTExtra" runat="server"
																			onfocus="this.blur();" CssClass="texttransparent" Width="50px">
																		</asp:TextBox>
																	</FooterTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn>
																	<ItemTemplate>
																	</ItemTemplate>
																	<ItemStyle Width="452px" />
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Fringe Benefit" ItemStyle-Width="50px" FooterStyle-Width="50px">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTCustomTick2" Width="50px"
																			runat="server" Text='<%# Eval("CustomTick2") %>'
																			onblur="$(this).closest('tr').find('.updatet').click();"
																			Onkeyup="calculateExpand($(this).closest('table'));">
																		</asp:TextBox>
																		<%--<telerik:FilteredTextBoxExtender ID="ftbct25" runat="server" TargetControlID="txtTCustomTick2"
																	ValidChars="1234567890.-">
																</telerik:FilteredTextBoxExtender>--%>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTCustomTick2" runat="server"
																			onfocus="this.blur();" CssClass="texttransparent" Width="50px">
																		</asp:TextBox>
																	</FooterTemplate>
																</telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="Total">
																	<ItemTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTTotal" onfocus="this.blur();"
																			CssClass="texttransparent" Width="50px" runat="server">
																		</asp:TextBox>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTGtotal" runat="server"
																			onfocus="this.blur();" CssClass="texttransparent" Width="50px">
																		</asp:TextBox>
																	</FooterTemplate>
																</telerik:GridTemplateColumn>
															</Columns>
														</MasterTableView>
													</telerik:RadGrid>
												</div>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn>
											<ItemTemplate>
												<asp:Image ID="imgSign" runat="server" Width="16px" ToolTip="Signature" Visible='<%# Eval("signature") != DBNull.Value ? true : false %>'
													ImageUrl="images/Signature.png" />
												<asp:Image ID="imgSignature" runat="server" CssClass="hoverGrid shadow transparent roundCorner"
													ImageUrl=' <%# Eval("signature") != DBNull.Value ?( "data:image/png;base64," + Convert.ToBase64String((byte[])Eval("signature"))) : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" %>'></asp:Image>
												<asp:HoverMenuExtender ID="hmeRes" runat="server" OffsetY="20" OffsetX="20" PopupControlID="imgSignature"
													TargetControlID="imgSign" HoverDelay="250">
												</asp:HoverMenuExtender>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn SortExpression="Name" HeaderText="Name" ShowFilterIcon="false" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
												<asp:HyperLink ID="lblname" runat="server" Width="100px" Text='<%# Eval("name") %>'
													NavigateUrl='<%# "adduser.aspx?type=1&uid=" + Eval("userid") %>' Target="_blank"></asp:HyperLink>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn SortExpression="Company" HeaderText="Company" ShowFilterIcon="false" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:Label ID="lblCompany" runat="server" Text='<%# Eval("Company") %>'></asp:Label>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Emp ID" ShowFilterIcon="false" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:Label ID="lblEmpID" runat="server" Text='<%# Eval("ref") %>'></asp:Label>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Emp Type" ShowFilterIcon="false" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:Label ID="lbltype" runat="server" Text='<%# Eval("usertype") %>'></asp:Label>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Pay" ShowFilterIcon="false">
											<ItemTemplate>
												<asp:CheckBox ID="chkPay" runat="server" Checked='<%# BindList(Eval("total")) == "" ? false : true %>' />
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Method" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:Label ID="lblHours" Style="display: none" runat="server" Text='<%# Eval("phour") %>'></asp:Label>
												<asp:Label ID="lblSalary" Style="display: none" runat="server" Text='<%# Eval("salary") %>'></asp:Label>
												<asp:Label ID="lblMlRate" Style="display: none" runat="server" Text='<%# lblProcessed.Visible == true ? Eval("mileage") : Eval("mileagerate") %>'></asp:Label>
												<asp:Label ID="lblHourlyRate" Style="display: none" runat="server" Text='<%# lblProcessed.Visible == true ? Eval("HourlyRate")  : Eval("HourlyRate") %>'></asp:Label>
												<asp:Label ID="lblMID" Style="display: none" runat="server" Text='<%# Eval("pmethod") %>'></asp:Label>
												<asp:Label ID="lblMethod" runat="server" Text='<%# Eval("paymethod") %>'></asp:Label>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Custom" ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:TextBox ID="txtCustom" Text='<%# BindList( Eval("custom") )%>' onfocus="this.blur();" CssClass="texttransparent" runat="server"
													Width="50px" Style="display: none"></asp:TextBox>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblCustom"
													runat="server" Style="display: none"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Hourly Rate($)" ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:TextBox ID="txtHRate" onfocus="this.blur();" CssClass="texttransparent" runat="server"
													Width="50px" Text='<%# BindList(Eval("HourlyRate")) %>'></asp:TextBox>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Rate Job" HeaderStyle-Width="50px" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<div width="50px"></div>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Rate Job Hr." HeaderStyle-Width="50px" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<div width="50px"></div>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Reg" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderColor="#D6D6D6"
											ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
											FooterStyle-BorderColor="#D6D6D6" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<asp:TextBox ID="txtReg" runat="server" Onkeyup="CalculateTimesheet();" Width="50px"
													Text='<%# BindList(Eval("Reg")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtReg"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblReg"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="OT" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderColor="#D6D6D6"
											ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
											FooterStyle-BorderColor="#D6D6D6" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<asp:TextBox ID="txtOT" runat="server" Onkeyup="CalculateTimesheet();" Width="50px"
													Text='<%# BindList(Eval("OT")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtOT"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblOT"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="1.7" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderColor="#D6D6D6"
											ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
											FooterStyle-BorderColor="#D6D6D6" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<asp:TextBox ID="txtoneseven" runat="server" Onkeyup="CalculateTimesheet();" Width="50px"
													Text='<%# BindList(Eval("NT")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtoneseven"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblNT"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="DT" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderColor="#D6D6D6"
											ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
											FooterStyle-BorderColor="#D6D6D6" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<asp:TextBox ID="txtDT" runat="server" Onkeyup="CalculateTimesheet();" Width="50px"
													Text='<%# BindList(Eval("DT")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtDT"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblDT"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="TT" ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid"
											ItemStyle-BorderColor="#D6D6D6" FooterStyle-BorderColor="#D6D6D6" HeaderStyle-BorderStyle="Solid"
											HeaderStyle-BorderColor="#D6D6D6" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<asp:TextBox ID="txtPTravel" runat="server" Onkeyup="CalculateTimesheet();" Width="50px"
													Text='<%# BindList(Eval("TT")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtPTravel"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblPTravel"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Total Time" ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid"
											ItemStyle-BorderColor="#D6D6D6" FooterStyle-BorderColor="#D6D6D6" HeaderStyle-BorderStyle="Solid"
											HeaderStyle-BorderColor="#D6D6D6" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<asp:TextBox ID="txtTimeTotal" onfocus="this.blur();" CssClass="texttransparent"
													runat="server" Width="50px"></asp:TextBox>
											</ItemTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Amount ($)" ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:TextBox ID="txtAmount" runat="server" onfocus="this.blur();" CssClass="texttransparent"
													Width="50px" Text='<%# BindList(Eval("dollaramount")) %>'></asp:TextBox>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblAmount"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Zone($)" ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:TextBox ID="txtZone" Width="50px" Onkeyup="CalculateTimesheet();" runat="server"
													Text='<%# BindList(Eval("zone")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtZone"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblZone"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Mileage (Miles)" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<asp:TextBox ID="txtMileage" Width="50px" Onkeyup="CalculateTimesheet();" runat="server"
													Text='<%# BindList(Eval("mileage")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtMileage"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblMileage"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Misc($)" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<asp:TextBox ID="txtMisc" Width="50px" Onkeyup="CalculateTimesheet();" runat="server"
													Text='<%# BindList(Eval("othere")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FTBExtender11" runat="server" TargetControlID="txtMisc"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblMisc"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Toll($)" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<asp:TextBox ID="txtToll" Width="50px" Onkeyup="CalculateTimesheet();" runat="server"
													Text='<%# BindList(Eval("toll")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FTBoxExtender11" runat="server" TargetControlID="txtToll"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblToll"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Extra Exp.($)" ItemStyle-Width="50px" FooterStyle-Width="50px">
											<ItemTemplate>
												<asp:TextBox ID="txtExtra" Width="50px" Onkeyup="CalculateTimesheet();" runat="server"
													Text='<%# BindList(Eval("Extra")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtExtra"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblExtra"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Holiday" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderColor="#D6D6D6"
											ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
											FooterStyle-BorderColor="#D6D6D6" ItemStyle-Width="70px" FooterStyle-Width="70px" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:Label ID="lblDlrH" runat="server" Text="$" Visible="false"></asp:Label>
												<asp:TextBox ID="txtHoliday" Onkeyup="CalculateTimesheet();" Width="50px" runat="server"
													Style="float: right" Text='<%# BindList(Eval("holiday")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtHoliday"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblHoliday"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Vacation" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderColor="#D6D6D6"
											ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
											FooterStyle-BorderColor="#D6D6D6" ItemStyle-Width="70px" FooterStyle-Width="70px" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:Label ID="lblDlrV" runat="server" Text="$" Visible="false"></asp:Label>
												<asp:TextBox ID="txtVacation" Onkeyup="CalculateTimesheet();" Width="50px" runat="server"
													Style="float: right" Text='<%# BindList(Eval("vacation")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtVacation"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblVacation"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Sick Time" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderColor="#D6D6D6"
											ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
											FooterStyle-BorderColor="#D6D6D6" ItemStyle-Width="70px" FooterStyle-Width="70px">
											<ItemTemplate>
												<asp:Label ID="lblDlrS" runat="server" Text="$" Visible="false"></asp:Label>
												<asp:TextBox ID="txtSick" Onkeyup="CalculateTimesheet();" Width="50px" runat="server"
													Style="float: right" Text='<%# BindList(Eval("sicktime")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtSick"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblSickTime"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Reimb($)" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:TextBox ID="txtReimb" Onkeyup="CalculateTimesheet();" Width="50px" runat="server"
													Text='<%# BindList(Eval("reimb")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtReimb"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblReimb"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Bonus($)" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:TextBox ID="txtBonus" Onkeyup="CalculateTimesheet();" Width="50px" runat="server"
													Text='<%# BindList(Eval("bonus")) %>'></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtBonus"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblBonus"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Salary($)" ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:TextBox ID="txtSalary" Onkeyup="CalculateTimesheet();" Text='<%# BindList(Eval("salary")) %>'
													runat="server" Width="50px"></asp:TextBox>
												<asp:FilteredTextBoxExtender ID="ftb12" runat="server" TargetControlID="txtSalary"
													ValidChars="1234567890.-">
												</asp:FilteredTextBoxExtender>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblSalary"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Fringe Benefit" ItemStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:TextBox ID="txtCustomT2" onfocus="this.blur();" CssClass="texttransparent" runat="server" Width="50px"></asp:TextBox>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblCustomT2"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
										<telerik:GridTemplateColumn HeaderText="Total($)" HeaderStyle-Width="80">
											<ItemTemplate>
												<asp:TextBox ID="txtTotal" onfocus="this.blur();" CssClass="texttransparent" Width="50px"
													runat="server" Text='<%# BindList(Eval("total")) %>'></asp:TextBox>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox onfocus="this.blur();" CssClass="texttransparent" Width="50px" ID="lblGtotal"
													runat="server"></asp:TextBox>
											</FooterTemplate>
										</telerik:GridTemplateColumn>
									</Columns>
								</MasterTableView>
							</telerik:RadGrid>
						</telerik:RadAjaxPanel>
					</div>
					<table>
                            <tr>
                                <td>

                                    <%--<asp:LinkButton ID="lnkSaved" runat="server" OnClick="lnkSaved_Click" Visible="false"
                                        Style="float: right">Timesheet Updates for this Period</asp:LinkButton>--%>
                                    <%--<asp:LinkButton ID="lnkBack" runat="server" Visible="false" Style="float: right"
                                        OnClick="lnkBack_Click">Back</asp:LinkButton>--%>
                                    <%--<asp:LinkButton ID="lnkMerge" runat="server" Visible="false" ToolTip="Update Timesheet with the newly added Tickets and Employees."
                                        Style="float: right; margin-right: 20px" OnClick="lnkMerge_Click">Merge Updates</asp:LinkButton>--%>
                                </td>
                            </tr>
                        </table>
                        <div class="clearfix"></div>
				</div>
			</div>

		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
	<script type="text/javascript">

								$(document).ready(function () {
									CalculateTimesheet();
								});

								function confirmUpdate(sender, message) {
									var update = confirm(message);
									if (update) {
										return true;
									} else {
										sender.value = sender.defaultValue;
										return false;
									}
								}

								function removeDummyRows() {
									var all = $(".dummy").map(function () {
										this.remove();
									}).get();
								}

								function divexpandcollapse(divname) {
									divname = 'div' + divname;
									var img = "img" + divname;
									if ($("#" + img).attr("src") == "images/plusgray.png") {
										$("#" + img)
											.closest("tr")
											.after("<tr class='dummy'><td></td><td colspan = '100%'>" + $("#" + divname).html() + "</td></tr>");
										$("#" + img).attr("src", "images/minusgray.png");

										var copygrid = $("#" + img).closest("tr").next().find("table[id*=gvTickets]");
										calculateExpand(copygrid);
									} else {
										$("#" + img).closest("tr").next().remove();
										$("#" + img).attr("src", "images/plusgray.png");
									}
								}

								function calculate(Gridview) {
									CalculateRate("#" + Gridview, 0);
								}

								function calculateExpand(Gridview) {
									CalculateRate(Gridview, 1);
									var gvprev = $(Gridview).closest('tr').prev().find("table[id*=gvTickets]");

									$(Gridview).find('tr:not(:first,:last)').each(function (index, value) {
										$(gvprev).find('tr:not(:first,:last)').each(function (secindex, secvalue) {

											if (index == secindex) {
												$(secvalue).find('input[id*=txtTCustom]').val($(value).find('input[id*=txtTCustom]').val());
												$(secvalue).find('input[id*=txtTRate]').val($(value).find('input[id*=txtTRate]').val());
												$(secvalue).find('input[id*=txtTReg]').val($(value).find('input[id*=txtTReg]').val());
												$(secvalue).find('input[id*=txtTOT]').val($(value).find('input[id*=txtTOT]').val());
												$(secvalue).find('input[id*=txtTNT]').val($(value).find('input[id*=txtTNT]').val());
												$(secvalue).find('input[id*=txtTDT]').val($(value).find('input[id*=txtTDT]').val());
												$(secvalue).find('input[id*=txtTAmount]').val($(value).find('input[id*=txtTAmount]').val());
												$(secvalue).find('input[id*=txtTravel]').val($(value).find('input[id*=txtTravel]').val());
												$(secvalue).find('input[id*=txtTZone]').val($(value).find('input[id*=txtTZone]').val());
												$(secvalue).find('input[id*=txtTMileage]').val($(value).find('input[id*=txtTMileage]').val());
												$(secvalue).find('input[id*=txtTExtra]').val($(value).find('input[id*=txtTExtra]').val());
												$(secvalue).find('input[id*=txtTTotal]').val($(value).find('input[id*=txtTTotal]').val());
												$(secvalue).find('input[id*=txtTMisc]').val($(value).find('input[id*=txtTMisc]').val());
												$(secvalue).find('input[id*=txtPToll]').val($(value).find('input[id*=txtPToll]').val());
												$(secvalue).find('input[id*=txtTJobRate]').val($(value).find('input[id*=txtTJobRate]').val());
												$(secvalue).find('input[id*=txtTCustomTick2]').val($(value).find('input[id*=txtTCustomTick2]').val());
												$(secvalue).find('input[id*=chkTJobRate]').prop('checked', $(value).find('input[id*=chkTJobRate]').is(':checked'));

											}
										});
									});
								}

								function CalculateRate(Gridview, expand) {
									var regtotal = 0;
									var ottotal = 0;
									var nttotal = 0;
									var dttotal = 0;
									var tttotal = 0;
									var tAmtotal = 0;
									var zonetotal = 0;
									var mileagetotal = 0;
									var extratotal = 0;
									var grandtotal = 0;
									var misctotal = 0;
									var tolltotal = 0;
									var Timetotals = 0;
									var CustomTotal = 0;
									var CustomT2Total = 0;

									var trprev = $(Gridview).closest('tr');
									if (expand == 1)
										trprev = $(Gridview).closest('tr').prev();

									var lblMethodID = trprev.find('span[id*=lblMID]');
									var lblHours = trprev.find('span[id*=lblHours]');
									var lblSalary = trprev.find('span[id*=lblSalary]');
									var lblHourlyRate = trprev.find('span[id*=lblHourlyRate]');
									var lblMileageRate = trprev.find('span[id*=lblMlRate]');

									$(Gridview).find('tr:not(:first,:last)').each(function () {
										var $tr = $(this);
										var txtCustom = $tr.find('input[id*=txtTCustom]');
										var txtRate = $tr.find('input[id*=txtTRate]');
										var txtReg = $tr.find('input[id*=txtTReg]');
										var txtOT = $tr.find('input[id*=txtTOT]');
										var txtNT = $tr.find('input[id*=txtTNT]');
										var txtDT = $tr.find('input[id*=txtTDT]');
										var txtTT = $tr.find('input[id*=txtTravel]');
										var txtAmount = $tr.find('input[id*=txtTAmount]');
										var txtZone = $tr.find('input[id*=txtTZone]');
										var txtMileage = $tr.find('input[id*=txtTMileage]');
										var txtExtra = $tr.find('input[id*=txtTExtra]');
										var txtTotal = $tr.find('input[id*=txtTTotal]');
										var txtMisc = $tr.find('input[id*=txtTMisc]');
										var txtToll = $tr.find('input[id*=txtPToll]');
										var txtTimeTotal = $tr.find('input[id*=txtPTimeTotal]');
										var txtJobRate = $tr.find('input[id*=txtTJobRate]');
										var chkTJobRate = $tr.find('input[id*=chkTJobRate]');
										var txtTCustomTick2 = $tr.find('input[id*=txtTCustomTick2]');

										var total = 0;
										var TimeAmount = 0;
										var TotalTime = Isnull(txtReg.val()) + Isnull(txtOT.val()) + Isnull(txtNT.val()) + Isnull(txtDT.val()) + Isnull(txtTT.val());
										var OvertimeAmount = 0;
										txtTimeTotal.val(TotalTime.toFixed(2));
										if (!isNaN(parseFloat(txtRate.val()))) {

											if ($(lblMethodID).text() == '0') {
												TimeAmount = 0;
											}
											else if ($(lblMethodID).text() == '1') {
												OvertimeAmount = Isnull(txtReg.val()) + (Isnull(txtNT.val()) * 1.7) + (Isnull(txtDT.val()) * 2) + Isnull(txtTT.val()) + (Isnull(txtOT.val()) * 1.5);
												if (chkTJobRate.is(':checked') == true)
													TimeAmount = OvertimeAmount * Isnull(txtJobRate.val());
												else
													TimeAmount = OvertimeAmount * Isnull(txtRate.val());
											}
											else if ($(lblMethodID).text() == '2') {
												TimeAmount = 0;
											}
										}
										$(txtAmount).val(TimeAmount.toFixed(2));
										total = TimeAmount + Isnull(txtZone.val()) + (Isnull(txtMileage.val()) * Isnull(lblMileageRate.text())) + Isnull(txtExtra.val()) + Isnull(txtMisc.val()) + Isnull(txtToll.val()) + Isnull(txtCustom.val()) + Isnull(txtTCustomTick2.val());
										$tr.find('input[id*=txtTTotal]').val(total.toFixed(2));

										Timetotals += TotalTime;
										regtotal += Isnull(txtReg.val());
										ottotal += Isnull(txtOT.val());
										nttotal += Isnull(txtNT.val());
										dttotal += Isnull(txtDT.val());
										tttotal += Isnull(txtTT.val());

										if ($(lblMethodID).text() == '1') {
											tAmtotal += Isnull(TimeAmount);
										}

										zonetotal += Isnull(txtZone.val());
										mileagetotal += Isnull(txtMileage.val());
										extratotal += Isnull(txtExtra.val());
										misctotal += Isnull(txtMisc.val());
										tolltotal += Isnull(txtToll.val());
										grandtotal += total;
										CustomTotal += Isnull(txtCustom.val());
										CustomT2Total += Isnull(txtTCustomTick2.val());

									});

									if ($(lblMethodID).text() == '0') {
										tAmtotal = Isnull(lblSalary.text());
									}
									else if ($(lblMethodID).text() == '2') {
										tAmtotal = Isnull(lblHours.text()) * Isnull(lblHourlyRate.text());
									}

									$(Gridview).find('tr:last').each(function () {
										var $tr = $(this);
										var txtCustom = $tr.find('input[id*=lblTCustom]');
										var txtRate = $tr.find('input[id*=lblTRate]');
										var txtReg = $tr.find('input[id*=lblTReg]');
										var txtOT = $tr.find('input[id*=lblTOT]');
										var txtNT = $tr.find('input[id*=lblTNT]');
										var txtDT = $tr.find('input[id*=lblTDT]');
										var txtTT = $tr.find('input[id*=lblTravel]');
										var txtTAmount = $tr.find('input[id*=lblTAmount]');
										var txtZone = $tr.find('input[id*=lblTZone]');
										var txtMileage = $tr.find('input[id*=lblTMileage]');
										var txtExtra = $tr.find('input[id*=lblTExtra]');
										var txtTotal = $tr.find('input[id*=lblTGtotal]');
										var txtMisc = $tr.find('input[id*=lblTMisc]');
										var txtToll = $tr.find('input[id*=lblPToll]');
										var txtCustomTick2 = $tr.find('input[id*=lblTCustomTick2]');
										var txtTimetotal = $tr.find('input[id*=lblPTimeTotal]');

										$(txtCustom).val(CustomTotal.toFixed(2));
										$(txtReg).val(regtotal.toFixed(2));
										$(txtOT).val(ottotal.toFixed(2));
										$(txtNT).val(nttotal.toFixed(2));
										$(txtDT).val(dttotal.toFixed(2));
										$(txtTT).val(tttotal.toFixed(2));
										$(txtTAmount).val(tAmtotal.toFixed(2));
										$(txtZone).val(zonetotal.toFixed(2));
										$(txtMileage).val(mileagetotal.toFixed(2));
										$(txtExtra).val(extratotal.toFixed(2));
										$(txtTotal).val(grandtotal.toFixed(2));
										$(txtMisc).val(misctotal.toFixed(2));
										$(txtToll).val(tolltotal.toFixed(2));
										$(txtCustomTick2).val(CustomT2Total.toFixed(2));
										$(txtTimetotal).val(Timetotals.toFixed(2));

										var txtPCustom = trprev.find('input[id*=txtCustom]');
										var txtPRate = trprev.find('input[id*=txtRate]');
										var txtPReg = trprev.find('input[id*=txtReg]');
										var txtPOT = trprev.find('input[id*=txtOT]');
										var txtPNT = trprev.find('input[id*=txtoneseven]');
										var txtPDT = trprev.find('input[id*=txtDT]');
										var txtPTT = trprev.find('input[id*=txtPTravel]');
										var txtPAmt = trprev.find('input[id*=txtAmount]');
										var txtPZone = trprev.find('input[id*=txtZone]');
										var txtPMileage = trprev.find('input[id*=txtMileage]');
										var txtPExtra = trprev.find('input[id*=txtExtra]');
										var txtPMisc = trprev.find('input[id*=txtMisc]');
										var txtPToll = trprev.find('input[id*=txtToll]');
										var txtPCustomT2 = trprev.find('input[id*=txtCustomT2]');

										$(txtPCustom).val(CustomTotal.toFixed(2));
										if ($(lblMethodID).text() == '2')
											$(txtPReg).val(lblHours.text());
										else
											$(txtPReg).val(regtotal.toFixed(2));
										$(txtPOT).val(ottotal.toFixed(2));
										$(txtPNT).val(nttotal.toFixed(2));
										$(txtPDT).val(dttotal.toFixed(2));
										$(txtPTT).val(tttotal.toFixed(2));
										$(txtPAmt).val(tAmtotal.toFixed(2));
										$(txtPZone).val(zonetotal.toFixed(2));
										$(txtPMileage).val(mileagetotal.toFixed(2));
										$(txtPExtra).val(extratotal.toFixed(2));
										$(txtPMisc).val(misctotal.toFixed(2));
										$(txtPToll).val(tolltotal.toFixed(2));
										$(txtPCustomT2).val(CustomT2Total.toFixed(2));

										CalculateTimesheet();
									});

								}

								function CalculateTimesheet() {
									var Gridview = $("#" + '<%= gvTimesheet.ClientID %>');
									var regtotal = 0;
									var ottotal = 0;
									var nttotal = 0;
									var dttotal = 0;
									var tttotal = 0;
									var Amttotal = 0;
									var zonetotal = 0;
									var mileagetotal = 0;
									var extratotal = 0;
									var Holidaytotal = 0;
									var Vacationtotal = 0;
									var Sicktotal = 0;
									var Reimbtotal = 0;
									var Bonustotal = 0;
									var grandtotal = 0;
									var misctotal = 0;
									var tolltotal = 0;
									var customtotal = 0;
									var customT2total = 0;

									$(Gridview).find('tr:not(:first,:last)').each(function () {
										var $tr = $(this);
										var txtCustom = $tr.find('input[id*=txtCustom]');
										var txtReg = $tr.find('input[id*=txtReg]');
										var txtOT = $tr.find('input[id*=txtOT]');
										var txtNT = $tr.find('input[id*=txtoneseven]');
										var txtDT = $tr.find('input[id*=txtDT]');
										var txtTT = $tr.find('input[id*=txtPTravel]');
										var txtAmount = $tr.find('input[id*=txtAmount]');
										var txtTimeTotal = $tr.find('input[id*=txtTimeTotal]');

										var txtZone = $tr.find('input[id*=txtZone]');
										var txtMileage = $tr.find('input[id*=txtMileage]');
										var txtExtra = $tr.find('input[id*=txtExtra]');
										var txtHoliday = $tr.find('input[id*=txtHoliday]');
										var txtVacation = $tr.find('input[id*=txtVacation]');
										var txtSick = $tr.find('input[id*=txtSick]');
										var txtReimb = $tr.find('input[id*=txtReimb]');
										var txtBonus = $tr.find('input[id*=txtBonus]');
										var txtTotal = $tr.find('input[id*=txtTotal]');
										var txtMisc = $tr.find('input[id*=txtMisc]');
										var txtToll = $tr.find('input[id*=txtToll]');
										var txtSalary = $tr.find('input[id*=txtSalary]');
										var txtCustomT2 = $tr.find('input[id*=txtCustomT2]');

										var lblMethodID = $tr.find('span[id*=lblMID]');
										var lblHours = $tr.find('span[id*=lblHours]');
										var lblSalary = $tr.find('span[id*=lblSalary]');
										var lblHourlyRate = $tr.find('span[id*=lblHourlyRate]');
										var lblMlRate = $tr.find('span[id*=lblMlRate]');

										$(txtTimeTotal).val((Isnull(txtReg.val()) + Isnull(txtOT.val()) + Isnull(txtNT.val()) + Isnull(txtDT.val()) + Isnull(txtTT.val())).toFixed(2));

										if ($(lblMethodID).text() != '1') {
											$(txtAmount).val(($(txtTimeTotal).val() * Isnull(lblHourlyRate.text())).toFixed(2));
										}

										var TimeAmount = $(txtAmount).val();

										var total = Isnull(TimeAmount) + Isnull(txtZone.val()) + (Isnull(txtMileage.val()) * Isnull(lblMlRate.text())) + Isnull(txtExtra.val()) + (Isnull(txtHoliday.val()) * Isnull(lblHourlyRate.text())) + (Isnull(txtVacation.val()) * Isnull(lblHourlyRate.text())) + (Isnull(txtSick.val()) * Isnull(lblHourlyRate.text())) + Isnull(txtReimb.val()) + Isnull(txtBonus.val()) + Isnull(txtToll.val()) + Isnull(txtMisc.val()) + Isnull(txtSalary.val()) + Isnull(txtCustom.val()) + Isnull(txtCustomT2.val());  // +(Isnull(txtFixedH.val()) * Isnull(lblHourlyRate.text()));
										if ($(lblMethodID).text() == '0')
											total = Isnull(TimeAmount) + Isnull(txtZone.val()) + (Isnull(txtMileage.val()) * Isnull(lblMlRate.text())) + Isnull(txtExtra.val()) + (Isnull(txtHoliday.val())) + (Isnull(txtVacation.val())) + (Isnull(txtSick.val())) + Isnull(txtReimb.val()) + Isnull(txtBonus.val()) + Isnull(txtToll.val()) + Isnull(txtMisc.val()) + Isnull(txtSalary.val()) + Isnull(txtCustom.val()) + Isnull(txtCustomT2.val()); //+(Isnull(txtFixedH.val()) * Isnull(lblHourlyRate.text()));

										$tr.find('input[id*=txtTotal]').val(total.toFixed(2));

										regtotal += Isnull(txtReg.val());
										ottotal += Isnull(txtOT.val());
										nttotal += Isnull(txtNT.val());
										dttotal += Isnull(txtDT.val());
										tttotal += Isnull(txtTT.val());
										Amttotal += Isnull(TimeAmount);
										zonetotal += Isnull(txtZone.val());
										mileagetotal += Isnull(txtMileage.val());
										extratotal += Isnull(txtExtra.val());
										Holidaytotal += Isnull(txtHoliday.val());
										Vacationtotal += Isnull(txtVacation.val());
										Sicktotal += Isnull(txtSick.val());
										Reimbtotal += Isnull(txtReimb.val());
										Bonustotal += Isnull(txtBonus.val());
										grandtotal += total;
										misctotal += Isnull(txtMisc.val());
										tolltotal += Isnull(txtToll.val());
										customtotal += Isnull(txtCustom.val());
										customT2total += Isnull(txtCustomT2.val());
									});

									$(Gridview).find('tr:last').each(function () {
										var $tr = $(this);
										var txtCustom = $tr.find('input[id*=lblCustom]');
										var txtReg = $tr.find('input[id*=lblReg]');
										var txtOT = $tr.find('input[id*=lblOT]');
										var txtNT = $tr.find('input[id*=lblNT]');
										var txtDT = $tr.find('input[id*=lblDT]');
										var txtTT = $tr.find('input[id*=lblPTravel]');
										var txtAmt = $tr.find('input[id*=lblAmount]');
										var txtZone = $tr.find('input[id*=lblZone]');
										var txtMileage = $tr.find('input[id*=lblMileage]');
										var txtExtra = $tr.find('input[id*=lblExtra]');
										var txtHoliday = $tr.find('input[id*=lblHoliday]');
										var txtVacation = $tr.find('input[id*=lblVacation]');
										var txtSick = $tr.find('input[id*=lblSick]');
										var txtReimb = $tr.find('input[id*=lblReimb]');
										var txtBonus = $tr.find('input[id*=lblBonus]');
										var txtTotal = $tr.find('input[id*=lblGtotal]');
										var txtMisc = $tr.find('input[id*=lblMisc]');
										var txtToll = $tr.find('input[id*=lblToll]');
										var txtCustomT2 = $tr.find('input[id*=lblCustomT2]');

										$(txtCustom).val(customtotal.toFixed(2));
										$(txtReg).val(regtotal.toFixed(2));
										$(txtOT).val(ottotal.toFixed(2));
										$(txtNT).val(nttotal.toFixed(2));
										$(txtDT).val(dttotal.toFixed(2));
										$(txtTT).val(tttotal.toFixed(2));
										$(txtAmt).val(Amttotal.toFixed(2));
										$(txtZone).val(zonetotal.toFixed(2));
										$(txtMileage).val(mileagetotal.toFixed(2));
										$(txtExtra).val(extratotal.toFixed(2));
										$(txtReimb).val(Reimbtotal.toFixed(2));
										$(txtBonus).val(Bonustotal.toFixed(2));
										$(txtTotal).val(grandtotal.toFixed(2));
										$(txtMisc).val(misctotal.toFixed(2));
										$(txtCustomT2).val(customT2total.toFixed(2));
										$(txtToll).val(tolltotal.toFixed(2));
									});
								}

								function Isnull(value) {
									value = parseFloat(value);
									if (isNaN(value)) {
										value = 0;
									}
									return value;
								}
	</script>
</asp:Content>
