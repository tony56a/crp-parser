using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Parsers
{
    class GameObjectParser
    {
        public static Dictionary<string, dynamic> parseGameObj(CrpReader reader, bool saveFile, string saveFileName, long fileSize, bool verbose)
        {
            Dictionary<string, dynamic> retVal = new Dictionary<string, dynamic>();

            long fileContentBegin = reader.BaseStream.Position;

            retVal["tag"] = reader.ReadString();
            retVal["layer"] = reader.ReadInt32();
            retVal["enabled"] = reader.ReadBoolean();

            int numProperties = reader.ReadInt32();

            for (int i = 0; i < numProperties; i++)
            {
                bool isNull = reader.ReadBoolean();
                if (!isNull)
                {
                    string assemblyQualifiedName = reader.ReadString();
                    string propertyType = assemblyQualifiedName.Split(new char[] { ',' })[0];
                    string propertyName = i + "_" + propertyType;
                    
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
                int bytesToRead = (int)(fileSize - (reader.BaseStream.Position - fileContentBegin));
                reader.ReadBytes(bytesToRead);
            }
            string json = JsonConvert.SerializeObject(retVal, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter());
            if (verbose)
            {
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
