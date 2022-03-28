using System.Data;

namespace BusinessEntity
{
    public class TestCustom
    {
        public string ConnConfig;
        #region::Private Declaration::
        private DataTable _TestCustomItem;
        private DataTable _TestCustomItemDelete;
        private DataTable _TestCustomValue;
        #endregion
        public DataTable TestCustomItem
        {
            get { return _TestCustomItem; }
            set { _TestCustomItem = value; }
        }
        public DataTable TestCustomItemDelete
        {
            get { return _TestCustomItemDelete; }
            set { _TestCustomItemDelete = value; }
        }
        public DataTable TestCustomValue
        {
            get { return _TestCustomValue; }
            set { _TestCustomValue = value; }
        }
    }
    public class Workflow
    {
        public string ConnConfig;       
        public DataTable workflowItem;
        public DataTable workflowItemDelete;
        public DataTable workflowValue;
       
    }
}
