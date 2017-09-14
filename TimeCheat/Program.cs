using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using TimeSaver.Abstractions;
using TimeSaver.Data.Source;

namespace TimeSaver
{
    class Program
    {
        static void Main()
        {
            Platform.Initialise();
            _workEntries = new Dictionary<string, Dictionary<string, double>>();
            Sources.WorkPerformed += DataSource_WorkPerformed;
            Platform.WaitUntilQuit();
            Platform.TearDown();
        }

        private static Dictionary<string, Dictionary<string, double>> _workEntries;

        private static void DataSource_WorkPerformed(object sender, WorkPerformedEventArgs e)
        {
            var wt = e.WorkType;
            var dayKey = wt.Start.ToString("d");
            if (!_workEntries.ContainsKey(dayKey))
            {
                _workEntries[dayKey] = new Dictionary<string, double>();
            }
            var catKey = wt.Category.Name;
            if (!_workEntries[dayKey].ContainsKey(catKey))
            {
                _workEntries[dayKey][catKey] = 0;
            }
            var time = (wt.End - wt.Start).TotalMinutes;
            _workEntries[dayKey][catKey] += time;

            Console.WriteLine($"Added entry for {catKey} ({time}), total is now {_workEntries[dayKey][catKey]}.");
        }
    }
}
