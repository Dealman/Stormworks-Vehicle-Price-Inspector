using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StormworksPriceList.Controls
{
    public partial class VehicleDisplay : UserControl
    {
        static SolidColorBrush Brush_SWBlue = new SolidColorBrush(Color.FromRgb(64, 145, 180));

        public Vehicle Vehicle { get; set; }

        public VehicleDisplay()
        {
            InitializeComponent();

            // For Testing Only
            NameLabel.Text = "My Vehicle";
            ValueLabel.Text = "$10 000";
            UpdatedLabel.Text = DateTime.Now.ToString();
        }

        public VehicleDisplay(Vehicle vehicle)
        {
            InitializeComponent();

            Vehicle = vehicle;

            VehicleImage.Source = vehicle.Icon;
            NameLabel.Text = Vehicle.Name;
            ValueLabel.Text = $"${Vehicle.GetVehicleCost()}";
            UpdatedLabel.Text = Vehicle.LastUpdated.ToString();
        }

        private void MainGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            MainGrid.Background = Brush_SWBlue;
        }

        private void MainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            MainGrid.Background = Brushes.Transparent;
        }

        private void MainGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
