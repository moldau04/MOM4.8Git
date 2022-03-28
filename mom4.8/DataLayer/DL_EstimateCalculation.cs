using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;


namespace DataLayer
{
    public class DL_EstimateCalculation
    {
        public DataSet GetAllCalculationType(EstimateCalculation objProp_estcal)
        {

            DataSet ds = null;

            try
            {
                string constring = objProp_estcal.ConnConfig;

                ds = SqlHelper.ExecuteDataset(constring, CommandType.Text, "select * from EstimateCalculationType");
            }
            catch (Exception e)
            {

            }
            return ds;

        }

        public DataSet GetTaxes()
        {
            DataSet ds = null;

            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, CommandType.Text, "select s.*,case s.Utype when 0 then 'Sales tax' when 1 then 'Use tax' end as Utypename, c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID order by name");
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        public EstimateCalculation CreateEstimateCalculation(EstimateCalculation objProp_estcal)
        {
            EstimateCalculation success = objProp_estcal;

            try
            {
                using (SqlConnection con = new SqlConnection(objProp_estcal.ConnConfig))
                {
                    con.Open();
                    using (SqlTransaction tans = con.BeginTransaction())
                    {
                        try
                        {



                            int id = (int)SqlHelper.ExecuteScalar(tans, EstimateCalculation.CREATE_ESTIMATETEMPLATE_CALCULATION,
                                objProp_estcal.EstimateCalculationTemplateLableName,
                                objProp_estcal.EstimateCalculationType,
                                objProp_estcal.EstimateCalculationTemplateBOMTID,
                                SetSafeBool(objProp_estcal.EstimateCalculationTemplateIsBOM),
                                SetSafeBool(objProp_estcal.EstimateCalculationTemplateIsTax),
                                SetSafeBool(objProp_estcal.EstimateCalculationInputBasedCalculation),
                                SetSafeBool(objProp_estcal.EstimateCalculationTemplateInputDataDerived),
                                SetSafeBool(objProp_estcal.EstimateCalculationTemplateShowClaculatedValue),
                                objProp_estcal.EstimateCalculationTemplateInputBasedCalculationfactor,
                                objProp_estcal.EstimateCalculationTemplateInputBasedCalculationOperation,
                                objProp_estcal.EstimateCalculationTemplateSequence
                                );
                            success.ID = id;
                            if (objProp_estcal.DerivedItems != null)
                            {
                                foreach (EstiateInputDataDerivedItems objProp_estcalDirvItems in objProp_estcal.DerivedItems)
                                {
                                    objProp_estcalDirvItems.EstimateCalculationForTemplateId = success.ID;
                                    objProp_estcalDirvItems.ID = (int)SqlHelper.ExecuteScalar(tans, EstimateCalculation.CREATE_ESTIMATETEMPLATE_CALCULATION_TEMPLATE_INPUT_DATA_DERIVED_ITEMS, objProp_estcalDirvItems.EstimateCalculationForTemplateId, objProp_estcalDirvItems.EstimateCalculationOnTemplateId, objProp_estcalDirvItems.Operation);
                                }
                            }

                            tans.Commit();


                        }
                        catch (Exception ex)
                        {

                            tans.Rollback();
                        }
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }


            return success;

        }

        public EstimateCalculation[] GetEstimateCalculations(EstimateCalculation objProp_estcal)
        {
            DataSet ds = null;
            //EstimateCalculation[] estimatecallist = null;
            List<EstimateCalculation> estimatecallist = new List<EstimateCalculation>();

            try
            {


                ds = SqlHelper.ExecuteDataset(objProp_estcal.ConnConfig, CommandType.StoredProcedure, EstimateCalculation.GET_ESTIMATETEMPLATE_CALCULATION);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                EstimateCalculation item = new EstimateCalculation();
                                item.ID = ds.Tables[0].Rows[i]["EstimateCalculationTemplateId"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplateId"]) : 0;
                                item.EstimateCalculationTemplateNo = ds.Tables[0].Rows[i]["EstimateCalculationTemplateNo"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateNo"]) : string.Empty;
                                item.EstimateCalculationTemplateLableName = ds.Tables[0].Rows[i]["EstimateCalculationTemplateLableName"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateLableName"]) : string.Empty;
                                item.EstimateCalculationType = ds.Tables[0].Rows[i]["EstimateCalculationTemplate_EstimateCalculationType"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplate_EstimateCalculationType"]) : 0;
                                item.EstimateCalculationTemplateBOMTID = ds.Tables[0].Rows[i]["EstimateCalculationTemplate_BOMTID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplate_BOMTID"]) : 0;
                                item.EstimateCalculationTemplateIsBOM = ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsBOM"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsBOM"]) : false;
                                item.EstimateCalculationTemplateIsTax = ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsTax"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsTax"]) : false;
                                item.EstimateCalculationInputBasedCalculation = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculation"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculation"]) : false;
                                item.EstimateCalculationTemplateInputDataDerived = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerived"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerived"]) : false;
                                item.EstimateCalculationTemplateShowClaculatedValue = ds.Tables[0].Rows[i]["EstimateCalculationTemplateShowClaculatedValue"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateShowClaculatedValue"]) : false;
                                item.EstimateCalculationTemplateInputBasedCalculationfactor = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationfactor"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationfactor"]) : 0;
                                item.EstimateCalculationTemplateInputBasedCalculationOperation = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationOperation"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationOperation"]) : string.Empty;
                                item.EstimateCalculationTemplateSequence = ds.Tables[0].Rows[i]["EstimateCalculationTemplateSequence"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplateSequence"]) : 0;
                                item.ExpenseType = ds.Tables[0].Rows[i]["EstimateCalculationName"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationName"]) : "";

                                if (item != null)
                                {
                                    DataSet ds2 = SqlHelper.ExecuteDataset(objProp_estcal.ConnConfig, EstimateCalculation.GET_ESTIMATETEMPLATE_CALCULATION_DERIVEDITEMS, item.ID);
                                    List<EstiateInputDataDerivedItems> ditems = new List<EstiateInputDataDerivedItems>();
                                    if (ds2 != null)
                                    {
                                        if (ds2.Tables.Count > 0)
                                        {
                                            if (ds2.Tables[0].Rows.Count > 0)
                                            {
                                                for (int xi = 0; xi < ds2.Tables[0].Rows.Count; xi++)
                                                {
                                                    EstiateInputDataDerivedItems ditem = new EstiateInputDataDerivedItems();
                                                    ditem.ID = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsId"]) : 0;
                                                    ditem.EstimateCalculationForTemplateId = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItems_EstimateCalculationForTemplateId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItems_EstimateCalculationForTemplateId"]) : 0;
                                                    ditem.EstimateCalculationOnTemplateId = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsTemplateId_EstimateCalculationOnTemplateId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsTemplateId_EstimateCalculationOnTemplateId"]) : 0;
                                                    ditem.Operation = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsOperation"] != DBNull.Value ? Convert.ToString(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsOperation"]) : "";
                                                    ditems.Add(ditem);
                                                }
                                            }
                                        }
                                    }
                                    item.DerivedItems = ditems.ToArray();
                                }


                                estimatecallist.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return estimatecallist.ToArray();
        }


        public EstimateCalculation GetEstimateCalculation(EstimateCalculation objProp_estcal)
        {
            DataSet ds = null;
            //EstimateCalculation[] estimatecallist = null;
            EstimateCalculation estimatecallist = new EstimateCalculation();

            try
            {


                ds = SqlHelper.ExecuteDataset(objProp_estcal.ConnConfig, EstimateCalculation.GET_ESTIMATETEMPLATE_CALCULATION, objProp_estcal.ID);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                EstimateCalculation item = new EstimateCalculation();
                                item.ID = ds.Tables[0].Rows[i]["EstimateCalculationTemplateId"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplateId"]) : 0;
                                item.EstimateCalculationTemplateNo = ds.Tables[0].Rows[i]["EstimateCalculationTemplateNo"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateNo"]) : string.Empty;
                                item.EstimateCalculationTemplateLableName = ds.Tables[0].Rows[i]["EstimateCalculationTemplateLableName"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateLableName"]) : string.Empty;
                                item.EstimateCalculationType = ds.Tables[0].Rows[i]["EstimateCalculationTemplate_EstimateCalculationType"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplate_EstimateCalculationType"]) : 0;
                                item.EstimateCalculationTemplateBOMTID = ds.Tables[0].Rows[i]["EstimateCalculationTemplate_BOMTID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplate_BOMTID"]) : 0;
                                item.EstimateCalculationTemplateIsBOM = ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsBOM"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsBOM"]) : false;
                                item.EstimateCalculationTemplateIsTax = ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsTax"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsTax"]) : false;
                                item.EstimateCalculationInputBasedCalculation = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculation"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculation"]) : false;
                                item.EstimateCalculationTemplateInputDataDerived = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerived"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerived"]) : false;
                                item.EstimateCalculationTemplateShowClaculatedValue = ds.Tables[0].Rows[i]["EstimateCalculationTemplateShowClaculatedValue"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateShowClaculatedValue"]) : false;
                                item.EstimateCalculationTemplateInputBasedCalculationfactor = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationfactor"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationfactor"]) : 0;
                                item.EstimateCalculationTemplateInputBasedCalculationOperation = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationOperation"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationOperation"]) : string.Empty;
                                item.EstimateCalculationTemplateSequence = ds.Tables[0].Rows[i]["EstimateCalculationTemplateSequence"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplateSequence"]) : 0;
                                item.ExpenseType = ds.Tables[0].Rows[i]["EstimateCalculationName"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationName"]) : "";
                                estimatecallist = item;
                            }
                        }
                    }
                }

                if (estimatecallist != null)
                {
                    DataSet ds2 = SqlHelper.ExecuteDataset(objProp_estcal.ConnConfig, EstimateCalculation.GET_ESTIMATETEMPLATE_CALCULATION_DERIVEDITEMS, estimatecallist.ID);
                    List<EstiateInputDataDerivedItems> ditems = new List<EstiateInputDataDerivedItems>();
                    if (ds2 != null)
                    {
                        if (ds2.Tables.Count > 0)
                        {
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                                {
                                    EstiateInputDataDerivedItems ditem = new EstiateInputDataDerivedItems();
                                    ditem.ID = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerivedItemsId"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerivedItemsId"]) : 0;
                                    ditem.EstimateCalculationForTemplateId = ds2.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerivedItems_EstimateCalculationForTemplateId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerivedItems_EstimateCalculationForTemplateId"]) : 0;
                                    ditem.EstimateCalculationOnTemplateId = ds2.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerivedItemsTemplateId_EstimateCalculationOnTemplateId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerivedItemsTemplateId_EstimateCalculationOnTemplateId"]) : 0;
                                    ditem.Operation = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerivedItemsOperation"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerivedItemsOperation"]) : "";
                                    ditems.Add(ditem);
                                }
                            }
                        }
                    }
                    estimatecallist.DerivedItems = ditems.ToArray();
                }
            }
            catch (Exception e)
            {

            }
            return estimatecallist;
        }

        public void UpdateSequence(EstimateCalculation[] objProp_estcal)
        {
            try
            {



                string constring = HttpContext.Current.Session["config"].ToString();
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();

                    using (SqlTransaction tans = con.BeginTransaction(""))
                    {

                        try
                        {

                            if (objProp_estcal != null)
                            {
                                foreach (EstimateCalculation item in objProp_estcal)
                                {

                                    SqlHelper.ExecuteNonQuery(tans, EstimateCalculation.UPDATE_ESTIMATETEMPLATE_SEQUENCE, item.ID, item.EstimateCalculationTemplateSequence);

                                }


                            }

                            tans.Commit();


                        }
                        catch (Exception ex)
                        {

                            tans.Rollback("");

                            throw ex;
                        }
                    }

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EstimateCalculation[] GetEstimateExpenseItems(EstimateCalculation objProp_estcal)
        {
            DataSet ds = null;
            //EstimateCalculation[] estimatecallist = null;
            List<EstimateCalculation> estimatecallist = new List<EstimateCalculation>();

            try
            {


                ds = SqlHelper.ExecuteDataset(objProp_estcal.ConnConfig, EstimateCalculation.GET_ESTIMATE_EXPENSE_CALCULATION, null, objProp_estcal.EstimateId, null, null);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                EstimateCalculation item = new EstimateCalculation();
                                item.ID = ds.Tables[0].Rows[i]["EstimateCalculationTemplateId"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplateId"]) : 0;
                                item.EstimateCalculationTemplateNo = ds.Tables[0].Rows[i]["EstimateCalculationTemplateNo"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateNo"]) : string.Empty;
                                item.EstimateCalculationTemplateLableName = ds.Tables[0].Rows[i]["EstimateCalculationTemplateLableName"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateLableName"]) : string.Empty;
                                item.EstimateCalculationType = ds.Tables[0].Rows[i]["EstimateCalculationTemplate_EstimateCalculationType"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplate_EstimateCalculationType"]) : 0;
                                item.EstimateCalculationTemplateBOMTID = ds.Tables[0].Rows[i]["EstimateCalculationTemplate_BOMTID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplate_BOMTID"]) : 0;
                                item.EstimateCalculationTemplateIsBOM = ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsBOM"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsBOM"]) : false;
                                item.EstimateCalculationTemplateIsTax = ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsTax"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsTax"]) : false;
                                item.EstimateCalculationInputBasedCalculation = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculation"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculation"]) : false;
                                item.EstimateCalculationTemplateInputDataDerived = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerived"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerived"]) : false;
                                item.EstimateCalculationTemplateShowClaculatedValue = ds.Tables[0].Rows[i]["EstimateCalculationTemplateShowClaculatedValue"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateShowClaculatedValue"]) : false;
                                item.EstimateCalculationTemplateInputBasedCalculationfactor = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationfactor"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationfactor"]) : 0;
                                item.EstimateCalculationTemplateInputBasedCalculationOperation = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationOperation"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationOperation"]) : string.Empty;
                                item.EstimateCalculationTemplateSequence = ds.Tables[0].Rows[i]["EstimateCalculationTemplateSequence"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplateSequence"]) : 0;
                                item.ExpenseType = ds.Tables[0].Rows[i]["EstimateCalculationName"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationName"]) : "";
                                item.EstimateCalculationValue = ds.Tables[0].Rows[i]["EstimateCalculationValue"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationValue"]) : "";
                                if (item != null)
                                {
                                    DataSet ds2 = SqlHelper.ExecuteDataset(objProp_estcal.ConnConfig, EstimateCalculation.GET_ESTIMATETEMPLATE_CALCULATION_DERIVEDITEMS, item.ID);
                                    List<EstiateInputDataDerivedItems> ditems = new List<EstiateInputDataDerivedItems>();
                                    if (ds2 != null)
                                    {
                                        if (ds2.Tables.Count > 0)
                                        {
                                            if (ds2.Tables[0].Rows.Count > 0)
                                            {
                                                for (int xi = 0; xi < ds2.Tables[0].Rows.Count; xi++)
                                                {
                                                    EstiateInputDataDerivedItems ditem = new EstiateInputDataDerivedItems();
                                                    ditem.ID = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsId"]) : 0;
                                                    ditem.EstimateCalculationForTemplateId = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItems_EstimateCalculationForTemplateId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItems_EstimateCalculationForTemplateId"]) : 0;
                                                    ditem.EstimateCalculationOnTemplateId = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsTemplateId_EstimateCalculationOnTemplateId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsTemplateId_EstimateCalculationOnTemplateId"]) : 0;
                                                    ditem.Operation = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsOperation"] != DBNull.Value ? Convert.ToString(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsOperation"]) : "";
                                                    ditems.Add(ditem);
                                                }
                                            }
                                        }
                                    }
                                    item.DerivedItems = ditems.ToArray();
                                }


                                estimatecallist.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return estimatecallist.ToArray();
        }

        public EstimateCalculation[] GetEstimateExpenseItemsByExpenseItem(EstimateCalculation objProp_estcal)
        {
            DataSet ds = null;
            //EstimateCalculation[] estimatecallist = null;
            List<EstimateCalculation> estimatecallist = new List<EstimateCalculation>();

            try
            {


                ds = SqlHelper.ExecuteDataset(objProp_estcal.ConnConfig, EstimateCalculation.GET_ESTIMATE_EXPENSE_CALCULATION, null, objProp_estcal.EstimateId, objProp_estcal.ID, objProp_estcal.EstimateCalculationTemplateInputBasedCalculationfactor);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                EstimateCalculation item = new EstimateCalculation();
                                item.ID = ds.Tables[0].Rows[i]["EstimateCalculationTemplateId"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplateId"]) : 0;
                                item.EstimateCalculationTemplateNo = ds.Tables[0].Rows[i]["EstimateCalculationTemplateNo"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateNo"]) : string.Empty;
                                item.EstimateCalculationTemplateLableName = ds.Tables[0].Rows[i]["EstimateCalculationTemplateLableName"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateLableName"]) : string.Empty;
                                item.EstimateCalculationType = ds.Tables[0].Rows[i]["EstimateCalculationTemplate_EstimateCalculationType"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplate_EstimateCalculationType"]) : 0;
                                item.EstimateCalculationTemplateBOMTID = ds.Tables[0].Rows[i]["EstimateCalculationTemplate_BOMTID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplate_BOMTID"]) : 0;
                                item.EstimateCalculationTemplateIsBOM = ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsBOM"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsBOM"]) : false;
                                item.EstimateCalculationTemplateIsTax = ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsTax"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateIsTax"]) : false;
                                item.EstimateCalculationInputBasedCalculation = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculation"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculation"]) : false;
                                item.EstimateCalculationTemplateInputDataDerived = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerived"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputDataDerived"]) : false;
                                item.EstimateCalculationTemplateShowClaculatedValue = ds.Tables[0].Rows[i]["EstimateCalculationTemplateShowClaculatedValue"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["EstimateCalculationTemplateShowClaculatedValue"]) : false;
                                item.EstimateCalculationTemplateInputBasedCalculationfactor = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationfactor"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationfactor"]) : 0;
                                item.EstimateCalculationTemplateInputBasedCalculationOperation = ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationOperation"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationTemplateInputBasedCalculationOperation"]) : string.Empty;
                                item.EstimateCalculationTemplateSequence = ds.Tables[0].Rows[i]["EstimateCalculationTemplateSequence"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EstimateCalculationTemplateSequence"]) : 0;
                                item.ExpenseType = ds.Tables[0].Rows[i]["EstimateCalculationName"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationName"]) : "";
                                item.EstimateCalculationValue = ds.Tables[0].Rows[i]["EstimateCalculationValue"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["EstimateCalculationValue"]) : "";
                                if (item != null)
                                {
                                    DataSet ds2 = SqlHelper.ExecuteDataset(objProp_estcal.ConnConfig, EstimateCalculation.GET_ESTIMATETEMPLATE_CALCULATION_DERIVEDITEMS, item.ID);
                                    List<EstiateInputDataDerivedItems> ditems = new List<EstiateInputDataDerivedItems>();
                                    if (ds2 != null)
                                    {
                                        if (ds2.Tables.Count > 0)
                                        {
                                            if (ds2.Tables[0].Rows.Count > 0)
                                            {
                                                for (int xi = 0; xi < ds2.Tables[0].Rows.Count; xi++)
                                                {
                                                    EstiateInputDataDerivedItems ditem = new EstiateInputDataDerivedItems();
                                                    ditem.ID = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsId"]) : 0;
                                                    ditem.EstimateCalculationForTemplateId = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItems_EstimateCalculationForTemplateId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItems_EstimateCalculationForTemplateId"]) : 0;
                                                    ditem.EstimateCalculationOnTemplateId = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsTemplateId_EstimateCalculationOnTemplateId"] != DBNull.Value ? Convert.ToInt32(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsTemplateId_EstimateCalculationOnTemplateId"]) : 0;
                                                    ditem.Operation = ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsOperation"] != DBNull.Value ? Convert.ToString(ds2.Tables[0].Rows[xi]["EstimateCalculationTemplateInputDataDerivedItemsOperation"]) : "";
                                                    ditems.Add(ditem);
                                                }
                                            }
                                        }
                                    }
                                    item.DerivedItems = ditems.ToArray();
                                }


                                estimatecallist.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return estimatecallist.ToArray();
        }


        private object SetSafeBool(bool obj)
        {
            if (!obj)
                return 0;
            else
                return 1;
        }
    }

}
