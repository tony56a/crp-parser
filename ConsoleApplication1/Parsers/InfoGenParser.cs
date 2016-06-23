using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Parsers
{
    public class InfoGenParser
    {
        public static Dictionary<string,dynamic> parseInfoGen(CrpReader reader, bool saveFile, string saveFileName, long fileSize)
        {
            Dictionary<string, dynamic> retVal = new Dictionary<string, dynamic>();

            long fileContentBegin = reader.BaseStream.Position;

            int numProperties = reader.ReadInt32();
            for(int i = 0; i < numProperties; i++)
            {
                bool isNull = reader.ReadBoolean();
                if (!isNull)
                {
                    string assemblyQualifiedName = reader.ReadString();
                    string propertyType = assemblyQualifiedName.Split(new char[] { ',' })[0];
                    string propertyName = reader.ReadString();
                    if (propertyType.Contains("[]"))
                    {
                        retVal[propertyName]=reader.readUnityArray(propertyType);
                    }else
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
            if (saveFile)
            {
                if (saveFile)
                {
                    string json = JsonConvert.SerializeObject(retVal, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter());
                    StreamWriter file = new StreamWriter(new FileStream(saveFileName + ".json", FileMode.Create));
                    file.Write(json);
                    file.Close();
                }
            }

            return retVal;
        }
    }
}
