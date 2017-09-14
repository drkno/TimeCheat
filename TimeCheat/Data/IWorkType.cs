using System;

namespace TimeSaver.Data
{
    public interface IWorkType
    {
        DateTime Start { get; }
        DateTime End { get; }
        WorkCategory Category { get; set; }
    }
}
