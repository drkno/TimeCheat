using System.Collections.Generic;

namespace TimeSaver.Data
{
    public class WorkCategory
    {
        public WorkCategory(dynamic data)
        {
            if (data == null)
            {
                Name = "Unknown";
                Rules = new Dictionary<string, string[]>();
            }
            else
            {
                Name = data.name;
                Rules = new Dictionary<string, string[]>();
                foreach (var rule in data.rules)
                {
                    var l = new List<string>();
                    foreach (var v in rule.Value)
                    {
                        l.Add(v.ToString());
                    }
                    Rules[rule.Name] = l.ToArray();
                }
            }
        }

        public string Name { get; }

        public IDictionary<string, string[]> Rules { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
