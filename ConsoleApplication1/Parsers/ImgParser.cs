using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Parsers
{
    public static class ImgParser
    {
        public static MagickImageInfo parseImage(CrpReader reader)
        {
            bool forceLinearFlag = reader.ReadBoolean();
            uint fileSize = reader.ReadUInt32();
            return new MagickImageInfo( reader.ReadBytes((int)fileSize) );
        }
    }
}
