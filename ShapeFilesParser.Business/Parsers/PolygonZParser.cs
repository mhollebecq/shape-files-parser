using ShapeFilesParser.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Parsers
{
    public class PolygonZParser : BaseShapeParser<PolygonZ>
    {
        //private PointParser _pointParser;

        public PolygonZParser()// : this(new PointParser())
        {

        }

        //public PolygonZParser(PointParser pointParser)
        //{
        //    this._pointParser = pointParser;
        //}

        public override PolygonZ Parse(byte[] recordContent)
        {
            //PolygonZ
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
            int shapeType = ReadInt(recordContent, 0, true);

            double Xmin, Ymin, Xmax, Ymax;//Bounding box
            int numParts, numPoints;

            Xmin = ReadDouble(recordContent, 4, true);
            Ymin = ReadDouble(recordContent, 12, true);
            Xmax = ReadDouble(recordContent, 20, true);
            Ymax = ReadDouble(recordContent, 28, true);

            numParts = ReadInt(recordContent, 36, true);
            numPoints = ReadInt(recordContent, 40, true);

            int[] partsArray = new int[numParts];
            for (int iParts = 0; iParts < numParts; iParts++)
            {
                int indexPart = 44 + iParts * 4;
                partsArray[iParts] = ReadInt(recordContent, indexPart, true);
            }

            Point[] pointsArray = new Point[numPoints];
            int startPoints = 44 + (4 * numParts);
            for (int iPoints = 0; iPoints < numPoints; iPoints++)
            {
                int indexPoint = startPoints + iPoints * 16;
                pointsArray[iPoints] = new Point(
                    ReadDouble(recordContent, indexPoint, true),
                    ReadDouble(recordContent, indexPoint + 8, true));
            }

            int startZmin = startPoints + 16 * numPoints;
            double Zmin = ReadDouble(recordContent, startZmin, true);
            double Zmax = ReadDouble(recordContent, startZmin + 8, true);

            double[] Zarray = new double[numPoints];
            int startZArray = startZmin + 16;
            for (int iZarray = 0; iZarray < numPoints; iZarray++)
            {
                int indexZ = startZArray + iZarray * 8;
                Zarray[iZarray] = ReadDouble(recordContent, indexZ, true);
            }

            int startMmin = startZArray + 8 * numPoints;
            double Mmin = ReadDouble(recordContent, startMmin, true);
            double Mmax = ReadDouble(recordContent, startMmin + 8, true);

            double[] Marray = new double[numPoints];
            int startMarray = startMmin + 16;
            for(int iMarray = 0; iMarray < numPoints; iMarray++)
            {
                int indexM = startMarray + iMarray * 8;
                Marray[iMarray] = ReadDouble(recordContent, indexM, true);
            }

            int currentIndex = startMarray + 8 * numPoints;

            return new PolygonZ(Xmin, Xmax,
                numParts, numPoints,
                partsArray, pointsArray,
                Zmin, Zmax,
                Zarray,
                Mmin, Mmax,
                Marray);
        }
    }
}
