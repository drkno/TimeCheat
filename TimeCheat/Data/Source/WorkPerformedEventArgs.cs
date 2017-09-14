using System;

namespace TimeSaver.Data.Source
{
    public class WorkPerformedEventArgs : EventArgs
    {
        public string SourceName { get; set; }
        public IWorkType WorkType { get; set; }

        public override string ToString()
        {
            return $"[WorkPerformedEventArgs source={SourceName} type={WorkType}]";
        }
    }
}
