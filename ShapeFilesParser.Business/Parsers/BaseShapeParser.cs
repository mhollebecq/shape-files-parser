using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Parsers
{
    public abstract class BaseShapeParser<T>
    {
        public abstract T Parse(byte[] recordContent);

        public int ReadShort(byte[] source, int index, bool littleEndian)
        {
            var data = new byte[2];
            for (int i = 0; i < 2; i++)
            {
                var currentIndex = index + (littleEndian ? 1 - i : i);
                data[i] = source[currentIndex];
            }
            return (data[0] << 8) | (data[1]);
        }

        public int ReadInt(byte[] source, int index, bool littleEndian)
        {
            var data = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                var currentIndex = index + (littleEndian ? 3 - i : i);
                data[i] = source[currentIndex];
            }
            return (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | (data[3]);
        }

        public double ReadDouble(byte[] source, int index, bool littleEndian)
        {
            var data = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                var currentIndex = index + (littleEndian != BitConverter.IsLittleEndian ? 7 - i : i);
                data[i] = source[currentIndex];
            }
            return BitConverter.ToDouble(data, 0);
        }
    }
}
