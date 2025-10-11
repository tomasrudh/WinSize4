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
using static System.Net.Mime.MediaTypeNames;

public class ClsSavedWindows
{
    public List<ClsWindowProps> Props = new List<ClsWindowProps>();
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
    /// <summary> Duplicates a window </summary>
    /// <param name="Props"></param>
    /// <returns>Tag</returns>
    //**********************************************
    public void DuplicateWindow(int Index)
    {
        this.Props.Add(this.Props[Index]);
        this.Props[this.Props.Count - 1].Tag = _nextTag++;
    }

    //**********************************************
    /// <summary>Updates properties for the supplied window properties</summary>
    /// <param name="ClsSavedWindowProps"></param>
    /// <returns>True if a window is updated, false if window is not found</returns>
    //**********************************************

    public bool UpdateWindowProperties(ClsWindowProps Props, List<ClsScreenList> CurrentScreenList, int screenCurrentIndex)
    {
        bool Found = false;
        int Index = GetIndexCurrentScreen(Props, CurrentScreenList, screenCurrentIndex);
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
    /// <summary> Finds the window with the supplied data in the current screen, or -1 </summary>
    /// <param name="ClsWindowProps, ClsScreenList"></param>
    /// <returns>Index</returns>
    //**********************************************
    //public int GetIndexCurrentScreen(ClsWindowProps CurrentWindowProps)
    public int GetIndexCurrentScreen(ClsWindowProps CurrentWindowProps, List<ClsScreenList> CurrentScreenList, int screenCurrentIndex)
    {
        int result = -1;

        // Get list of matching saved windows
        List<ClsWindowProps> matchingSavedWindows = new List<ClsWindowProps>();
        for (int i = 0; i < this.Props.Count; i++)
        {
            bool FoundTitle = true;
            if (this.Props[i].SearchTitleInclude)
            {
                switch (this.Props[i].SearchTypeInclude)
                {
                    case ClsWindowProps.Full:
                        FoundTitle = (CurrentWindowProps.TitleInclude == this.Props[i].TitleInclude);
                        break;
                    case ClsWindowProps.Contains:
                        FoundTitle = CurrentWindowProps.TitleInclude.Contains(this.Props[i].TitleInclude);
                        break;
                    case ClsWindowProps.StartsWith:
                        FoundTitle = CurrentWindowProps.TitleInclude.StartsWith(this.Props[i].TitleInclude);
                        break;
                }
            }
            bool FoundExe = true;
            if (this.Props[i].SearchExe)
                FoundExe = (CurrentWindowProps.Exe == this.Props[i].Exe);
            bool FoundWindowClass = true;
            if (this.Props[i].ConsiderWindowClass)
                FoundWindowClass = (CurrentWindowProps.WindowClass == this.Props[i].WindowClass);

            if (FoundTitle && FoundExe && FoundWindowClass)
            {
                matchingSavedWindows.Add(this.Props[i]);
            }
        }
        for (int i = 0; i < matchingSavedWindows.Count; i++)
        {
            if (matchingSavedWindows[i].MonitorBoundsWidth == CurrentScreenList[screenCurrentIndex].BoundsWidth &&
                matchingSavedWindows[i].MonitorBoundsHeight == CurrentScreenList[screenCurrentIndex].BoundsHeight &&
                matchingSavedWindows[i].Primary == CurrentScreenList[screenCurrentIndex].Primary)
            {
                result = GetWindowIndexByTag(matchingSavedWindows[i].Tag);
                break;
            }
        }
        return result;
    }

    //**********************************************
    /// <summary> Finds the first window with the supplied data, or -1 </summary>
    /// <param name="ClsWindowProps, ClsScreenList"></param>
    /// <returns>Index</returns>
    //**********************************************
    //public int GetIndexAllScreens(ClsWindowProps CurrentWindowProps)
    public int GetIndexAllScreens(ClsWindowProps WindowProps, List<ClsScreenList> ScreenList, int ScreenIndex)
    {
        int result = -1;

        //ClsDebug.AddText("GetIndexAllScreens: " + WindowProps.Title);
        // Get list of matching saved windows
        List<ClsWindowProps> matchingSavedWindows = new List<ClsWindowProps>();
        for (int i = 0; i < this.Props.Count; i++)
        {
            bool FoundTitleInclude = true;
            if (this.Props[i].SearchTitleInclude)
            {
                FoundTitleInclude = false;
                switch (this.Props[i].SearchTypeInclude)
                {
                    case ClsWindowProps.Full:
                        FoundTitleInclude = (WindowProps.Title == this.Props[i].TitleInclude);
                        break;
                    case ClsWindowProps.Contains:
                        if (WindowProps.Title.Contains("Loading"))
                            Console.WriteLine(WindowProps.Title);
                        foreach (string titleInclude in (this.Props[i].TitleInclude).Split('|'))
                        {
                            if (WindowProps.Title.Contains(titleInclude))
                            {
                                FoundTitleInclude = true;
                                break;
                            }
                        }
                        break;
                    case ClsWindowProps.StartsWith:
                        foreach (string titleInclude in (this.Props[i].TitleInclude).Split('|'))
                        {
                            if (WindowProps.Title.StartsWith(titleInclude)) {
                                FoundTitleInclude = true;
                                break;
                            }
                        }
                        break;
                }
            }
            bool FoundTitleExclude = false;
            if (this.Props[i].SearchTitleExclude)
            {
                switch (this.Props[i].SearchTypeExclude)
                {
                    case ClsWindowProps.Full:
                        FoundTitleExclude = (WindowProps.Title == this.Props[i].TitleExclude);
                        break;
                    case ClsWindowProps.Contains:
                        //FoundTitleExclude = WindowProps.Title.Contains(this.Props[i].TitleExclude);
                        foreach (string titleExclude in (this.Props[i].TitleExclude).Split('|'))
                        {
                            if (WindowProps.Title.Contains(titleExclude)) {
                                FoundTitleExclude = true;
                                break;
                            }
                        }
                        break;
                    case ClsWindowProps.StartsWith:
                        //FoundTitleExclude = WindowProps.Title.StartsWith(this.Props[i].TitleExclude);
                        foreach (string titleExclude in (this.Props[i].TitleExclude).Split('|'))
                        {
                            if (WindowProps.Title.StartsWith(titleExclude)) {
                                FoundTitleExclude = true;
                                break;
                            }
                        }
                        break;
                }
            }
            bool FoundExe = true;
            if (this.Props[i].SearchExe)
                FoundExe = (WindowProps.Exe == this.Props[i].Exe);
            bool FoundWindowClass = true;
            if (this.Props[i].ConsiderWindowClass)
                FoundWindowClass = (WindowProps.WindowClass == this.Props[i].WindowClass);

            if (FoundTitleInclude && !FoundTitleExclude && FoundExe && FoundWindowClass)
            {
                matchingSavedWindows.Add(this.Props[i]);
            }
        }

        for (int i = 0; i < matchingSavedWindows.Count; i++)
        {
            for (int j = 0; j < ScreenList.Count; j++)
            {
                // Another screen than current and not primary
                if (matchingSavedWindows[i].MonitorBoundsWidth == ScreenList[j].BoundsWidth &&
                    matchingSavedWindows[i].MonitorBoundsHeight == ScreenList[j].BoundsHeight &&
                    matchingSavedWindows[i].Primary == ScreenList[j].Primary &&
                    j != ScreenIndex &&
                    ScreenList[j].Primary == false &&
                    ScreenList[j].Present)
                {
                    //ClsDebug.AddText("  Another screen than current and not primary (" + j + ")");
                    result = GetWindowIndexByTag(matchingSavedWindows[i].Tag);
                    break;
                }
            }
        }
        for (int i = 0; i < matchingSavedWindows.Count; i++)
        {
            for (int j = 0; j < ScreenList.Count; j++)
            {
                // Another screen than current and primary
                if (matchingSavedWindows[i].MonitorBoundsWidth == ScreenList[j].BoundsWidth &&
                    matchingSavedWindows[i].MonitorBoundsHeight == ScreenList[j].BoundsHeight &&
                    matchingSavedWindows[i].Primary == ScreenList[j].Primary &&
                    j != ScreenIndex &&
                    ScreenList[j].Primary == true &&
                    ScreenList[j].Present)
                {
                    //ClsDebug.AddText("  Another screen than current and primary (" + j + ")");
                    result = GetWindowIndexByTag(matchingSavedWindows[i].Tag);
                    break;
                }
            }
        }
        for (int i = 0; i < matchingSavedWindows.Count; i++)
        {
            for (int j = 0; j < ScreenList.Count; j++)
            {
                // This screen matches
                if (matchingSavedWindows[i].MonitorBoundsWidth == ScreenList[j].BoundsWidth &&
                    matchingSavedWindows[i].MonitorBoundsHeight == ScreenList[j].BoundsHeight &&
                    matchingSavedWindows[i].Primary == ScreenList[j].Primary &&
                    ScreenList[j].Present &&
                    j == ScreenIndex)
                {
                    //ClsDebug.AddText("  This screen matches (" + j + ")");
                    result = GetWindowIndexByTag(matchingSavedWindows[i].Tag);
                    break;
                }
            }
        }
        //ClsDebug.AddText("  Saved window index chosen: " + result);
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
    /// <summary> Saves data to the specified path </summary>
    //**********************************************
    public void Save(string activePath)
    {
        var options = new JsonSerializerOptions() { WriteIndented = true };
        var saveProps = new List<List<ClsWindowProps>>();

        Directory.CreateDirectory(activePath);

        // Clean out only old window position files from the target directory
        var fileEntries = Directory.GetFiles(activePath, "*.json");
        foreach (string fileName in fileEntries)
        {
            if (Regex.IsMatch(Path.GetFileName(fileName), @"^\d{3,5}x\d{3,5}P?\.json$"))
            {
                File.Delete(fileName);
            }
        }

        // Group window properties by screen configuration
        foreach (var prop in this.Props)
        {
            var group = saveProps.FirstOrDefault(g =>
                g[0].MonitorBoundsWidth == prop.MonitorBoundsWidth &&
                g[0].MonitorBoundsHeight == prop.MonitorBoundsHeight &&
                g[0].Primary == prop.Primary);

            if (group != null)
            {
                group.Add(prop);
            }
            else
            {
                saveProps.Add(new List<ClsWindowProps> { prop });
            }
        }

        // Write the new files
        foreach (var group in saveProps)
        {
            string fileNameWindows = $"{group[0].MonitorBoundsWidth}x{group[0].MonitorBoundsHeight}";
            fileNameWindows += group[0].Primary ? "P.json" : ".json";
            string fullPath = Path.Combine(activePath, fileNameWindows);
            using (var writer = new StreamWriter(fullPath))
            {
                String json = JsonSerializer.Serialize(group, options);
                writer.Write(json);
            }
        }
    }

    //**********************************************
    /// <summary> Loads data from the specified path </summary>
    //**********************************************
    public void Load(string activePath)
    {
        this.Props.Clear();
        _nextTag = 0; // Reset tag counter on load

        Directory.CreateDirectory(activePath);
        var fileEntries = Directory.GetFiles(activePath, "*.json");

        foreach (string fileName in fileEntries)
        {
            if (Regex.IsMatch(Path.GetFileName(fileName), @"^\d{3,5}x\d{3,5}P?\.json$"))
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    String json = r.ReadToEnd();
                    if (json.Length > 0)
                    {
                        var loadedProps = JsonSerializer.Deserialize<List<ClsWindowProps>>(json);
                        foreach (var item in loadedProps)
                        {
                            // AddWindow will assign the correct, unique tag
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
