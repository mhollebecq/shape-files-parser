using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Models
{
    public struct PolygonZ
    {
        private double[] marray;
        private double mmax;
        private double mmin;
        private int numParts;
        private int numPoints;
        private int[] partsArray;
        private Point[] pointsArray;
        private double xmax;
        private double xmin;
        private double[] zarray;
        private double zmax;
        private double zmin;

        public PolygonZ(double xmin, double xmax, int numParts, int numPoints, int[] partsArray, Point[] pointsArray, double zmin, double zmax, double[] zarray, double mmin, double mmax, double[] marray)
        {
            this.xmin = xmin;
            this.xmax = xmax;
            this.numParts = numParts;
            this.numPoints = numPoints;
            this.partsArray = partsArray;
            this.pointsArray = pointsArray;
            this.zmin = zmin;
            this.zmax = zmax;
            this.zarray = zarray;
            this.mmin = mmin;
            this.mmax = mmax;
            this.marray = marray;
        }
    }
}
