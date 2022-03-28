using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BusinessEntity
{
    public class UserCustom
    {
        public string ConnConfig;

        #region::Private Declaration::
        
        private DataTable _UserCustomItem;
        private DataTable _UserCustomItemDelete;
        private DataTable _UserCustomValue;
        
        #endregion
        
        public DataTable UserCustomItem
        {
            get { return _UserCustomItem; }
            set { _UserCustomItem = value; }
        }
        
        public DataTable UserCustomItemDelete
        {
            get { return _UserCustomItemDelete; }
            set { _UserCustomItemDelete = value; }
        }
        
        public DataTable UserCustomValue
        {
            get { return _UserCustomValue; }
            set { _UserCustomValue = value; }
        }
    }
}
