using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace WinSize4
{
    public static class ClsDebug
    {
        public static bool Debug = false;
        public static string _text = "";
        private static string _activePath; // Will be set by frmMain at startup

        //**********************************************
        /// <summary>
        /// Sets the active directory for logging. Must be called once at startup.
        /// </summary>
        //**********************************************
        public static void Initialize(string path)
        {
            _activePath = path;
        }

        //**********************************************
        /// <summary>
        /// Checks if the path has been set. Prevents crashes.
        /// </summary>
        //**********************************************
        private static bool IsPathInitialized()
        {
            if (string.IsNullOrEmpty(_activePath))
            {
                // Fallback to prevent crashes if Initialize() is not called.
                // In a properly functioning app, this should not be hit.
                _activePath = Path.GetDirectoryName(Application.ExecutablePath);
            }
            return true;
        }

        public static void ClearLog()
        {
            if (!IsPathInitialized()) return;
            Directory.CreateDirectory(_activePath);
            string fullPath = Path.Combine(_activePath, "Debug.txt");
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            _text = "";
        }

        public static void AddText(string Text)
        {
            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            _text += dt + " " + Text + "\n";
        }

        public static void LogText()
        {
            if (!Debug || !IsPathInitialized()) return;
            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Directory.CreateDirectory(_activePath);
            string fullPath = Path.Combine(_activePath, "Debug.txt");
            using (var writer = new StreamWriter(fullPath, true))
            {
                writer.WriteLine(dt + " " + _text);
            }
            _text = "";
        }

        public static void LogNow(string Text)
        {
            if (!Debug || !IsPathInitialized()) return;
            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Directory.CreateDirectory(_activePath);
            string fullPath = Path.Combine(_activePath, "Debug.txt");
            using (var writer = new StreamWriter(fullPath, true))
            {
                writer.WriteLine(dt + " " + Text);
            }
        }

        public static void LogToEvent(Exception ex, EventLogEntryType Type, string text)
        {
            if (ex.StackTrace == null)
            {
                try
                {
                    EventLog.WriteEntry("WinSize4", text, Type, 1);
                }
                catch
                (Exception ex2)
                {
                    EventLog.WriteEntry("Application", text, Type, 1);
                }
                if (Debug)
                {
                    Console.WriteLine(text);
                }
            }
            else
            {
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                int line = frame.GetFileLineNumber();
                EventLog.WriteEntry("WinSize4", ex.Message + "\n" + st.ToString() + text, Type, 1);
                if (Debug)
                {
                    Console.WriteLine(ex.Message + "\n" + st.ToString() + "\n" + text);
                }
            }
        }
    }
}
