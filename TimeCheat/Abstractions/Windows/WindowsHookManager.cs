using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using TimeSaver.Data.Source.Windowing;

namespace TimeSaver.Abstractions.Windows
{
    internal static class WindowsHookManager
    {
        private enum WinEvent
        {
            OutOfContext = 0,
            SystemForegroundEvent = 3
        }

        public static event EventHandler<WindowChangedEventArgs> WindowChanged;
        private delegate void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWinEventHook(int eventMin, int eventMax, IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc, int idProcess, int idThread, int dwflags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        private static IntPtr _windowEventHook;
        private static WinEventProc _callback;

        public static void TearDown()
        {
            if (_windowEventHook != IntPtr.Zero)
            {
                UnhookWinEvent(_windowEventHook);
                _callback = null;
            }
        }

        public static void Initialise()
        {
            if (_windowEventHook == IntPtr.Zero)
            {
                _callback = new WinEventProc(WindowEventCallback);
                _windowEventHook = SetWinEventHook((int)WinEvent.SystemForegroundEvent, (int)WinEvent.SystemForegroundEvent,
                    IntPtr.Zero, _callback, 0, 0, (int)WinEvent.OutOfContext);

                if (_windowEventHook == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            var curr = GetActiveWindow();
            WindowChanged?.Invoke(null, new WindowChangedEventArgs { WindowTitle = GetActiveWindowTitle(curr), WindowId = (uint)curr });
        }

        private static void WindowEventCallback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            WindowChanged?.Invoke(null, new WindowChangedEventArgs {WindowTitle = GetActiveWindowTitle(hwnd), WindowId = (uint) hwnd});
        }

        private static string GetActiveWindowTitle(IntPtr hwnd)
        {
            var len = GetWindowTextLength(hwnd) + 1;
            var buff = new StringBuilder(len);
            return GetWindowText(hwnd, buff, len) > 0 ? buff.ToString() : null;
        }
    }
}
