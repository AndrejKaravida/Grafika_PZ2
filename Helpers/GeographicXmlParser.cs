using PZ2.Models;
using System.Collections.Generic;
using System.Xml;

namespace PZ2.Helpers
{
    public class GeographicXmlParser
    {
        public static void LoadSubstations(List<PowerEntity> elements, double newX, double newY, List<double> xPoints, List<double> yPoints)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Geographic.xml");
            XmlNodeList nodeList;
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");

            foreach (XmlNode node in nodeList)
            {
                SubstationEntity sub = new SubstationEntity();
                sub.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                sub.Name = node.SelectSingleNode("Name").InnerText;
                sub.X = double.Parse(node.SelectSingleNode("X").InnerText);
                sub.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                sub.ToolTip = "Substation\nID: " + sub.Id + " \n Name: " + sub.Name;

                elements.Add(sub);

                Converter.ToLatLon(sub.X, sub.Y, 34, out newX, out newY);
                xPoints.Add(newX);
                yPoints.Add(newY);
            }
        }

        public static void LoadSwitches(List<PowerEntity> elements, double newX, double newY, List<double> xPoints, List<double> yPoints)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load("Geographic.xml");
            XmlNodeList nodeList;


            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            foreach (XmlNode node in nodeList)
            {
                SwitchEntity sw = new SwitchEntity();
                sw.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                sw.Name = node.SelectSingleNode("Name").InnerText;
                sw.X = double.Parse(node.SelectSingleNode("X").InnerText);
                sw.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                sw.Status = node.SelectSingleNode("Status").InnerText;
                sw.ToolTip = "Switch\nID: " + sw.Id + " \n Name: " + sw.Name + "\n Status: " + sw.Status;

                elements.Add(sw);

                Converter.ToLatLon(sw.X, sw.Y, 34, out newX, out newY);
                xPoints.Add(newX);
                yPoints.Add(newY);
            }

        }

        public static void LoadNodes(List<PowerEntity> elements, double newX, double newY, List<double> xPoints, List<double> yPoints)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Geographic.xml");
            XmlNodeList nodeList;

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            foreach (XmlNode node in nodeList)
            {
                NodeEntity newNode = new NodeEntity();
                newNode.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                newNode.Name = node.SelectSingleNode("Name").InnerText;
                newNode.X = double.Parse(node.SelectSingleNode("X").InnerText);
                newNode.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                newNode.ToolTip = "Node\nID: " + newNode.Id + "\n  Name: " + newNode.Name;

                elements.Add(newNode);

                Converter.ToLatLon(newNode.X, newNode.Y, 34, out newX, out newY);
                xPoints.Add(newX);
                yPoints.Add(newY);
            }
        }

        public static void LoadLineEntities(List<LineEntity> lineEntities)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Geographic.xml");
            XmlNodeList nodeList;

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
            foreach (XmlNode node in nodeList)
            {
                LineEntity l = new LineEntity();
                l.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                l.Name = node.SelectSingleNode("Name").InnerText;
                if (node.SelectSingleNode("IsUnderground").InnerText.Equals("true"))
                {
                    l.IsUnderground = true;
                }
                else
                {
                    l.IsUnderground = false;
                }
                l.R = float.Parse(node.SelectSingleNode("R").InnerText);
                l.ConductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText;
                l.LineType = node.SelectSingleNode("LineType").InnerText;
                l.ThermalConstantHeat = long.Parse(node.SelectSingleNode("ThermalConstantHeat").InnerText);
                l.FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText);
                l.SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText);

                lineEntities.Add(l);
            }
        }


    }
}
