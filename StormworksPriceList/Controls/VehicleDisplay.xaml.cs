using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StormworksPriceList.Controls
{
    public partial class VehicleDisplay : UserControl
    {
        private bool isSelected = false;
        public bool IsSelected { get { return isSelected; } set { isSelected = value; UpdateBackground(); } }

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
            ValueLabel.Text = $"${String.Format("{0:N}", Vehicle.GetVehicleCost()).Replace(",00", "")}";//$"${Vehicle.GetVehicleCost()}";
            UpdatedLabel.Text = Vehicle.LastUpdated.ToString();
        }
        //string html = String.Format("Order Total: {0:C}", moneyvalue); 
        void UpdateBackground()
        {
            if (IsSelected)
                MainGrid.Background = Brush_SWBlue;
            else
                MainGrid.Background = Brushes.Transparent;
        }

        private void MainGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            MainGrid.Background = Brush_SWBlue;
        }

        private void MainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsSelected)
                MainGrid.Background = Brushes.Transparent;
        }
    }
}
