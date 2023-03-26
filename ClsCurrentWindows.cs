using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinSize4
{

    public class ClsCurrentWindows
    {
        public List<ClsWindows> Windows = new();

        //**********************************************
        /// <summary> Adds hWnd </summary>
        /// <param name="hWnd"></param>
        //**********************************************
        public void Add(long hWnd)
        {
            this.Windows.Add(new ClsWindows());
            this.Windows[this.Windows.Count - 1].hWnd = hWnd;
            GetWindowThreadProcessId((IntPtr)hWnd, out int Pid);
            this.Windows[this.Windows.Count - 1].Pid = Pid;
            UpdateWindowProperties(this.Windows.Count - 1);
        }

        //**********************************************
        /// <summary> Finds index to the first window with the supplied hWnd, or -1 </summary>
        /// <param name="hWnd"></param>
        /// <returns>currentWindowIndex</returns>
        //**********************************************
        public int GetIndexForhWnd(long hWnd)
        {
            int foundIndex = -1;
            for (int index = 0; index < this.Windows.Count; index++)
            {
                if (this.Windows[index].hWnd == hWnd)
                {
                    foundIndex = index;
                    break;
                }
            }
            return foundIndex;
        }

        //**********************************************
        /// <summary> Finds index to the first window with the supplied Pid and another Hwnd </summary>
        /// <param name="Pid"></param>
        /// <param name="hWnd"></param>
        /// <returns>currentWindowIndex, or -1</returns>
        //**********************************************
        public int GetIndexForPid(int Pid, long hWnd)
        {
            int foundIndex = -1;
            for (int index = 0; index < this.Windows.Count; index++)
            {
                if (this.Windows[index].Pid == Pid && this.Windows[index].hWnd != hWnd)
                {
                    foundIndex = index;
                    break;
                }
            }
            return foundIndex;
        }

        //**********************************************
        /// <summary> Removes closed this.Windows </summary>
        //**********************************************
        public void CleanWindowsList()
        {
            var windowHandles = new List<IntPtr>();
            foreach (KeyValuePair<IntPtr, string> window in ClsWindowGetter.GetOpenWindows())
            {
                windowHandles.Add(window.Key);
            }
            for (int i = 0; i < this.Windows.Count; i++)
            {
                if (!windowHandles.Contains((IntPtr)this.Windows[i].hWnd))
                {
                    this.Windows.RemoveAt(i);
                }
            }
        }

        //**********************************************
        /// <summary> Removes closed this.Windows </summary>
        /// Takes long time!
        //**********************************************
        public void CleanWindowsListX()
        {
            try
            {
                var windowHandles = new List<IntPtr>();
                foreach (Process _window in Process.GetProcesses())
                {
                    _window.Refresh();
                    if (_window.MainWindowHandle != IntPtr.Zero)
                    {
                        windowHandles.Add(_window.MainWindowHandle);
                    }
                }
                for (int i = 0; i < this.Windows.Count; i++)
                {
                    if (!windowHandles.Contains((IntPtr)this.Windows[i].hWnd))
                    {
                        this.Windows.RemoveAt(i);
                    }
                }
            }
            catch { }
        }

        //**********************************************
        /// <summary>Updates properties for the supplied window index</summary>
        /// <param name="Index"></param>
        //**********************************************
        public void UpdateWindowProperties(int Index)
        {
            try
            {
                var Props = new ClsWindowProps();
                Props = GetWindowProperties(this.Windows[Index].hWnd);
                this.Windows[Index].Props = Props;
                this.Windows[Index].Present = true;
                //this.Windows[Index].Moved = false;
            }
            catch
            (Exception ex)
            {
                EventLog.WriteEntry("WinSize4", ex.Message, EventLogEntryType.Error, 1);
            }
        }

        //**********************************************
        /// <summary>Updates properties for the supplied window index</summary>
        /// <param name="hWnd"></param>
        /// <returns>ClsWindowProps</returns>
        //**********************************************
        public ClsWindowProps GetWindowProperties(long hWnd)
        {
            ClsWindowProps Props = new();
            try
            {
                StringBuilder ClassName = new(256);
                GetClassName((IntPtr)hWnd, ClassName, ClassName.Capacity);
                Props.WindowClass = ClassName.ToString();

                Screen screen = Screen.FromHandle((IntPtr)hWnd);
                Props.MonitorBoundsWidth = screen.Bounds.Width;
                Props.MonitorBoundsHeight = screen.Bounds.Height;
                Props.Primary = screen.Primary;

                int length = GetWindowTextLength((IntPtr)hWnd);
                var builder = new StringBuilder(length);
                GetWindowText((IntPtr)hWnd, builder, length + 1);
                Props.Title = builder.ToString();

                //Props.Name = screen.Bounds.Width + "x" + screen.Bounds.Height + ((screen.Primary) ? "P" : "") + " - " + Props.Title;
                Props.Name = Props.Title;

                var rect = new Rectangle();
                GetWindowRect((IntPtr)hWnd, ref rect);

                Props.Top = rect.Top;
                Props.Left = rect.Left;

                Props.Width = rect.Width - rect.Left;
                Props.Height = rect.Height - rect.Top;

                GetWindowThreadProcessId((IntPtr)hWnd, out int ProcIdXL);
                Process xproc = Process.GetProcessById(ProcIdXL);
                if (xproc is null)
                {
                    Props.Exe = "";
                }
                else
                {
                    Props.Exe = xproc.ProcessName;
                }
            }
            catch
            (Exception ex)
            {
                EventLog.WriteEntry("WinSize4", ex.Message, EventLogEntryType.Error, 2);
            }
            return Props;
        }

        //**********************************************
        /// <summary> Moves window to the matching saved window, run this method on ClsSavedWindows </summary>
        /// <param name="SavedWindow">The window to use for location</param>
        /// <param name=""></param>
        //**********************************************
        public void MoveCurrentWindow(int currentWindowIndex, ClsWindowProps savedWindowProps, ClsScreenList savedScreenProps, ClsScreenList currentScreenProps)
        {
            WINDOWPLACEMENT wp = new();
            GetWindowPlacement((IntPtr)this.Windows[currentWindowIndex].hWnd, ref wp);
            wp.showCmd = 1; // 1- Normal; 2 - Minimize; 3 - Maximize;
            if (savedWindowProps.FullScreen)
            {
                wp.showCmd = 3; // 1- Normal; 2 - Minimize; 3 - Maximize;
                SetWindowPlacement((IntPtr)this.Windows[currentWindowIndex].hWnd, ref wp);
            }
            else
            {
                if (!savedWindowProps.MaxWidth && !savedWindowProps.MaxHeight)
                {
                    SetWindowPlacement((IntPtr)this.Windows[currentWindowIndex].hWnd, ref wp);
                    SetWindowPos((IntPtr)this.Windows[currentWindowIndex].hWnd, savedScreenProps.X, savedWindowProps.Left, savedWindowProps.Top, savedWindowProps.Width, savedWindowProps.Height, 0);
                }

                if (savedWindowProps.MaxWidth && !savedWindowProps.MaxHeight)
                {
                    SetWindowPlacement((IntPtr)this.Windows[currentWindowIndex].hWnd, ref wp);
                    MoveWindow((IntPtr)this.Windows[currentWindowIndex].hWnd, 0, savedWindowProps.Top, savedScreenProps.CustomWidth, savedWindowProps.Height, true);
                }

                if (!savedWindowProps.MaxWidth && savedWindowProps.MaxHeight)
                {
                    SetWindowPlacement((IntPtr)this.Windows[currentWindowIndex].hWnd, ref wp);
                    MoveWindow((IntPtr)this.Windows[currentWindowIndex].hWnd, savedWindowProps.Left, savedScreenProps.Y, savedWindowProps.Width, savedScreenProps.CustomHeight, true);
                }

                if (savedWindowProps.MaxWidth && savedWindowProps.MaxHeight)
                {
                    SetWindowPlacement((IntPtr)this.Windows[currentWindowIndex].hWnd, ref wp);
                    MoveWindow((IntPtr)this.Windows[currentWindowIndex].hWnd, currentScreenProps.X, currentScreenProps.Y, savedScreenProps.CustomWidth, savedScreenProps.CustomHeight, true);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public uint length;
            public uint flags;
            public uint showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public Rectangle rcNormalPosition;
            public Rectangle rcDevice;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        public void ResetMoved()
        {
            foreach (ClsWindows Win in this.Windows)
            {
                Win.Moved = false;
            }
        }

        //**********************************************
        // API calls
        //**********************************************

        [DllImport("user32.dll")]

        static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("USER32.DLL")]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_NOACTIVATE = 0x0010;
    }
}