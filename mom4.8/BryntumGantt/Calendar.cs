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
    
    public partial class Calendar
    {
        public Calendar()
        {
            this.CalendarDays = new HashSet<CalendarDay>();
            this.ChildrenRaw = new HashSet<Calendar>();
            this.Resources = new HashSet<Resource>();
            this.Tasks = new HashSet<Task>();
        }
    
        public override int Id { get; set; }
        public override Nullable<int> parentIdRaw { get; set; }
        public string Name { get; set; }
        public Nullable<int> DaysPerMonth { get; set; }
        public Nullable<int> DaysPerWeek { get; set; }
        public Nullable<int> HoursPerDay { get; set; }
        public Nullable<byte> WeekendsAreWorkdaysRaw { get; set; }
        public Nullable<int> WeekendFirstDay { get; set; }
        public Nullable<int> WeekendSecondDay { get; set; }
        public string DefaultAvailabilityRaw { get; set; }
    
        public virtual ICollection<CalendarDay> CalendarDays { get; set; }
        public virtual ICollection<Calendar> ChildrenRaw { get; set; }
        public virtual Calendar Parent { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
