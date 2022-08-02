using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;

namespace StormworksPriceList
{
    
    public static class RegistryManager
    {
        public static string StormworksPath = "";
        public static string DefinitionsPath = "";

        public static void Initialize()
        {
            try
            {
                using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 573090"))
                {
                    if (regKey != null)
                    {
                        var path = regKey.GetValue("InstallLocation").ToString();
                        if (!String.IsNullOrWhiteSpace(path))
                        {
                            StormworksPath = path;
                            DefinitionsPath = Path.Combine(path, "rom", "data", "definitions");
                        } else {
                            MessageBox.Show("Failed to find the Stormworks install path in the registry. Please, specify Stormworks location manually.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    } else {
                        MessageBox.Show("Failed to find the Steam App 573090 SubKey in the registry. Please, specify Stormworks location manually.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            } catch (Exception e) {
                MessageBox.Show($"An error occurred while trying to read the registry to find Stormworks install location.\n\nError Message:\n{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
