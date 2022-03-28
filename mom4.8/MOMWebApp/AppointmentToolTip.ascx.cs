using System; 
using Telerik.Web.UI;

public partial class AppointmentToolTip : System.Web.UI.UserControl
{

    private Appointment apt;

    public Appointment TargetAppointment
    {
        get
        {
            return apt;
        }

        set
        {
            apt = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    } 

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);   

        if (apt != null)
        {
            if (apt.ID.ToString() != "")
            {
                lblSubject.Text = apt.Subject;
                Timerange.Text = apt.Start.ToString() + "-" + apt.End.ToString(); 
                Category.Text = apt.Attributes["cat"].ToString();
                Status.Text = apt.Attributes["assignname"].ToString();
                Address.Text = apt.Attributes["address"].ToString();
                Phone.Text = apt.Attributes["phone"].ToString();
                Date.Text = apt.Attributes["edate"].ToString();
                Reason.Text = apt.Attributes["fdesc"].ToString();
                Resolution.Text = apt.Attributes["descres"].ToString(); 
                #region Ticket Icon
                //Icon doc


                string DocumentCount = apt.Attributes["DocumentCount"].ToString();
                if (DocumentCount != "0")
                {
                    ImgDocument.ImageUrl = "images/Document.png";
                    ImgDocument.Visible = true;
                    ImgDocument.ToolTip = "Documents";
                }
                else
                {
                    ImgDocument.Visible = false;
                }

                    // Icon Alert

                    string dispalert = apt.Attributes["dispalert"].ToString();
                if (dispalert == "1")
                {
                    imgalert.ImageUrl = "images/alert.png";
                    imgalert.Visible = true;
                    imgalert.ToolTip = "Dispatch Alert";
                }
                else {

                    imgalert.Visible = false;
                }

                    // Icon credithold
                    string credithold = apt.Attributes["credithold"].ToString();

                if (credithold == "1")
                {
                    imgcredithold.ImageUrl = "images/credithold.png";
                    imgcredithold.Visible = true;
                    imgcredithold.ToolTip = "Credit Hold";
                }
                else
                {
                    imgcredithold.Visible = false;
                }

                    // Icon MS
                    string comp = apt.Attributes["comp"].ToString();

                if (comp == "2")
                {
                    imgMS.ImageUrl = "images/1331034893_pda.png";
                    imgMS.Visible = true;
                    imgMS.ToolTip = "MS";
                }
                else
                {
                    imgMS.Visible = false;
                }


                    // icon Confirmed

                    string Confirmed = apt.Attributes["Confirmed"].ToString();

                if (Confirmed == "1")
                {
                    imgConfirmed.ImageUrl = "images/1331036429_Check.png";
                    imgConfirmed.Visible = true;
                    imgConfirmed.ToolTip = "Confirmed";
                }
                else
                {
                    imgConfirmed.Visible = false;
                }


                    string clearcheck = apt.Attributes["clearcheck"].ToString();

                if (clearcheck == "1")
                {
                    imgreview.ImageUrl = "images/review.png";
                    imgreview.Visible = true;
                    imgreview.ToolTip = "Reviewed";
                }
                else
                {
                    imgreview.Visible = false;
                }


                    //Img Signature

                    string SignatureIMG = apt.Attributes["SignatureIMG"].ToString();
                if (SignatureIMG != string.Empty && SignatureIMG.Length > 0)
                {
                    ImgSignature.ImageUrl = SignatureIMG;
                    ImgSignature.Visible = true;
                    ImgSignature.ToolTip = "Signature";
                }
                else
                {
                    ImgSignature.Visible = false;
                }

                    // Img  Chargeable

                    string charge = apt.Attributes["charge"].ToString();

                string invoice = apt.Attributes["invoice"].ToString();

                string manualinvoice = apt.Attributes["manualinvoice"].ToString();

                string qbinvoiceid = apt.Attributes["qbinvoiceid"].ToString();
                
                string Chargeableurl = ChargeableImage(charge, invoice, manualinvoice, qbinvoiceid);
                if (Chargeableurl != string.Empty && Chargeableurl.Length > 0)
                {
                    ImgChargeable.ImageUrl = Chargeableurl;
                    ImgChargeable.Visible = true;
                    ImgChargeable.ToolTip = "Chargeable";
                }
                else {
                    ImgChargeable.Visible = false;
                }

            }

            #endregion
        }
    }
      
    public string ChargeableImage(string charge, string invoice, string manualinvoice, string QBinvoiceid)
    {
        string img = string.Empty;
        if (charge == "1")
        {
            img = "images/dollarRed.png";
        }
        if (charge == "0" && invoice != "0" && invoice.Trim() != "")
        {
            img = "images/dollar.png";
        }
        if (charge == "0" && manualinvoice.Trim() != "")
        {
            img = "images/dollar.png";
        }
        if (QBinvoiceid != "")
        {
            img = "images/dollarblue.png";
        }
        return img;
    }
}