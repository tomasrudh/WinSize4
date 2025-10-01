namespace WinSize4
{
    public class ClsWindows
    {
        public long hWnd;
        public int Pid;
        public ClsWindowProps Props;
        public Boolean Moved;
    }

    public class ClsWindowProps
    {
        public int Tag
        { get; set; }
        public string Name
        { get; set; } = "";
        public string WindowClass
        { get; set; } = "";
        public int MonitorBoundsWidth
        { set; get; }
        public int MonitorBoundsHeight
        { set; get; }
        public bool Primary
        { get; set; }
        public string Title
        { set; get; } = "";
        public int Left
        { set; get; }
        public int Top
        { set; get; }
        public int Width
        { set; get; }
        public int Height
        { set; get; }
        public string Exe
        { set; get; } = "";
        public string TitleInclude
        { set; get; } = "";
        public bool SearchTitleInclude
        { set; get; } = false;
        public int SearchTypeInclude
        { set; get; } = Full;
        public string TitleExclude
        { set; get; } = "";
        public bool SearchTitleExclude
        { set; get; } = false;
        public int SearchTypeExclude
        { set; get; } = Full;
        public bool SearchExe
        { set; get; } = true;
        public bool MaxWidth
        { set; get; }
        public bool MaxHeight
        { set; get; }
        public bool FullScreen
        { set; get; }
        public bool IgnoreChildWindows
        { set; get; } = true;
        public bool ConsiderWindowClass
        { set; get; } = true;
        public bool Present
        { set; get; }
        public bool AlwaysMove
        { set; get; } = false;
        public bool CanResize
        { set; get; } = true;
        public bool Disabled
        { get; set; } = false; // Default to false (not disabled)

        public const int Full = 0;
        public const int Contains = 1;
        public const int StartsWith = 2;
    }
}
