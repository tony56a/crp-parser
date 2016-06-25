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
        public static MagickImage parseImage(CrpReader reader, bool saveFile, string saveFileName, long fileSize, bool verbose)
        {
            bool forceLinearFlag = reader.ReadBoolean();
            uint imgLength = reader.ReadUInt32();
            MagickImage retVal = new MagickImage(reader.ReadBytes((int)imgLength));

            string fileName = saveFileName + ".png";
            retVal.Format = MagickFormat.Png;

            if (verbose)
            {
                Console.WriteLine("Read image file {0}", fileName);
            }
            if (saveFile)
            {
                retVal.Write(fileName);
            }
            
            
            return retVal;
        }

    }



}
