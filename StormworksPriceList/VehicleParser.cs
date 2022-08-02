using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace StormworksPriceList
{
    public class VehicleParser
    {
        private XmlDocument XML = new XmlDocument();

        private string path = "";
        private int blockCost = 2;

        private List<ComponentData> componentDatas = new List<ComponentData>();

        private static Regex rxFix = new Regex(" (\\d\\d)=\"\\d+\"", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex rxDataVersion = new Regex("data_version=\"(\\d+)\"", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public VehicleParser(string file)
        {
            if (!File.Exists(file))
                return;

            path = file;
            string fileContents = File.ReadAllText(path);
            int dataVersion = int.Parse(rxDataVersion.Match(fileContents).Groups[1].Value);

            if (dataVersion >= 3)
            {
                XML.Load(path);
            } else {
                // Version 2 and below contains broken XML formatting; attempt to fix
                string fixedXML = rxFix.Replace(fileContents, "");
                XML.LoadXml(fixedXML);
            }
        }

        public List<ComponentData> ParseComponents()
        {
            XmlNodeList componentsNode = XML.GetElementsByTagName("c");
            int blockCount = 0; // Normal bocks don't have a definition attribute

            List<string> componentNames = new List<string>();
            List<string> componentDisplayNames = new List<string>();
            List<int> componentPrices = new List<int>();
            List<int> componentAmounts = new List<int>();

            if (componentsNode.Count > 0)
            {
                foreach (XmlNode xmlNode in componentsNode)
                {
                    if (xmlNode.Attributes["d"] != null)
                    {
                        string componentName = xmlNode.Attributes["d"].Value;
                        if (componentNames.Contains(componentName))
                        {
                            int componentNameIndex = componentNames.FindIndex(x => x == componentName);
                            componentAmounts[componentNameIndex] += 1;
                        } else {
                            componentNames.Add(componentName);
                            componentPrices.Add(ComponentParser.GetComponentValue(componentName));
                            componentDisplayNames.Add(ComponentParser.GetComponentName(componentName));
                            componentAmounts.Add(1);
                        }
                    } else {
                        // V3: if "d" is null, it's a 01_block
                        if (xmlNode.Attributes.Count == 0 || xmlNode.Attributes["t"] != null)
                            if (xmlNode.FirstChild.Attributes["sc"] != null)
                                blockCount++;
                    }
                }

                // Build the ComponentDatas, easier list to work with
                for (int i = 0; i < componentNames.Count; i++)
                {
                    ComponentData componentData = new ComponentData()
                    {
                        Name = componentDisplayNames[i],
                        Price = componentPrices[i],
                        Amount = componentAmounts[i],
                        TotalCost = componentPrices[i] * componentAmounts[i]
                    };

                    componentDatas.Add(componentData);
                }

                // Let's not forget the blocks [TODO: Should probably make this not hardcoded]
                if (blockCount > 0)
                {
                    ComponentData componentData = new ComponentData()
                    {
                        Name = "Block",
                        Price = blockCost,
                        Amount = blockCount
                    };
                }
            }

            return componentDatas;
        }
    }
}
