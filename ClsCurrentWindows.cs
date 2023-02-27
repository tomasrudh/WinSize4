using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WinSize4
{

    public class ClsCurrentWindows
    {
        public List<ClsWindows> Windows = new List<ClsWindows>();

        //**********************************************
        /// <summary> Finds index to the first window with the supplied hWnd, or -1 </summary>
        /// <param name="hWnd"></param>
        /// <returns>currentWindowIndex</returns>
        //**********************************************
        public int GetIndexForhWnd(long hWnd)
        {
            int index;
            for (index = 0; index < this.Windows.Count; index++)
            {
                if (this.Windows[index].hWnd == hWnd)
                    break;
            }
            if (index == this.Windows.Count)
                index = -1;
            return index;
        }

        //**********************************************
        /// <summary> Adds hWnd </summary>
        /// <param name="hWnd"></param>
        //**********************************************
        public void Add(long hWnd)
        {
            this.Windows.Add(new ClsWindows());
            this.Windows[this.Windows.Count - 1].hWnd = hWnd;
            UpdateWindowProperties(this.Windows.Count - 1);
        }

        //**********************************************
        /// <summary> Sets the property Moved to True for all windows </summary>
        /// <param name="hWnd"></param>
        //**********************************************
        public void SetMoved(long hWnd)
        {
            for (int i = 0; i < this.Windows.Count; i++)
            {
                if (this.Windows[i].hWnd == hWnd)
                {
                    this.Windows[i].Moved = true;
                    break;
                }
            }
        }

        //**********************************************
        /// <summary> Removes closed this.Windows, to be used in Currentthis.Windows </summary>
        //**********************************************
        public void CleanWindowsList()
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
                var props = new ClsWindowProps();
                //int currentWindowIndex = GetIndexForhWnd(hWnd);

                Screen screen = Screen.FromHandle((IntPtr)this.Windows[Index].hWnd);
                props.MonitorBoundsWidth = screen.Bounds.Width;
                props.MonitorBoundsHeight = screen.Bounds.Height;
                props.Primary = screen.Primary;

                int length = GetWindowTextLength((IntPtr)this.Windows[Index].hWnd);
                var builder = new StringBuilder(length);
                GetWindowText((IntPtr)this.Windows[Index].hWnd, builder, length + 1);
                props.Title = builder.ToString();

                //props.Name = screen.Bounds.Width + "x" + screen.Bounds.Height + ((screen.Primary) ? "P" : "") + " - " + props.Title;
                props.Name = props.Title;

                var rect = new Rectangle();
                GetWindowRect((IntPtr)this.Windows[Index].hWnd, ref rect);

                props.Top = rect.Top;
                props.Left = rect.Left;

                props.Width = rect.Width - rect.Left;
                props.Height = rect.Height - rect.Top;

                GetWindowThreadProcessId((IntPtr)this.Windows[Index].hWnd, out int ProcIdXL);
                Process xproc = Process.GetProcessById(ProcIdXL);
                if (xproc is null)
                {
                    props.Exe = "";
                }
                else
                {
                    props.Exe = xproc.ProcessName;
                }
                this.Windows[Index].Props = props;
                this.Windows[Index].Present = true;
                this.Windows[Index].Moved = false;
            }
            catch
            (Exception ex)
            { }
        }

        //**********************************************
        /// <summary> Moves window to the matching saved window, run this method on ClsSavedWindows </summary>
        /// <param name="SavedWindow">The window to use for location</param>
        /// <param name=""></param>
        //**********************************************
        public void MoveCurrentWindow(int currentWindowIndex, ClsWindowProps savedWindowProps, ClsScreenList savedScreenProps, ClsScreenList currentScreenProps)
        {
            if (!savedWindowProps.MaxWidth && !savedWindowProps.MaxHeight)
                //MoveWindow((IntPtr)this.Windows[currentWindowIndex].hWnd, savedWindowProps.Left, savedWindowProps.Top, savedWindowProps.Width, savedWindowProps.Height, true);
                SetWindowPos((IntPtr)this.Windows[currentWindowIndex].hWnd, savedScreenProps.X, savedWindowProps.Left, savedWindowProps.Top, savedWindowProps.Width, savedWindowProps.Height, 0);

            if (savedWindowProps.MaxWidth && !savedWindowProps.MaxHeight)
                MoveWindow((IntPtr)this.Windows[currentWindowIndex].hWnd, 0, savedWindowProps.Top, savedScreenProps.CustomWidth, savedWindowProps.Height, true);

            if (!savedWindowProps.MaxWidth && savedWindowProps.MaxHeight)
                MoveWindow((IntPtr)this.Windows[currentWindowIndex].hWnd, savedWindowProps.Left, savedScreenProps.Y, savedWindowProps.Width, savedScreenProps.CustomHeight, true);

            if (savedWindowProps.MaxWidth && savedWindowProps.MaxHeight)
                MoveWindow((IntPtr)this.Windows[currentWindowIndex].hWnd, currentScreenProps.X, currentScreenProps.Y, savedScreenProps.CustomWidth, savedScreenProps.CustomHeight, true);
            //SetWindowPos((IntPtr)this.Windows[currentWindowIndex].hWnd, 0, 0, 0, -100, -100, 0X4 | 1 | 0x0010);

            if (savedWindowProps.FullScreen)
            {
                WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
                GetWindowPlacement((IntPtr)this.Windows[currentWindowIndex].hWnd, ref wp);
                wp.showCmd = 3; // 1- Normal; 2 - Minimize; 3 - Maximize;
                SetWindowPlacement((IntPtr)this.Windows[currentWindowIndex].hWnd, ref wp);
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

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_NOACTIVATE = 0x0010;
    }
}