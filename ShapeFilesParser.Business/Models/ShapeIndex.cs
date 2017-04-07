using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business.Models
{
    public class ShapeIndex
    {
        public int ContentLength { get; internal set; }
        public int Offset { get; internal set; }
        public Dictionary<string,string> Metadatas { get; set; }
    }
}
