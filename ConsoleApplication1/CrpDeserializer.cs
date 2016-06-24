using CrpParser;
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

        public void parseFile(Options options)
        {
            string magicStr = new string(reader.ReadChars(4));
            if (magicStr.Equals(Consts.MAGICSTR))
            {
                CrpHeader header = parseHeader();

                if (options.SaveFiles)
                {
                    string path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + header.mainAssetName + "_contents";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Environment.CurrentDirectory = (path);
                }

                if (options.Verbose)
                {
                    Console.WriteLine(header);
                }

                for (int i = 0; i < header.numAssets; i++)
                {
                    parseAssets(header, i,options.SaveFiles,options.Verbose);
                }
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

        private void parseAssets(CrpHeader header,int index,bool saveFiles,bool isVerbose)
        {
            
            bool isNullFlag = reader.ReadBoolean();
            if (!isNullFlag)
            {
                string assemblyQualifiedName = reader.ReadString();
                string assetType = assemblyQualifiedName.Split(new char[] { ',' })[0];
                long assetContentLen = header.assets[index].assetSize - (2 + assemblyQualifiedName.Length);
                string assetName = reader.ReadString();
                assetContentLen -= (1 + assetName.Length);

                string fileName = string.Format("{0}_{1}_{2}", assetName, index,header.assets[index].assetType.ToString());
                assetParser.parseObject((int)assetContentLen, assetType, saveFiles, fileName, isVerbose);
            }
 
        }

    }
}
