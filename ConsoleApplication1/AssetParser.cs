using ConsoleApplication1.Parsers;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication1
{
    internal class AssetParser
    {

        internal delegate dynamic Parser(CrpReader reader, Boolean saveFile, String saveFileName, Int64 fileSize, Boolean verbose);

        private readonly IDictionary<String, Parser> _parsers;

        public AssetParser(IDictionary<String, Parser> parsers)
        {
            _parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        public static AssetParser MakeDefault()
        {
            var parsers = new Dictionary<String, Parser>
            {
                ["ColossalFramework.Importers.Image"] = ImgParser.ParseImage,
                ["UnityEngine.Mesh"] = MeshParser.ParseMesh,
                ["UnityEngine.Texture2D"] = ImgParser.ParseImage,
                ["BuildingInfoGen"] = InfoGenParser.ParseInfoGen,
                ["PropInfoGen"] = InfoGenParser.ParseInfoGen,
                ["TreeInfoGen"] = InfoGenParser.ParseInfoGen,
                ["VehicleInfoGen"] = InfoGenParser.ParseInfoGen,
                ["CustomAssetMetaData"] = InfoGenParser.ParseInfoGen,
                ["UnityEngine.Material"] = MaterialParser.ParseMaterial,
                ["UnityEngine.GameObject"] = GameObjectParser.ParseGameObj
            };
            return new AssetParser(parsers);
        }

        public void ParseObject(CrpReader reader, Int32 length, String format, Boolean saveFile = false, String saveFilePath = null, Boolean verbose = false)
        {
            if (_parsers.ContainsKey(format))
            {
                _parsers[format](reader, saveFile, saveFilePath, length, verbose);
            }
            else
            {
                var retVal = reader.ReadBytes(length);
                if (saveFile)
                {
                    var file = new BinaryWriter(new FileStream(saveFilePath + ".bin", FileMode.Create));
                    file.Write(retVal);
                    file.Close();
                }
                if (verbose)
                {
                    Console.WriteLine("{0} bytes read for {1}", length, saveFilePath + ".bin");
                }
            }
        }
    }
}
