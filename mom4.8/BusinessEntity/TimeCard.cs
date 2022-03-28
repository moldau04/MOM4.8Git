using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class TimeCard
    {
        public string ConnConfig { get; set; }
        public string DBName { get; set; }
        public string SearchText { get; set; }
        public int IsJob { get; set; }
        public int Loc { get; set; }
        public int JobId { get; set; }
        public string Worker { get; set; }
        public int Userid{ get; set; }
        public int EN { get; set; }
    }

    public class TimeCardProject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string fDesc { get; set; }
        public int Loc { get; set; }
        public string Tag { get; set; }
        public string State { get; set; }
        public string Cat { get; set; }
        public string Category { get; set; }
        public int Id { get; set; }
        public string Unit { get; set; }
        public string Type { get; set; }
        public string Fdesc { get; set; }
        public string Status { get; set; }
        public string Building { get; set; }
        public string BType { get; set; }
        public string Code { get; set; }
        public string Selected { get; set; }
        public string Address { get; set; }
        public string IsContract { get; set; }

        
    }

    public class SuperVisor
    {
        public int ID { get; set; }
        public string FDesc { get; set; }
        public int UserId { get; set; }
    }

    public class Worker
    {
        public int ID { get; set; }
        public string FDesc { get; set; }
        public string Super { get; set; }
    }

    public class Category
    {
        public int ID { get; set; }
        public string Type { get; set; }
       
    }

    public class TimeInputCardViewModel
    {
        public List<SuperVisor> lstSuperVisor { get; set; }
        public List<Worker> lstWorker { get; set; }
        public List<Category> lstCategory { get; set; }
    }

}
