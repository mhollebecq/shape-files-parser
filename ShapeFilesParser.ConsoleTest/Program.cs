﻿using ShapeFilesParser.Business;
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
            ShapeManager<PointZ> shapeManager = new ShapeManager<PointZ>(new Business.Parsers.PointZParser(), 11);
            var shapes = shapeManager.ParseShapeFiles(sourcename);
        }
    }
}