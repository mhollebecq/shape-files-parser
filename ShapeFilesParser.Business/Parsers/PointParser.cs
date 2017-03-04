using ShapeFilesParser.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Parsers
{
    public class PointParser : BaseShapeParser<Point>
    {
        public override GeometryType GeometryType
        {
            get
            {
                return GeometryType.Point;
            }
        }

        public override Point Parse(byte[] recordContent, ReadIntDelegate readInt, ReadDoubleDelegate readDouble)
        {
            int shapeType = readInt(recordContent, 0, true);
            double x = readDouble(recordContent, 4, true);
            double y = readDouble(recordContent, 12, true);

            return new Point(x, y);
        }
    }
}
