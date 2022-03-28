using BusinessEntity;
using DataLayer;
using System.Data;


namespace BusinessLayer
{
    public class BL_EstimateCalculation
    {
        DL_EstimateCalculation _objDLEstimateCalculation = new DL_EstimateCalculation();
        public DataSet GetAllCalculationType(EstimateCalculation objProp_estcal)
        {


            return _objDLEstimateCalculation.GetAllCalculationType(objProp_estcal);

        }
        public DataSet GetTaxes()
        {
            return _objDLEstimateCalculation.GetTaxes();
            
        }

         public EstimateCalculation CreateEstimateCalculation(EstimateCalculation objProp_estcal)
        {
            return _objDLEstimateCalculation.CreateEstimateCalculation(objProp_estcal);

        }

         public EstimateCalculation[] GetEstimateCalculations(EstimateCalculation objProp_estcal)
         {
             return _objDLEstimateCalculation.GetEstimateCalculations(objProp_estcal);

         }

         public EstimateCalculation[] GetEstimateExpenseItems(EstimateCalculation objProp_estcal)
         {
             return _objDLEstimateCalculation.GetEstimateExpenseItems(objProp_estcal);

         }

         public EstimateCalculation[] GetEstimateExpenseItemsByExpenseItem(EstimateCalculation objProp_estcal)
         {
             return _objDLEstimateCalculation.GetEstimateExpenseItemsByExpenseItem(objProp_estcal);
         }

         public void UpdateSequence(EstimateCalculation[] objProp_estcal)
         {
             _objDLEstimateCalculation.UpdateSequence(objProp_estcal);
         }
         



    }
}
