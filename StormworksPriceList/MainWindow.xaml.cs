using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using StormworksPriceList.Controls;
using System.Linq;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;

namespace StormworksPriceList
{
    public partial class MainWindow : Window
    {
        static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string vehiclesPath = Path.Combine(appDataPath, "Stormworks", "data", "vehicles");

        public MainWindow()
        {
            InitializeComponent();
            RegistryManager.Initialize();

            // [TODO: Sort this newest to last]
            if (Directory.Exists(vehiclesPath))
            {
                var vehicleFiles = Directory.GetFiles(vehiclesPath, "*.xml");

                ImageContainer.Children.Clear();
                foreach (string filePath in vehicleFiles)
                {
                    Vehicle vehicle = new Vehicle(filePath);
                    VehicleDisplay vehicleDisplay = new VehicleDisplay(vehicle);

                    vehicleDisplay.MouseLeftButtonUp += VehicleDisplay_MouseLeftButtonUp;

                    ImageContainer.Children.Add(vehicleDisplay);
                }
            }
        }

        string[] myColors =
            {
                "#9c9fd5",
                "#7f8dd8",
                "#cfdae7",
                "#4fa2e6",
                "#154d7a",
                "#35d7fb",
                "#aff7f0",
                "#0c6d57",
                "#36a27f",
                "#89ddba"
            };

        Color[] myNewColors =
            {
                Color.FromRgb(143, 0, 0),
                Color.FromRgb(255, 0, 43),
                Color.FromRgb(255, 127, 39),
                Color.FromRgb(253, 165, 2),
                Color.FromRgb(255, 231, 39),
                Color.FromRgb(0, 226, 50),
                Color.FromRgb(0, 158, 29),
                Color.FromRgb(0, 131, 49),
                Color.FromRgb(41, 173, 255),
                Color.FromRgb(0, 89, 255)
            };

        private void VehicleDisplay_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (PieContainer.Children.Count > 0)
                PieContainer.Children.Clear();

            VehicleDisplay vehicleDisplay = (VehicleDisplay)sender;
            if (vehicleDisplay != null && vehicleDisplay.Vehicle != null)
            {
                int totalVehicleCost = vehicleDisplay.Vehicle.GetVehicleCost();
                int top10Cost = 0;
                var sortedList = vehicleDisplay.Vehicle.ComponentDatas.OrderByDescending(d => d.GetTotalCost()).ToList();
                Pie lastPie = null;

                // Calculate total cost for top 10 most expensive component groups
                for (int i = 0; i < (sortedList.Count >= 10 ? 10 : sortedList.Count); i++)
                {
                    top10Cost += sortedList[i].Price;//GetTotalCost();
                }

                // Build pie chart
                for (int i = 0; i < (sortedList.Count >= 10 ? 10 : sortedList.Count); i++)
                {
                    Pie pie = new Pie
                    {
                        Slice = (double)sortedList[i].Price / top10Cost,
                        //Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(myColors[i]),
                        Fill = new SolidColorBrush(myNewColors[9-i]),
                        Opacity = 0.5,
                        Width = 128,
                        Height = 128,
                        Mode = PieMode.Slice,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1.0,
                        ToolTip = $"{sortedList[i].Name}[x{sortedList[i].Amount}]: ${sortedList[i].GetTotalCost()} (${sortedList[i].Price} each)"
                    };

                    pie.MouseEnter += Pie_MouseEnter;
                    pie.MouseLeave += Pie_MouseLeave;

                    if (lastPie != null)
                        pie.StartAngle = lastPie.EndAngle;

                    lastPie = pie;
                    PieContainer.Children.Add(pie);
                    //Debug.WriteLine($"[{sortedList[i].Name}]: x{sortedList[i].Amount}, ${sortedList[i].Price} á ${sortedList[i].GetTotalCost()}");
                }
            }
        }

        private void Pie_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Pie pie = (Pie)sender;
            pie.Opacity = 0.5;
        }

        private void Pie_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Pie pie = (Pie)sender;
            pie.Opacity = 1.0;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToHorizontalOffset(scv.HorizontalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
