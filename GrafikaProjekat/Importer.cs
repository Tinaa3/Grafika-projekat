using GrafikaProjekat.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GrafikaProjekat
{
    public class Importer
    {
        public static string fileName = "Geographic.xml";


        public Dictionary<long, SubstationEntity> GetSubstations()
        {
            if (File.Exists(fileName))
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument xmlDocument = XDocument.Load(stream);

                IEnumerable<SubstationEntity> substations = xmlDocument.Root.Elements("Substations").Elements("SubstationEntity").Select(substation => new SubstationEntity
                (
                    long.Parse(substation.Element("Id").Value),
                    substation.Element("Name").Value,
                    double.Parse(substation.Element("X").Value, CultureInfo.InvariantCulture),
                    double.Parse(substation.Element("Y").Value, CultureInfo.InvariantCulture)
                ));

                Dictionary<long, SubstationEntity> substationsDict = substations.ToDictionary(x => x.Id, x => x);

                return substationsDict;
            }
            else
            {
                return null;
            }
        }

        public Dictionary<long, NodeEntity> GetNodes()
        {
            if (File.Exists(fileName))
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument xmlDocument = XDocument.Load(stream);

                IEnumerable<NodeEntity> nodes = xmlDocument.Root.Elements("Nodes").Elements("NodeEntity").Select(node => new NodeEntity
                (
                    long.Parse(node.Element("Id").Value),
                    node.Element("Name").Value,
                    double.Parse(node.Element("X").Value, CultureInfo.InvariantCulture),
                    double.Parse(node.Element("Y").Value, CultureInfo.InvariantCulture)
                ));

                Dictionary<long, NodeEntity> nodeDict = nodes.ToDictionary(x => x.Id, x => x);

                return nodeDict;
            }
            else
            {
                return null;
            }
        }

        public Dictionary<long, SwitchEntity> GetSwitches()
        {
            if (File.Exists(fileName))
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument xmlDocument = XDocument.Load(stream);

                IEnumerable<SwitchEntity> switches = xmlDocument.Root.Elements("Switches").Elements("SwitchEntity").Select(newSwitch => new SwitchEntity
                (
                    long.Parse(newSwitch.Element("Id").Value),
                    newSwitch.Element("Name").Value,
                    newSwitch.Element("Status").Value,
                    double.Parse(newSwitch.Element("X").Value, CultureInfo.InvariantCulture),
                    double.Parse(newSwitch.Element("Y").Value, CultureInfo.InvariantCulture)
                ));

                Dictionary<long, SwitchEntity> switchesDict = switches.ToDictionary(x => x.Id, x => x);

                return switchesDict;
            }
            else
            {
                return null;
            }
        }

        public Dictionary<long, LineEntity> GetLines()
        {
            if (File.Exists(fileName))
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument xmlDocument = XDocument.Load(stream);

                IEnumerable<LineEntity> lines = xmlDocument.Root.Elements("Lines").Elements("LineEntity").Select(line => new LineEntity
                {
                    Id = long.Parse(line.Element("Id").Value),
                    Name = line.Element("Name").Value,
                    IsUnderground = bool.Parse(line.Element("IsUnderground").Value),
                    R = double.Parse(line.Element("R").Value, CultureInfo.InvariantCulture),
                    ConductorMaterial = line.Element("ConductorMaterial").Value,
                    LineType = line.Element("LineType").Value,
                    ThermalConstantHeat = long.Parse(line.Element("ThermalConstantHeat").Value),
                    FirstEnd = long.Parse(line.Element("FirstEnd").Value),
                    SecondEnd = long.Parse(line.Element("SecondEnd").Value),
                    Vertices = new List<Point>(line.Elements("Vertices").Elements("Point").Select(point => new Point
                    (
                        double.Parse(point.Element("X").Value, CultureInfo.InvariantCulture),
                        double.Parse(point.Element("Y").Value, CultureInfo.InvariantCulture)
                    )).ToList())
                });

                Dictionary<long, LineEntity> linesDict = lines.ToDictionary(x => x.Id, x => x);

                return linesDict;
            }
            else
            {
                return null;
            }
        }
    }
}
