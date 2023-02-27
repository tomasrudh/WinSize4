using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WinSize4
{
    public class ClsSavedScreens
    {
        public List<ClsScreenList> ScreenList = new List<ClsScreenList>();
        private string _path = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";
        private string _fileNameWindows = "Screens.json";

        //**********************************************
        /// <summary> Get index in WindowProps for the supplied values </summary>
        //**********************************************
        public int GetIndexForWindow(ClsWindowProps WindowProps)
        {
            int index = -1;
            for (int i = 0; i < this.ScreenList.Count; i++)
            {
                if (this.ScreenList[i].BoundsWidth == WindowProps.MonitorBoundsWidth &&
                    this.ScreenList[i].BoundsHeight == WindowProps.MonitorBoundsHeight &&
                    this.ScreenList[i].Primary == WindowProps.Primary)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        //**********************************************
        /// <summary> Get index in WindowProps for the supplied values </summary>
        //**********************************************
        public int GetIndexForScreen(ClsScreenList ScreenList)
        {
            int index = -1;
            for (int i = 0; i < this.ScreenList.Count; i++)
            {
                if (this.ScreenList[i].BoundsWidth == ScreenList.BoundsWidth &&
                    this.ScreenList[i].BoundsHeight == ScreenList.BoundsHeight &&
                    this.ScreenList[i].Primary == ScreenList.Primary)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        //**********************************************
        /// <summary> Saves data to disk </summary>
        //**********************************************
        public void Save()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            Directory.CreateDirectory(_path);
            using (var writer = new StreamWriter(_path + "\\" + _fileNameWindows))
            {
                String _json = JsonSerializer.Serialize(this.ScreenList, options);
                writer.Write(_json);
            }
        }

        //**********************************************
        /// <summary> Loads data from disk </summary>
        //**********************************************
        public void Load()
        {
            if (File.Exists(_path + "\\" + _fileNameWindows))
            {
                using (StreamReader r = new StreamReader(_path + "\\" + _fileNameWindows))
                {
                    String json = r.ReadToEnd();
                    this.ScreenList = JsonSerializer.Deserialize<List<ClsScreenList>>(json);
                }
            }
        }

    }
}
