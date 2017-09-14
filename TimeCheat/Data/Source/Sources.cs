using System;
using System.Collections.Generic;
using TimeSaver.Abstractions;
using TimeSaver.Data.Source.Outlook;
using TimeSaver.Data.Source.Windowing;

namespace TimeSaver.Data.Source
{
    public static class Sources
    {
        private static IDataSource[] _defaultSources;
        public static IDataSource[] DefaultSources
        {
            get
            {
                if (_defaultSources == null)
                {
                    var sources = new List<IDataSource> {new WindowingDataSource()};
                    if (Platform.OperatingSystem == Platform.Os.Windows)
                    {
                        sources.Add(new OutlookDataSource());
                    }
                    _defaultSources = sources.ToArray();
                }
                return _defaultSources;
            }
        }

        public static CategorisationEngine CategorisationEngine = new CategorisationEngine();

        public static event EventHandler<WorkPerformedEventArgs> WorkPerformed;

        static Sources()
        {
            foreach (var defaultSource in DefaultSources)
            {
                defaultSource.SetCategorisationEngine(CategorisationEngine);
                defaultSource.WorkPerformed += DefaultSource_WorkPerformed;
            }
        }

        private static void DefaultSource_WorkPerformed(object sender, WorkPerformedEventArgs e)
        {
            WorkPerformed?.Invoke(sender, e);
        }
    }
}
