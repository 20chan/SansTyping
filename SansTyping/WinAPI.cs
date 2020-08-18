using System;
using System.Runtime.InteropServices;

namespace Hook {
    internal static class WinAPI {
        internal const int WH_MOUSE_LL = 14;
        internal const int WM_LBUTTONDOWN = 0x0201;
        internal const int WM_LBUTTONUP = 0x0202;
        internal const int WM_MOUSEMOVE = 0x0200;
        internal const int WM_MOUSEWHEEL = 0x020A;
        internal const int WM_RBUTTONDOWN = 0x0204;
        internal const int WM_RBUTTONUP = 0x0205;

        internal const int WH_KEYBOARD_LL = 13;
        internal const int WM_KEYDOWN = 0x0100;
        internal const int WM_KEYUP = 0x101;
        internal const int WM_SYSTEMKEYUP = 0x0105;
        internal const int WM_SYSKEYDOWN = 0x104;

        public const uint LBDOWN = 0x00000002;
        public const uint LBUP = 0x00000004;
        public const uint RBDOWN = 0x00000008;
        public const uint RBUP = 0x000000010;
        public const uint MBDOWN = 0x00000020;
        public const uint MBUP = 0x000000040;
        public const uint WHEEL = 0x00000800;

        internal struct KBDLLHOOKSTRUCT {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        internal delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")]
        internal static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("User32.dll")]
        public static extern void keybd_event(uint vk, uint scan, uint flags, uint extraInfo);
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);
    }
}