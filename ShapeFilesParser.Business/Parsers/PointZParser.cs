using ShapeFilesParser.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Parsers
{
    public class PointZParser : BaseShapeParser<PointZ>
    {
        public override GeometryType GeometryType
        {
            get
            {
                return GeometryType.PointZ;
            }
        }

        public override PointZ Parse(byte[] recordContent, ReadIntDelegate readInt, ReadDoubleDelegate readDouble)
        {
            var x = readDouble(recordContent, 4, true);
            var y = readDouble(recordContent, 12, true);
            var z = readDouble(recordContent, 20, true);
            var m = readDouble(recordContent, 28, true);
            return new PointZ(x, y, z, m);
        }
    }
}
