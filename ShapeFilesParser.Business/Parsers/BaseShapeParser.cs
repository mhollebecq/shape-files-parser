using ShapeFilesParser.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Parsers
{
    public abstract class BaseShapeParser<T>
    {
        public abstract GeometryType GeometryType { get; }

        public abstract T Parse(byte[] recordContent, ReadIntDelegate readInt, ReadDoubleDelegate readDouble);
    }
}
