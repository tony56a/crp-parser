using Newtonsoft.Json;
using System;
using System.IO;

namespace ConsoleApplication1.Parsers
{
    class MaterialParser
    {
        public static MaterialStub ParseMaterial(CrpReader reader, Boolean saveFile, String saveFileName, Int64 fileSize, Boolean verbose)
        {
            var fileContentBegin = reader.BaseStream.Position;
            var retVal = new MaterialStub
            {
                shaderName = reader.ReadString(),
                numProperties = reader.ReadInt32()
            };
            for (var i = 0; i < retVal.numProperties; i++)
            {
                var propertyType = reader.ReadInt32();
                var propertyName = reader.ReadString();
                switch (propertyType)
                {
                    case 0:
                        retVal.colors[propertyName] = reader.singlarObjParser["UnityEngine.Color"]();
                        break;
                    case 1:
                        retVal.vectors[propertyName] = reader.singlarObjParser["UnityEngine.Vector4"]();
                        break;
                    case 2:
                        retVal.floats[propertyName] = reader.ReadSingle();
                        break;
                    case 3:
                        var isNull = reader.ReadBoolean();
                        if (!isNull)
                        {
                            retVal.textures[propertyName] = reader.ReadString();
                        }
                        else
                        {
                            retVal.textures[propertyName] = "";
                        }
                        break;
                }
            }

            if ((reader.BaseStream.Position - fileContentBegin) != fileSize)
            {
                var bytesToRead = (Int32)(fileSize - (reader.BaseStream.Position - fileContentBegin));
                reader.ReadBytes(bytesToRead);
            }
            var fileName = saveFileName + ".json";
            var json = JsonConvert.SerializeObject(retVal, Formatting.Indented);
            if (verbose)
            {
                Console.WriteLine("Read info file {0}", fileName);
                Console.WriteLine(json);
            }
            if (saveFile)
            {
                using var stream = new FileStream(saveFileName + ".json", FileMode.Create);
                var file = new StreamWriter(stream);
                file.Write(json);
            }

            return retVal;
        }
    }
}
