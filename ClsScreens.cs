using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Windows.ApplicationModel.Activation;

namespace WinSize4
{
    public class ClsScreens
    {
        //**********************************************
        /// <summary> AddWindow Screen to list of screens </summary>
        /// <param name="screen"></param>
        //**********************************************
        public void Add(ClsScreenList screen)
        {
            this.ScreenList.Add(screen);
        }

        //**********************************************
        /// <summary> Get all current screens </summary>
        //**********************************************
        public void GetScreens()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                bool found = false;
                foreach (ClsScreenList savedScreen in ScreenList)
                {
                    if (savedScreen.BoundsWidth == screen.Bounds.Width && savedScreen.BoundsHeight == screen.Bounds.Height && savedScreen.Primary == screen.Primary)
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
                    this.ScreenList.Add(_newScreen);
                }
            }
        }

        public List<ClsScreenList> ScreenList = new List<ClsScreenList>();
        private string _path = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";
        private string _fileNameWindows = "Screens.json";

        //**********************************************
        /// <summary> Get index in ScreenList for the supplied ClsWindowProps </summary>
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
        /// <summary> Get index in ScreenList for the supplied ClsScreenList </summary>
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
        /// <summary>Marks screens as present</summary>
        //**********************************************
        public void SetPresent()
        {
            foreach (ClsScreenList Screen in this.ScreenList)
            {
                foreach (Screen CurrentScreen in System.Windows.Forms.Screen.AllScreens)
                {
                    if (Screen.BoundsWidth == CurrentScreen.Bounds.Width &&
                        Screen.BoundsHeight == CurrentScreen.Bounds.Height &&
                        Screen.Primary == CurrentScreen.Primary)
                    {
                        Screen.Present = true;
                    }
                    else
                    {
                        Screen.Present = false;
                    }
                }
            }
        }

        //**********************************************
        /// <summary>Cleans out Screen no longer available</summary>
        /// <returns>True if at least one Screen was deleted</returns>
        //**********************************************
        public bool CleanScreenList()
        {
            GetScreens();
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
