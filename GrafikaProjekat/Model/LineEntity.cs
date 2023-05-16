using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GrafikaProjekat.Model
{
    public class LineEntity
    {
        private long id;
        private string name;
        private bool isUnderground;
        private double r;
        private string conductorMaterial;
        private string lineType;
        private long thermalConstantHeat;
        private long firstEnd;
        private long secondEnd;
        private List<Point> vertices;

        public long Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                }
            }
        }

        public bool IsUnderground
        {
            get { return isUnderground; }
            set
            {
                if (isUnderground != value)
                {
                    isUnderground = value;
                }
            }
        }

        public double R
        {
            get { return r; }
            set
            {
                if (r != value)
                {
                    r = value;
                }
            }
        }

        public string ConductorMaterial
        {
            get { return conductorMaterial; }
            set
            {
                if (conductorMaterial != value)
                {
                    conductorMaterial = value;
                }
            }
        }

        public string LineType
        {
            get { return lineType; }
            set
            {
                if (lineType != value)
                {
                    lineType = value;
                }
            }
        }

        public long ThermalConstantHeat
        {
            get { return thermalConstantHeat; }
            set
            {
                if (thermalConstantHeat != value)
                {
                    thermalConstantHeat = value;
                }
            }
        }

        public long FirstEnd
        {
            get { return firstEnd; }
            set
            {
                if (firstEnd != value)
                {
                    firstEnd = value;
                }
            }
        }

        public long SecondEnd
        {
            get { return secondEnd; }
            set
            {
                if (secondEnd != value)
                {
                    secondEnd = value;
                }
            }
        }

        public List<Point> Vertices
        {
            get { return vertices; }
            set
            {
                if (vertices != value)
                {
                    vertices = value;
                }
            }
        }
    }
}
