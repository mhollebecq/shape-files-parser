using ShapeFilesParser.Business;
using ShapeFilesParser.Business.Models;
using ShapeFilesParser.Business.Parsers;
using ShapeFilesParser.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.ConsoleTest
{
    class Program
    {
        static string basePath = @"C:\Users\mathi\Downloads\ROUTE500_2-1__SHP_LAMB93_FXX_2016-07-06\ROUTE500\1_DONNEES_LIVRAISON_2016-07-00089\R500_2-1_SHP_LAMB93_FR-ED161\";

        static void Main(string[] args)
        {
            LinqToShape();
            string sourcename = @"ADMINISTRATIF\LIMITE_ADMINISTRATIVE";//your shape file base name
            ShapeManager shapeManager = new ShapeManager();
            var shapes = shapeManager.GetInfos(System.IO.Path.Combine(basePath, sourcename));
        }

        static void LinqToShape()
        {
            string sourcename = @"RESEAU_ROUTIER\AERODROME";
            Query<Record<PointZ>> set = new Query<Record<Business.Models.PointZ>>(new ShapeQueryProvider());
            var whereResult = set.Where(s => s.Index == 2).ToList();
        }

        static void PolylineZ()
        {
            string sourcename = @"ADMINISTRATIF\LIMITE_ADMINISTRATIVE";//your shape file base name
            ShapeManager shapeManager = new ShapeManager();
            var shapes = shapeManager.GetShapes(System.IO.Path.Combine(basePath, sourcename), new PolylineZParser());
        }

        static void PointZ()
        {
            string sourcename = @"RESEAU_ROUTIER\AERODROME";//your shape file base name
            ShapeManager shapeManager = new ShapeManager();
            var shapes = shapeManager.GetShapes(sourcename, new Business.Parsers.PointZParser());
        }

        static void PolygonZ()
        {
            string sourcename = basePath + @"ADMINISTRATIF\COMMUNE";//your shape file base name
            ShapeManager shapeManager = new ShapeManager();
            var shapes = shapeManager.GetShapes(sourcename, new Business.Parsers.PolygonZParser());
        }
    }
}
