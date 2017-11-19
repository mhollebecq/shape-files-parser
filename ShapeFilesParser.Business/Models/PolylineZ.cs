using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Models
{
    public struct PolylineZ
    {
        public double Xmin { get; set; }
        public double Ymin { get; set; }
        public double Xmax { get; set; }
        public double Ymax { get; set; }

        public int NumParts { get; set; }
        public int NumPoints { get; set; }
        public int[] Parts { get; set; }
        public Point[] Points { get; set; }
        public double Zmin { get; internal set; }
        public double Zmax { get; internal set; }
        public double Mmin { get; internal set; }
        public double Mmax { get; internal set; }
        public double[] ZArray { get; internal set; }
        public double[] MArray { get; internal set; }
    }
}
