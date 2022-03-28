using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Data;
using BusinessEntity;

namespace BusinessLayer
{
    public class BL_UserCustom
    {
        DL_UserCustom objDL_UserCustom = new DL_UserCustom();

        public DataSet GetAllUserCustom(String conn, String dbName)
        {
            return objDL_UserCustom.GetAllUserCustom(conn, dbName);
        }

        public int CreateAndUpdateUserCustom(UserCustom usercustom)
        {
            return objDL_UserCustom.CreateAndUpdateUserCustom(usercustom);
        }

        public DataSet GetUserCustomFieldValue(String conn, int userId)
        {
            return objDL_UserCustom.GetUserCustomFieldValue(conn, userId);
        }

    }
}
