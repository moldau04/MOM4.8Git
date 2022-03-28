using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for NumericTextbox
/// </summary>
/// 

namespace CustomControls
{
    public class HTML5_NumericTextbox : TextBox
    {
        public HTML5_NumericTextbox()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "Number");            
            base.Render(writer);
        }        
    }
}
