<%@ Control Language="C#" AutoEventWireup="true" Inherits="uc_gvChecklist" Codebehind="uc_gvChecklist.ascx.cs" %>

<asp:GridView ID="gvDepartment" runat="server" AutoGenerateColumns="False" PageSize="20" ShowFooter="true" 
    CssClass="table table-bordered table-striped table-condensed flip-content" OnRowCommand="gvDepartment_RowCommand">
    <AlternatingRowStyle CssClass="oddrowcolor" />
    <FooterStyle CssClass="footer" />
    <RowStyle CssClass="evenrowcolor" />
    <SelectedRowStyle CssClass="selectedrowcolor" />
    <Columns>
        <asp:TemplateField HeaderText="Line" HeaderStyle-Width="4%">
            <ItemTemplate>
                <asp:Label ID="lblIndex" runat="server" Text='<%# Eval("line") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:ImageButton ID="ibtnDeleteCItem" OnClientClick="return confirm('Are you sure you want to delete the items? This will delete the items from Equipments using the template too.')"
                    CausesValidation="false" ToolTip="Delete" ImageUrl="images/menu_delete.png" runat="server"/>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" />
            </ItemTemplate>
            <FooterTemplate>
                <asp:ImageButton ID="ibtnUpArr" CausesValidation="false" ToolTip="Up Arrow" ImageUrl="~/images/uparr.png" runat="server"
                        CommandName="UpArr"/>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Description">
            <ItemTemplate>
                <asp:TextBox ID="txtDescription" runat="server" MaxLength="100" Text='<%# Eval("fdesc") %>'
                    CssClass="form-control input-sm input-small" style="width:110px!important;"></asp:TextBox>
            </ItemTemplate>
            <FooterTemplate>
                <asp:ImageButton ID="ibtnDownArr" CausesValidation="false" ToolTip="Down Arrow" ImageUrl="~/images/downarr.png" runat="server"
                    CommandName="DownArr"/>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect1" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date">
            <ItemTemplate>
                <asp:DropDownList ID="ddlControl" runat="server" CssClass="form-control input-sm input-small" style="width:90px!important;"
                    SelectedValue='<%# Eval("Format") == DBNull.Value ? "" : Eval("Format") %>'>
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="0">Short Date</asp:ListItem>
                        <asp:ListItem Value="1">Text</asp:ListItem>
                        <asp:ListItem Value="2">Dropdown</asp:ListItem>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Reference">
            <ItemTemplate>
                <asp:DropDownList ID="ddlRefControl" runat="server" CssClass="form-control input-sm input-small" style="width:90px!important;"
                    SelectedValue='<%# Eval("RefFormat") == DBNull.Value ? "" : Eval("RefFormat") %>'>
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="0">Currency</asp:ListItem>
                        <asp:ListItem Value="1">Text</asp:ListItem>
                        <asp:ListItem Value="2">Dropdown</asp:ListItem>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>