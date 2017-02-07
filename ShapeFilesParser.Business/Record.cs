using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business
{
    public class Record<T>
    {
        public int Index { get; set; }
        public T Shape { get; set; }
        public Dictionary<string, string> Metadata { get; set; }

        public Record(int index, T shape, Dictionary<string, string> metadata)
        {
            Index = index;
            Shape = shape;
            Metadata = metadata;
        }
    }
}
