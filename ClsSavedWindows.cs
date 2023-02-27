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

public class ClsSavedWindows
{
    public List<ClsWindowProps> Props = new List<ClsWindowProps>();
    private string _path = Environment.GetEnvironmentVariable("LocalAppData") + "\\WinSize4";

    //**********************************************
    /// <summary> Adds a new window </summary>
    /// <param name="Props"></param>
    //**********************************************
    public void Add(ClsWindowProps Props, long hWnd)
    {
        this.Props.Add(Props);
        this.Props[this.Props.Count - 1].Tag = FindNextTag();
    }

    //**********************************************
    /// <summary>Sets properties for the supplied window handle</summary>
    /// <param name="hWnd"></param>
    //**********************************************
    public void SetWindowProperties(long hWnd)
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

        int index = GetIndex(props);
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

    public bool UpdateWindowProperties(ClsWindowProps Props)
    {
        bool Found = false;
        int Index = GetIndex(Props);
        if (Index > -1)
        {
            Found = true;
            this.Props[Index].Left = Props.Left;
            this.Props[Index].Top = Props.Top;
            this.Props[Index].Width = Props.Width;
            this.Props[Index].Height = Props.Height;
            this.Props[Index].MaxWidth = false;
            this.Props[Index].MaxHeight = false;
            this.Props[Index].FullScreen = false;
        }
        return Found;
    }

    //**********************************************
    /// <summary> Finds the first window with the supplied data, or -1 </summary>
    /// <param name="ClsWindowProps"></param>
    /// <returns>Index</returns>
    //**********************************************
    public int GetIndex(ClsWindowProps Props)
    {
        int result = -1;
        for (int index = 0; index < this.Props.Count; index++)
        {
            bool FoundTitle = true;
            if (this.Props[index].SearchTitle)
            {
                switch (this.Props[index].SearchType)
                {
                    case ClsWindowProps.Full:
                        FoundTitle = (Props.Title == this.Props[index].Title);
                        break;
                    case ClsWindowProps.Contains:
                        FoundTitle = Props.Title.Contains(this.Props[index].Title);
                        break;
                    case ClsWindowProps.StartsWith:
                        FoundTitle = Props.Title.StartsWith(this.Props[index].Title);
                        break;
                }
            }
            bool FoundExe = true;
            if (this.Props[index].SearchExe)
                FoundExe = (Props.Exe == this.Props[index].Exe);
            if (FoundTitle &&
                FoundExe &&
                this.Props[index].MonitorBoundsWidth == Props.MonitorBoundsWidth &&
                this.Props[index].MonitorBoundsHeight == Props.MonitorBoundsHeight &&
                this.Props[index].Primary == Props.Primary)
            {
                result = index;
                break;
            }
        }
        return result;
    }

    //**********************************************
    /// <summary> Get the index of the window with the supplied Tag </summary>
    /// <param name="Tag"></param>
    /// <returns>index</returns>
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
        return null;
    }

    //**********************************************
    /// <summary> Finds the next available tag </summary>
    /// <returns>Tag</returns>
    //**********************************************
    public int FindNextTag()
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
                            this.Props.Add(item);
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
