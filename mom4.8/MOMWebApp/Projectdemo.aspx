<%@ Page Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="Projectdemo" Codebehind="Projectdemo.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        var selectedpage = 1;
        var sort = "asc";
        var sortcol = "ID";

        $(document).ready(function () {



            GetProject(1, -1);
            $(".ajax__tab_header").find("span").removeClass("ajax__tab_active");

            $("#spnid-1").addClass("ajax__tab_active");

            $('.ajax__tab_tab').live('click', function () {
                sort = "asc";
                sortcol = "ID";

                GetProject(1, $(this).attr("id"));

                $(".ajax__tab_header").find("span").removeClass("ajax__tab_active");

                $("#spnid" + $(this).attr("id")).addClass("ajax__tab_active");



            });

            $("#tblpdetails .clickable").live("click", function (e, val) {

                var item = $(e, this);
                //var val = $(this).find("#chkSelect").is(":checked")

                if ($(this).closest("tr").hasClass("selectedRow") == true) {
                    $(this).find("#chkSelect").attr("checked", false);
                    $(this).removeClass("selectedRow");
                }
                else {
                    $("#tblpdetails tr").removeClass("selectedRow");
                    $("#tblpdetails tr").find("#chkSelect").attr("checked", false);
                    if ($(this).first().hasClass("header") != true) {

                        $(this).find("#chkSelect").attr("checked", true);
                        $(this).addClass("selectedRow");
                        return;

                    }
                }

                //$("#tblpdetails tr").removeClass("selectedRow");
                //$("#tblpdetails tr").find("#chkSelect").attr("checked", false);


            });

            $("#tblpdetails .clickable").live("dblclick", function (e, val) {

                if ($(this).first().hasClass("header") != true) {

                    var id = $(this).find("td #hdnid").val();

                    window.open('addProject.aspx?uid=' + id);
                }
            });

            $('#lnkClear').live("click", function () {
                //alert("sds");
                $("#ddlSearch").val($('#ddlSearch option').first().val()).change();
                $("#ddllocation").val($('#ddllocation option').first().val()).change();
                $("#ddlStatus").val($('#ddlStatus option').first().val()).change();
                $("#ddlDateRange").val($('#ddlDateRange option').first().val()).change();
                $("#ddlPages").val($('#ddlPages option').first().val()).change();
                $("#txtSearch").val('');
                $("#txtInvDt").val('');
                $("#txtfromDate").val('');
                $("#txtToDate").val('');

                GetProject(1, -1);

                return false;
            });

            $("#ddlPages").live('change', function () {

                GetProject($("#ddlPages option:selected").text(), $(".ajax__tab_active").find('.ajax__tab_tab').attr("id"));

            });

            $(".pagefirst").live('click', function () {

                $("#ddlPages").val($('#ddlPages option').first().val()).change();
            });

            $(".pagelast").live('click', function () {
                $("#ddlPages").val($('#ddlPages option').last().val()).change();

            });

            $(".pageprev").live('click', function () {
                if ($('#ddlPages option:selected').prev().val() > 0)
                    $("#ddlPages").val($('#ddlPages option:selected').prev().val()).change();
                else
                    return;

            });

            $(".pagenext").live('click', function () {
                if ($('#ddlPages option:selected').next().val() > 0)
                    $("#ddlPages").val($('#ddlPages option:selected').next().val()).change();
                else
                    return;

            });

            $(".fa-search").live('click', function () {

                GetProject($("#ddlPages option:selected").text(), $(".ajax__tab_active").find('.ajax__tab_tab').attr("id"));
                return false;
            });

            $("#ddlSearch").live('change', function () {

                var value = $("#ddlSearch option:selected").val();
                if (value == "j.Status") {

                    $("#<%=ddlStatus.ClientID%>").removeClass("hide");
                    $("#<%=txtSearch.ClientID%>").addClass("hide");
                    $("#<%=txtInvDt.ClientID%>").addClass("hide");
                    $("#<%=ddllocation.ClientID%>").addClass("hide");

                    //ddlStatus.Visible = true;
                    //txtSearch.Visible = false;
                    //txtInvDt.Visible = false;
                    //ddllocation.Visible = false;
                }
                else if (value == "j.fdate") {

                    $("#<%=ddlStatus.ClientID%>").addClass("hide");
                    $("#<%=txtSearch.ClientID%>").addClass("hide");
                    $("#<%=txtInvDt.ClientID%>").removeClass("hide");
                    $("#<%=ddllocation.ClientID%>").addClass("hide");

                    //ddlStatus.Visible = false;
                    //txtSearch.Visible = false;
                    //txtInvDt.Visible = true;
                    //ddllocation.Visible = false;

                }
                else if (value == "j.loc") {


                    $("#<%=ddlStatus.ClientID%>").addClass("hide");
                    $("#<%=txtSearch.ClientID%>").addClass("hide");
                    $("#<%=txtInvDt.ClientID%>").addClass("hide");
                    $("#<%=ddllocation.ClientID%>").removeClass("hide");

                    //ddlStatus.Visible = false;
                    //txtSearch.Visible = false;
                    //txtInvDt.Visible = false;
                    //ddllocation.Visible = true;

                }
                else {

                    $("#<%=ddlStatus.ClientID%>").addClass("hide");
                    $("#<%=txtSearch.ClientID%>").removeClass("hide");
                    $("#<%=txtInvDt.ClientID%>").addClass("hide");
                    $("#<%=ddllocation.ClientID%>").addClass("hide");

                    //ddlStatus.Visible = false;
                    //txtSearch.Visible = true;
                    //txtInvDt.Visible = false;
                    //ddllocation.Visible = false;

                }
            });

            $("#ddlDateRange").live('change', function () {
                var selectedVal = $("#ddlDateRange option:selected").val();
                if (selectedVal == 1) {
                    $('#txtfromDate').addClass("hide");
                    $('#txtToDate').addClass("hide");

                }
                else {
                    $('#txtfromDate').removeClass("hide");
                    $('#txtToDate').removeClass("hide");
                }
            });


        });


function GetProject(pageindex, tabindex) {



    $("#iframeloading").show();
    $("#tblpdetails").remove();
    var column = $('#ddlSearch option:selected').val();
    selectedpage = pageindex;
    var searchterm = SearchTerm();
    // alert($('#ddlDateRange option:selected').val());

    $.ajax({
        type: "POST",
        url: "ProjectDemo.aspx/GetProject",
        data: '{searchBy: "' + column + '", SearchValue: "' + searchterm + '", page:"' + pageindex + '",stdate:"' + $("#txtfromDate").val() + '",enddate:"' + $("#txtToDate").val() + '",Department:"' + tabindex + '",Range:"' + $('#ddlDateRange option:selected').val() + '",SortDir:"' + sort + '",SortCol:"' + sortcol + '"}',
        //data: '{searchTerm: "", column: "", page:"' + pageindex + '",stdate:"",enddate:"",Department:"' + tabindex + '"}',

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: OnSuccess,
        failure: function (response) {
            //alert(response.d);
            $('#iframeloading').hide();


        },
        error: function (response) {
            $('#iframeloading').hide();



        }
    });

}


function OnSuccess(response) {

    var result = response.d.ReponseObject;

    if (response.d.Header.HasError == false) {

        var trHTML = '';
        var totalhour = 0.00;
        var totalOnOrder = 0.00;
        var totalbilled = 0.00;
        var totallabour = 0.00;
        var totalmaterial = 0.00;
        var totalexpenses = 0.00;
        var totalOveallexpenses = 0.00;
        var totalnet = 0.00;
        var totalprofit = 0.00;


        //alert(result.Items.length);
        trHTML += '<table id="tblpdetails" class="table table-bordered table-striped table-condensed flip-content" rules="all" style="width:100%;border-collapse:collapse;" border="1" cellspacing="0">'
        trHTML += '<tbody><tr class="header"><th scope="col">&nbsp;</th><th scope="col"><a id="Tag" onclick="Sort(this);"><input type="hidden" value="Tag"/>Location</a></th><th scope="col"><a id="ID" onclick="Sort(this);"><input type="hidden" value="ID"/>Project #</a></th>'
            + '<th scope="col"><a id="fdesc" onclick="Sort(this);"><input type="hidden" value="fdesc"/>Desc</a></th><th scope="col"><a id="Status" onclick="Sort(this);"><input type="hidden" value="Status"/>Status</a></th>' +
            '<th scope="col"><a id="fDate" onclick="Sort(this);"><input type="hidden" value="fDate"/>Date Created</a></th><th scope="col"><a id="NHour" onclick="Sort(this);"><input type="hidden" value="NHour"/>Hours</a></th>' +
            '<th scope="col"><a id="NComm" onclick="Sort(this);"><input type="hidden" value="NComm"/>Total On Order</a></th><th scope="col"><a id="NRev" onclick="Sort(this);"><input type="hidden" value="NRev"/>Total Billed</a></th>' +
            '<th scope="col"><a id="NLabor" onclick="Sort(this);"><input type="hidden" value="NLabor"/>Labor Expense</a></th><th scope="col"><a id="NMat" onclick="Sort(this);"><input type="hidden" value="NMat"/>Material Expense</a></th>' +
            '<th scope="col"><a id="NOMat" onclick="Sort(this);"><input type="hidden" value="NOMat"/>Expenses</a></th><th scope="col"><a id="NCost" onclick="Sort(this);"><input type="hidden" value="NCost"/>Total Expenses</a></th>' +
            '<th scope="col"><a id="NProfit" onclick="Sort(this);"><input type="hidden" value="NProfit"/>Net</a></th><th scope="col"><a id="NRatio" onclick="Sort(this);"><input type="hidden" value="NRatio"/>% in Profit</a></th></tr>';


        $.each(result.Items, function (i, item) {


            // alert(i % 2);
            var rowcss = i % 2 == 0 ? "evenrowcolor" : "oddrowcolor";
            trHTML += '<tr class="' + rowcss + ' clickable" style="cursor:pointer;">'

            //for (var i = 0; i < item.length; i++) {


                    <%--<asp:HiddenField ID="hdnid" runat="server" Value='<%# Bind("ID") %>' ClientIDMode="Static" />--%>
            trHTML += '<td style="width:1%;"><input id="chkSelect" name="chkSelect" type="checkbox"><input type="hidden" id="hdnid" value="' + item.ID + '"/></td>'
                   + '<td style="width:8%;"><span>' + item.Location + '</span></td>'
                   + '<td style="width:1%;"><span id="lblID">' + item.ID + '</span></td>'
                   + '<td style="width:8%;"><span id="lblDesc">' + item.Description + '</span></td>'
                   + '<td style="width:1%;"><span id="lblstatus">' + item.Status + '</span></td>'
                   + '<td style="width:2%;"><span id="lblFDate">' + item.DateCreated + '</span></td>'
                   + '<td style="width:5%;" align="right"><span id="lblHour">' + item.Hours + '</span></td>'
                   + '<td style="width:5%;" align="right"><span id="lblpo">' + item.TotalOnOrder + '</span></td>'
                + '<td style="width:5%;" align="right"><span id="lblRev" style="color:' + item.TotalBilledcolor + ';">' + item.TotalBilledText + '</span></td>'


                + '<td style="width:5%;" align="right"><span id="lblLaborExp" style="color:' + item.LaborExpensecolor + ';">' + item.LaborExpenseText + '</span> </td>'


                + '<td style="width:5%;" align="right"><span id="lblMaterialExp" style="color:' + item.MaterialExpensecolor + ';">' + item.MaterialExpenseText + '</span> </td>'

                + '<td style="width:5%;" align="right"><span id="lblExpenses" style="color:' + item.Expensecolor + ';">' + item.ExpenseText + '</span> </td>'

                + '<td style="width:5%;" align="right"><span id="lblTotalExp" style="color:' + item.TotalExpensescolor + ';">' + item.TotalExpensesText + '</span> </td>'

                + '<td style="width:5%;" align="right"><span id="lblNet" style="color:' + item.Netcolor + ';">' + item.NetText + '</span></td>'


                + '<td style="width:5%;" align="right"><span id="lblPercent">' + item.Profit + '</span></td>'


            // }
            trHTML += '</tr>'


        });


        if (result.Items.length > 0) {
            //Calculations
            //alert(result.Totals.HoursTotal);

            totalhour = result.Totals.HoursTotal;
            totalOnOrder = result.Totals.TotalOnOrderTotal;
            totalbilled = result.Totals.TotalBilledTotal;
            totallabour = result.Totals.LaborExpenseTotal;
            totalmaterial = result.Totals.MaterialExpenseTotal;
            totalexpenses = result.Totals.TotalExpenses;
            totalOveallexpenses = result.Totals.TotalExpensesTotal;
            totalnet = result.Totals.NetTotal;
            totalprofit = result.Totals.ProfitTotal;



            var rowcss = result.Items.length - 1 % 2 == 0 ? "evenrowcolor footer" : "oddrowcolor footer";
            trHTML += '<tr class="' + rowcss + '">'



            trHTML += '<td style="width:1%;"></td>'
                   + '<td style="width:8%;"><span>Total:</span></td>'
                   + '<td style="width:1%;"><span id="lblID"></span></td>'
                   + '<td style="width:8%;"><span id="lblDesc"></span></td>'
                   + '<td style="width:1%;"><span id="lblstatus"></span></td>'
                   + '<td style="width:2%;"><span id="lblFDate"></span></td>'
                   + '<td style="width:5%;" align="right"><span id="lblHour">' + totalhour + '</span></td>'
                   + '<td style="width:5%;" align="right"><span id="lblpo">' + totalOnOrder + '</span></td>'
                   + '<td style="width:5%;" align="right"><span id="lblRev" style="color:Black;">' + totalbilled + '</span></td>'
                   + '<td style="width:5%;" align="right"><span id="lblLaborExp" style="color:Black;">' + totallabour + '</span> </td>'
                   + '<td style="width:5%;" align="right"><span id="lblMaterialExp" style="color:Black;">' + totalmaterial + '</span> </td>'
                   + '<td style="width:5%;" align="right"><span id="lblExpenses" style="color:Black;">' + totalexpenses + '</span> </td>'
                   + '<td style="width:5%;" align="right"><span id="lblTotalExp" style="color:Black;">' + totalOveallexpenses + '</span> </td>'
                   + '<td style="width:5%;" align="right"><span id="lblNet" style="color:Black;">' + totalnet + '</span></td>'
                   + '<td style="width:5%;" align="right"><span id="lblPercent">' + totalprofit + '</span></td>'



            trHTML += '</tr>'

            //Footer
            trHTML += '<td colspan="15" class="pager"><div align="center"><a class="pagefirst"><img src="images/First.png" style="vertical-align: top !important;" /></a>&nbsp; &nbsp;<a class="pageprev"><img src="images/Backward.png" style="vertical-align: top !important;" /></a>&nbsp; &nbsp; <span>Page</span><select id="ddlPages" name="ddlPages"></select><span>of </span><span id="lblPageCount"></span>&nbsp; &nbsp;<a class="pagenext"><img src="images/Forward.png" style="vertical-align: top !important;" /></a>&nbsp; &nbsp;<a class="pagelast"><img src="images/last.png" style="vertical-align: top !important;" /></a>'
            trHTML += '</div></td></tr>'

        }




        trHTML += '</tbody></table>';

        $("#tblpdetails").remove();

        $('#dvProjecDetails').append(trHTML);

        var sel = $('#ddlPages');
        for (var pages = 0; pages < result.PageCount; pages++) {
            var page = pages + 1;
            if (selectedpage == page) {
                sel.append('<option value="' + page + '" selected="">' + page + '</option>');
            }
            else {
                sel.append('<option value="' + page + '">' + page + '</option>');
            }
        }
        // alert(selectedpage);
        //$('#ddlPages option[value=' + selectedpage + ']').attr('selected', 'selected');
        // $("#ddlPages select").val(selectedpage);

        //$("#ddlPages").val(selectedpage);
        $("#lblPageCount").text(result.PageCount);
        $("#lblRecordCount").text(result.TotalCount);

    }
    else {

        var msg = response.d.Header.ErrorMessages;
        noty({
            text: msg.join(','),
            type: 'error',
            layout: 'topCenter',
            closeOnSelfClick: false,
            timeout: false,
            theme: 'noty_theme_default',
            closable: true
        });

    }
    $("#iframeloading").hide();




}



function SearchTerm() {
    // return jQuery.trim($("[id*=txtSearch]").val());

    var searchedVal = '';
    var value = $("#ddlSearch option:selected").val();
    if (value == "j.Status") {

        searchedVal = $("#ddlStatus option:selected").val();

    }
    else if (value == "j.fdate") {


        searchedVal = $("#txtInvDt").val();


    }
    else if (value == "j.loc") {


        searchedVal = $("#ddllocation option:selected").val();
    }
    else {


        searchedVal = $("#txtSearch").val();

    }

    return searchedVal;


};


function CheckDelete() {



    if (($("#tblpdetails tr").not($("#tblpdetails tr:first-child")).find("#chkSelect:checked")).length > 0) {
        if (($("#tblpdetails tr").not($("#tblpdetails tr:first-child")).find("#chkSelect:checked")).length > 1) {
            //alert('Please select any one PO to delete.')

        }
        else {

            var res = confirm('Do you really want to delete this Project ?');
            if (res == true) {
                DeletePo();
            }

        }
    }
    else {
        alert('Please select a Project to delete.');
    }


    //else if()




    return false;
}

function DeletePo() {
    var objseq = [];
    (($("#tblpdetails tr").not("#tblpdetails tr:first-child")).find("#chkSelect:checked")).each(function () {

        var id = $(this).parent().parent().find("td #hdnid").val();

        objseq.push({ ID: id });
    });

    if (objseq != null) {
        $.ajax({
            type: "POST",
            url: "Projectdemo.aspx/DeleteProject",
            data: '{items:' + JSON.stringify(objseq) + '}',
            //data: '{searchTerm: "", column: "", page:"' + pageindex + '",stdate:"",enddate:"",Department:"' + tabindex + '"}',
            //LableName:'Materials',CalculationType:'1',ShowClaculatedValue:'undefined',InputBasedCalculationOperation:'0',Sequence:'1'
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: DeleteOnSuccess,
            failure: function (response) {
                //alert(response.d);
                $('#iframeloading').hide();


            },
            error: function (response) {
                $('#iframeloading').hide();



            }
        });
    }
}
function DeleteOnSuccess(response) {

    var result = response.d.ReponseObject;

    if (response.d.Header.HasError == false) {
        var msg = response.d.Header.ErrorMessages;
        noty({
            text: msg.join(','),
            type: 'success',
            layout: 'topCenter',
            closeOnSelfClick: false,
            timeout: false,
            theme: 'noty_theme_default',
            closable: true
        });
    }
    else {

        var msg = response.d.Header.ErrorMessages;
        noty({
            text: msg.join(','),
            type: 'error',
            layout: 'topCenter',
            closeOnSelfClick: false,
            timeout: false,
            theme: 'noty_theme_default',
            closable: true
        });
    }

    GetProject($("#ddlPages option:selected").text(), $(".ajax__tab_active").find('.ajax__tab_tab').attr("id"));

}

function Sort(col) {
    if (sortcol == col.id) {
        if (sort == "asc")
            sort = "desc";
        else
            sort = "asc";
    }
    else {
        sortcol = col.id;
        sort = "asc";

    }

    GetProject($("#ddlPages option:selected").text(), $(".ajax__tab_active").find('.ajax__tab_tab').attr("id"));

}
function CheckEdit() {

    if ($("#tblpdetails .selectedRow").length == 1) {

        var id = $("#tblpdetails .selectedRow").find('#hdnid').val();

        window.location.href = "AddProject.aspx?id=" + id;


    }
    else {
        alert("Please select a vendor.")
    }

    return false;


}

    </script>
    <style type="text/css">
        .selectedRow {
            background: rgb(249, 190, 122) none repeat scroll 0% 0% !important;
        }

        .hide {
            display: none;
        }
        /* default layout */
        .ajax__tab_default .ajax__tab_header {
            white-space: normal !important;
        }

        .ajax__tab_default .ajax__tab_outer {
            display: -moz-inline-box;
            display: inline-block;
        }

        .ajax__tab_default .ajax__tab_inner {
            display: -moz-inline-box;
            display: inline-block;
        }

        .ajax__tab_default .ajax__tab_tab {
            overflow: hidden;
            text-align: center;
            display: -moz-inline-box;
            display: inline-block;
        }

        .ajax__tab_xp .ajax__tab_disabled {
            cursor: default;
            color: #A0A0A0;
        }


        /* xp theme top / default */
        .ajax__tab_xp .ajax__tab_header {
            font-family: verdana,tahoma,helvetica;
            font-size: 11px;
            background: url(WebResource.axd?d=1O2RDXzusH5Fio72iwpRkmrYHT88DN_G2er6wemBmNXNDN7aPgMpQdQifVV0cWo7EyimMcO3Gl2SWcRZ5SFuI8Oh51KG-RemvZGttmOScR3OrUIwekw7nz3mTi4_GHj7KlSkXQ2&t=636034226650184190) repeat-x bottom;
        }

            .ajax__tab_xp .ajax__tab_header .ajax__tab_outer {
                padding-right: 4px;
                background: url(WebResource.axd?d=u0znvyHb7kytqlbLZb3T1M1a0Ak6MrgjhTK1vFOECue4hfKQ4Is03ap5nXtAsN6o-jxHLgNXOAEdoyqSjUvkJRpeJzOb_DE2rWQYVre5HpqB2sPfrx1hJsYUZllo_0fKEGGWJQ2&t=636034226650184190) no-repeat right;
                height: 21px;
            }

            .ajax__tab_xp .ajax__tab_header .ajax__tab_inner {
                padding-left: 3px;
                background: url(WebResource.axd?d=FNWYvdROq8AkSeXdZZch2BCNf_lHLyyB9odbedU8xxXkD_qy4TEGKJ4ryZb6TPrKukd3yLZL1Vs2LMdtFCawyZlDXK5YtxesdiaebJTG6JUv1bTs3bK-huoOo5ve4x-v2m_kkw2&t=636034226650184190) no-repeat;
            }

            .ajax__tab_xp .ajax__tab_header .ajax__tab_tab {
                height: 13px;
                padding: 4px;
                margin: 0px;
                background: url(WebResource.axd?d=NpEpAGQ2ioYBhv3DzWWGBxfwqsGQn4Y-TQYUicB6z2NhOsD0CNaJ-Q1zSCwHRVr5nxid6cmrXPfykBnOOqfYwtyyXWYXT0GWKZ_818Lk50lATw99-uWIV6GHoXFygnLVHwMP1w2&t=636034226650184190) repeat-x;
            }

            .ajax__tab_xp .ajax__tab_header .ajax__tab_hover .ajax__tab_outer {
                cursor: pointer;
                background: url(WebResource.axd?d=KW6ehzvK8opOavBTqNDK_HPFwGP6kd-jMiBznIFcBPJwS9GSkd7hI8WN6PtCbU0NqlD_PyM-Xgz8aKHOelwBKthLr2mhkLnh9DxRnfF7Vy4KfjI4do2xXAaEnvJ7xcsUK8yI5Q2&t=636034226650184190) no-repeat right;
            }

            .ajax__tab_xp .ajax__tab_header .ajax__tab_hover .ajax__tab_inner {
                cursor: pointer;
                background: url(WebResource.axd?d=jldwXxybuOaVRTqjvXY1DFR37w2GCpYDctR8vIVsNYHBE0CO4EQjeAzm3ktb-bjjxbQT_29z4tlja29fLENdw1NQ1jD8yW2HUznWlScVe3o74yctEkKByZi18NRMAjNMdiMx4w2&t=636034226650184190) no-repeat;
            }

            .ajax__tab_xp .ajax__tab_header .ajax__tab_hover .ajax__tab_tab {
                cursor: pointer;
                background: url(WebResource.axd?d=LQIlfVzZmkmBRt77398bv8fyKKIYLXlZIyBGl6e4tjQYSB7fk8UCP2vfFJ8mOfPnOReL7EA6Bgew6yHoExH9nkhTwd9vRl7cJNX05dgam1tiuWOl5mQnfx6uS7-w3bcH5owOTQ2&t=636034226650184190) repeat-x;
            }
            /*.ajax__tab_xp .ajax__tab_header .ajax__tab_active { margin-top: 1px; } */
            .ajax__tab_xp .ajax__tab_header .ajax__tab_active .ajax__tab_outer {
                background: url(WebResource.axd?d=haLu67iaZHSVVz1hnq7Fg_3SsyFaJaEnhVPedXr6i-q4G2f0uG0f6wXx-2mhw9IV-U3tfHDPkUctdWDFymBsabkTjP_MM4zrVviad8j3u5vjyQK9Wj6GHT7ZKjy7nW-21rP0Bg2&t=636034226650184190) no-repeat right;
            }

            .ajax__tab_xp .ajax__tab_header .ajax__tab_active .ajax__tab_inner {
                background: url(WebResource.axd?d=U-zJwyQ-ab40K3j5DOBAhgs-MSzvNlcu95vc0s06uRlJBSMP2g54ZwP1RfM1wT8-UoIQ0OavSWhiPmWxtAXDnUamGKZwW0Pohg5NWbJ2TieSlFH4O2k7p542svkOXUYosastnw2&t=636034226650184190) no-repeat;
            }

            .ajax__tab_xp .ajax__tab_header .ajax__tab_active .ajax__tab_tab {
                background: url(WebResource.axd?d=Wa0d9k_OZ32u30VBLwlMv9aDnWQypRK0n4o5Qx5W0CmM0CuxZF5haBtseh6AUSTfqs5ijLo9otA8hG6WJPnQaNA-b-lznXYvdcNJqk5CesHS2viQ7y0lTMUp7zcWMXRSIt2Jyw2&t=636034226650184190) repeat-x;
            }

        .ajax__tab_xp .ajax__tab_body {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            border: 1px solid #999999;
            border-top: 0;
            padding: 8px;
            background-color: #ffffff;
        }

        /* xp theme vertical left */
        .ajax__tab_xp .ajax__tab_header_verticalleft {
            font-family: verdana,tahoma,helvetica;
            font-size: 11px;
            background: url(WebResource.axd?d=1O2RDXzusH5Fio72iwpRkmrYHT88DN_G2er6wemBmNXNDN7aPgMpQdQifVV0cWo7EyimMcO3Gl2SWcRZ5SFuI8Oh51KG-RemvZGttmOScR3OrUIwekw7nz3mTi4_GHj7KlSkXQ2&t=636034226650184190) repeat-y right;
        }

            .ajax__tab_xp .ajax__tab_header_verticalleft .ajax__tab_outer {
                padding-right: 4px;
                background: url(WebResource.axd?d=5aRvA_6pIs7oySgEmrD0SC8DBB_W0XkGWOnYabZbmJsE3LIAsPG8Ro0WX_lLWxOmnUMbRTgEuk_y61lwn1kzvp9cAuqXcTeCXRSRG7mr78F3sb7BZw_wm4YTcu4cp3WNlhqKknHaP56x2dB8S8YlkAGeBE01&t=636034226650184190) no-repeat right;
                height: 21px;
            }

            .ajax__tab_xp .ajax__tab_header_verticalleft .ajax__tab_inner {
                padding-left: 3px;
                background: url(WebResource.axd?d=wqHSvXsuLkv5VNLIAdBGlUCww1b95oNxaN8RrwlLP1usmmH3NDByBhjeY-n5u2nq-5DjZ9LZw6M4911xFiPqtsUMQlYKH6uE25LvKcfEaO5nfHBb12A06q_U3RgIxfs_457pMDEGsLZQunzqSacc1yre3Eo1&t=636034226650184190) no-repeat;
            }

            .ajax__tab_xp .ajax__tab_header_verticalleft .ajax__tab_tab {
                height: 13px;
                padding: 4px;
                margin: 0px;
                background: url(WebResource.axd?d=dBzm-MdjT_np8OIR_cL_ZYF67imEgmhKm8QQfzv_3rBhWSX2S-y8j_Xpahi5Q1cy7NqhIhmTMQZhiROBVhfM_P138B_Qrpc8xHRrChFLv5N4yThpgiMcGSxNw4mh6KBhjlKlUg2&t=636034226650184190) repeat-x;
            }

            .ajax__tab_xp .ajax__tab_header_verticalleft .ajax__tab_hover .ajax__tab_outer {
                cursor: pointer;
                background: url(WebResource.axd?d=dRXdnRO2C9wW8EdHqNlCM4dnnxzHIvP9vPhRB375wS_QoJINGiXfefi2J87J4fFT1I1lQ7QgvxzjPTDbK_zqiWFRYA90jUhtxqBfcmiw0J0SQgfPdxEIAQJXtU_CjTvW9hO3qV0MbXwLMkFn748YXmP2uqI1&t=636034226650184190) no-repeat right;
            }

            .ajax__tab_xp .ajax__tab_header_verticalleft .ajax__tab_hover .ajax__tab_inner {
                cursor: pointer;
                background: url(WebResource.axd?d=iBIDKHLxigoUmDh7o-TfvtgbAkIqF_iY-BOBrPVexvWUpMGD19iOdeGqJSlp0KA5HVz0z7roLAvjvH6LGPyUkfkIcGhw7Bn-N--F1DTi29o3imD6E3HrlNba_mTa_QWRWTmTHaxJako2frZ2qsF5sRhxL_Y1&t=636034226650184190) no-repeat;
            }

            .ajax__tab_xp .ajax__tab_header_verticalleft .ajax__tab_hover .ajax__tab_tab {
                cursor: pointer;
                background: url(WebResource.axd?d=fyCbiOhoMZjJ7H1jGanDP-v3BQ96Faf7yVBzhFDOy3cwFLWPww8_KW7vA4eEbLZ7wEOi3rKfpTYvxqjc0WCuBPMhto-BlhV-sjV1Nmrv6JKHd2MbmZ5f4XvJGsfZW6H5KTtHCm4zLNZwE_i6FcQMPSAdEuY1&t=636034226650184190) repeat-x;
            }

            .ajax__tab_xp .ajax__tab_header_verticalleft .ajax__tab_active {
                margin-top: 1px;
            }

                .ajax__tab_xp .ajax__tab_header_verticalleft .ajax__tab_active .ajax__tab_outer {
                    background: url(WebResource.axd?d=Q88D-59SemLXLBJX76wRWecH1YxgJKCFeu-r5J13AW5ajXfmYJa7zmoyWxKYsgkiFQQEPuI5gtzdTlx9TrDPCVqgv1li_GhEQBbriTJp39lVdW-rQ8GGrxw4SKCsJHq2fHCUkl1II0gJpQ5oI-GqLVNGyWk1&t=636034226650184190) no-repeat right;
                }

                .ajax__tab_xp .ajax__tab_header_verticalleft .ajax__tab_active .ajax__tab_inner {
                    background: url(WebResource.axd?d=OzfVOxSF2miLMDsqDY9xDhciUnr0VCE5zCEGZb0ZhpTzWChOmr6Y5u8fTIa0AdVSD566AMnBBTK46uCrn21M3TnhvskkV5_fOcx5YwTGNvnSONe1e3AmjRpQbBTI5cgPPaey6HtDCsg92w8pt5WybRfrbtc1&t=636034226650184190) no-repeat;
                }

                .ajax__tab_xp .ajax__tab_header_verticalleft .ajax__tab_active .ajax__tab_tab {
                    background: url(WebResource.axd?d=c9pUrDb4sT3kTadkGw1Mq4vreHU-W_-9h0Bda-jNP0M_F8SjeBquI2OL-nVsciXKrVwjxVD_6DmsTdBUMOgj7GNCFvaGGpXTBCuWtTvfERDo8iUOjPhcZ1HvF5Sf4QywkiMNPqvJaHkPUqp6-9h_wyelZXE1&t=636034226650184190) repeat-x;
                }

        .ajax__tab_xp .ajax__tab_body_verticalleft {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            border: 1px solid #999999;
            border-left: 0;
            padding: 8px;
            background-color: #ffffff;
        }

        /* xp theme vertical right */
        .ajax__tab_xp .ajax__tab_header_verticalright {
            font-family: verdana,tahoma,helvetica;
            font-size: 11px;
            background: url(WebResource.axd?d=1O2RDXzusH5Fio72iwpRkmrYHT88DN_G2er6wemBmNXNDN7aPgMpQdQifVV0cWo7EyimMcO3Gl2SWcRZ5SFuI8Oh51KG-RemvZGttmOScR3OrUIwekw7nz3mTi4_GHj7KlSkXQ2&t=636034226650184190) repeat-y left;
        }

            .ajax__tab_xp .ajax__tab_header_verticalright .ajax__tab_outer {
                padding-right: 4px;
                background: url(WebResource.axd?d=w68UsRrXHyrbfq7NEjsW1V_J7LcZRu3awpUJOQvfnb4YyJmAbrUG2zgP8Irz1dz696Z6JpW_V9FBcfXW9MdxB1mpToxGfoyrNEcH-xLLwM0ZAHJkNSN3fx7gLI744WB5w88ifz4igwLO1QbfMEXgapEZ6t41&t=636034226650184190) no-repeat right;
                height: 21px;
            }

            .ajax__tab_xp .ajax__tab_header_verticalright .ajax__tab_inner {
                padding-left: 3px;
                background: url(WebResource.axd?d=2A9yE7RLEhAO2qyavEWb5BLwBnTyG1GTAN3PT1rHcm0MDmanun3An8iQ7LTT0qHybl-3tGPTRGfF3UAAoQnfbP_SOtNdBeRvq-N4zusc3iavNR1IwYlzIYMekZ3LbFQZmMeKPqZIbXmI7h8xFiwSR-dtH5U1&t=636034226650184190) no-repeat;
            }

            .ajax__tab_xp .ajax__tab_header_verticalright .ajax__tab_tab {
                height: 13px;
                padding: 4px;
                margin: 0px;
                background: url(WebResource.axd?d=bDP2qAFMRW4N5alQj7hqFgKbEykGMQtkAt7UPAcWKYmhSgEOMwbWTOR7UMCzX4g8aby6qedi-qZ6D6erYYqFlXEWaZC5yeucCtCigxtk7o97OK5hV_3a0IFsOqWEC2DLQCNXnQ2&t=636034226650184190) repeat-x;
            }

            .ajax__tab_xp .ajax__tab_header_verticalright .ajax__tab_hover .ajax__tab_outer {
                cursor: pointer;
                background: url(WebResource.axd?d=4ngmz13gGyLzOrdSvxcCMagp8S9_qoVkpZR7vjHGroYuNpLImvk1I1vnmk6xOGEolBsM1w81ckd8OVbs-xNl3dBYmuVPzroQVMgOBfsCOyZ0BLUvavyizvxZRA-PGCbuMS3BBgRjpqMw-UFOqF3gE9KGo5o1&t=636034226650184190) no-repeat right;
            }

            .ajax__tab_xp .ajax__tab_header_verticalright .ajax__tab_hover .ajax__tab_inner {
                cursor: pointer;
                background: url(WebResource.axd?d=iU3GvEwuL_yQpzseQwUmQ_Lgob1Hs0CtA6t32OBDbTb44QbMLu0K0zfBSkPh7ucBFx3_Cve-Lvtfd1sKNaW-GcwvrKQQYAwhdB7sV4ouOACNxGKKyNblETOWS3Rih-UAZ8e6fyIlKKhGywlXP6Fhkn5eoas1&t=636034226650184190) no-repeat;
            }

            .ajax__tab_xp .ajax__tab_header_verticalright .ajax__tab_hover .ajax__tab_tab {
                cursor: pointer;
                background: url(WebResource.axd?d=j1K1-6OQP48lam6S_D-imN3K3o4_wYzvTBZoc3tExyxogSJHmMZ4mYgvEqYDOLLXIDA6m7fJjrjqgJSLLdmJtsO9A0rnGRniW4FtjQvJKeWz6F93bsEzLkLCmnAdNlnfJ8b_fodJo4h373EqVYjyUUecLtQ1&t=636034226650184190) repeat-x;
            }

            .ajax__tab_xp .ajax__tab_header_verticalright .ajax__tab_active {
                margin-top: 1px;
            }

                .ajax__tab_xp .ajax__tab_header_verticalright .ajax__tab_active .ajax__tab_outer {
                    background: url(WebResource.axd?d=6TC9Y7mKrnZLBjJktAch9oaqUGl-wXHlv9VOP_UgnJyp978li3vHpcKD0NeDPYZZH-4Xz_kh8wIm9k1NWss0DgujqSbkctY242niFyTLYbzYNhsLuHgexFz1WKtPaa3H8d2c66v0fMuWa4tZ7TBA8v_C7uI1&t=636034226650184190) no-repeat right;
                }

                .ajax__tab_xp .ajax__tab_header_verticalright .ajax__tab_active .ajax__tab_inner {
                    background: url(WebResource.axd?d=VPdTqfX7ZNbHBiW6O_s27yCsoLh2sklQVMNE54F8_Fy3u5MExd1lAybnDjao3YlQ9-dUAxhNu_V3y8LsuYBYE_UrsS-SG4hR4_UERh4kJzKPH-UtNH9R72Clv8qxPONZvT8y2DEB9nPpuw2ZrR3Kpk5sCcU1&t=636034226650184190) no-repeat;
                }

                .ajax__tab_xp .ajax__tab_header_verticalright .ajax__tab_active .ajax__tab_tab {
                    background: url(WebResource.axd?d=IquSUaY7hexRUzsI8SjrBcLVWie5NQTMutXvzW2dDl1y10apJ8VZWRpbHN86JEsxo32QmX4Zxuqp4fHx6ex66eOCEG6Ig12beQWGdn-cYpTaDLEaaf5EANhCnnWlH-2wHT6Mvh_m3GqRjCknKR3AGa5Pw3k1&t=636034226650184190) repeat-x;
                }

        .ajax__tab_xp .ajax__tab_body_verticalright {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            border: 1px solid #999999;
            border-right: 0;
            padding: 8px;
            background-color: #ffffff;
        }

        /* header on bottom */
        .ajax__tab_xp .ajax__tab_header_bottom {
            font-family: verdana,tahoma,helvetica;
            font-size: 11px;
            background: url(WebResource.axd?d=1O2RDXzusH5Fio72iwpRkmrYHT88DN_G2er6wemBmNXNDN7aPgMpQdQifVV0cWo7EyimMcO3Gl2SWcRZ5SFuI8Oh51KG-RemvZGttmOScR3OrUIwekw7nz3mTi4_GHj7KlSkXQ2&t=636034226650184190) repeat-x top;
        }

            .ajax__tab_xp .ajax__tab_header_bottom .ajax__tab_outer {
                padding-right: 4px;
                background: url(WebResource.axd?d=n5VUlZ0K6MwDJnsQs4EUKmPIIdXA2yULo23XW_mrHhv16JoL8eFNE9NRIESTwVEKyndwuuLa6AJsxJb8uCkNC8cTIeKGGVfyq11_vAN5JOZkramKT1e6bJrhpwrZ0_TG8AzQ9A2&t=636034226650184190) no-repeat right;
                height: 21px;
            }

            .ajax__tab_xp .ajax__tab_header_bottom .ajax__tab_inner {
                padding-left: 3px;
                background: url(WebResource.axd?d=wQlxA1k9UOE97UrIWdMbnIOYAayKUavqOc2R_fnTxh4XyZ-FFhnD6zwRVx7olSaTrYAvAWep_tqwkHkVA_aTBS1H4XIkYFCyR8G6wTfXiWZth7pgs5HnG8MV8RuppDqhmTOz6Q2&t=636034226650184190) no-repeat;
            }

            .ajax__tab_xp .ajax__tab_header_bottom .ajax__tab_tab {
                height: 17px;
                padding: 0px 4px 4px 4px;
                margin: 0px;
                background: url(WebResource.axd?d=5QMJHFwvzG2LMVP-4VTT9EIlepff4mo67kTlb2boogc8uGGfIAnJ9NbiWwgn2oUq9v4-gfLxr8Eqr2zRZaAvDgCEtcoNK1vXIGo9n_hiON7IHjL5DOExPyg6yfPiPG6S8stAHw2&t=636034226650184190) repeat-x;
            }

            .ajax__tab_xp .ajax__tab_header_bottom .ajax__tab_hover .ajax__tab_outer {
                cursor: pointer;
                background: url(WebResource.axd?d=wfQCHDC8_pwjjuqDSd-vau3SsIi9LQLNgsVpwf-bWj7NVIO0IrCM6o50ASZJ8HAMGPJFQ28UaJM2akgRPQaGHmTygbpjtepNghp46nEToIdcHjPoiquDC9K683zuHZ62uLZdLRDtEPFL3VeA_MwUxhVEXYM1&t=636034226650184190) no-repeat right;
            }

            .ajax__tab_xp .ajax__tab_header_bottom .ajax__tab_hover .ajax__tab_inner {
                cursor: pointer;
                background: url(WebResource.axd?d=q9CrutHHIpRYoYU6T5gggciVxW6gnw8tSKZ077ttFFjw_ViEaEpiuL4z0x9Ez16f_Ab9tDR5GjZ9r-npK0dV-_7psquyRjULwXl3deLbe_JILjDXjb7GQipZyl315tv0XVbG-_cT4_eO08P76QvdbmLqd8c1&t=636034226650184190) no-repeat;
            }

            .ajax__tab_xp .ajax__tab_header_bottom .ajax__tab_hover .ajax__tab_tab {
                cursor: pointer;
                background: url(WebResource.axd?d=2QSfncTYU5qYvo4oZdexadxHO2Eaz6Z6chA_j7Lhx4R0GSwWnkP9m-2pbI-UgHmLmzsJ1p_6Yz9J1139DFb3E08PGRVxCc8u_wcN7oiC7Vos5xHHkJPIRv9UJX3-k5qB8cBW_Q2&t=636034226650184190) repeat-x;
            }

            .ajax__tab_xp .ajax__tab_header_bottom .ajax__tab_active .ajax__tab_outer {
                background: url(WebResource.axd?d=uRjeBUWHeiVRGfDAgZL2AaHMQyBCjgLUBadnOiOjjhCjIH-E0No9JA-rI2lbvrmFfh_qXW53-1YHafYcjD07gcCfvSULur2oMjGxFJKYZrJEGawRZ6Fej82ntLF1A9I2BKwlv0nHdBHB5WjOU7p9amQ_hmw1&t=636034226650184190) no-repeat right;
            }

            .ajax__tab_xp .ajax__tab_header_bottom .ajax__tab_active .ajax__tab_inner {
                background: url(WebResource.axd?d=nMerAAYStTCgKH8drNjEuORKDj1pOg4fz_vqU1EhqjX61_VowFFdiz2T1O51HV2Lp90JGDc-CZKgWqqGtvYKcUn3WojTzv7FJScmzOQdHFKqZ1Raba8LDKsJ2shuaciFkKrxf4_F3wgaRJ6oLMZ9bzg6_ao1&t=636034226650184190) no-repeat;
            }

            .ajax__tab_xp .ajax__tab_header_bottom .ajax__tab_active .ajax__tab_tab {
                background: url(WebResource.axd?d=tnX5zlBpNvPJbw8GcOaEvKlCgQW6kAzPrbJxIDy7UMgyg9e3EE02lgDYkmVCzwizUO6aaGXCMoyatpapXqe4JyIaQ-xXjFFf5r2yKPbSHx0j2p4-RzuCus7f-6Z0-YCcGmC5KA2&t=636034226650184190) repeat-x;
            }

        .ajax__tab_xp .ajax__tab_body_bottom {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            border: 1px solid #999999;
            border-bottom: 0;
            padding: 8px;
            background-color: #ffffff;
        }

        /* scrolling */
        .ajax__scroll_horiz {
            overflow-x: scroll;
        }

        .ajax__scroll_vert {
            overflow-y: scroll;
        }

        .ajax__scroll_both {
            overflow: scroll;
        }

        .ajax__scroll_auto {
            overflow: auto;
        }

        .ajax__scroll_none {
            overflow: hidden;
        }

        /* plain theme */
        .ajax__tab_plain .ajax__tab_outer {
            text-align: center;
            vertical-align: middle;
            border: 2px solid #999999;
        }

        .ajax__tab_plain .ajax__tab_inner {
            text-align: center;
            vertical-align: middle;
        }

        .ajax__tab_plain .ajax__tab_body {
            text-align: center;
            vertical-align: middle;
        }

        .ajax__tab_plain .ajax__tab_header {
            text-align: center;
            vertical-align: middle;
        }

        .ajax__tab_plain .ajax__tab_active .ajax__tab_outer {
            background: #FFFFE1;
        }

        .spinnermodal {
            background-color: #FFFFFF;
            height: 100%;
            left: 0;
            opacity: 0.5;
            position: fixed;
            top: 0;
            width: 100%;
            z-index: 100000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="page-cont-top">

            <div class="page-bar-right">
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div id="iframeloading" style="display: none; z-index: 10001;" class="spinnermodal">
                <div style="position: fixed; z-index: 10001; top: 50%; left: 50%; height: 65px">
                    Loading...
                 <%--   <img src="~/Images/loader.gif" alt="Loading..." />--%>
                </div>
            </div>
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <%--<ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Project</asp:Label></li>
                        <li>
                           

                            <asp:HyperLink ID="lnkAdd" runat="server" NavigateUrl="addproject.aspx"
                                ToolTip="Add New" CssClass="icon-addnew" Target="_blank"></asp:HyperLink>
                        </li>
                        <li>
                            <asp:LinkButton CssClass="icon-edit" ToolTip="Edit"
                                ID="lnkEdit" runat="server" CausesValidation="False" OnClick="lnkEdit_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-delete" ToolTip="Delete"
                                ID="lnkDelete" runat="server" OnClientClick="return CheckDelete();" CausesValidation="False"
                                OnClick="lnkDelete_Click"></asp:LinkButton></li>
                        <li>
                            <ul class="nav navbar-nav pull-right">
                                <li class="dropdown dropdown-user">
                                    <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                    <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                        <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear: both;"></div>
                                        </a></li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false"
                                OnClick="lnkClose_Click"></asp:LinkButton></li>
                    </ul>--%>

                    <ul class="lnklist-header">
                        <li>
                            <asp:label cssclass="title_text" id="lblHeader" runat="server">Project</asp:label>
                        </li>
                        <li>
                            <%--<asp:LinkButton CssClass="icon-addnew" ID="lnkAdd" ToolTip="Add New"
                                runat="server" CausesValidation="False" OnClick="lnkAdd_Click"></asp:LinkButton>--%>

                            <asp:hyperlink id="lnkAdd" runat="server" navigateurl="addproject.aspx"
                                tooltip="Add New" cssclass="icon-addnew" target="_blank"></asp:hyperlink>
                        </li>
                        <li>
                            <asp:linkbutton cssclass="icon-edit" tooltip="Edit"
                                id="lnkEdit" runat="server" causesvalidation="False" onclick="lnkEdit_Click" onclientclick="return CheckEdit();"></asp:linkbutton>
                        </li>
                        <li>
                            <asp:linkbutton cssclass="icon-delete" tooltip="Delete"
                                id="lnkDelete" runat="server" onclientclick="return CheckDelete();" causesvalidation="False"
                                onclick="lnkDelete_Click"></asp:linkbutton>
                        </li>
                        <li>
                            <ul class="nav navbar-nav pull-right">
                                <li class="dropdown dropdown-user">
                                    <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                    <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                        <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear: both;"></div>
                                        </a></li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <asp:linkbutton cssclass="icon-closed" id="lnkClose" runat="server" tooltip="Close" causesvalidation="false"
                                onclick="lnkClose_Click"></asp:linkbutton>
                        </li>
                    </ul>


                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <%--<div class="search-customer">
                        <div class="sc-form">

                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    Search for Items where
                                <asp:DropDownList ID="ddlSearch" runat="server"
                                    CssClass="form-control input-sm input-small" ClientIDMode="Static">
                                </asp:DropDownList>
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control input-sm input-small" ClientIDMode="Static" Style="width: 250px !important;" placeholder="Search for Project#,Date,location,Status,Description" ToolTip="Search for Project#,Date,location,Status,Description"></asp:TextBox>
                                    Start Date
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control input-sm input-small date-picker" ClientIDMode="Static"></asp:TextBox>
                                    End Date
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control input-sm input-small date-picker" ClientIDMode="Static"></asp:TextBox>
                                    <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server"><i class="fa fa-search"></i></asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>


                        <ul>
                            <li>
                                <a id="lnkClear">Clear</a>
                            </li>
                            <li>
                                <asp:LinkButton ID="lnkShowAll" runat="server">Show All Items</asp:LinkButton></li>
                            <li>
                                <asp:UpdatePanel ID="updRecordCount" runat="server">
                                    <ContentTemplate>
                                        <span>
                                            <asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic;" ClientIDMode="Static"></asp:Label></span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </li>
                        </ul>
                    </div>--%>

                    <div class="search-customer">
                        <div class="sc-form">
                            <label for="">
                                Select for Projects where
                            </label>

                            <asp:dropdownlist id="ddlSearch" runat="server" cssclass="form-control input-sm input-small" clientidmode="Static">
                                <asp:ListItem Value="">Select</asp:ListItem>
                                <asp:ListItem Value="j.id">Project #</asp:ListItem>
                                <asp:ListItem Value="j.fdate">Date</asp:ListItem>
                                <asp:ListItem Value="j.loc">Location</asp:ListItem>
                                <asp:ListItem Value="j.Status">Status</asp:ListItem>
                                <asp:ListItem Value="j.fdesc">Description</asp:ListItem>
                            </asp:dropdownlist>


                            <asp:dropdownlist id="ddlStatus" runat="server" cssclass="form-control input-sm input-small hide"
                                width="200px" tabindex="13" clientidmode="Static">
                                <asp:ListItem Value="0">Active</asp:ListItem>
                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                            </asp:dropdownlist>
                            <asp:dropdownlist id="ddllocation" runat="server" cssclass="form-control input-sm input-small hide"
                                clientidmode="Static">
                            </asp:dropdownlist>


                            <asp:textbox id="txtInvDt" runat="server" cssclass="form-control input-sm input-small hide" clientidmode="Static"></asp:textbox>
                            <asp:calendarextender id="txtInvDt_CalendarExtender" runat="server" enabled="True"
                                targetcontrolid="txtInvDt">
                            </asp:calendarextender>
                            <asp:textbox id="txtSearch" runat="server" cssclass="form-control input-sm input-small" clientidmode="Static"></asp:textbox>

                            <asp:dropdownlist id="ddlDateRange" runat="server" cssclass="form-control input-sm input-small "
                                style="width: 200px!important;" tabindex="14" clientidmode="Static">
                                <asp:ListItem Value="1">Cumulative</asp:ListItem>
                                <asp:ListItem Value="2">Date Range - Activity</asp:ListItem>
                                <asp:ListItem Value="3">Date Range - Closed</asp:ListItem>
                                <asp:ListItem Value="4">Date Range - Created</asp:ListItem>
                            </asp:dropdownlist>


                            <asp:label id="lblSdate" runat="server" text="Date From" cssclass="hide" clientidmode="Static" />
                            <asp:textbox id="txtfromDate" runat="server" cssclass="form-control input-sm input-small hide" maxlength="50"
                                width="80px" clientidmode="Static"></asp:textbox>
                            <asp:calendarextender id="txtfromDate_CalendarExtender" runat="server" enabled="True"
                                targetcontrolid="txtfromDate">
                            </asp:calendarextender>

                            <asp:label id="lblEdate" runat="server" text="Date To" cssclass="hide" />
                            <asp:textbox id="txtToDate" runat="server" cssclass="form-control input-sm input-small hide" maxlength="50"
                                width="80px" clientidmode="Static"></asp:textbox>
                            <asp:calendarextender id="txtToDate_CalendarExtender" runat="server" enabled="True"
                                targetcontrolid="txtToDate">
                            </asp:calendarextender>

                            <asp:label id="Label1" runat="server" text="Date From" cssclass="hide" />
                            <asp:textbox id="TextBox1" runat="server" cssclass="form-control input-sm input-small hide" maxlength="50"
                                width="80px"></asp:textbox>
                            <asp:calendarextender id="CalendarExtender1" runat="server" enabled="True"
                                targetcontrolid="txtfromDate">
                            </asp:calendarextender>

                            <asp:label id="Label2" runat="server" text="Date To" cssclass="hide" />
                            <asp:textbox id="TextBox2" runat="server" cssclass="form-control input-sm input-small hide" maxlength="50"
                                width="80px"></asp:textbox>
                            <asp:calendarextender id="CalendarExtender2" runat="server" enabled="True"
                                targetcontrolid="txtToDate">
                            </asp:calendarextender>
                            <%--<asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click"
                                ToolTip="Search" CssClass="btn submit"><i class="fa fa-search"></i></asp:LinkButton>--%>
                            <asp:linkbutton id="lnkSearch" cssclass="btn submit" runat="server"><i class="fa fa-search"></i></asp:linkbutton>

                        </div>
                        <ul style="float: left; padding-bottom: 10px;">
                            <%--<li>
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All Projects</asp:LinkButton>
                            </li>
                            <li>
                                <asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic;"></asp:Label>
                            </li>--%>

                            <li>
                                <a id="lnkClear">Clear</a>
                            </li>
                            <li>
                                <asp:linkbutton id="lnkShowAll" runat="server">Show All Projects</asp:linkbutton>
                            </li>
                            <li>
                                <asp:updatepanel id="updRecordCount" runat="server">
                                    <ContentTemplate>
                                        <span>
                                            <asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic;" ClientIDMode="Static"></asp:Label></span>
                                    </ContentTemplate>
                                </asp:updatepanel>
                            </li>

                        </ul>
                    </div>
                    <div class="clearfix"></div>
                    <div class="table-scrollable" style="border: none">
                        <div class="ajax__tab_xp ajax__tab_container ajax__tab_default" style="visibility: visible;">
                            <asp:repeater id="rptDepartment" runat="server" clientidmode="Static">
                                <HeaderTemplate>
                                    <div class="ajax__tab_header" style="visibility: visible;">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span id='spnid<%# Eval("ID") %>' class=""><%--ajax__tab_active--%>
                                        <span class="ajax__tab_outer">
                                            <span class="ajax__tab_inner">
                                                <a class="ajax__tab_tab" style="text-decoration: none;" id='<%# Eval("ID") %>'>
                                                    <span><%# Eval("type") %></span>

                                                </a>
                                            </span>

                                        </span>

                                    </span>
                                </ItemTemplate>
                                <FooterTemplate>
                        </div>
                        </FooterTemplate>

                            </asp:Repeater>

                            <div id="dvProjecDetails" class="ajax__tab_panel">
                            </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- edit-tab end -->
    <div class="clearfix"></div>
    </div>
    <!-- END DASHBOARD STATS -->
    <div class="clearfix"></div>

</asp:Content>

