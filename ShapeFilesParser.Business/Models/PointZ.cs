using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Models
{
    public struct PointZ
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;
        public readonly double M;

        public PointZ(double x, double y, double z, double m)
        {
            X = x;
            Y = y;
            Z = z;
            M = m;
        }
    }
}
