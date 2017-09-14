using System;
using System.Collections.Generic;

namespace TimeSaver.Data.Sink
{
    interface IDataSink
    {
        void Publish(Dictionary<DateTime, Dictionary<string, double>> data);
    }
}
