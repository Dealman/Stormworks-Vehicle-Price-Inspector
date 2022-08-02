using System;
using System.IO;
using System.Text.RegularExpressions;

namespace StormworksPriceList
{
    // Note: Block definitions contain invalid XML formatting, some nodes contain attributes like "00=x" which is not valid.
    // thus we don't use the XML parsing method.
    public static class ComponentParser
    {
        static Regex rxName = new Regex(" name=\"(.*?)\"", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex rxValue = new Regex("value=\"(\\d+)\"", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static int GetComponentValue(string componentName)
        {
            if (!String.IsNullOrWhiteSpace(RegistryManager.DefinitionsPath))
            {
                var filePath = Path.Combine(RegistryManager.DefinitionsPath, $"{componentName}.xml");
                if (File.Exists(filePath))
                {
                    var fileData = File.ReadAllText(filePath);

                    return int.Parse(rxValue.Match(fileData).Groups[1].Value);
                }
            }

            return -1;
        }

        public static string GetComponentName(string componentName)
        {
            if (!String.IsNullOrWhiteSpace(RegistryManager.DefinitionsPath))
            {
                var filePath = Path.Combine(RegistryManager.DefinitionsPath, $"{componentName}.xml");
                if (File.Exists(filePath))
                {
                    var fileData = File.ReadAllText(filePath);

                    return rxName.Match(fileData).Groups[1].Value;
                }
            }

            return "";
        }
    }
}
