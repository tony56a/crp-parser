using System;
using System.IO;

namespace ConsoleApplication1.Parsers
{
    public static class ImgParser
    {
        public static String ParseImage(CrpReader reader, Boolean saveFile, String saveFileName, Int64 fileSize, Boolean verbose)
        {
            var ihavenoidea  = reader.ReadUInt32();
            var forceLinearFlag = reader.ReadBoolean();
            var imgLength = reader.ReadUInt32();

            var fileName = saveFileName + ".png";

            if (verbose)
            {
                Console.WriteLine("Read image file {0}", fileName);
            }
            if (saveFile)
            {
                using (var file = File.Create(fileName))
                {
                    for (var i = 0U; i < imgLength; i++)
                    {
                        file.WriteByte(reader.ReadByte());
                    }
                }
            }
            else
            {
                reader.BaseStream.Position += fileSize;
            }
            return fileName;
        }
    }
}
