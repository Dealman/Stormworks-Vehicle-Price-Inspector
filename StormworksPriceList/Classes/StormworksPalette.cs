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

        static StormworksPalette()
        {
            Background_Blue.Freeze();
            Background_Dark_1.Freeze();
        }
    }
}
