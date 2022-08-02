using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StormworksPriceList.Controls
{
    /// <summary>
    /// Interaction logic for RoundButton.xaml
    /// </summary>
    public partial class RoundButton : UserControl
    {
        public RoundButton()
        {
            InitializeComponent();
        }

        private void ButtonCircle_MouseEnter(object sender, MouseEventArgs e)
        {
            ButtonCircle.StrokeThickness = 5.0;
        }

        private void ButtonCircle_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonCircle.StrokeThickness = 0.0;
        }
    }
}
