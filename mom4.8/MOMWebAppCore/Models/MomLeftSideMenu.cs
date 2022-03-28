using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAppCore.Models
{
    public class MomLeftSideMenu
    {
        public string URL { get; set; }
        public string URLText { get; set; }

        public bool Permission { get; set; }
        public List<MomLeftSideSubMenu> MomLeftSideSubMenu { get; set; }
    }

    public class MomLeftSideSubMenu
    {
        public string URL { get; set; }
        public string URLText { get; set; }

        public bool Permission { get; set; }
    }
}
