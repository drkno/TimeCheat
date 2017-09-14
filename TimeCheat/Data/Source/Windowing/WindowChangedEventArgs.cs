using System;

namespace TimeSaver.Data.Source.Windowing
{
    public class WindowChangedEventArgs : EventArgs
    {
        public uint WindowId { get; set; }
        public string WindowTitle { get; set; }
    }
}
