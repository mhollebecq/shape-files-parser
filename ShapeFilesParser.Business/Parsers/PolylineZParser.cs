using ShapeFilesParser.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Parsers
{
    public class PolylineZParser : BaseShapeParser<PolylineZ>
    {
        public override GeometryType GeometryType => GeometryType.PolyLineZ;

        public override PolylineZ Parse(byte[] recordContent, ReadIntDelegate readInt, ReadDoubleDelegate readDouble)
        {
            //PolyLine
            //{
            //                Double[4] Box // Bounding Box
            //                Integer NumParts // Number of Parts
            //                Integer NumPoints // Total Number of Points
            //                Integer[NumParts] Parts // Index to First Point in Part
            //                Point[NumPoints] Points // Points for All Parts
            //                Double[2] Z Range // Bounding Z Range
            //                Double[NumPoints] Z Array // Z Values for All Points
            //                Double[2] M Range // Bounding Measure Range
            //                Double[NumPoints] M Array // Measures
            //}
            int shapeType = readInt(recordContent, 0, true);

            double Xmin = readDouble(recordContent, 4, true);
            double Ymin = readDouble(recordContent, 12, true);
            double Xmax = readDouble(recordContent, 20, true);
            double Ymax = readDouble(recordContent, 28, true);

            int numParts = readInt(recordContent, 36, true);
            int numPoints = readInt(recordContent, 40, true);

            int[] parts = new int[numParts];
            Point[] points = new Point[numPoints];

            for (int iParts = 0; iParts < numParts; iParts++)
            {
                parts[iParts] = readInt(recordContent, 44 + 4 * iParts, true);
            }

            int startPointIndex = 44 + numParts * 4;
            for (int iPoints = 0; iPoints < numPoints; iPoints++)
            {
                points[iPoints] = new Point(readDouble(recordContent, startPointIndex + 16 * iPoints, true),
                    readDouble(recordContent, startPointIndex + 16 * iPoints + 8, true));
            }

            int startZBoundIndex = startPointIndex + numPoints * 16;
            double Zmin = readDouble(recordContent, startZBoundIndex, true);
            double Zmax = readDouble(recordContent, startZBoundIndex + 8, true);
            double[] ZArray = new double[numPoints];
            for(int iPoint = 0; iPoint < numPoints; iPoint++)
            {
                ZArray[iPoint] = readDouble(recordContent, startZBoundIndex + 16 + iPoint * 8, true);
            }

            int startMBoundIndex = startZBoundIndex + 16 + numPoints * 8;
            double Mmin = readDouble(recordContent, startMBoundIndex, true);
            double Mmax = readDouble(recordContent, startMBoundIndex + 8, true);
            double[] MArray = new double[numPoints];
            for(int iPoint = 0; iPoint<numPoints; iPoint++)
            {
                MArray[iPoint] = readDouble(recordContent, startMBoundIndex + 16 + iPoint * 8, true);
            }

            return new PolylineZ()
            {
                Xmin = Xmin,
                Xmax = Xmax,
                NumParts = numParts,
                NumPoints = numPoints,
                Parts = parts,
                Points = points,
                Ymax = Ymax,
                Ymin = Ymin,
                Zmin = Zmin,
                Zmax = Zmax,
                Mmin = Mmin,
                Mmax = Mmax,
                ZArray = ZArray,
                MArray = MArray
            };
        }
    }
}
