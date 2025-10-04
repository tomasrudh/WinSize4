using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Windows.ApplicationModel.Activation;

namespace WinSize4
{
    public class ClsScreens
    {
        public List<ClsScreenList> ScreenList = new List<ClsScreenList>();
        private string _path = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";
        private string _fileNameWindows = "Screens.json";

        //**********************************************
        /// <summary> Get all current screens </summary>
        /// <returns>True if a screen is added</returns>
        //**********************************************
        public bool AddNewScreens()
        {
            bool Added = false;
            foreach (Screen screen in Screen.AllScreens)
            {
                bool found = false;
                foreach (ClsScreenList savedScreen in ScreenList)
                {
                    if (savedScreen.BoundsWidth == screen.Bounds.Width &&
                        savedScreen.BoundsHeight == screen.Bounds.Height &&
                        savedScreen.Primary == screen.Primary)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ClsScreenList _newScreen = new ClsScreenList
                    {
                        BoundsWidth = screen.Bounds.Width,
                        BoundsHeight = screen.Bounds.Height,
                        WorkingAreaWidth = screen.WorkingArea.Width,
                        WorkingAreaHeight = screen.WorkingArea.Height,
                        CustomLeft = screen.WorkingArea.Left,
                        CustomTop = screen.WorkingArea.Top,
                        CustomWidth = screen.WorkingArea.Width,
                        CustomHeight = screen.WorkingArea.Height,
                        X = screen.Bounds.X,
                        Y = screen.Bounds.Y,
                        Primary = screen.Primary
                    };
                    ClsDebug.LogNow("AddNewScreens: New screen added " + screen.Bounds.Width + " " + screen.Bounds.Height + " " + screen.Primary);
                    this.ScreenList.Add(_newScreen);
                    Added = true;
                }
            }
            return Added;
        }

        //**********************************************
        /// <summary> Get index in ScreenList for the supplied ClsCurrentWindowProps </summary>
        //**********************************************
        public int GetScreenIndexForWindow(ClsWindowProps Props)
        {
            int index = -1;
            for (int i = 0; i < this.ScreenList.Count; i++)
            {
                if (this.ScreenList[i].BoundsWidth == Props.MonitorBoundsWidth &&
                    this.ScreenList[i].BoundsHeight == Props.MonitorBoundsHeight &&
                    this.ScreenList[i].Primary == Props.Primary)
                {
                    index = i;
                    break;
                }
            }
            ClsDebug.AddText("GetScreenIndexForWindow: " + index);
            return index;
        }

        //**********************************************
        /// <summary>Marks screens as present</summary>
        /// <returns>True if screen presence has changed</returns>
        //**********************************************
        public bool SetPresent()
        {
            bool Changed = false;
            Screen[] AllScreens = System.Windows.Forms.Screen.AllScreens;
            //foreach (ClsScreenList Screen in this.ScreenList)
            for (int i = 0; i < this.ScreenList.Count; i++)
            {
                //foreach (Screen CurrentScreen in AllScreens)
                bool Found = false;
                for (int j = 0; j < AllScreens.Length; j++)
                {
                    if (this.ScreenList[i].BoundsWidth == AllScreens[j].Bounds.Width &&
                        this.ScreenList[i].BoundsHeight == AllScreens[j].Bounds.Height &&
                        this.ScreenList[i].Primary == AllScreens[j].Primary)
                    {
                        Found = true;
                        break;
                    }
                }
                if (Found)
                {
                    if (this.ScreenList[i].Present == false)
                    {
                        Changed = true;
                        ClsDebug.LogNow("SetPresent: Screen " + i + " is now present");
                    }
                    this.ScreenList[i].Present = true;
                }
                else
                {
                    if (this.ScreenList[i].Present == true)
                    {
                        Changed = true;
                        ClsDebug.LogNow("SetPresent: Screen " + i + " is no longer present");
                    }
                    this.ScreenList[i].Present = false;
                }
            }
            return Changed;
        }

        //**********************************************
        /// <summary>Cleans out Screen no longer available</summary>
        /// <returns>True if at least one Screen was deleted</returns>
        //**********************************************
        public bool CleanScreenList()
        {
            AddNewScreens();
            bool Deleted = false;
            Screen[] CurrentScreens = Screen.AllScreens;
            //foreach (ClsScreenList ListScr in this.ScreenList)
            for (int i = 0; i < this.ScreenList.Count; i++)
            {
                bool Found = false;
                foreach (Screen CurScr in CurrentScreens)
                {
                    if (this.ScreenList[i].BoundsWidth == CurScr.Bounds.Width &&
                        this.ScreenList[i].BoundsHeight == CurScr.Bounds.Height &&
                        this.ScreenList[i].Primary == CurScr.Primary)
                    {
                        Found = true;
                    }
                }
                if (!Found)
                {
                    ClsDebug.LogNow("CleanScreenList: Screen deleted " + this.ScreenList[i].BoundsWidth + " " + this.ScreenList[i].BoundsHeight + "" + this.ScreenList[i].Primary);
                    this.ScreenList.RemoveAt(i);
                    Deleted = true;
                }
            }
            return Deleted;
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
