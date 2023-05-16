using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaProjekat.Model
{
    public class Point
    {
        private double x;
        private double y;

        public double X
        {
            get { return x; }
            set
            {
                if (x != value)
                {
                    x = value;
                }
            }
        }

        public double Y
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                }
            }
        }

        public Point(double x, double y)
        {
            double convertedX;
            double convertedY;
            UTMToDecimalConversion.ToLatLon(x, y, 34, out convertedX, out convertedY);

            X = convertedX;
            Y = convertedY;
        }

        public Point()
        {

        }

    }
}
