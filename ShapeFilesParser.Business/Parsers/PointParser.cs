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
        public override Point Parse(byte[] recordContent)
        {
            int shapeType = ReadInt(recordContent, 0, true);
            double x = ReadDouble(recordContent, 4, true);
            double y = ReadDouble(recordContent, 12, true);

            return new Point(x, y);
        }
    }
}
