<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TelerikCustomGantt.aspx.cs" Inherits="MOMWebApp.TelerikCustomGantt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml"   style="height: 100%;">
<head runat="server">
    <title></title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <style >
        #ganttPanel{
            height:100%;
            margin: 0;
            padding: 0;
            font-family:Lato, sans-serif;
            font-size:13px;
        }
        #gantt {
            height: 100% !important;
            margin: 0;
            padding: 0;
            border: none;
            font-family: Lato, sans-serif;
            font-size: 13px;
        }
        div.k-widget.k-window.radSkin_Bootstrap{
            font-family: Lato, sans-serif;
            font-size: 13px;
        }

        /* Hide toolbars during export */
        .k-pdf-export .rgtToolbar {
            display: none;
        }

        .rgtToolbar.rgtFooter {
            height:0px;
        }
    </style>
</head>
<body style="height: 100%;margin:0">
    
    <form id="form1" runat="server" style="
        height: 99%;
        width: 99.7%;
">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <script type="text/javascript">
            //Put your JavaScript code here.
        </script>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gantt">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gantt" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <div style="height: 100%;margin:0">
            <telerik:RadGantt runat="server"
                ID="gantt"
                AutoGenerateColumns="false"
                SelectedView="WeekView"
                EnableResources="true"
                EnablePdfExport="false" 
                >
                <Columns>
                    <%--<telerik:GanttBoundColumn DataField="Id" Width="80"></telerik:GanttBoundColumn>--%>
                    <telerik:GanttBoundColumn DataField="Title" Width="250"></telerik:GanttBoundColumn>
                    <telerik:GanttBoundColumn DataField="Start" HeaderText="Start Date" Width="149"></telerik:GanttBoundColumn>
                    <telerik:GanttBoundColumn DataField="End" HeaderText="End Date"  Width="149"></telerik:GanttBoundColumn>
                    <telerik:GanttBoundColumn DataField="PercentComplete" HeaderText="% Completed"  Width="100"></telerik:GanttBoundColumn>
                    <telerik:GanttResourceColumn  HeaderText="Assigned Resources" Width="120"></telerik:GanttResourceColumn>
                    <%--<telerik:GanttDependencyColumn />--%>
                    <%--<telerik:GanttDe HeaderText="Assigned Resources"></telerik:GanttDe>--%>
                    <telerik:GanttBoundColumn DataField="CusDuration" HeaderText="Duration" Width="80" DataType="Number" UniqueName="CusDuration"></telerik:GanttBoundColumn>
                    <telerik:GanttBoundColumn DataField="Vendor" HeaderText="Vendor" DataType="String" UniqueName="Vendor" Width="155"></telerik:GanttBoundColumn>
                    <telerik:GanttBoundColumn DataField="Description" HeaderText="Notes" DataType="String" UniqueName="Description" Width="120"></telerik:GanttBoundColumn>
                    
                    
                </Columns>
                <CustomTaskFields >
                    <telerik:GanttCustomField PropertyName="Description" ClientPropertyName="description"  />
                    <telerik:GanttCustomField PropertyName="CusDuration" ClientPropertyName="cusDuration"  />
                    <telerik:GanttCustomField PropertyName="Vendor" ClientPropertyName="vendor"  />
                    <telerik:GanttCustomField PropertyName="VendorID" ClientPropertyName="vendorId"  />
                </CustomTaskFields>
                <WebServiceSettings Path="GanttCustomService.asmx" />
            </telerik:RadGantt>
        </div>
    </form>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <script>

        function gantt_add(e) {
            setTimeout(function () {
                var gantt = $("#gantt").getKendoGantt();
                //gantt.dataSource.read()
                gantt.dependencies.read()
            }, 100);
        }
        function gantt_dataBinding(e) {
            //alert("dataBinding: ");
        }
        function gantt_dataBound(e) {
            //alert("dataBoun: ");
        }
        function gantt_navigate(e) {
            //alert("navigat: ");
        }
        function dtaa() {
            this.prefixText = null;
            this.con = null;
            this.custID = null;
        }
        function gantt_edit(e) {
            //alert("Editing task: ", e.task.title)
            setTimeout(function () {
                $('.rgtTreelistContent.radGridContent>table>tbody>tr>td>input[name="vendor"]').autocomplete({
                    source: function (request, response) {
                        //var txtVendor = this.id;
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetVendorNameProject",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load vendor name");
                            }
                        });
                    },
                    select: function (event, ui) {
                        //var txtVendor = this.id;
                        //var hdnVendorID = document.getElementById(txtVendor.replace('txtVendor', 'hdnVendorId'));
                        var str = ui.item.Name;
                        var strId = ui.item.ID;
                        if (str == "No Record Found!") {
                            $(this).val("");
                        }
                        else {
                            $(this).val(str);
                            //(hdnVendorID).val(strId);
                        }
                        return false;
                    },
                    focus: function (event, ui) {
                        //var txtVendor = this.id;
                        var str = ui.item.Name;
                        if (str == "No Record Found!") {
                            $(this).val("");
                        }
                        else {
                            $(this).val(str);
                            //(hdnVendorID).val(strId);
                        }
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".radTextbox"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.ID;
                        var result_item = item.Name;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_value == 0) {
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                        else {
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                    };
                });
            }, 50);
        }
        //function gantt_change(e) {

        //    alert("change: ")
        //}
        function gantt_save(e) {
            setTimeout(function () {
                var gantt = $("#gantt").getKendoGantt();
                gantt.dataSource.read()
                gantt.dependencies.read()
            },50);
        }
        $(document).ready(function () {
            var gantt = $("#gantt").data("kendoGantt");
            gantt.bind("save", gantt_save);
            gantt.bind("add", gantt_add);

            gantt.bind("edit", gantt_edit);
            //gantt.bind("change", gantt_change);
            gantt.bind("navigate", gantt_navigate);
            gantt.bind("dataBinding", gantt_dataBinding);
            gantt.bind("dataBound", gantt_dataBound);

            $('.rgtToolbar.rgtFooter').hide();
        });
    </script>
</body>
</html>
