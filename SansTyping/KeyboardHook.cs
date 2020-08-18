using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Hook.WinAPI;

namespace Hook {
    public delegate bool KeyboardEventCallback(int vkCode);
    public static class KeyboardHook {
        public static bool GetSystemKeyEvent { get; set; } = true;
        public static event KeyboardEventCallback KeyDown;
        public static event KeyboardEventCallback KeyUp;

        private static readonly LowLevelProc _proc;
        private static IntPtr _hookID = IntPtr.Zero;

        static KeyboardHook() {
            _proc = HookCallback;
        }

        static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0) {
                var hookStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);

                int vkCode = hookStruct.vkCode;
                int flags = hookStruct.flags;

                if (wParam == (IntPtr)WM_KEYDOWN
                    || (GetSystemKeyEvent && wParam == (IntPtr)WM_SYSKEYDOWN))
                    if (KeyDown?.Invoke(vkCode) == false)
                        return (IntPtr)1;

                if (wParam == (IntPtr)WM_KEYUP
                    || (GetSystemKeyEvent && wParam == (IntPtr)WM_SYSTEMKEYUP))
                    if (KeyUp?.Invoke(vkCode) == false)
                        return (IntPtr)1;
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public static void HookStart() {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule) {
                _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle("user32"), 0);
            }
        }

        public static void HookEnd() {
            UnhookWindowsHookEx(_hookID);
        }
    }
}