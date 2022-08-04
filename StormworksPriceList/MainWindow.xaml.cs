using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using StormworksPriceList.Controls;
using System.Linq;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Input;
using StormworksPriceInspector.Classes;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using StormworksPriceInspector.Enums;
using System.Windows.Shapes;

namespace StormworksPriceList
{
    public partial class MainWindow : Window
    {
        static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string vehiclesPath = System.IO.Path.Combine(appDataPath, "Stormworks", "data", "vehicles");

        #region Sorting Variables
        private SortingEnum.SortDirection sortDirection = SortingEnum.SortDirection.Ascending;
        private SortingEnum.SortDirection SortDirection { get { return sortDirection; } set { sortDirection = value; sortDirectionLabel.Text = sortDirection.ToString(); UpdateVehicleDisplay(); } }
        private SortingEnum.SortOption sortOption = SortingEnum.SortOption.Name;
        private SortingEnum.SortOption SortOption { get { return sortOption; } set { sortOption = value; UpdateVehicleDisplay(); } }
        #endregion
        private List<VehicleDisplay> vehicleDisplays = new List<VehicleDisplay>();
        private List<Rectangle> buttonRectangles;

        public MainWindow()
        {
            InitializeComponent();
            RegistryManager.Initialize();
            PieContainer.Children.Clear();

            // Populate rectangle list
            buttonRectangles = new List<Rectangle> {
                (Rectangle)sortButtonName.Children[0],
                (Rectangle)sortButtonCost.Children[0],
                (Rectangle)sortButtonDate.Children[0]
            };

            // TODO: Use ObservableCollection instead, bind to ImageContainer
            if (vehicleDisplays.Count > 0)
                vehicleDisplays.Clear();

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

                    vehicleDisplays.Add(vehicleDisplay);
                }

                UpdateVehicleDisplay();
            }
        }

        Color[] pieChartColors =
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

        // TODO: Refactor this method
        private void VehicleDisplay_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // New selection, remove old pie chart
            if (PieContainer.Children.Count > 0)
                PieContainer.Children.Clear();

            // Remove any previously selected display
            for (int i = 0; i < vehicleDisplays.Count; i++)
            {
                if (vehicleDisplays[i].IsSelected)
                    vehicleDisplays[i].IsSelected = false;
            }

            VehicleDisplay vehicleDisplay = (VehicleDisplay)sender;
            if (vehicleDisplay != null && vehicleDisplay.Vehicle != null)
            {
                vehicleDisplay.IsSelected = true;
                int totalVehicleCost = vehicleDisplay.Vehicle.GetVehicleCost();
                int top10Cost = 0;
                var sortedList = vehicleDisplay.Vehicle.ComponentDatas.OrderByDescending(d => d.TotalCost).ToList();
                Pie lastPie = null;

                // Calculate total cost for top 10 most expensive component groups
                for (int i = 0; i < (sortedList.Count >= 10 ? 10 : sortedList.Count); i++)
                {
                    top10Cost += sortedList[i].TotalCost;//GetTotalCost();
                }

                // Build pie chart
                for (int i = 0; i < (sortedList.Count >= 10 ? 10 : sortedList.Count); i++)
                {
                    Pie pie = new Pie
                    {
                        Slice = (double)sortedList[i].TotalCost / top10Cost,
                        Fill = new SolidColorBrush(pieChartColors[9-i]),
                        Opacity = 0.5,
                        Width = 128,
                        Height = 128,
                        Mode = PieMode.Slice,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1.0,
                        ToolTip = $"{sortedList[i].Name}[x{sortedList[i].Amount}]: ${sortedList[i].TotalCost} (${sortedList[i].Price} each)"
                    };

                    pie.MouseEnter += Pie_MouseEnter;
                    pie.MouseLeave += Pie_MouseLeave;

                    if (lastPie != null)
                        pie.StartAngle = lastPie.EndAngle;

                    lastPie = pie;
                    PieContainer.Children.Add(pie);
                }

                // Populate DataGridening
                ComponentDataGrid.ItemsSource = vehicleDisplay.Vehicle.ComponentDatas;
            }
        }

        #region Pie Chart Events
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
        #endregion

        #region MainWindow UI Events
        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToHorizontalOffset(scv.HorizontalOffset - e.Delta);
            e.Handled = true;
        }
        private void TitleGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!MinimizeButton.IsMouseOver && !CloseButton.IsMouseOver)
            {
                Cursor = Cursors.SizeAll;
                DragMove();
                Cursor = null;
            }
        }
        private void MinimizeButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => WindowState = System.Windows.WindowState.Minimized;
        private void CloseButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => Close();
        private void MinimizeButton_MouseEnter(object sender, MouseEventArgs e) => MinimizeButton.Foreground = StormworksPalette.Text_Active;

        private void MinimizeButton_MouseLeave(object sender, MouseEventArgs e) => MinimizeButton.Foreground = StormworksPalette.Text_Inactive;

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e) => CloseButton.Foreground = StormworksPalette.Text_Active;

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e) => CloseButton.Foreground = StormworksPalette.Text_Inactive;
        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender == sortButtonDirection)
                SortDirection = (SortDirection == SortingEnum.SortDirection.Ascending) ? SortingEnum.SortDirection.Descending : SortingEnum.SortDirection.Ascending;

            if (sender == sortButtonName)
                SortOption = SortingEnum.SortOption.Name;

            if (sender == sortButtonCost)
                SortOption = SortingEnum.SortOption.Cost;

            if (sender == sortButtonDate)
                SortOption = SortingEnum.SortOption.Date;
        }
        private void Button_MouseEnterLeave(object sender, MouseEventArgs e)
        {
            var grid = sender as Grid;

            if (grid is null)
                return;

            if (e.RoutedEvent.Name == "MouseEnter")
                grid.Children[0].Opacity = 0.5;

            if (e.RoutedEvent.Name == "MouseLeave")
                grid.Children[0].Opacity = 1.0;

        }
        #endregion

        private void UpdateVehicleDisplay()
        {
            ImageContainer.Children.Clear();
            buttonRectangles.ForEach(x => x.Fill = StormworksPalette.Background_Dark_1);

            switch (sortOption)
            {
                case SortingEnum.SortOption.Name:
                    vehicleDisplays = (SortDirection == SortingEnum.SortDirection.Ascending) ? vehicleDisplays.OrderBy(x => x.Vehicle.Name).ToList() : vehicleDisplays.OrderByDescending(x => x.Vehicle.Name).ToList();
                    buttonRectangles[0].Fill = StormworksPalette.Background_Blue;
                    break;
                case SortingEnum.SortOption.Cost:
                    vehicleDisplays = (SortDirection == SortingEnum.SortDirection.Ascending) ? vehicleDisplays.OrderBy(x => x.Vehicle.GetVehicleCost()).ToList() : vehicleDisplays.OrderByDescending(x => x.Vehicle.GetVehicleCost()).ToList();
                    buttonRectangles[1].Fill = StormworksPalette.Background_Blue;
                    break;
                case SortingEnum.SortOption.Date:
                    vehicleDisplays = (SortDirection == SortingEnum.SortDirection.Ascending) ? vehicleDisplays.OrderBy(x => x.Vehicle.LastUpdated).ToList() : vehicleDisplays.OrderByDescending(x => x.Vehicle.LastUpdated).ToList();
                    buttonRectangles[2].Fill = StormworksPalette.Background_Blue;
                    break;
            }

            vehicleDisplays.ForEach(x => ImageContainer.Children.Add(x));
        }
    }
}
