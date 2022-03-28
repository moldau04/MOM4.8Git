<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="uc_CustomerSearch" Codebehind="uc_CustomerSearch.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<script type="text/javascript">

    $(document).ready(function () {

        ///////////// Ajax call for customer auto search ////////////////////                
        var query = "";
        function dtaa() {
            this.prefixText = null;
            this.con = null;
            this.custID = null;
        }
        $("#<%= txtCustomer.ClientID %>").autocomplete({
            source: function (request, response) {

                var dtaaa = new dtaa();
                dtaaa.prefixText = request.term;
                query = request.term;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "CustomerAuto.asmx/GetCustomer",
                    data: JSON.stringify(dtaaa),
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        response($.parseJSON(data.d));
                    },
                    error: function (result) {
                        alert("Due to unexpected errors we were unable to load customers");
                    }
                });
            },
            select: function (event, ui) {
                $("#<%= txtCustomer.ClientID %>").val(ui.item.label);
                $("#<%= hdnCustID.ClientID %>").val(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                $("#<%= txtCustomer.ClientID %>").val(ui.item.label);
                return false;
            },
            minLength: 0,
            delay: 250
        })._renderItem = function (ul, item) {
            debugger;
            var result_item = item.label;
            var result_desc = item.desc;
            var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
            result_item = result_item.replace(x, function (FullMatch, n) {
                return '<span class="highlight">' + FullMatch + '</span>'
            });
            if (result_desc != null) {
                result_desc = result_desc.replace(x, function (FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });
            }
            return $("<li></li>")
            .data("item.autocomplete", item)
            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
            .appendTo(ul);
        };


        $("#<%= txtCustomer.ClientID %>").keyup(function (e) {
            var hdnId = document.getElementById('<%= hdnCustID.ClientID %>');
            if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                hdnId.value = '';
            }
            if (e.value == '') {
                hdnId.value = '';
            }
        });


    });

    function ChkCust(sender, args) {
        var CustName = document.getElementById('<%=txtCustomer.ClientID%>');
        var hdnId = document.getElementById('<%=hdnCustID.ClientID%>');
        if (CustName != '') {
            if (hdnId.value == '') {
                args.IsValid = false;
            }
        }
    }


</script>

<asp:HiddenField ID="hdnCustID" runat="server" />
<div class="input-field col s12">
    <div class="row">
        <asp:Label ID="lblCstmr" runat="server" AssociatedControlID="txtCustomer">Customer Name</asp:Label>
        <asp:TextBox ID="txtCustomer" runat="server" autocomplete="off" CssClass="register_input_bg_address searchinput"
            TabIndex="3" ToolTip="Customer Name "></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
            Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtCustomer">
        </asp:FilteredTextBoxExtender>
        <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ChkCust"
            ControlToValidate="txtCustomer" Display="None" ErrorMessage="Please select the customer"
            SetFocusOnError="True"></asp:CustomValidator>
        <asp:ValidatorCalloutExtender ID="CustomValidator2_ValidatorCalloutExtender" runat="server"
            Enabled="True" PopupPosition="TopLeft" TargetControlID="CustomValidator2">
        </asp:ValidatorCalloutExtender>
    </div>
</div>
