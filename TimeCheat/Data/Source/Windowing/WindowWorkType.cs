using System;

namespace TimeSaver.Data.Source.Windowing
{
    public class WindowWorkType : IWorkType
    {
        public WindowWorkType(DateTime end, DateTime start, string windowTitle)
        {
            Start = start;
            End = end;
            WindowTitle = windowTitle;
        }

        public DateTime Start { get; }
        public DateTime End { get; }
        public WorkCategory Category { get; set; }
        public string WindowTitle { get; }

        public override string ToString()
        {
            return $"[WindowWorkType start={Start} end={End} cat={Category} title={WindowTitle}]";
        }
    }
}
