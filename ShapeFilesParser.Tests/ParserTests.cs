using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShapeFilesParser.Business;
using ShapeFilesParser.Business.Parsers;

namespace ShapeFilesParser.Tests
{
    [TestClass]
    public class ParserTests
    {
        private string _basePath; 

        public ParserTests()
        {
#warning use your own files an base folder to test
            _basePath = @"BaseFolder\";//your shape file base name
        }

        [TestMethod]
        public void ParsePointZ()
        {
            string fullpath = _basePath + @"RESEAU_ROUTIER\AERODROME";

            PointZParser parser = new PointZParser();
            ShapeManager shapeManager = new ShapeManager();
            var index = shapeManager.GetInfos(fullpath);

            Assert.IsTrue(index.GlobalShapeType == Business.Models.GeometryType.PointZ);

            var shapesPointZ = shapeManager.GetShapes(fullpath, parser);
            var shape = shapeManager.GetShape(fullpath, parser, index[0]);
        }

        [TestMethod]
        public void ParsePolygonZ()
        {
            string fullpath = _basePath + @"ADMINISTRATIF\COMMUNE";

            PolygonZParser parser = new PolygonZParser();
            ShapeManager shapeManager = new ShapeManager();
            var index = shapeManager.GetInfos(fullpath);

            Assert.IsTrue(index.GlobalShapeType == Business.Models.GeometryType.PolygonZ);

            var shapesPointZ = shapeManager.GetShapes(fullpath, parser);
            var shape = shapeManager.GetShape(fullpath, parser, index[0]);
        }
    }
}
