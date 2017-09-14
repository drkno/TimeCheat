using System;
using System.Runtime.InteropServices;

namespace TimeSaver.Abstractions.Windows
{
    internal class WindowsEventWaiter : IEventWaiter
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Msg
        {
            // ReSharper disable MemberCanBePrivate.Local
            public readonly IntPtr Hwnd;
            public readonly uint Message;
            public readonly IntPtr WParam;
            public readonly IntPtr LParam;
            public readonly uint Time;
            public readonly IntPtr Point;
            // ReSharper restore MemberCanBePrivate.Local
        }
        
        const uint PmRemove = 1;
        const uint WmQuit = 0x0012;

        [DllImport("user32.dll")]
        private static extern bool PeekMessage(out Msg lpMsg, IntPtr hwnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("user32.dll")]
        private static extern bool TranslateMessage(ref Msg lpMsg);

        [DllImport("user32.dll")]
        private static extern IntPtr DispatchMessage(ref Msg lpMsg);

        public void WaitUntilQuit()
        {
            while (true)
            {
                if (PeekMessage(out Msg msg, IntPtr.Zero, 0, 0, PmRemove))
                {
                    if (msg.Message == WmQuit)
                        break;

                    TranslateMessage(ref msg);
                    DispatchMessage(ref msg);
                }
            }
        }
    }
}
