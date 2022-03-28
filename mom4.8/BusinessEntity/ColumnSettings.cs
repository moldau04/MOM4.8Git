using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class ColumnSettings
    {
        public string Name { get; set; }
        public bool Display { get; set; }
        public int Width { get; set; }
        public int OrderIndex { get; set; }
    }
}
