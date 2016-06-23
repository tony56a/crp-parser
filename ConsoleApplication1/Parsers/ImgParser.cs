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
        public static MagickImage parseImage(CrpReader reader, bool saveFile, string saveFileName,long fileSize)
        {
            bool forceLinearFlag = reader.ReadBoolean();
            uint imgLength = reader.ReadUInt32();
            MagickImage retVal = new MagickImage(reader.ReadBytes((int)imgLength));
            if (saveFile)
            {
                if ( retVal.Format == MagickFormat.Png)
                {
                    retVal.Write(saveFileName + ".png");
                }
                else if ( retVal.Format == MagickFormat.Dds)
                {
                    retVal.Write(saveFileName + ".dds");
                }
            }
            return retVal;
        }

    }



}
