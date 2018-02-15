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
        static string basePath = @"C:\Users\mathieu\Downloads\ROUTE500_2-1__SHP_LAMB93_FXX_2017-05-31\ROUTE500\1_DONNEES_LIVRAISON_2017-06-00213\R500_2-1_SHP_LAMB93_FR-ED171\";

        static void Main(string[] args)
        {
            //PointZ();
            LinqToShape();
            string sourcename = @"ADMINISTRATIF\LIMITE_ADMINISTRATIVE";//your shape file base name
            ShapeManager shapeManager = new ShapeManager();
            var shapes = shapeManager.GetInfos(System.IO.Path.Combine(basePath, sourcename));
        }

        static void LinqToShape()
        {
            string sourcename = @"RESEAU_ROUTIER\AERODROME";
            Query<Record<Point>> set = new Query<Record<Point>>(new ShapeQueryProvider(System.IO.Path.Combine(basePath, sourcename)));
            var whereResult = set.Where(s => s.Index == 2 || s.Index == 3).ToList();
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
            var shapes = shapeManager.GetShapes(System.IO.Path.Combine(basePath, sourcename), new Business.Parsers.PointZParser());
        }

        static void PolygonZ()
        {
            string sourcename = basePath + @"ADMINISTRATIF\COMMUNE";//your shape file base name
            ShapeManager shapeManager = new ShapeManager();
            var shapes = shapeManager.GetShapes(sourcename, new Business.Parsers.PolygonZParser());
        }
    }
}
