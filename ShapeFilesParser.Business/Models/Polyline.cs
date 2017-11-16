using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Models
{
    public struct Polyline
    {
        public double Xmin { get; set; }
        public double Ymin { get; set; }
        public double Xmax { get; set; }
        public double Ymax { get; set; }

        public int NumParts { get; set; }
        public int NumPoints { get; set; }
        public int[] Parts { get; set; }
        public Point[] Points { get; set; }
    }
}
