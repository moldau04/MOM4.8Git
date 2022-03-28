using BusinessEntity;
using DataLayer;
using System.Data;

namespace BusinessLayer
{
    public class BL_STax
    {
        DL_STax objSTax = new DL_STax();

        public DataSet GetAllSTaxByUType(STax info)
        {
            return objSTax.GetAllSTaxByUType(info);
        }
    }
}
