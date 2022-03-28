<%@ Page Title="" Language="C#" MasterPageFile="~/MOMRadWindow.master" AutoEventWireup="true" CodeBehind="PayrollCalculationDetials.aspx.cs" Inherits="MOMWebApp.PayrollCalculationDetials" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div style="padding-top: 12px;">
        <div class="row" style="margin-bottom: 0px!important; padding-left: 0; padding-right: 0;">
            <div class="input-field col s3 main-name">
                <div class="row">
                    <asp:Label ID="lblname" runat="server" Text="" Style="font-weight: 700;"></asp:Label>
                    <br />
                    <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="lblCityStateZip" runat="server" Text=""></asp:Label>
                </div>
            </div>

            <div class="input-field col s3 main-filed">
                <div class="row " style="margin-bottom: 0px!important">
                    <asp:TextBox ID="txttotalWages" runat="server" Enabled="false" Style="font-weight: bold; color: #000000bf;"></asp:TextBox>
                    <label for="txttotalWages" style="font-weight: bold; color: #0000009e;">Total Wages</label>
                </div>
            </div>
            <div class="input-field col s3 3 main-filed">
                <div class="row " style="margin-bottom: 0px!important">
                    <asp:TextBox ID="txttotaldeductions" runat="server" ReadOnly="true" Style="font-weight: bold; color: #000000bf;"></asp:TextBox>
                    <label for="txttotaldeductions" style="font-weight: bold; color: #0000009e;">Total Deductions</label>
                </div>
            </div>
            <div class="input-field col s3 3 main-filed">
                <div class="row " style="margin-bottom: 0px!important">
                    <asp:TextBox ID="txtnetpay" runat="server" ReadOnly="true" Style="font-weight: bold; color: #000000bf;"></asp:TextBox>
                    <label for="txtnetpay" style="font-weight: bold; color: #0000009e;">Net Pay</label>
                </div>
            </div>
        </div>
    </div>--%>
    <asp:HiddenField runat="server" ID="hdnEmpId" />
    <div class="row">
        <ul class="tabs tab-demo-active white" id="tabProject" runat="server" style="width: 100%;">
            <li class="tab col s2" id="liVertexRequest" runat="server">
                <a class="white-text waves-effect waves-light active" id="aVendorCheck" href="#dvRequest" runat="server">Request</a>
            </li>
            <li class="tab col s2" id="liVertexResponse" runat="server">
                <a class="white-text waves-effect waves-light" id="aPayCheck" href="#dvResponse" runat="server">Response</a>
            </li>
            <li class="tab col s2" id="liDeductions" runat="server">
                <a class="white-text waves-effect waves-light active" id="aDeduction" href="#dvDeduction" runat="server">Deduction</a>
            </li>
          <%--  <li class="tab col s2" id="liCalculations" runat="server">
                <a class="white-text waves-effect waves-light" id="aCalculation" href="#dvCalculations" runat="server">Calculation</a>
            </li>--%>
        </ul>
    </div>
    <div id="dvRequest" class="col s12 tab-container-border lighten-4" style="display: block; padding: 1px 0px;">
        <div id="divRequestView">
                <textarea class="request-holder" rows="25" cols = "60"></textarea>
        </div>
    </div>
    <div id="dvResponse" class="col s12 tab-container-border lighten-4" style="display: block; padding: 1px 0px;">
         <textarea class="response-holder" rows="25" cols = "60"></textarea>
    </div>
    <div id="dvDeduction" class="col s12 tab-container-border lighten-4" style="display: block; padding: 1px 0px;">
        <div>
             <telerik:RadGrid ID="RadGrid_Deduction" runat="server" AutoGenerateColumns="False" FilterType="CheckList"
                            CssClass="gvWagePayRate table table-bordered table-striped table-condensed flip-content" margin-bottom="0px"
                            AllowSorting="true" ShowFooter="true" EnableViewState="true" OnNeedDataSource="RadGrid_Deduction_NeedDataSource">
                            <AlternatingItemStyle CssClass="oddrowcolor" />
                            <ActiveItemStyle CssClass="evenrowcolor" />
                            <FooterStyle CssClass="footer" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="True"></Selecting>
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True" DataKeyNames="ID">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Deductions" SortExpression="Desc" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDDesc" runat="server" Style="font-size: 12px;" Text='<%# Bind("fDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" Text="Total :-"></asp:Label>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Amount" HeaderText="Amount($)" SortExpression="Amount" AllowFiltering="false" FooterAggregateFormatString="{0:C}" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDAmt" runat="server" Style="text-align: right; font-size: 12px;" Text='<%# Bind("Amount")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="YTDAmount" HeaderText="YTD($)" SortExpression="YTD" AllowFiltering="false" FooterAggregateFormatString="{0:C}" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDAmtYTD" runat="server" Style="text-align: right; font-size: 12px;" Text='<%# Bind("YTDAmount")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <SelectedItemStyle CssClass="selectedrowcolor" />
                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                            </FilterMenu>
                        </telerik:RadGrid>
        </div>
    </div>
    <div id="dvCalculations" class="col s12 tab-container-border lighten-4" style="display: block; padding: 1px 0px;">
        <div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="server">
    <script src="js/commonAPI.js"></script>
    <script type="text/javascript">
        $(function () {
            var xmlRequest = $("[id*=xmlRequest]");
            var xmlResponse = $("[id*=xmlResponse]");
            var empId = $("[id*=hdnEmpId]").val();
            var Model = {
                FrequencyId: empId,
            };
            AccessControler.Post(Model, 30000, "./api/PayrollApi/GetVertexData", function (response) {
                if (response != null) {
                    var json = JSON.parse(response);
                    var prettyXmlRequest = prettifyXml([
                        json[0].Request
                    ].join('\n'));
                    var prettyXmlResponse = prettifyXml([
                        json[0].Request
                    ].join('\n'));
                    document.querySelector('.request-holder').value = prettyXmlRequest;
                    document.querySelector('.response-holder').value = prettyXmlResponse;
                }
            });
            return false;
        });

        var prettifyXml = function (sourceXml) {
            var xmlDoc = new DOMParser().parseFromString(sourceXml, 'application/xml');
            var xsltDoc = new DOMParser().parseFromString([
                // describes how we want to modify the XML - indent everything
                '<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform">',
                '  <xsl:strip-space elements="*"/>',
                '  <xsl:template match="para[content-style][not(text())]">', // change to just text() to strip space in text nodes
                '    <xsl:value-of select="normalize-space(.)"/>',
                '  </xsl:template>',
                '  <xsl:template match="node()|@*">',
                '    <xsl:copy><xsl:apply-templates select="node()|@*"/></xsl:copy>',
                '  </xsl:template>',
                '  <xsl:output indent="yes"/>',
                '</xsl:stylesheet>',
            ].join('\n'), 'application/xml');

            var xsltProcessor = new XSLTProcessor();
            xsltProcessor.importStylesheet(xsltDoc);
            var resultDoc = xsltProcessor.transformToDocument(xmlDoc);
            var resultXml = new XMLSerializer().serializeToString(resultDoc);
            return resultXml;
        };

       
      

    </script>
</asp:Content>
