using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public class DynamicTemplateField : ITemplate
{
    string strTextBoxName;
    string strGridName;
    string strGridName2;
    public DynamicTemplateField(string name, string GridName, string GridName2)
    {
        this.strTextBoxName = name;
        this.strGridName = GridName;
        this.strGridName2 = GridName2;
    }

    public void InstantiateIn(Control container)
    {
        TextBox txt1 = new TextBox();
        txt1.ID = "txt" + strTextBoxName;
        txt1.Width = 50;
        txt1.Text = "0";
        txt1.Attributes.Add("onkeydown", "NumericValid(event);");
        txt1.Attributes.Add("onblur", "CalculatePercentage('" + strGridName + "'); CalculatePercentage('" + strGridName2 + "');");
        txt1.DataBinding += new EventHandler(txt1_DataBinding);
        container.Controls.Add(txt1);
    }
    private void txt1_DataBinding(object sender, EventArgs e)
    {
        TextBox target = (TextBox)sender;
        GridViewRow container = (GridViewRow)target.NamingContainer;
        target.Text = ((DataRowView)container.DataItem)[strTextBoxName].ToString();
    }
}
public class DynamicTemplateFieldFooter : ITemplate
{
    string strTextBoxName;
    string strvalue;
    int ID;
    public DynamicTemplateFieldFooter(string name, string value, int ID)
    {
        this.strTextBoxName = name;
        this.strvalue = value;
        this.ID = ID;
    }

    public void InstantiateIn(Control container)
    {
        TextBox txt1 = new TextBox();
        txt1.ID = "txt" + strTextBoxName + "T";
        txt1.Width = 50;
        txt1.CssClass = "texttransparent";
        txt1.Attributes.Add("onfocus", "this.blur();");
        container.Controls.Add(txt1);

        HiddenField hdn1 = new HiddenField();
        hdn1.ID = "hdn" + strTextBoxName + "T";
        hdn1.Value = strvalue;
        container.Controls.Add(hdn1);

        HiddenField hdn2 = new HiddenField();
        hdn2.ID = "hdnID" + strTextBoxName + "ID";
        hdn2.Value = ID.ToString();
        container.Controls.Add(hdn2);
    }
}

public class DynamicTemplateFieldTotal : ITemplate
{
    string strTextBoxName;
    public DynamicTemplateFieldTotal(string name)
    {
        this.strTextBoxName = name;
    }

    public void InstantiateIn(Control container)
    {
        TextBox txt1 = new TextBox();
        txt1.ID = "txt" + strTextBoxName;
        txt1.Width = 50;
        txt1.Text = "0";
        txt1.CssClass = "texttransparent";
        txt1.Attributes.Add("onfocus", "this.blur();");
        container.Controls.Add(txt1);
    }
   
}