using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeFilesParser.Business.Models;

namespace ShapeFilesParser.Business.Parsers
{
    public class PolygonParser : BaseShapeParser<Polygon>
    {
        public override GeometryType GeometryType => GeometryType.Polygon;

        public override Polygon Parse(byte[] recordContent, ReadIntDelegate readInt, ReadDoubleDelegate readDouble)
        {
            int shapeType = readInt(recordContent, 0, true);

            double Xmin = readDouble(recordContent, 4, true);
            double Ymin = readDouble(recordContent, 12, true);
            double Xmax = readDouble(recordContent, 20, true);
            double Ymax = readDouble(recordContent, 28, true);

            int numParts = readInt(recordContent, 36, true);
            int numPoints = readInt(recordContent, 40, true);

            int[] parts = new int[numParts];
            Point[] points = new Point[numPoints];

            for (int iParts = 0; iParts < numParts; iParts++)
            {
                parts[iParts] = readInt(recordContent, 44 + 4 * iParts, true);
            }

            int startPointIndex = 44 + numParts * 4;
            for (int iPoints = 0; iPoints < numPoints; iPoints++)
            {
                points[iPoints] = new Point(readDouble(recordContent, startPointIndex + 16 * iPoints, true),
                    readDouble(recordContent, startPointIndex + 16 * iPoints + 8, true));
            }
            return new Polygon()
            {
                Xmin = Xmin,
                Xmax = Xmax,
                NumParts = numParts,
                NumPoints = numPoints,
                Parts = parts,
                Points = points,
                Ymax = Ymax,
                Ymin = Ymin
            };
        }
    }
}
