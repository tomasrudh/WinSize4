using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinSize4
{
    public class ClsSettings
    {
        //public ClsSettingsList SettingsList = new ClsSettingsList();
        public string HotKeyLeft = "alt";
        public string HotKeyRight = "ctrl";
        public string HotKeyCharacter = "Z";
        public bool showAllWindows = true;
        public bool resetIfNewScreen = false;
        public bool runAtLogin = false;
        public string dataPath = "";
        private string _fileNameWindows = "Settings.json";
        public bool Debug = false;
        public int Interval = 500;
        public bool isPaused = false;

        public ClsSettings()
        {
            dataPath = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";
            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\data"))
            {
                dataPath = Directory.GetCurrentDirectory() + "\\data";
            }
            //ClsDebug.ClearLog(dataPath);
            //ClsDebug.LogNow(dataPath, "\nData directory is set to: " + dataPath);
            ClsDebug.LogToEvent(new Exception(), EventLogEntryType.Information, "\nData directory is set to: " + dataPath);
        }

        //**********************************************
        /// <summary> Saves data to disk </summary>
        //**********************************************
        public void SaveToFile()
        {
            ClsSettingsList saveList = new ClsSettingsList();
            saveList.HotKeyCharacter = this.HotKeyCharacter;
            saveList.HotKeyLeft = this.HotKeyLeft;
            saveList.HotKeyRight = this.HotKeyRight;
            saveList.showAllWindows = this.showAllWindows;
            saveList.resetIfNewScreen = this.resetIfNewScreen;
            saveList.runAtLogin = this.runAtLogin;
            saveList.Debug = ClsDebug.Debug;
            saveList.Interval = this.Interval;
            saveList.isPaused = this.isPaused;

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            Directory.CreateDirectory(dataPath);
            using (var writer = new StreamWriter(dataPath + "\\" + _fileNameWindows))
            {
                String _json = JsonSerializer.Serialize(saveList, options);
                writer.Write(_json);
            }
        }

        //**********************************************
        /// <summary> Loads data from disk </summary>
        //**********************************************
        public void LoadFromFile()
        {
            try
            {
                ClsSettingsList saveList = new ClsSettingsList();

                if (File.Exists(dataPath + "\\" + _fileNameWindows))
                {
                    using (StreamReader r = new StreamReader(dataPath + "\\" + _fileNameWindows))
                    {
                        String json = r.ReadToEnd();
                        saveList = JsonSerializer.Deserialize<ClsSettingsList>(json);
                    }
                    this.HotKeyCharacter = saveList.HotKeyCharacter;
                    this.HotKeyLeft = saveList.HotKeyLeft;
                    this.HotKeyRight = saveList.HotKeyRight;
                    this.showAllWindows = saveList.showAllWindows;
                    this.resetIfNewScreen = saveList.resetIfNewScreen;
                    this.runAtLogin = saveList.runAtLogin;
                    ClsDebug.Debug = saveList.Debug;
                    if (saveList.Interval > 0)
                        this.Interval = saveList.Interval;
                    this.isPaused = saveList.isPaused;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "WinSize4", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public int GetHotKeyNumber(string HotKey)
        {
            switch (HotKey)
            {
                case "Alt":
                    return 1;
                case "Ctrl":
                    return 2;
                case "Shift":
                    return 4;
                default:
                    return 0;
            }
        }
        public string GetHotKeyText(int HotKey)
        {
            switch (HotKey)
            {
                case 1:
                    return "Alt";
                case 2:
                    return "Ctrl";
                case 4:
                    return "Shift";
                default:
                    return "None";
            }
        }
    }

    public class ClsSettingsList
    {
        public string HotKeyLeft
        { set; get; }
        public string HotKeyRight
        { set; get; }
        public string HotKeyCharacter
        { set; get; }
        public bool showAllWindows
        { set; get; }
        public bool resetIfNewScreen
        { set; get; }
        public bool runAtLogin
        { set; get; }
        public bool Debug
        { set; get; }
        public int Interval
        { set; get; }
        public bool isPaused
        { set; get; }
    }
}