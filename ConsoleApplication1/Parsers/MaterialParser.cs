using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Parsers
{
    class MaterialParser
    {
        public static MaterialStub parseMaterial(CrpReader reader, bool saveFile, string saveFileName,long fileSize,bool verbose)
        {
            long fileContentBegin = reader.BaseStream.Position;
            MaterialStub retVal = new MaterialStub();
            retVal.shaderName = reader.ReadString();
            retVal.numProperties = reader.ReadInt32();
            for(int i =0; i< retVal.numProperties; i++)
            {
                int propertyType = reader.ReadInt32();
                string propertyName = reader.ReadString();
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
                        bool isNull = reader.ReadBoolean();
                        if (!isNull)
                        {
                            retVal.textures[propertyName] = reader.ReadString();
                        }else
                        {
                            retVal.textures[propertyName] = "";
                        }
                        break;
                }
            }

            if((reader.BaseStream.Position-fileContentBegin) != fileSize)
            {
                int bytesToRead = (int)(fileSize - (reader.BaseStream.Position - fileContentBegin));
                reader.ReadBytes(bytesToRead);
            }
            string fileName = saveFileName + ".json";
            string json = JsonConvert.SerializeObject(retVal, Formatting.Indented);
            if(verbose)
            {
                Console.WriteLine("Read info file {0}", fileName);
                Console.WriteLine(json);
            }
            if (saveFile)
            {
                StreamWriter file = new StreamWriter(new FileStream(saveFileName + ".json", FileMode.Create));
                file.Write(json);
                file.Close();
            }

            return retVal;
        }
    }
}
