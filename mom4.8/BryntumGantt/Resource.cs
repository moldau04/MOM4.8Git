//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bryntum.Gantt
{
    using System;
    using System.Collections.Generic;
    
    public partial class Resource
    {
        public Resource()
        {
            this.Assignments = new HashSet<Assignment>();
        }
    
        public override int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> CalendarIdRaw { get; set; }
    
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual Calendar Calendar { get; set; }
    }
}
