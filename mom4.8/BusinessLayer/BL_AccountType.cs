using BusinessEntity;
using DataLayer;
using System.Data;

namespace BusinessLayer
{
    public class BL_AccountType
    {
        DL_AccountType _objDLAcType = new DL_AccountType();
        public DataSet GetAllType(AccountType _objAcType)
        {
            return _objDLAcType.GetType(_objAcType);
        }
        public DataSet GetTypeByAccount(AccountType _objAcType)
        {
            return _objDLAcType.GetTypeByAccount(_objAcType);
        }
        public DataSet GetSubTypeByID(AccountType _objAcType)
        {
            return _objDLAcType.GetSubTypeByID(_objAcType);
        }
        public void AddSubAccount(AccountType objChart)
        {
            _objDLAcType.AddSubAccount(objChart);
        }
        public DataSet GetAllSubAccout(AccountType _objAcType)
        {
            return _objDLAcType.GetAllSubAccout(_objAcType);
        }
        public DataSet AddCentral(Central _objCentral)
        {
            return _objDLAcType.AddCentral(_objCentral);
        }
    }
}
