using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business
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

    public class Record<T>
    {
        public int Index { get; set; }
        public T Shape { get; set; }
        public Dictionary<string, string> Metadata { get; set; }

        public Record(int index, T shape, Dictionary<string, string> metadata)
        {
            Index = index;
            Shape = shape;
            Metadata = metadata;
        }
    }
}
