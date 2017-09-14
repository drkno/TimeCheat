using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TimeSaver.Abstractions;

namespace TimeSaver.Data.Source.Windowing
{
    public class WindowingDataSource : IDataSource
    {
        public string UniqueName { get; } = "Windowing";
        private string _title;
        private DateTime _last;
        private CategorisationEngine _engine;

        public event EventHandler<WorkPerformedEventArgs> WorkPerformed;

        public WindowingDataSource()
        {
            _title = null;
            _last = DateTime.Now;
            Platform.WindowChanged += WindowsHookManager_WindowChanged;
        }

        private void WindowsHookManager_WindowChanged(object _, WindowChangedEventArgs e)
        {
            var oldTitle = _title;
            var oldTime = _last;
            _title = e.WindowTitle;
            _last = DateTime.Now;
            if (oldTitle == null || _engine == null)
            {
                return;
            }
            var workType = new WindowWorkType(_last, oldTime, oldTitle);
            _engine.DetermineCategory(UniqueName, ref workType, IsCategory);
            var ev = new WorkPerformedEventArgs
            {
                SourceName = UniqueName,
                WorkType = workType
            };
            WorkPerformed?.Invoke(this, ev);
        }

        private bool IsCategory(WindowWorkType work, IDictionary<string, string[]> categoryRules)
        {
            var rules = CategorisationEngine.GetOrDefault(ref categoryRules, "window");
            return rules.Any(w => w == work.WindowTitle) || rules.Any(w => Regex.IsMatch(work.WindowTitle, w, RegexOptions.Compiled));
        }

        public void SetCategorisationEngine(CategorisationEngine engine)
        {
            _engine = engine;
        }

        public void Dispose()
        {
            _engine = null;
        }
    }
}
