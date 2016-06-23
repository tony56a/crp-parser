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
            assetParser = new AssetParser(reader);
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
            string path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + output.mainAssetName + "_contents";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Environment.CurrentDirectory = (path);

            for (int i = 0; i < output.numAssets; i++)
            {
                parseAssets(output,output.assets[i].assetSize);
            }
            return output;
        }

        private void parseAssets(CrpHeader header,long assetLength)
        {
            
            bool isNullFlag = reader.ReadBoolean();
            if (!isNullFlag)
            {
                string assemblyQualifiedName = reader.ReadString();
                string assetType = assemblyQualifiedName.Split(new char[] { ',' })[0];
                long assetContentLen = assetLength - (2 + assemblyQualifiedName.Length);
                string assetName = reader.ReadString();
                assetContentLen -= (1 + assetName.Length);

                assetParser.parseObject((int)assetContentLen, assetType, true, assetName);
            }

           
        }

    }
}
