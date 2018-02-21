using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoshopBeepFix
{
    public class KeyboardHookController : IDisposable
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static IntPtr _hookID = IntPtr.Zero;
        private GlobalKeyboardHook _globalKeyboardHook;

        private Action<Keys> _keyDownCallback;
        private Action<Keys> _keyUpCallback;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public KeyboardHookController(Action<Keys> keyDownCallback, Action<Keys> keyUpCallback)
        {
            _keyDownCallback = keyDownCallback;
            _keyUpCallback = keyUpCallback;
            _globalKeyboardHook = new GlobalKeyboardHook();

            _hookID = _globalKeyboardHook.SetHook(HookCallback);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    _keyDownCallback((Keys)vkCode);
                }
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    _keyUpCallback((Keys)vkCode);
                }
            }
            return GlobalKeyboardHook.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            GlobalKeyboardHook.UnhookWindowsHookEx(_hookID);
        }
    }
}