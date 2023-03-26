using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WinSize4;

public class ClsSavedWindows
{
    public List<ClsWindowProps> Props = new List<ClsWindowProps>();
    private string _path = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";
    private int _nextTag = 0;

    //**********************************************
    /// <summary> Adds a new window </summary>
    /// <param name="Props"></param>
    /// <returns>Tag</returns>
    //**********************************************
    public int AddWindow(ClsWindowProps Props)
    {
        this.Props.Add(Props);
        this.Props[this.Props.Count - 1].Tag = _nextTag++;
        if (this.Props[this.Props.Count - 1].Exe == "explorer")
            this.Props[this.Props.Count - 1].IgnoreChildWindows = false;
        return _nextTag;
    }

    //**********************************************
    /// <summary>Sets properties for the supplied window handle</summary>
    /// <param name="hWnd"></param>
    //**********************************************
    public void SetWindowProperties(long hWnd, List<ClsScreenList> CurrentScreenList, int screenCurrentIndex)
    {
        var props = new ClsWindowProps();

        int length = GetWindowTextLength((IntPtr)hWnd);
        var builder = new StringBuilder(length);
        GetWindowText((IntPtr)hWnd, builder, length + 1);
        props.Title = builder.ToString();

        GetWindowThreadProcessId((IntPtr)hWnd, out int ProcIdXL);
        Process xproc = Process.GetProcessById(ProcIdXL);
        if (xproc is null)
        {
            props.Exe = "";
        }
        else
        {
            props.Exe = xproc.ProcessName;
        }

        int index = GetIndex(props, CurrentScreenList, screenCurrentIndex);
        if (index > -1)
        {

            Screen _screen = Screen.FromHandle((IntPtr)hWnd);
            props.MonitorBoundsWidth = _screen.Bounds.Width;
            props.MonitorBoundsHeight = _screen.Bounds.Height;

            var rect = new Rectangle();
            GetWindowRect((IntPtr)hWnd, ref rect);

            props.Top = rect.Top;
            props.Left = rect.Left;

            props.Width = rect.Width - rect.Left;
            props.Height = rect.Height - rect.Top;

            this.Props[index] = props;
        }
    }

    //**********************************************
    /// <summary>Updates properties for the supplied window properties</summary>
    /// <param name="ClsWindowProps"></param>
    /// <returns>True if a window is updated, false if window is not found</returns>
    //**********************************************

    public bool UpdateWindowProperties(ClsWindowProps Props, List<ClsScreenList> CurrentScreenList, int screenCurrentIndex)
    {
        bool Found = false;
        int Index = GetIndex(Props, CurrentScreenList, screenCurrentIndex);
        if (Index > -1)
        {
            Found = true;
            this.Props[Index].Left = Props.Left;
            this.Props[Index].Top = Props.Top;
            this.Props[Index].Width = Props.Width;
            this.Props[Index].Height = Props.Height;
            //this.Props[Index].MaxWidth = false;
            //this.Props[Index].MaxHeight = false;
            //this.Props[Index].FullScreen = false;
        }
        return Found;
    }

    //**********************************************
    /// <summary> Finds the first window with the supplied data, or -1 </summary>
    /// <param name="ClsWindowProps, ClsScreenList"></param>
    /// <returns>Index</returns>
    //**********************************************
    //public int GetIndex(ClsWindowProps CurrentWindowProps)
    public int GetIndex(ClsWindowProps CurrentWindowProps, List<ClsScreenList> CurrentScreenList, int screenCurrentIndex)
    {
        int result = -1;

        // Get list of matching saved windows
        List<ClsWindowProps> matchingSavedWindows = new List<ClsWindowProps>();
        for (int i = 0; i < this.Props.Count; i++)
        {
            bool FoundTitle = true;
            if (this.Props[i].SearchTitle)
            {
                switch (this.Props[i].SearchType)
                {
                    case ClsWindowProps.Full:
                        FoundTitle = (CurrentWindowProps.Title == this.Props[i].Title);
                        break;
                    case ClsWindowProps.Contains:
                        FoundTitle = CurrentWindowProps.Title.Contains(this.Props[i].Title);
                        break;
                    case ClsWindowProps.StartsWith:
                        FoundTitle = CurrentWindowProps.Title.StartsWith(this.Props[i].Title);
                        break;
                }
            }
            bool FoundExe = true;
            if (this.Props[i].SearchExe)
                FoundExe = (CurrentWindowProps.Exe == this.Props[i].Exe);
            bool FoundWindowClass = true;
            if (this.Props[i].ConsiderWindowClass)
                FoundWindowClass = (CurrentWindowProps.WindowClass == this.Props[i].WindowClass);

            if (FoundTitle &&
                FoundExe &&
                FoundWindowClass &&
                this.Props[i].MonitorBoundsWidth == CurrentWindowProps.MonitorBoundsWidth &&
                this.Props[i].MonitorBoundsHeight == CurrentWindowProps.MonitorBoundsHeight &&
                this.Props[i].Primary == CurrentWindowProps.Primary)
            {
                matchingSavedWindows.Add(this.Props[i]);

            }
        }

        for (int i = 0; i < matchingSavedWindows.Count; i++)
        {
            for (int j = 0; j < CurrentScreenList.Count; j++)
            {
                // Another screen than current and not primary
                if (matchingSavedWindows[i].MonitorBoundsWidth == CurrentScreenList[j].BoundsWidth &&
                    matchingSavedWindows[i].MonitorBoundsHeight == CurrentScreenList[j].BoundsHeight &&
                    matchingSavedWindows[i].Primary == CurrentScreenList[j].Primary &&
                    j != screenCurrentIndex && CurrentScreenList[j].Primary == false)
                {
                    result = i;
                    break;
                }
            }
        }
        for (int i = 0; i < matchingSavedWindows.Count; i++)
        {
            for (int j = 0; j < CurrentScreenList.Count; j++)
            {
                // Another screen than current and primary
                if (matchingSavedWindows[i].MonitorBoundsWidth == CurrentScreenList[j].BoundsWidth &&
                    matchingSavedWindows[i].MonitorBoundsHeight == CurrentScreenList[j].BoundsHeight &&
                    matchingSavedWindows[i].Primary == CurrentScreenList[j].Primary &&
                    j != screenCurrentIndex && CurrentScreenList[j].Primary == true)
                {
                    result = i;
                    break;
                }
            }
        }
        for (int i = 0; i < matchingSavedWindows.Count; i++)
        {
            for (int j = 0; j < CurrentScreenList.Count; j++)
            {
                // This screen matches
                if (matchingSavedWindows[i].MonitorBoundsWidth == CurrentScreenList[j].BoundsWidth &&
                    matchingSavedWindows[i].MonitorBoundsHeight == CurrentScreenList[j].BoundsHeight &&
                    matchingSavedWindows[i].Primary == CurrentScreenList[j].Primary &&
                    j == screenCurrentIndex)
                {
                    result = GetWindowIndexByTag(matchingSavedWindows[i].Tag);
                    break;
                }
            }
        }
        return result;
    }

    //**********************************************
    /// <summary> Get the index of the window with the supplied Tag </summary>
    /// <param name="Tag"></param>
    /// <returns>i</returns>
    //**********************************************
    public int GetWindowIndexByTag(int Tag)
    {
        for (int i = 0; i < Props.Count; i++)
        {
            if (Props[i].Tag == Tag)
                return i;
        }
        return -1;
    }

    //**********************************************
    /// <summary> Get the window with the supplied Tag </summary>
    /// <param name="Tag"></param>
    /// <returns>ClsWindowProps</returns>
    //**********************************************
    public ClsWindowProps GetWindowByTag(int Tag)
    {
        for (int i = 0; i < Props.Count; i++)
        {
            if (Props[i].Tag == Tag)
                return Props[i];
        }
        return new ClsWindowProps();
    }

    //**********************************************
    /// <summary> Finds the next available tag </summary>
    /// <returns>Tag</returns>
    //**********************************************
    private int FindNextTag()
    {
        int max = 0;
        for (int i = 0; i < this.Props.Count; i++)
        {
            if (this.Props[i].Tag > max)
                max = this.Props[i].Tag;
        }
        return max + 1;
    }

        //**********************************************
        /// <summary> Saves data to disk </summary>
        //**********************************************
        public void Save()
    {
        string fileNameWindows;
        var options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        List<List<ClsWindowProps>> saveProps = new List<List<ClsWindowProps>>();
        bool found;
        int j;

        // Remove all saved files
        var fileEntries = Directory.GetFiles(_path, "*.json");
        foreach (string fileName in fileEntries)
        {
            if (Regex.Match(fileName, @"\d{3,5}x\d{3,5}P?\.json").Success)
            {
                File.Delete(fileName);
            }
        }

        for (int i = 0; i < this.Props.Count; i++)
        {
            found = false;
            for (j = 0; j < saveProps.Count; j++)
            {
                if (this.Props[i].MonitorBoundsWidth == saveProps[j][0].MonitorBoundsWidth &&
                    this.Props[i].MonitorBoundsHeight == saveProps[j][0].MonitorBoundsHeight &&
                    this.Props[i].Primary == saveProps[j][0].Primary)
                {
                    saveProps[j].Add(this.Props[i]);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                saveProps.Add(new List<ClsWindowProps>());
                saveProps[saveProps.Count - 1].Add(this.Props[i]);
            }
        }
        Directory.CreateDirectory(_path);
        for (int i = 0; i < saveProps.Count; i++)
        {
            fileNameWindows = saveProps[i][0].MonitorBoundsWidth + "x" + saveProps[i][0].MonitorBoundsHeight;
            fileNameWindows += (saveProps[i][0].Primary) ? "P.json" : ".json";
            using (var writer = new StreamWriter(_path + "\\" + fileNameWindows))
            {
                String json = JsonSerializer.Serialize(saveProps[i], options);
                writer.Write(json);
            }
        }
    }

    //**********************************************
    /// <summary> Loads data from disk </summary>
    //**********************************************
    public void Load()
    {
        this.Props.Clear();
        int Id = 0;
        System.IO.Directory.CreateDirectory(_path);
        List<ClsWindowProps> loadProps = new List<ClsWindowProps>();
        var fileEntries = Directory.GetFiles(_path, "*.json");
        foreach (string fileName in fileEntries)
        {
            if (Regex.Match(fileName, @"\d{3,5}x\d{3,5}P?\.json").Success)
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    String json = r.ReadToEnd();
                    if (json.Length > 0)
                    {
                        loadProps = JsonSerializer.Deserialize<List<ClsWindowProps>>(json);
                        foreach (var item in loadProps)
                        {
                            item.Tag = Id;
                            Id++;
                            AddWindow(item);
                        }
                    }
                }
            }
        }
        this.Props.Sort((o1, o2) => o1.Name.CompareTo(o2.Name));
    }

    //**********************************************
    // API calls
    //**********************************************

    [DllImport("USER32.DLL")]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("USER32.DLL")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

}
