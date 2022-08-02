using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace StormworksPriceList
{
    public class Vehicle
    {
        public string FilePath { get; set; }
        public string Name { get; }
        public DateTime LastUpdated { get; }
        public BitmapImage Icon { get; set; }
        public List<ComponentData> ComponentDatas = new List<ComponentData>();

        public Vehicle(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            FilePath = filePath;
            Name = Path.GetFileNameWithoutExtension(FilePath);
            LastUpdated = File.GetLastWriteTime(FilePath);

            GetIcon();
            ParseVehicle();
        }

        void ParseVehicle()
        {
            VehicleParser vehicleParser = new VehicleParser(FilePath);
            ComponentDatas = vehicleParser.ParseComponents();
        }

        void GetIcon()
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(FilePath.Replace(".xml", ".png"), UriKind.Absolute);
            bitmapImage.EndInit();

            Icon = bitmapImage;
        }

        public int GetVehicleCost()
        {
            if (ComponentDatas.Count > 0)
            {
                int totalCost = 0;

                foreach (ComponentData componentData in ComponentDatas)
                {
                    totalCost += componentData.TotalCost;
                }

                return totalCost;
            }

            return -1;
        }
    }
}
