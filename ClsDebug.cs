using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace WinSize4
{
    public static class ClsDebug
    {
        public static bool Debug = false;
        public static string _text = "";

        public static void ClearLog()
        {
            string _path = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";
            Directory.CreateDirectory(_path);
            string _FileName = "Debug.txt";
            if (File.Exists(Path.Combine(_path, _FileName)))
            {
                File.Delete(Path.Combine(_path, _FileName));
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
            string _path = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";
            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Directory.CreateDirectory(_path);
            string _FileName = "Debug.txt";
            if (Debug)
            {
                using (var writer = new StreamWriter(_path + "\\" + _FileName, true))
                {
                    writer.WriteLine(dt + " " + _text);
                }
            }
            _text = "";
        }

        public static void LogNow(string Text)
        {
            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            string _path = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";
            Directory.CreateDirectory(_path);
            string _FileName = "Debug.txt";
            if (Debug)
            {
                using (var writer = new StreamWriter(_path + "\\" + _FileName, true))
                {
                    writer.WriteLine(dt + " " + Text);
                }
            }
        }

        public static void LogToEvent(Exception ex, EventLogEntryType Type, string text)
        {
            if (ex.StackTrace == null)
            {
                EventLog.WriteEntry("WinSize4", text, Type, 1);
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
