using ShapeFilesParser.Business;
using ShapeFilesParser.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcename = @"RESEAU_ROUTIER\AERODROME";//your shape file base name
            ShapeManager shapeManager = new ShapeManager();
            var shapes = shapeManager.GetShapes(sourcename, new Business.Parsers.PointZParser());
        }
    }
}
