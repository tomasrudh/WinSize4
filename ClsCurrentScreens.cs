namespace WinSize4
{
    public class ClsCurrentScreens
    {
        public List<ClsScreenList> ScreenList = new List<ClsScreenList>();

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

        //**********************************************
        /// <summary> Get index in ScreenList for the supplied values, or -1 </summary>
        //**********************************************
        public int GetIndex(ClsWindowProps Win)
        {
            int index = -1;
            for (int i = 0; i < this.ScreenList.Count; i++)
            {
                if (this.ScreenList[i].BoundsWidth == Win.MonitorBoundsWidth
                    && this.ScreenList[i].BoundsHeight == Win.MonitorBoundsHeight
                    && this.ScreenList[i].Primary == Win.Primary)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        //**********************************************
        /// <summary> Add screen to list of screens </summary>
        /// <param name="screen"></param>
        //**********************************************
        public void Add(ClsScreenList screen)
        {
            this.ScreenList.Add(screen);
        }

        /// <summary>Cleans out screen no longer available</summary>
        /// <returns>True if at least one screen was deleted</returns>
        public bool CleanScreenList()
        {
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
    }
}

