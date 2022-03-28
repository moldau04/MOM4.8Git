<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="uc_LocationSearch" Codebehind="uc_LocationSearch.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script type="text/javascript">

    $(document).ready(function() {


        ///////////// Ajax call for location auto search ////////////////////
        var queryloc = "";
        function dtaa() {
            this.prefixText = null;
            this.con = '';
            this.custID = null;
        }
        $("#<%=txtLocation.ClientID%>").autocomplete(
                {
                    source: function(request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        queryloc = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetLocation",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function(data) {
                                response($.parseJSON(data.d));
                            },
                            error: function(result) {
                                alert("Due to unexpected errors we were unable to load customers");
                            }
                        });
                    },
                    select: function(event, ui) {
                        $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                        $("#<%=hdnLocId.ClientID%>").val(ui.item.value);
                        return false;
                    },
                    focus: function(event, ui) {
                        $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
            .data("autocomplete")._renderItem = function(ul, item) {
                var result_item = item.label;
                var result_desc = item.desc;
                var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...            
                result_item = result_item.replace(x, function(FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });
                if (result_desc != null) {
                    result_desc = result_desc.replace(x, function(FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                }
                return $("<li></li>")
		        .data("item.autocomplete", item)
		        .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
		        .appendTo(ul);
            };

        $("#<%=txtLocation.ClientID%>").keyup(function(e) {
            var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
            if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                hdnLocId.value = '';
            }
            if (e.value == '') {
                hdnLocId.value = '';
            }
        });

    });

    function ChkLocation(sender, args) {
        var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
        if (hdnLocId.value == '') {
            args.IsValid = false;
        }
    }

</script>

<asp:HiddenField ID="hdnLocId" runat="server"  />
<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:TextBox ID="txtLocation" runat="server" autocomplete="off" CssClass="form-control searchinputloc"
                placeholder="Search by location name, phone#, address etc." TabIndex="4" ToolTip="Location Name "></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                Enabled="false" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
            </asp:FilteredTextBoxExtender>
            <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ChkLocation"
                ControlToValidate="txtLocation" Display="None" ErrorMessage="Please select the location"
                SetFocusOnError="True">
            </asp:CustomValidator>
            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                PopupPosition="Right" TargetControlID="CustomValidator2">
            </asp:ValidatorCalloutExtender>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLocation"
                Display="None" ErrorMessage="Location Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
            </asp:ValidatorCalloutExtender>
        </td>
    </tr>
</table>
