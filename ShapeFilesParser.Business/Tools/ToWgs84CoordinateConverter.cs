using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Tools
{
    public class ToWgs84CoordinateConverter
    {
        private double _halfMajorAxis;
        private double _flattening;
        private double _firstParallel;
        private double _secondParallel;
        private double _longOrigin;
        private double _latOrigin;
        private double _xOrigin;
        private double _yOrigin;

        private static ToWgs84CoordinateConverter _fromLambert93;

        public static ToWgs84CoordinateConverter FromLambert93
        {
            get
            {
                if (_fromLambert93 == null)
                    _fromLambert93 = new ToWgs84CoordinateConverter(6378137, 1.0 / 298.256222101, 44, 49, 3, 46.30, 700000, 6600000);
                return _fromLambert93;
            }
        }


        private ToWgs84CoordinateConverter(double halfMajorAxis, double flattening, double firstParallel, double secondeParallel, double longOrigin, double latOrigin, double xorigin, double yOrigin)
        {
            _halfMajorAxis = halfMajorAxis;
            _flattening = flattening;
            _firstParallel = firstParallel;
            _secondParallel = secondeParallel;
            _longOrigin = longOrigin;
            _latOrigin = latOrigin;
            _xOrigin = xorigin;
            _yOrigin = yOrigin;
        }

        public void Convert(double sourceX, double sourceY, out double lat, out double lon)
        {
            throw new NotImplementedException();
        }
    }
}
