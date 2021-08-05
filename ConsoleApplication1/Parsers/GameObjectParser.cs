using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication1.Parsers
{
    class GameObjectParser
    {
        public static Dictionary<String, dynamic> ParseGameObj(CrpReader reader, Boolean saveFile, String saveFileName, Int64 fileSize, Boolean verbose)
        {
            var retVal = new Dictionary<String, dynamic>
            {
                ["tag"] = reader.ReadString(),
                ["layer"] = reader.ReadInt32(),
                ["enabled"] = reader.ReadBoolean()
            };

            var fileContentBegin = reader.BaseStream.Position;

            
            var numProperties = reader.ReadInt32();

            for (var i = 0; i < numProperties; i++)
            {
                var isNull = reader.ReadBoolean();
                if (!isNull)
                {
                    var assemblyQualifiedName = reader.ReadString();
                    var propertyType = assemblyQualifiedName.Split(new Char[] { ',' })[0];
                    var propertyName = i + "_" + propertyType;

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
            if (verbose)
            {
                Console.WriteLine(json);
            }

            if (saveFile)
            {
                using (var stream = new FileStream(saveFileName + ".json", FileMode.Create))
                {
                    var file = new StreamWriter(stream);
                    file.Write(json);
                }
            }

            return retVal;
        }
    }
}
