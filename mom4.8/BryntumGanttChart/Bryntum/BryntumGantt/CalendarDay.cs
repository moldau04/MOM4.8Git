//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BryntumGanttChart.Bryntum.BryntumGantt
{
    using System;
    using System.Collections.Generic;
    
    public partial class CalendarDay
    {
        public override int Id { get; set; }
        public int CalendarIdRaw { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string AvailabilityRaw { get; set; }
        public Nullable<int> Weekday { get; set; }
        public Nullable<System.DateTime> OverrideStartDate { get; set; }
        public Nullable<System.DateTime> OverrideEndDate { get; set; }
        public Nullable<byte> IsWorkingDayRaw { get; set; }
        public string Cls { get; set; }
    
        public virtual Calendar Calendar { get; set; }
    }
}