using System.Configuration;

namespace Microsoft.ApplicationBlocks.Data
{
    public class Config
    {
        private static string _MS = string.Empty;        

        static Config()
        {
            for (int index = 0; index < ConfigurationManager.ConnectionStrings.Count; index++)
            {
                _MS = ConfigurationManager.ConnectionStrings["MS.DbConnection"].ConnectionString;
            }
        }
        public static string MS
        {
            get
            {
                return _MS;
            }
        }
    }
}
