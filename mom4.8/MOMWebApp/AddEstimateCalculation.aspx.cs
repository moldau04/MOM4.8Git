using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;

public partial class AddEstimateCalculation : System.Web.UI.Page
{
    #region::Declaration::
    EstimateCalculation objProp_estcal = new EstimateCalculation();
    BL_EstimateCalculation _blEstimateCalculation = new BL_EstimateCalculation();
    BL_Job objBL_Job = new BL_Job();
    JobT _objJob = new JobT();
    #endregion
    #region ::Page Events::
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["config"] == null)
                return;

            BindData();
        }
    }
    #endregion
    #region ::Methods::
    private void BindData()
    {
        _blEstimateCalculation = new BL_EstimateCalculation();
        objProp_estcal.ConnConfig = WebBaseUtility.ConnectionString;

        #region Calcution type
        ddlCalculationType.Items.Add(new ListItem("Select Type", "0"));
        DataSet dscalculationtype = _blEstimateCalculation.GetAllCalculationType(objProp_estcal);
        if (dscalculationtype != null)
        {
            if (dscalculationtype.Tables.Count > 0)
            {
                if (dscalculationtype.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dscalculationtype.Tables[0].Rows.Count; i++)
                    {
                        ddlCalculationType.Items.Add(new ListItem(Convert.ToString(dscalculationtype.Tables[0].Rows[i]["EstimateCalculationName"]), Convert.ToString(dscalculationtype.Tables[0].Rows[i]["ID"])));
                    }
                }
            }
        }
        #endregion

        #region Bom Type
        ddlBomItems.Items.Add(new ListItem("Select BOM Type", "0"));
        DataSet dsBomType = new DataSet();
        _objJob.ConnConfig = Session["config"].ToString();
        dsBomType = objBL_Job.GetBomTypeForEstimateCalculation(_objJob);

        if (dsBomType != null)
        {
            if (dsBomType.Tables.Count > 0)
            {
                if (dsBomType.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsBomType.Tables[0].Rows.Count; i++)
                    {
                        ddlBomItems.Items.Add(new ListItem(Convert.ToString(dsBomType.Tables[0].Rows[i]["Type"]), Convert.ToString(dsBomType.Tables[0].Rows[i]["ID"])));
                    }
                }
            }
        }
        #endregion

        #region Tax
        //ddltax.Items.Add(new ListItem("Select Tax", "0"));
        //DataSet dsTax = new DataSet();

        //dsTax = _blEstimateCalculation.GetTaxes();

        //if (dsTax != null)
        //{
        //    if (dsTax.Tables.Count > 0)
        //    {
        //        if (dsTax.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dsTax.Tables[0].Rows.Count; i++)
        //            {
        //                ddltax.Items.Add(new ListItem(Convert.ToString(dsTax.Tables[0].Rows[i]["Name"]), Convert.ToString(dsTax.Tables[0].Rows[i]["Name"])));
        //            }
        //        }
        //    }
        //}
        #endregion

    }
    #endregion
    #region ::Events::
    #endregion

    #region ::WebMethods::
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<EstimateCalculation> SaveEstimateCalculation(string LableName, int CalculationType,
    string ShowClaculatedValue, string InputBasedCalculationOperation, string Sequence, string BomType, string Calculationfactor, EstiateInputDataDerivedItems[] calculations, string CalculationText)
    {
        WebMethodResponse<EstimateCalculation> jsonPOInformation = new BusinessEntity.WebMethodResponse<EstimateCalculation>();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        jsonPOInformation.Header = new BusinessEntity.WebMethodHeader();



        try
        {
            BL_EstimateCalculation objBL_estcal = new BL_EstimateCalculation();
            EstimateCalculation objProp_estcal = new EstimateCalculation();
            objProp_estcal.ConnConfig = WebBaseUtility.ConnectionString;
            objProp_estcal.EstimateCalculationTemplateLableName = LableName;
            objProp_estcal.EstimateCalculationType = CalculationType;
            objProp_estcal.EstimateCalculationTemplateShowClaculatedValue = Convert.ToBoolean(ShowClaculatedValue);
            objProp_estcal.EstimateCalculationTemplateInputBasedCalculationfactor = string.IsNullOrEmpty(Calculationfactor) ? 0 : Convert.ToDecimal(Calculationfactor);
            objProp_estcal.EstimateCalculationTemplateInputBasedCalculationOperation = InputBasedCalculationOperation;
            objProp_estcal.EstimateCalculationTemplateSequence = Convert.ToInt32(Sequence);
            objProp_estcal.EstimateCalculationTemplateBOMTID = Convert.ToInt32(BomType);

            if (objProp_estcal.EstimateCalculationType == 1 && (objProp_estcal.EstimateCalculationTemplateBOMTID != null || objProp_estcal.EstimateCalculationTemplateBOMTID != 0))
                objProp_estcal.EstimateCalculationTemplateIsBOM = true;
            else if (objProp_estcal.EstimateCalculationType == 3)
                objProp_estcal.EstimateCalculationTemplateIsTax = true;
            else if (objProp_estcal.EstimateCalculationType == 2 && (!string.IsNullOrEmpty(objProp_estcal.EstimateCalculationTemplateInputBasedCalculationOperation)
                && objProp_estcal.EstimateCalculationTemplateInputBasedCalculationOperation != "0"))
                objProp_estcal.EstimateCalculationInputBasedCalculation = true;

            objProp_estcal.EstimateCalculationTemplateInputDataDerived = calculations.Length > 0 ? true : false;
            objProp_estcal.DerivedItems = calculations;

            objProp_estcal = objBL_estcal.CreateEstimateCalculation(objProp_estcal);
            objProp_estcal.ExpenseType = CalculationText;

            if (objProp_estcal != null)
            {
                if (objProp_estcal.ID != 0)
                    jsonPOInformation.Header.HasError = false;
                else
                    jsonPOInformation.Header.HasError = true;

            }
            else
                jsonPOInformation.Header.HasError = true;

            jsonPOInformation.ReponseObject = objProp_estcal;


        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonPOInformation.Header.HasError = true;
            jsonPOInformation.Header.ErrorMessages = strmsg;
        }
        return jsonPOInformation;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<EstimateCalculation[]> GetEstimateCalculation(string id)
    {
        WebMethodResponse<EstimateCalculation[]> jsonPOInformation = new BusinessEntity.WebMethodResponse<EstimateCalculation[]>();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        jsonPOInformation.Header = new BusinessEntity.WebMethodHeader();



        try
        {
            BL_EstimateCalculation objBL_estcal = new BL_EstimateCalculation();
            EstimateCalculation objProp_estcal = new EstimateCalculation();
            EstimateCalculation[] objProp_estcalist = null;
            objProp_estcal.ConnConfig = WebBaseUtility.ConnectionString;
            objProp_estcal.ID = Convert.ToInt32(id);


            objProp_estcalist = objBL_estcal.GetEstimateCalculations(objProp_estcal);

            if (objProp_estcalist != null)
            {
                jsonPOInformation.Header.HasError = false;
            }
            else
                jsonPOInformation.Header.HasError = true;

            jsonPOInformation.ReponseObject = objProp_estcalist;


        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonPOInformation.Header.HasError = true;
            jsonPOInformation.Header.ErrorMessages = strmsg;
        }
        return jsonPOInformation;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<string> SaveEstimateSequence(EstimateCalculation[] calculations)
    {
        WebMethodResponse<string> jsonPOInformation = new BusinessEntity.WebMethodResponse<string>();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        jsonPOInformation.Header = new BusinessEntity.WebMethodHeader();



        try
        {
            BL_EstimateCalculation objBL_estcal = new BL_EstimateCalculation();
            EstimateCalculation objProp_estcal = new EstimateCalculation();
            objProp_estcal.ConnConfig = WebBaseUtility.ConnectionString;
            if (calculations != null)
            {

                objBL_estcal.UpdateSequence(calculations);

            }



            jsonPOInformation.ReponseObject = string.Empty;


        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonPOInformation.Header.HasError = true;
            jsonPOInformation.Header.ErrorMessages = strmsg;
        }
        return jsonPOInformation;
    }


    #endregion
}

