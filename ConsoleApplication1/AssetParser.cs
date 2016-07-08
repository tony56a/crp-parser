using ConsoleApplication1.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class AssetParser
    {
        private CrpReader reader;

        delegate dynamic Parser(CrpReader reader, bool saveFile, string saveFileName,long fileSize,bool verbose);

        Dictionary<string, Parser> parsers = new Dictionary<string, Parser>();

        public AssetParser(CrpReader reader) {
            this.reader = reader;

            this.parsers["ColossalFramework.Importers.Image"] = ImgParser.parseImage;
            this.parsers["UnityEngine.Mesh"] = MeshParser.parseMesh;
            this.parsers["UnityEngine.Texture2D"] = ImgParser.parseImage;
            this.parsers["BuildingInfoGen"] = InfoGenParser.parseInfoGen;
            this.parsers["PropInfoGen"] = InfoGenParser.parseInfoGen;
            this.parsers["TreeInfoGen"] = InfoGenParser.parseInfoGen;
            this.parsers["VehicleInfoGen"] = InfoGenParser.parseInfoGen;
            this.parsers["CustomAssetMetaData"] = InfoGenParser.parseInfoGen;
            this.parsers["UnityEngine.Material"] = MaterialParser.parseMaterial;

            this.parsers["UnityEngine.GameObject"] = GameObjectParser.parseGameObj;

        }

        public dynamic parseObject(int length, string format, bool saveFile = false, string saveFilePath = null,bool verbose=false)
        {
            dynamic retVal;
            if (parsers.ContainsKey(format))
            {
                retVal = this.parsers[format](reader, saveFile, saveFilePath,length,verbose);

            }
            else
            {
                retVal = reader.ReadBytes(length);
                if (saveFile)
                {
                    System.IO.BinaryWriter file = new BinaryWriter(new FileStream(saveFilePath + ".bin", FileMode.Create));
                    file.Write(retVal);
                    file.Close();
                }
                if (verbose)
                {
                    Console.WriteLine("{0} bytes read for {1}",length, saveFilePath + ".bin");
                }
            }
            return retVal;
        }
    }
}
