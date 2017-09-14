using System;

namespace TimeSaver.Data.Source
{
    public interface IDataSource : IDisposable
    {
        string UniqueName { get; }
        void SetCategorisationEngine(CategorisationEngine engine);
        event EventHandler<WorkPerformedEventArgs> WorkPerformed;
    }
}
