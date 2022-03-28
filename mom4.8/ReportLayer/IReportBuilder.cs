using Stimulsoft.Report;
using System.Collections.Generic;

namespace ReportLayer
{
    public interface IReportBuilder<T>
    {
        StiReport GetReportTemplate();
        void Build(StiReport report, T input, IReadOnlyDictionary<string, object> reportParams);
    }
}
