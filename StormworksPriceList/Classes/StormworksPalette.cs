using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace StormworksPriceInspector.Classes
{
    public static class StormworksPalette
    {
        public static SolidColorBrush Background_Blue = new SolidColorBrush(Color.FromRgb(64, 145, 180));
        public static SolidColorBrush Background_Dark_1 = new SolidColorBrush(Color.FromRgb(25, 25, 25));

        public static SolidColorBrush Text_Active = new SolidColorBrush(Color.FromRgb(230, 230, 230));
        public static SolidColorBrush Text_Inactive = new SolidColorBrush(Color.FromRgb(20, 20, 25));
        public static SolidColorBrush Text_Header = new SolidColorBrush(Color.FromRgb(191, 120, 86));

        static StormworksPalette()
        {
            Background_Blue.Freeze();
            Background_Dark_1.Freeze();
            Text_Active.Freeze();
            Text_Inactive.Freeze();
            Text_Header.Freeze();
        }
    }
}
