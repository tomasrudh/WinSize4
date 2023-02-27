using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

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
        private string _path = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";
        private string _fileNameWindows = "Settings.json";

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

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            Directory.CreateDirectory(_path);
            using (var writer = new StreamWriter(_path + "\\" + _fileNameWindows))
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

                if (File.Exists(_path + "\\" + _fileNameWindows))
                {
                    using (StreamReader r = new StreamReader(_path + "\\" + _fileNameWindows))
                    {
                        String json = r.ReadToEnd();
                        saveList = JsonSerializer.Deserialize<ClsSettingsList>(json);
                    }
                    this.HotKeyCharacter = saveList.HotKeyCharacter;
                    this.HotKeyLeft = saveList.HotKeyLeft;
                    this.HotKeyRight = saveList.HotKeyRight;
                    this.showAllWindows = saveList.showAllWindows;
                    this.resetIfNewScreen = saveList.resetIfNewScreen;
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
    }
}
