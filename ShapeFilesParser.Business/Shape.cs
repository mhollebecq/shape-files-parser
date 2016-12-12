using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business
{
    public class Shape
    {
        public List<Point> Points = new List<Point>();
        public List<PointZ> PointZs = new List<PointZ>();
        //public List<Record> Records = new List<Record>();

        public Shape(string name)
        {
            ParseShp(name + ".shp");
        }

        private void ParseShp(string shp)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(shp)))
            {
                byte[] headerArray = new byte[100];
                int read = reader.Read(headerArray, 0, 100);
                var fileCode = ReadInt(headerArray, 0, false);
                var fileLength = ReadInt(headerArray, 24, false);
                var version = ReadInt(headerArray, 28, true);
                var globalShapeType = ReadInt(headerArray, 32, true);
                var xMin = ReadDouble(headerArray, 36, true);
                var yMin = ReadDouble(headerArray, 44, true);
                var xMax = ReadDouble(headerArray, 52, true);
                var yMax = ReadDouble(headerArray, 60, true);
                var zMin = ReadDouble(headerArray, 68, true);
                var zMax = ReadDouble(headerArray, 76, true);
                var mMin = ReadDouble(headerArray, 84, true);
                var mMax = ReadDouble(headerArray, 92, true);

                byte[] recordHeader = new byte[8];
                while (reader.Read(recordHeader, 0, 8) != 0)
                {
                    var recordNumber = ReadInt(recordHeader, 0, false);
                    var contentLength = ReadInt(recordHeader, 4, false);

                    byte[] recordContent = new byte[2 * contentLength];
                    reader.Read(recordContent, 0, 2 * contentLength);
                    var recordShapeType = ReadInt(recordContent, 0, true);
                    switch (recordShapeType)
                    {
                        case 1:
                            AddPoint(recordContent);
                            break;
                        case 11:
                            AddPointZ(recordContent);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void AddPoint(byte[] recordContent)
        {
            var x = ReadDouble(recordContent, 4, true);
            var y = ReadDouble(recordContent, 12, true);
            Points.Add(new Point(x, y));
        }

        private void AddPointZ(byte[] recordContent)
        {
            var x = ReadDouble(recordContent, 4, true);
            var y = ReadDouble(recordContent, 12, true);
            var z = ReadDouble(recordContent, 20, true);
            var m = ReadDouble(recordContent, 28, true);
            PointZs.Add(new PointZ(x, y, z, m));
        }

        private int ReadInt(byte[] source, int index, bool littleEndian)
        {
            var data = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                var currentIndex = index + (littleEndian ? 3 - i : i);
                data[i] = source[currentIndex];
            }
            return (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | (data[3]);
        }

        private double ReadDouble(byte[] source, int index, bool littleEndian)
        {
            var data = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                var currentIndex = index + (littleEndian != BitConverter.IsLittleEndian ? 7 - i : i);
                data[i] = source[currentIndex];
            }
            return BitConverter.ToDouble(data, 0);
        }
    }

    public struct Point
    {
        public readonly double X;
        public readonly double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

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
