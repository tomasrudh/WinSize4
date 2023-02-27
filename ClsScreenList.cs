using System;

namespace WinSize4
{
    public class ClsScreenList : ICloneable
    {
        public object Clone()
        {
            var ScreenList = (ClsScreenList)MemberwiseClone();
            return ScreenList;
        }

        public int BoundsWidth
        { set; get; }

        public int BoundsHeight
        { set; get; }

        public int WorkingAreaWidth
        { set; get; }

        public int WorkingAreaHeight
        { set; get; }

        public int CustomLeft
        { set; get; }

        public int CustomTop
        { set; get; }

        public int CustomWidth
        { set; get; }

        public int CustomHeight
        { set; get; }

        public int X
        { set; get; }

        public int Y
        { set; get; }

        public bool Primary
        { set; get; }
    }
}