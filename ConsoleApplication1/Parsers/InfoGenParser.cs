using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication1.Parsers
{
    public class InfoGenParser
    {
        public static Dictionary<String, dynamic> ParseInfoGen(CrpReader reader, Boolean saveFile, String saveFileName, Int64 fileSize, Boolean verbose)
        {
            var retVal = new Dictionary<String, dynamic>();

            var fileContentBegin = reader.BaseStream.Position;

            var numProperties = reader.ReadInt32();
            for (var i = 0; i < numProperties; i++)
            {
                var isNull = reader.ReadBoolean();
                if (!isNull)
                {
                    var assemblyQualifiedName = reader.ReadString();
                    var propertyType = assemblyQualifiedName.Split(new Char[] { ',' })[0];
                    var propertyName = reader.ReadString();
                    if (propertyType.Contains("[]"))
                    {
                        retVal[propertyName] = reader.readUnityArray(propertyType);
                    }
                    else
                    {
                        retVal[propertyName] = reader.readUnityObj(propertyType);
                    }

                }
            }

            if ((reader.BaseStream.Position - fileContentBegin) != fileSize)
            {
                var bytesToRead = (Int32)(fileSize - (reader.BaseStream.Position - fileContentBegin));
                reader.ReadBytes(bytesToRead);
            }
            var json = JsonConvert.SerializeObject(retVal, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter());
            var fileName = saveFileName + ".json";
            if (verbose)
            {
                Console.WriteLine("Read info file {0}", fileName);
                Console.WriteLine(json);
            }
            if (saveFile)
            {
                var file = new StreamWriter(new FileStream(fileName, FileMode.Create));
                file.Write(json);
                file.Close();
            }

            return retVal;
        }
    }
}
