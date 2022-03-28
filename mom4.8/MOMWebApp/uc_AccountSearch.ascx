<%@ Control Language="C#" AutoEventWireup="true" Inherits="uc_AccountSearch" Codebehind="uc_AccountSearch.ascx.cs" %>
<style>
    .ui-autocomplete {
        max-height: 300px;
        overflow-y: auto; /* prevent horizontal scrollbar */
        overflow-x: hidden; /* add padding to account for vertical scrollbar */
        z-index: 1000 !important;
    }
</style>

<script type="text/javascript">
    //function pageLoad(sender, args) {

        $(document).ready(function () {

            //if (args.get_isPartialLoad()) {
            var query = "";
            function a_dta() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }
            $("#<%=txtGLAcct.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new a_dta();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAccountName",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load accounts");
                        }
                    });
                },
               select: function (event, ui) {
                    $("#<%=txtGLAcct.ClientID%>").val(ui.item.label);
                    $("#<%=hdnAcctID.ClientID%>").val(ui.item.value);

                    //Change Unrecognized Revenue on BILLING Tab base on Billing Code on FINANCE Tab
                    $('#ctl00_ContentPlaceHolder1_txtUnrecognizedExpense').val(ui.item.label);
                    $('#ctl00_ContentPlaceHolder1_hdnUnrecognizedExpense').val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtGLAcct.ClientID%>").val(ui.item.label);

                    //Change Unrecognized Revenue on BILLING Tab base on Billing Code on FINANCE Tab
                    $('#ctl00_ContentPlaceHolder1_txtUnrecognizedExpense').val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
           .bind('click', function () { $(this).autocomplete("search"); })
           .data("ui-autocomplete")._renderItem = function (ul, item) {
               //debugger;
               var ula = ul;
               var itema = item;
               var result_value = item.value;
               var result_item = item.acct;
               var result_desc = item.label;

               var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
               result_item = result_item.replace(x, function (FullMatch, n) {
                   return '<span class="highlight">' + FullMatch + '</span>'
               });
               if (result_desc != null) {
                   result_desc = result_desc.replace(x, function (FullMatch, n) {
                       return '<span class="highlight">' + FullMatch + '</span>'
                   });
               }

               if (result_value == 0) {
                   //return $("<li></li>")
                   //.data("item.autocomplete", item)
                   //.append("<a>" + result_item + "</a>")
                   //.appendTo(ul);
               }
               else {
                   return $("<li></li>")
                   .data("item.autocomplete", item)
                   .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                   .appendTo(ul);
               }

           }
        });


    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        var query = "";
        function a_dta() {
            this.prefixText = null;
            this.con = null;
            this.custID = null;
        }
        $("#<%=txtGLAcct.ClientID%>").autocomplete({
            source: function (request, response) {
                var dtaaa = new a_dta();
                dtaaa.prefixText = request.term;
                query = request.term;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "AccountAutoFill.asmx/GetAccountName",
                    data: JSON.stringify(dtaaa),
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        response($.parseJSON(data.d));
                    },
                    error: function (result) {
                        alert("Due to unexpected errors we were unable to load accounts");
                    }
                });
            },
            select: function (event, ui) {
                $("#<%=txtGLAcct.ClientID%>").val(ui.item.label);
                $("#<%=hdnAcctID.ClientID%>").val(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                $("#<%=txtGLAcct.ClientID%>").val(ui.item.label);
                return false;
            },
            minLength: 0,
            delay: 250
        })
           .bind('click', function () { $(this).autocomplete("search"); })
           .data("ui-autocomplete")._renderItem = function (ul, item) {
               //debugger;
               var ula = ul;
               var itema = item;
               var result_value = item.value;
               var result_item = item.acct;
               var result_desc = item.label;

               var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
               result_item = result_item.replace(x, function (FullMatch, n) {
                   return '<span class="highlight">' + FullMatch + '</span>'
               });
               if (result_desc != null) {
                   result_desc = result_desc.replace(x, function (FullMatch, n) {
                       return '<span class="highlight">' + FullMatch + '</span>'
                   });
               }

               if (result_value == 0) {
                   //return $("<li></li>")
                   //.data("item.autocomplete", item)
                   //.append("<a>" + result_item + "</a>")
                   //.appendTo(ul);
               }
               else {
                   return $("<li></li>")
                   .data("item.autocomplete", item)
                   .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                   .appendTo(ul);
               }

           }
    });
    //}
</script>
<asp:TextBox ID="txtGLAcct" Placeholder="Search by acct# and name" runat="server"></asp:TextBox>
<asp:HiddenField ID="hdnAcctID" runat="server" />
