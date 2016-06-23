using ConsoleApplication1.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class AssetParser
    {
        private CrpReader reader;

        delegate dynamic Parser(CrpReader reader);

        Dictionary<string, Parser> parsers = new Dictionary<string, Parser>();

        public AssetParser(CrpReader reader) {
            this.reader = reader;

            this.parsers["ColossalFramework.Importers.Image"] = ImgParser.parseImage;
            this.parsers["UnityEngine.Texture2D"] = ImgParser.parseImage;

        }
    }
}
