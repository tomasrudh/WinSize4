using System.Runtime.InteropServices;
using System.Text;
using hWnd = System.IntPtr;

namespace WinSize4
{
    public class ClsWindowGetter
    {
        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static IDictionary<hWnd, string> GetOpenWindows()
        {
            hWnd shellWindow = GetShellWindow();
            Dictionary<hWnd, string> windows = new Dictionary<hWnd, string>();

            EnumWindows(delegate (hWnd hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);

                windows[hWnd] = builder.ToString();
                return true;

            }, 0);

            return windows;
        }

        private delegate bool EnumWindowsProc(hWnd hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(hWnd hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(hWnd hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(hWnd hWnd);

        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();
    }

}
