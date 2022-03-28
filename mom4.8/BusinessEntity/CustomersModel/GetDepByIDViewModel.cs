﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetDepByIDViewModel
    {
        public int Ref { get; set; }
        public DateTime fDate { get; set; }
        public int Bank { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public int TransID { get; set; }
        public int EN { get; set; }
        public Int16 IsRecon { get; set; }

    }
}
