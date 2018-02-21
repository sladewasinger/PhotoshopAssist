using System;
using System.Runtime.InteropServices;

namespace PhotoshopBeepFix
{
    public static class WindowHandler
    {
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

		public static bool IsWindowClosed(IntPtr ptr)
        {
            return ptr == IntPtr.Zero;
        }

		public static bool IsWindowInFocus(IntPtr ptr)
        {
            return GetForegroundWindow() == ptr && !IsWindowClosed(ptr);
        }
    }
}
