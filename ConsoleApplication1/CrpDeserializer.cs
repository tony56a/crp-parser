using ConsoleApplication1.Parsers;
using CrpParser;
using CrpParser.Utils;
using ImageMagick;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication1
{
	public class CrpDeserializer
    {
        public string filePath;
        private FileStream stream;
        private CrpReader reader;
        private AssetParser assetParser;
        private Dictionary<string, int> typeRefCount = new Dictionary<string, int>();

		/// <summary>
		/// Initializes the object by opening the specified CRP file. Note and be prepared to handle the exceptions
		/// that may be thrown.
		/// </summary>
		/// <param name="filePath">Path to the CRP file that needs to be opened.</param>
		/// <exception cref="System.IO.IOException">Thrown if i.e the filePath parameter is incorrect, file is missing,
		/// in use, etc.</exception>
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
                if (options.SaveFiles)
                {
                    StreamWriter file = new StreamWriter(new FileStream(header.mainAssetName + "_header.json", FileMode.Create));
                    string json = JsonConvert.SerializeObject(header, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter());
                    file.Write(json);
                    file.Close();
                }
                if (header.isLut)
                {
                    parseLut(header, options.SaveFiles, options.Verbose);
                }
                else
                {
                    for (int i = 0; i < header.numAssets; i++)
                    {
                        parseAssets(header, i, options.SaveFiles, options.Verbose);
                    }
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
            string encryptedAuthor = reader.ReadString();
            if (encryptedAuthor.Length > 0)
            {
                output.authorName = CryptoUtils.Decrypt(encryptedAuthor);
            }
            else
            {
                output.authorName = "Unknown";
            }
            output.pkgVersion = reader.ReadUInt32();
            output.mainAssetName = reader.ReadString();
            output.numAssets = reader.ReadInt32();
            output.contentBeginIndex = reader.ReadInt64();

            output.assets = new List<CrpAssetInfoHeader>();
            for (int i = 0; i < output.numAssets; i++)
            {
                CrpAssetInfoHeader info = new CrpAssetInfoHeader();
                info.assetName = reader.ReadString();
                info.assetChecksum = reader.ReadString();
                info.assetType = (Consts.AssetTypeMapping)(reader.ReadInt32());
                if(info.assetType == Consts.AssetTypeMapping.userLut)
                {
                    output.isLut = true;
                }
                info.assetOffsetBegin = reader.ReadInt64();
                info.assetSize = reader.ReadInt64();
                output.assets.Add(info);

            }

            return output;
        }

        /// <summary>
        /// Special Parser for LUTs, we're only grabbing the headerless PNG file (for now)
        /// </summary>
        /// <param name="header"></param>
        /// <param name="saveFiles"></param>
        /// <param name="isVerbose"></param>
        private void parseLut(CrpHeader header, bool saveFiles, bool isVerbose)
        {
            //Find the first instance of data(PNG file)
            CrpAssetInfoHeader info = header.assets.Find(asset => asset.assetName.Contains(Consts.DATA_EXTENSION));

            //Generate a name for the file
            string fileName = string.Format("{0}.png", StrUtils.limitStr(info.assetName), info.assetType.ToString());

            //Should be unnessecary in current version(stream pointer should already be at start of file),
            //but advance stream pointer to file position
            reader.BaseStream.Seek(info.assetOffsetBegin, SeekOrigin.Current);

            //Read file and deal with it as apporiate.
            MagickImage retVal = ImgParser.parseImgFile(reader, (uint)info.assetSize);
            if (isVerbose)
            {
                Console.WriteLine("Read image file {0}", fileName);
            }
            if (saveFiles)
            {
                retVal.Write(fileName);
            }


        }

        private void parseAssets(CrpHeader header, int index, bool saveFiles, bool isVerbose)
        {

            bool isNullFlag = reader.ReadBoolean();
            if (!isNullFlag)
            {
                string assemblyQualifiedName = reader.ReadString();
                string assetType = assemblyQualifiedName.Split(new char[] { ',' })[0];
                long assetContentLen = header.assets[index].assetSize - (2 + assemblyQualifiedName.Length);
                string assetName = reader.ReadString();
                assetContentLen -= (1 + assetName.Length);

                string fileName = string.Format("{0}_{1}_{2}", StrUtils.limitStr(assetName), index, header.assets[index].assetType.ToString());
                assetParser.parseObject((int)assetContentLen, assetType, saveFiles, fileName, isVerbose);
            } 
        }

    }
}
