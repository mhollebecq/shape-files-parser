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

        public override PointZ Parse(byte[] recordContent)
        {
            var x = ReadDouble(recordContent, 4, true);
            var y = ReadDouble(recordContent, 12, true);
            var z = ReadDouble(recordContent, 20, true);
            var m = ReadDouble(recordContent, 28, true);
            return new PointZ(x, y, z, m);
        }
    }
}
