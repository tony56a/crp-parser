using ConsoleApplication1.Parsers;
using CrpParser;
using CrpParser.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication1
{
    internal class CrpDeserializer
    {
        private readonly AssetParser _assetParser;

        /// <summary>
        /// Initializes the object by opening the specified CRP file. Note and be prepared to handle the exceptions
        /// that may be thrown.
        /// </summary>
        /// <param name="filePath">Path to the CRP file that needs to be opened.</param>
        /// <exception cref="System.IO.IOException">Thrown if i.e the filePath parameter is incorrect, file is missing,
        /// in use, etc.</exception>
        public CrpDeserializer(AssetParser assetParser)
        {
            _assetParser = assetParser;
        }

        public void ParseFile(FileInfo fileInfo, Boolean saveFiles, Boolean verbose)
        {
            if (!fileInfo.Exists) throw new InvalidOperationException($"can't find file: \"{fileInfo.FullName}\"");
            using (var stream = File.Open(fileInfo.FullName, FileMode.Open))
            {
                var reader = new CrpReader(stream);

                var magicStr = new String(reader.ReadChars(4));
                if (magicStr.Equals(Consts.MAGICSTR))
                {
                    var header = ParseHeader(reader);

                    if (saveFiles)
                    {
                        var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + header.mainAssetName + "_contents";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        Environment.CurrentDirectory = (path);
                    }

                    if (verbose)
                    {
                        Console.WriteLine(header);
                    }
                    if (saveFiles)
                    {
                        using var streamwriter = new StreamWriter(new FileStream(header.mainAssetName + "_header.json", FileMode.Create));
                        var json = JsonConvert.SerializeObject(header, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter());
                        streamwriter.Write(json);
                        streamwriter.Close();
                    }
                    if (header.isLut)
                    {
                        ParseLut(reader, header, saveFiles, verbose);
                    }
                    else
                    {
                        for (var i = 0; i < header.numAssets; i++)
                        {
                            ParseAssets(reader, header, i, saveFiles, verbose);
                        }
                    }
                }
                else
                {
                    throw new InvalidDataException("Invalid file format!");
                }
            };
        }

        private static CrpHeader ParseHeader(CrpReader reader)
        {
            var output = new CrpHeader
            {
                formatVersion = reader.ReadUInt16(),
                packageName = reader.ReadString()
            };
            var encryptedAuthor = reader.ReadString();
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
            for (var i = 0; i < output.numAssets; i++)
            {
                var info = new CrpAssetInfoHeader
                {
                    assetName = reader.ReadString(),
                    assetChecksum = reader.ReadString(),
                    assetType = (Consts.AssetTypeMapping)(reader.ReadInt32())
                };
                if (info.assetType == Consts.AssetTypeMapping.userLut)
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
        /// <param name="verbose"></param>
        private static void ParseLut(CrpReader reader, CrpHeader header, Boolean saveFiles, Boolean verbose)
        {
            //Find the first instance of data(PNG file)
            var info = header.assets.Find(asset => asset.assetName.Contains(Consts.DATA_EXTENSION));

            //Generate a name for the file
            var fileName = $"{StrUtils.limitStr(info.assetName)}.png";

            //Should be unnessecary in current version(stream pointer should already be at start of file),
            //but advance stream pointer to file position
            reader.BaseStream.Seek(info.assetOffsetBegin, SeekOrigin.Current);

            //Read file and deal with it as apporiate.
            var retVal = ImgParser.ParseImage(reader, saveFiles, fileName, info.assetSize, verbose);
            if (verbose)
            {
                Console.WriteLine("Read image file {0}", fileName);
            }
        }

        private void ParseAssets(CrpReader reader, CrpHeader header, Int32 index, Boolean saveFiles, Boolean isVerbose)
        {
            var isNullFlag = reader.ReadBoolean();
            if (!isNullFlag)
            {
                var assemblyQualifiedName = reader.ReadString();
                var assetType = assemblyQualifiedName.Split(',')[0];
                var assetContentLen = header.assets[index].assetSize - (2 + assemblyQualifiedName.Length);
                var assetName = reader.ReadString();
                assetContentLen -= (1 + assetName.Length);

                var fileName = $"{StrUtils.limitStr(assetName)}_{index}_{header.assets[index].assetType}";
                _assetParser.ParseObject(reader, (Int32)assetContentLen, assetType, saveFiles, fileName, isVerbose);
            }
        }
    }
}
