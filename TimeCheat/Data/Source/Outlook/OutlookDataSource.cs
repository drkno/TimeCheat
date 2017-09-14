using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Outlook;

namespace TimeSaver.Data.Source.Outlook
{
    public class OutlookDataSource : IDataSource
    {
        public event EventHandler<WorkPerformedEventArgs> WorkPerformed;
        public string UniqueName { get; } = "Outlook Calendar";
        private CategorisationEngine _engine;
        private readonly Application _app;

        public OutlookDataSource()
        {
            _app = new Application();
            _app.Reminder += App_Reminder;
        }

        private void App_Reminder(dynamic item)
        {
            var wt = new OutlookWorkType(item.Start, item.End, item.Subject, item.Location);
            _engine.DetermineCategory(UniqueName, ref wt, IsCategory);
            WorkPerformed?.Invoke(this, new WorkPerformedEventArgs {SourceName = UniqueName, WorkType = wt});
        }

        private bool IsCategory(OutlookWorkType outlookWorkType, IDictionary<string, string[]> rules)
        {
            if (!rules.ContainsKey("outlook"))
            {
                return false;
            }
            var locRules = CategorisationEngine.GetOrDefault(ref rules, "outlook");
            return !locRules.Any(r => outlookWorkType.Subject.ToLower().Contains(r) || outlookWorkType.Location.ToLower().Contains(r));
        }

        public void SetCategorisationEngine(CategorisationEngine engine)
        {
            _engine = engine;
        }
        
        public void Dispose()
        {
            _app.Reminder -= App_Reminder;
            _engine = null;
        }
    }
}
