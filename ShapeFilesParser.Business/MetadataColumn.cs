namespace ShapeFilesParser.Business
{
    internal class MetadataColumn
    {
        public MetadataColumn()
        {
        }

        public byte Example { get; internal set; }
        public byte FieldDecimal { get; internal set; }
        public byte FieldLength { get; internal set; }
        public string FieldName { get; internal set; }
        public char FieldType { get; internal set; }
        public byte ProductionMDXFieldFlag { get; internal set; }
        public int WorkAreaId { get; internal set; }
    }
}