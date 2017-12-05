using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Models
{
    public struct PolygonZ
    {
        public double Xmin { get; set; }
        public double Ymin { get; set; }
        public double Xmax { get; set; }
        public double Ymax { get; set; }
        public double Mmin { get; set; }
        public double Zmin { get; set; }
        public double Mmax { get; set; }
        public double[] Marray { get; }
        public double Zmax { get; set; }
        public double[] Zarray { get; }
        public int NumParts { get; set; }
        public int NumPoints { get; set; }
        public int[] Parts { get; set; }
        public Point[] Points { get; set; }

        public PolygonZ(double xmin, double xmax, double ymin, double ymax, int numParts, int numPoints, int[] partsArray, Point[] pointsArray, double zmin, double zmax, double[] zarray, double mmin, double mmax, double[] marray)
        {
            this.Xmin = xmin;
            this.Xmax = xmax;
            this.Ymin = ymin;
            this.Ymax = ymax;
            this.NumParts = numParts;
            this.NumPoints = numPoints;
            this.Parts = partsArray;
            this.Points = pointsArray;
            this.Zmin = zmin;
            this.Zmax = zmax;
            this.Zarray = zarray;
            this.Mmin = mmin;
            this.Mmax = mmax;
            this.Marray = marray;
        }
    }
}
