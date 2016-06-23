using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class CrpDeserializer
    {
        public string filePath;
        private FileStream stream;
        private CrpReader reader;
        private AssetParser assetParser;
        private Dictionary<string, int> typeRefCount = new Dictionary<string, int>();

        public CrpDeserializer(string filePath)
        {
            stream = File.Open(filePath, FileMode.Open);
            reader = new CrpReader(stream);
        }

        public void parseFile()
        {
            string magicStr = new string(reader.ReadChars(4));
            if (magicStr.Equals(Consts.MAGICSTR))
            {
                CrpHeader header = parseHeader();
                Console.WriteLine(header);


            }
            else
            {
                throw new InvalidDataException("Invalid file format!");
            }
        }

        private CrpHeader parseHeader()
        {
            CrpHeader output = new CrpHeader();
            output.formatVersion = reader.ReadUInt16();
            output.packageName = reader.ReadString();
            output.authorName = CryptoUtils.Decrypt(reader.ReadString());
            output.pkgVersion = reader.ReadUInt32();
            output.mainAssetName = reader.ReadString();
            output.numAssets = reader.ReadInt32();
            output.contentBeginIndex = reader.ReadInt64();

            output.assets = new List<CrpAssetInfoHeader>();
            for(int i = 0; i<output.numAssets; i++)
            {
                CrpAssetInfoHeader info = new CrpAssetInfoHeader();
                info.assetName = reader.ReadString();
                info.assetChecksum = reader.ReadString();
                info.assetType = (Consts.AssetTypeMapping)(reader.ReadInt32());
                info.assetOffsetBegin = reader.ReadInt64();
                info.assetSize = reader.ReadInt64();
                output.assets.Add(info);

            }

            return output;
        }

        private void parseAssets(CrpHeader header)
        {
            for(int i =0; i<header.numAssets; i++)
            {
                bool isNullFlag = reader.ReadBoolean();
                if(!isNullFlag)
                {
                    string assemblyQualifiedName = reader.ReadString();
                    string assetType = new AssemblyName(assemblyQualifiedName).FullName;
                    long assetLen = header.assets[i].assetSize - ( sizeof(bool) + sizeof(byte) + assetType.Length );
                    string assetName = header.assets[i].assetName;

                    if (!(assetType == "UnityEngine.Mesh" && typeRefCount.ContainsKey(assetType))) {
                        assetName = reader.ReadString();
                        assetLen -= (sizeof(byte) + assetName.Length);
                    }
                    if (typeRefCount.ContainsKey(assetType))
                    {

                    }
                    assetParser.parseAsset(assetType, assetLen);
                }
            }
        }

    }
}
