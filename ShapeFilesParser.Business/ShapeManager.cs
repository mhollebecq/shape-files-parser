﻿using ShapeFilesParser.Business.Models;
using ShapeFilesParser.Business.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Business
{
    public class ShapeManager
    {

        public ShapeManager()//BaseShapeParser<T> parser)
        {
            //this.parser = parser;
            //this.recordShapeType = (int)parser.GeometryType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceName">File name without extension</param>
        public ShapeIndexList GetInfos(string sourceName)
        {

            var metadatas = ParseMetadatas(sourceName + ".dbf");
            var indexList = ParseIndex(sourceName + ".shx");
            for (int i = 0; i < indexList.Count; i++)
            {
                indexList[i].Metadatas = metadatas[i];
            }

            return indexList;
        }
        
        public List<Record<T>> GetShapes<T>(string sourceName, BaseShapeParser<T> parser)
        {

            var metadatas = ParseMetadatas(sourceName + ".dbf");
            var shapes = ParseShp(sourceName + ".shp", parser);
            List<Record<T>> recordList = new List<Record<T>>();
            for (int i = 0; i < shapes.Count; i++)
            {
                Record<T> newRecord = new Record<T>(i, shapes[i], metadatas[i]);
                recordList.Add(newRecord);
            }

            return recordList;
        }

        public Record<T> GetShape<T>(string sourceName, BaseShapeParser<T> parser, ShapeIndex index)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(sourceName + ".shp")))
            {
                byte[] bytesHeader = new byte[8];
                byte[] bytesRecord = new byte[2* index.ContentLength];
                reader.BaseStream.Seek(2 * index.Offset, SeekOrigin.Begin);
                reader.Read(bytesHeader, 0, 8);
                var recordNumber = ReadInt(bytesHeader, 0, false);
                var contentLength = ReadInt(bytesHeader, 4, false);

                reader.Read(bytesRecord, 0, 2* index.ContentLength);
                T shape = parser.Parse(bytesRecord, ReadInt, ReadDouble);
                Record<T> record = new Record<T>(recordNumber, shape, index.Metadatas);
                return record;
            }
        }


        private List<Dictionary<string, string>> ParseMetadatas(string dbf)
        {
            //          0               1 byte              Valid dBASE for DOS file; bits 0 - 2 indicate version number, bit 3 indicates the presence of a dBASE for DOS memo file, bits 4 - 6 indicate the presence of a SQL table, bit 7 indicates the presence of any memo file(either dBASE m PLUS or dBASE for DOS)
            //          1 - 3           3 bytes             Date of last update; formatted as YYMMDD
            //          4 - 7           32 - bit number     Number of records in the database file
            //          8 - 9           16 - bit number     Number of bytes in the header
            //          10 - 11         16 - bit number     Number of bytes in the record
            //          12 - 13         2 bytes             Reserved; fill with 0
            //          14              1 byte              Flag indicating incomplete transaction(1)
            //          15              1 byte              Encryption flag(2)
            //          16 - 27         12 bytes            Reserved for dBASE for DOS in a multi - user environment
            //          28              1 byte              Production.mdx file flag; 01H if there is a production.mdx file, 00H if not
            //          29              1 byte              Language driver ID
            //          30 - 31         2 bytes             Reserved; fill with 0
            //          32 - n(3)(4)    32 bytes each       Field descriptor array(the structure of this array is shown in Table Database field descriptor bytes)
            //          n + 1           1 byte              0DH as the field descriptor array terminator

            //          1.The ISMARKEDO function checks this flag.BEGIN TRANSACTION sets it to 01, END TRANSACTION and ROLLBACK reset it to 00.
            //          2.If this flag is set to 01H, the message Database encrypted appears.Changing this flag to 00H removes the message, but does not decrypt the file.
            //          3.n is the last byte in the field descriptor array. The size of the array depends on the number of fields in the database file.n is equal to 31 + 32 * (the number of fields).
            //          4.The maximum number of fields is 255.

            List<Dictionary<string, string>> records = new List<Dictionary<string, string>>();
            using (BinaryReader reader = new BinaryReader(File.OpenRead(dbf)))
            {
                byte[] headerPart1Array = new byte[32];
                int read = reader.Read(headerPart1Array, 0, 32);
                byte dBASE = headerPart1Array[0];
                int versionNumber = (dBASE & 0xE0) >> 5;
                int dbaseForDosMemoFilePresence = (dBASE & 0x10) >> 4;
                int sqlTablePresence = (dBASE & 0x0E) >> 1;
                int memoFilePresence = dBASE & 0x01;
                DateTime lastUpdate = new DateTime(headerPart1Array[1] + 1900, headerPart1Array[2], headerPart1Array[3]);
                int numberOfRecords = ReadInt(headerPart1Array, 4, true);
                int numberOfBytesInHeader = ReadShort(headerPart1Array, 8, true);
                int numberOfBytesInRecord = ReadShort(headerPart1Array, 10, true);
                byte incomplete = headerPart1Array[14];
                byte encryption = headerPart1Array[15];
                byte productionFlag = headerPart1Array[28];
                byte langageDriverId = headerPart1Array[29];

                int numberOfField = (numberOfBytesInHeader - 33) / 32;
                List<MetadataColumn> metadatasColumns = new List<MetadataColumn>();
                for (int indexField = 0; indexField < numberOfField; indexField++)
                {
                    byte[] fieldDescriptorArray = new byte[32];
                    read = reader.Read(fieldDescriptorArray, 0, 32);

                    //0 - 10    11 bytes    Field name in ASCII(zero - filled)
                    //11        1 byte      Field type in ASCII(C, D, F, L, M, or N)
                    //12 - 15   4 bytes     Reserved
                    //16        1 byte      Field length in binary(1)
                    //17        1 byte      Field decimal count in binary
                    //18 - 19   2 bytes     Work area ID
                    //20        1 byte      Example
                    //21 - 30   10 bytes    Reserved
                    //31        1 byte      Production MDX field flag; 01H if field has an index tag in the production MDX file, 00H if not

                    //1.The maximum length of a field is 254(FEH).

                    StringBuilder sb = new StringBuilder();
                    for (int indexName = 0; indexName < 11; indexName++)
                    {
                        byte charAsByte = fieldDescriptorArray[indexName];
                        if (charAsByte == 0) break;
                        sb.Append((char)charAsByte);
                    }
                    var fieldName = sb.ToString();
                    char fieldType = (char)fieldDescriptorArray[11];
                    byte fieldLength = fieldDescriptorArray[16];
                    byte fieldDecimal = fieldDescriptorArray[17];
                    int workAreaID = ReadShort(fieldDescriptorArray, 18, true);
                    byte example = fieldDescriptorArray[20];
                    byte productionMDXFieldFlag = fieldDescriptorArray[31];

                    //C(Character)   All ASCII characters(padded with whitespaces up to the field's length)
                    //D(Date)    Numbers and a character to separate month, day, and year(stored internally as 8 digits in YYYYMMDD format)
                    //F(Floating point) - .0123456789(right justified, padded with whitespaces)
                    //L(Logical) YyNnTtFf ? (? when not initialized)
                    //M(Memo)    All ASCII characters(stored internally as 10 digits representing a.dbt block number, right justified, padded with whitespaces)
                    //N(Numeric) - .0123456789(right justified, padded with whitespaces)

                    MetadataColumn column = new MetadataColumn();
                    column.FieldName = fieldName;
                    column.FieldType = fieldType;
                    column.FieldLength = fieldLength;
                    column.FieldDecimal = fieldDecimal;
                    column.WorkAreaId = workAreaID;
                    column.Example = example;
                    column.ProductionMDXFieldFlag = productionMDXFieldFlag;
                    metadatasColumns.Add(column);
                }

                byte fieldDescriptorArrayTerminator = reader.ReadByte();//I'll be back


                byte[] arrayRecord = new byte[numberOfBytesInRecord];
                while ((read = reader.Read(arrayRecord, 0, numberOfBytesInRecord)) == numberOfBytesInRecord)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int indexRecord = 0; indexRecord < numberOfBytesInRecord; indexRecord++)
                    {
                        byte charAsByte = arrayRecord[indexRecord];
                        if (charAsByte == 0) break;
                        sb.Append((char)charAsByte);
                    }
                    var totalRecord = sb.ToString();
                    Dictionary<string, string> fields = new Dictionary<string, string>();
                    totalRecord = totalRecord.Substring(1);
                    foreach (var column in metadatasColumns)
                    {
                        var my = totalRecord.Substring(0, column.FieldLength);
                        fields.Add(column.FieldName, my);
                        totalRecord = totalRecord.Substring(column.FieldLength, totalRecord.Length - column.FieldLength);
                    }

                    records.Add(fields);
                }
            }
            return records;
        }

        private ShapeIndexList ParseIndex(string shx)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(shx)))
            {
                ShapeIndexList list = new ShapeIndexList();
                byte[] headerArray = new byte[100];
                int read = reader.Read(headerArray, 0, 100);
                list.FileCode = ReadInt(headerArray, 0, false);
                list.FileLength = ReadInt(headerArray, 24, false);
                list.Version = ReadInt(headerArray, 28, true);
                list.GlobalShapeType = (GeometryType)ReadInt(headerArray, 32, true);
                list.XMin = ReadDouble(headerArray, 36, true);
                list.YMin = ReadDouble(headerArray, 44, true);
                list.XMax = ReadDouble(headerArray, 52, true);
                list.YMax = ReadDouble(headerArray, 60, true);
                list.ZMin = ReadDouble(headerArray, 68, true);
                list.ZMax = ReadDouble(headerArray, 76, true);
                list.MMin = ReadDouble(headerArray, 84, true);
                list.MMax = ReadDouble(headerArray, 92, true);

                byte[] recordHeader = new byte[8];
                while (reader.Read(recordHeader, 0, 8) != 0)
                {
                    ShapeIndex shapeIndex = new ShapeIndex();
                    shapeIndex.Offset = ReadInt(recordHeader, 0, false);
                    shapeIndex.ContentLength = ReadInt(recordHeader, 4, false);
                    list.Add(shapeIndex);
                }

                return list;
            }
        }

        public List<T> ParseShp<T>(string shp, BaseShapeParser<T> parser)
        {
            List<T> shapes = new List<T>();
            using (BinaryReader reader = new BinaryReader(File.OpenRead(shp)))
            {
                byte[] headerArray = new byte[100];
                int read = reader.Read(headerArray, 0, 100);
                var fileCode = ReadInt(headerArray, 0, false);
                var fileLength = ReadInt(headerArray, 24, false);
                var version = ReadInt(headerArray, 28, true);
                var globalShapeType = ReadInt(headerArray, 32, true);
                var xMin = ReadDouble(headerArray, 36, true);
                var yMin = ReadDouble(headerArray, 44, true);
                var xMax = ReadDouble(headerArray, 52, true);
                var yMax = ReadDouble(headerArray, 60, true);
                var zMin = ReadDouble(headerArray, 68, true);
                var zMax = ReadDouble(headerArray, 76, true);
                var mMin = ReadDouble(headerArray, 84, true);
                var mMax = ReadDouble(headerArray, 92, true);

                if (globalShapeType != (int)parser.GeometryType) throw new InvalidOperationException($"Invalid record shape type : expected {parser.GeometryType} but found {globalShapeType}");
                byte[] recordHeader = new byte[8];
                while (reader.Read(recordHeader, 0, 8) != 0)
                {
                    var recordNumber = ReadInt(recordHeader, 0, false);
                    var contentLength = ReadInt(recordHeader, 4, false);

                    byte[] recordContent = new byte[2 * contentLength];
                    reader.Read(recordContent, 0, 2 * contentLength);
                    var recordShapeType = ReadInt(recordContent, 0, true);

                    shapes.Add(parser.Parse(recordContent, new ReadIntDelegate(ReadInt), new ReadDoubleDelegate(ReadDouble)));
                }
            }

            return shapes;
        }

        private int ReadShort(byte[] source, int index, bool littleEndian)
        {
            var data = new byte[2];
            for (int i = 0; i < 2; i++)
            {
                var currentIndex = index + (littleEndian ? 1 - i : i);
                data[i] = source[currentIndex];
            }
            return (data[0] << 8) | (data[1]);
        }

        private int ReadInt(byte[] source, int index, bool littleEndian)
        {
            var data = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                var currentIndex = index + (littleEndian ? 3 - i : i);
                data[i] = source[currentIndex];
            }
            return (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | (data[3]);
        }

        private double ReadDouble(byte[] source, int index, bool littleEndian)
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
    public delegate double ReadDoubleDelegate(byte[] source, int index, bool littleEndian);
    public delegate int ReadIntDelegate(byte[] source, int index, bool littleEndian);
}
