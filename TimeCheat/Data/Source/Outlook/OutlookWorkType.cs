using System;

namespace TimeSaver.Data.Source.Outlook
{
    public class OutlookWorkType : IWorkType
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public WorkCategory Category { get; set; }
        public string Subject { get; }
        public string Location { get; }

        public OutlookWorkType(DateTime start, DateTime end, string subject, string location)
        {
            Start = start;
            End = end;
            Subject = subject;
            Location = location;
        }

        public override string ToString()
        {
            return $"[OutlookWorkType start={Start} end={End} cat={Category} sub={Subject} loc={Location}]";
        }
    }
}
