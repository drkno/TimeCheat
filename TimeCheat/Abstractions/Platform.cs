using System;
using TimeSaver.Abstractions.Windows;
using TimeSaver.Data.Source.Windowing;

namespace TimeSaver.Abstractions
{
    public static class Platform
    {
        public enum Os
        {
            Windows,
            Unknown
        }

        public static Os OperatingSystem { get; }
        public static event EventHandler<WindowChangedEventArgs> WindowChanged;

        private static readonly IEventWaiter Waiter;
        
        static Platform()
        {
            if (Environment.OSVersion.ToString().Contains("Windows"))
            {
                OperatingSystem = Os.Windows;
            }
            else
            {
                OperatingSystem = Os.Unknown;
            }

            switch (OperatingSystem)
            {
                case Os.Windows:
                    Waiter = new WindowsEventWaiter();
                    break;
                case Os.Unknown:
                    throw new InvalidOperationException("Unknown platform.");
            }
        }

        public static void Initialise()
        {
            switch (OperatingSystem)
            {
                case Os.Windows:
                    WindowsHookManager.WindowChanged += OnWindowChanged;
                    WindowsHookManager.Initialise();
                    break;
            }
        }

        public static void TearDown()
        {
            switch (OperatingSystem)
            {
                case Os.Windows:
                    WindowsHookManager.TearDown();
                    WindowsHookManager.WindowChanged -= OnWindowChanged;
                    break;
            }
        }

        public static void WaitUntilQuit()
        {
            Waiter.WaitUntilQuit();
        }

        private static void OnWindowChanged(object sender, WindowChangedEventArgs e)
        {
            WindowChanged?.Invoke(sender, e);
        }
    }
}
