using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Models
{
    public class ShapeIndexList : List<ShapeIndex>
    {
        public int FileCode { get; set; }
        public int FileLength { get; set; }
        public int Version { get; set; }
        public GeometryType GlobalShapeType { get; set; }
        public double XMin { get; set; }
        public double YMin { get; set; }
        public double XMax { get; set; }
        public double YMax { get; set; }
        public double ZMin { get; set; }
        public double ZMax { get; set; }
        public double MMin { get; set; }
        public double MMax { get; set; }
    }
}
