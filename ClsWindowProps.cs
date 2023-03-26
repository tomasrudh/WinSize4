using System;

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
    public bool SearchTitle
    { set; get; } = true;
    public int SearchType
    { set; get; } = Full;
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

    public const int Full = 0;
    public const int Contains = 1;
    public const int StartsWith = 2;
}
