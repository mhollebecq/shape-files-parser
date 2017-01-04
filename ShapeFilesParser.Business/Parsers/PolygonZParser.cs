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
            double Xmin, Ymin, Xmax, Ymax;
            Xmin = ReadDouble(recordContent, 4, true);
            Ymin = ReadDouble(recordContent, 8, true);
            Xmax = ReadDouble(recordContent, 12, true);
            Ymax = ReadDouble(recordContent, 16, true);
            throw new NotImplementedException();
        }
    }
}
