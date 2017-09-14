using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace TimeSaver.Data.Source
{
    public class CategorisationEngine
    {
        public static WorkCategory Unknown = new WorkCategory(null);
        private readonly List<WorkCategory> _categories = new List<WorkCategory>();

        public static string[] GetOrDefault(ref IDictionary<string, string[]> source, string key)
        {
            return source.TryGetValue(key, out string[] val) ? val : new string[0];
        }

        public void DetermineCategory<T>(string sourceName, ref T workType, Func<T, IDictionary<string, string[]>, bool> isCategory) where T : IWorkType
        {
            foreach (var workCategory in _categories)
            {
                var rules = workCategory.Rules;
                var inc = GetOrDefault(ref rules, "include");
                var exc = GetOrDefault(ref rules, "exclude");

                var include = inc.Length == 0 || inc.Any(i => i == sourceName);
                var exclude = exc.Length == 0 || exc.All(e => e != sourceName);

                if (include && exclude && isCategory(workType, rules))
                {
                    workType.Category = workCategory;
                    return;
                }
            }
            workType.Category = Unknown;
        }

        public CategorisationEngine()
        {
            dynamic data = JArray.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "categories.json")));
            foreach (var d in data)
            {
                var category = new WorkCategory(d);
                _categories.Add(category);
            }
        }
    }
}
